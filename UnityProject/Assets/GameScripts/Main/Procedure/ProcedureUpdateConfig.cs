// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-01-08       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;

namespace GameMain
{
    public class ProcedureUpdateConfig : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        private ProcedureOwner _procedureOwner;

        private string _urlLuaConfigZip;
        private ConfigVersion _remoteConfigVersion;

        private int _downloadFileThreshold = 100;
        private bool _isNeedDownloadAll = false;
        private List<ConfigFileInfo> _downloadList = new List<ConfigFileInfo>(10);
        private int _downloadFileCount = 0;
        private int _tmpDownloadFileCount = 0;
        Dictionary<string, DownLoadNode> _downLoadDic = new Dictionary<string, DownLoadNode>(10);

        bool m_UpdateConfigComplete = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _procedureOwner = procedureOwner;
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"检查配置表版本...");

            InitPackage(procedureOwner).Forget();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_UpdateConfigComplete)
            {
                // 初始化资源未完成则继续等待
                return;
            }

            GameEvent.Send(ProcedureEventDefines.Events.PROCEDURE_RES_UPDATE_SUCCESS);
            ChangeState<ProcedureLoadAssembly>(procedureOwner);
        }

        private async UniTaskVoid InitPackage(ProcedureOwner procedureOwner)
        {
            await UniTask.Yield();
            // 0.判断开启更新配置
            // 1.获取沙盒内版本
            // 2.获取包内版本
            // 3.比较版本
            // 4.如果包内版本比沙盒高，删了沙盒中的配置
            // 5.获取远程版本
            // 6.比较远程版本和当前版本
            //  不用更新：进入下一个流程
            // 7.如果远程版本比当前版本高，下载新的配置，写入temp文件
            // 8.解压
            // 9.删除temp文件
            // 10.刷新沙盒版本
            // 11.进入下一个流程
            GameModule.Resource.InitConfig();
            bool enableUpdateConfig = SettingsUtils.EnableUpdateConfig();

            if (!enableUpdateConfig)
            {
                m_UpdateConfigComplete = true;
                return;
            }

            await UniTask.Yield();
            ConfigVersion localVersion = GameModule.Resource.ConfigVersion;

            try
            {
                // 5
                if (SettingsUtils.IsOverrideUpdateInfo() && !string.IsNullOrEmpty(GameModule.Resource.RemoteConfigVersion))
                {
                    _remoteConfigVersion = new ConfigVersion(GameModule.Resource.RemoteConfigVersion);
                }
                else
                {
                    RemoteConfigData data = await RequestRemoteConfigData();
                    if (data == null)
                    {
                        OnRequestRemoteConfigVersionFailed(procedureOwner, "获取配置表版本失败...", "RequestRemoteConfigData http request failed");
                        return;
                    }
                    _remoteConfigVersion = new ConfigVersion(data.version);
                }
                Log.Debug($"Updated config Version : from {localVersion.VersionStr} to {_remoteConfigVersion.VersionStr}");

                // 6
                // if (ConfigVersion.Compare(_remoteConfigVersion, localVersion))
                if(_remoteConfigVersion.IsValid && !_remoteConfigVersion.VersionStr.Equals(localVersion.VersionStr))
                {
                    // 拉取最新配置文件列表
                    await UniTask.Yield();
                    await TryDownLoadLuaCon(_remoteConfigVersion);
                }
                else
                {
                    m_UpdateConfigComplete = true;
                }
            }
            catch (Exception e)
            {
                OnRequestRemoteConfigVersionFailed(procedureOwner, "获取配置表版本失败...", e.Message);
            }
        }

        private void OnConfigUpdateSuccess()
        {
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"配置表更新成功");
            GameModule.Resource.ConfigMode = ConfigMode.Sandbox;
            GameModule.Resource.ConfigVersion = _remoteConfigVersion;
            GameModule.Resource.SaveConfigVersion(_remoteConfigVersion.VersionStr);
            Log.Info("ProcedureUpdateConfig, update config success, version:{0}, ConfigMode:{1}",
                _remoteConfigVersion.VersionStr, GameModule.Resource.ConfigMode);
            m_UpdateConfigComplete = true;
        }

        private void OnRequestRemoteConfigVersionFailed(ProcedureOwner procedureOwner, string title, string message)
        {
            Log.Error($"{message}");

            // 打开启动UI。
            UILoadMgr.Show(UIDefine.UILoadUpdate, title);

            UILoadTip.ShowMessageBox($"配置表更新失败", MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Retry
                , () => { Retry(procedureOwner); },
                Application.Quit);
        }

        private void Retry(ProcedureOwner procedureOwner)
        {
            // 打开启动UI。
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"重新获取配置表版本中...");

            InitPackage(procedureOwner).Forget();
        }

        /// <summary>
        /// 请求远程配置表version。
        /// </summary>
        private async UniTask<RemoteConfigData> RequestRemoteConfigData()
        {
            // 打开启动UI。
            UILoadMgr.Show(UIDefine.UILoadUpdate);

            var urlCheckVersion = SettingsUtils.GetConfigCheckVersionURL();

            if (string.IsNullOrEmpty(urlCheckVersion))
            {
                Log.Error("ProcedureUpdateConfig, remote url is empty or null");
                return null;
            }

            try
            {
                var updateDataStr = await Utility.Http.Get(urlCheckVersion);

                RemoteConfigData data;
                if (SettingsUtils.IsConfigUpdateFromGm())
                {
                    data = Utility.Json.ToObject<RemoteConfigData>(updateDataStr);
                }
                else
                {
                    data = new RemoteConfigData { version = updateDataStr };
                }
                return data;
            }
            catch (Exception e)
            {
                // 打开启动UI。
                UILoadTip.ShowMessageBox("请求远程配置版本失败！点击确认重试", MessageShowType.TwoButton,
                    LoadStyle.StyleEnum.Style_Retry
                    , () => { InitPackage(_procedureOwner).Forget(); }, Application.Quit);
                Log.Error(e);
                Log.Info(urlCheckVersion);
                return null;
            }
        }

        #region 获取配置列表

        private async UniTask TryDownLoadLuaCon(ConfigVersion updateConfigVersion)
        {
            // 检查目录
            Utility.File.CheckFolder(SettingsUtils.ConfigSandBoxFolder, true);
            Utility.File.CheckFolder(SettingsUtils.ConfigDownloadFolder, true);
            Utility.File.CheckFolder(SettingsUtils.ConfigSandBoxFolder_Tables, true);
            Utility.File.CheckFolder(SettingsUtils.ConfigSandBoxFolder_Tables_Enum, true);
            Utility.File.CheckFolder(SettingsUtils.ConfigSandBoxFolder_Tables_Module, true);

            var urlConfigList = SettingsUtils.GetConfigFileListURL(updateConfigVersion.VersionStr);
            // 获取服务器文件列表
            HttpRequestUtils.HttpDownLoad(urlConfigList, null, (success, userdata, flag) =>
            {
                if (success)
                {
                    if (userdata == null)
                    {
                        Debug.LogError("ProcedureUpdateConfig 配置表列表为空");
                        return;
                    }

                    try
                    {
                        byte[] bytes = userdata as byte[];
                        string configFileList = $"{SettingsUtils.ConfigDownloadFolder}/{SettingsUtils.ConfigFileListName}";
                        File.WriteAllBytes(configFileList, bytes);
                        string configFilesJson = System.Text.Encoding.UTF8.GetString(bytes);

                        //解析配置文件列表
                        ConfigFileList remoteConfigList = Utility.Json.ToObject<ConfigFileList>(configFilesJson);
                        if (remoteConfigList == null)
                        {
                            Debug.LogError("ProcedureUpdateConfig 配置表列表解析失败");
                            return;
                        }
                        ConfigFileList localConfigList = GetLocalConfigList();
                        Dictionary<string, ConfigFileInfo> localConfigDict =
                            new Dictionary<string, ConfigFileInfo>(localConfigList.files.Count);
                        foreach (var info in localConfigList.files)
                        {
                            localConfigDict[info.name] = info;
                        }

                        _downloadList.Clear();
                        foreach (var remoteFileInfo in remoteConfigList.files)
                        {
                            bool needUpdate = true;
                            if (localConfigDict.TryGetValue(remoteFileInfo.name, out var localFileInfo))
                            {
                                if (localFileInfo.md5.Equals(remoteFileInfo.md5))
                                {
                                    needUpdate = false;
                                }
                            }

                            if (needUpdate)
                            {
                                _downloadList.Add(remoteFileInfo);
                            }
                        }

                        DownLoadConfigFiles(_downloadList);
                    }
                    catch (Exception e)
                    {
                        OnRequestRemoteConfigVersionFailed(_procedureOwner, "下载失败...", e.Message);
                    }
                }
                else
                {
                    OnRequestRemoteConfigVersionFailed(_procedureOwner, "下载失败...", "download error");
                }
            }, null);
        }

        /// <summary>
        /// 获取本地配置文件列表
        /// </summary>
        /// <returns></returns>
        ConfigFileList GetLocalConfigList()
        {
            string fileListStr;
            // 获取本地配置文件列表
            if (GameModule.Resource.ConfigMode == ConfigMode.Package)
                fileListStr = GameModule.Resource.GetPackageConfigFileList();
            else
                fileListStr = GameModule.Resource.GetSandboxConfigFileList();
            ConfigFileList localConfigList = Utility.Json.ToObject<ConfigFileList>(fileListStr);
            return localConfigList;
        }

        #endregion

        #region 散装下载

        void DownLoadConfigFiles(List<ConfigFileInfo> downloadList)
        {
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"更新配置表，已更新");
            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(0);

            if (downloadList.Count == 0)
            {
                OnConfigUpdateSuccess();
            }
            else
            {
                _downloadFileCount = downloadList.Count;
                _tmpDownloadFileCount = 0;
                _downLoadDic.Clear();
                for (int i = 0; i < downloadList.Count; i++)
                {
                    var name = downloadList[i].name;
                    var url = SettingsUtils.GetConfigDownloadURL(downloadList[i].fileName, downloadList[i].path, downloadList[i].name);
                    var save = Path.Combine(SettingsUtils.ConfigSandBoxFolder,
                        $"{downloadList[i].path}/{downloadList[i].name}");
                    Log.Debug("download start Config :" + url);

                    DownLoadNode node = new DownLoadNode(name, url, save, OnRequestDownloadCallback);
                    _downLoadDic.Add(url, node);
                    HttpRequestUtils.HttpDownLoad(url, null, node.OnRequestCallback, node.OnDownLoadProgress);
                }
            }
        }

        void OnRequestDownloadCallback(bool isSuccess, string url, string name, string savePath)
        {
            if (!isSuccess)
            {
                OnRequestRemoteConfigVersionFailed(_procedureOwner, "下载失败...", $"{name}, download error");
                return;
            }

            DownLoadNode node;
            if (_downLoadDic.TryGetValue(url, out node))
            {
                _tmpDownloadFileCount++;

                float progress = 100f * _tmpDownloadFileCount / _downloadFileCount;
                UILoadMgr.Show(UIDefine.UILoadUpdate,
                    $"更新配置表，已更新 {_tmpDownloadFileCount}/{_downloadFileCount} ({progress:F2}%)");
                LoadUpdateLogic.Instance.DownProgressAction?.Invoke(progress);
                Log.Debug($"更新配置表，已更新 {_tmpDownloadFileCount}/{_downloadFileCount} ({progress:F2}%)");

                _downloadList.RemoveAll(item => item.name == node.name);
                if (_downloadList.Count <= 0)
                {
                    MoveConfigFilesToSandbox();
                    OnConfigUpdateSuccess();
                }
            }
            else
            {
                Debug.LogError("??? 没有下载对象");
            }
        }

        private void MoveConfigFilesToSandbox()
        {
            string tmpConfigFileList = $"{SettingsUtils.ConfigDownloadFolder}/{SettingsUtils.ConfigFileListName}";
            string sandboxConfigFileList = $"{SettingsUtils.ConfigSandBoxFolder_Tables}/{SettingsUtils.ConfigFileListName}";
            if (File.Exists(sandboxConfigFileList))
                File.Delete(sandboxConfigFileList);
            File.Move(tmpConfigFileList, sandboxConfigFileList);
        }

        #endregion

        #region Zip全部下载

        void DownLoadConfigZip(string updateConfigVersion)
        {
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"获取配置列表中...");
            _urlLuaConfigZip = string.Format(_urlLuaConfigZip, updateConfigVersion);
            HttpRequestUtils.HttpDownLoad(_urlLuaConfigZip, null, OnRequestCallback, OnDownLoadProgress);
        }

        void OnRequestCallback(bool isSuccess, object userdata, object flag)
        {
            if (isSuccess)
            {
                string folderPath = SettingsUtils.ConfigDownloadFolder;
                Log.Info($"ProcedureUpdateConfig 开始删除临时文件夹 {folderPath}");
                Utility.File.DeleteFolder(folderPath);
                Utility.File.CheckFolder(folderPath, true);
                System.IO.File.WriteAllBytes(SettingsUtils.ConfigZipName, userdata as byte[]);

                Debug.Log(
                    $"ProcedureUpdateConfig 开始解压{SettingsUtils.ConfigZipName},到{SettingsUtils.ConfigSandBoxFolder}");
                Utility.File.UnZip(SettingsUtils.ConfigZipName, SettingsUtils.ConfigSandBoxFolder);
                OnConfigUpdateSuccess();
            }
            else
            {
                Debug.LogError($"ProcedureUpdateConfig 配置表下载失败 url：{_urlLuaConfigZip}");
                // 打开启动UI。
                UILoadTip.ShowMessageBox("配置表下载失败！点击确认重试", MessageShowType.TwoButton,
                    LoadStyle.StyleEnum.Style_Retry
                    , () => { InitPackage(_procedureOwner).Forget(); }, Application.Quit);
            }
        }

        void OnDownLoadProgress(string url, float curLength, float maxLength)
        {
            float progress = curLength / maxLength * 100;
            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(progress);
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"正在更新配置表，已更新 {curLength}/{maxLength} ({progress:F2}%)");
        }

        #endregion
    }

    public class DownLoadNode
    {
        public string name;
        public string url;
        public string savePath;
        public float process;
        Action<bool, string, string, string> _resCallback;
        BestHttpManager.OnDownLoadProgress _progressCallback;

        public DownLoadNode(string name, string url, string savePath, Action<bool, string, string, string> callback = null,
            BestHttpManager.OnDownLoadProgress onProgress = null)
        {
            this.name = name;
            this.savePath = savePath;
            this.url = url;
            this._resCallback = callback;
            this._progressCallback = onProgress;
        }

        public void OnRequestCallback(bool isSuccess, object userdata, object flag)
        {
            if (isSuccess)
            {
                Log.Debug("下载成功 :" + url);
                if (File.Exists(savePath))
                {
                    Log.Debug("下载成功 但是本地已经有此文件了 先删除在保存:" + url);
                    File.Delete(savePath);
                }

                /// todo try catch
                System.IO.File.WriteAllBytes(savePath, userdata as byte[]);
            }
            else
            {
                Log.Debug("下载失败 :" + url);
            }

            if (this._resCallback != null)
            {
                this._resCallback(isSuccess, url, name, savePath);
            }
        }

        public void OnDownLoadProgress(string url, float curLength, float maxLength)
        {
            SetProcessValue(curLength / maxLength);
            if (this._progressCallback != null)
            {
                this._progressCallback(url, curLength, maxLength);
            }
        }

        public float GetProcessValue()
        {
            return process;
        }

        public void SetProcessValue(float value)
        {
            process = value;
        }
    }
}