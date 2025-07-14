// GMLogEventManager.cs
// Created by nancheng.
// DateTime: 2024年2月27日 17:36:57
// Desc: GM日志打点

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using SDK.BaseCore;
using UnityEngine;
using Utils.JsonUtils;
using UW;
using XLua;
using TEngine;

[LuaCallCSharp]
public class GmLogEventManager : GameBaseModule
{
    [Serializable]
    public class GmLog
    {
        public string project_id = "7";
        public string index = "uw_log_event";
        public GmLogData data;

        public override string ToString()
        {
            if (data != null)
            {
                return data.ToString();
            }

            return "";
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
    
    [Serializable]
    public class GmLogData
    {
        public string action;
        public string param;
        public long timestamp;
        public string uid;
        public string serverId;
        public string deviceId;
        public string version;
        
        public override string ToString()
        {
            return action + timestamp + uid;
        }
    }
    
    [Serializable]
    public class GmLogParam
    {
        public string EventName;
        public GmLog GMLog;

        public GmLogParam(string eventName, GmLog gmLog)
        {
            this.EventName = eventName;
            this.GMLog = gmLog;
        }
        
        public string GetCacheStr()
        {
            return this.EventName + "#_#" + this.GMLog.ToString();
        }
    }

    private class CacheTask : ThreadTask
    {
        public delegate void CacheAction();

        private CacheAction _action;

        public CacheTask(CacheAction action)
        {
            _action = action;
        }
        
        public override void Process()
        {
            if (_action != null)
            {
                _action();
            }    
        }
    }
    
    // 新打点 放到 参数队列 ,缓存的Dic里
    // 参数队列中取出，发送打点协议，移到toRemove字典中,缓存Dic中清除
    // 成功后，toRemove移除，失败放到队列头，从新打点
    // 退出游戏时，将队列中，缓存Dic，toRemove中的打点缓存到本地
    // 初始化时，从本地中加载打点数据，塞到队列中

    #region property
    public override bool Updatable => true;
    private Queue<GmLogParam> _gmLogParamQueue;
    private Dictionary<string, GmLogParam> _toRemoveParamDic;
    private List<GmLogParam> _cacheParamList;
    private bool _isRecordChanged;
    private bool _canExecuteSaveTask;
    private bool _canExecuteLoadTask;
    private bool _isLoadFinish;
    private float _nextSaveTime;

    private MultiThread _cacheTread;

    private object _lockObj;
    
    private static readonly string CACHE_DIR_FULL_PATH = Path.Combine(Application.persistentDataPath, GmLogConstant.LOCAL_CACHE_DIR);
    private static readonly string CACHE_FILE_FULL_PATH = Path.Combine(CACHE_DIR_FULL_PATH, GmLogConstant.LOCAL_CACHE_LOG_FILE);
    #endregion

    #region public

    public override void Initialize()
    {
        _gmLogParamQueue = new Queue<GmLogParam>();
        _toRemoveParamDic = new Dictionary<string, GmLogParam>();
        _cacheParamList = new List<GmLogParam>();
        _isRecordChanged = false;
        _nextSaveTime = GmLogConstant.SAVE_CACHE_TIMING_SECOND;

        _lockObj = new object();
        
        CheckCacheDir();
        InitCacheThread();
        TryLoadLocalCache();
    }

    public void Record(string eventName, string param = "")
    {
        var gmLog = GetGmLog(eventName, param);
        var gmLogParam = new GmLogParam(eventName, gmLog);
        AddRecord(gmLogParam);
    }

    public void Record_Params(string eventName, LuaTable table)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        foreach (var key in table.GetKeys())
        {
            var value = table.Get<string>(key.ToString());
            param[key.ToString()] = value;
        }
        
        var gmLog = GetGmLog(eventName, param);
        var gmLogParam = new GmLogParam(eventName, gmLog);
        AddRecord(gmLogParam);
    }
    
    public override void Update(float elapsedTime, float realElapsedTime)
    {
        HandleReCord();
        
        TimingSaveCache();
    }

    public override void OnApplicationQuit()
    {
        SyncSaveLocalCache();
        Release();
    }
    
    #endregion

    #region private

    private void AddRecord(GmLogParam gmLogParam)
    {
        if (!_gmLogParamQueue.Contains(gmLogParam) && !gmLogParam.EventName.IsNullOrEmpty())
        {
            _gmLogParamQueue.Enqueue(gmLogParam);
        }
    }

    private GmLog GetGmLog(string eventName, string param = "")
    {
        var gmLogData = new GmLogData
        {
            action = eventName,
            param = param,
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            uid = PlayerPrefs.GetString("Setting.LOGIN_UID", ""),
            serverId = PlayerPrefs.GetString("Setting.SERVER_ID", ""),
            deviceId = PlatformUtils.deviceUuid,
            version = SDKLuaHelper.GetVersionName() + "(" + SDKLuaHelper.GetVersionCode() + ")"
        };
        
        var jsonData = new GmLog()
        {
            data = gmLogData,
        };

        return jsonData;
    }

    private GmLog GetGmLog(string eventName, Dictionary<string, string> paramData)
    {
        var jsonData = GetGmLog(eventName, "");

        var data = jsonData.data;
        if (data != null)
        {
            foreach (var param in paramData)
            {
                data.param += param.Key + " = " + param.Value + "_";
            }
        }
        
        return jsonData;
    }

