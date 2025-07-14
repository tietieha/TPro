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
    public class CinemachineCinemachineCoreWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cinemachine.CinemachineCore);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetActiveBrain", _m_GetActiveBrain);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetVirtualCamera", _m_GetVirtualCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsLive", _m_IsLive);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsLiveInBlend", _m_IsLiveInBlend);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GenerateCameraActivationEvent", _m_GenerateCameraActivationEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GenerateCameraCutEvent", _m_GenerateCameraCutEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindPotentialTargetBrain", _m_FindPotentialTargetBrain);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTargetObjectWarped", _m_OnTargetObjectWarped);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "BrainCount", _g_get_BrainCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VirtualCameraCount", _g_get_VirtualCameraCount);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 10, 7);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "kStreamingVersion", Cinemachine.CinemachineCore.kStreamingVersion);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DeltaTime", _g_get_DeltaTime);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CurrentTime", _g_get_CurrentTime);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "sShowHiddenObjects", _g_get_sShowHiddenObjects);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "GetInputAxis", _g_get_GetInputAxis);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "UniformDeltaTimeOverride", _g_get_UniformDeltaTimeOverride);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CurrentTimeOverride", _g_get_CurrentTimeOverride);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "GetBlendOverride", _g_get_GetBlendOverride);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CameraUpdatedEvent", _g_get_CameraUpdatedEvent);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CameraCutEvent", _g_get_CameraCutEvent);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "sShowHiddenObjects", _s_set_sShowHiddenObjects);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "GetInputAxis", _s_set_GetInputAxis);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "UniformDeltaTimeOverride", _s_set_UniformDeltaTimeOverride);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "CurrentTimeOverride", _s_set_CurrentTimeOverride);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "GetBlendOverride", _s_set_GetBlendOverride);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "CameraUpdatedEvent", _s_set_CameraUpdatedEvent);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "CameraCutEvent", _s_set_CameraCutEvent);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Cinemachine.CinemachineCore();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.CinemachineCore constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetActiveBrain(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetActiveBrain( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVirtualCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetVirtualCamera( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsLive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.ICinemachineCamera _vcam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    
                        var gen_ret = gen_to_be_invoked.IsLive( _vcam );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsLiveInBlend(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.ICinemachineCamera _vcam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    
                        var gen_ret = gen_to_be_invoked.IsLiveInBlend( _vcam );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GenerateCameraActivationEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.ICinemachineCamera _vcam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    Cinemachine.ICinemachineCamera _vcamFrom = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 3, typeof(Cinemachine.ICinemachineCamera));
                    
                    gen_to_be_invoked.GenerateCameraActivationEvent( _vcam, _vcamFrom );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GenerateCameraCutEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.ICinemachineCamera _vcam = (Cinemachine.ICinemachineCamera)translator.GetObject(L, 2, typeof(Cinemachine.ICinemachineCamera));
                    
                    gen_to_be_invoked.GenerateCameraCutEvent( _vcam );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindPotentialTargetBrain(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.CinemachineVirtualCameraBase _vcam = (Cinemachine.CinemachineVirtualCameraBase)translator.GetObject(L, 2, typeof(Cinemachine.CinemachineVirtualCameraBase));
                    
                        var gen_ret = gen_to_be_invoked.FindPotentialTargetBrain( _vcam );
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
            
            
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineCore.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DeltaTime(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, Cinemachine.CinemachineCore.DeltaTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentTime(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, Cinemachine.CinemachineCore.CurrentTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BrainCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BrainCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VirtualCameraCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineCore gen_to_be_invoked = (Cinemachine.CinemachineCore)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.VirtualCameraCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sShowHiddenObjects(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Cinemachine.CinemachineCore.sShowHiddenObjects);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GetInputAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineCore.GetInputAxis);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UniformDeltaTimeOverride(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, Cinemachine.CinemachineCore.UniformDeltaTimeOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentTimeOverride(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, Cinemachine.CinemachineCore.CurrentTimeOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GetBlendOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineCore.GetBlendOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraUpdatedEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineCore.CameraUpdatedEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraCutEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.CinemachineCore.CameraCutEvent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sShowHiddenObjects(RealStatePtr L)
        {
		    try {
                
			    Cinemachine.CinemachineCore.sShowHiddenObjects = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GetInputAxis(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineCore.GetInputAxis = translator.GetDelegate<Cinemachine.CinemachineCore.AxisInputDelegate>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UniformDeltaTimeOverride(RealStatePtr L)
        {
		    try {
                
			    Cinemachine.CinemachineCore.UniformDeltaTimeOverride = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrentTimeOverride(RealStatePtr L)
        {
		    try {
                
			    Cinemachine.CinemachineCore.CurrentTimeOverride = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GetBlendOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineCore.GetBlendOverride = translator.GetDelegate<Cinemachine.CinemachineCore.GetBlendOverrideDelegate>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraUpdatedEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineCore.CameraUpdatedEvent = (Cinemachine.CinemachineBrain.BrainEvent)translator.GetObject(L, 1, typeof(Cinemachine.CinemachineBrain.BrainEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraCutEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Cinemachine.CinemachineCore.CameraCutEvent = (Cinemachine.CinemachineBrain.BrainEvent)translator.GetObject(L, 1, typeof(Cinemachine.CinemachineBrain.BrainEvent));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
