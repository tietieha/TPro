using System;
using System.Collections;
using GameBase;
using UnityEngine;
using XLua;

namespace TEngine
{
    /// <summary>
    /// Lua模块。
    /// </summary>
    public sealed class LuaModule : DynamicModule
    {
        private const string kLUA_ROOT = "Assets/GameAssets/Lua";
        private const string kCONFIG_ROOT = "Assets/GameAssets/Config";
        public static string configTageName = "Tables.Module.";
        public static string configTageNameEnum = "Tables.Enum.";

        private LuaEnv m_LuaEnv;

        public LuaEnv Env
        {
            get { return m_LuaEnv; }
        }

        [CSharpCallLua]
        public delegate void LuaAction();

        [CSharpCallLua]
        public delegate void LuaActionUpdate(float deltaTime, float unscaledDeltaTime);

        [CSharpCallLua]
        public delegate void LuaActionFixedUpdate(float fixedDeltaTime);

        [CSharpCallLua]
        public delegate void LuaActionOnApplicationFocus(bool focus);

        private LuaAction m_LateUpdate;
        private LuaActionUpdate m_Update;
        private LuaActionFixedUpdate m_FixedUpdate;
        private LuaActionOnApplicationFocus m_OnApplicationFocus;
        private LuaAction m_OnApplicationQuit;
        private LuaAction m_doStringDebugFile;

        #region Module Life Circle

        // public override void Update(float elapsedTime, float realElapsedTime)
        private void Update()
        {
            m_Update?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // 加载并执行 Lua 脚本文件内容
                m_doStringDebugFile?.Invoke();
            }
#endif
        }

        private void LateUpdate()
        {
            m_LateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            m_FixedUpdate?.Invoke(GameTime.fixedDeltaTime);
        }

        private void OnApplicationFocus(bool focus)
        {
            m_OnApplicationFocus?.Invoke(focus);
        }

        private void OnApplicationQuit()
        {
            m_OnApplicationQuit?.Invoke();
        }

        #endregion

        #region XLua functions

        public LuaFunction GetFunction(string name)
        {
            return m_LuaEnv.CustomGlobal.Get<LuaFunction>(name);
        }

        public LuaTable GetTable(string name)
        {
            return m_LuaEnv.CustomGlobal.Get<LuaTable>(name);
        }

        public T GetGlobal<T>(string name)
        {
            return m_LuaEnv.Global.Get<T>(name);
        }

        /// <summary>
        /// todo: 缓存高频函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        public void CallGlobalFunction(string funcName, string args)
        {
            var luaFunction = m_LuaEnv.CustomGlobal.Get<Action<string>>(funcName);
            luaFunction.Invoke(args);
        }

        public void CallGlobalFunction(string funcName, string arg1, string arg2)
        {
            var luaFunction = m_LuaEnv.CustomGlobal.Get<Action<string, string>>(funcName);
            luaFunction.Invoke(arg1, arg2);
        }

        public void CallGlobalFunction(string funcName, object[] args)
        {
            var luaFunction = m_LuaEnv.CustomGlobal.Get<Action<object[]>>(funcName);
            luaFunction.Invoke(args);
        }

        #endregion


        private byte[] MyLoader(ref string filePath)
        {
            if (filePath == "emmy_core")
            {
                Log.Debug("pass lua debug" + filePath);
                return null;
            }

            string fullPath = filePath.Replace(".", "/");
            bool isDataTable = filePath.Contains(configTageName) ||
                               filePath.Contains(configTageNameEnum);
            if (isDataTable)
            {
                if (GameModule.Resource.ConfigMode == ConfigMode.Sandbox)
                {
                    var sandboxPath = $"{SettingsUtils.ConfigSandBoxFolder}/{fullPath}.lua.txt";
                    if (System.IO.File.Exists(sandboxPath))
                    {
                        return System.IO.File.ReadAllBytes(sandboxPath);
                    }
                }

#if UNITY_EDITOR
                fullPath = $"{kCONFIG_ROOT}/{fullPath}.lua.txt";
                var scriptText = Utility.File.ReadAssetsBytes(fullPath);
                return scriptText;
#else
                fullPath = $"{SettingsUtils.ConfigStreamingAssetsFolder}/{fullPath}.lua.txt";
                return Utility.File.ReadStreamingAssetsBytes(fullPath);
#endif
            }
            else
            {
                fullPath = $"{kLUA_ROOT}/{fullPath}.lua.txt";
                filePath = fullPath;
                if (!GameModule.Resource.CheckLocationValid(fullPath))
                    return null;
                var scriptText = GameModule.Resource.LoadAsset<TextAsset>(fullPath);
                var bytes = scriptText.bytes;
                GameModule.Resource.UnloadAsset(scriptText);
                return bytes;
            }
        }