    private void HandleReCord()
    {
        for (int i = GmLogConstant.MAX_RECORD_COUNT_FRAME; i >= 0; i--)
        {
            if (_gmLogParamQueue != null && _gmLogParamQueue.Count > 0)
            {
                DoRecord(_gmLogParamQueue.Dequeue());
            }
        }
    }
    
    // 请求的queue 发送 移除 
    private void DoRecord(GmLogParam param)
    {
        var flag = param.GetCacheStr();
        AddCacheParam(param);
        HttpRequestUtils.PostByJson(GmLogConstant.POST_URL_JSON, param.GMLog.ToJson(), OnRequestCallback, flag);
        if (!_toRemoveParamDic.ContainsKey(flag))
        {
            _toRemoveParamDic.Add(flag, param);
        }
    }

    private void OnRequestCallback(bool isSuccess, object userdata, object flag)
    {
        GmLogParam param;
        if (_toRemoveParamDic.TryGetValue(flag.ToString(), out param))
        {
            _toRemoveParamDic.Remove(flag.ToString());
            RemoveCacheParam(param);
        }
        
        if (isSuccess)
        {
            JObject data;
            try
            {
                data = JsonHelper.FromJson(userdata.ToString());
                if (data != null && data["statusCode"].ToInt() == 1)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                
            }
        }
        // 从新打点
        _gmLogParamQueue.Enqueue(param);
    }
    
    private void AddCacheParam(GmLogParam gmLogParam)
    {
        if (!_cacheParamList.Contains(gmLogParam) && !gmLogParam.EventName.IsNullOrEmpty())
        {
            _cacheParamList.Add(gmLogParam);
            _isRecordChanged = true;
        }
    }

    private void RemoveCacheParam(GmLogParam gmLogParam)
    {
        if (_cacheParamList.Contains(gmLogParam))
        {
            _cacheParamList.Remove(gmLogParam);
        }
    }
    
    private void TimingSaveCache()
    {
        if (_isLoadFinish)
        {
            _nextSaveTime -= Time.deltaTime;
        }
        
        if (_isLoadFinish && _isRecordChanged && _nextSaveTime < 0 && !_canExecuteLoadTask)
        {
            _nextSaveTime = GmLogConstant.SAVE_CACHE_TIMING_SECOND;
            _canExecuteSaveTask = true;

            var task = new CacheTask(SaveLocalCache);
            _cacheTread.AddTask(task);
        }
    }
    
    private void InitCacheThread()
    {
        if (_cacheTread == null)
        {
            _cacheTread = new MultiThread("gm_log_thread");
            _cacheTread.Start();
        }
    }
    
    private void TryLoadLocalCache()
    {
        _canExecuteLoadTask = true;
        var task = new CacheTask(LoadLocalCache);
        _cacheTread.AddTask(task);
    }

    private void SaveLocalCache()
    {
        lock (_lockObj)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(CACHE_FILE_FULL_PATH))
                {
                    for (int i = 0; i < _cacheParamList.Count; i++)
                    {
                        sw.WriteLine(JsonUtility.ToJson(_cacheParamList[i]));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (_isRecordChanged)
            {
                _isRecordChanged = false;
            }

            _canExecuteSaveTask = false;
        }
    }

    private void SyncSaveLocalCache()
    {
        lock (_lockObj)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(CACHE_FILE_FULL_PATH))
                {
                    for (int i = 0; i < _cacheParamList.Count; i++)
                    {
                        sw.WriteLine(JsonUtility.ToJson(_cacheParamList[i]));
                    }

                    while (_gmLogParamQueue.Peek() != null)
                    {
                        sw.WriteLine(JsonUtility.ToJson(_gmLogParamQueue.Dequeue()));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
    
    private void LoadLocalCache()
    {
        lock (_lockObj)
        {
            try
            {
                using (StreamReader sw = new StreamReader(CACHE_FILE_FULL_PATH))
                {
                    string cacheStr;
                    while ((cacheStr = sw.ReadLine()) != null)
                    {
                        if (!cacheStr.IsNullOrEmpty())
                        {
                            var gmLogParam = JsonUtility.FromJson<GmLogParam>(cacheStr);
                            AddRecord(gmLogParam);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            _canExecuteLoadTask = false;
            _isLoadFinish = true;

            if (File.Exists(CACHE_FILE_FULL_PATH))
            {
                File.WriteAllText(CACHE_FILE_FULL_PATH, string.Empty);
            }
        }
    }

    private void CheckCacheDir()
    {
        if (!Directory.Exists(CACHE_DIR_FULL_PATH))
        {
            Directory.CreateDirectory(CACHE_DIR_FULL_PATH);
        }

        if (!File.Exists(CACHE_FILE_FULL_PATH))
        {
            File.Create(CACHE_FILE_FULL_PATH).Dispose();
        }
    }

    private void Release()
    {
        if (_cacheTread != null)
        {
            _cacheTread.Stop();
            _cacheTread = null;
        }
        
        _toRemoveParamDic.Clear();
        _gmLogParamQueue.Clear();
        _cacheParamList.Clear();
    }
    #endregion
}