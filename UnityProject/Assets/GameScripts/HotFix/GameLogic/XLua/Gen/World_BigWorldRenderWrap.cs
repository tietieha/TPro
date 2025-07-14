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
    public class WorldBigWorldRenderWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(World.BigWorldRender);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadMap", _m_LoadMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetWorldLoop", _m_SetWorldLoop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetShadowCasterPassEnable", _m_SetShadowCasterPassEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SwitchShadowCasterPassEnable", _m_SwitchShadowCasterPassEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnInit", _m_UnInit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadMapData", _m_LoadMapData);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "isDebug", _g_get_isDebug);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mapOffset", _g_get_mapOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "resRenderData", _g_get_resRenderData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mainCamera", _g_get_mainCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "testLod", _g_get_testLod);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MapData", _g_get_MapData);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "isDebug", _s_set_isDebug);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mapOffset", _s_set_mapOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "resRenderData", _s_set_resRenderData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mainCamera", _s_set_mainCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "testLod", _s_set_testLod);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MapData", _s_set_MapData);
            
			
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
					
					var gen_ret = new World.BigWorldRender();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to World.BigWorldRender constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<ResRenderData>(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    ResRenderData _data = (ResRenderData)translator.GetObject(L, 2, typeof(ResRenderData));
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.LoadMap( _data, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<ResRenderData>(L, 2)) 
                {
                    ResRenderData _data = (ResRenderData)translator.GetObject(L, 2, typeof(ResRenderData));
                    
                    gen_to_be_invoked.LoadMap( _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to World.BigWorldRender.LoadMap!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<ResRenderData>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    ResRenderData _data = (ResRenderData)translator.GetObject(L, 2, typeof(ResRenderData));
                    UnityEngine.Vector3 _offset;translator.Get(L, 3, out _offset);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.Init( _data, _offset, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<ResRenderData>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    ResRenderData _data = (ResRenderData)translator.GetObject(L, 2, typeof(ResRenderData));
                    UnityEngine.Vector3 _offset;translator.Get(L, 3, out _offset);
                    
                    gen_to_be_invoked.Init( _data, _offset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to World.BigWorldRender.Init!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetWorldLoop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _b = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetWorldLoop( _b );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetShadowCasterPassEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetShadowCasterPassEnable( _enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwitchShadowCasterPassEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SwitchShadowCasterPassEnable(  );
                    
                    
                    
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
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UnInit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadMapData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.TextAsset>(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    UnityEngine.TextAsset _textAsset = (UnityEngine.TextAsset)translator.GetObject(L, 2, typeof(UnityEngine.TextAsset));
                    System.Action _callBack = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.LoadMapData( _textAsset, _callBack );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    string _filePath = LuaAPI.lua_tostring(L, 2);
                    System.Action _callBak = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.LoadMapData( _filePath, _callBak );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to World.BigWorldRender.LoadMapData!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isDebug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isDebug);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mapOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.mapOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_resRenderData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.resRenderData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mainCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mainCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_testLod(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.testLod);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MapData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MapData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isDebug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isDebug = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mapOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.mapOffset = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_resRenderData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.resRenderData = (ResRenderData)translator.GetObject(L, 2, typeof(ResRenderData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mainCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mainCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_testLod(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.testLod = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MapData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.BigWorldRender gen_to_be_invoked = (World.BigWorldRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MapData = (World.WorldZoneMapData)translator.GetObject(L, 2, typeof(World.WorldZoneMapData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
