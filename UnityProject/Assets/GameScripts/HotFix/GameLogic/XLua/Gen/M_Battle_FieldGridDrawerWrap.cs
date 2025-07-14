#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLuaBase.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MBattleFieldGridDrawerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.Battle.FieldGridDrawer);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetGridData", _m_SetGridData);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "gridSize", _g_get_gridSize);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "gridSize", _s_set_gridSize);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new M.Battle.FieldGridDrawer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.FieldGridDrawer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGridData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.FieldGridDrawer gen_to_be_invoked = (M.Battle.FieldGridDrawer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<UnityEngine.Vector3> _positions = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
                    System.Collections.Generic.List<int> _ids = (System.Collections.Generic.List<int>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<int>));
                    float _newGridSize = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetGridData( _positions, _ids, _newGridSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.Battle.FieldGridDrawer gen_to_be_invoked = (M.Battle.FieldGridDrawer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.gridSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.Battle.FieldGridDrawer gen_to_be_invoked = (M.Battle.FieldGridDrawer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.gridSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
