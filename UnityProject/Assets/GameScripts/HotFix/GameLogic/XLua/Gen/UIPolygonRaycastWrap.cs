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
    public class UIPolygonRaycastWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UIPolygonRaycast);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsRaycastLocationValid", _m_IsRaycastLocationValid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsPointInPolygon", _m_IsPointInPolygon);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "points", _g_get_points);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "points", _s_set_points);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UIPolygonRaycast does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsRaycastLocationValid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIPolygonRaycast gen_to_be_invoked = (UIPolygonRaycast)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _screenPoint;translator.Get(L, 2, out _screenPoint);
                    UnityEngine.Camera _eventCamera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    
                        var gen_ret = gen_to_be_invoked.IsRaycastLocationValid( _screenPoint, _eventCamera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsPointInPolygon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIPolygonRaycast gen_to_be_invoked = (UIPolygonRaycast)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _point;translator.Get(L, 2, out _point);
                    
                        var gen_ret = gen_to_be_invoked.IsPointInPolygon( _point );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_points(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIPolygonRaycast gen_to_be_invoked = (UIPolygonRaycast)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.points);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_points(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIPolygonRaycast gen_to_be_invoked = (UIPolygonRaycast)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.points = (UnityEngine.Vector2[])translator.GetObject(L, 2, typeof(UnityEngine.Vector2[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
