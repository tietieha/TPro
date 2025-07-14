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
    public class WorldPathFinderStraightAStarPathfinderWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(World.PathFinder.StraightAStarPathfinder);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 11, 11);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToFindPathByQr", _m_ToFindPathByQr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToFindPathByXy", _m_ToFindPathByXy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindPath", _m_FindPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryGoStraight", _m_TryGoStraight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddNeighbors", _m_AddNeighbors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BuildPath", _m_BuildPath);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "path", _g_get_path);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pathDistance", _g_get_pathDistance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "findResult", _g_get_findResult);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sIndex", _g_get_sIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "eIndex", _g_get_eIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "eQr", _g_get_eQr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mapData", _g_get_mapData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "endNote", _g_get_endNote);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "openList", _g_get_openList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "closeSet", _g_get_closeSet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cannotGoStraightSet", _g_get_cannotGoStraightSet);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "path", _s_set_path);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pathDistance", _s_set_pathDistance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "findResult", _s_set_findResult);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sIndex", _s_set_sIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "eIndex", _s_set_eIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "eQr", _s_set_eQr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mapData", _s_set_mapData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "endNote", _s_set_endNote);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "openList", _s_set_openList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "closeSet", _s_set_closeSet);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cannotGoStraightSet", _s_set_cannotGoStraightSet);
            
			
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
					
					var gen_ret = new World.PathFinder.StraightAStarPathfinder();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to World.PathFinder.StraightAStarPathfinder constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    World.WorldZoneMapData _data = (World.WorldZoneMapData)translator.GetObject(L, 2, typeof(World.WorldZoneMapData));
                    
                    gen_to_be_invoked.Init( _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToFindPathByQr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _q = LuaAPI.xlua_tointeger(L, 2);
                    int _r = LuaAPI.xlua_tointeger(L, 3);
                    int _q1 = LuaAPI.xlua_tointeger(L, 4);
                    int _r1 = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.ToFindPathByQr( _q, _r, _q1, _r1 );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToFindPathByXy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _x1 = LuaAPI.xlua_tointeger(L, 4);
                    int _y1 = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.ToFindPathByXy( _x, _y, _x1, _y1 );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.FindPath(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryGoStraight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    World.PathFinder.FindPathNodeNote _curNote = (World.PathFinder.FindPathNodeNote)translator.GetObject(L, 2, typeof(World.PathFinder.FindPathNodeNote));
                    
                    gen_to_be_invoked.TryGoStraight( _curNote );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddNeighbors(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    World.PathFinder.FindPathNodeNote _curNote = (World.PathFinder.FindPathNodeNote)translator.GetObject(L, 2, typeof(World.PathFinder.FindPathNodeNote));
                    
                    gen_to_be_invoked.AddNeighbors( _curNote );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuildPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BuildPath(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_path(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.path);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pathDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.pathDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_findResult(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.findResult);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.sIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_eIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.eIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_eQr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.eQr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mapData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mapData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_endNote(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.endNote);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_openList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.openList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_closeSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.closeSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cannotGoStraightSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cannotGoStraightSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_path(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.path = (System.Collections.ArrayList)translator.GetObject(L, 2, typeof(System.Collections.ArrayList));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pathDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pathDistance = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_findResult(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                World.PathFinder.FindResult gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.findResult = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.sIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_eIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.eIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_eQr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.eQr = (int[])translator.GetObject(L, 2, typeof(int[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mapData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mapData = (World.WorldZoneMapData)translator.GetObject(L, 2, typeof(World.WorldZoneMapData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_endNote(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.endNote = (World.PathFinder.FindPathNodeNote)translator.GetObject(L, 2, typeof(World.PathFinder.FindPathNodeNote));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_openList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.openList = (PriorityQueue<World.PathFinder.FindPathNodeNote>)translator.GetObject(L, 2, typeof(PriorityQueue<World.PathFinder.FindPathNodeNote>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_closeSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.closeSet = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cannotGoStraightSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.PathFinder.StraightAStarPathfinder gen_to_be_invoked = (World.PathFinder.StraightAStarPathfinder)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cannotGoStraightSet = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
