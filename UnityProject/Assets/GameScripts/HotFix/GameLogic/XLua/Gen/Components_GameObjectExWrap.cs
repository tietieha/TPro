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
    public class ComponentsGameObjectExWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Components.GameObjectEx);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetVisible", _m_SetVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPosition", _m_SetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosition", _m_GetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLocalScale", _m_SetLocalScale);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLocalScale", _m_GetLocalScale);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLocalPosition", _m_SetLocalPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLocalPosition", _m_GetLocalPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetRotation", _m_SetRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRotation", _m_GetRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLocalRotation", _m_SetLocalRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLocalRotation", _m_GetLocalRotation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToReset", _m_ToReset);
			
			
			
			
			
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
					
					var gen_ret = new Components.GameObjectEx();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Components.GameObjectEx constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _b = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetVisible( _b );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetPosition( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPosition(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLocalScale(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetLocalScale( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalScale(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetLocalScale(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLocalPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetLocalPosition( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetLocalPosition(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    float _w = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetRotation( _x, _y, _z, _w );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetRotation(  );
                        translator.PushUnityEngineQuaternion(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLocalRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    float _w = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetLocalRotation( _x, _y, _z, _w );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalRotation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetLocalRotation(  );
                        translator.PushUnityEngineQuaternion(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToReset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Components.GameObjectEx gen_to_be_invoked = (Components.GameObjectEx)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ToReset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
