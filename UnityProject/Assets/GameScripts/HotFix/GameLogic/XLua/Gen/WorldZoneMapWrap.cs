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
    public class WorldZoneMapWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(WorldZoneMap);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 17, 17);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearZoneMap", _m_ClearZoneMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnInit", _m_UnInit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsInClip", _m_IsInClip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetOutlineParam", _m_SetOutlineParam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetRenderParam", _m_SetRenderParam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BreathZone", _m_BreathZone);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMaterialInZone", _m_GetMaterialInZone);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToPoint", _m_MoveToPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HideZoneRoot", _m_HideZoneRoot);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUpdate", _m_OnUpdate);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "touchCamera", _g_get_touchCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "zoneRoot", _g_get_zoneRoot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "edgeRoot", _g_get_edgeRoot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "shadowOffset", _g_get_shadowOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "splashTextures", _g_get_splashTextures);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cityUIScaleUp", _g_get_cityUIScaleUp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lastCamPosition", _g_get_lastCamPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lastRect", _g_get_lastRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "camVertsRect", _g_get_camVertsRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "inited", _g_get_inited);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_lastClippedZones", _g_get__lastClippedZones);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_clippedZones", _g_get__clippedZones);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_lastClippedEdges", _g_get__lastClippedEdges);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_clippedEdges", _g_get__clippedEdges);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DstAlpha", _g_get_DstAlpha);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SrcAlpha", _g_get_SrcAlpha);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FadeDir", _g_get_FadeDir);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "touchCamera", _s_set_touchCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "zoneRoot", _s_set_zoneRoot);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "edgeRoot", _s_set_edgeRoot);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "shadowOffset", _s_set_shadowOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "splashTextures", _s_set_splashTextures);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cityUIScaleUp", _s_set_cityUIScaleUp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lastCamPosition", _s_set_lastCamPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lastRect", _s_set_lastRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "camVertsRect", _s_set_camVertsRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "inited", _s_set_inited);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_lastClippedZones", _s_set__lastClippedZones);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_clippedZones", _s_set__clippedZones);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_lastClippedEdges", _s_set__lastClippedEdges);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_clippedEdges", _s_set__clippedEdges);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DstAlpha", _s_set_DstAlpha);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SrcAlpha", _s_set_SrcAlpha);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FadeDir", _s_set_FadeDir);
            
			
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
					
					var gen_ret = new WorldZoneMap();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to WorldZoneMap constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearZoneMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearZoneMap(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UnInit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsInClip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _worldPos;translator.Get(L, 2, out _worldPos);
                    
                        var gen_ret = gen_to_be_invoked.IsInClip( _worldPos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetOutlineParam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Transform _trans = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    WorldCityColor _cfg = (WorldCityColor)translator.GetObject(L, 3, typeof(WorldCityColor));
                    WorldZoneInfo _zoneInfo = (WorldZoneInfo)translator.GetObject(L, 4, typeof(WorldZoneInfo));
                    
                    gen_to_be_invoked.SetOutlineParam( _trans, _cfg, _zoneInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRenderParam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Material _material = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
                    WorldCityColor _cfg = (WorldCityColor)translator.GetObject(L, 3, typeof(WorldCityColor));
                    
                    gen_to_be_invoked.SetRenderParam( _material, _cfg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BreathZone(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    WorldZoneInfo _zoneInfo = (WorldZoneInfo)translator.GetObject(L, 2, typeof(WorldZoneInfo));
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    int _loop = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.BreathZone( _zoneInfo, _time, _loop );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMaterialInZone(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _body = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                        var gen_ret = gen_to_be_invoked.GetMaterialInZone( _body );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _sx = (float)LuaAPI.lua_tonumber(L, 2);
                    float _sy = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.MoveToPoint( _sx, _sy );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HideZoneRoot(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.HideZoneRoot(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_touchCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.touchCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_zoneRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.zoneRoot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_edgeRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.edgeRoot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_shadowOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.shadowOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_splashTextures(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.splashTextures);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cityUIScaleUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.cityUIScaleUp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lastCamPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.lastCamPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lastRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.lastRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_camVertsRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.camVertsRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_inited(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.inited);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__lastClippedZones(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._lastClippedZones);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__clippedZones(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._clippedZones);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__lastClippedEdges(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._lastClippedEdges);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__clippedEdges(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._clippedEdges);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DstAlpha(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DstAlpha);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SrcAlpha(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SrcAlpha);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FadeDir(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.FadeDir);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_touchCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.touchCamera = (BitBenderGames.MobileTouchCamera)translator.GetObject(L, 2, typeof(BitBenderGames.MobileTouchCamera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_zoneRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.zoneRoot = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_edgeRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.edgeRoot = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_shadowOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.shadowOffset = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_splashTextures(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.splashTextures = (UnityEngine.Texture2D[])translator.GetObject(L, 2, typeof(UnityEngine.Texture2D[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cityUIScaleUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cityUIScaleUp = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lastCamPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.lastCamPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lastRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.lastRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_camVertsRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.camVertsRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_inited(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.inited = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__lastClippedZones(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._lastClippedZones = (System.Collections.Generic.List<WorldZoneInfo>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<WorldZoneInfo>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__clippedZones(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._clippedZones = (System.Collections.Generic.List<WorldZoneInfo>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<WorldZoneInfo>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__lastClippedEdges(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._lastClippedEdges = (System.Collections.Generic.List<WorldZoneInfo>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<WorldZoneInfo>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__clippedEdges(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._clippedEdges = (System.Collections.Generic.List<WorldZoneInfo>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<WorldZoneInfo>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DstAlpha(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DstAlpha = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SrcAlpha(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SrcAlpha = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FadeDir(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneMap gen_to_be_invoked = (WorldZoneMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FadeDir = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
