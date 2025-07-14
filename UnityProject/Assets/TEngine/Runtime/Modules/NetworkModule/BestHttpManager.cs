// file:    HttpRequestManager.cs
// author:  nancheng
// date:    2023年10月23日-10:39:55
// desc:    BestHttp 网络请求管理类

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BestHTTP;
using TEngine;
using UnityEngine;

public enum RequestDataType
{
    Text = 1,
    Bytes,
    Texture
}

#region RequestFormData
public class RequestFormData
{
    private Dictionary<string, string> _formData = new Dictionary<string, string>();
    
    public void AddField(string filedName, string value)
    {
        if (!_formData.ContainsKey(filedName) && !filedName.IsNullOrEmpty() && !value.IsNullOrEmpty())
        {
            _formData.Add(filedName, value);
        }
    }

    public Dictionary<string, string> GetFormData()
    {
        return _formData;
    }
}
#endregion

#region BestHttpParam

public class BestHttpParam
{
    public string Uri;
    
    public RequestDataType RequestDataType;
    public HTTPMethods MethodType;
    public string ContentType;
    
    public RequestFormData FormData;
    public byte[] RawData;
    public Stream UploadStream;
    
    public int MaxRetryCount;
    
    public string DownLoadSavePath;
    public string UpLoadFilePath;
    
    public BestHttpManager.RequestCallback RequestCallback;
    public BestHttpManager.OnUpLoadProgress OnUpLoadProgress;
    public BestHttpManager.OnDownLoadProgress OnDownLoadProgress;

    public object Flag;     // 用于请求标识，判定是从谁发的请求
    public string ErrorLog;

    private const int MAX_RETRY_COUNT = 2;    // 重新请求最大次数，超过次数后为真正的失败
    private int _retryCount;
    
    public BestHttpParam()
    {
        MethodType = HTTPMethods.Get;
        _retryCount = 0;
        MaxRetryCount = MAX_RETRY_COUNT;
        RequestDataType = RequestDataType.Text;
    }
    
    public bool IsEffective()
    {
        if (!string.IsNullOrEmpty(Uri) && RequestCallback != null)
        {
            return true;
        }
        
        Debug.LogWarning("网络请求无效，请检查, url = " + Uri);
        return false;
    }

    public void AddRetryCount()
    {
        _retryCount++;
    }
    
    public bool CanRetry()
    {
        return _retryCount < MaxRetryCount;
    }
}

#endregion

public class BestHttpManager : SingletonBehaviourAot<BestHttpManager>
{
    // 请求流程
    // 新加的请求，失败的请求，添加到_bestHttpParamQueue中
    // 从_bestHttpParamQueue取出MAX_WORK_REQUEST_COUNT个请求，进行真正的请求，并且将parma 和 request 存储到 _toRemoveParamDic
    // 请求回调后，请求成功 toRemove中移除，请求失败，toRemove中移除，取出param，RetryCount++，需要重新请求，加到_bestHttpParamQueue中，不需要了，直接remove

    #region property

    private static readonly float CONNECT_SERVER_TIME_OUT = 10; // 服务器连接超时时间
    private static readonly int MAX_WORK_REQUEST_COUNT = 5;    // 同帧同时请求数量 
    
    private Queue<BestHttpParam> _bestHttpParamQueue = new Queue<BestHttpParam>();
    private Dictionary<HTTPRequest, BestHttpParam> _toRemoveParamDic = new Dictionary<HTTPRequest, BestHttpParam>();

    public delegate void RequestCallback(bool isSuccess, object userdata, object flag = null);

    public delegate void OnDownLoadProgress(string url, float curLength, float maxLength);

    public delegate void OnUpLoadProgress(string url, float curLength, float maxLength);
    
    #endregion

