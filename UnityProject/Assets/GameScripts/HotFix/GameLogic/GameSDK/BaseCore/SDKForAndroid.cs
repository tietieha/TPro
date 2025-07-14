using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using TEngine;
using UnityEngine;
using Utils;

namespace SDK.BaseCore
{
    public class SDKForAndroid : SDKBase
    {
        #region JavaHelper

        // _jo is JavaObject For short
        private readonly AndroidJavaObject _jo;
        
        // GAID: 不确定是什么时候获取到GAID，在这里缓存一下
        private string _gaid;
        
        /*
         * 获取到了主Activity作为通信的桥梁（对应到Java其实就是AndroidManifest注册为主活动的那个Activity）
         */
        public SDKForAndroid()
        {
            var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // 调用主Activity的方法
        private void Call(string funcName, params object[] args)
        {
            try
            {
                _jo?.Call(funcName, args);
            }
            catch (System.Exception e)
            {
                Log.Warning(e.Message);
            }
        }

        // 调用主Activity带返回值的方法
        private string CallFun(string funcName, params object[] args)
        {
            var rlt = "";
            try
            {
                if (_jo != null)
                {
                    rlt = _jo.Call<string>(funcName, args);
                }
            }
            catch (System.Exception e)
            {
                Log.Warning(e.Message);
            }

            return rlt;
        }

        #endregion

        public override void AddListener()
        {
            Call("SDKAddListener", new AndroidSDKListener());
        }

        public override void Init()
        {
            Call("SDKInit");
        }

        public override void SetGameUid(string gameUid)
        {
            //Call("SetGameUid", gameUid);
        }

        public override void SendDataToNative(string funcName, string data)
        {
            // Call("SDKSendDataToNative", funcName, data);
        }

        public override string GetDataFromNative(string funcName, string data)
        {
            return CallFun("SDKGetDataFromNative", funcName, data);
        }

        public void SetGAID(string gaid)
        {
            _gaid = gaid;
        }

        public string GetGAID()
        {
            return _gaid;
        }
        #region 支付

        public override void Pay(string data)
        {
            if (!CheckIsInitSDK())
            {
                return;
            }

            Call("SDKPay", data);
        }

        public override void QueryPurchaseOrder()
        {
            Call("QueryPurchaseOrder");
        }


        public override void InitPurchase()
        {
            Call("InitGooglePay");
        }

        public override string GetPurchaseCurrencyCode(string productId)
        {
            string payCurrentCode = StorageUtils.GetString(GameDefines.SettingKeys.UwCurrencyCodeKey, "");
            return payCurrentCode;
        }

        public override string GetPurchaseLocalPrice(string productId)
        {
            if (productId.IsNullOrEmpty())
            {
                return "0";
            }

            return "";
        }

        public override void ConsumeCallback(string orderId, int state)
        {
            Call("ConsumeCallback", orderId, state);
        }

        #endregion
    }
}