        public override void Init()
        {
            m_LuaEnv = new LuaEnv();
            LuaArrAccessAPI.RegisterPinFunc(m_LuaEnv.L);
            m_LuaEnv.AddLoader(MyLoader);
        }

        public override void Start()
        {
            // 等一帧
            Utility.Unity.StartCoroutine(_startLua());
        }

        public void OnStart(Action complete = null)
        {
            _startLua();
            complete?.Invoke();
        }

        public void DoString(string doScript)
        {
            if (string.IsNullOrEmpty(doScript))
            {
                Log.Error("LuaManager.DoString args is null or empty!");
                return;
            }

            if (m_LuaEnv == null)
            {
                Log.Error("LuaManager.DoString m_LuaEnv is null!");
                return;
            }

            m_LuaEnv.DoString(doScript);
        }

        private IEnumerator _startLua()
        {
            yield return UnityConstans.waitForEndOfFrame;
            m_LuaEnv.DoString("require 'Main'");
            m_LuaEnv.Global.Get<Action>("Start").Invoke();

            m_LuaEnv.Global.Get("Update", out m_Update);
            m_LuaEnv.Global.Get("LateUpdate", out m_LateUpdate);
            m_LuaEnv.Global.Get("FixedUpdate", out m_FixedUpdate);
            m_LuaEnv.Global.Get("OnApplicationFocus", out m_OnApplicationFocus);
            m_LuaEnv.Global.Get("OnApplicationQuit", out m_OnApplicationQuit);
            m_LuaEnv.Global.Get("OnDoStringDebugFile", out m_doStringDebugFile);
        }


        // 函数调用环境
        public struct CallContext
        {
            public string funcName;
            public LuaTable table;
            public LuaFunction function;
            public int oldTop;
            public int errFunc;
            public int paramCount;
        }

        // 获取到table对象
        private static bool GetTableObject(string tableName, string funcName, out LuaTable table,
            out string newFuncName)
        {
            table = null;
            newFuncName = funcName;

            LuaEnv env = GameApp.Lua.Env;
            if (env == null)
            {
                Log.Info(" LuaManager.Instance.LuaEnv is null !");
                return false;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                int dotIndex = funcName.LastIndexOf('.');
                if (dotIndex >= 0)
                {
                    tableName = funcName.Substring(0, dotIndex);
                    newFuncName = funcName.Substring(dotIndex + 1);
                }
            }

            // 如果tableName有值的话，就直接获取table实体
            if (!string.IsNullOrEmpty(tableName))
            {
                table = env.CustomGlobal.Get<LuaTable>(tableName);
            }

            return true;
        }

        // 预备调用；调用此函数后不能随意再操作lua栈
        private static CallContext PrepareCall(LuaTable table, string funcName)
        {
            CallContext con = new CallContext();

            LuaEnv env = GameApp.Lua.Env;
            if (env == null)
            {
                Log.Info(" LuaManager.Instance.LuaEnv is null !");
                return con;
            }

            LuaFunction func = null;

            // table = null表示是全局函数
            if (table != null)
            {
                func = table.Get<LuaFunction>(funcName);
            }
            else
            {
                func = env.Global.Get<LuaFunction>(funcName);
            }

            con.table = table;
            con.function = func;

            if (func != null)
            {
                int oldTop;
                int errFunc;
                func.Prepare(out oldTop, out errFunc);

                con.funcName = funcName;
                con.oldTop = oldTop;
                con.errFunc = errFunc;

                // 如果有table的话，需要把table穿第一个参数，否则在lua中self会丢失。
                // 这个地方的处理其实不是很友善，因为如此处理之后，PrepareCall调用之后只能push，不能做其他操作了。
                if (table != null)
                {
                    PushAny(ref con, table);
                }
            }

            return con;
        }

