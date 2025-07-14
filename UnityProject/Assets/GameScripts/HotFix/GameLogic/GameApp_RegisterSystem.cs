using System.Collections.Generic;
using GameBase;
using GameLogic;
using TEngine;
using TMPro;

public partial class GameApp : Singleton<GameApp>
{
    #region Modules
    /// <summary>
    /// Lua
    /// </summary>
    private static LuaModule _luaModule;
    public static LuaModule Lua => _luaModule ??= GameModule.Get<LuaModule>();

    /// <summary>
    /// Socket
    /// </summary>
    private static SocketManager _socketManager;
    public static SocketManager Socket => _socketManager ??= GameModule.Get<SocketManager>();

    /// <summary>
    /// Event
    /// </summary>
    private static EventModule _eventModule;
    public static EventModule EventModule => _eventModule ??= GameModule.Get<EventModule>();

    private static EventComponent eventCom;
    public static EventComponent Event
    {
        get
        {
            if (_eventModule == null)
            {
                _eventModule = GameModule.Get<EventModule>();
                if (_eventModule != null)
                {
                    eventCom = _eventModule.GetEventComponent();
                }
            }

            return eventCom;
        }
    }
    #endregion

    private List<ILogicSys> _listLogicMgr;

    public override void Active()
    {
        RegisterDynamicModule();
        CodeTypes.Instance.Init(_hotfixAssembly.ToArray());
        EventInterfaceHelper.Init();
        _listLogicMgr = new List<ILogicSys>();
        RegisterAllSystem();
        InitSystemSetting();
    }

    /// <summary>
    /// 注册动态模块。
    /// </summary>
    private void RegisterDynamicModule()
    {
        ModuleSystem.RegisterDynamicModule<SocketManager>();
        ModuleSystem.RegisterDynamicModule<EventModule>();
        ModuleSystem.RegisterDynamicModule<LuaModule>();
        // ModuleSystem.RegisterDynamicModule<CoroutineComponent>();
    }

    /// <summary>
    /// 设置一些通用的系统属性。
    /// </summary>
    private void InitSystemSetting()
    {
        var asset = GameModule.Resource.LoadAsset<UITextStyleAsset>(GameDefines.UITextAssetPath);
        if (asset != null)
        {
            TMPRenderParameter.SetTextStyleAsset(asset);
        }
    }

    /// <summary>
    /// 注册所有逻辑系统
    /// </summary>
    private void RegisterAllSystem()
    {
        //带生命周期的单例系统。
        AddLogicSys(BehaviourSingleSystem.Instance);
    }

    /// <summary>
    /// 注册逻辑系统。
    /// </summary>
    /// <param name="logicSys">ILogicSys</param>
    /// <returns></returns>
    private bool AddLogicSys(ILogicSys logicSys)
    {
        if (_listLogicMgr.Contains(logicSys))
        {
            Log.Fatal("Repeat add logic system: {0}", logicSys.GetType().Name);
            return false;
        }

        if (!logicSys.OnInit())
        {
            Log.Fatal("{0} Init failed", logicSys.GetType().Name);
            return false;
        }

        _listLogicMgr.Add(logicSys);

        return true;
    }
}