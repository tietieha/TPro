// ----------------------------------------
//
//  CrashSightAgent.cs
//
//  Author:
//       Yeelik,
//
//  Copyright (c) 2015 CrashSight. All rights reserved.
//
// ----------------------------------------
//
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

using GCloud.UQM;

// We dont use the LogType enum in Unity as the numerical order doesnt suit our purposes
/// <summary>
/// Log severity. 
/// { Log, LogDebug, LogInfo, LogWarning, LogAssert, LogError, LogException }
/// </summary>
public enum CSLogSeverity
{
    LogSilent,
    LogError,
    LogWarning,
    LogInfo,
    LogDebug,
    LogVerbose
}
public enum CSReportType
{
    InterfaceReport,
    LogCallback,
    LogCallbackThreaded
}

/// <summary>
/// CrashSight agent.
/// </summary>
public sealed class CrashSightAgent
{
    private static string crashUploadUrl = string.Empty;

    public static List<int> callbackThreads = new List<int>();

    public static object callbackThreadsLock = new object();

    // Define delegate support multicasting to replace the 'Application.LogCallback'
    public delegate void LogCallbackDelegate(string condition, string stackTrace, LogType type);

    private static event LogCallbackDelegate _LogCallbackEventHandler;

    private static bool _isInitialized = false;

    private static LogType _autoReportLogLevel = LogType.Error;

#pragma warning disable 414
    private static bool _debugMode = false;

    private static bool _autoQuitApplicationAfterReport = false;

    private static Func<Dictionary<string, string>> _LogCallbackExtrasHandler;

    private static bool _uncaughtAutoReportOnce = false;


    /************************************全平台接口************************************/

    // 初始化CrashSight
    // appId：appId
    public static void InitWithAppId(string appId)
    {
        if (IsInitialized)
        {
            DebugLog(null, "CrashSightAgent has already been initialized.");

            return;
        }

        if (string.IsNullOrEmpty(appId))
        {
            return;
        }

        // init the sdk with app id
        UQMCrash.InitWithAppId(appId);
        DebugLog(null, "Initialized with app id: {0} crashUploadUrl: {1}", appId, crashUploadUrl);

        // Register the LogCallbackHandler by Application.RegisterLogCallback(Application.LogCallback)
        _RegisterExceptionHandler();
    }

    public static void ReportException(System.Exception e, string message)
    {
        if (!IsInitialized)
        {
            return;
        }

        DebugLog(null, "Report exception: {0}\n------------\n{1}\n------------", message, e);

        _HandleException(e, message, false);
    }

    public static void ReportException(string name, string message, string stackTrace)
    {
        if (!IsInitialized)
        {
            return;
        }

        DebugLog(null, "Report exception: {0} {1} \n{2}", name, message, stackTrace);

        _HandleException(LogType.Exception, name, message, stackTrace, false, CSReportType.InterfaceReport);
    }

    public static void ReportException(int type, string exceptionName, string exceptionMsg, string exceptionStack, Dictionary<string, string> extInfo, int dumpNativeType = 0, string errorAttachmentPath = "")
    {
        if (!IsInitialized)
        {
            return;
        }
        UQMCrash.ReportException(type, exceptionName, exceptionMsg, exceptionStack, extInfo, dumpNativeType, errorAttachmentPath);
    }

