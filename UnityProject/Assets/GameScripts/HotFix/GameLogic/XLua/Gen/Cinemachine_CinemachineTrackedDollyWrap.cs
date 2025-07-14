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
    public class CinemachineCinemachineTrackedDollyWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cinemachine.CinemachineTrackedDolly);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 14, 12);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMaxDampTime", _m_GetMaxDampTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MutateCameraState", _m_MutateCameraState);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsValid", _g_get_IsValid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Stage", _g_get_Stage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Path", _g_get_m_Path);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PathPosition", _g_get_m_PathPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PositionUnits", _g_get_m_PositionUnits);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PathOffset", _g_get_m_PathOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_XDamping", _g_get_m_XDamping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_YDamping", _g_get_m_YDamping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ZDamping", _g_get_m_ZDamping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CameraUp", _g_get_m_CameraUp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PitchDamping", _g_get_m_PitchDamping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_YawDamping", _g_get_m_YawDamping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_RollDamping", _g_get_m_RollDamping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_AutoDolly", _g_get_m_AutoDolly);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Path", _s_set_m_Path);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PathPosition", _s_set_m_PathPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PositionUnits", _s_set_m_PositionUnits);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PathOffset", _s_set_m_PathOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_XDamping", _s_set_m_XDamping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_YDamping", _s_set_m_YDamping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ZDamping", _s_set_m_ZDamping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CameraUp", _s_set_m_CameraUp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PitchDamping", _s_set_m_PitchDamping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_YawDamping", _s_set_m_YawDamping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_RollDamping", _s_set_m_RollDamping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_AutoDolly", _s_set_m_AutoDolly);
            
			
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
					
					var gen_ret = new Cinemachine.CinemachineTrackedDolly();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.CinemachineTrackedDolly constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMaxDampTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_MutateCameraState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.CameraState _curState;translator.Get(L, 2, out _curState);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.MutateCameraState( ref _curState, _deltaTime );
                    translator.Push(L, _curState);
                        translator.Update(L, 2, _curState);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsValid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsValid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Stage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Stage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Path(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Path);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PathPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_PathPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PositionUnits(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_PositionUnits);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PathOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.m_PathOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_XDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_XDamping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_YDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_YDamping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_ZDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_ZDamping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_CameraUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_CameraUp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PitchDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_PitchDamping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_YawDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_YawDamping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_RollDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_RollDamping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_AutoDolly(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_AutoDolly);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Path(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_Path = (Cinemachine.CinemachinePathBase)translator.GetObject(L, 2, typeof(Cinemachine.CinemachinePathBase));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PathPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_PathPosition = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PositionUnits(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachinePathBase.PositionUnits gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_PositionUnits = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PathOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_PathOffset = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_XDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_XDamping = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_YDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_YDamping = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_ZDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_ZDamping = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_CameraUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachineTrackedDolly.CameraUpMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_CameraUp = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PitchDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_PitchDamping = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_YawDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_YawDamping = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_RollDamping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_RollDamping = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_AutoDolly(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.CinemachineTrackedDolly gen_to_be_invoked = (Cinemachine.CinemachineTrackedDolly)translator.FastGetCSObj(L, 1);
                Cinemachine.CinemachineTrackedDolly.AutoDolly gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_AutoDolly = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
