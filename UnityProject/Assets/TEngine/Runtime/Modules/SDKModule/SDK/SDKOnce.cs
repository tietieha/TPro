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
using System.Linq;
using Platform;
using Platform.Data;
using UnityEngine;

namespace TEngine.SDK
{
    public class SDKOnce : SDKAndroid
    {
        private static readonly string[] DOWNLOAD_PROGRESS = { "10", "30", "50", "70" };

        public override void Init()
        {
            Log.Info("[SDK]: Once SDK 初始化");
            SdkPlatform.api().initSDK(OncePlatformCallback);
        }

        private void OncePlatformCallback(string jsonParam)
        {
            Log.Debug("[SDK] OncePlatformCallback jsonParam:" + jsonParam);
            SDKBaseResponse sdkBaseResponse = Utility.Json.ToObject<SDKBaseResponse>(jsonParam);
            OPCode opcode = (OPCode)Enum.Parse(typeof(OPCode), sdkBaseResponse.opcode);
            switch (opcode)
            {
                case OPCode.INIT:
                    IsInitSuccess = true;
                    Log.Info("[SDK]: Once初始化成功");
                    EventClient(PlatformDefines.Events.Game_Init_Success);
                    SDKInitResponse initResponse = Utility.Json.ToObject<SDKInitResponse>(jsonParam);
                    if (SettingsUtils.IsOverrideUpdateInfo())
                    {
                        SettingsUtils.OverrideNoticeUrl = initResponse.data.noticeUrl;
                        SettingsUtils.SetOverrideUpdateInfo(initResponse.data.configUrl, initResponse.data.hostServerURL, initResponse.data.fetchServerUrl, initResponse.data.defaultServerId);
                        GameModule.Resource.PackageVersion = initResponse.data.currentVersion;
                        GameModule.Resource.RemoteConfigVersion = initResponse.data.configVersion;
                    }

                    break;
                case OPCode.LOGIN_SUCCESS:
                    EventClient(PlatformDefines.Events.Game_Login_Call_Success);
                    SDKHelper.CallLuaGlobal("OnSDKCallback", sdkBaseResponse.opcode, jsonParam);
                    break;
                default:
                    SDKHelper.CallLuaGlobal("OnSDKCallback", sdkBaseResponse.opcode, jsonParam);
                    break;
            }
        }

        #region OP

        /// <summary>
        /// 登录
        /// </summary>
        public override void Login()
        {
            base.Login();
            EventClient(PlatformDefines.Events.Game_Login_Call);
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法登录");
                return;
            }

            SdkPlatform.api().login();
        }

