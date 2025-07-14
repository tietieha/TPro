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
    public class SquareGuideMaskWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SquareGuideMask);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateCenterDataByDimension", _m_CalculateCenterDataByDimension);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTargetCenter", _m_GetTargetCenter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget2DGameObject", _m_SetTarget2DGameObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget3DGameObject", _m_SetTarget3DGameObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetIsShowHollow", _m_SetIsShowHollow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUICamera", _m_SetUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSceneCamera", _m_SetSceneCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget2DHollowSize", _m_SetTarget2DHollowSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget3DHollowSize", _m_SetTarget3DHollowSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnClickShowHollow", _m_OnClickShowHollow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "_shrinkTime", _g_get__shrinkTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maskColor", _g_get_maskColor);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "_shrinkTime", _s_set__shrinkTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maskColor", _s_set_maskColor);
            
			
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
					
					var gen_ret = new SquareGuideMask();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SquareGuideMask constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateCenterDataByDimension(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CalculateCenterDataByDimension(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetCenter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTargetCenter(  );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget2DGameObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _gameObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.SetTarget2DGameObject( _gameObject );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget3DGameObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _gameObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.SetTarget3DGameObject( _gameObject );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetIsShowHollow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _state = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetIsShowHollow( _state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.SetUICamera( _camera );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSceneCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.SetSceneCamera( _camera );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget2DHollowSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _distance = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetTarget2DHollowSize( _distance );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget3DHollowSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float[] _size = (float[])translator.GetObject(L, 2, typeof(float[]));
                    
                    gen_to_be_invoked.SetTarget3DHollowSize( _size );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnClickShowHollow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnClickShowHollow(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDestroy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__shrinkTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked._shrinkTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maskColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.maskColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__shrinkTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._shrinkTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maskColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SquareGuideMask gen_to_be_invoked = (SquareGuideMask)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.maskColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
