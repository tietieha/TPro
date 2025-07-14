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

namespace Platform.Data
{
    public static class PlatformDefines
    {
        public class Events
        {
            public const string Game_Init_Success = "game_init_success"; // 游戏收到SDK初始化成功
            public const string Game_Patch_Start_Outflow = "game_patch_start_outflow"; // 热更新开始
            public const string Game_Patch_Checkversion_Start = "game_patch_checkversion_start"; // 获取最新资源版本
            public const string Game_Patch_Checkversion_Success = "game_patch_checkversion_success"; // 检测资源版本完成
            public const string Game_Patch_Checkversion_Fail = "game_patch_checkversion_fail"; // 检测资源版本失败
            public const string Game_Patch_Show = "game_patch_show"; // 热更下载确认窗曝光
            public const string Game_Patch_Confirm = "game_patch_confirm"; // 用户确认下载
            public const string Game_Patch_Start = "game_patch_start"; // 开始下载资源
            public const string Game_Patch_Error = "game_patch_error"; // 单文件下载异常
            public const string Game_Patch_Progress = "game_patch_progress"; // 下载进度完成10%、30%、50%、70%时分别上报一次
            public const string Game_Patch_Download_Success = "game_patch_download_success"; // 成功完成资源下载
            public const string Game_Patch_Unzip_Start = "game_patch_unzip_start"; // 开始解压资源
            public const string Game_Patch_Unzip_Success = "game_patch_unzip_success"; // 解压资源成功
            public const string Game_Patch_Unzip_Fail = "game_patch_unzip_fail"; // 解压资源失败
            public const string Game_Patch_Success = "game_patch_success"; // 资源更新成功
            public const string Game_Patch_Res_Start = "game_patch_res_start"; // 开始加载资源文件
            public const string Game_Patch_Res_Success = "game_patch_res_success"; // 加载资源文件成功
            public const string Game_Patch_Script_Start = "game_patch_script_start"; // 开始加载脚本文件
            public const string Game_Patch_Script_Success = "game_patch_script_success"; // 加载脚本文件成功
            public const string Game_Patch_Config_Start = "game_patch_config_start"; // 开始加载配置文件
            public const string Game_Patch_Config_Success = "game_patch_config_success"; // 加载配置文件成功
            public const string Game_Patch_Success_Outflow = "game_patch_success_outflow"; // 热更新完成
            public const string Game_Login_View_Outflow = "game_login_view_outflow"; // 进入游戏登录主界面
            public const string Game_Login_Call = "game_login_call"; // 游戏调用SDK登录接口

            public const string Game_Login_Call_Success = "game_login_call_success";      // 游戏调用SDK登录接口成功
            public const string Game_LoginToken_Outflow = "game_logintoken_outflow";      // 登录Token曝光
            public const string Game_LoginToken_Success_Outflow = "game_logintoken_success_outflow";  // 登录Token成功
            public const string Game_LoginToken_Fail = "game_logintoken_fail";            // 登录Token失败

            public const string Game_Server_Login_Outflow = "game_server_login_outflow";          // 服务器登录曝光
            public const string Game_Server_Maintenance = "game_server_maintenance";             // 服务器维护中
            public const string Game_Server_Full = "game_server_full";                           // 服务器已满
            public const string Game_Server_Queue_Start = "game_server_queue_start";              // 开始排队
            public const string Game_Server_Queue_Cancel = "game_server_queue_cancel";           // 取消排队
            public const string Game_Server_Login_Success_Outflow = "game_server_login_success_outflow";  // 服务器登录成功
            public const string Game_Createrole_Show_Outflow = "game_createrole_show_outflow";    // 角色创建界面曝光
            public const string Game_Createrole_Click_Outflow = "game_createrole_click_outflow";  // 点击创建角色
            public const string Game_Createrole_Success_Outflow = "game_createrole_success_outflow";  // 角色创建成功
            public const string Game_Server_Enter_Outflow = "game_server_enter_outflow";         // 进入游戏服务器
        }
    }

    public enum OPCode
    {
        INIT,               //初始化
        LOGIN_SUCCESS,      //登录成功
        LOGIN_FAILED,       //登录失败
        LOGIN_CANCEL,       //登录取消
        BIND,               //绑定完成（海外游戏专用）
        PRODUCTS,           //商品列表
        PAY_SUCCESS,        //支付成功
        PAY_FAILED,         //支付失败
        PAY_CANCEL,         //支付取消
        LOGOUT,             //登出账号
        SERVERS,            //服务器列表
        SERVER,             //服务器详情
        ROLES,              //角色列表
    }
}