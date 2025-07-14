using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SDK.BaseCore
{
    public static class SDKLuaHelper
    {
        public static string GetVersionCode()
        {
            return SDKManager.Instance.GetVersionCode();
        }

        public static string GetVersionName()
        {
            return SDKManager.Instance.GetVersionName();
        }

        public static void SetGameUid(string id)
        {
            SDKManager.Instance.SetGameUid(id);
        }

        public static void SetAppsFlyerCustomerUserId(string id)
        {
            SDKManager.Instance.SetAppsFlyerCustomerUserId(id);
        }

        public static void SetFirebaseUid(string uid)
        {
            SDKManager.Instance.SetFirebaseUid(uid);
        }

        public static string GetAppsFlyerId()
        {
            return SDKManager.Instance.GetAppsFlyerId();
        }

        public static string GetGAID()
        {
            return SDKManager.Instance.GetGAID();
        }

        public static string GetIDFA()
        {
            return SDKManager.Instance.GetIDFA();
        }
        
        public static async Task<string> GetFirebaseId(Action<string> onGetFireBaseId)
        {
            return await SDKManager.Instance.GetFirebaseId(onGetFireBaseId);
        }

        public static void LogEvent(string eventName,
            LogEventChannel channel = LogEventChannel.AppsFlyer | LogEventChannel.Facebook | LogEventChannel.Firebase)
        {
            SDKManager.Instance.LogEvent(eventName, channel);
        }

        public static void LogCustomPurchase(string cost,
            LogEventChannel channel = LogEventChannel.AppsFlyer | LogEventChannel.Facebook | LogEventChannel.Firebase)
        {
            SDKManager.Instance.LogCustomPurchase(cost, channel);
        }

        public static void LogPurchase(string cost, string itemId,
            LogEventChannel channel = LogEventChannel.AppsFlyer | LogEventChannel.Facebook | LogEventChannel.Firebase)
        {
            SDKManager.Instance.LogPurchase(cost, itemId, channel);
        }

        public static void LoginGoogleEmail(Action<string> onLoginSuccess)
        {
            SDKManager.Instance.LoginGoogleEmail(onLoginSuccess);
        }

        public static void LoginGooglePlay(Action<string> onLoginSuccess)
        {
            SDKManager.Instance.LoginGooglePlay(onLoginSuccess);
        }
        
        public static void LoginAppleId(Action<bool, string> callback)
        {
            SDKManager.Instance.LoginAppleId(callback);
        }

        public static string GetDataFromNative(string funcName, string data = "")
        {
            return SDKManager.Instance.GetDataFromNative(funcName, data);
        }

        public static void SendDataToNative(string funcName, string data = "")
        {
            SDKManager.Instance.SendDataToNative(funcName, data);
        }

        public static void QuitGame()
        {
            SDKManager.Instance.QuitGame();
        }
        public static void OnOpenCameraPhoto(long uuid ,int photoType,int uploadPhotoType)
        {
            SDKManager.Instance.OnOpenCameraPhoto(uuid, photoType, uploadPhotoType);
        }
        public static void CancelAllNotifications() 
        {
            // Debug.LogError("SDKLuaHelper      CancelAllNotifications");
            SDKManager.Instance.CancelAllNotifications();
        }

        public static void SendNotification(int id, string title, string content, long time)
        {
            // Debug.LogError($"SDKLuaHelper      SendNotification     id = {id}    title = {title}    content = {content}  time = {time}");
            SDKManager.Instance.SendNotification(id,title,content,time);
        }
        
        public static void GetFCMToken(Action<string> action)
        {
            // Debug.LogError("SDKLuaHelper        SDKLuaHelper");
#if UNITY_EDITOR
            action?.Invoke("");
#else
            SDKManager.Instance.GetFCMToken(action);
#endif
        }
    }
}