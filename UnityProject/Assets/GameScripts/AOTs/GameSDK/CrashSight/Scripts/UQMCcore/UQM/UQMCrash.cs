using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GCloud.UQM
{
    public enum UQMCrashLevel
    {
        CSLogLevelSilent = 0, //关闭日志记录功能
        CSLogLevelError = 1,
        CSLogLevelWarn = 2,
        CSLogLevelInfo = 3,
        CSLogLevelDebug = 4,
        CSLogLevelVerbose = 5,
    }

    public static class UQMCrash
    {
#if !UNITY_EDITOR
#if (UNITY_ANDROID || UNITY_IOS || UNITY_OPENHARMONY)
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_configAutoReportLogLevelAdapter(int level);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_configGameTypeAdapter(int gameType);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_configCallbackTypeAdapter(Int32 callbackType);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_configDefaultAdapter([MarshalAs(UnmanagedType.LPStr)] string channel,
            [MarshalAs(UnmanagedType.LPStr)] string version,
            [MarshalAs(UnmanagedType.LPStr)] string user,
            long delay);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_configCrashServerUrlAdapter([MarshalAs(UnmanagedType.LPStr)] string serverUrl);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_configDebugModeAdapter(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_initWithAppIdAdapter([MarshalAs(UnmanagedType.LPStr)] string appId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_logRecordAdapter(int level, [MarshalAs(UnmanagedType.LPStr)] string message);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_addSceneDataAdapter([MarshalAs(UnmanagedType.LPStr)] string k, [MarshalAs(UnmanagedType.LPStr)] string v);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_reportExceptionAdapter(int type, [MarshalAs(UnmanagedType.LPStr)] string exceptionName,
            [MarshalAs(UnmanagedType.LPStr)] string exceptionMsg, [MarshalAs(UnmanagedType.LPStr)] string exceptionStack, [MarshalAs(UnmanagedType.LPStr)] string extras, 
            [MarshalAs(UnmanagedType.LPStr)] string paramsJson, bool quitProgram, int dumpNativeType, [MarshalAs(UnmanagedType.LPStr)] string errorAttachmentPath);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setUserIdAdapter([MarshalAs(UnmanagedType.LPStr)] string userId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setSceneAdapter([MarshalAs(UnmanagedType.LPStr)] string sceneId, bool upload);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_unityCrashCallback();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_unregisterUnityCrashCallback();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_unityCrashLogCallback();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_reRegistAllMonitorsAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_closeAllMonitorsAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_reportLogInfo([MarshalAs(UnmanagedType.LPStr)] string msgType,[MarshalAs(UnmanagedType.LPStr)] string msg);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setAppVersionAdapter([MarshalAs(UnmanagedType.LPStr)] string appVersion);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setDeviceIdAdapter([MarshalAs(UnmanagedType.LPStr)] string deviceId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setCustomizedDeviceIDAdapter([MarshalAs(UnmanagedType.LPStr)] string deviceId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr cs_getSDKDefinedDeviceIDAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setCustomizedMatchIDAdapter([MarshalAs(UnmanagedType.LPStr)] string matchId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr cs_getSDKSessionIDAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr cs_getCrashUuidAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setDeviceModelAdapter([MarshalAs(UnmanagedType.LPStr)] string deviceModel);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setLogPathAdapter([MarshalAs(UnmanagedType.LPStr)] string logPath);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_testOomCrashAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_testJavaCrashAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_testOcCrashAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_testNativeCrashAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_testANRAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern long cs_getCrashThreadId();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setLogcatBufferSize(int size);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setCallbackMsgAdapter([MarshalAs(UnmanagedType.LPStr)] string deviceModel);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_startDumpRoutine(int dumpMode, int startTimeMode, long startTime,
         long dumpInterval, int dumpTimes, bool saveLocal, [MarshalAs(UnmanagedType.LPStr)] string savePath);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_startMonitorFdCount(int interval, int limit, int dumpType);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int cs_getExceptionType(string name);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_testUseAfterFreeAdapter();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setEnableGetPackageInfo(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setServerEnv([MarshalAs(UnmanagedType.LPStr)] string serverEnv);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setEngineInfo([MarshalAs(UnmanagedType.LPStr)] string version, [MarshalAs(UnmanagedType.LPStr)] string buildConfig, [MarshalAs(UnmanagedType.LPStr)] string language, [MarshalAs(UnmanagedType.LPStr)] string locale);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setGpuInfo([MarshalAs(UnmanagedType.LPStr)] string version, [MarshalAs(UnmanagedType.LPStr)] string vendor, [MarshalAs(UnmanagedType.LPStr)] string renderer);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool cs_isLastSessionCrash();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr cs_getLastSessionUserId();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool cs_checkFdCount(int limit, int dumpType, bool upload);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setOomLogPath([MarshalAs(UnmanagedType.LPStr)] string logPath);
#elif UNITY_STANDALONE_WIN || UNITY_XBOXONE
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_InitContext([MarshalAs(UnmanagedType.LPStr)] string id, [MarshalAs(UnmanagedType.LPStr)] string version, [MarshalAs(UnmanagedType.LPStr)] string key);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ReportExceptionW(int type,[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string message,[MarshalAs(UnmanagedType.LPStr)] string stack_trace,
                                   [MarshalAs(UnmanagedType.LPStr)] string extras, bool is_async, [MarshalAs(UnmanagedType.LPWStr)] string attachmentPath = "");
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetUserValue([MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetVehEnable(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetExtraHandler(bool extra_handle_enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetCustomLogDirW([MarshalAs(UnmanagedType.LPWStr)] string log_path);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetUserId([MarshalAs(UnmanagedType.LPStr)] string user_id);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_MonitorEnable(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_PrintLog(int level, [MarshalAs(UnmanagedType.LPStr)] string tag, [MarshalAs(UnmanagedType.LPStr)] string format, [MarshalAs(UnmanagedType.LPStr)] string arg);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_UploadGivenPathDump([MarshalAs(UnmanagedType.LPStr)] string dump_dir, bool is_extra_check);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ReportCrash();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ReportDump([MarshalAs(UnmanagedType.LPStr)] string dump_dir, bool is_async);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetEnvironmentName([MarshalAs(UnmanagedType.LPStr)] string name);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_InitWithAppId([MarshalAs(UnmanagedType.LPStr)] string app_id);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetAppVersion([MarshalAs(UnmanagedType.LPStr)] string app_version);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigCrashServerUrl([MarshalAs(UnmanagedType.LPStr)] string crash_server_url);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigDebugMode(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetDeviceId([MarshalAs(UnmanagedType.LPStr)] string device_id);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigCrashReporter(int log_level);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_TestNativeCrash();
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetDumpType(int dump_type);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_AddValidExpCode(ulong exp_code);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_UploadCrashWithGuid([MarshalAs(UnmanagedType.LPStr)] string guid);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetCrashUploadEnable(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetWorkSpaceW([MarshalAs(UnmanagedType.LPWStr)] string workspace);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetEngineInfo([MarshalAs(UnmanagedType.LPStr)] string version, [MarshalAs(UnmanagedType.LPStr)] string buildConfig, [MarshalAs(UnmanagedType.LPStr)] string language, [MarshalAs(UnmanagedType.LPStr)] string locale);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetCustomAttachDirW([MarshalAs(UnmanagedType.LPWStr)] string log_path);
#elif UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_InitWithAppId([MarshalAs(UnmanagedType.LPStr)] string appId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetAppVersion([MarshalAs(UnmanagedType.LPStr)] string appVersion);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ReportException(int type, [MarshalAs(UnmanagedType.LPStr)] string name,
                                [MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string stack_trace,
                                [MarshalAs(UnmanagedType.LPStr)] string extras, bool quit);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetUserId([MarshalAs(UnmanagedType.LPStr)] string userId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_EnableDebugMode(bool isDebug);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetErrorUploadInterval(int interval);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetCrashServerUrl([MarshalAs(UnmanagedType.LPStr)] string crashServerUrl);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetUserValue([MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetErrorUploadEnable(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_PrintLog(int level, [MarshalAs(UnmanagedType.LPStr)] string tag, [MarshalAs(UnmanagedType.LPStr)] string data);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetDeviceId([MarshalAs(UnmanagedType.LPStr)] string deviceId);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigCrashReporter(int logLevel);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_TestNativeCrash();
#if UNITY_PS4 || UNITY_PS5
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetLogPath([MarshalAs(UnmanagedType.LPStr)] string path);
#endif
#elif UNITY_STANDALONE_LINUX
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigCrashServerUrl([MarshalAs(UnmanagedType.LPStr)] string user_id);  //需要传入Uid
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_Init([MarshalAs(UnmanagedType.LPStr)] string app_id, [MarshalAs(UnmanagedType.LPStr)] string app_key,
                                [MarshalAs(UnmanagedType.LPStr)] string app_version);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetUserId([MarshalAs(UnmanagedType.LPStr)] string user_id);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetDeviceId([MarshalAs(UnmanagedType.LPStr)] string device_id);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetAppVersion([MarshalAs(UnmanagedType.LPStr)] string app_version);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_PrintLog(int level, [MarshalAs(UnmanagedType.LPStr)] string format, [MarshalAs(UnmanagedType.LPStr)] string arg);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetRecordFileDir([MarshalAs(UnmanagedType.LPStr)] string record_dir);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_AddSceneData([MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_SetLogPath([MarshalAs(UnmanagedType.LPStr)] string log_path);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ReportException(int type, [MarshalAs(UnmanagedType.LPStr)] string name,
                                [MarshalAs(UnmanagedType.LPStr)] string reason, [MarshalAs(UnmanagedType.LPStr)] string stackTrace,
                                [MarshalAs(UnmanagedType.LPStr)] string extras, bool quit, int dumpNativeType = 0);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_InitWithAppId([MarshalAs(UnmanagedType.LPStr)] string app_id);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigDebugMode(bool enable);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_ConfigCrashReporter(int log_level);
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CS_TestNativeCrash();
#endif
#endif

#if UNITY_OPENHARMONY
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_reportExceptionV1Adapter(int type, [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string stackTrace,
            [MarshalAs(UnmanagedType.LPStr)] string extras, bool quitProgram);
#endif
        /// <summary>
        /// Crash回调方法，提供上报用户数据能力
        /// </summary>
        public static event OnUQMStringRetEventHandler<int> CrashBaseRetEvent;

        public static event OnUQMStringRetSetLogPathEventHandler<int> CrashSetLogPathRetEvent;
        public static event OnUQMRetLogUploadEventHandler<int> CrashLogUploadRetEvent;

        private static AndroidJavaClass _gameAgentClass = null;
        private static bool _isLoadedSo = false;
        private static int _gameType = 0; // COCOS=1, UNITY=2, UNREAL=3
        private static readonly string GAME_AGENT_CLASS = "com.uqm.crashsight.core.api.CrashSightPlatform";

        public static AndroidJavaClass CrashSightPlatform
        {
            get
            {
                if (_gameAgentClass == null)
                {
                    _gameAgentClass = new AndroidJavaClass(GAME_AGENT_CLASS);
                }
                return _gameAgentClass;
            }
        }

        private static void LoadCrashSightCoreSo()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (_isLoadedSo)
            {
                return;
            }
            try
            {
                CrashSightPlatform.CallStatic<bool>("loadCrashSightCoreSo");
                _isLoadedSo = true;
            }
            catch (Exception ex)
            {
                UQMLog.LogError("loadSo with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
#endif
        }

        public static void ConfigCallbackType(Int32 callbackType)
        {
            try
            {
                UQMLog.Log("ConfigCallbackType  callbackType=" + callbackType);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            cs_configCallbackTypeAdapter(callbackType);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ConfigCallbackType with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ConfigGameType(int gameType)
        {
            try
            {
                UQMLog.Log("SetGameType gameType=" + gameType);
                _gameType = gameType;
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_configGameTypeAdapter(gameType);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetGameType with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ConfigAutoReportLogLevel(int level)
        {
            try
            {
                UQMLog.Log("ConfigAutoReportLogLevel  level=" + level);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_configAutoReportLogLevelAdapter(level);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ConfigCrashReporter(level);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_ConfigCrashReporter(level);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_ConfigCrashReporter(level);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ConfigAutoReportLogLevel with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ConfigCrashServerUrl(string serverUrl)
        {
            try
            {
                UQMLog.Log("ConfigCrashServerUrl  serverUrl=" + serverUrl);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS || UNITY_OPENHARMONY) && !UNITY_EDITOR
                cs_configCrashServerUrlAdapter(serverUrl);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ConfigCrashServerUrl(serverUrl);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_SetCrashServerUrl(serverUrl);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_ConfigCrashServerUrl(serverUrl);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ConfigCrashServerUrl with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ConfigDebugMode(bool enable)
        {
            try
            {
                if (enable)
                {
                    UQMLog.SetLevel(UQMLog.Level.Log);
                }
                LoadCrashSightCoreSo();
                UQMLog.Log("ConfigDebugMode  enable=" + enable);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_configDebugModeAdapter(enable);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ConfigDebugMode(enable);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_EnableDebugMode(enable);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_ConfigDebugMode(enable);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ConfigDebugMode with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ConfigDefault(string channel, string version, string user, long delay)
        {
            try
            {
                UQMLog.Log("ConfigDefault  channel=" + channel + " version=" + version + " user=" + user + " delay=" + delay);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_configDefaultAdapter(channel, version, user, delay);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ConfigDefault with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void InitWithAppId(string appId)
        {
            try
            {
                UQMLog.Log("InitWithAppId appId = " + appId);
                LoadCrashSightCoreSo();
                if (_gameType == 0) {
                    ConfigGameType(2);  // 默认Unity
                }
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#if UNITY_5_3_OR_NEWER
                    string Vendor = SystemInfo.graphicsDeviceVendor;
                    string Render = SystemInfo.graphicsDeviceName;
                    string Version = SystemInfo.graphicsDeviceVersion;
                    cs_setGpuInfo(Version, Vendor, Render);
#endif
                    cs_initWithAppIdAdapter(appId);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                    CS_InitWithAppId(appId);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                    CS_InitWithAppId(appId);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                    CS_InitWithAppId(appId);
#elif UNITY_OPENHARMONY && !UNITY_EDITOR
                    OpenHarmonyJSObject CrashSightObject = null;
                    CrashSightObject = new OpenHarmonyJSObject("CrashSightObj");
                    CrashSightObject.Call<int>("initContext");
                    cs_initWithAppIdAdapter(appId);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("InitWithAppId with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void InitContext(string userId, string version, string key)
        {
            try
            {
                UQMLog.Log("InitContext user_id = " + userId);
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_MonitorEnable(false);
                CS_InitContext(userId,version, key );
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("InitWithAppId with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }


        public static void LogRecord(int level, string message)
        {
            try
            {
                UQMLog.Log("LogRecord  level=" + level + " message=" + message);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_logRecordAdapter (level, message);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_PrintLog(level, "", "%s", message);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_PrintLog(level, "", message);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_PrintLog(level, "%s", message);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("LogRecord with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void AddSceneData(string k, string v)
        {
            try
            {
                UQMLog.Log("AddSceneData  key=" + k + " value=" + v);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS || UNITY_OPENHARMONY) && !UNITY_EDITOR
                cs_addSceneDataAdapter(k, v);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_SetUserValue(k, v);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_SetUserValue(k, v);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_AddSceneData(k, v);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("AddSceneData with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ReportException(int type, string name, string message, string stackTrace, string extras, bool quitProgram)
        {
            try
            {
                UQMLog.Log("ReportException  name=" + name + " quitProgram=" + quitProgram);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_reportExceptionAdapter (type, name, message, stackTrace, extras, null, quitProgram, 0, null);
#endif
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ReportExceptionW(type, name, message, stackTrace, extras, true, "");
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_ReportException(type, name, message, stackTrace, extras, quitProgram);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_ReportException(type, name, message, stackTrace, extras, quitProgram, 0);
#elif ( UNITY_OPENHARMONY) && !UNITY_EDITOR
                cs_reportExceptionV1Adapter (type, name, message, stackTrace, extras, quitProgram);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ReportException with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ReportException(int type, string exceptionName, string exceptionMsg, string exceptionStack, Dictionary<string, string> extInfo, int dumpNativeType = 0, string errorAttachmentPath = "")
        {
            try
            {
                UQMLog.Log(string.Format("ReportException exceptionName={0} exceptionMsg={1} dumpNativeType={2}", exceptionName, exceptionMsg, dumpNativeType));
                LoadCrashSightCoreSo();
                string paramsJson = MiniJSON.Json.Serialize(extInfo);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_reportExceptionAdapter (type, exceptionName, exceptionMsg, exceptionStack, null, paramsJson, false, dumpNativeType, errorAttachmentPath);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ReportExceptionW(type, exceptionName, exceptionMsg, exceptionStack, paramsJson, true, errorAttachmentPath);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_ReportException(type, exceptionName, exceptionMsg, exceptionStack, paramsJson, false);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_ReportException(type, exceptionName, exceptionMsg, exceptionStack, paramsJson, false, dumpNativeType);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ReportException with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetUserId(string userId)
        {
            try
            {
                UQMLog.Log("SetUserId userId = " + userId);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS || UNITY_OPENHARMONY) && !UNITY_EDITOR
                cs_setUserIdAdapter(userId);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_SetUserId(userId);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_SetUserId(userId);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_SetUserId(userId);

#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetUserId with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetScene(string sceneId, bool upload)
        {
            try
            {
                UQMLog.Log("SetScene sceneId = " + sceneId + ", upload = " + upload);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    cs_setSceneAdapter(sceneId, upload);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetScene with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ReRegistAllMonitors()
        {
            try
            {
                UQMLog.Log("ReRegistAllMonitors");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_reRegistAllMonitorsAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ReRegistAllMonitors with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void CloseAllMonitors()
        {
            try
            {
                UQMLog.Log("CloseAllMonitors");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_closeAllMonitorsAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("CloseAllMonitors with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ReportLogInfo(string msgType, string msg) {
            try
            {
                UQMLog.Log("ReportLogInfo");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_reportLogInfo(msgType, msg);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ReportLogInfo with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetAppVersion(string appVersion)
        {
            try
            {
                UQMLog.Log("SetAppVersion appVersion = " + appVersion);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS || UNITY_OPENHARMONY) && !UNITY_EDITOR
                    cs_setAppVersionAdapter(appVersion);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                    CS_SetAppVersion(appVersion);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                    CS_SetAppVersion(appVersion);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetAppVersion with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetDeviceId(string deviceId)
        {
            try
            {
                UQMLog.Log("SetDeviceId deviceId = " + deviceId);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_setDeviceIdAdapter(deviceId);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_SetDeviceId(deviceId);
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_SetDeviceId(deviceId);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_SetDeviceId(deviceId);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetDeviceId with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetCustomizedDeviceID(string deviceId)
        {
            try
            {
                UQMLog.Log("SetCustomizedDeviceID deviceId = " + deviceId);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    cs_setCustomizedDeviceIDAdapter(deviceId);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetCustomizedDeviceID with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static string GetSDKDefinedDeviceID()
        {
            try
            {
                UQMLog.Log("GetSDKDefinedDeviceID");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                IntPtr tranResult = cs_getSDKDefinedDeviceIDAdapter();
                return Marshal.PtrToStringAnsi(tranResult);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("GetSDKDefinedDeviceID with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
            return "";
        }


        public static void SetCustomizedMatchID(string matchId)
        {
            try
            {
                UQMLog.Log("SetCustomizedMatchID matchId = " + matchId);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    cs_setCustomizedMatchIDAdapter(matchId);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetCustomizedMatchID with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static string GetSDKSessionID()
        {
            try
            {
                UQMLog.Log("GetSDKSessionID");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                IntPtr tranResult = cs_getSDKSessionIDAdapter();
                return Marshal.PtrToStringAnsi(tranResult);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("GetSDKSessionID with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
            return "";
        }

        public static string GetCrashUuid()
        {
            try
            {
                UQMLog.Log("GetCrashUuid");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                IntPtr tranResult = cs_getCrashUuidAdapter();
                return Marshal.PtrToStringAnsi(tranResult);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("GetCrashUuid with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
            return "";
        }

        public static void SetDeviceModel(string deviceModel)
        {
            try
            {
                UQMLog.Log("SetDeviceModel deviceModel = " + deviceModel);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    cs_setDeviceModelAdapter(deviceModel);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetDeviceModel with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetLogPath(string logPath)
        {
            try
            {
                UQMLog.Log("SetLogPath logPath = " + logPath);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_setLogPathAdapter(logPath);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_SetCustomLogDirW(logPath);
#elif (UNITY_PS4 || UNITY_PS5) && !UNITY_EDITOR
                CS_SetLogPath(logPath);
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_SetLogPath(logPath);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetLogPath with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetCrashCallback()
        {
            try
            {
                UQMLog.Log("SetCrashCallback");
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_unityCrashCallback();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetCrashCallback with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }

        }

        public static void UnsetCrashCallback()
        {
            try
            {
                UQMLog.Log("UnsetCrashCallback");
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_unregisterUnityCrashCallback();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("UnsetCrashCallback with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }

        }

        public static void SetCrashLogCallback()
        {
            try
            {
                UQMLog.Log("SetCrashLogCallback");
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_unityCrashLogCallback();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetCrashLogCallback with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }

        }

        //callback
        internal static string OnCrashCallbackMessage(int methodId, int crashType)
        {
            UQMLog.Log("OnCrashCallbackMessage  methodId= " + methodId + " crashType=" + crashType);
            if (CrashBaseRetEvent != null)
            {
                try
                {
                    return CrashBaseRetEvent(methodId, crashType);
                }
                catch (Exception e)
                {
                    UQMLog.LogError(e.StackTrace);
                }
            }
            else
            {
                UQMLog.LogError("No callback for OnCrashCallbackMessage !");
            }
            return "";
        }

        internal static string OnCrashCallbackData(int methodId, int crashType)
        {
            UQMLog.Log("OnCrashCallbackData  methodId= " + methodId + " crashType=" + crashType);
            if (CrashBaseRetEvent != null)
            {
                try
                {
                    return CrashBaseRetEvent(methodId, crashType);
                }
                catch (Exception e)
                {
                    UQMLog.LogError(e.StackTrace);
                }
            }
            else
            {
                UQMLog.LogError("No callback for OnCrashCallbackData !");
            }
            return "";
        }

        internal static string OnCrashSetLogPathMessage(int methodId, int crashType)
        {
            UQMLog.Log("OnCrashSetLogPathMessage  methodId= " + methodId + " crashType=" + crashType);
            if (CrashSetLogPathRetEvent != null)
            {
                try
                {
                    return CrashSetLogPathRetEvent(methodId, crashType);
                }
                catch (Exception e)
                {
                    UQMLog.LogError(e.StackTrace);
                }
            }
            else
            {
                UQMLog.LogError("No callback for OnCrashSetLogPathMessage !");
            }
            return "";
        }

        internal static string OnCrashLogUploadMessage(int methodId, int crashType, int result)
        {
            UQMLog.Log("OnCrashLogUploadMessage  methodId= " + methodId + " crashType=" + crashType);
            if (CrashLogUploadRetEvent != null)
            {
                try
                {
                    CrashLogUploadRetEvent(methodId, crashType, result);
                }
                catch (Exception e)
                {
                    UQMLog.LogError(e.StackTrace);
                }
            }
            else
            {
                UQMLog.LogError("No callback for OnCrashLogUploadMessage !");
            }
            return "";
        }

        internal static string OnCrashCallbackNoRet(int methodId, int crashType)
        {
            UQMLog.Log("OnCrashCallbackNoRet  methodId= " + methodId + " crashType=" + crashType);
            if (CrashBaseRetEvent != null)
            {
                try
                {
                    return CrashBaseRetEvent(methodId, crashType);
                }
                catch (Exception e)
                {
                    UQMLog.LogError(e.StackTrace);
                }
            }
            else
            {
                UQMLog.LogError("No callback for OnCrashCallbackNoRet !");
            }
            return "";
        }

        public static void ConfigCallBack()
        {
            SetCrashCallback();
            UQMMessageCenter.Instance.Init();
        }

        public static void UnregisterCallBack()
        {
            UnsetCrashCallback();
            UQMMessageCenter.Instance.Uninit();
        }

        public static void ConfigLogCallBack()
        {
            SetCrashLogCallback();
            UQMMessageCenter.Instance.Init();
        }

        // Test cases
        public static void TestOomCrash()
        {
            try
            {
                UQMLog.Log("TestOomCrash");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_testOomCrashAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("TestOomCrash with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void TestJavaCrash()
        {
            try
            {
                UQMLog.Log("TestJavaCrash");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_testJavaCrashAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("TestJavaCrash with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void TestOcCrash()
        {
            try
            {
                UQMLog.Log("TestOcCrash");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_testOcCrashAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("TestOcCrash with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void TestNativeCrash()
        {
            try
            {
                UQMLog.Log("TestNativeCrash");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_testNativeCrashAdapter();
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_TestNativeCrash();
#elif (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_TestNativeCrash();
#elif UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_TestNativeCrash();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("TestNativeCrash with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void TestANR()
        {
            try
            {
                UQMLog.Log("TestANR");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_testANRAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("TestANR with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }


        public static long GetCrashThreadId()
        {
            long thread_id = -1;
            try
            {
                UQMLog.Log("GetCrashThreadId");
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                thread_id = cs_getCrashThreadId();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("GetCrashThreadId with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
            return thread_id;
        }

        public static void SetLogcatBufferSize(int size)
        {
            try
            {
                UQMLog.Log("SetLogcatBufferSize:" + size);
#if UNITY_ANDROID && !UNITY_EDITOR
                cs_setLogcatBufferSize(size);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetLogcatBufferSize with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetCallbackMsg(string data)
        {
             try
             {
                  UQMLog.Log("SetCallBackMsg SetCallBackMsg = " + data);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  cs_setCallbackMsgAdapter(data);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("SetDeviceModel with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
        }

        public static void StartDumpRoutine(int dumpMode, int startTimeMode, long startTime,
            long dumpInterval, int dumpTimes, bool saveLocal, string savePath)
        {
             try
             {
                  UQMLog.Log("StartDumpRoutine dumpMode = " + dumpMode
                      + ", startTimeMode = " + startTimeMode
                      + ", startTime = " + startTime
                      + ", dumpInterval = " + dumpInterval
                      + ", dumpTimes = " + dumpTimes
                      + ", saveLocal = " + saveLocal
                      + ", savePath = " + savePath);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  cs_startDumpRoutine(dumpMode, startTimeMode, startTime, dumpInterval, dumpTimes, saveLocal, savePath);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("StartDumpRoutine with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
        }

        public static void StartMonitorFdCount(int interval, int limit, int dumpType)
        {
             try
             {
                  UQMLog.Log("StartMonitorFdCount interval = " + interval
                      + ", limit = " + limit + ", dumpType = " + dumpType);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  cs_startMonitorFdCount(interval, limit, dumpType);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("StartMonitorFdCount with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
        }

        public static int getExceptionType(string name)
        {
             int type = 0;
             try
             {
                  UQMLog.Log("getExceptionType name = " + name);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  type = cs_getExceptionType(name);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("getExceptionType with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
             return type;
        }
        
        public static void TestUseAfterFree()
        {
            try
            {
                UQMLog.Log("TestUseAfterFree");
                LoadCrashSightCoreSo();
#if UNITY_ANDROID && !UNITY_EDITOR
                cs_testUseAfterFreeAdapter();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("TestUseAfterFree with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetServerEnv(string serverEnv)
        {
             try
             {
                  UQMLog.Log("SetServerEnv serverEnv = " + serverEnv);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  cs_setServerEnv(serverEnv);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                  CS_SetEnvironmentName(serverEnv);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("SetServerEnv with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
        }

        public static void SetVehEnable(bool enable)
        {
            try
            {
                UQMLog.Log("SetVehEnable:" + enable);
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_SetVehEnable(enable);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetVehEnable with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ReportCrash()
        {
            try
            {
                UQMLog.Log("ReportCrash");
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ReportCrash();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ReportCrash with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void ReportDump(string dump_path, bool is_async)
        {
            try
            {
                UQMLog.Log("ReportDump:" + dump_path + ", is_async:" + is_async);
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_ReportDump(dump_path, is_async);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("ReportDump with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetExtraHandler(bool extra_handle_enable)
        {
            try
            {
                UQMLog.Log("SetExtraHandler:" + extra_handle_enable);
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_SetExtraHandler(extra_handle_enable);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetExtraHandler with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void UploadGivenPathDump(string dump_dir, bool is_extra_check)
        {
            try
            {
                UQMLog.Log("UploadGivenPathDump:" + dump_dir + ", is_extra_check:" + is_extra_check);
#if (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                CS_UploadGivenPathDump(dump_dir, is_extra_check);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("UploadGivenPathDump with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetErrorUploadInterval(int interval)
        {
            try
            {
                UQMLog.Log("SetErrorUploadInterval:" + interval);
#if (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_SetErrorUploadInterval(interval);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetErrorUploadInterval with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetErrorUploadEnable(bool enable)
        {
            try
            {
                UQMLog.Log("SetErrorUploadEnable:" + enable);
#if (UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH) && !UNITY_EDITOR
                CS_SetErrorUploadEnable(enable);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetErrorUploadEnable with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetRecordFileDir(string record_dir)
        {
            try
            {
                UQMLog.Log("SetRecordFileDir:" + record_dir);
#if UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_SetRecordFileDir(record_dir);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetRecordFileDir with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void Init(string app_id, string app_key, string app_version)
        {
            try
            {
                UQMLog.Log("Init:" + app_id + ", app_key:" + app_key +", app_version:" + app_version);
#if UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                CS_Init(app_id, app_key, app_version);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("Init with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        
        public static void setEnableGetPackageInfo(bool enable)
        {
            try
            {
                LoadCrashSightCoreSo();
                UQMLog.Log("setEnableGetPackageInfo  enable=" + enable);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_setEnableGetPackageInfo(enable);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("setEnableGetPackageInfo with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetDumpType(int dump_type)
        {
            try
            {
                UQMLog.Log("SetDumpType  dump_type=" + dump_type);
#if (UNITY_STANDALONE_WIN) && !UNITY_EDITOR
                CS_SetDumpType(dump_type);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetDumpType with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void AddValidExpCode(ulong exp_code)
        {
            try
            {
                UQMLog.Log("AddValidExpCode  exp_code=" + exp_code);
#if (UNITY_STANDALONE_WIN) && !UNITY_EDITOR
                CS_AddValidExpCode(exp_code);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("AddValidExpCode with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void UploadCrashWithGuid(string guid)
        {
            try
            {
                UQMLog.Log("UploadCrashWithGuid  guid=" + guid);
#if (UNITY_STANDALONE_WIN) && !UNITY_EDITOR
                CS_UploadCrashWithGuid(guid);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("UploadCrashWithGuid with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetCrashUploadEnable(bool enable)
        {
            try
            {
                UQMLog.Log("SetCrashUploadEnable  enable=" + enable);
#if (UNITY_STANDALONE_WIN) && !UNITY_EDITOR
                CS_SetCrashUploadEnable(enable);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetCrashUploadEnable with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetWorkSpace(string workspace)
        {
            try
            {
                UQMLog.Log("SetWorkSpace  workspace=" + workspace);
#if (UNITY_STANDALONE_WIN) && !UNITY_EDITOR
                CS_SetWorkSpaceW(workspace);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetWorkSpace with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void SetEngineInfo(string version, string buildConfig, string language, string locale)
        {
             try
             {
                  UQMLog.Log("SetEngineInfo version = " + version + ", buildConfig = " + buildConfig + ", language = " + language + ", locale = " + locale);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  cs_setEngineInfo(version, buildConfig, language, locale);
#elif (UNITY_STANDALONE_WIN || UNITY_XBOXONE) && !UNITY_EDITOR
                  CS_SetEngineInfo(version, buildConfig, language, locale);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("SetEngineInfo with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
        }

        public static void SetCustomAttachDir(string path)
        {
#if (UNITY_STANDALONE_WIN) && !UNITY_EDITOR
            CS_SetCustomAttachDirW(path);
#endif
        }


        public static bool IsLastSessionCrash()
        {
            bool isCrash = false;
            try
            {
                UQMLog.Log("IsLastSessionCrash");
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                isCrash = cs_isLastSessionCrash();
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("IsLastSessionCrash with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
            return isCrash;
        }

        public static string GetLastSessionUserId()
        {
            try
            {
                UQMLog.Log("GetLastSessionUserId");
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                IntPtr tranResult = cs_getLastSessionUserId();
                return Marshal.PtrToStringAnsi(tranResult);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("GetLastSessionUserId with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
            return "";
        }

        public static bool CheckFdCount(int limit, int dumpType, bool upload)
        {
             try
             {
                  UQMLog.Log("CheckFdCount limit = " + limit + ", dumpType = " + dumpType + ", upload = " + upload);
                  LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                  return cs_checkFdCount(limit, dumpType, upload);
#endif
             }
             catch (Exception ex)
             {
                  UQMLog.LogError("CheckFdCount with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
             }
             return false;
        }

        public static void SetOomLogPath(string logPath)
        {
            try
            {
                UQMLog.Log("SetOomLogPath logPath = " + logPath);
                LoadCrashSightCoreSo();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                cs_setOomLogPath(logPath);
#endif
            }
            catch (Exception ex)
            {
                UQMLog.LogError("SetOomLogPath with unknown error = \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}