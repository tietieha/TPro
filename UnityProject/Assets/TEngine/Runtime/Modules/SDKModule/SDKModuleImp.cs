// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-06-04       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using TEngine.SDK;

namespace TEngine
{
    [UpdateModule]
    internal partial class SDKModuleImp : ModuleImp
    {
        public bool IsSDKOn { get; private set; } = true;

        public bool IsInitSuccess {
            get
            {
                return Sdk.IsInitSuccess;
            }
        }

        private SDKBase _sdkBase;
        public SDKBase Sdk
        {
            get
            {
                if (_sdkBase == null)
                {
#if ONCE_SDK // 因为Once有全平台
                    _sdkBase = new SDKOnce();
#elif UNITY_IOS
                    _sdkBase = new SDKIOS();
#elif UNITY_ANDROID && !UNITY_EDITOR
                    _sdkBase = new SDKAndroid();
                    IsSDKOn = false;
#else
                    _sdkBase = new SDKEditor();
                    IsSDKOn = false; // 编辑器模式下不开启SDK
#endif
                }
                return _sdkBase;
            }
        }

        internal override void Shutdown()
        {
            // CrashSight
            CrashSightAgent.CloseAllMonitors();

            UnregisterEvent();
        }

        public void Init()
        {
            Log.Info("[SDK] ModuleImp: 初始化SDK模块");
            RegisterEvent();

#if !UNITY_EDITOR
            // CrashSight
            CrashSightAgent.ReRegistAllMonitors();
            CrashSightAgent.EnableExceptionHandler();
            // CrashSightAgent.ConfigCrashServerUrl("https://android.crashsight.qq.com/pb/async");
            // CrashSightAgent.InitWithAppId("bec4480c76");
#endif
            Sdk.Init();
        }

        public void Login()
        {
            Sdk.Login();
        }

        public void Logout()
        {
            Sdk.Logout();
        }

        public void Server(int serverId = 0)
        {
            Sdk.Server(serverId);
        }

        public void ExitGame()
        {
            Sdk.ExitGame();
        }

        public void ShowTerms(string type)
        {
            Sdk.ShowTerms(type);
        }

        public bool WebviewAvailable(string url)
        {
            return Sdk.WebviewAvailable(url);
        }

        public void Webview(string url, string param = "")
        {
            Sdk.Webview(url, param);
        }

        #region Once
        public void GameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel, string roleCreateTime, string roleData)
        {
            if (Sdk is SDKOnce)
            {
                var sdkOnce = Sdk as SDKOnce;
                if (sdkOnce != null)
                    sdkOnce.GameStarted(newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime, roleData);
            }
        }

        public void RoleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            if (Sdk is SDKOnce)
            {
                var sdkOnce = Sdk as SDKOnce;
                if (sdkOnce != null)
                    sdkOnce.RoleUpdate(roleName, roleLevel, vipLevel, roleData);
            }
        }

        public void Pay(string payData)
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                sdkOnce.Pay(payData);
        }

        public bool DelAvailable()
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                return sdkOnce.DelAvailable();

            return false;
        }

        public void DelAccount()
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                sdkOnce.DelAccount();
        }

        public void EventClient(string eventName, string eventData = "")
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                sdkOnce.EventClient(eventName, eventData);
        }

        public void Placard()
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                sdkOnce.Placard();
        }

        public bool PlacardAvailable()
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                return sdkOnce.PlacardAvailable();

            return false;
        }

        public bool PlacardUpdated()
        {
            var sdkOnce = Sdk as SDKOnce;
            if (sdkOnce != null)
                return sdkOnce.PlacardUpdated();

            return false;
        }
        #endregion

        #region 设备信息
        public string GetCarrierName()
        {
            return Sdk.GetCarrierName();
        }

        public string GetIMEI()
        {
            return Sdk.GetIMEI();
        }

        public string GetOAID()
        {
            return Sdk.GetOAID();
        }
        #endregion

        #region 渠道
        public string GetPlatformChannel()
        {
            return Sdk.GetPlatformChannel();
        }

        public string GetLoginChannel()
        {
            return Sdk.GetLoginChannel();
        }

        #endregion

        #region CommonCall

        public string Call(string funcName, params object[] args)
        {
            return Sdk.Call(funcName, args);
        }

        public string StaticCall(string funcName, params object[] args)
        {
            return Sdk.StaticCall(funcName, args);
        }

        #endregion

    }
}