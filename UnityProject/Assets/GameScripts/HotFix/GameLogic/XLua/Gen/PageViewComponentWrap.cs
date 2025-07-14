﻿#if USE_UNI_LUA
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
    public class PageViewComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(PageViewComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Refresh", _m_Refresh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "pageTo", _m_pageTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBeginDrag", _m_OnBeginDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDrag", _m_OnEndDrag);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnPageChanged", _g_get_OnPageChanged);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "smooting", _g_get_smooting);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sensitivity", _g_get_sensitivity);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnPageChanged", _s_set_OnPageChanged);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "smooting", _s_set_smooting);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sensitivity", _s_set_sensitivity);
            
			
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
					
					var gen_ret = new PageViewComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to PageViewComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Refresh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Refresh(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_pageTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.pageTo( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBeginDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnBeginDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEndDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnEndDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnPageChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnPageChanged);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_smooting(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.smooting);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.sensitivity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnPageChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnPageChanged = translator.GetDelegate<System.Action<int>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_smooting(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.smooting = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PageViewComponent gen_to_be_invoked = (PageViewComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.sensitivity = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
