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
    public class BitBenderGamesPolygonBoundaryTouchCameraWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BitBenderGames.PolygonBoundaryTouchCamera);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCameraBoundary", _m_SetCameraBoundary);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIsBoundaryPosition", _m_GetIsBoundaryPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClampToBoundaries", _m_GetClampToBoundaries);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "PolygonBoundaryVertices", _g_get_PolygonBoundaryVertices);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UsePolygonBoundary", _g_get_UsePolygonBoundary);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "PolygonBoundaryVertices", _s_set_PolygonBoundaryVertices);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UsePolygonBoundary", _s_set_UsePolygonBoundary);
            
			
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
					
					var gen_ret = new BitBenderGames.PolygonBoundaryTouchCamera();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.PolygonBoundaryTouchCamera constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCameraBoundary(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int[] _points = (int[])translator.GetObject(L, 2, typeof(int[]));
                    
                    gen_to_be_invoked.SetCameraBoundary( _points );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIsBoundaryPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _testPosition;translator.Get(L, 2, out _testPosition);
                    
                        var gen_ret = gen_to_be_invoked.GetIsBoundaryPosition( _testPosition );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClampToBoundaries(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _newPosition;translator.Get(L, 2, out _newPosition);
                    bool _includeSpringBackMargin = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetClampToBoundaries( _newPosition, _includeSpringBackMargin );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _newPosition;translator.Get(L, 2, out _newPosition);
                    
                        var gen_ret = gen_to_be_invoked.GetClampToBoundaries( _newPosition );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.PolygonBoundaryTouchCamera.GetClampToBoundaries!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PolygonBoundaryVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PolygonBoundaryVertices);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UsePolygonBoundary(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UsePolygonBoundary);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PolygonBoundaryVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PolygonBoundaryVertices = (System.Collections.Generic.List<UnityEngine.Vector2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector2>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UsePolygonBoundary(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.PolygonBoundaryTouchCamera gen_to_be_invoked = (BitBenderGames.PolygonBoundaryTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UsePolygonBoundary = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
