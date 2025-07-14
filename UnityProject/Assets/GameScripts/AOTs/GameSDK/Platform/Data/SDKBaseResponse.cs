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

namespace Platform.Data
{

    public class SDKBaseResponse
    {
        public string opcode; // 操作码
        public string msg;
    }

    #region SDK Init Response

    public class InitResponseData
    {
        public string currentVersion;   // 当前版本号
        public string configVersion;    // 配置版本号
        public string hostServerURL;    // 热更地址
        public string fetchServerUrl;   // 获取服务器列表的URL
        public string configUrl;        // 配置文件URL
        public string noticeUrl;        // 公告URL
        public int defaultServerId;     // 默认服务器ID
    }

    public class SDKInitResponse : SDKBaseResponse
    {
        public InitResponseData data;
    }
    #endregion

    #region Login Response
    public class PlatformData
    {
        public int appId; // 应用ID
        public string channelId; // 渠道ID
        public string trackId; // 追踪ID
        public string deviceId; // 设备ID
        public string advertisingId; // 广告ID
        public string language; // 语言
        public string versionName; // 版本名称
        public int versionCode; // 版本代码
    }

    public class SDKLoginResponse : SDKBaseResponse
    {
        public string token; // 2048个字符以内：一次性有效，用于游戏服务器登录鉴权验证
        public string ticket; // 256个字符以内：当前登录期内有效，用于周边系统票据验证，严禁用于登录鉴权
        public string userId; // 用户ID
        public int target; // 非 0 时指向游戏服务器，有两层作用：新用户时作为导流作用，帮助玩家优先选择对应服务器；老用户时作为指向玩家上次登录服务器 ID。
        public PlatformData data; // 游戏基础信息字段，内容不固定，将整个 data 字段上传至游戏服务器作为服务端打点的模板。
    }

    #endregion
}