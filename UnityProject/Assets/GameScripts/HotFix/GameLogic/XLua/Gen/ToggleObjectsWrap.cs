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
    public class ToggleObjectsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ToggleObjects);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddListener", _m_AddListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveListener", _m_RemoveListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetOnValueChange", _m_SetOnValueChange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetValue", _m_SetValue);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "audioClipPath", _g_get_audioClipPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "toggle", _g_get_toggle);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "audioClipPath", _s_set_audioClipPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "toggle", _s_set_toggle);
            
			
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
					
					var gen_ret = new ToggleObjects();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ToggleObjects constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.AddListener(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RemoveListener(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetOnValueChange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<ToggleObjects> _onValueChange = translator.GetDelegate<System.Action<ToggleObjects>>(L, 2);
                    
                    gen_to_be_invoked.SetOnValueChange( _onValueChange );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _isOn = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetValue( _isOn );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_audioClipPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.audioClipPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_toggle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.toggle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_audioClipPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.audioClipPath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_toggle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ToggleObjects gen_to_be_invoked = (ToggleObjects)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.toggle = (UnityEngine.UI.Toggle)translator.GetObject(L, 2, typeof(UnityEngine.UI.Toggle));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
