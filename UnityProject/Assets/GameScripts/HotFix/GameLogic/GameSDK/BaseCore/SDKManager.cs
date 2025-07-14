using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BestHTTP.JSON;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using GameBase;
using NiceJson;
using SDK.AppleIdLogin;
#if UNITY_ANDROID

#endif
using SDK.Purchase;
using TEngine;
using UnityEngine;
using Utils.JsonUtils;
using UW;

namespace SDK.BaseCore
{
    public enum PlatformID
    {
        UnKnown = 0,
        Google = 1,
        AppStore = 2,
    }

    // Flags标签用于位枚举，可以用组合和拆分来check枚举值
    [Flags]
    public enum LogEventChannel
    {
        None = 0,
        Facebook = 1,
        Firebase = 2,
        AppsFlyer = 4,
    }

    // SDK管理器
    public class SDKManager : Singleton<SDKManager>
    {
        private SDKBase _sdkBase;

        public void InitSDKInstance()
        {
            if (PlatformUtils.IsEditor())
            {
                _sdkBase = new SDKBase();
            }
            else if (PlatformUtils.IsAndroidPlatform())
            {
                _sdkBase = new SDKForAndroid();
            }
            else if (PlatformUtils.IsIOSPlatform())
            {
                _sdkBase = new SDKForIOS();
            }
            else
            {
                _sdkBase = new SDKBase();
            }

            _sdkBase.AddListener();
            _sdkBase.Init();
        }

        public void SetGameUid(string id)
        {
            _sdkBase.SetGameUid(id);
        }

        public void OnSDKInit(string result)
        {
            var jsonDic = JsonHelper.FromJson(result);
            var platformId = (string) jsonDic["platform"];
            _sdkBase.SetPlatformId(platformId);
        }

        public PlatformID GetPlatform()
        {
            if (PlatformUtils.IsEditor())
                return PlatformID.Google;
            return (PlatformID) _sdkBase.GetPlatformId();
        }

        // 获取大版本号
        public string GetVersionName()
        {
            return Application.version;
        }
        
        // 获取小版本号
        public string GetVersionCode()
        {
            return "1411";
// #if UNITY_EDITOR
//             if (PlatformUtils.IsAndroidPlatform())
//                 return StringUtils.IntToString(UnityEditor.PlayerSettings.Android.bundleVersionCode);
//             if (PlatformUtils.IsIOSPlatform())
//                 return UnityEditor.PlayerSettings.iOS.buildNumber;
//             return "1";
// #endif
//             var versionCode = GetDataFromNative("PM_getVersionCode", "");
//             if (string.IsNullOrEmpty(versionCode))
//                 versionCode = "1";
//             return versionCode;
        }
        
        #region Unity call Native

        public void SendDataToNative(string funcName, string data = "")
        {
            _sdkBase.SendDataToNative(funcName, data);
        }

        public string GetDataFromNative(string funcName, string data = "")
        {
            return _sdkBase.GetDataFromNative(funcName, data);
        }

        #endregion

        #region 支付

        // 是否再调用原生sdk ,如果调用了会触发前后台切换，需要设为true,防止重登
        private bool _isCallingNativeSDK;

        // 记一个开始支付的标志位
        private void CallNativeSDKStart()
        {
            if (_isCallingNativeSDK)
            {
                return;
            }

            _isCallingNativeSDK = true;
        }

        public void CallNativeSDKOver()
        {
            if (!_isCallingNativeSDK)
            {
                return;
            }

            _isCallingNativeSDK = false;
        }

        public void InitPurchase()
        {
            _sdkBase.InitPurchase();
        }

        public void SDKPay(string data)
        {
            CallNativeSDKStart();
            _sdkBase.Pay(data);
        }

        public string GetPurchaseCurrencyCode(string productId)
        {
            return _sdkBase.GetPurchaseCurrencyCode(productId);
        }

