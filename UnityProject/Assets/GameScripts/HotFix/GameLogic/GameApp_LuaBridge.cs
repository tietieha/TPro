using GameBase;
using TEngine;

public partial class GameApp : Singleton<GameApp>
{
    // 连接Aot和Lua
    public static void CallLuaGlobal(string funcName, string arg1)
    {
        if (Lua == null)
        {
            Log.Error("Lua is null, cannot call Lua global function: {0}", funcName);
            return;
        }

        Lua.CallGlobalFunction(funcName, arg1);
    }

    public static void CallLuaGlobal(string funcName, string arg1, string arg2)
    {
        if (Lua == null)
        {
            Log.Error("Lua is null, cannot call Lua global function: {0}", funcName);
            return;
        }

        Lua.CallGlobalFunction(funcName, arg1, arg2);
    }

    public static void CallLuaGlobal(string funcName, object[] args)
    {
        if (Lua == null)
        {
            Log.Error("Lua is null, cannot call Lua global function: {0}", funcName);
            return;
        }

        Lua.CallGlobalFunction(funcName, args);
    }
}