        /// <summary>
        /// 登出
        /// </summary>
        public override void Logout()
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: SDK未初始化成功，无法登出");
                return;
            }
            SdkPlatform.api().logout();
        }

        public override void Server(int serverId = 0)
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法获取服务器信息");
                return;
            }

            SdkPlatform.api().server(serverId);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="newRole">是否是新创建角色，true 是，false 不是；</param>
        /// <param name="roleId">角色 ID</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleLevel">角色等级</param>
        /// <param name="vipLevel">VIP等级</param>
        /// <param name="roleCreateTime">创建时间（字符串时间戳，单位秒）</param>
        /// <param name="roleData">角色数据（游戏可以传入自定义字符串，在角色列表回调中返回给游戏客户端）</param>
        public void GameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel, string roleCreateTime, string roleData)
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法通知游戏开始");
                return;
            }
            Log.Info("SDKModuleImp: 调用SDK通知游戏开始");
            SdkPlatform.api().gameStarted(newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime, roleData);
        }

        /// <summary>
        /// 角色更新
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleLevel">角色等级</param>
        /// <param name="vipLevel">VIP等级</param>
        /// <param name="roleData">角色数据</param>
        public void RoleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法更新角色信息");
                return;
            }
            Log.Info("SDKModuleImp: 调用SDK更新角色信息");
            SdkPlatform.api().roleUpdate(roleName, roleLevel, vipLevel, roleData);
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="payData">游戏服务器返回的platformOrderData字符串(格式：最大2048个字符的加密字符串)</param>
        public void Pay(string payData)
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法进行支付");
                return;
            }
            Log.Info("SDKModuleImp: 调用SDK进行支付");
            SdkPlatform.api().pay(payData);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public override void ExitGame()
        {
            base.ExitGame();
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法退出游戏");
                return;
            }
            SdkPlatform.api().exitGame();
        }

        public override void ShowTerms(string type)
        {
            base.ShowTerms(type);
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法显示服务条款");
                return;
            }
            SdkPlatform.api().showTerms(type);
        }

        public override bool WebviewAvailable(string url)
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法检查Webview可用性");
                return false;
            }
            return SdkPlatform.api().webviewAvailable(url);
        }

        public override void Webview(string url, string param = "")
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法打开webview");
                return;
            }
            SdkPlatform.api().webview(url, param);
        }

        /// <summary>
        /// 删除账号可见
        /// </summary>
        /// <returns></returns>
        public bool DelAvailable()
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法检查删除可用性");
                return false;
            }
            Log.Info("[SDK]:Once 调用SDK检查删除可用性");
            return SdkPlatform.api().delAvailable();
        }

        /// <summary>
        /// 删除账号
        /// 首先根据删除账号可见接口（delAvailable）返回值显示和隐藏删除账号按钮；然后调用删除账号面板接口打开删除面板；
        /// </summary>
        public void DelAccount()
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法删除账号");
                return;
            }
            Log.Info("[SDK]:Once 调用SDK删除账号");
            SdkPlatform.api().del();
        }

        /// <summary>
        /// 客户端事件打点
        /// </summary>
        /// <param name="eventKey">事件关键字</param>
        /// <param name="eventValue">事件参数，需要传输json字符串。无参数时传空字符串</param>
        public void EventClient(string eventKey, string eventValue = "")
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法发送客户端事件");
                return;
            }
            Log.Info($"[SDK]:Once 调用SDK发送客户端事件 {eventKey}");
            SdkPlatform.api().eventClient(eventKey, eventValue);
        }

        public void Placard()
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法获取公告");
                return;
            }
            Log.Info("[SDK]:Once 调用SDK获取公告");
            SdkPlatform.api().placard();
        }

        public bool PlacardAvailable()
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法检查公告可用性");
                return false;
            }
            Log.Info("[SDK]:Once 调用SDK检查公告可用性");
            return SdkPlatform.api().placardAvailable();
        }

        public bool PlacardUpdated()
        {
            if (!IsInitSuccess)
            {
                Log.Error("[SDK]: Once未初始化成功，无法检查公告更新");
                return false;
            }
            Log.Info("[SDK]:Once 调用SDK检查公告更新");
            return SdkPlatform.api().placardUpdated();
        }

        #endregion

        #region Events

        public override void OnResStartUpdate()
        {
            EventClient(PlatformDefines.Events.Game_Patch_Start_Outflow);
        }

        public override void OnResCheckVersionStart(string oldVersion, string newVersion)
        {
            var versionData = new
            {
                current_version = oldVersion,
                update_version = newVersion
            };
            string versionJson = Utility.Json.ToJson(versionData);
            EventClient(PlatformDefines.Events.Game_Patch_Checkversion_Start, versionJson);
        }

        public override void OnResCheckVersionSuccess(string fileSize)
        {
            var eventValue = new
            {
                file_size = fileSize,
            };
            string eventJson = Utility.Json.ToJson(eventValue);
            EventClient(PlatformDefines.Events.Game_Patch_Checkversion_Success, eventJson);
        }

        public override void OnResCheckVersionFail(string reason)
        {
            var eventValue = new
            {
                reason = reason,
            };
            string eventJson = Utility.Json.ToJson(eventValue);
            EventClient(PlatformDefines.Events.Game_Patch_Checkversion_Fail, eventJson);
        }

        public override void OnResUpdateSuccess()
        {
            EventClient(PlatformDefines.Events.Game_Patch_Success_Outflow);
        }

        public override void OnSDKEvent(string args)
        {
            string[] argArr = args.Split('|');
            string eventName = argArr.Length > 0 ? argArr[0] : string.Empty;
            switch (eventName)
            {
                case ProcedureEventDefines.Command.PROCEDURE_RES_PATCH_SHOW:
                    EventClient(PlatformDefines.Events.Game_Patch_Show);
                    break;
                case ProcedureEventDefines.Command.PROCEDURE_RES_PATCH_CONFIRM:
                    EventClient(PlatformDefines.Events.Game_Patch_Confirm);
                    break;
                case ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_START:
                    EventClient(PlatformDefines.Events.Game_Patch_Start);
                    break;
                case ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_PROGRESS:
                    if (argArr.Length < 2) return;
                    var progressStr = argArr[1];
                    if (DOWNLOAD_PROGRESS.Contains(progressStr))
                    {
                        var progressData = new
                        {
                            progress = progressStr
                        };
                        string progressJson = Utility.Json.ToJson(progressData);
                        EventClient(PlatformDefines.Events.Game_Patch_Progress, progressJson);
                    }
                    break;
                case ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_SUCCESS:
                    EventClient(PlatformDefines.Events.Game_Patch_Download_Success);
                    EventClient(PlatformDefines.Events.Game_Patch_Success);
                    break;
                case ProcedureEventDefines.Command.PROCEDURE_SCRIPT_START:
                    EventClient(PlatformDefines.Events.Game_Patch_Script_Start);
                    break;
                case ProcedureEventDefines.Command.PROCEDURE_SCRIPT_SUCCESS:
                    EventClient(PlatformDefines.Events.Game_Patch_Script_Success);
                    break;

                case ProcedureEventDefines.Events.COMMON_SIMULATE_CRASH:
                    CrashSightAgent.TestNativeCrash();
                    break;
                case ProcedureEventDefines.Events.ENTER_LOBBY:
                    EventClient(PlatformDefines.Events.Game_Login_View_Outflow);
                    break;
                case ProcedureEventDefines.Events.CHECK_LOGIN_TOKEN:
                    EventClient(PlatformDefines.Events.Game_LoginToken_Outflow);
                    break;
                case ProcedureEventDefines.Events.CHECK_LOGIN_TOKEN_SUCCESS:
                    EventClient(PlatformDefines.Events.Game_LoginToken_Success_Outflow);
                    break;
                case ProcedureEventDefines.Events.CHECK_LOGIN_TOKEN_FAIL:
                    EventClient(PlatformDefines.Events.Game_LoginToken_Fail);
                    break;
                case ProcedureEventDefines.Events.SERVER_LOGIN:
                    EventClient(PlatformDefines.Events.Game_Server_Login_Outflow);
                    break;
                case ProcedureEventDefines.Events.SERVER_LOGIN_SUCCESS:
                    EventClient(PlatformDefines.Events.Game_Server_Login_Success_Outflow);
                    break;
                case ProcedureEventDefines.Events.CREATE_ROLE_SHOW:
                    EventClient(PlatformDefines.Events.Game_Createrole_Show_Outflow);
                    break;
                case ProcedureEventDefines.Events.CREATE_ROLE_CLICK:
                    EventClient(PlatformDefines.Events.Game_Createrole_Click_Outflow);
                    break;
                case ProcedureEventDefines.Events.CREATE_ROLE_SUCCESS:
                    EventClient(PlatformDefines.Events.Game_Createrole_Success_Outflow);
                    break;
                case ProcedureEventDefines.Events.ENTER_GAME:
                    EventClient(PlatformDefines.Events.Game_Server_Enter_Outflow);
                    break;
                default:
                    Log.Warning($"[SDK]: Unhandled SDK event: {eventName}");
                    break;
            }
        }

        #endregion

        #region 渠道

        public override string GetPlatformChannel()
        {
            return SdkPlatform.param().channelId();
        }

        public override string GetLoginChannel()
        {
            return SdkPlatform.param().trackId();
        }

        public override string GetIMEI()
        {
            return SdkPlatform.param().deviceId();
        }

        public override string GetOAID()
        {
            return SdkPlatform.param().advertisingId();
        }

        #endregion
    }
}