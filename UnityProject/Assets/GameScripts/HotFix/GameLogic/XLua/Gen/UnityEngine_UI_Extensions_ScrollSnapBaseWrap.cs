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
    public class UnityEngineUIExtensionsScrollSnapBaseWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.UI.Extensions.ScrollSnapBase);
			Utils.BeginObjectRegister(type, L, translator, 0, 15, 25, 24);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetEnableStatus", _m_ResetEnableStatus);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitialiseChildObjects", _m_InitialiseChildObjects);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetEnable", _m_SetEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateVisible", _m_UpdateVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextScreen", _m_NextScreen);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreviousScreen", _m_PreviousScreen);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GoToScreen", _m_GoToScreen);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartScreenChange", _m_StartScreenChange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CurrentPageObject", _m_CurrentPageObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBeginDrag", _m_OnBeginDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrag", _m_OnDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDrag", _m_OnEndDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLerp", _m_SetLerp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangePage", _m_ChangePage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerClick", _m_OnPointerClick);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentPage", _g_get_CurrentPage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnSelectionChangeStartEvent", _g_get_OnSelectionChangeStartEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnSelectionPageChangedEvent", _g_get_OnSelectionPageChangedEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnSelectionChangeEndEvent", _g_get_OnSelectionChangeEndEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartingScreen", _g_get_StartingScreen);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PageStep", _g_get_PageStep);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Pagination", _g_get_Pagination);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HideButtonWhenDisable", _g_get_HideButtonWhenDisable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrevButton", _g_get_PrevButton);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NextButton", _g_get_NextButton);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "transitionSpeed", _g_get_transitionSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseHardSwipe", _g_get_UseHardSwipe);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseFastSwipe", _g_get_UseFastSwipe);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseSwipeDeltaThreshold", _g_get_UseSwipeDeltaThreshold);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FastSwipeThreshold", _g_get_FastSwipeThreshold);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeVelocityThreshold", _g_get_SwipeVelocityThreshold);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeDeltaThreshold", _g_get_SwipeDeltaThreshold);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseTimeScale", _g_get_UseTimeScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaskArea", _g_get_MaskArea);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaskBuffer", _g_get_MaskBuffer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DontAllowReOnEnable", _g_get_DontAllowReOnEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "JumpOnEnable", _g_get_JumpOnEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RestartOnEnable", _g_get_RestartOnEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseParentTransform", _g_get_UseParentTransform);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChildObjects", _g_get_ChildObjects);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnSelectionChangeStartEvent", _s_set_OnSelectionChangeStartEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnSelectionPageChangedEvent", _s_set_OnSelectionPageChangedEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnSelectionChangeEndEvent", _s_set_OnSelectionChangeEndEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StartingScreen", _s_set_StartingScreen);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PageStep", _s_set_PageStep);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Pagination", _s_set_Pagination);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "HideButtonWhenDisable", _s_set_HideButtonWhenDisable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrevButton", _s_set_PrevButton);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "NextButton", _s_set_NextButton);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "transitionSpeed", _s_set_transitionSpeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseHardSwipe", _s_set_UseHardSwipe);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseFastSwipe", _s_set_UseFastSwipe);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseSwipeDeltaThreshold", _s_set_UseSwipeDeltaThreshold);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FastSwipeThreshold", _s_set_FastSwipeThreshold);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SwipeVelocityThreshold", _s_set_SwipeVelocityThreshold);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SwipeDeltaThreshold", _s_set_SwipeDeltaThreshold);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseTimeScale", _s_set_UseTimeScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaskArea", _s_set_MaskArea);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaskBuffer", _s_set_MaskBuffer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DontAllowReOnEnable", _s_set_DontAllowReOnEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "JumpOnEnable", _s_set_JumpOnEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RestartOnEnable", _s_set_RestartOnEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseParentTransform", _s_set_UseParentTransform);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ChildObjects", _s_set_ChildObjects);
            
			
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
					
					var gen_ret = new UnityEngine.UI.Extensions.ScrollSnapBase();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.UI.Extensions.ScrollSnapBase constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetEnableStatus(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetEnableStatus(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitialiseChildObjects(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitialiseChildObjects(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _isEnable = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetEnable( _isEnable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateVisible(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextScreen(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.NextScreen(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreviousScreen(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.PreviousScreen(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GoToScreen(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _screenIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.GoToScreen( _screenIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartScreenChange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StartScreenChange(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CurrentPageObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.CurrentPageObject(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    UnityEngine.Transform _returnObject;
                    
                    gen_to_be_invoked.CurrentPageObject( out _returnObject );
                    translator.Push(L, _returnObject);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.UI.Extensions.ScrollSnapBase.CurrentPageObject!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBeginDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_OnDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnDrag( _eventData );
                    
                    
                    
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
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_SetLerp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _value = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetLerp( _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangePage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _page = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangePage( _page );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerClick( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentPage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CurrentPage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnSelectionChangeStartEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnSelectionChangeStartEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnSelectionPageChangedEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnSelectionPageChangedEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnSelectionChangeEndEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnSelectionChangeEndEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartingScreen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.StartingScreen);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PageStep(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PageStep);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Pagination(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Pagination);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HideButtonWhenDisable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.HideButtonWhenDisable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrevButton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrevButton);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NextButton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.NextButton);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_transitionSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.transitionSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseHardSwipe(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseHardSwipe);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseFastSwipe(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseFastSwipe);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseSwipeDeltaThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseSwipeDeltaThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FastSwipeThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.FastSwipeThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeVelocityThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SwipeVelocityThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeDeltaThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SwipeDeltaThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseTimeScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseTimeScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaskArea(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MaskArea);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaskBuffer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MaskBuffer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DontAllowReOnEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DontAllowReOnEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_JumpOnEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.JumpOnEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RestartOnEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.RestartOnEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseParentTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseParentTransform);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChildObjects(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ChildObjects);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnSelectionChangeStartEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnSelectionChangeStartEvent = (UnityEngine.UI.Extensions.ScrollSnapBase.SelectionChangeStartEvent)translator.GetObject(L, 2, typeof(UnityEngine.UI.Extensions.ScrollSnapBase.SelectionChangeStartEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnSelectionPageChangedEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnSelectionPageChangedEvent = (UnityEngine.UI.Extensions.ScrollSnapBase.SelectionPageChangedEvent)translator.GetObject(L, 2, typeof(UnityEngine.UI.Extensions.ScrollSnapBase.SelectionPageChangedEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnSelectionChangeEndEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnSelectionChangeEndEvent = (UnityEngine.UI.Extensions.ScrollSnapBase.SelectionChangeEndEvent)translator.GetObject(L, 2, typeof(UnityEngine.UI.Extensions.ScrollSnapBase.SelectionChangeEndEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StartingScreen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StartingScreen = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PageStep(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PageStep = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Pagination(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Pagination = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HideButtonWhenDisable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HideButtonWhenDisable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrevButton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrevButton = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NextButton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.NextButton = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_transitionSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.transitionSpeed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseHardSwipe(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseHardSwipe = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseFastSwipe(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseFastSwipe = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseSwipeDeltaThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseSwipeDeltaThreshold = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FastSwipeThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FastSwipeThreshold = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SwipeVelocityThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SwipeVelocityThreshold = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SwipeDeltaThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SwipeDeltaThreshold = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseTimeScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseTimeScale = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaskArea(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaskArea = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaskBuffer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaskBuffer = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DontAllowReOnEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DontAllowReOnEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_JumpOnEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.JumpOnEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RestartOnEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RestartOnEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseParentTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseParentTransform = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChildObjects(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.UI.Extensions.ScrollSnapBase gen_to_be_invoked = (UnityEngine.UI.Extensions.ScrollSnapBase)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ChildObjects = (UnityEngine.GameObject[])translator.GetObject(L, 2, typeof(UnityEngine.GameObject[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
