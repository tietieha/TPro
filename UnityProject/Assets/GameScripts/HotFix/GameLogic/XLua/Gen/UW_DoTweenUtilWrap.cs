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
    public class UWDoTweenUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UW.DoTweenUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "To_Float", _m_To_Float_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "To_Int", _m_To_Int_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "To_Vector3", _m_To_Vector3_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "To_Vector2", _m_To_Vector2_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "To_String", _m_To_String_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoPath", _m_DoPath_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new UW.DoTweenUtil();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UW.DoTweenUtil constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_Float_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _start = (float)LuaAPI.lua_tonumber(L, 1);
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 4, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _onCompleteLua = (XLua.LuaFunction)translator.GetObject(L, 5, typeof(XLua.LuaFunction));
                    
                        var gen_ret = UW.DoTweenUtil.To_Float( _start, _to, _duration, _setter, _onCompleteLua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_Int_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _start = LuaAPI.xlua_tointeger(L, 1);
                    int _to = LuaAPI.xlua_tointeger(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 4, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _onCompleteLua = (XLua.LuaFunction)translator.GetObject(L, 5, typeof(XLua.LuaFunction));
                    
                        var gen_ret = UW.DoTweenUtil.To_Int( _start, _to, _duration, _setter, _onCompleteLua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_Vector3_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 4, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _onCompleteLua = (XLua.LuaFunction)translator.GetObject(L, 5, typeof(XLua.LuaFunction));
                    
                        var gen_ret = UW.DoTweenUtil.To_Vector3( _start, _to, _duration, _setter, _onCompleteLua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_Vector2_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector2 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector2 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 4, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _onCompleteLua = (XLua.LuaFunction)translator.GetObject(L, 5, typeof(XLua.LuaFunction));
                    
                        var gen_ret = UW.DoTweenUtil.To_Vector2( _start, _to, _duration, _setter, _onCompleteLua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_String_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _start = LuaAPI.lua_tostring(L, 1);
                    string _to = LuaAPI.lua_tostring(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 4, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _onCompleteLua = (XLua.LuaFunction)translator.GetObject(L, 5, typeof(XLua.LuaFunction));
                    
                        var gen_ret = UW.DoTweenUtil.To_String( _start, _to, _duration, _setter, _onCompleteLua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _tf = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _start;translator.Get(L, 2, out _start);
                    UnityEngine.Vector3 _to;translator.Get(L, 3, out _to);
                    UnityEngine.Vector3 _to2;translator.Get(L, 4, out _to2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 5);
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 6, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _onCompleteLua = (XLua.LuaFunction)translator.GetObject(L, 7, typeof(XLua.LuaFunction));
                    
                        var gen_ret = UW.DoTweenUtil.DoPath( _tf, _start, _to, _to2, _duration, _setter, _onCompleteLua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
