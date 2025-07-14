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
    public class TEngineMaterialExtensionMaterialMgrWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(TEngine.MaterialExtension.MaterialMgr);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeMat", _m_ChangeMat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeMatColorProperty", _m_ChangeMatColorProperty);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChildrenList", _g_get_ChildrenList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TestKey", _g_get_TestKey);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ChildrenList", _s_set_ChildrenList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TestKey", _s_set_TestKey);
            
			
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
					
					var gen_ret = new TEngine.MaterialExtension.MaterialMgr();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.MaterialExtension.MaterialMgr constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeMat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.MaterialExtension.MaterialMgr gen_to_be_invoked = (TEngine.MaterialExtension.MaterialMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ChangeMat( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeMatColorProperty(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.MaterialExtension.MaterialMgr gen_to_be_invoked = (TEngine.MaterialExtension.MaterialMgr)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    string _propertyKey = LuaAPI.lua_tostring(L, 2);
                    float _targetAlpha = (float)LuaAPI.lua_tonumber(L, 3);
                    float _during = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _changeFinish = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.ChangeMatColorProperty( _propertyKey, _targetAlpha, _during, _changeFinish );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _propertyKey = LuaAPI.lua_tostring(L, 2);
                    float _targetAlpha = (float)LuaAPI.lua_tonumber(L, 3);
                    float _during = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.ChangeMatColorProperty( _propertyKey, _targetAlpha, _during );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.MaterialExtension.MaterialMgr.ChangeMatColorProperty!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChildrenList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.MaterialExtension.MaterialMgr gen_to_be_invoked = (TEngine.MaterialExtension.MaterialMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ChildrenList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TestKey(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.MaterialExtension.MaterialMgr gen_to_be_invoked = (TEngine.MaterialExtension.MaterialMgr)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.TestKey);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChildrenList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.MaterialExtension.MaterialMgr gen_to_be_invoked = (TEngine.MaterialExtension.MaterialMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ChildrenList = (System.Collections.Generic.List<TEngine.MaterialExtension.MaterialRenderChild>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<TEngine.MaterialExtension.MaterialRenderChild>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TestKey(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.MaterialExtension.MaterialMgr gen_to_be_invoked = (TEngine.MaterialExtension.MaterialMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TestKey = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
