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
    public class UWHoldEventTriggerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UW.HoldEventTrigger);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerDown", _m_OnPointerDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerUp", _m_OnPointerUp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerExit", _m_OnPointerExit);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "HoldEvent", _g_get_HoldEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DelayEvent", _g_get_DelayEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PressDownEvent", _g_get_PressDownEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PressUpEvent", _g_get_PressUpEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsRepeatTrigger", _g_get_IsRepeatTrigger);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "HoldEvent", _s_set_HoldEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DelayEvent", _s_set_DelayEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PressDownEvent", _s_set_PressDownEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PressUpEvent", _s_set_PressUpEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsRepeatTrigger", _s_set_IsRepeatTrigger);
            
			
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
					
					var gen_ret = new UW.HoldEventTrigger();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UW.HoldEventTrigger constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerDown( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerUp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerUp( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerExit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerExit( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HoldEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.HoldEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DelayEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DelayEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PressDownEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PressDownEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PressUpEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PressUpEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsRepeatTrigger(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsRepeatTrigger);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HoldEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HoldEvent = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 2, typeof(UnityEngine.Events.UnityEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DelayEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DelayEvent = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 2, typeof(UnityEngine.Events.UnityEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PressDownEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PressDownEvent = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 2, typeof(UnityEngine.Events.UnityEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PressUpEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PressUpEvent = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 2, typeof(UnityEngine.Events.UnityEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsRepeatTrigger(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UW.HoldEventTrigger gen_to_be_invoked = (UW.HoldEventTrigger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsRepeatTrigger = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
