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
    public class BitBenderGamesTouchInputControllerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BitBenderGames.TouchInputController);
			Utils.BeginObjectRegister(type, L, translator, 0, 16, 4, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Awake", _m_Awake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEventTriggerPointerDown", _m_OnEventTriggerPointerDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RestartDrag", _m_RestartDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InputClick", _m_InputClick);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragStart", _e_OnDragStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnFingerDown", _e_OnFingerDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnFingerUp", _e_OnFingerUp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragUpdate", _e_OnDragUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragStop", _e_OnDragStop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchStart", _e_OnPinchStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchUpdate", _e_OnPinchUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchUpdateExtended", _e_OnPinchUpdateExtended);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchStop", _e_OnPinchStop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLongTapProgress", _e_OnLongTapProgress);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInputClick", _e_OnInputClick);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsPinching", _g_get_IsPinching);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LongTapStartsDrag", _g_get_LongTapStartsDrag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsInputOnLockedArea", _g_get_IsInputOnLockedArea);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stopUpdate", _g_get_stopUpdate);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsInputOnLockedArea", _s_set_IsInputOnLockedArea);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stopUpdate", _s_set_stopUpdate);
            
			
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
					
					var gen_ret = new BitBenderGames.TouchInputController();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Awake(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Awake(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEventTriggerPointerDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.OnEventTriggerPointerDown( _go );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.EventSystems.BaseEventData>(L, 2)) 
                {
                    UnityEngine.EventSystems.BaseEventData _baseEventData = (UnityEngine.EventSystems.BaseEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.BaseEventData));
                    
                    gen_to_be_invoked.OnEventTriggerPointerDown( _baseEventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnEventTriggerPointerDown!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RestartDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RestartDrag(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InputClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _clickPosition;translator.Get(L, 2, out _clickPosition);
                    bool _isDoubleClick = LuaAPI.lua_toboolean(L, 3);
                    bool _isLongTap = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.InputClick( _clickPosition, _isDoubleClick, _isLongTap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsPinching(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsPinching);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LongTapStartsDrag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LongTapStartsDrag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsInputOnLockedArea(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsInputOnLockedArea);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stopUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.stopUpdate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsInputOnLockedArea(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsInputOnLockedArea = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stopUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.stopUpdate = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnDragStart(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.InputDragStartDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.InputDragStartDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.InputDragStartDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnDragStart += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnDragStart -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnDragStart!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnFingerDown(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.Input1PositionDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.Input1PositionDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.Input1PositionDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnFingerDown += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnFingerDown -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnFingerDown!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnFingerUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.Input1PositionDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.Input1PositionDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.Input1PositionDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnFingerUp += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnFingerUp -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnFingerUp!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnDragUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.DragUpdateDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.DragUpdateDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.DragUpdateDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnDragUpdate += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnDragUpdate -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnDragUpdate!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnDragStop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.DragStopDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.DragStopDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.DragStopDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnDragStop += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnDragStop -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnDragStop!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchStart(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.PinchStartDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.PinchStartDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.PinchStartDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnPinchStart += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnPinchStart -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchStart!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.PinchUpdateDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.PinchUpdateDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.PinchUpdateDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnPinchUpdate += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnPinchUpdate -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchUpdate!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchUpdateExtended(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.PinchUpdateExtendedDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.PinchUpdateExtendedDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.PinchUpdateExtendedDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnPinchUpdateExtended += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnPinchUpdateExtended -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchUpdateExtended!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchStop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                System.Action gen_delegate = translator.GetDelegate<System.Action>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnPinchStop += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnPinchStop -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnPinchStop!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnLongTapProgress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.InputLongTapProgress gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.InputLongTapProgress>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.InputLongTapProgress!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnLongTapProgress += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnLongTapProgress -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnLongTapProgress!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnInputClick(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			BitBenderGames.TouchInputController gen_to_be_invoked = (BitBenderGames.TouchInputController)translator.FastGetCSObj(L, 1);
                BitBenderGames.TouchInputController.InputClickDelegate gen_delegate = translator.GetDelegate<BitBenderGames.TouchInputController.InputClickDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need BitBenderGames.TouchInputController.InputClickDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnInputClick += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnInputClick -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.TouchInputController.OnInputClick!");
            return 0;
        }
        
		
		
    }
}
