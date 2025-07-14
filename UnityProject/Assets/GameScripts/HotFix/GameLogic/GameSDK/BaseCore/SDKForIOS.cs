
#if UNITY_IOS
using System.Runtime.InteropServices;
using Unity.Notifications.iOS;
using Utils.JsonUtils;
using System;
using Unity.Notifications.iOS;
using UnityEngine;

#endif

namespace SDK.BaseCore
{
    public class SDKForIOS : SDKBase
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void SDKInit();

        [DllImport("__Internal")]
        private static extern void SDKSendDataToNative(string funcName, string data);

        [DllImport("__Internal")]
        private static extern string SDKGetDataFromNative(string funcName, string data);

        private IosSDKListener _listener;

        public override void AddListener()
        {
            _listener = new IosSDKListener();
        }

        public override void Init()
        {
            SDKInit();
            iOSNotificationCenter.OnRemoteNotificationReceived += notification => { };
        }

        public override void SendDataToNative(string funcName, string data)
        {
            SDKSendDataToNative(funcName, data);
        }

        public override string GetDataFromNative(string funcName, string data)
        {
            return SDKGetDataFromNative(funcName, data);
        }


        #region SysInfo

        [DllImport("__Internal")]
        private static extern string _IOS_GetMemoryInfo();

        #endregion


        #region 支付

        [DllImport("__Internal")]
        private static extern void unity_iap_init();

        [DllImport("__Internal")]
        private static extern void unity_iap_check();

        [DllImport("__Internal")]
        private static extern void unity_iap_pay(string productIdentifier, int count, string serverOrderId);

        [DllImport("__Internal")]
        private static extern void unity_iap_finish_transaction(string transactionIdentifier);

        public override void InitPurchase()
        {
            if (!CheckIsInitSDK()) return;
            unity_iap_init();
        }

        public override void QueryPurchaseOrder()
        {
            if (!CheckIsInitSDK()) return;
            unity_iap_check();
        }

        public override void Pay(string data)
        {
            if (!CheckIsInitSDK()) return;
            var json = JsonHelper.FromJson(data);
            var productIdentifier = json["skuId"].ToString();
            const int count = 1;
            var serverOrderId = json["selfOrderId"].ToString();
            unity_iap_pay(productIdentifier, count, serverOrderId);
        }

        public override void ConsumeCallback(string orderId, int state)
        {
            if (!CheckIsInitSDK()) return;
            unity_iap_finish_transaction(orderId);
        }


        public override string GetPurchaseCurrencyCode(string productId)
        {
            return "";
        }

        public override string GetPurchaseLocalPrice(string productId)
        {
            if (productId.IsNullOrEmpty())
            {
                return "0";
            }

            return "";
        }

        #endregion
        #region 推送
        //清除所有
        public override void CancelAllNotifications()
        {
            iOSNotificationCenter.ApplicationBadge = 0;
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        public override void SendNotification(int id, string title, string content, long time)
        {
            // Debug.LogError("SendNotification..zzzzzzzzzzzzz" + id + ".." + content + ".." + time);
            if(time == 0)
                return;
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = TimeSpan.FromMilliseconds(time),
                Repeats = false
            };
            var data = new iOSNotification()
            {
                Identifier = $"UW_IOS_Notification_{id}",
                Title = title,
                Body = content,
                Trigger = timeTrigger,
                ShowInForeground = false
                //ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound | PresentationOption.Badge)
            };
            iOSNotificationCenter.ScheduleNotification(data);
            // Debug.LogError("SendNotification..zzzzzzzzzzzzz endddd");
        }
        
        #endregion
#endif
       
    }
}