    // 设置用户ID
    // userId：用户ID
    public static void SetUserId(string userId)
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "Set user id: {0}", userId);

        UQMCrash.SetUserId(userId);
    }

    // 添加自定义数据
    // key：键
    // value：值
    public static void AddSceneData(string key, string value)
    {
        if (!IsInitialized)
        {
            return;
        }

        DebugLog(null, "Add scene data: [{0}, {1}]", key, value);

        UQMCrash.AddSceneData(key, value);
    }

    public static void SetUserValue(string key, int value)
    {
        AddSceneData("I#" + key, "" + value);
    }

    public static void SetUserValue(string key, string value)
    {
        AddSceneData("K#" + key, value);
    }

    public static void SetUserValue(string key, string[] value)
    {
        string valueStr = string.Join("#", value);
        AddSceneData("S#" + key, valueStr);
    }

    // 设置应用版本号
    // appVersion：版本号
    public static void SetAppVersion(string appVersion)
    {
        DebugLog(null, "Set app version: {0}", appVersion);

        UQMCrash.SetAppVersion(appVersion);
    }

    // 设置上报域名
    // crashServerUrl：上报域名
    public static void ConfigCrashServerUrl(string crashServerUrl)
    {
        DebugLog(null, "Config crashServerUrl:{0}", crashServerUrl);
        UQMCrash.ConfigCrashServerUrl(crashServerUrl);
    }

    // 设置自定义日志路径（Switch不可用）
    // logPath：日志路径
    public static void SetLogPath(string logPath)
    {
        UQMCrash.SetLogPath(logPath);
    }

    // debug开关
    public static void ConfigDebugMode(bool enable)
    {
        _debugMode = enable;
        UQMCrash.ConfigDebugMode(enable);
        DebugLog(null, "{0} the log message print to console", enable ? "Enable" : "Disable");
    }

    // 设置deviceId
    // deviceId：设备唯一标识
    public static void SetDeviceId(string deviceId)
    {
        UQMCrash.SetDeviceId(deviceId);
    }

    // 设置日志上报等级
    // logLevel：Off=0,Error=1,Warn=2,Info=3,Debug=4
    public static void ConfigCrashReporter(int logLevel)
    {
        UQMCrash.ConfigAutoReportLogLevel(logLevel);
    }
    public static void ConfigCrashReporter(CSLogSeverity logLevel)
    {
        UQMCrash.ConfigAutoReportLogLevel((int)logLevel);
    }

    // 添加自定义日志
    // level：日志级别
    // format：日志格式
    // args：可变参数
    public static void PrintLog(CSLogSeverity level, string format, params object[] args)
    {
        if (string.IsNullOrEmpty(format))
        {
            return;
        }
        if (args == null || args.Length == 0)
        {
            UQMCrash.LogRecord((int)level, format);
        }
        else
        {
            UQMCrash.LogRecord((int)level, string.Format(format, args));
        }
    }

    // 测试native崩溃
    public static void TestNativeCrash()
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "test native crash");

        UQMCrash.TestNativeCrash();
    }

    // 设置子场景（PS4、PS5、Switch、Linux暂不可用）
    // serverEnv:子场景名称
    public static void SetEnvironmentName(string serverEnv)
    {
        UQMCrash.SetServerEnv(serverEnv);
    }

    public static void RegisterCrashCallback(CrashSightCallback callback)
    {
        if (callback != null)
        {
            UQMCrash.CrashBaseRetEvent += callback.OnCrashBaseRetEvent;
            UQMCrash.ConfigCallBack();
        }
        else
        {
            DebugLog(null, "RegisterCallback failed: callback is null.");
        }
    }

    public static void UnregisterCrashCallback()
    {
        UQMCrash.UnregisterCallBack();
    }

    public static void RegisterCrashLogCallback(CrashSightLogCallback callback)
    {
        if (callback != null)
        {
            UQMCrash.CrashSetLogPathRetEvent += callback.OnSetLogPathEvent;
            UQMCrash.CrashLogUploadRetEvent += callback.OnLogUploadResultEvent;
            UQMCrash.ConfigLogCallBack();
        }
        else
        {
            DebugLog(null, "RegisterCallback failed: callback is null.");
        }
    }

    public static void EnableExceptionHandler()
    {
        if (IsInitialized)
        {
            DebugLog(null, "CrashSightAgent has already been initialized.");
            return;
        }

        DebugLog(null, "Only enable the exception handler, please make sure you has initialized the sdk in the native code in associated Android or iOS project.");

        // Register the LogCallbackHandler by Application.RegisterLogCallback(Application.LogCallback)
        _RegisterExceptionHandler();
    }

    public static void RegisterLogCallback(LogCallbackDelegate handler)
    {
        if (handler != null)
        {
            DebugLog(null, "Add log callback handler: {0}", handler);

            _LogCallbackEventHandler += handler;
        }
    }

    public static void UnregisterLogCallback(LogCallbackDelegate handler)
    {
        if (handler != null)
        {
            DebugLog(null, "Remove log callback handler");

            _LogCallbackEventHandler -= handler;
        }
    }

    public static void SetLogCallbackExtrasHandler(Func<Dictionary<string, string>> handler)
    {
        if (handler != null)
        {
            _LogCallbackExtrasHandler = handler;

            DebugLog(null, "Add log callback extra data handler : {0}", handler);
        }
    }

    public static void ConfigAutoQuitApplication(bool autoQuit)
    {
        _autoQuitApplicationAfterReport = autoQuit;
    }

    public static bool AutoQuitApplicationAfterReport
    {
        get { return _autoQuitApplicationAfterReport; }
    }

    public static void DebugLog(string tag, string format, params object[] args)
    {
        if (!_debugMode)
        {
            return;
        }

        if (string.IsNullOrEmpty(format))
        {
            return;
        }

        UQMLog.Log(string.Format("{0}:{1}", tag, string.Format(format, args)));
    }

    public static bool IsInitialized
    {
        get { return _isInitialized; }
    }

    public static void _RegisterExceptionHandler()
    {
        try
        {
            // hold only one instance

#if UNITY_5 || UNITY_5_3_OR_NEWER
            Application.logMessageReceived += _OnLogCallbackHandlerMain;
            Application.logMessageReceivedThreaded += _OnLogCallbackHandlerThreaded;
#else
            Application.RegisterLogCallback(_OnLogCallbackHandlerMain);
            Application.RegisterLogCallbackThreaded(_OnLogCallbackHandlerThreaded);
#endif
            AppDomain.CurrentDomain.UnhandledException += _OnUncaughtExceptionHandler;

            string version = Application.unityVersion;
#if UNITY_EDITOR
            string buildConfig = "editor";
#elif DEVELOPMENT_BUILD
            string buildConfig = "development";
#else
            string buildConfig = "release";
#endif
            SystemLanguage language = Application.systemLanguage;
            CultureInfo locale = CultureInfo.CurrentCulture;
            UQMCrash.SetEngineInfo(version, buildConfig, language.ToString(), locale.ToString());

            _isInitialized = true;

            DebugLog(null, "Register the log callback in Unity {0}", Application.unityVersion);
        }
        catch
        {

        }
    }

    public static void _UnregisterExceptionHandler()
    {
        try
        {
#if UNITY_5 || UNITY_5_3_OR_NEWER
            Application.logMessageReceived -= _OnLogCallbackHandlerMain;
            Application.logMessageReceivedThreaded -= _OnLogCallbackHandlerThreaded;
#else
            Application.RegisterLogCallback(null);
            Application.RegisterLogCallbackThreaded(null);
#endif
            System.AppDomain.CurrentDomain.UnhandledException -= _OnUncaughtExceptionHandler;
            DebugLog(null, "Unregister the log callback in unity {0}", Application.unityVersion);
        }
        catch
        {

        }
    }

    public static void SetCrashSightStackTraceEnable(bool enable)
    {
        CrashSightStackTrace.setEnable(enable);
    }

    /************************************移动端接口************************************/

    // 各类上报的回调开关（Android、iOS）
    // callbackType：目前是5种类型，用5位表示。第一位表示crash，第二位表示anr，第三位表示u3d c#
    // error，第四位表示js，第五位表示lua
    public static void ConfigCallbackType(Int32 callbackType)
    {
        UQMCrash.ConfigCallbackType(callbackType);
    }

    // 设置deviceModel（Android、iOS）
    // deviceModel：手机型号
    public static void SetDeviceModel(string deviceModel)
    {
        UQMCrash.SetDeviceModel(deviceModel);
    }

    // Report log statistics（Android、iOS）
    // msgType：消息类型
    // msg：消息详情
    public static void ReportLogInfo(string msgType, string msg)
    {
        UQMCrash.ReportLogInfo(msgType, msg);
    }

    // 设置场景（Android、iOS）
    // sceneId：场景ID
    public static void SetScene(string sceneId, bool upload = false)
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "Set scene: {0}, upload:{1}", sceneId, upload);

        UQMCrash.SetScene(sceneId, upload);
    }
    public static void SetScene(int sceneId)
    {
        SetScene("" + sceneId);
    }

    // 当崩溃发生时，获取崩溃线程ID，失败时返回-1（Android、iOS）
    public static long GetCrashThreadId()
    {
        if (!IsInitialized)
        {
            return -1;
        }
        DebugLog(null, "GetCrashThreadId");

        return UQMCrash.GetCrashThreadId();
    }

    // 设置自定义device ID（Android、iOS）
    // deviceId:device ID
    public static void SetCustomizedDeviceID(string deviceId)
    {
        UQMCrash.SetCustomizedDeviceID(deviceId);
    }

    // 获取SDK生成的device ID（Android、iOS）
    public static string GetSDKDefinedDeviceID()
    {
        return UQMCrash.GetSDKDefinedDeviceID();
    }

    // 设置自定义match ID（Android、iOS）
    // matchId:match ID
    public static void SetCustomizedMatchID(string matchId)
    {
        UQMCrash.SetCustomizedMatchID(matchId);
    }

    // 获取SDK生成的session ID（Android、iOS）
    public static string GetSDKSessionID()
    {
        return UQMCrash.GetSDKSessionID();
    }

    // 测试oom（Android、iOS）
    public static void TestOomCrash()
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "test oom crash");

        UQMCrash.TestOomCrash();
    }

    // 测试java崩溃（Android）
    public static void TestJavaCrash()
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "test java crash");

        UQMCrash.TestJavaCrash();
    }

    // 测试ANR（Android）
    public static void TestANR()
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "test ANR");

        UQMCrash.TestANR();
    }

    // 获取崩溃UUID（Android）
    public static string GetCrashUuid()
    {
        return UQMCrash.GetCrashUuid();
    }

    // 设置logcat缓存大小（Android）
    public static void SetLogcatBufferSize(int size)
    {
        UQMCrash.SetLogcatBufferSize(size);
    }

    // 测试objective-C崩溃（iOS）
    public static void TestOcCrash()
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "test oc crash");

        UQMCrash.TestOcCrash();
    }

    // 启动定时dump（Android）
    // dumpMode:dump模式，1：dump，2：minidump
    // startTimeMode:启动时间模式，0：绝对时间，1：相对时间，单位：毫秒
    // startTime:启动时间
    // dumpInterval:dump间隔，单位：毫秒
    // dumpTimes:dump次数
    // saveLocal:是否保存本地
    // savePath:本地保存路径
    public static void StartDumpRoutine(int dumpMode, int startTimeMode, long startTime,
                long dumpInterval, int dumpTimes, bool saveLocal, string savePath)
    {
        if (!IsInitialized)
        {
            return;
        }
        UQMCrash.StartDumpRoutine(dumpMode, startTimeMode, startTime, dumpInterval, dumpTimes, saveLocal, savePath);
    }

    // 监控FD数量（Android）
    // interval:扫描间隔，单位：毫秒
    // limit:FD数量限制，超过将触发一次上报
    // dumpType:dump方式，0：不上报dump，1：系统接口dump，3：minidump
    public static void StartMonitorFdCount(int interval, int limit, int dumpType)
    {
        if (!IsInitialized)
        {
            return;
        }
        UQMCrash.StartMonitorFdCount(interval, limit, dumpType);
    }

    // 获取异常类型编号（Android、iOS）
    // name:异常类型名，如“c#”、“js”、“lua”、“custom1”等
    // 返回值:异常类型编号，可用于填写ReportException接口的type
    public static int getExceptionType(string name)
    {
        if (!IsInitialized)
        {
            return 0;
        }
        return UQMCrash.getExceptionType(name);
    }

    // 测试释放后使用内存（Android）
    public static void TestUseAfterFree()
    {
        if (!IsInitialized)
        {
            return;
        }
        DebugLog(null, "test UseAfterFree");

        UQMCrash.TestUseAfterFree();
    }

    // 重启CrashSight监控（Android）
    public static void ReRegistAllMonitors()
    {
        _isInitialized = true;
        UQMCrash.ReRegistAllMonitors();
        DebugLog(null, "ReRegistAllMonitors");
    }

    // 关闭CrashSight监控（Android）
    public static void CloseAllMonitors()
    {
        UQMCrash.CloseAllMonitors();
        DebugLog(null, "CloseAllMonitors");
    }

    public static void setEnableGetPackageInfo(bool enable)
    {
        DebugLog(null, "setEnableGetPackageInfo: " + enable);

        UQMCrash.setEnableGetPackageInfo(enable);
    }

    public static bool IsLastSessionCrash()
    {
        if (!IsInitialized)
        {
            return false;
        }
        DebugLog(null, "IsLastSessionCrash");

        return UQMCrash.IsLastSessionCrash();
    }

    public static string GetLastSessionUserId()
    {
        if (!IsInitialized)
        {
            return "";
        }
        DebugLog(null, "GetLastSessionUserId");

        return UQMCrash.GetLastSessionUserId();
    }

    public static bool CheckFdCount(int limit, int dumpType, bool upload)
    {
        if (!IsInitialized)
        {
            return false;
        }
        return UQMCrash.CheckFdCount(limit, dumpType, upload);
    }

    // 设置OOM日志路径（Android、iOS）
    // logPath：日志路径
    public static void SetOomLogPath(string logPath)
    {
        UQMCrash.SetOomLogPath(logPath);
    }

    /************************************PC、Xbox端接口************************************/

    // VEH异常捕获开关（Win、Xbox）
    public static void SetVehEnable(bool enable)
    {
        UQMCrash.SetVehEnable(enable);
        DebugLog(null, "SetVehEnable");
    }

    // 主动上报崩溃（Win、Xbox）
    public static void ReportCrash()
    {
        UQMCrash.ReportCrash();
        DebugLog(null, "ReportCrash");
    }

    // 主动上报dump（Win、Xbox）
    // dump_path：dump目录
    // is_async：是否异步
    public static void ReportDump(string dump_path, bool is_async)
    {
        UQMCrash.ReportDump(dump_path, is_async);
        DebugLog(null, "ReportDump");
    }

    // 额外异常捕获开关（Win、Xbox）
    public static void SetExtraHandler(bool extra_handle_enable)
    {
        UQMCrash.SetExtraHandler(extra_handle_enable);
        DebugLog(null, "SetExtraHandler");
    }

    // 上传dump文件（Win、Xbox）
    // dump_dir：dump文件目录
    // is_extra_check：默认填false即可
    public static void UploadGivenPathDump(string dump_dir, bool is_extra_check)
    {
        UQMCrash.UploadGivenPathDump(dump_dir, is_extra_check);
        DebugLog(null, "UploadGivenPathDump");
    }

    // 设置dump类型（Win、Xbox）
    // dump_type：dump类型，详见Windows官方定义
    public static void SetDumpType(int dump_type)
    {
        UQMCrash.SetDumpType(dump_type);
    }

    // 设置可用的异常类型，使用前请咨询CrashSight开发人员（Win、Xbox）
    // exp_code：异常代码，详见Windows官方定义
    public static void AddValidExpCode(ulong exp_code)
    {
        UQMCrash.AddValidExpCode(exp_code);
    }

    // 根据问题GUID上报（Win、Xbox）
    // guid：指向唯一问题的代码，可通过回调获取
    public static void UploadCrashWithGuid(string guid)
    {
        UQMCrash.UploadCrashWithGuid(guid);
    }

    // 崩溃上报开关（Win、Xbox）
    public static void SetCrashUploadEnable(bool enable)
    {
        UQMCrash.SetCrashUploadEnable(enable);
    }

    // 设置工作空间（Win、Xbox）
    // workspace：工作空间的绝对路径
    public static void SetWorkSpace(string workspace)
    {
        UQMCrash.SetWorkSpace(workspace);
    }

    // 设置附件路径（Win、Xbox）
    // path：附件的绝对路径
    public static void SetCustomAttachDir(string path)
    {
        UQMCrash.SetCustomAttachDir(path);
    }

    /************************************PS4、PS5、Switch端接口************************************/

    // 设置错误上报间隔（PS4、PS5、Switch）
    // interval：错误上报间隔（单位：秒）
    public static void SetErrorUploadInterval(int interval)
    {
        UQMCrash.SetErrorUploadInterval(interval);
        DebugLog(null, "SetErrorUploadInterval");
    }

    // 错误上报开关（PS4、PS5、Switch）
    public static void SetErrorUploadEnable(bool enable)
    {
        UQMCrash.SetErrorUploadEnable(enable);
        DebugLog(null, "SetErrorUploadEnable");
    }

    /************************************Linux端接口************************************/

    // 设置所有记录文件的路径(Linux)
    // record_dir:记录文件的路径
    public static void SetRecordFileDir(string record_dir)
    {
        UQMCrash.SetRecordFileDir(record_dir);
        DebugLog(null, "SetRecordFileDir");
    }

    /************************************已废弃接口************************************/

    // 初始化（Win、Xbox）
    // userId：用户id（Win、Xbox）
    // version：应用版本号
    // key：appkey
    public static void InitContext(string userId, string version, string key)
    {
        if (IsInitialized)
        {
            DebugLog(null, "CrashSightAgent has already been initialized.");

            return;
        }

        if (userId == null || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(key))
        {
            return;
        }

        _isInitialized = true;
        // init the sdk with app id
        UQMCrash.InitContext(userId, version, key);
        DebugLog(null, "Initialized with userId: {0} version: {1} key: {2}", userId, version, key);

        // Register the LogCallbackHandler by Application.RegisterLogCallback(Application.LogCallback)
        _RegisterExceptionHandler();
    }

    // 初始化（Linux）
    // app_id:appid
    // app_key:appkey
    // app_version:版本号
    public static void Init(string app_id, string app_key, string app_version)
    {
        if (IsInitialized)
        {
            DebugLog(null, "CrashSightAgent has already been initialized.");

            return;
        }

        if (string.IsNullOrEmpty(app_id) || string.IsNullOrEmpty(app_key) || string.IsNullOrEmpty(app_version))
        {
            return;
        }

        _isInitialized = true;
        // init the sdk with app id
        UQMCrash.Init(app_id, app_key, app_version);
        DebugLog(null, "Initialized with app_id: {0} app_key: {1} app_version: {2}", app_id, app_key, app_version);

        // Register the LogCallbackHandler by Application.RegisterLogCallback(Application.LogCallback)
        _RegisterExceptionHandler();
    }

    // 初始化前填写配置信息（Android、iOS、Switch）
    // channel：已弃用，默认填“”即可
    // version：应用版本号
    // user：用户id
    // delay：延时
    public static void ConfigDefault(string channel, string version, string user, long delay)
    {
        DebugLog(null, "Config default channel:{0}, version:{1}, user:{2}, delay:{3}", channel, version, user, delay);
        UQMCrash.ConfigDefault(channel, version, user, delay);
    }

    #region Privated Fields and Methods
    /************************************private部分************************************/

    private static void _OnLogCallbackHandlerMain(string condition, string stackTrace, LogType type)
    {
        _OnLogCallbackHandler(condition, stackTrace, type, CSReportType.LogCallback);
    }

    private static void _OnLogCallbackHandlerThreaded(string condition, string stackTrace, LogType type)
    {
        _OnLogCallbackHandler(condition, stackTrace, type, CSReportType.LogCallbackThreaded);
    }

    private static void _OnLogCallbackHandler(string condition, string stackTrace, LogType type, CSReportType rType)
    {
        if (_LogCallbackEventHandler != null)
        {
            _LogCallbackEventHandler(condition, stackTrace, type);
        }

        if (!IsInitialized)
        {
            return;
        }

        if (!string.IsNullOrEmpty(condition) && condition.Contains("[CrashSightAgent] <Log>"))
        {
            return;
        }

        if (_uncaughtAutoReportOnce)
        {
            return;
        }

        if (callbackThreads.Contains(Thread.CurrentThread.ManagedThreadId))
        {
            return;
        }

        _HandleException(type, null, condition, stackTrace, true, rType);
    }

    private static void _OnUncaughtExceptionHandler(object sender, System.UnhandledExceptionEventArgs args)
    {
        if (args == null || args.ExceptionObject == null)
        {
            return;
        }

        try
        {
            if (args.ExceptionObject.GetType() != typeof(System.Exception))
            {
                return;
            }
        }
        catch
        {
            if (UnityEngine.Debug.isDebugBuild == true)
            {
                UnityEngine.Debug.Log("CrashSightAgent: Failed to report uncaught exception");
            }

            return;
        }

        if (!IsInitialized)
        {
            return;
        }

        if (_uncaughtAutoReportOnce)
        {
            return;
        }

        _HandleException((System.Exception)args.ExceptionObject, null, true);
    }

    private static void _HandleException(System.Exception e, string message, bool uncaught)
    {
        if (e == null)
        {
            return;
        }

        if (!IsInitialized)
        {
            return;
        }

        string name = e.GetType().Name;
        string reason = e.Message;

        if (!string.IsNullOrEmpty(message))
        {
            reason = string.Format("{0}{1}***{2}", reason, Environment.NewLine, message);
        }

        StringBuilder stackTraceBuilder = new StringBuilder(512);

        StackTrace stackTrace = new StackTrace(e, true);
        int count = stackTrace.FrameCount;
        for (int i = 0; i < count; i++)
        {
            StackFrame frame = stackTrace.GetFrame(i);

            stackTraceBuilder.AppendFormat("{0}.{1}", frame.GetMethod().DeclaringType.Name, frame.GetMethod().Name);

            ParameterInfo[] parameters = frame.GetMethod().GetParameters();
            if (parameters == null || parameters.Length == 0)
            {
                stackTraceBuilder.Append(" () ");
            }
            else
            {
                stackTraceBuilder.Append(" (");

                int pcount = parameters.Length;

                ParameterInfo param = null;
                for (int p = 0; p < pcount; p++)
                {
                    param = parameters[p];
                    stackTraceBuilder.AppendFormat("{0} {1}", param.ParameterType.Name, param.Name);

                    if (p != pcount - 1)
                    {
                        stackTraceBuilder.Append(", ");
                    }
                }
                param = null;

                stackTraceBuilder.Append(") ");
            }

            string fileName = frame.GetFileName();
            if (!string.IsNullOrEmpty(fileName) && !fileName.ToLower().Equals("unknown"))
            {
                fileName = fileName.Replace("\\", "/");

                int loc = fileName.ToLower().IndexOf("/assets/");
                if (loc < 0)
                {
                    loc = fileName.ToLower().IndexOf("assets/");
                }

                if (loc > 0)
                {
                    fileName = fileName.Substring(loc);
                }

                stackTraceBuilder.AppendFormat("(at {0}:{1})", fileName, frame.GetFileLineNumber());
            }
            stackTraceBuilder.AppendLine();
        }

        // report
        _reportException(uncaught, name, reason, stackTraceBuilder.ToString());
    }

    private static bool ShouldSkipFrame(string frame)
    {
        string[] skipPatterns = { "System.Collections.Generic.", "ShimEnumerator", "CrashSight" };

        foreach (var pattern in skipPatterns)
        {
            if (frame.StartsWith(pattern, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static void _reportException(bool uncaught, string name, string reason, string stackTrace)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        if (string.IsNullOrEmpty(stackTrace))
        {
            stackTrace = StackTraceUtility.ExtractStackTrace();
        }

        if (string.IsNullOrEmpty(stackTrace))
        {
            stackTrace = "Empty";
        }
        else
        {

            try
            {
                string[] frames = stackTrace.Split('\n');

                if (frames != null && frames.Length > 0)
                {

                    StringBuilder trimFrameBuilder = new StringBuilder(512);

                    string frame = null;
                    int count = frames.Length;
                    for (int i = 0; i < count; i++)
                    {
                        frame = frames[i];

                        if (string.IsNullOrEmpty(frame) || string.IsNullOrEmpty(frame.Trim()))
                        {
                            continue;
                        }

                        frame = frame.Trim();

                        if (string.IsNullOrEmpty(frame) || ShouldSkipFrame(frame) || frame.Contains("..ctor"))
                        {
                            continue;
                        }

                        int start = -1;
                        int end = -1;
                        if (frame.Contains("(at") && frame.Contains("/assets/"))
                        {
                            start = frame.IndexOf("(at", StringComparison.OrdinalIgnoreCase);
                            end = frame.IndexOf("/assets/", StringComparison.OrdinalIgnoreCase);

                        }
                        if (start > 0 && end > 0)
                        {
                            trimFrameBuilder.AppendFormat("{0}(at {1}", frame.Substring(0, start).Replace(":", "."), frame.Substring(end));
                        }
                        else
                        {
                            trimFrameBuilder.Append(frame.Replace(":", "."));
                        }

                        trimFrameBuilder.AppendLine();
                    }

                    stackTrace = trimFrameBuilder.ToString();
                }
            }
            catch
            {
                PrintLog(CSLogSeverity.LogWarning, "{0}", "Error to parse the stack trace");
            }

        }

        PrintLog(CSLogSeverity.LogError, "ReportException: " + name + " " + reason + "\n*********\n" + stackTrace + "\n*********");

        _uncaughtAutoReportOnce = uncaught && _autoQuitApplicationAfterReport;

        string extraInfo = string.Empty;
        Dictionary<string, string> extras = null;
        if (_LogCallbackExtrasHandler != null)
        {
            extras = _LogCallbackExtrasHandler();
        }

        if (extras != null && extras.Count > 0)
        {
            StringBuilder builder = new StringBuilder(128);
            foreach (KeyValuePair<string, string> kvp in extras)
            {
                builder.AppendFormat("\"{0}\" : \"{1}\", ", kvp.Key, kvp.Value);
            }
            builder.Length -= 2;  // 去掉最后一个逗号和空格
            extraInfo = "{" + builder.ToString() + "}";
        }
        UQMCrash.ReportException(4, name, reason, stackTrace, extraInfo, uncaught && _autoQuitApplicationAfterReport);
    }

    private static int valueOf(LogType logLevel)
    {
        switch(logLevel)
        {
            case LogType.Exception:
                return 1;
            case LogType.Error:
                return 2;
            case LogType.Assert:
                return 3;
            case LogType.Warning:
                return 4;
            case LogType.Log:
                return 5;
            default:
                return 6;
        }
    }

    private static bool isEnableAutoReport(LogType logLevel)
    {
        return valueOf(logLevel) <= valueOf(_autoReportLogLevel);
    }

    private static void _HandleException(LogType logLevel, string name, string message, string stackTrace, bool uncaught, CSReportType rType)
    {
        if (!IsInitialized)
        {
            DebugLog(null, "It has not been initialized.");
            return;
        }

        if ((uncaught && !isEnableAutoReport(logLevel)))
        {
            DebugLog(null, "Not report exception for level {0}", logLevel.ToString());
            return;
        }

        string type = null;
        string reason = "";

        if (!string.IsNullOrEmpty(message))
        {
            try
            {
                if ((LogType.Exception == logLevel) && message.Contains("Exception"))
                {

                    Match match = new Regex(@"^(?<errorType>\S+):\s*(?<errorMessage>.*)", RegexOptions.Singleline).Match(message);

                    if (match.Success)
                    {
                        if (stackTrace.Contains("UnityEngine.Debug:LogException"))
                        {
                            if (rType == CSReportType.LogCallback)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (rType == CSReportType.LogCallbackThreaded)
                            {
                                return;
                            }
                        }
                        type = match.Groups["errorType"].Value.Trim();
                        reason = match.Groups["errorMessage"].Value.Trim();
                    }
                    else if (rType == CSReportType.LogCallback)
                    {
                        return;
                    }
                }
                else if ((LogType.Error == logLevel) && message.StartsWith("Unhandled Exception:", StringComparison.Ordinal))
                {

                    Match match = new Regex(@"^Unhandled\s+Exception:\s*(?<exceptionName>\S+):\s*(?<exceptionDetail>.*)", RegexOptions.Singleline).Match(message);

                    if (match.Success)
                    {
                        if (rType == CSReportType.LogCallbackThreaded)
                        {
                            return;
                        }
                        string exceptionName = match.Groups["exceptionName"].Value.Trim();
                        string exceptionDetail = match.Groups["exceptionDetail"].Value.Trim();

                        //
                        int dotLocation = exceptionName.LastIndexOf(".");
                        if (dotLocation > 0 && dotLocation != exceptionName.Length)
                        {
                            type = exceptionName.Substring(dotLocation + 1);
                        }
                        else
                        {
                            type = exceptionName;
                        }

                        int stackLocation = exceptionDetail.IndexOf(" at ");
                        if (stackLocation > 0)
                        {
                            //
                            reason = exceptionDetail.Substring(0, stackLocation);
                            // substring after " at "
                            string callStacks = exceptionDetail.Substring(stackLocation + 3).Replace(" at ", "\n").Replace("in <filename unknown>:0", string.Empty).Replace("[0x00000]", string.Empty);
                            //
                            stackTrace = stackTrace + "\n" + callStacks.Trim();

                        }
                        else
                        {
                            reason = exceptionDetail;
                        }

                        // for LuaScriptException
                        if (type.Equals("LuaScriptException") && exceptionDetail.Contains(".lua") && exceptionDetail.Contains("stack traceback:"))
                        {
                            stackLocation = exceptionDetail.IndexOf("stack traceback:");
                            if (stackLocation > 0)
                            {
                                reason = exceptionDetail.Substring(0, stackLocation);
                                // substring after "stack traceback:"
                                string callStacks = exceptionDetail.Substring(stackLocation + 16).Replace(" [", " \n[");

                                //
                                stackTrace = stackTrace + "\n" + callStacks.Trim();
                            }
                        }
                    }
                    else if (rType == CSReportType.LogCallback)
                    {
                        return;
                    }
                }
                else if (rType == CSReportType.LogCallback)
                {
                    return;
                }
            }
            catch
            {

            }

            if (string.IsNullOrEmpty(reason))
            {
                reason = message;
            }
        }

        if (string.IsNullOrEmpty(name))
        {
            if (string.IsNullOrEmpty(type))
            {
                string sLogLevel = "Log";
                switch (logLevel)
                {
                    case LogType.Log:
                        sLogLevel = "Log";
                        break;
                    case LogType.Warning:
                        sLogLevel = "LogWarning";
                        break;
                    case LogType.Assert:
                        sLogLevel = "LogAssert";
                        break;
                    case LogType.Error:
                        sLogLevel = "LogError";
                        break;
                    case LogType.Exception:
                        sLogLevel = "LogException";
                        break;
                }
                type = "Unity" + sLogLevel;
                if (CrashSightStackTrace.enable)
                {
                    try
                    {
                        stackTrace = CrashSightStackTrace.ExtractStackTrace();
                        //去掉前三行
                        if (stackTrace.Contains("CrashSightAgent:_HandleException(LogType, String, String, String, Boolean)")
                            && stackTrace.Contains("CrashSightAgent:_OnLogCallbackHandler(String, String, LogType)")
                            && stackTrace.Contains("UnityEngine.Application:CallLogCallback(String, String, LogType, Boolean)"))
                        {
                            int count = stackTrace.IndexOf("\n");
                            count = stackTrace.IndexOf("\n", count + 1);
                            count = stackTrace.IndexOf("\n", count + 1);
                            stackTrace = stackTrace.Substring(count + 1);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        else
        {
            type = name;
        }

        _reportException(uncaught, type, reason, stackTrace);
    }

    #endregion
}