        public string GetPurchaseLocalPrice(string productId)
        {
            return _sdkBase.GetPurchaseLocalPrice(productId);
        }

        public void OnSDKPurchase(string result)
        {
            CallNativeSDKOver();
        }

        public void OnSetGAID(string gaid)
        {
            if (PlatformUtils.IsAndroidPlatform() && _sdkBase is SDKForAndroid)
            {
                var sdk = _sdkBase as SDKForAndroid;
                if (sdk != null) sdk.SetGAID(gaid);
                
                LuaModule.CallLuaFunc("CSCallLuaUtils", "OnSetGAID", gaid);
            }
        }

        public string GetGAID()
        {
            if (PlatformUtils.IsEditor())
            {
                return "";
            }
            
            if (PlatformUtils.IsAndroidPlatform() && _sdkBase is SDKForAndroid)
            {
                var sdk = _sdkBase as SDKForAndroid;
                if (sdk != null)
                {
                    return sdk.GetGAID();
                }
            }

            return "";
        }

        public string GetIDFA()
        {
            if (PlatformUtils.IsEditor())
            {
                return "";
            }
            
            if (PlatformUtils.IsIOSPlatform())
            {
                return GetDataFromNative("PM_getIDFA");
            }

            return "";
        }
        
        public void ConsumeCallback(string orderId, int state)
        {
            _sdkBase.ConsumeCallback(orderId, state);
        }

        public void QueryPurchaseOrder()
        {
            _sdkBase.QueryPurchaseOrder();
        }

        #endregion

        #region FireBase

        private bool _isFirebaseInit;

        // 初始化Firebase，主要是check依赖关系
        public async void InitFirebase()
        {
        }
        

        // Firebase打点
        private void LogFirebaseEvent(string eventName)
        {
            if (!_isFirebaseInit)
            {
                return;
            }
           
        }

        private void LogFirebaseEvent(string eventName, string parameterName, string parameterValue)
        {
            if (!_isFirebaseInit)
            {
                return;
            }

        }

        // Firebase带参数打点
        private void LogFirebaseEvent(string eventName, SkeinEngine.Parameter[] parameters)
        {
            if (!_isFirebaseInit)
            {
                return;
            }

        }

        // Firebase支付打点
        private void LogFirebasePurchase(string cost)
        {
        }

        // 传玩家的uid
        public void SetFirebaseUid(string uid)
        {
        }

