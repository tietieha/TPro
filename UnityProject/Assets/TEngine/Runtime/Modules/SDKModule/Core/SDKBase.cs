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

namespace TEngine.SDK
{
    public class SDKBase
    {
        public bool IsInitSuccess { get; set; } = false;

        public virtual void Init()
        {
            IsInitSuccess = true;
            Log.Info("[SDK]: SDKBase 初始化成功");
        }

        public virtual void Login()
        {
            Log.Info("[SDK]: Login");
        }

        public virtual void Logout()
        {
            Log.Info("[SDK]: Logout");
        }

        /// <summary>
        /// 登录服务器
        /// </summary>
        /// <param name="serverId"></param>
        public virtual void Server(int serverId = 0)
        {
            Log.Info($"[SDK]: ServerList, ServerId: {serverId}");
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public virtual void ExitGame()
        {
            Log.Info("[SDK]: ExitGame");
        }

        /// <summary>
        /// 隐私合规
        /// </summary>
        /// <param name="type"></param>
        public virtual void ShowTerms(string type)
        {
            Log.Debug($"[SDK]: ShowTerms, Type: {type}");
        }

        public virtual bool WebviewAvailable(string url)
        {
            Log.Debug($"[SDK]: WebviewAvailable, URL: {url}");
            return false;
        }

        public virtual void Webview(string url, string param = "")
        {
        }

        #region Events
        public virtual void OnSDKInitSuccess()
        {
        }

        public virtual void OnResStartUpdate()
        {
        }

        public virtual void OnResCheckVersionStart(string oldVersion, string newVersion)
        {
        }

        public virtual void OnResUpdateSuccess()
        {
        }

        public virtual void OnResCheckVersionFail(string reason)
        {
        }

        public virtual void OnResCheckVersionSuccess(string fileSize)
        {
        }

        /// <summary>
        /// cmd|param1|param2|...
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnSDKEvent(string args)
        {
        }
        #endregion


        #region 设备信息

        public virtual string GetCarrierName()
        {
            return "unknown";
        }

        public virtual string GetIMEI()
        {
            Log.Warning("[SDK]: GetIMEI Default.");
            return "unknown";
        }

        public virtual string GetOAID()
        {
            Log.Warning("[SDK]: GetOAID Default.");
            return "unknown";
        }

        #endregion

        public virtual string GetPlatformChannel()
        {
            Log.Warning("[SDK]: GetPlatformChannel Default.");
            return "unknown";
        }

        public virtual string GetLoginChannel()
        {
            Log.Warning("[SDK]: GetLoginChannel Default.");
            return "unknown";
        }

        public virtual string Call(string funcName, params object[] args)
        {
            Log.Warning($"[SDK]: {funcName} Default.");
            return string.Empty;
        }

        public virtual string StaticCall(string funcName, params object[] args)
        {
            Log.Warning($"[SDK]: {funcName} Default.");
            return string.Empty;
        }
    }
}