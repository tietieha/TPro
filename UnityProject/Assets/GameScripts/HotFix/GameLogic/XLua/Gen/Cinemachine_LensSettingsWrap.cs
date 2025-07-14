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
    public class CinemachineLensSettingsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cinemachine.LensSettings);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 13, 12);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SnapshotCameraReadOnlyProperties", _m_SnapshotCameraReadOnlyProperties);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Validate", _m_Validate);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Orthographic", _g_get_Orthographic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SensorSize", _g_get_SensorSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Aspect", _g_get_Aspect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsPhysicalCamera", _g_get_IsPhysicalCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FieldOfView", _g_get_FieldOfView);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OrthographicSize", _g_get_OrthographicSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NearClipPlane", _g_get_NearClipPlane);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FarClipPlane", _g_get_FarClipPlane);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Dutch", _g_get_Dutch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ModeOverride", _g_get_ModeOverride);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LensShift", _g_get_LensShift);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GateFit", _g_get_GateFit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FocusDistance", _g_get_FocusDistance);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Orthographic", _s_set_Orthographic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SensorSize", _s_set_SensorSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsPhysicalCamera", _s_set_IsPhysicalCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FieldOfView", _s_set_FieldOfView);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OrthographicSize", _s_set_OrthographicSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "NearClipPlane", _s_set_NearClipPlane);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FarClipPlane", _s_set_FarClipPlane);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Dutch", _s_set_Dutch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ModeOverride", _s_set_ModeOverride);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LensShift", _s_set_LensShift);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GateFit", _s_set_GateFit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FocusDistance", _s_set_FocusDistance);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "FromCamera", _m_FromCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Lerp", _m_Lerp_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Default", _g_get_Default);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Default", _s_set_Default);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6))
				{
					float _verticalFOV = (float)LuaAPI.lua_tonumber(L, 2);
					float _orthographicSize = (float)LuaAPI.lua_tonumber(L, 3);
					float _nearClip = (float)LuaAPI.lua_tonumber(L, 4);
					float _farClip = (float)LuaAPI.lua_tonumber(L, 5);
					float _dutch = (float)LuaAPI.lua_tonumber(L, 6);
					
					var gen_ret = new Cinemachine.LensSettings(_verticalFOV, _orthographicSize, _nearClip, _farClip, _dutch);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.Push(L, default(Cinemachine.LensSettings));
			        return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.LensSettings constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FromCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _fromCamera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    
                        var gen_ret = Cinemachine.LensSettings.FromCamera( _fromCamera );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SnapshotCameraReadOnlyProperties(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Camera>(L, 2)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.SnapshotCameraReadOnlyProperties( _camera );
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Cinemachine.LensSettings>(L, 2)) 
                {
                    Cinemachine.LensSettings _lens;translator.Get(L, 2, out _lens);
                    
                    gen_to_be_invoked.SnapshotCameraReadOnlyProperties( ref _lens );
                    translator.Push(L, _lens);
                        translator.Update(L, 2, _lens);
                        
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.LensSettings.SnapshotCameraReadOnlyProperties!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Lerp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Cinemachine.LensSettings _lensA;translator.Get(L, 1, out _lensA);
                    Cinemachine.LensSettings _lensB;translator.Get(L, 2, out _lensB);
                    float _t = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = Cinemachine.LensSettings.Lerp( _lensA, _lensB, _t );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Validate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                    gen_to_be_invoked.Validate(  );
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Orthographic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Orthographic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SensorSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.SensorSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Aspect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Aspect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsPhysicalCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsPhysicalCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Default(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Cinemachine.LensSettings.Default);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FieldOfView(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FieldOfView);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OrthographicSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.OrthographicSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NearClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.NearClipPlane);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FarClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FarClipPlane);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Dutch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Dutch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ModeOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.Push(L, gen_to_be_invoked.ModeOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LensShift(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.LensShift);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GateFit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.Push(L, gen_to_be_invoked.GateFit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FocusDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FocusDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Orthographic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.Orthographic = LuaAPI.lua_toboolean(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SensorSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.SensorSize = gen_value;
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsPhysicalCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.IsPhysicalCamera = LuaAPI.lua_toboolean(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Default(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Cinemachine.LensSettings gen_value;translator.Get(L, 1, out gen_value);
				Cinemachine.LensSettings.Default = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FieldOfView(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.FieldOfView = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OrthographicSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.OrthographicSize = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NearClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.NearClipPlane = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FarClipPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.FarClipPlane = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Dutch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.Dutch = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ModeOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                Cinemachine.LensSettings.OverrideModes gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ModeOverride = gen_value;
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LensShift(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LensShift = gen_value;
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GateFit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                UnityEngine.Camera.GateFitMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.GateFit = gen_value;
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FocusDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.LensSettings gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.FocusDistance = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
