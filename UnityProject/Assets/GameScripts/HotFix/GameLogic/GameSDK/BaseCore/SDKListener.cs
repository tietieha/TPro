using SDK.Purchase;
using TEngine;
using UnityEngine;
using Utils.JsonUtils;

#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;
#endif

namespace SDK.BaseCore
{
    public class SDKListener
    {
        // 向Native发送所需数据
        public object GetDataFromGame(string funcName, string data)
        {
            return null;
        }

        // 从Native接收数据并处理
        public void SendDataToGame(string funcName, string data)
        {
            switch (funcName)
            {
                case "OnSDKInit":
                    SDKManager.Instance.OnSDKInit(data);
                    break;
                case "SetPriceList":
                    var receivedData = JsonHelper.FromJson(data);
                    var info = (string) receivedData["data"];
                    PurchaseUtils.SetSkuInfoFromNative(info);
                    break;
                case "OnSDKPurchase":
                    SDKManager.Instance.OnSDKPurchase(data);
                    break;
                case "setGaid":
                    receivedData = JsonHelper.FromJson(data);
                    var gaid = (string) receivedData["1"];
                    SDKManager.Instance.OnSetGAID(gaid);
                    break;
                case "UploadImageCallBack":
                {
                    SDKManager.Instance.CallNativeSDKOver();
                    var recvData =JsonHelper.FromJson(data);
                    string urlPath = (string)recvData["1"];
                    string funTypeStr = (string)recvData["2"];
                    int funType = funTypeStr.ToInt();
                    if (!string.IsNullOrEmpty(urlPath))
                    {
                        //上传照片
                        SDKManager.Instance.BeginUploadImgToServer(urlPath);
                    }
                    else
                    {
                        SDKManager.Instance.FailedUploadImgToServer();
                    }
                    break;
                }
            }
        }
    }
    
    public class AndroidSDKListener : AndroidJavaProxy
    {
        private readonly SDKListener _listener;
        // 通过JavaProxy的方式和SdkListener.java进行通信
        public AndroidSDKListener() : base("com.sdkmanager.SdkListener")
        {
            _listener = new SDKListener();
        }

        // 原生层从游戏层获取数据
        public object GetDataFromGame(string funcName, string data)
        {
            return _listener.GetDataFromGame(funcName, data);
        }

        // 原生层发送数据到游戏层
        public void SendDataToGame(string funcName, string data)
        {
            // TimerManager.Instance.AddOneShotTask(0, () =>
            // {
            //     _listener.SendDataToGame(funcName, data);
            // });
        }
    }

    #if UNITY_IOS
    public class IosSDKListener
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callbackN0Delegate(IntPtr funcName, IntPtr data);
        public delegate string callbackN1Delegate(IntPtr funcName, IntPtr data);
        [DllImport("__Internal")]
        public static extern void SetN0CallBack(IntPtr callback);

        [DllImport("__Internal")]
        public static extern void SetN1CallBack(IntPtr callback);
        
        private static SDKListener _listener;
        public IosSDKListener()
        {
            _listener = new SDKListener();
            RegisterCallback();
        }

        private void RegisterCallback()
        {
            callbackN0Delegate callbackN0_delegate = SendDataToGame;
            callbackN1Delegate callbackN1_delegate = GetDataFromGame;

            //将Delegate转换为非托管的函数指针
            IntPtr intptrN0_delegate = Marshal.GetFunctionPointerForDelegate(callbackN0_delegate);
            IntPtr intptrN1_delegate = Marshal.GetFunctionPointerForDelegate(callbackN1_delegate);

            //调用非托管函数
            SetN0CallBack(intptrN0_delegate);
            SetN1CallBack(intptrN1_delegate);
        }
        
        //原生层发送数据到游戏层
        [MonoPInvokeCallback(typeof(callbackN0Delegate))]
        static void SendDataToGame(IntPtr funcName, IntPtr data)
        {
            var funcNameStr = Marshal.PtrToStringAnsi(funcName);
            var dataStr = Marshal.PtrToStringAnsi(data);
            TimerManager.Instance.AddOneShotTask(0, () =>
            {
                _listener.SendDataToGame(funcNameStr, dataStr);
            });
        }

        //原生层直接从游戏层 - 获取数据
        [MonoPInvokeCallback(typeof(callbackN1Delegate))]
        static string GetDataFromGame(IntPtr funcName, IntPtr data)
        {
            var retObj = _listener.GetDataFromGame(Marshal.PtrToStringAnsi(funcName), Marshal.PtrToStringAnsi(data));
            return Convert.ToString(retObj);
        }
    }
#endif
}