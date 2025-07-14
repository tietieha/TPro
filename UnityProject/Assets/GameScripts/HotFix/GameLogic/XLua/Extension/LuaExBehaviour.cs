using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace XLua
{
    /// <summary>
    /// lua脚本生命周期
    /// </summary>
    public class LuaExBehaviour : MonoBehaviour
    {
        public LuaTable m_LuaTable{get;private set;}
        private LuaFunction luaFunc_Start;
        private LuaFunction luaFunc_OnDispose;
        private static Object s_Null = null;

        public static LuaTable Add(GameObject go, LuaTable luaTable)
        {
            var new_func = LuaExBehaviour.FindLuaFunction(luaTable, "new");
            if(new_func == null)
            {
                Log.Error($"添加lua组件出错,lua模块未包含New方法 GameObject{go.name}");
                return null;
            }
            object[] rets = LuaExBehaviour.CallLuaFunction(luaTable, new_func);
            if (rets.Length != 1) {
                Log.Error($"添加lua组件出错,new函数必须返回一个table数据作为lua模块的对象  GameObject{go.name}");
                return null;
            }

            var behaviour = go.GetComponent<LuaExBehaviour>();
            if(behaviour != null)
            {
                Log.Error($"{go.name} 没有删除 < LuaExBehaviour > 组件");
                behaviour.ClearLuaEx();
            }
            else
            {
                behaviour = go.AddComponent<LuaExBehaviour>();
            }

            LuaTable newLuaTable = rets[0] as LuaTable;
            var mLuaBehaviour = go.GetComponent<LuaBehaviour>();
            if(mLuaBehaviour != null)
            {
                mLuaBehaviour.Eject();
                mLuaBehaviour.Inject(newLuaTable);
            }
            
            newLuaTable.Set("gameObject", go);
            newLuaTable.Set("transform", go.transform);
            var luaFunc_onCreate = LuaExBehaviour.FindLuaFunction(newLuaTable, "onCreate");
            if (luaFunc_onCreate != null)
            {
                LuaExBehaviour.CallLuaFunction(newLuaTable, luaFunc_onCreate);
            }
            behaviour.InitLuaFunc(newLuaTable);

            return newLuaTable;
        }

        void InitLuaFunc(LuaTable luaTable)
        {
            m_LuaTable = luaTable;
            if(m_LuaTable == null) return;
            
            var lua_Awake_func = LuaExBehaviour.FindLuaFunction(m_LuaTable, "Awake");
            luaFunc_Start = LuaExBehaviour.FindLuaFunction(m_LuaTable, "Start");
            luaFunc_OnDispose = LuaExBehaviour.FindLuaFunction(m_LuaTable,"onDispose");
            OneObjDelegate onDisposeDel = onDispose;
            m_LuaTable.Set("_dispose", onDisposeDel);
            
            //m_LuaTable.Set("lua_component", this);
            LuaExBehaviour.CallLuaFunction(m_LuaTable, lua_Awake_func);
        }

        void Start()
        {
            LuaExBehaviour.CallLuaFunction(m_LuaTable, luaFunc_Start);
        }

        void onDispose(object userdata)
        {                
            LuaExBehaviour.CallLuaFunction(m_LuaTable, luaFunc_OnDispose);
            ClearLuaEx();
            var behaviour = gameObject.GetComponent<LuaExBehaviour>();
            if(behaviour != null)
            {
                GameObject.DestroyImmediate(behaviour);
            }
        }

        void OnDestroy()
        {
            ClearLuaEx();
        }

        private void ClearLuaEx()
        { 
            if(m_LuaTable != null)
            {
                m_LuaTable.Set("gameObject", s_Null);
                m_LuaTable.Set("transform", s_Null);
                m_LuaTable.Set("_dispose", s_Null);
            }
            var mLuaBehaviour = gameObject.GetComponent<LuaBehaviour>();
            if(mLuaBehaviour != null)
            {
                mLuaBehaviour.Eject();
            }
            m_LuaTable = null;
            luaFunc_Start = null;
            luaFunc_OnDispose = null;
        }

        #region 静态方法
        
        private static LuaFunction FindLuaFunction(LuaTable luaTable, string funcName)
        {
            LuaFunction func = luaTable.Get<LuaFunction>(funcName);
            if(func != null)
            {
                return func;
            }
            return null;
        }

        private static object[] CallLuaFunction(LuaTable luaTable, LuaFunction func,params object[] userdata)
        {
            if(func != null)
            {
                return func.Call(luaTable, userdata);
            }
            return null;
        }
        #endregion
        
    }
}
