// file:    HttpRequestUtils.cs
// author:  nancheng
// date:    2023年10月23日-15:13:03
// desc:    网络请求工具类

public class HttpRequestUtils
{
    /// <summary>
    /// 获取表单数据
    /// </summary>
    /// <returns></returns>
    public static RequestFormData GetRequestFormData()
    {
        return new RequestFormData();
    }

    /// <summary>
    /// 直接请求
    /// </summary>
    public static void Get(string url, RequestDataType dataType, BestHttpManager.RequestCallback callback)
    {
        BestHttpManager.Instance.Get(url, dataType, callback);
    }
    
    /// <summary>
    /// 表单请求
    /// </summary>
    public static void PostByFormData(string url, RequestDataType dataType, RequestFormData formData, BestHttpManager.RequestCallback callback)
    {
        BestHttpManager.Instance.PostByFormData(url, dataType, formData, callback);
    }
    
    /// <summary>
    /// 原生数据请求
    /// </summary>
    public static void PostByRawData(string url, RequestDataType dataType, byte[] rawData, BestHttpManager.RequestCallback callback)
    {
        BestHttpManager.Instance.PostByRawData(url, dataType, rawData, callback);
    }
    
    /// <summary>
    /// 原生数据请求
    /// </summary>
    public static void PostByJson(string url, string jsonData, BestHttpManager.RequestCallback callback, object flag)
    {
        BestHttpManager.Instance.PostByJson(url, jsonData, callback, flag);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    public static void HttpDownLoad(string url, string savePath, BestHttpManager.RequestCallback callback, BestHttpManager.OnDownLoadProgress onProgress)
    {
        BestHttpManager.Instance.HttpDownLoad(url, savePath, callback, onProgress);
    }

    public static void HttpDownLoadImage(string url, BestHttpManager.RequestCallback callback, object flag)
    {
        BestHttpManager.Instance.HttpDownLoadImage(url, callback,flag);
    }
    
    /// <summary>
    /// 上传文件
    /// </summary>
    public static void HttpUpLoad(string url, string filePath, BestHttpManager.RequestCallback callback, BestHttpManager.OnUpLoadProgress onProgress)
    {
        BestHttpManager.Instance.HttpUpLoad(url, filePath, callback, onProgress);
    }
}