        public async Task<string> GetFirebaseId(Action<string> onGetFirebaseId)
        {
            if (!_isFirebaseInit)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        #endregion

        #region AppsFlyer

        // 传玩家的uid
        public void SetAppsFlyerCustomerUserId(string customerId)
        {
        }

        public string GetAppsFlyerId()
        {
            return "";
        }

        private void LogAppsFlyerEvent(string eventName, Dictionary<string, string> dictData)
        {
        }

        private void LogAppsFlyerEvent(string eventName)
        {
            var dictData = new Dictionary<string, string>();
            LogAppsFlyerEvent(eventName, dictData);
        }

        // AppsFlyer支付打点
        private void LogAppsFlyerPurchase(string cost, string itemId)
        {

        }

        #endregion

        #region Facebook

        private void LogFacebookEvent(string eventName, float? value = null)
        {

        }

        // Facebook支付打点
        private void LogFacebookPurchase(string cost, string itemId = "")
        {

        }

        #endregion

        #region 渠道打点

        // 无需打点的情况
        private bool DontNeedLogEvent()
        {
            if (PlatformUtils.IsEditor())
            {
                return true;
            }
            
            return false;
        }

        public void LogEvent(string eventName,
            LogEventChannel channel = LogEventChannel.AppsFlyer | LogEventChannel.Facebook | LogEventChannel.Firebase)
        {
            if (DontNeedLogEvent())
            {
                return;
            }
            
            if (channel.HasFlag(LogEventChannel.Facebook))
            {
                LogFacebookEvent(eventName);
            }

            if (channel.HasFlag(LogEventChannel.AppsFlyer))
            {
                LogAppsFlyerEvent(eventName);
            }

            if (channel.HasFlag(LogEventChannel.Firebase))
            {
                LogFirebaseEvent(eventName);
            }
        }

        // 支付打点
        public void LogPurchase(string cost, string itemId,
            LogEventChannel channel = LogEventChannel.AppsFlyer | LogEventChannel.Facebook | LogEventChannel.Firebase)
        {
            if (DontNeedLogEvent())
            {
                return;
            }
            
            if (channel.HasFlag(LogEventChannel.Facebook))
            {
                LogFacebookPurchase(cost, itemId);
            }

            if (channel.HasFlag(LogEventChannel.AppsFlyer))
            {
                LogAppsFlyerPurchase(cost, itemId);
            }

            if (channel.HasFlag(LogEventChannel.Firebase))
            {
                LogFirebasePurchase(cost);
            }
        }

        // 自定义需求打点，看发行需求要不要调用
        public void LogCustomPurchase(string cost,
            LogEventChannel channel = LogEventChannel.AppsFlyer | LogEventChannel.Facebook | LogEventChannel.Firebase)
        {
            if (DontNeedLogEvent())
            {
                return;
            }
            
            const string eventName = "purchase_success";
            const string paramName = "revenue";

            if (channel.HasFlag(LogEventChannel.Facebook))
            {
                LogFacebookEvent(eventName, cost.ToFloat());
            }

            if (channel.HasFlag(LogEventChannel.AppsFlyer))
            {
                LogAppsFlyerEvent(eventName, new Dictionary<string, string> {{paramName, cost}});
            }

            if (channel.HasFlag(LogEventChannel.Firebase))
            {
                LogFirebaseEvent(eventName, paramName, cost);
            }
        }

        #endregion

        #region 账号登录

        public void LoginGoogleEmail(Action<string> onLoginSuccess = null)
        {
        }

        public void LoginGooglePlay(Action<string> onLoginSuccess = null)
        {
        }

        public void LoginAppleId(Action<bool, string> callback = null)
        {
            AppleIdLoginMono.Instance.SignInWithApple(callback);
        }

        #endregion

        #region Other

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        // TimerManager.Instance.AddOneShotTask(0.5f, () =>
        // {
        //     GetDataFromNative("PM_QuitGame", "");
        // });
#else
        Application.Quit();
#endif
        }

        //打开相机、相册  photoType: 0 相机  1 相册
        public void OnOpenCameraPhoto(long uuid ,int photoType,int uploadPhotoType)
        {
            try
            {
                CallNativeSDKStart();
                var jsonObject = new JsonObject();
                jsonObject.Add("uid",uuid.ToString());
                jsonObject.Add("code",photoType);
                jsonObject.Add("funType",uploadPhotoType);

                string jsonData = jsonObject.ToJsonString();
                SendDataToNative("PM_OnUploadPhoto", jsonData);
            }
            catch (Exception e)
            {
                Log.Warning(e);
            }
        }

        public void BeginUploadImgToServer(string filePath)
        {
            Debug.LogError("c# BeginUploadImgToServer  filepath =  "+filePath);
            LuaModule.CallLuaFunc("ShipBuildingCSCallLua", "BeginUploadImgToServer",filePath);
        }

        public void FailedUploadImgToServer()
        {
            LuaModule.CallLuaFunc("ShipBuildingCSCallLua", "FailedUploadImgToServer");
        }
        
        #endregion

        #region 推送

        public void CancelAllNotifications() 
        {
            _sdkBase?.CancelAllNotifications();
        }

        public void SendNotification(int id, string title, string content, long time)
        {
            _sdkBase?.SendNotification(id,title,content,time);
        }

        public void GetFCMToken(Action<string> action)
        {
            _sdkBase?.GetFCMToken(action);
        }

        #endregion
    }
}