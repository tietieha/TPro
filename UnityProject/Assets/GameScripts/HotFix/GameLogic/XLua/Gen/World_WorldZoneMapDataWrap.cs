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
    public class WorldWorldZoneMapDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(World.WorldZoneMapData);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 9, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Load", _m_Load);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InBlockingArea", _m_InBlockingArea);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridType", _m_GetGridType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InBlockingAreaByXy", _m_InBlockingAreaByXy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridZone", _m_GetGridZone);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridZoneByIndex", _m_GetGridZoneByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPortIndex", _m_GetPortIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IndexToXy", _m_IndexToXy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "XyToIndex", _m_XyToIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QrToIndex", _m_QrToIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DistanceByIndex", _m_DistanceByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "XyInMap", _m_XyInMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CouldStraightGoByQr", _m_CouldStraightGoByQr);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "width", _g_get_width);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "height", _g_get_height);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scale", _g_get_scale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "version", _g_get_version);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hexRadius", _g_get_hexRadius);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "zones", _g_get_zones);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "grid2zone", _g_get_grid2zone);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "grids", _g_get_grids);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "businessEdgeDatas", _g_get_businessEdgeDatas);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "width", _s_set_width);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "height", _s_set_height);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scale", _s_set_scale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "version", _s_set_version);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "hexRadius", _s_set_hexRadius);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "zones", _s_set_zones);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "grid2zone", _s_set_grid2zone);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "grids", _s_set_grids);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "businessEdgeDatas", _s_set_businessEdgeDatas);
            
			
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
					
					var gen_ret = new World.WorldZoneMapData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to World.WorldZoneMapData constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Load(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.IO.BinaryReader _br = (System.IO.BinaryReader)translator.GetObject(L, 2, typeof(System.IO.BinaryReader));
                    
                    gen_to_be_invoked.Load( _br );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InBlockingArea(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _q = LuaAPI.xlua_tointeger(L, 2);
                    int _r = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.InBlockingArea( _q, _r );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _q = LuaAPI.xlua_tointeger(L, 2);
                    int _r = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetGridType( _q, _r );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InBlockingAreaByXy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.InBlockingAreaByXy( _x, _y );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridZone(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _q = LuaAPI.xlua_tointeger(L, 2);
                    int _r = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetGridZone( _q, _r );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridZoneByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetGridZoneByIndex( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPortIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _zoneId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPortIndex( _zoneId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IndexToXy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.IndexToXy( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_XyToIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.XyToIndex( _x, _y );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<int[]>(L, 2)) 
                {
                    int[] _xy = (int[])translator.GetObject(L, 2, typeof(int[]));
                    
                        var gen_ret = gen_to_be_invoked.XyToIndex( _xy );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to World.WorldZoneMapData.XyToIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QrToIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _q = LuaAPI.xlua_tointeger(L, 2);
                    int _r = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.QrToIndex( _q, _r );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<int[]>(L, 2)) 
                {
                    int[] _qr = (int[])translator.GetObject(L, 2, typeof(int[]));
                    
                        var gen_ret = gen_to_be_invoked.QrToIndex( _qr );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to World.WorldZoneMapData.QrToIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DistanceByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _leftIndex = LuaAPI.xlua_tointeger(L, 2);
                    int _rightIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.DistanceByIndex( _leftIndex, _rightIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_XyInMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int[] _xy = (int[])translator.GetObject(L, 2, typeof(int[]));
                    
                        var gen_ret = gen_to_be_invoked.XyInMap( _xy );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CouldStraightGoByQr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int[] _sQr = (int[])translator.GetObject(L, 2, typeof(int[]));
                    int[] _eQr = (int[])translator.GetObject(L, 3, typeof(int[]));
                    
                        var gen_ret = gen_to_be_invoked.CouldStraightGoByQr( _sQr, _eQr );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_width(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.width);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_height(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.height);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.scale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_version(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.version);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hexRadius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.hexRadius);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_zones(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.zones);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_grid2zone(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.grid2zone);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_grids(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.grids);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_businessEdgeDatas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.businessEdgeDatas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_width(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.width = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_height(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.height = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.scale = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_version(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.version = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_hexRadius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.hexRadius = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_zones(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.zones = (System.Collections.Generic.Dictionary<int, WorldZoneData>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<int, WorldZoneData>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_grid2zone(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.grid2zone = (short[])translator.GetObject(L, 2, typeof(short[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_grids(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.grids = LuaAPI.lua_tobytes(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_businessEdgeDatas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.WorldZoneMapData gen_to_be_invoked = (World.WorldZoneMapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.businessEdgeDatas = (World.WorldBusinessEdgeData[])translator.GetObject(L, 2, typeof(World.WorldBusinessEdgeData[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
