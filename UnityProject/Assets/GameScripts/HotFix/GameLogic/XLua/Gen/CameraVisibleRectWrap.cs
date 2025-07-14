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
    public class CameraVisibleRectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(CameraVisibleRect);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateVisibleRect", _m_UpdateVisibleRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalcPosByViewPoint", _m_CalcPosByViewPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetVisibleRect", _m_GetVisibleRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCameraCenterPos", _m_GetCameraCenterPos);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ToVector3ZeroY", _m_ToVector3ZeroY_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new CameraVisibleRect();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CameraVisibleRect constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CameraVisibleRect gen_to_be_invoked = (CameraVisibleRect)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateVisibleRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CameraVisibleRect gen_to_be_invoked = (CameraVisibleRect)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateVisibleRect(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalcPosByViewPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CameraVisibleRect gen_to_be_invoked = (CameraVisibleRect)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.CalcPosByViewPoint( _x, _y );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)) 
                {
                    float _tanY = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _forward;translator.Get(L, 3, out _forward);
                    float _x = (float)LuaAPI.lua_tonumber(L, 4);
                    float _y = (float)LuaAPI.lua_tonumber(L, 5);
                    float _cameraHeight = (float)LuaAPI.lua_tonumber(L, 6);
                    float _startX = (float)LuaAPI.lua_tonumber(L, 7);
                    float _startY = (float)LuaAPI.lua_tonumber(L, 8);
                    
                        var gen_ret = gen_to_be_invoked.CalcPosByViewPoint( _tanY, _forward, _x, _y, _cameraHeight, _startX, _startY );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    float _tanY = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _forward;translator.Get(L, 3, out _forward);
                    float _x = (float)LuaAPI.lua_tonumber(L, 4);
                    float _y = (float)LuaAPI.lua_tonumber(L, 5);
                    float _cameraHeight = (float)LuaAPI.lua_tonumber(L, 6);
                    float _startX = (float)LuaAPI.lua_tonumber(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.CalcPosByViewPoint( _tanY, _forward, _x, _y, _cameraHeight, _startX );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    float _tanY = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _forward;translator.Get(L, 3, out _forward);
                    float _x = (float)LuaAPI.lua_tonumber(L, 4);
                    float _y = (float)LuaAPI.lua_tonumber(L, 5);
                    float _cameraHeight = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.CalcPosByViewPoint( _tanY, _forward, _x, _y, _cameraHeight );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CameraVisibleRect.CalcPosByViewPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVisibleRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CameraVisibleRect gen_to_be_invoked = (CameraVisibleRect)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetVisibleRect(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCameraCenterPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CameraVisibleRect gen_to_be_invoked = (CameraVisibleRect)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetCameraCenterPos(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToVector3ZeroY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector2 _target;translator.Get(L, 1, out _target);
                    
                        var gen_ret = CameraVisibleRect.ToVector3ZeroY( _target );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
