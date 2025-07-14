// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-06-07       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

namespace TEngine
{
    public static class ProcedureEventDefines
    {
        public class Events
        {
            public const string COMMON_SIMULATE_CRASH = "COMMON_SIMULATE_CRASH"; // 模拟崩溃
            public const string COMMON = "COMMON"; // 通用

            public const string SDK_INIT_SUCCESS = "SDK_INIT_SUCCESS"; // SDK初始化完成
            public const string PROCEDURE_RES_START_UPDATE = "PROCEDURE_RES_START_UPDATE"; // 开始热更
            public const string PROCEDURE_RES_CHECK_VERSION = "PROCEDURE_RES_CHECK_VERSION"; // 检查版本
            public const string PROCEDURE_RES_CHECK_VERSION_SUCCESS = "PROCEDURE_RES_CHECK_VERSION_SUCCESS"; // 检查版本成功
            public const string PROCEDURE_RES_CHECK_VERSION_FAIL = "PROCEDURE_RES_CHECK_VERSION_FAIL"; // 检查版本失败
            public const string PROCEDURE_RES_UPDATE_SUCCESS = "PROCEDURE_RES_UPDATE_SUCCESS"; // 热更成功

            public const string ENTER_LOBBY = "ENTER_LOBBY"; // 进入大厅
            public const string CHECK_LOGIN_TOKEN = "CHECK_LOGIN_TOKEN"; // 开始平台token校验
            public const string CHECK_LOGIN_TOKEN_SUCCESS = "CHECK_LOGIN_TOKEN_SUCCESS"; // 平台token校验成功
            public const string CHECK_LOGIN_TOKEN_FAIL = "CHECK_LOGIN_TOKEN_FAIL"; // 平台token校验失败

            public const string SERVER_LOGIN = "SERVER_LOGIN"; // 进入游戏
            public const string SERVER_LOGIN_SUCCESS = "SERVER_LOGIN_SUCCESS"; // 服务器登录成功

            public const string CREATE_ROLE_SHOW = "CREATE_ROLE_SHOW"; // 显示创建角色界面
            public const string CREATE_ROLE_CLICK = "CREATE_ROLE_CLICK"; // 点击创建角色按钮
            public const string CREATE_ROLE_SUCCESS = "CREATE_ROLE_SUCCESS"; // 创建角色成功

            public const string ENTER_GAME = "ENTER_GAME"; // 进入游戏

        }

        public class Command
        {
            public const string PROCEDURE_RES_PATCH_SHOW = "PROCEDURE_RES_PATCH_SHOW"; // 热更下载确认窗曝光
            public const string PROCEDURE_RES_PATCH_CONFIRM = "PROCEDURE_RES_PATCH_CONFIRM"; // 用户确认下载

            public const string PROCEDURE_RES_DOWNLOAD_START = "PROCEDURE_RES_DOWNLOAD_START"; // 开始下载资源
            public const string PROCEDURE_RES_DOWNLOAD_ERROR = "PROCEDURE_RES_DOWNLOAD_ERROR"; // 下载失败
            public const string PROCEDURE_RES_DOWNLOAD_PROGRESS = "PROCEDURE_RES_DOWNLOAD_PROGRESS"; // 下载进度
            public const string PROCEDURE_RES_DOWNLOAD_SUCCESS = "PROCEDURE_RES_DOWNLOAD_SUCCESS"; // 下载完成

            public const string PROCEDURE_SCRIPT_START = "PROCEDURE_SCRIPT_START"; // 开始加载脚本
            public const string PROCEDURE_SCRIPT_SUCCESS = "PROCEDURE_SCRIPT_SUCCESS"; // 加载脚本成功
        }
    }
}