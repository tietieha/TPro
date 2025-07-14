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
    public class CinemachineCinemachineVirtualCameraWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cinemachine.CinemachineVirtualCamera);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 7, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMaxDampTime", _m_GetMaxDampTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InternalUpdateCameraState", _m_InternalUpdateCameraState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InvalidateComponentPipeline", _m_InvalidateComponentPipeline);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentOwner", _m_GetComponentOwner);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetComponentPipeline", _m_GetComponentPipeline);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCinemachineComponent", _m_GetCinemachineComponent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTargetObjectWarped", _m_OnTargetObjectWarped);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceCameraPosition", _m_ForceCameraPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTransitionFromCamera", _m_OnTransitionFromCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFov", _m_SetFov);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "State", _g_get_State);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LookAt", _g_get_LookAt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Follow", _g_get_Follow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_LookAt", _g_get_m_LookAt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Follow", _g_get_m_Follow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Lens", _g_get_m_Lens);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Transitions", _g_get_m_Transitions);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LookAt", _s_set_LookAt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Follow", _s_set_Follow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_LookAt", _s_set_m_LookAt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Follow", _s_set_m_Follow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Lens", _s_set_m_Lens);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Transitions", _s_set_m_Transitions);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 2, 2);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PipelineName", Cinemachine.CinemachineVirtualCamera.PipelineName);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CreatePipelineOverride", _g_get_CreatePipelineOverride);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DestroyPipelineOverride", _g_get_DestroyPipelineOverride);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "CreatePipelineOverride", _s_set_CreatePipelineOverride);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "DestroyPipelineOverride", _s_set_DestroyPipelineOverride);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Cinemachine.CinemachineVirtualCamera();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.CinemachineVirtualCamera constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMaxDampTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMaxDampTime(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InternalUpdateCameraState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _worldUp;translator.Get(L, 2, out _worldUp);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.InternalUpdateCameraState( _worldUp, _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InvalidateComponentPipeline(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InvalidateComponentPipeline(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentOwner(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetComponentOwner(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentPipeline(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetComponentPipeline(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCinemachineComponent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.CinemachineCore.Stage _stage;translator.Get(L, 2, out _stage);
                    
                        var gen_ret = gen_to_be_invoked.GetCinemachineComponent( _stage );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTargetObjectWarped(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _positionDelta;translator.Get(L, 3, out _positionDelta);
                    
                    gen_to_be_invoked.OnTargetObjectWarped( _target, _positionDelta );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceCameraPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    UnityEngine.Quaternion _rot;translator.Get(L, 3, out _rot);
                    
                    gen_to_be_invoked.ForceCameraPosition( _pos, _rot );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTransitionFromCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.ICinemachineCamera _fromCam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    UnityEngine.Vector3 _worldUp;translator.Get(L, 3, out _worldUp);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.OnTransitionFromCamera( _fromCam, _worldUp, _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFov(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fov = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetFov( _fov );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_State(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.State);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LookAt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Follow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_LookAt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Follow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Lens(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Lens);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Transitions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Transitions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CreatePipelineOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineVirtualCamera.CreatePipelineOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DestroyPipelineOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineVirtualCamera.DestroyPipelineOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LookAt = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Follow = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_LookAt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_LookAt = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Follow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_Follow = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Lens(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                Cinemachine.LensSettings gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_Lens = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Transitions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineVirtualCamera gen_to_be_invoked = (Cinemachine.CinemachineVirtualCamera)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachineVirtualCameraBase.TransitionParams gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_Transitions = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CreatePipelineOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineVirtualCamera.CreatePipelineOverride = translator.GetDelegate<Cinemachine.CinemachineVirtualCamera.CreatePipelineDelegate>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DestroyPipelineOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineVirtualCamera.DestroyPipelineOverride = translator.GetDelegate<Cinemachine.CinemachineVirtualCamera.DestroyPipelineDelegate>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
