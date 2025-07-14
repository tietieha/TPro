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
    public class UILineRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UILineRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 15, 14);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUseBezier", _m_SetUseBezier);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPoints", _m_SetPoints);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAnim", _m_PlayAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SamplePointsByCount", _m_SamplePointsByCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBezierPoint", _m_GetBezierPoint);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "mainTexture", _g_get_mainTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "texture", _g_get_texture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loop", _g_get_loop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "normalOffset", _g_get_normalOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "debug", _g_get_debug);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "width", _g_get_width);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "uvZoom", _g_get_uvZoom);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cornerOffset", _g_get_cornerOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "numIter", _g_get_numIter);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CornerDistance", _g_get_CornerDistance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "catmullRate", _g_get_catmullRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bezierRate", _g_get_bezierRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bezierIndexList", _g_get_bezierIndexList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "edgeDist", _g_get_edgeDist);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pts", _g_get_pts);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "texture", _s_set_texture);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "loop", _s_set_loop);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "normalOffset", _s_set_normalOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "debug", _s_set_debug);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "width", _s_set_width);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "uvZoom", _s_set_uvZoom);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cornerOffset", _s_set_cornerOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "numIter", _s_set_numIter);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CornerDistance", _s_set_CornerDistance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "catmullRate", _s_set_catmullRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "bezierRate", _s_set_bezierRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "bezierIndexList", _s_set_bezierIndexList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "edgeDist", _s_set_edgeDist);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pts", _s_set_pts);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CrossProductError", UILineRenderer.CrossProductError);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new UILineRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UILineRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUseBezier(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _useBezier = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetUseBezier( _useBezier );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPoints(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2[] _points = (UnityEngine.Vector2[])translator.GetObject(L, 2, typeof(UnityEngine.Vector2[]));
                    
                    gen_to_be_invoked.SetPoints( _points );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _onComplete = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.PlayAnim( _duration, _onComplete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SamplePointsByCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _P0;translator.Get(L, 2, out _P0);
                    UnityEngine.Vector2 _P1;translator.Get(L, 3, out _P1);
                    UnityEngine.Vector2 _P2;translator.Get(L, 4, out _P2);
                    int _count = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.SamplePointsByCount( _P0, _P1, _P2, _count );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBezierPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _t = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector2 _P0;translator.Get(L, 3, out _P0);
                    UnityEngine.Vector2 _P1;translator.Get(L, 4, out _P1);
                    UnityEngine.Vector2 _P2;translator.Get(L, 5, out _P2);
                    
                        var gen_ret = gen_to_be_invoked.GetBezierPoint( _t, _P0, _P1, _P2 );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mainTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mainTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_texture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.texture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.loop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_normalOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.normalOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_debug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.debug);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_width(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.width);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_uvZoom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.uvZoom);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cornerOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.cornerOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_numIter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.numIter);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CornerDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CornerDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_catmullRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.catmullRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bezierRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.bezierRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bezierIndexList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.bezierIndexList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_edgeDist(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.edgeDist);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.pts);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_texture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.texture = (UnityEngine.Texture)translator.GetObject(L, 2, typeof(UnityEngine.Texture));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.loop = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_normalOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.normalOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_debug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.debug = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_width(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.width = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_uvZoom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.uvZoom = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cornerOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cornerOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_numIter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.numIter = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CornerDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CornerDistance = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_catmullRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.catmullRate = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bezierRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.bezierRate = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bezierIndexList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.bezierIndexList = (int[])translator.GetObject(L, 2, typeof(int[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_edgeDist(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.edgeDist = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILineRenderer gen_to_be_invoked = (UILineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pts = (UnityEngine.Vector2[])translator.GetObject(L, 2, typeof(UnityEngine.Vector2[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