    #region public
    public void Get(string url, RequestDataType dataType, RequestCallback callback)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            RequestDataType = dataType,
            RequestCallback = callback,
            ErrorLog = "Get",
        };

        Request(param);
    }
    
    public void PostByFormData(string url, RequestDataType dataType, RequestFormData formData, RequestCallback callback)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            MethodType = HTTPMethods.Post,
            RequestDataType = dataType,
            FormData = formData,
            RequestCallback = callback,
            ErrorLog = "PostByFormData",
        };

        Request(param);
    }

    public void PostByRawData(string url, RequestDataType dataType, byte[] rawData, RequestCallback callback)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            MethodType = HTTPMethods.Post,
            RequestDataType = dataType,
            RawData = rawData,
            RequestCallback = callback,
            ErrorLog = "PostByRawData",
        };

        Request(param);
    }

    /// <summary>
    /// Json格式post请求，json数据需要转成string
    /// </summary>
    /// <param name="url"></param>
    /// <param name="jsonData"></param>
    /// <param name="callback"></param>
    public void PostByJson(string url, string jsonData, RequestCallback callback, object flag)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            MethodType = HTTPMethods.Post,
            ContentType = "application/json",
            RequestDataType = RequestDataType.Text,
            RawData = Encoding.UTF8.GetBytes(jsonData),
            RequestCallback = callback,
            ErrorLog = "PostByJson",
            Flag = flag,
        };

        Request(param);
    }
    
    public void HttpDownLoad(string url, string savePath, RequestCallback callback, OnDownLoadProgress onProgress)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            MethodType = HTTPMethods.Get,
            RequestDataType = RequestDataType.Bytes,
            DownLoadSavePath = savePath,
            RequestCallback = callback,
            OnDownLoadProgress = onProgress,
            ErrorLog = "HttpDownLoad",
        };

        Request(param);
    }
    
    public void HttpDownLoadImage(string url, RequestCallback callback, object flag)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            MethodType = HTTPMethods.Get,
            RequestCallback = callback,
            ErrorLog = "HttpDownLoadImage",
            Flag = flag,
            RequestDataType = RequestDataType.Texture
        };

        Request(param);
    }
    
    public void HttpUpLoad(string url, string filePath, RequestCallback callback, OnUpLoadProgress onProgress)
    {
        var param = new BestHttpParam
        {
            Uri = url,
            MethodType = HTTPMethods.Post,
            ContentType = "multipart/form-data",
            UploadStream = new FileStream(filePath, FileMode.Open),
            UpLoadFilePath = filePath,
            RequestCallback = callback,
            OnUpLoadProgress = onProgress,
            ErrorLog = "HttpUpLoad",
        };

        Request(param);
    }
    #endregion

    #region private
    
    private void Request(BestHttpParam param)
    {
        if (!_bestHttpParamQueue.Contains(param) && param != null && param.IsEffective())
        {
            _bestHttpParamQueue.Enqueue(param);
        }
    }
    
    // protected override void OnUpdate(float delta)
    // {
    //     base.OnUpdate(delta);
    //
    //     HandleRequest();
    // }
    private void Update()
    {
        HandleRequest();
    }

    private void HandleRequest()
    {
        for (int i = MAX_WORK_REQUEST_COUNT; i >= 0; i--)
        {
            if (_bestHttpParamQueue.Count > 0)
            {
                DoRequest(_bestHttpParamQueue.Dequeue());
            }
        }
    }

    private void DoRequest(BestHttpParam param)
    {
        var request = new HTTPRequest(new Uri(param.Uri));
        request.MethodType = param.MethodType;
        request.Timeout = TimeSpan.FromSeconds(CONNECT_SERVER_TIME_OUT);
        request.ConnectTimeout = TimeSpan.FromSeconds(CONNECT_SERVER_TIME_OUT);

        if (!string.IsNullOrEmpty(param.ContentType))
        {
            request.SetHeader("Content-Type", param.ContentType);
        }

        if (param.RawData != null)
        {
            request.RawData = param.RawData;
        }

        if (param.FormData != null)
        {
            foreach (var data in param.FormData.GetFormData())
            {
                request.AddField(data.Key, data.Value);
            }
        }
        
        request.Callback = OnRequestFinish(param.RequestDataType, param.RequestCallback, param.ErrorLog, param.Flag);
        
        if (param.OnDownLoadProgress != null)
        {
            request.OnProgress = OnProgress(param.OnDownLoadProgress);
        }

        if (param.OnUpLoadProgress != null)
        {
            request.OnUploadProgress = OnUploadProgress(param.OnUpLoadProgress);
        }

        request.Send();

        if (!_toRemoveParamDic.ContainsKey(request))
        {
            _toRemoveParamDic.Add(request, param);
        }
    }

    private OnRequestFinishedDelegate OnRequestFinish(RequestDataType dataType, RequestCallback callback, string errorMsg, object flag = null)
    {
        return (request, response) =>
        {
            if (request.State == HTTPRequestStates.Finished && response.IsSuccess)
            {
                OnRequestSuccess(request, response, dataType, callback, flag);
            }
            else
            {
                OnRequestFailed(request, callback, errorMsg, flag);
            }
        };
    }
    
    private void OnRequestSuccess(HTTPRequest request, HTTPResponse response, RequestDataType dataType, RequestCallback callback, object flag = null)
    {
        if (dataType == RequestDataType.Bytes)
        {
            callback?.Invoke(true, response.Data.Clone(), flag);
        }
        
        else if (dataType == RequestDataType.Texture)
        {
            var texture = response.DataAsTexture2D;
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            callback?.Invoke(true, sprite, flag);
        }
        else
        {
            callback?.Invoke(true, response.DataAsText.Clone(), flag);
        }
        
        if (_toRemoveParamDic.TryGetValue(request, out var param))
        {
            if (param != null && !string.IsNullOrEmpty(param.DownLoadSavePath))
            {
                File.WriteAllBytes(param.DownLoadSavePath, response.Data);
                
                _toRemoveParamDic.Remove(request);
            }
        }
    }
    
    private void OnRequestFailed(HTTPRequest request, RequestCallback callback, string errorMsg, object flag)
    {
        _toRemoveParamDic.TryGetValue(request, out var param);
        _toRemoveParamDic.Remove(request);
        
        if (param != null && param.CanRetry())
        {
            param.AddRetryCount();
            Request(param);
        }
        else
        {
            callback?.Invoke(false, "", flag);
            Debug.LogWarning(errorMsg + " request failed url = " + request.Uri);
        }
    }

    private OnDownloadProgressDelegate OnProgress(OnDownLoadProgress progress)
    {
        return (originalRequest, downloaded, length) => { progress(originalRequest.Uri.AbsoluteUri, downloaded, length); };
    }

    private OnUploadProgressDelegate OnUploadProgress(OnUpLoadProgress progress)
    {
        return (originalRequest, downloaded, length) => { progress(originalRequest.Uri.AbsoluteUri, downloaded, length); };
    }

    public override void Release()
    {
        foreach (var request in _toRemoveParamDic.Keys)
        { 
            request.Dispose();
        }
        _toRemoveParamDic.Clear();
        
        _bestHttpParamQueue.Clear();

        base.Release();
    }
    
    #endregion
}

// todo
// 1.图片资源存储
// 2.下载文件存储
// 3.断点续传