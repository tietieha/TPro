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
    public class GameLogicSimpleToggleWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLogic.SimpleToggle);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetState", _m_SetState);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Active", _g_get_Active);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Inactive", _g_get_Inactive);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Active", _s_set_Active);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Inactive", _s_set_Inactive);
            
			
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
					
					var gen_ret = new GameLogic.SimpleToggle();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLogic.SimpleToggle constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLogic.SimpleToggle gen_to_be_invoked = (GameLogic.SimpleToggle)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _select = LuaAPI.xlua_tointeger(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    System.Action<int> _selectToggleEvent = translator.GetDelegate<System.Action<int>>(L, 4);
                    
                    gen_to_be_invoked.Init( _select, _index, _selectToggleEvent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLogic.SimpleToggle gen_to_be_invoked = (GameLogic.SimpleToggle)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _state = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetState( _state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.SimpleToggle gen_to_be_invoked = (GameLogic.SimpleToggle)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Active);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Inactive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.SimpleToggle gen_to_be_invoked = (GameLogic.SimpleToggle)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Inactive);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.SimpleToggle gen_to_be_invoked = (GameLogic.SimpleToggle)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Active = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Inactive(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.SimpleToggle gen_to_be_invoked = (GameLogic.SimpleToggle)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Inactive = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
