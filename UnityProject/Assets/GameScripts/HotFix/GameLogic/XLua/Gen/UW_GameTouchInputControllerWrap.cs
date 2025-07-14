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
    public class UWGameTouchInputControllerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UW.GameTouchInputController);
			Utils.BeginObjectRegister(type, L, translator, 0, 22, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RestartDrag", _m_RestartDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InputDragStart", _m_InputDragStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DragUpdate", _m_DragUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DragStop", _m_DragStop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PinchStart", _m_PinchStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PinchUpdate", _m_PinchUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ProgressLongTap", _m_ProgressLongTap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InputClick", _m_InputClick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TouchDown", _m_TouchDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TouchUp", _m_TouchUp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PinchStop", _m_PinchStop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentInputPosition", _m_GetCurrentInputPosition);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnFingerDown", _e_OnFingerDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnFingerUp", _e_OnFingerUp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragStart", _e_OnDragStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragUpdate", _e_OnDragUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragStop", _e_OnDragStop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchStart", _e_OnPinchStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchUpdate", _e_OnPinchUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPinchStop", _e_OnPinchStop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLongTapProgress", _e_OnLongTapProgress);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInputClick", _e_OnInputClick);
			
			
			
			
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
					
					var gen_ret = new UW.GameTouchInputController();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RestartDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RestartDrag(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InputDragStart(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    bool _isLongTap = LuaAPI.lua_toboolean(L, 3);
                    UnityEngine.Vector3 _offset;translator.Get(L, 4, out _offset);
                    
                    gen_to_be_invoked.InputDragStart( _position, _isLongTap, _offset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DragUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _dragPositionStart;translator.Get(L, 2, out _dragPositionStart);
                    UnityEngine.Vector3 _dragPositionCurrent;translator.Get(L, 3, out _dragPositionCurrent);
                    UnityEngine.Vector3 _correctionOffset;translator.Get(L, 4, out _correctionOffset);
                    
                    gen_to_be_invoked.DragUpdate( _dragPositionStart, _dragPositionCurrent, _correctionOffset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DragStop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _dragStopPosition;translator.Get(L, 2, out _dragStopPosition);
                    UnityEngine.Vector3 _dragFinalMomentum;translator.Get(L, 3, out _dragFinalMomentum);
                    
                    gen_to_be_invoked.DragStop( _dragStopPosition, _dragFinalMomentum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PinchStart(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _pinchCenter;translator.Get(L, 2, out _pinchCenter);
                    float _pinchDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.PinchStart( _pinchCenter, _pinchDistance );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PinchUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _pinchCenter;translator.Get(L, 2, out _pinchCenter);
                    float _pinchDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    float _pinchStartDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.PinchUpdate( _pinchCenter, _pinchDistance, _pinchStartDistance );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ProgressLongTap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _progress = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.ProgressLongTap( _progress );
                    
                    
                    
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
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_TouchDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                    gen_to_be_invoked.TouchDown( _position );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TouchUp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                    gen_to_be_invoked.TouchUp( _position );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PinchStop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.PinchStop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentInputPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetCurrentInputPosition(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnFingerDown(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.Input1PositionDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.Input1PositionDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.Input1PositionDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnFingerDown!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnFingerUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.Input1PositionDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.Input1PositionDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.Input1PositionDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnFingerUp!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnDragStart(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.InputDragStartDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.InputDragStartDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.InputDragStartDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnDragStart!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnDragUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.DragUpdateDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.DragUpdateDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.DragUpdateDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnDragUpdate!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnDragStop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.DragStopDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.DragStopDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.DragStopDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnDragStop!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchStart(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.PinchStartDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.PinchStartDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.PinchStartDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnPinchStart!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.PinchUpdateDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.PinchUpdateDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.PinchUpdateDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnPinchUpdate!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPinchStop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnPinchStop!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnLongTapProgress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.InputLongTapProgress gen_delegate = translator.GetDelegate<UW.GameTouchInputController.InputLongTapProgress>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.InputLongTapProgress!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnLongTapProgress!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnInputClick(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			UW.GameTouchInputController gen_to_be_invoked = (UW.GameTouchInputController)translator.FastGetCSObj(L, 1);
                UW.GameTouchInputController.InputClickDelegate gen_delegate = translator.GetDelegate<UW.GameTouchInputController.InputClickDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UW.GameTouchInputController.InputClickDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to UW.GameTouchInputController.OnInputClick!");
            return 0;
        }
        
		
		
    }
}
