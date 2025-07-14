using XLua;

[LuaCallCSharp]
public enum EventId
{
    None = 0,
    TestEvent,
    //网络模块
    NETWORK_SOCKET_CONNECT_SUCCESS,//登录成功（cs->lua）
    NETWORK_SOCKET_CONNECT_FAILURE,//断开连接（cs->lua）
    NETWORK_FORCE_HEART_BEAT,//切回前台强制心跳（cs->lua）

    RETURN_LOGIN,//返回登录
    
    DEBUG_CONNECT_GAMESERVER,   //链接测试服

    //本地化加载
    LOCALIZATION_LOAD_SUCCESS, //本地化加载成功


    TZ_Camera_Moved,
    TZ_Camera_Move_End,
    TZ_Camera_Change_View,
    TZ_Camera_Change_Refresh_Ship,

    OPEN_UI_MAIN_VIEW,
    GUIDE_CLOSE_GUIDE_MAIN_VIEW,     //暂时留着没用
    GUIDE_REMOVE_GUIDE_WEAK,
    GUIDE_OPEN_UIWIDGET_STARTGUIDE_CS,
    
    THRONE_APPEAR_FINISH,
}
 