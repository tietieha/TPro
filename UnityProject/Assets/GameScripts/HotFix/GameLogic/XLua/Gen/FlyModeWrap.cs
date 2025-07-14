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
    public class FlyModeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FlyMode);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetValue", _m_SetValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFlyValue", _m_SetFlyValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreatePlay", _m_CreatePlay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IconInstance", _m_IconInstance);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QueueCall", _m_QueueCall);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextPlay", _m_NextPlay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Pause", _m_Pause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Stop", _m_Stop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Destory", _m_Destory);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_TargePrefab", _g_get_m_TargePrefab);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_trailRenderer", _g_get_m_trailRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "info", _g_get_info);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_isPause", _g_get_m_isPause);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_isCreatePlay", _g_get_m_isCreatePlay);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_TargePrefab", _s_set_m_TargePrefab);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_trailRenderer", _s_set_m_trailRenderer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "info", _s_set_info);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_isPause", _s_set_m_isPause);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_isCreatePlay", _s_set_m_isCreatePlay);
            
			
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
					
					var gen_ret = new FlyMode();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FlyMode constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& translator.Assignable<UnityEngine.UI.Image>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<System.Action>(L, 6)) 
                {
                    UnityEngine.Vector3 __starPosition;translator.Get(L, 2, out __starPosition);
                    UnityEngine.Vector2 __endPosition;translator.Get(L, 3, out __endPosition);
                    UnityEngine.UI.Image __icon = (UnityEngine.UI.Image)translator.GetObject(L, 4, typeof(UnityEngine.UI.Image));
                    int __count = LuaAPI.xlua_tointeger(L, 5);
                    System.Action __CallBack = translator.GetDelegate<System.Action>(L, 6);
                    
                    gen_to_be_invoked.SetValue( __starPosition, __endPosition, __icon, __count, __CallBack );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)&& translator.Assignable<UnityEngine.UI.Image>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.GameObject>(L, 6)&& translator.Assignable<System.Action>(L, 7)) 
                {
                    UnityEngine.Vector3 __starPosition;translator.Get(L, 2, out __starPosition);
                    UnityEngine.Vector2 __endPosition;translator.Get(L, 3, out __endPosition);
                    UnityEngine.UI.Image __icon = (UnityEngine.UI.Image)translator.GetObject(L, 4, typeof(UnityEngine.UI.Image));
                    int __count = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 6, typeof(UnityEngine.GameObject));
                    System.Action __CallBack = translator.GetDelegate<System.Action>(L, 7);
                    
                    gen_to_be_invoked.SetValue( __starPosition, __endPosition, __icon, __count, __object, __CallBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FlyMode.SetValue!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFlyValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 __starPosition;translator.Get(L, 2, out __starPosition);
                    UnityEngine.Vector3 __endPosition;translator.Get(L, 3, out __endPosition);
                    UnityEngine.UI.Image __icon = (UnityEngine.UI.Image)translator.GetObject(L, 4, typeof(UnityEngine.UI.Image));
                    int __count = LuaAPI.xlua_tointeger(L, 5);
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 6, typeof(UnityEngine.GameObject));
                    System.Action __CallBack = translator.GetDelegate<System.Action>(L, 7);
                    
                    gen_to_be_invoked.SetFlyValue( __starPosition, __endPosition, __icon, __count, __object, __CallBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Play(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreatePlay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Image>(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    System.Action __Callback = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.CreatePlay( __sprite, __Callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.UI.Image>(L, 2)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    
                    gen_to_be_invoked.CreatePlay( __sprite );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Image>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    System.Action __Callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.CreatePlay( __sprite, __object, __Callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Image>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.CreatePlay( __sprite, __object );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FlyMode.CreatePlay!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IconInstance(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Image>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    System.Action __callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.IconInstance( __sprite, __object, __callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Image>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.IconInstance( __sprite, __object );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.UI.Image>(L, 2)) 
                {
                    UnityEngine.UI.Image __sprite = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
                    
                    gen_to_be_invoked.IconInstance( __sprite );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FlyMode.IconInstance!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueueCall(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int __id = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.QueueCall( __id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextPlay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.NextPlay(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Pause(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Pause(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Stop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Destory(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Destory(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_TargePrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_TargePrefab);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_trailRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_trailRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_info(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.info);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_isPause(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_isPause);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_isCreatePlay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_isCreatePlay);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_TargePrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_TargePrefab = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_trailRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_trailRenderer = (UnityEngine.TrailRenderer)translator.GetObject(L, 2, typeof(UnityEngine.TrailRenderer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_info(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.info = (FlyTabelAsset)translator.GetObject(L, 2, typeof(FlyTabelAsset));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_isPause(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_isPause = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_isCreatePlay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyMode gen_to_be_invoked = (FlyMode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_isCreatePlay = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
