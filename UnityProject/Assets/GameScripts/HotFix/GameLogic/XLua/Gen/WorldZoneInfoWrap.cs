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
    public class WorldZoneInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(WorldZoneInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 22, 22);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetData", _m_SetData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBodyPosition", _m_SetBodyPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetEdgePosition", _m_SetEdgePosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToggleBody", _m_ToggleBody);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToggleEdge", _m_ToggleEdge);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "index", _g_get_index);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rect", _g_get_rect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "data", _g_get_data);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "body", _g_get_body);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "shadow", _g_get_shadow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "edge", _g_get_edge);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "zone_visible", _g_get_zone_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "zone_loaded", _g_get_zone_loaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "icon_visible", _g_get_icon_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "icon_loaded", _g_get_icon_loaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "edge_visible", _g_get_edge_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "edge_loaded", _g_get_edge_loaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "battleNews_visible", _g_get_battleNews_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "marchNews_visible", _g_get_marchNews_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cityChangeNews_visible", _g_get_cityChangeNews_visible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "finded", _g_get_finded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "findState", _g_get_findState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsDirty", _g_get_IsDirty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pos_body", _g_get_pos_body);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pos_cityIcon", _g_get_pos_cityIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pos_edge", _g_get_pos_edge);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "canvasGroup", _g_get_canvasGroup);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "index", _s_set_index);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rect", _s_set_rect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "data", _s_set_data);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "body", _s_set_body);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "shadow", _s_set_shadow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "edge", _s_set_edge);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "zone_visible", _s_set_zone_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "zone_loaded", _s_set_zone_loaded);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "icon_visible", _s_set_icon_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "icon_loaded", _s_set_icon_loaded);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "edge_visible", _s_set_edge_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "edge_loaded", _s_set_edge_loaded);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "battleNews_visible", _s_set_battleNews_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "marchNews_visible", _s_set_marchNews_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cityChangeNews_visible", _s_set_cityChangeNews_visible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "finded", _s_set_finded);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "findState", _s_set_findState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsDirty", _s_set_IsDirty);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pos_body", _s_set_pos_body);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pos_cityIcon", _s_set_pos_cityIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pos_edge", _s_set_pos_edge);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "canvasGroup", _s_set_canvasGroup);
            
			
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
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<WorldZoneData>(L, 2))
				{
					WorldZoneData _info = (WorldZoneData)translator.GetObject(L, 2, typeof(WorldZoneData));
					
					var gen_ret = new WorldZoneInfo(_info);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to WorldZoneInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    WorldZoneData _info = (WorldZoneData)translator.GetObject(L, 2, typeof(WorldZoneData));
                    
                    gen_to_be_invoked.SetData( _info );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBodyPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    
                    gen_to_be_invoked.SetBodyPosition( _pos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetEdgePosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    
                    gen_to_be_invoked.SetEdgePosition( _pos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToggleBody(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _visible = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.ToggleBody( _visible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToggleEdge(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _visible = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.ToggleEdge( _visible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_index(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.index);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.rect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_data(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.data);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_body(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.body);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_shadow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.shadow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_edge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.edge);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_zone_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.zone_visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_zone_loaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.zone_loaded);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_icon_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.icon_visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_icon_loaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.icon_loaded);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_edge_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.edge_visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_edge_loaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.edge_loaded);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_battleNews_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.battleNews_visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_marchNews_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.marchNews_visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cityChangeNews_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.cityChangeNews_visible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_finded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.finded);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_findState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.findState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsDirty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsDirty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pos_body(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.pos_body);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pos_cityIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.pos_cityIcon);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pos_edge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.pos_edge);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_canvasGroup(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.canvasGroup);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_index(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.index = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.RectInt gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.rect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_data(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.data = (WorldZoneData)translator.GetObject(L, 2, typeof(WorldZoneData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_body(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.body = (UnityEngine.SpriteRenderer)translator.GetObject(L, 2, typeof(UnityEngine.SpriteRenderer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_shadow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.shadow = (UnityEngine.SpriteRenderer)translator.GetObject(L, 2, typeof(UnityEngine.SpriteRenderer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_edge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.edge = (WorldZoneEdge)translator.GetObject(L, 2, typeof(WorldZoneEdge));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_zone_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.zone_visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_zone_loaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.zone_loaded = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_icon_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.icon_visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_icon_loaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.icon_loaded = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_edge_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.edge_visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_edge_loaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.edge_loaded = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_battleNews_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.battleNews_visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_marchNews_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.marchNews_visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cityChangeNews_visible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cityChangeNews_visible = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_finded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.finded = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_findState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.findState = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsDirty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsDirty = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pos_body(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.pos_body = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pos_cityIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.pos_cityIcon = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pos_edge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.pos_edge = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_canvasGroup(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                WorldZoneInfo gen_to_be_invoked = (WorldZoneInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.canvasGroup = (UnityEngine.CanvasGroup)translator.GetObject(L, 2, typeof(UnityEngine.CanvasGroup));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
