// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-06-25       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using UnityEngine;
using UnityEngine.Android;

namespace TEngine.SDK
{
    public class SDKAndroid : SDKBase
    {
        #region JavaHelper
        private readonly AndroidJavaObject _jo;

        public SDKAndroid()
        {
            var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }

        private T StaticCall<T>(string method, params object[] param)
        {
            try
            {
                return _jo.CallStatic<T>(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return default(T);
        }

        private T Call<T>(string funcName, params object[] args)
        {
            try
            {
                return _jo.Call<T>(funcName, args);
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }

            return default(T);
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

        public override string Call(string funcName, params object[] args)
        {
            try
            {
                return _jo?.Call<string>(funcName, args);
            }
            catch (System.Exception e)
            {
                Log.Error($"[SDK]: {funcName} failed: {e.Message}");
            }
            return string.Empty;
        }

        public override string StaticCall(string funcName, params object[] args)
        {
            try
            {
                return _jo.CallStatic<string>(funcName, args);
            }
            catch (System.Exception e)
            {
                Log.Error($"[SDK]: {funcName} failed: {e.Message}");
            }
            return string.Empty;
        }
        #endregion

        #region 设备信息
        public override string GetCarrierName()
        {
            try
            {
                AndroidJavaObject telephonyManager = _jo.Call<AndroidJavaObject>("getSystemService", "phone");
                // 获取运营商名称（如 "中国移动"、"China Mobile"）
                string carrierName = telephonyManager.Call<string>("getNetworkOperatorName");
                Log.Debug("[SDK]Carrier: " + carrierName);

                // 获取运营商代码（MCC+MNC，如 "46000"）
                string operatorCode = telephonyManager.Call<string>("getNetworkOperator");
                Log.Debug("[SDK]Operator Code: " + operatorCode);
            }
            catch (Exception e)
            {
                Log.Error($"[SDK]: GetCarrierName failed: {e.Message}");
            }

            return string.Empty;
        }

        public override string GetIMEI()
        {
            // if (!Permission.HasUserAuthorizedPermission("android.permission.READ_PHONE_STATE"))
            // {
            //     Permission.RequestUserPermission("android.permission.READ_PHONE_STATE");
            // }

            try
            {
                AndroidJavaObject telephonyManager = _jo.Call<AndroidJavaObject>("getSystemService", "phone");
                string imei = telephonyManager.Call<string>("getDeviceId"); // 或 getImei() for API 26+
                Log.Debug("上报 IMEI: " + imei);
                return imei;
            }
            catch (Exception e)
            {
                Log.Error($"[SDK]: GetIMEI failed: {e.Message}");
            }

            return base.GetIMEI();
        }
        #endregion
    }
}