// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-06-05       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using System.Reflection;
using UnityEngine;

namespace TEngine
{
    public class SDKHelper
    {
        public static bool IsSDKOn => GameModule.SDK.SdkModuleImp?.IsSDKOn ?? false;

        public static bool IsOnceSDK
        {
            get
            {
#if ONCE_SDK
                return true;
#endif
                return false;
            }
        }

        #region 渠道信息
        public static string GetPlatformChannel()
        {
            return GameModule.SDK.SdkModuleImp?.GetPlatformChannel() ?? string.Empty;
        }

        public static string GetLoginChannel()
        {
            return GameModule.SDK.SdkModuleImp?.GetLoginChannel() ?? string.Empty;
        }

        public static string GetVersionName()
        {
            return Application.version;
        }
        #endregion

        public static void Login()
        {
            GameModule.SDK.SdkModuleImp?.Login();
        }

        public static void Logout()
        {
            GameModule.SDK.SdkModuleImp?.Logout();
        }

        public static void Server(int serverId = 0)
        {
            GameModule.SDK.SdkModuleImp?.Server(serverId);
        }

        public static void ExitGame()
        {
            GameModule.SDK.SdkModuleImp?.ExitGame();
        }

        public static void ShowTerms(string type)
        {
            GameModule.SDK.SdkModuleImp?.ShowTerms(type);
        }

        public static bool WebViewAvailable(string url)
        {
            return GameModule.SDK.SdkModuleImp?.WebviewAvailable(url) ?? false;
        }

        public static void Webview(string url, string param = "")
        {
            GameModule.SDK.SdkModuleImp?.Webview(url, param);
        }
        #region Once




        public static void GameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel, string roleCreateTime, string roleData)
        {
            GameModule.SDK.SdkModuleImp?.GameStarted(newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime, roleData);
        }

        public static void RoleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            GameModule.SDK.SdkModuleImp?.RoleUpdate(roleName, roleLevel, vipLevel, roleData);
        }

        public static void Pay(string payData)
        {
            GameModule.SDK.SdkModuleImp?.Pay(payData);
        }



        public static bool DelAvailable()
        {
            return GameModule.SDK.SdkModuleImp?.DelAvailable() ?? false;
        }

        public static void DelAccount()
        {
            GameModule.SDK.SdkModuleImp?.DelAccount();
        }

        public static void EventClient(string eventName, string eventData = "")
        {
            GameModule.SDK.SdkModuleImp?.EventClient(eventName, eventData);
        }

        public static void Placard()
        {
            GameModule.SDK.SdkModuleImp?.Placard();
        }

        public static bool PlacardAvailable()
        {
            return GameModule.SDK.SdkModuleImp?.PlacardAvailable() ?? false;
        }

        public static bool PlacardUpdated()
        {
            return GameModule.SDK.SdkModuleImp?.PlacardUpdated() ?? false;
        }
        #endregion

        #region Func
        public static void CallLuaGlobal(string funcName, string arg1)
        {
            var callLuaGlobalMethod = GameModule.GameAppType.GetMethod("CallLuaGlobal", new Type[] { typeof(string), typeof(string)});
            if (callLuaGlobalMethod == null)
            {
                Log.Fatal($"GameApp.CallLuaGlobal<string> not found.");
                return;
            }

            object[] objects = { funcName, arg1 };
            callLuaGlobalMethod.Invoke(GameModule.GameAppType, objects);
        }
        public static void CallLuaGlobal(string funcName, string arg1, string arg2)
        {
            var callLuaGlobalMethod = GameModule.GameAppType.GetMethod("CallLuaGlobal", new Type[] { typeof(string), typeof(string), typeof(string)});
            if (callLuaGlobalMethod == null)
            {
                Log.Fatal($"GameApp.CallLuaGlobal<string, string> not found.");
                return;
            }

            object[] objects = { funcName, arg1, arg2 };
            callLuaGlobalMethod.Invoke(GameModule.GameAppType, objects);
        }

        public static void CallLuaGlobal(string funcName, object[] args)
        {
            var callLuaGlobalMethod = GameModule.GameAppType.GetMethod("CallLuaGlobal", new Type[] { typeof(string), typeof(object[])});
            if (callLuaGlobalMethod == null)
            {
                Log.Fatal($"GameApp.CallLuaGlobal<object[]> not found.");
                return;
            }

            object[] objects = { funcName, args };
            callLuaGlobalMethod.Invoke(GameModule.GameAppType, objects);
        }

        #endregion

        #region Events

        public static void OnSDKEvent(string args)
        {
            GameModule.SDK.SdkModuleImp?.OnSDKEvent(args);
        }


        #endregion

        #region 设备信息
        public static string GetCarrierName()
        {
            return GameModule.SDK.SdkModuleImp?.GetCarrierName() ?? string.Empty;
        }

        public static string GetIMEI()
        {
            return GameModule.SDK.SdkModuleImp?.GetIMEI() ?? string.Empty;
        }

        public static string GetOAID()
        {
            return GameModule.SDK.SdkModuleImp?.GetOAID() ?? string.Empty;
        }

        /// <summary>
        /// 获取磁盘可用空间 MB
        /// </summary>
        /// <returns></returns>
        public static float GetAvailableStorage()
        {
            float availableStorage = 0f;
            try
            {
                availableStorage = float.Parse(Call("GetAvailableStorage"));
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return availableStorage;
        }

        public static string Call(string funcName, params object[] args)
        {
            return GameModule.SDK.SdkModuleImp?.Call(funcName, args) ?? string.Empty;
        }

        public static string StaticCall(string funcName, params object[] args)
        {
            return GameModule.SDK.SdkModuleImp?.StaticCall(funcName, args) ?? string.Empty;
        }

        #endregion
    }
}