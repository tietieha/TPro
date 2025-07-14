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
    public class EventComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(EventComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Check", _m_Check);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Subscribe", _m_Subscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unsubscribe", _m_Unsubscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetDefaultHandler", _m_SetDefaultHandler);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Fire", _m_Fire);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FireFromLua", _m_FireFromLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FireNow", _m_FireNow);
			
			
			
			
			
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
					
					var gen_ret = new EventComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to EventComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Check(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    System.EventHandler<GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameEventArgs>>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Check( _id, _handler );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Subscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.EventHandler<GameEventArgs>>(L, 3)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    System.EventHandler<GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameEventArgs>>(L, 3);
                    
                    gen_to_be_invoked.Subscribe( _id, _handler );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<EventId>(L, 2)&& translator.Assignable<System.EventHandler<GameEventArgs>>(L, 3)) 
                {
                    EventId _id;translator.Get(L, 2, out _id);
                    System.EventHandler<GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameEventArgs>>(L, 3);
                    
                    gen_to_be_invoked.Subscribe( _id, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EventComponent.Subscribe!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unsubscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.EventHandler<GameEventArgs>>(L, 3)) 
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    System.EventHandler<GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameEventArgs>>(L, 3);
                    
                    gen_to_be_invoked.Unsubscribe( _id, _handler );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<EventId>(L, 2)&& translator.Assignable<System.EventHandler<GameEventArgs>>(L, 3)) 
                {
                    EventId _id;translator.Get(L, 2, out _id);
                    System.EventHandler<GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameEventArgs>>(L, 3);
                    
                    gen_to_be_invoked.Unsubscribe( _id, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EventComponent.Unsubscribe!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetDefaultHandler(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.EventHandler<GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameEventArgs>>(L, 2);
                    
                    gen_to_be_invoked.SetDefaultHandler( _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Fire(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    GameEventArgs _e = (GameEventArgs)translator.GetObject(L, 3, typeof(GameEventArgs));
                    
                    gen_to_be_invoked.Fire( _sender, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FireFromLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    GameEventArgs _e = (GameEventArgs)translator.GetObject(L, 3, typeof(GameEventArgs));
                    
                    gen_to_be_invoked.FireFromLua( _sender, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FireNow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EventComponent gen_to_be_invoked = (EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    GameEventArgs _e = (GameEventArgs)translator.GetObject(L, 3, typeof(GameEventArgs));
                    
                    gen_to_be_invoked.FireNow( _sender, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
