using AOT;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;


namespace GCloud.UQM
{
    public class RetArgsWrapper
    {
        private readonly int methodId;
        private readonly int crashType;
        private readonly int logUploadResult;

        public int MethodId
        {
            get { return methodId; }
        }

        public int CrashType
        {
            get { return crashType; }
        }

        public int LogUploadResult
        {
            get { return logUploadResult; }
        }

        public RetArgsWrapper(int _methodId, int _crashType, int _logUploadResult)
        {
            methodId = _methodId;
            crashType = _crashType;
            logUploadResult = _logUploadResult;
        }
    }

    #region UQMMessageCenter

    public class UQMMessageCenter : MonoBehaviour
    {
        #region json ret and callback

        private static bool initialzed = false;

        private delegate string UQMRetJsonEventHandler(int methodId, int callType, int logUploadResult);

        [MonoPInvokeCallback(typeof(UQMRetJsonEventHandler))]
        public static string OnUQMRet(int methodId, int crashType, int logUploadResult)
        {
            var argsWrapper = new RetArgsWrapper(methodId, crashType, logUploadResult);
            UQMLog.Log("OnUQMRet, the methodId is ( " + methodId + " )  crashType=" + crashType);
            if (methodId == (int)UQMMethodNameID.UQM_CRASH_CALLBACK_EXTRA_MESSAGE
                || methodId == (int)UQMMethodNameID.UQM_CRASH_CALLBACK_EXTRA_DATA
                || methodId == (int)UQMMethodNameID.UQM_CRASH_CALLBACK_SET_LOG_PATH
                || methodId == (int)UQMMethodNameID.UQM_CRASH_CALLBACK_LOG_UPLOAD_RESULT
                )
            {
                lock (CrashSightAgent.callbackThreadsLock) {
                    CrashSightAgent.callbackThreads.Add(Thread.CurrentThread.ManagedThreadId);
                }
                string result = SynchronousDelegate(argsWrapper);
                lock (CrashSightAgent.callbackThreadsLock) {
                    CrashSightAgent.callbackThreads.Remove(Thread.CurrentThread.ManagedThreadId);
                }
                return result;
            }
            return "";
        }

#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN || UNITY_OPENHARMONY) && !UNITY_EDITOR
        [DllImport(UQM.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void cs_setUnityCallback(UQMRetJsonEventHandler eventHandler);
#endif
        #endregion

        static UQMMessageCenter instance;

        public static UQMMessageCenter Instance
        {
            get
            {
                if (instance != null) return instance;
                var bridgeGameObject = new GameObject { name = "UQMMessageCenter" };
                DontDestroyOnLoad(bridgeGameObject);
                instance = bridgeGameObject.AddComponent(typeof(UQMMessageCenter)) as UQMMessageCenter;
                UQMLog.Log("UQMMessageCenter  instance=" + instance);
                return instance;
            }
        }

        public void Init()
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN || UNITY_OPENHARMONY) && !UNITY_EDITOR
           if (initialzed) {
                return;
            }
            cs_setUnityCallback(OnUQMRet);
            initialzed = true;
#endif
            UQMLog.Log("UQM Init, set unity callback");
        }

        public void Uninit()
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN || UNITY_OPENHARMONY) && !UNITY_EDITOR
            cs_setUnityCallback(null);
#endif
            UQMLog.Log("UQM Uninit, set unity callback to null");
        }

        static string SynchronousDelegate(object arg)
        {
            var argsWrapper = (RetArgsWrapper)arg;
            var methodId = argsWrapper.MethodId;
            var crashType = argsWrapper.CrashType;

            UQMLog.Log("the methodId is ( " + methodId + " ) and crashType=" + crashType);
            switch (methodId)
            {
#if UNITY_WSA
#else
                case (int)UQMMethodNameID.UQM_CRASH_CALLBACK_EXTRA_MESSAGE:
                    return UQMCrash.OnCrashCallbackMessage(methodId, crashType);

                case (int)UQMMethodNameID.UQM_CRASH_CALLBACK_EXTRA_DATA:
                    return UQMCrash.OnCrashCallbackData(methodId, crashType);

                case (int)UQMMethodNameID.UQM_CRASH_CALLBACK_SET_LOG_PATH:
                    return UQMCrash.OnCrashSetLogPathMessage(methodId, crashType);

                case (int)UQMMethodNameID.UQM_CRASH_CALLBACK_LOG_UPLOAD_RESULT:
                    return UQMCrash.OnCrashLogUploadMessage(methodId, crashType, argsWrapper.LogUploadResult);
#endif
            }
            return "";
        }
#endregion
    }
}