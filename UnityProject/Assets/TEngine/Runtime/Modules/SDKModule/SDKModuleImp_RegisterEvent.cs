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

namespace TEngine
{
    /// <summary>
    /// 主要是打点用的
    /// </summary>
    internal partial class SDKModuleImp
    {
        private void RegisterEvent()
        {
            GameEvent.AddEventListener(ProcedureEventDefines.Events.SDK_INIT_SUCCESS, OnSDKInitSuccess);
            GameEvent.AddEventListener(ProcedureEventDefines.Events.PROCEDURE_RES_START_UPDATE, OnResStartUpdate);
            GameEvent.AddEventListener<string, string>(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION, OnResCheckVersionStart);
            GameEvent.AddEventListener<string>(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION_SUCCESS, OnResCheckVersionSuccess);
            GameEvent.AddEventListener<string>(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION_FAIL, OnResCheckVersionFail);
            GameEvent.AddEventListener(ProcedureEventDefines.Events.PROCEDURE_RES_UPDATE_SUCCESS, OnResUpdateSuccess);

            GameEvent.AddEventListener<string>(ProcedureEventDefines.Events.COMMON, OnSDKEvent);
        }

        private void UnregisterEvent()
        {
            GameEvent.RemoveEventListener(ProcedureEventDefines.Events.SDK_INIT_SUCCESS, OnSDKInitSuccess);
            GameEvent.RemoveEventListener(ProcedureEventDefines.Events.PROCEDURE_RES_START_UPDATE, OnResStartUpdate);
            GameEvent.RemoveEventListener<string, string>(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION, OnResCheckVersionStart);
            GameEvent.RemoveEventListener<string>(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION_SUCCESS, OnResCheckVersionSuccess);
            GameEvent.RemoveEventListener<string>(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION_FAIL, OnResCheckVersionFail);
            GameEvent.RemoveEventListener(ProcedureEventDefines.Events.PROCEDURE_RES_UPDATE_SUCCESS, OnResUpdateSuccess);

            GameEvent.RemoveEventListener<string>(ProcedureEventDefines.Events.COMMON, OnSDKEvent);
        }

        #region 监听C#的
        private void OnSDKInitSuccess()
        {
            Sdk.OnSDKInitSuccess();
        }

        private void OnResStartUpdate()
        {
            Sdk.OnResStartUpdate();
        }

        private void OnResCheckVersionStart(string oldVersion, string newVersion)
        {
            Sdk.OnResCheckVersionStart(oldVersion, newVersion);
        }

        private void OnResUpdateSuccess()
        {
            Sdk.OnResUpdateSuccess();
        }

        private void OnResCheckVersionFail(string reason)
        {
            Sdk.OnResCheckVersionFail(reason);
        }

        private void OnResCheckVersionSuccess(string fileSize)
        {
            Sdk.OnResCheckVersionSuccess(fileSize);
        }
        #endregion

        #region Lua 通知的

        public void OnSDKEvent(string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                Log.Error("[SDK] OnSDKEvent: args is null or empty.");
                return;
            }
            Log.Debug($"[SDK] OnSDKEvent: {args}");
            Sdk.OnSDKEvent(args);
        }
        #endregion
    }
}