        private static void PushAny(ref CallContext context, object obj)
        {
            context.function.PushAny(obj);
            context.paramCount++;
            return;
        }

        private static void PushInteger(ref CallContext context, int n)
        {
            context.function.PushInteger(n);
            context.paramCount++;
            return;
        }

        private static void PushInteger64(ref CallContext context, long n)
        {
            context.function.PushInteger64(n);
            context.paramCount++;
            return;
        }

        private static void PushDouble(ref CallContext context, double d)
        {
            context.function.PushNumber(d);
            context.paramCount++;
            return;
        }

        // 结束调用
        private static T EndCall<T>(CallContext context)
        {
            object ret =
                context.function.EndCall(context.oldTop, context.errFunc, context.paramCount);
            if (ret != null)
            {
                if (ret is LuaTable)
                {
                    LuaTable retTab = ret as LuaTable;
                    return retTab.Cast<T>();
                }
                else
                {
                    return (T)Convert.ChangeType(ret, typeof(T));
                }
            }
            else
            {
                return default;
            }
        }

        // 从下面开始枚举所有的lua调用处理，为了不要进行GC，所以枚举出大部分函数类型！

        // none
        public static T CallLuaFunc<T>(string tableName, string funcName)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName)
        {
            CallLuaFunc<object>(tableName, funcName);
        }

        public static void CallLuaFunc(LuaTable table, string funcName)
        {
            CallLuaFunc<object>(table, funcName);
        }

        // int
        public static T CallLuaFunc<T>(string tableName, string funcName, int p1)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, int p1)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger(ref con, p1);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, int p1)
        {
            CallLuaFunc<object>(tableName, funcName, p1);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, int p1)
        {
            CallLuaFunc<object>(table, funcName, p1);
        }

        // int, int
        public static T CallLuaFunc<T>(string tableName, string funcName, int p1, int p2)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, int p1, int p2)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger(ref con, p1);
            PushInteger(ref con, p2);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, int p1, int p2)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, int p1, int p2)
        {
            CallLuaFunc<object>(table, funcName, p1, p2);
        }

        // int, int, int
        public static T CallLuaFunc<T>(string tableName, string funcName, int p1, int p2, int p3)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2, p3);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, int p1, int p2, int p3)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger(ref con, p1);
            PushInteger(ref con, p2);
            PushInteger(ref con, p3);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, int p1, int p2, int p3)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2, p3);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, int p1, int p2, int p3)
        {
            CallLuaFunc<object>(table, funcName, p1, p2, p3);
        }

        // double
        public static T CallLuaFunc<T>(string tableName, string funcName, double p1)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, double p1)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushDouble(ref con, p1);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, double p1)
        {
            CallLuaFunc<object>(tableName, funcName, p1);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, double p1)
        {
            CallLuaFunc<object>(table, funcName, p1);
        }

        // double, double
        public static T CallLuaFunc<T>(string tableName, string funcName, double p1, double p2)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, double p1, double p2)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushDouble(ref con, p1);
            PushDouble(ref con, p2);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, double p1, double p2)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, double p1, double p2)
        {
            CallLuaFunc<object>(table, funcName, p1, p2);
        }

        // double, double, double
        public static T CallLuaFunc<T>(string tableName, string funcName, double p1, double p2,
            double p3)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2, p3);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, double p1, double p2,
            double p3)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushDouble(ref con, p1);
            PushDouble(ref con, p2);
            PushDouble(ref con, p3);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, double p1, double p2,
            double p3)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2, p3);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, double p1, double p2,
            double p3)
        {
            CallLuaFunc<object>(table, funcName, p1, p2, p3);
        }

        // string
        public static T CallLuaFunc<T>(string tableName, string funcName, string p1)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, string p1)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushAny(ref con, p1);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, string p1)
        {
            CallLuaFunc<object>(tableName, funcName, p1);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, string p1)
        {
            CallLuaFunc<object>(table, funcName, p1);
        }

        // string, string
        public static T CallLuaFunc<T>(string tableName, string funcName, string p1, string p2)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, string p1, string p2)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushAny(ref con, p1);
            PushAny(ref con, p2);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, string p1, string p2)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, string p1, string p2)
        {
            CallLuaFunc<object>(table, funcName, p1, p2);
        }

        // string, string, string
        public static T CallLuaFunc<T>(string tableName, string funcName, string p1, string p2,
            string p3)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2, p3);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, string s1, string s2,
            string s3)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushAny(ref con, s1);
            PushAny(ref con, s2);
            PushAny(ref con, s3);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, string p1, string p2,
            string p3)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2, p3);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, string p1, string p2,
            string p3)
        {
            CallLuaFunc<object>(table, funcName, p1, p2, p3);
        }

        // string, int
        public static T CallLuaFunc<T>(string tableName, string funcName, string p1, int p2)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, string p1, int p2)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushAny(ref con, p1);
            PushInteger(ref con, p2);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, string p1, int p2)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, string p1, int p2)
        {
            CallLuaFunc<object>(table, funcName, p1, p2);
        }

        // int, string
        public static T CallLuaFunc<T>(string tableName, string funcName, int p1, string p2)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, int p1, string p2)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger(ref con, p1);
            PushAny(ref con, p2);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, int p1, string p2)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, int p1, string p2)
        {
            CallLuaFunc<object>(table, funcName, p1, p2);
        }

        // 其他未归类
        public static T CallLuaFunc<T>(string tableName, string funcName, params object[] args)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, args);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, params object[] args)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                PushAny(ref con, args[i]);
            }

            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, params object[] args)
        {
            CallLuaFunc<object>(tableName, funcName, args);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, params object[] args)
        {
            CallLuaFunc<object>(table, funcName, args);
        }

        //获取lua表结构得属性值
        public static T GetLuaAttr<T>(LuaTable tabHandler, string attrName)
        {
            return tabHandler.Get<T>(attrName);
        }

        //////////////////////long
        // long
        public static T CallLuaFunc<T>(string tableName, string funcName, long p1)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, long p1)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger64(ref con, p1);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, long p1)
        {
            CallLuaFunc<object>(tableName, funcName, p1);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, long p1)
        {
            CallLuaFunc<object>(table, funcName, p1);
        }

        // int, int
        public static T CallLuaFunc<T>(string tableName, string funcName, long p1, long p2)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, long p1, long p2)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger64(ref con, p1);
            PushInteger64(ref con, p2);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, long p1, long p2)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, long p1, long p2)
        {
            CallLuaFunc<object>(table, funcName, p1, p2);
        }

        // int, int, int
        public static T CallLuaFunc<T>(string tableName, string funcName, long p1, long p2, long p3)
        {
            LuaTable table;
            string newFuncName;
            GetTableObject(tableName, funcName, out table, out newFuncName);
            return CallLuaFunc<T>(table, newFuncName, p1, p2, p3);
        }

        public static T CallLuaFunc<T>(LuaTable table, string funcName, long p1, long p2, long p3)
        {
            CallContext con = PrepareCall(table, funcName);
            if (con.function == null)
            {
                return default;
            }

            PushInteger64(ref con, p1);
            PushInteger64(ref con, p2);
            PushInteger64(ref con, p3);
            return EndCall<T>(con);
        }

        public static void CallLuaFunc(string tableName, string funcName, long p1, long p2, long p3)
        {
            CallLuaFunc<object>(tableName, funcName, p1, p2, p3);
        }

        public static void CallLuaFunc(LuaTable table, string funcName, long p1, long p2, long p3)
        {
            CallLuaFunc<object>(table, funcName, p1, p2, p3);
        }
    }
}