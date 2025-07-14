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
    public class MPathFindingBattleMapWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.PathFinding.BattleMap);
			Utils.BeginObjectRegister(type, L, translator, 0, 48, 10, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidth", _m_GetWidth);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHeight", _m_GetHeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridSize", _m_GetGridSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridHalfSize", _m_GetGridHalfSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOrigiPos", _m_GetOrigiPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFrameDeltaMs", _m_GetFrameDeltaMs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNode2D", _m_GetNode2D);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBattleRandom", _m_GetBattleRandom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BeforeBattle", _m_BeforeBattle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Tick", _m_Tick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReleaseMap", _m_ReleaseMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetDebugger", _m_SetDebugger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NodeOccupyChanged", _m_NodeOccupyChanged);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClampWorldPosInMap", _m_ClampWorldPosInMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WorldPosToGrid", _m_WorldPosToGrid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridCenter", _m_GetGridCenter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsGridInMap", _m_IsGridInMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNode", _m_GetNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMapDebugger", _m_GetMapDebugger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnitIdsInRange", _m_GetUnitIdsInRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnitsInCircle", _m_GetUnitsInCircle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPathRequestFromCache", _m_GetPathRequestFromCache);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReturnPathRequestToCache", _m_ReturnPathRequestToCache);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_AddUnitOccupy_GridPos", _m_Internal_AddUnitOccupy_GridPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_RemoveUnitOccupy_GridPos", _m_Internal_RemoveUnitOccupy_GridPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindNearestNotOccupiedNode", _m_FindNearestNotOccupiedNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PushPathRequest", _m_PushPathRequest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindPath", _m_FindPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Raycast", _m_Raycast);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RaycastObstacled", _m_RaycastObstacled);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMapConfig", _m_GetMapConfig);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnitDict", _m_GetUnitDict);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTeam", _m_GetTeam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnit", _m_GetUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitUnitAndTeam", _m_InitUnitAndTeam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddExtenTeam", _m_AddExtenTeam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitAllUnitGridData", _m_InitAllUnitGridData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitSingleTeam", _m_InitSingleTeam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUnitHero", _m_SetUnitHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUnitSoldier", _m_SetUnitSoldier);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateLongAttackRelation", _m_CreateLongAttackRelation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveLongAttackReleation", _m_RemoveLongAttackReleation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QueryAttackTarget_HighPriority", _m_QueryAttackTarget_HighPriority);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unit_GetAttackSource", _m_Unit_GetAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Team_GetAttackSource", _m_Team_GetAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Team_GetAttackSourceCount", _m_Team_GetAttackSourceCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNearestTeam", _m_GetNearestTeam);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "MinWorldPosX", _g_get_MinWorldPosX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MinWorldPosY", _g_get_MinWorldPosY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxWorldPosX", _g_get_MaxWorldPosX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxWorldPosY", _g_get_MaxWorldPosY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BspTreeLeft", _g_get_BspTreeLeft);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BspTreeRight", _g_get_BspTreeRight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityPoolManager", _g_get_EntityPoolManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ProcessedPathThisFrame", _g_get_ProcessedPathThisFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ProcessedNodeThisFrame", _g_get_ProcessedNodeThisFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OutUnitIdList", _g_get_OutUnitIdList);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "BspTreeLeft", _s_set_BspTreeLeft);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BspTreeRight", _s_set_BspTreeRight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EntityPoolManager", _s_set_EntityPoolManager);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OutUnitIdList", _s_set_OutUnitIdList);
            
			
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
					
					var gen_ret = new M.PathFinding.BattleMap();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.BattleMap constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWidth(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetWidth(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetHeight(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetGridSize(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridHalfSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetGridHalfSize(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOrigiPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetOrigiPos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFrameDeltaMs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetFrameDeltaMs(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNode2D(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetNode2D(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBattleRandom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetBattleRandom(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleEntityPoolManager _mgr = (M.PathFinding.BattleEntityPoolManager)translator.GetObject(L, 2, typeof(M.PathFinding.BattleEntityPoolManager));
                    int _orginX = LuaAPI.xlua_tointeger(L, 3);
                    int _orginY = LuaAPI.xlua_tointeger(L, 4);
                    int _width = LuaAPI.xlua_tointeger(L, 5);
                    int _height = LuaAPI.xlua_tointeger(L, 6);
                    int _gridSizeFixIntScale = LuaAPI.xlua_tointeger(L, 7);
                    int _frameDeltaMs = LuaAPI.xlua_tointeger(L, 8);
                    int _maxPathPosDis = LuaAPI.xlua_tointeger(L, 9);
                    
                        var gen_ret = gen_to_be_invoked.Init( _mgr, _orginX, _orginY, _width, _height, _gridSizeFixIntScale, _frameDeltaMs, _maxPathPosDis );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BeforeBattle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _randSeed = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.BeforeBattle( _randSeed );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Tick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Tick(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReleaseMap(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetDebugger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.IBattleMapDebugger _debugger = (M.PathFinding.IBattleMapDebugger)translator.GetObject(L, 2, typeof(M.PathFinding.IBattleMapDebugger));
                    
                    gen_to_be_invoked.SetDebugger( _debugger );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NodeOccupyChanged(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Node _n = (M.PathFinding.Node)translator.GetObject(L, 2, typeof(M.PathFinding.Node));
                    
                    gen_to_be_invoked.NodeOccupyChanged( _n );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClampWorldPosInMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _worldPos;translator.Get(L, 2, out _worldPos);
                    
                        var gen_ret = gen_to_be_invoked.ClampWorldPosInMap( _worldPos );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldPosToGrid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _worldPos;translator.Get(L, 2, out _worldPos);
                    
                        var gen_ret = gen_to_be_invoked.WorldPosToGrid( _worldPos );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridCenter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetGridCenter( _x, _y );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsGridInMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.IsGridInMap( _x, _y );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetNode( _x, _y );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMapDebugger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMapDebugger(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitIdsInRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    int _radius = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetUnitIdsInRange( _unit, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitsInCircle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _radius = LuaAPI.xlua_tointeger(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.GetUnitsInCircle( _x, _y, _radius, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPathRequestFromCache(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPathRequestFromCache(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReturnPathRequestToCache(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.PathRequest _r = (M.PathFinding.PathRequest)translator.GetObject(L, 2, typeof(M.PathFinding.PathRequest));
                    
                    gen_to_be_invoked.ReturnPathRequestToCache( _r );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_AddUnitOccupy_GridPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    int _gridX = LuaAPI.xlua_tointeger(L, 3);
                    int _gridY = LuaAPI.xlua_tointeger(L, 4);
                    M.PathFinding.EUnitType _unitType;translator.Get(L, 5, out _unitType);
                    
                    gen_to_be_invoked.Internal_AddUnitOccupy_GridPos( _u, _gridX, _gridY, _unitType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_RemoveUnitOccupy_GridPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    int _gridX = LuaAPI.xlua_tointeger(L, 3);
                    int _gridY = LuaAPI.xlua_tointeger(L, 4);
                    M.PathFinding.EUnitType _unitType;translator.Get(L, 5, out _unitType);
                    
                    gen_to_be_invoked.Internal_RemoveUnitOccupy_GridPos( _u, _gridX, _gridY, _unitType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindNearestNotOccupiedNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<M.PathFinding.EUnitType>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    M.PathFinding.EUnitType _unitType;translator.Get(L, 4, out _unitType);
                    int _maxTestNum = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.FindNearestNotOccupiedNode( _x, _y, _unitType, _maxTestNum );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<M.PathFinding.EUnitType>(L, 4)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    M.PathFinding.EUnitType _unitType;translator.Get(L, 4, out _unitType);
                    
                        var gen_ret = gen_to_be_invoked.FindNearestNotOccupiedNode( _x, _y, _unitType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.BattleMap.FindNearestNotOccupiedNode!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PushPathRequest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.PathRequest _request = (M.PathFinding.PathRequest)translator.GetObject(L, 2, typeof(M.PathFinding.PathRequest));
                    
                    gen_to_be_invoked.PushPathRequest( _request );
                    
                    
                    
                    return 0;
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
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.PathRequest _request = (M.PathFinding.PathRequest)translator.GetObject(L, 2, typeof(M.PathFinding.PathRequest));
                    M.PathFinding.EUnitType _layerIndex;translator.Get(L, 3, out _layerIndex);
                    M.PathFinding.ELayerType _layerType;translator.Get(L, 4, out _layerType);
                    int _stopDis = LuaAPI.xlua_tointeger(L, 5);
                    int _maxTestedNodeCount = LuaAPI.xlua_tointeger(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.FindPath( _request, _layerIndex, _layerType, _stopDis, _maxTestedNodeCount );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Raycast(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.EUnitType _layerIndex;translator.Get(L, 2, out _layerIndex);
                    M.PathFinding.ELayerType _layerType;translator.Get(L, 3, out _layerType);
                    FixPoint.FixInt2 _startWorld;translator.Get(L, 4, out _startWorld);
                    FixPoint.FixInt2 _endWorld;translator.Get(L, 5, out _endWorld);
                    M.PathFinding.Integer2 _interGrid;translator.Get(L, 6, out _interGrid);
                    
                        var gen_ret = gen_to_be_invoked.Raycast( _layerIndex, _layerType, _startWorld, _endWorld, ref _interGrid );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _interGrid);
                        translator.Update(L, 6, _interGrid);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RaycastObstacled(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.EUnitState _state;translator.Get(L, 2, out _state);
                    M.PathFinding.Integer2 _startPosInGrid;translator.Get(L, 3, out _startPosInGrid);
                    M.PathFinding.Integer2 _endPosInGrid;translator.Get(L, 4, out _endPosInGrid);
                    M.PathFinding.Integer2 _interGrid;translator.Get(L, 5, out _interGrid);
                    M.PathFinding.Integer2 _openGrid;translator.Get(L, 6, out _openGrid);
                    
                        var gen_ret = gen_to_be_invoked.RaycastObstacled( _state, _startPosInGrid, _endPosInGrid, ref _interGrid, ref _openGrid );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _interGrid);
                        translator.Update(L, 5, _interGrid);
                        
                    translator.Push(L, _openGrid);
                        translator.Update(L, 6, _openGrid);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMapConfig(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMapConfig(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitDict(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetUnitDict(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetTeam( _teamId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetUnit( _unitId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitUnitAndTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleMapConfig _c = (M.PathFinding.BattleMapConfig)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMapConfig));
                    
                    gen_to_be_invoked.InitUnitAndTeam( _c );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddExtenTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleMapConfig _c = (M.PathFinding.BattleMapConfig)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMapConfig));
                    
                    gen_to_be_invoked.AddExtenTeam( _c );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitAllUnitGridData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitAllUnitGridData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitSingleTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _side = LuaAPI.xlua_tointeger(L, 2);
                    int _teamIdx = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.InitSingleTeam( _side, _teamIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.SetUnitHero( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitSoldier(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.SetUnitSoldier( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateLongAttackRelation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _srcTeamIndex = LuaAPI.xlua_tointeger(L, 2);
                    int _dstUnitIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.CreateLongAttackRelation( _srcTeamIndex, _dstUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveLongAttackReleation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _srcTeamIndex = LuaAPI.xlua_tointeger(L, 2);
                    int _dstUnitIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.RemoveLongAttackReleation( _srcTeamIndex, _dstUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryAttackTarget_HighPriority(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _srcTeamId = LuaAPI.xlua_tointeger(L, 2);
                    int _dstTeamId = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.QueryAttackTarget_HighPriority( _srcTeamId, _dstTeamId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = gen_to_be_invoked.Unit_GetAttackSource( _u );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Team _t = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
                    
                        var gen_ret = gen_to_be_invoked.Team_GetAttackSource( _t );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetAttackSourceCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Team _t = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
                    
                        var gen_ret = gen_to_be_invoked.Team_GetAttackSourceCount( _t );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNearestTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = gen_to_be_invoked.GetNearestTeam( _u );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MinWorldPosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MinWorldPosX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MinWorldPosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MinWorldPosY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxWorldPosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaxWorldPosX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxWorldPosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaxWorldPosY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BspTreeLeft(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BspTreeLeft);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BspTreeRight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BspTreeRight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityPoolManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityPoolManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ProcessedPathThisFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ProcessedPathThisFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ProcessedNodeThisFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ProcessedNodeThisFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OutUnitIdList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OutUnitIdList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BspTreeLeft(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BspTreeLeft = (M.PathFinding.BSPTree)translator.GetObject(L, 2, typeof(M.PathFinding.BSPTree));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BspTreeRight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BspTreeRight = (M.PathFinding.BSPTree)translator.GetObject(L, 2, typeof(M.PathFinding.BSPTree));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EntityPoolManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EntityPoolManager = (M.PathFinding.BattleEntityPoolManager)translator.GetObject(L, 2, typeof(M.PathFinding.BattleEntityPoolManager));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OutUnitIdList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMap gen_to_be_invoked = (M.PathFinding.BattleMap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OutUnitIdList = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
