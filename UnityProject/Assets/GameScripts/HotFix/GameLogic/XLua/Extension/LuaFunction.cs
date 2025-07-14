// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-12-11       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using LuaAPI = XLua.LuaDLL.Lua;

namespace XLua
{
    public partial class LuaFunction : LuaBase
    {
        public void Prepare(out int oldTop, out int errFunc)
        {
            int nArgs = 0;
            var L = luaEnv.L;
            /*int*/
            oldTop = LuaAPI.lua_gettop(L);
            /*int*/
            errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);

            LuaAPI.lua_getref(L, luaReference);
        }

        public void PushAny(object obj)
        {
            luaEnv.translator.PushAny(luaEnv.L, obj);
        }

        public void PushInteger(int i)
        {
            LuaAPI.xlua_pushinteger(luaEnv.L, i);
        }
        public void PushInteger64(long i)
        {
            LuaAPI.lua_pushint64(luaEnv.L, i);
        }

        public void PushNumber(double d)
        {
            LuaAPI.lua_pushnumber(luaEnv.L, d);
        }

        public object EndCall(int oldTop, int errFunc, int args)
        {
            var L = luaEnv.L;
            var translator = luaEnv.translator;

            int error = LuaAPI.lua_pcall(L, args, -1, errFunc);
            if (error != 0)
            {
                try
                {
                    luaEnv.ThrowExceptionFromError(oldTop);
                }
                catch (LuaException e)
                {
                    return default;
                }
            }

            LuaAPI.lua_remove(L, errFunc);
            return translator.PopValues2(L, oldTop);
        }

        public void Action<T1, T2, T3>(T1 a1, T2 a2, T3 a3)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                var L = luaEnv.L;
                var translator = luaEnv.translator;
                int oldTop = LuaAPI.lua_gettop(L);
                int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
                LuaAPI.lua_getref(L, luaReference);
                translator.PushByType(L, a1);
                translator.PushByType(L, a2);
                translator.PushByType(L, a3);
                int error = LuaAPI.lua_pcall(L, 3, 0, errFunc);
                if (error != 0)
                    luaEnv.ThrowExceptionFromError(oldTop);
                LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }

        public void Action<T1, T2, T3, T4>(T1 a1, T2 a2, T3 a3, T4 a4)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                var L = luaEnv.L;
                var translator = luaEnv.translator;
                int oldTop = LuaAPI.lua_gettop(L);
                int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
                LuaAPI.lua_getref(L, luaReference);
                translator.PushByType(L, a1);
                translator.PushByType(L, a2);
                translator.PushByType(L, a3);
                translator.PushByType(L, a4);
                int error = LuaAPI.lua_pcall(L, 4, 0, errFunc);
                if (error != 0)
                    luaEnv.ThrowExceptionFromError(oldTop);
                LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }

        public void Action<T1, T2, T3, T4, T5>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                var L = luaEnv.L;
                var translator = luaEnv.translator;
                int oldTop = LuaAPI.lua_gettop(L);
                int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
                LuaAPI.lua_getref(L, luaReference);
                translator.PushByType(L, a1);
                translator.PushByType(L, a2);
                translator.PushByType(L, a3);
                translator.PushByType(L, a4);
                translator.PushByType(L, a5);
                int error = LuaAPI.lua_pcall(L, 5, 0, errFunc);
                if (error != 0)
                    luaEnv.ThrowExceptionFromError(oldTop);
                LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }

        public void Action<T1, T2, T3, T4, T5, T6>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                var L = luaEnv.L;
                var translator = luaEnv.translator;
                int oldTop = LuaAPI.lua_gettop(L);
                int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
                LuaAPI.lua_getref(L, luaReference);
                translator.PushByType(L, a1);
                translator.PushByType(L, a2);
                translator.PushByType(L, a3);
                translator.PushByType(L, a4);
                translator.PushByType(L, a5);
                translator.PushByType(L, a6);
                int error = LuaAPI.lua_pcall(L, 6, 0, errFunc);
                if (error != 0)
                    luaEnv.ThrowExceptionFromError(oldTop);
                LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }

        public TResult Func<T1, T2, T3, TResult>(T1 a1, T2 a2, T3 a3)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                var L = luaEnv.L;
                var translator = luaEnv.translator;
                int oldTop = LuaAPI.lua_gettop(L);
                int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
                LuaAPI.lua_getref(L, luaReference);
                translator.PushByType(L, a1);
                translator.PushByType(L, a2);
                translator.PushByType(L, a3);
                int error = LuaAPI.lua_pcall(L, 3, 1, errFunc);
                if (error != 0)
                    luaEnv.ThrowExceptionFromError(oldTop);
                TResult ret;
                try
                {
                    translator.Get(L, -1, out ret);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    LuaAPI.lua_settop(L, oldTop);
                }
                return ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }
    }
}