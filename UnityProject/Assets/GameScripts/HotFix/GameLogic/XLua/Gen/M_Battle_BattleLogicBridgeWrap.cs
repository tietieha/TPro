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
    public class MBattleBattleLogicBridgeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.Battle.BattleLogicBridge);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unit_GetWillAttackSource_All", _m_Unit_GetWillAttackSource_All);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 122, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleMap_Init", _m_BattleMap_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitUnitAndTeam", _m_InitUnitAndTeam_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BeforeBattle", _m_BeforeBattle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReleaseMap", _m_ReleaseMap_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitSingleTeam", _m_InitSingleTeam_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleMapTick", _m_BattleMapTick_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitSoldier", _m_SetUnitSoldier_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnit", _m_GetUnit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitUpgradeHero", _m_SetUnitUpgradeHero_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleMapConfigAddUnit", _m_BattleMapConfigAddUnit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReviveUnit", _m_ReviveUnit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReviveVirtualHero", _m_ReviveVirtualHero_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearBattleMapConfig", _m_ClearBattleMapConfig_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddNewTeamByBattleMapConfig", _m_AddNewTeamByBattleMapConfig_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitAttackRange", _m_SetUnitAttackRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsUnitAttackTargetInRange", _m_IsUnitAttackTargetInRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitAttackRange", _m_GetUnitAttackRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitFaceDirToTarget", _m_SetUnitFaceDirToTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "unitCurrentStateIs", _m_unitCurrentStateIs_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitSpeed", _m_SetUnitSpeed_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitSpeed", _m_GetUnitSpeed_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitTarget", _m_SetUnitTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitTargetUnit", _m_SetUnitTargetUnit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitDead", _m_SetUnitDead_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_MoveToTargetById", _m_ChangeState_MoveToTargetById_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_MoveStraight", _m_ChangeState_MoveStraight_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_MoveTo", _m_ChangeState_MoveTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_FollowHero", _m_ChangeState_FollowHero_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_SearchEnemy", _m_ChangeState_SearchEnemy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_Stop", _m_ChangeState_Stop_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_Stop_Attack", _m_ChangeState_Stop_Attack_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_WaitVirtualHeroAssignTarget", _m_ChangeState_WaitVirtualHeroAssignTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_Attack", _m_ChangeState_Attack_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_JumpTo", _m_ChangeState_JumpTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetJumpTarget", _m_GetJumpTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsJumpReachTarget", _m_IsJumpReachTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_StopVoid", _m_ChangeState_StopVoid_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTeamRadius", _m_GetTeamRadius_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSurroundingPositions", _m_GetSurroundingPositions_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsMoveStraightReachTarget", _m_IsMoveStraightReachTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetReachTargetSt", _m_GetReachTargetSt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTargetInRange_TargetTeam", _m_GetTargetInRange_TargetTeam_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTargetInRange", _m_GetTargetInRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetWillAttackTargetId", _m_GetWillAttackTargetId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTargetInRangeNotVirtualHero", _m_GetTargetInRangeNotVirtualHero_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_VirtualHeroMovePathTo", _m_ChangeState_VirtualHeroMovePathTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_VirtualHeroAssignTarget", _m_ChangeState_VirtualHeroAssignTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_SoldierFollowVirtualHero", _m_ChangeState_SoldierFollowVirtualHero_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_SoldierSearchEnemy", _m_ChangeState_SoldierSearchEnemy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetAttackSource", _m_Team_GetAttackSource_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetAttackSourceCount", _m_Team_GetAttackSourceCount_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_MoveWithTurnAngle", _m_ChangeState_MoveWithTurnAngle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_HorseFollowVirtualHero", _m_ChangeState_HorseFollowVirtualHero_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_HeroMovePathTo", _m_ChangeState_HeroMovePathTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_HeroMoveStraightTo", _m_ChangeState_HeroMoveStraightTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_HeroSearchEnemy", _m_ChangeState_HeroSearchEnemy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeState_SoldierDashMove", _m_ChangeState_SoldierDashMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CleanTargetByTeamIndex", _m_CleanTargetByTeamIndex_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAttackDistanceToTarget", _m_GetAttackDistanceToTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_GetWillAttackSource_Melee", _m_Unit_GetWillAttackSource_Melee_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_GetWillAttackSource_Melee_AnyOne", _m_Unit_GetWillAttackSource_Melee_AnyOne_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_GetWillAttackSource_LongRange", _m_Unit_GetWillAttackSource_LongRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_All", _m_Team_GetWillAttackSource_All_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_Melee", _m_Team_GetWillAttackSource_Melee_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_MeleeOne", _m_Team_GetWillAttackSource_MeleeOne_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_Melee_Count", _m_Team_GetWillAttackSource_Melee_Count_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_Melee_InRange", _m_Team_GetWillAttackSource_Melee_InRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_Is_Target_InRange", _m_Unit_Is_Target_InRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_LongRange", _m_Team_GetWillAttackSource_LongRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetWillAttackSource_LongRange_Count", _m_Team_GetWillAttackSource_LongRange_Count_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_GetDashEnemyList", _m_Unit_GetDashEnemyList_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_GetDashDamageEnemyList", _m_Unit_GetDashDamageEnemyList_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_GetFacePos", _m_Unit_GetFacePos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Get_Unit_newHeroTarget", _m_Get_Unit_newHeroTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Get_Unit_isHeroWarningDis", _m_Get_Unit_isHeroWarningDis_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Team_GetNearestTeam", _m_Team_GetNearestTeam_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_ChangeTarget", _m_Unit_ChangeTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_PeekPriorityTarget", _m_Unit_PeekPriorityTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_RemovePriorityTargetById", _m_Unit_RemovePriorityTargetById_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_AddFrontPriorityTarget", _m_Unit_AddFrontPriorityTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_CleanPriorityTarget", _m_Unit_CleanPriorityTarget_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_AddSkillWillAttack", _m_Unit_AddSkillWillAttack_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_PeekPriorityTarget_WillAttackSource_Melee_AnyOne", _m_Unit_PeekPriorityTarget_WillAttackSource_Melee_AnyOne_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unit_PeekPriorityTarget_WillAttackSource_InRange_AnyOne", _m_Unit_PeekPriorityTarget_WillAttackSource_InRange_AnyOne_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsUnitAttackNodeInRange", _m_IsUnitAttackNodeInRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGridNodePos", _m_GetGridNodePos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnitCanMove", _m_UnitCanMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMoveTargetTeamId_TargetTeam", _m_GetMoveTargetTeamId_TargetTeam_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitsInRadius", _m_GetUnitsInRadius_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitsInCircle", _m_GetUnitsInCircle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTwoUnitAngle", _m_GetTwoUnitAngle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetWorldPos", _m_GetWorldPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetNearestUnitId", _m_GetNearestUnitId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleMap_InitRvo", _m_BattleMap_InitRvo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleAddUnitRvo", _m_BattleAddUnitRvo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleAddVirtualHeroRvo", _m_BattleAddVirtualHeroRvo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleMapRvoTick", _m_BattleMapRvoTick_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetTargetId", _m_BattleRvoSetTargetId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoStopMove", _m_BattleRvoStopMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitsInRadiusRvo", _m_GetUnitsInRadiusRvo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUnitsInCircleRvo", _m_GetUnitsInCircleRvo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUnitDeadRvo", _m_SetUnitDeadRvo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetAgentAttackRange", _m_SetAgentAttackRange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetAgentStopDis", _m_SetAgentStopDis_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetTargetSpeed", _m_BattleRvoSetTargetSpeed_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetTargetAvoidRatio", _m_BattleRvoSetTargetAvoidRatio_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetTargetPosition", _m_BattleRvoSetTargetPosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetAgentRatio", _m_BattleRvoSetAgentRatio_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoGetAgentsDistance", _m_BattleRvoGetAgentsDistance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoIsAgentValid", _m_BattleRvoIsAgentValid_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoBuildAgentTree", _m_BattleRvoBuildAgentTree_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoAgentRevive", _m_BattleRvoAgentRevive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoDeadKdTreeUpdate", _m_BattleRvoDeadKdTreeUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoAfterDeadAgentRevive", _m_BattleRvoAfterDeadAgentRevive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetAgentPosition", _m_BattleRvoSetAgentPosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoSetAgentAIType", _m_BattleRvoSetAgentAIType_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoAgentEncircled", _m_BattleRvoAgentEncircled_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoNeedChangeNearestAgent", _m_BattleRvoNeedChangeNearestAgent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoGetAgentsInRotatedSector", _m_BattleRvoGetAgentsInRotatedSector_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BattleRvoGetAgentsInRotatedRect", _m_BattleRvoGetAgentsInRotatedRect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAngleFromPoints", _m_GetAngleFromPoints_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new M.Battle.BattleLogicBridge();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattleLogicBridge constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleMap_Init_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.BattleEntityPoolManager _mgr = (M.PathFinding.BattleEntityPoolManager)translator.GetObject(L, 2, typeof(M.PathFinding.BattleEntityPoolManager));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _width = LuaAPI.xlua_tointeger(L, 5);
                    int _height = LuaAPI.xlua_tointeger(L, 6);
                    int _gridSize = LuaAPI.xlua_tointeger(L, 7);
                    int _frameTime = LuaAPI.xlua_tointeger(L, 8);
                    int _maxPathPosDis = LuaAPI.xlua_tointeger(L, 9);
                    
                    M.Battle.BattleLogicBridge.BattleMap_Init( _battleMap, _mgr, _x, _y, _width, _height, _gridSize, _frameTime, _maxPathPosDis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitUnitAndTeam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.BattleMapConfig _mapConfig = (M.PathFinding.BattleMapConfig)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMapConfig));
                    
                    M.Battle.BattleLogicBridge.InitUnitAndTeam( _battleMap, _mapConfig );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BeforeBattle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _randSeed = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.BeforeBattle( _battleMap, _randSeed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseMap_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    
                    M.Battle.BattleLogicBridge.ReleaseMap( _battleMap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitSingleTeam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _side = LuaAPI.xlua_tointeger(L, 2);
                    int _teamIdx = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.InitSingleTeam( _battleMap, _side, _teamIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleMapTick_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    
                    M.Battle.BattleLogicBridge.BattleMapTick( _battleMap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitSoldier_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.SetUnitSoldier( _battleMap, _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnit_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnit( _battleMap, _id );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitUpgradeHero_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.SetUnitUpgradeHero( _battleMap, _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleMapConfigAddUnit_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMapConfig _mapConfig = (M.PathFinding.BattleMapConfig)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMapConfig));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _id = LuaAPI.xlua_tointeger(L, 4);
                    int _teamId = LuaAPI.xlua_tointeger(L, 5);
                    int _side = LuaAPI.xlua_tointeger(L, 6);
                    bool _isHero = LuaAPI.lua_toboolean(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    bool _isInstrument = LuaAPI.lua_toboolean(L, 9);
                    bool _isMelee = LuaAPI.lua_toboolean(L, 10);
                    double _speed = LuaAPI.lua_tonumber(L, 11);
                    double _attackRange = LuaAPI.lua_tonumber(L, 12);
                    double _radius = LuaAPI.lua_tonumber(L, 13);
                    int _extend = LuaAPI.xlua_tointeger(L, 14);
                    int _greenCircleRadius = LuaAPI.xlua_tointeger(L, 15);
                    int _meleeStandNum = LuaAPI.xlua_tointeger(L, 16);
                    int _rangeStandNum = LuaAPI.xlua_tointeger(L, 17);
                    LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 18, typeof(LuaArrAccess));
                    
                    M.Battle.BattleLogicBridge.BattleMapConfigAddUnit( _mapConfig, _x, _y, _id, _teamId, _side, _isHero, _isVirtual, _isInstrument, _isMelee, _speed, _attackRange, _radius, _extend, _greenCircleRadius, _meleeStandNum, _rangeStandNum, _luaArray );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReviveUnit_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _newId = LuaAPI.xlua_tointeger(L, 2);
                    int _posx;
                    int _posy;
                    
                    M.Battle.BattleLogicBridge.ReviveUnit( _unit, _newId, out _posx, out _posy );
                    LuaAPI.xlua_pushinteger(L, _posx);
                        
                    LuaAPI.xlua_pushinteger(L, _posy);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReviveVirtualHero_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _posx;
                    int _posy;
                    
                    M.Battle.BattleLogicBridge.ReviveVirtualHero( _unit, out _posx, out _posy );
                    LuaAPI.xlua_pushinteger(L, _posx);
                        
                    LuaAPI.xlua_pushinteger(L, _posy);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearBattleMapConfig_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMapConfig _mapConfig = (M.PathFinding.BattleMapConfig)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMapConfig));
                    
                    M.Battle.BattleLogicBridge.ClearBattleMapConfig( _mapConfig );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddNewTeamByBattleMapConfig_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.BattleMapConfig _mapConfig = (M.PathFinding.BattleMapConfig)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMapConfig));
                    
                    M.Battle.BattleLogicBridge.AddNewTeamByBattleMapConfig( _battleMap, _mapConfig );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitAttackRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _range = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.SetUnitAttackRange( _unit, _range );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUnitAttackTargetInRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    M.PathFinding.Unit _target = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.IsUnitAttackTargetInRange( _unit, _target );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitAttackRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitAttackRange( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitFaceDirToTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.SetUnitFaceDirToTarget( _unit, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_unitCurrentStateIs_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    M.PathFinding.EUnitState _st;translator.Get(L, 2, out _st);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.unitCurrentStateIs( _unit, _st );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitSpeed_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    double _speed = LuaAPI.lua_tonumber(L, 2);
                    
                    M.Battle.BattleLogicBridge.SetUnitSpeed( _unit, _speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitSpeed_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitSpeed( _unit );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.SetUnitTarget( _unit, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitTargetUnit_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    M.PathFinding.Unit _target = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.SetUnitTargetUnit( _unit, _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitDead_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _isDie = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.SetUnitDead( _unit, _isDie );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveToTargetById_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetId = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_MoveToTargetById( _unit, _targetId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveStraight_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.ChangeState_MoveStraight( _unit, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _speed = LuaAPI.xlua_tointeger(L, 4);
                    int _isMoveToPos = LuaAPI.xlua_tointeger(L, 5);
                    
                    M.Battle.BattleLogicBridge.ChangeState_MoveTo( _unit, _x, _y, _speed, _isMoveToPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_FollowHero_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.ChangeState_FollowHero( _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SearchEnemy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _teamIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_SearchEnemy( _unit, _teamIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_Stop_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.ChangeState_Stop( _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_Stop_Attack_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_Stop_Attack( _unit, _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_WaitVirtualHeroAssignTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.ChangeState_WaitVirtualHeroAssignTarget( _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_Attack_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_Attack( _unit, _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_JumpTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.ChangeState_JumpTo( _unit, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetJumpTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x;
                    int _y;
                    
                    M.Battle.BattleLogicBridge.GetJumpTarget( _unit, out _x, out _y );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsJumpReachTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.IsJumpReachTarget( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_StopVoid_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.ChangeState_StopVoid( _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTeamRadius_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetTeamRadius( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSurroundingPositions_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _radius = LuaAPI.xlua_tointeger(L, 4);
                    int _angleStart = LuaAPI.xlua_tointeger(L, 5);
                    int _angle = LuaAPI.xlua_tointeger(L, 6);
                    int _minNum = LuaAPI.xlua_tointeger(L, 7);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetSurroundingPositions( _unit, _x, _y, _radius, _angleStart, _angle, _minNum );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsMoveStraightReachTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.IsMoveStraightReachTarget( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetReachTargetSt_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetReachTargetSt( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetInRange_TargetTeam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetTargetInRange_TargetTeam( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetInRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetTargetInRange( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWillAttackTargetId_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetWillAttackTargetId( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetInRangeNotVirtualHero_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetTargetInRangeNotVirtualHero( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_VirtualHeroMovePathTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_VirtualHeroMovePathTo( _unit, _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_VirtualHeroAssignTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _attackTargetVirtualHeroId = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_VirtualHeroAssignTarget( _unit, _attackTargetVirtualHeroId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SoldierFollowVirtualHero_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.ChangeState_SoldierFollowVirtualHero( _unit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SoldierSearchEnemy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_SoldierSearchEnemy( _unit, _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetAttackSource_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetAttackSource( _battleMap, _teamIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetAttackSourceCount_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetAttackSourceCount( _battleMap, _teamIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveWithTurnAngle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetX = LuaAPI.xlua_tointeger(L, 2);
                    int _targetY = LuaAPI.xlua_tointeger(L, 3);
                    int _initDirInRadian = LuaAPI.xlua_tointeger(L, 4);
                    int _turnAngleSpeedRadianPerSecend = LuaAPI.xlua_tointeger(L, 5);
                    int _circleRadius = LuaAPI.xlua_tointeger(L, 6);
                    
                    M.Battle.BattleLogicBridge.ChangeState_MoveWithTurnAngle( _unit, _targetX, _targetY, _initDirInRadian, _turnAngleSpeedRadianPerSecend, _circleRadius );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HorseFollowVirtualHero_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _initDirInRadian = LuaAPI.xlua_tointeger(L, 2);
                    int _turnAngleSpeedRadianPerSecend = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.ChangeState_HorseFollowVirtualHero( _unit, _initDirInRadian, _turnAngleSpeedRadianPerSecend );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HeroMovePathTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetUnitId = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_HeroMovePathTo( _unit, _targetUnitId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HeroMoveStraightTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.ChangeState_HeroMoveStraightTo( _unit, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HeroSearchEnemy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.ChangeState_HeroSearchEnemy( _unit, _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SoldierDashMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _faceOffSet = LuaAPI.xlua_tointeger(L, 2);
                    int _dashRadius = LuaAPI.xlua_tointeger(L, 3);
                    int _damageRadius = LuaAPI.xlua_tointeger(L, 4);
                    int _dashJumpDamgeTime = LuaAPI.xlua_tointeger(L, 5);
                    int _dashHeroExtraW = LuaAPI.xlua_tointeger(L, 6);
                    int _dashHeroExtraH = LuaAPI.xlua_tointeger(L, 7);
                    int _maxSlidDur = LuaAPI.xlua_tointeger(L, 8);
                    
                    M.Battle.BattleLogicBridge.ChangeState_SoldierDashMove( _unit, _faceOffSet, _dashRadius, _damageRadius, _dashJumpDamgeTime, _dashHeroExtraW, _dashHeroExtraH, _maxSlidDur );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CleanTargetByTeamIndex_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _battleMap = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.CleanTargetByTeamIndex( _battleMap, _teamIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackDistanceToTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    M.PathFinding.Unit _tarUnit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetAttackDistanceToTarget( _unit, _tarUnit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetWillAttackSource_All(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattleLogicBridge gen_to_be_invoked = (M.Battle.BattleLogicBridge)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 3, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = gen_to_be_invoked.Unit_GetWillAttackSource_All( _map, _unit );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetWillAttackSource_Melee_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_GetWillAttackSource_Melee( _map, _unit );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetWillAttackSource_Melee_AnyOne_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_GetWillAttackSource_Melee_AnyOne( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetWillAttackSource_LongRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_GetWillAttackSource_LongRange( _map, _unit );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_All_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_All( _map, _teamId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_Melee_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_Melee( _map, _teamId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_MeleeOne_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_MeleeOne( _map, _teamId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_Melee_Count_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_Melee_Count( _map, _teamId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_Melee_InRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_Melee_InRange( _map, _teamId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_Is_Target_InRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _uId = LuaAPI.xlua_tointeger(L, 2);
                    int _targetVirtualIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_Is_Target_InRange( _map, _uId, _targetVirtualIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_LongRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_LongRange( _map, _teamId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetWillAttackSource_LongRange_Count_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetWillAttackSource_LongRange_Count( _map, _teamId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetDashEnemyList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_GetDashEnemyList( _map, _unitId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetDashDamageEnemyList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_GetDashDamageEnemyList( _map, _unitId );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_GetFacePos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleLogicBridge.Unit_GetFacePos( _u, ref _x, ref _y );
                    LuaAPI.lua_pushnumber(L, _x);
                        
                    LuaAPI.lua_pushnumber(L, _y);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get_Unit_newHeroTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _warningDis = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Get_Unit_newHeroTarget( _u, _warningDis );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get_Unit_isHeroWarningDis_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _target = LuaAPI.xlua_tointeger(L, 2);
                    int _warningDis = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Get_Unit_isHeroWarningDis( _u, _target, _warningDis );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Team_GetNearestTeam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Team_GetNearestTeam( _u );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_ChangeTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _target = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.Unit_ChangeTarget( _u, _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_PeekPriorityTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_PeekPriorityTarget( _u );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_RemovePriorityTargetById_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _targetId = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.Unit_RemovePriorityTargetById( _u, _targetId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_AddFrontPriorityTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _target = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.Unit_AddFrontPriorityTarget( _u, _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_CleanPriorityTarget_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                    M.Battle.BattleLogicBridge.Unit_CleanPriorityTarget( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_AddSkillWillAttack_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _tarIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.Unit_AddSkillWillAttack( _u, _tarIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_PeekPriorityTarget_WillAttackSource_Melee_AnyOne_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_PeekPriorityTarget_WillAttackSource_Melee_AnyOne( _u );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unit_PeekPriorityTarget_WillAttackSource_InRange_AnyOne_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.Unit_PeekPriorityTarget_WillAttackSource_InRange_AnyOne( _u );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUnitAttackNodeInRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.IsUnitAttackNodeInRange( _unit, _x, _y );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridNodePos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _ox;
                    int _oy;
                    
                    M.Battle.BattleLogicBridge.GetGridNodePos( _map, _x, _y, out _ox, out _oy );
                    LuaAPI.xlua_pushinteger(L, _ox);
                        
                    LuaAPI.xlua_pushinteger(L, _oy);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnitCanMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _bCan = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.UnitCanMove( _u, _bCan );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMoveTargetTeamId_TargetTeam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetMoveTargetTeamId_TargetTeam( _unit );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitsInRadius_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    M.PathFinding.Unit _unit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    int _radius = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitsInRadius( _map, _unit, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitsInCircle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 1, typeof(M.PathFinding.BattleMap));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _radius = LuaAPI.xlua_tointeger(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitsInCircle( _map, _x, _y, _radius, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTwoUnitAngle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    M.PathFinding.Unit _u2 = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetTwoUnitAngle( _u, _u2 );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWorldPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetWorldPos( _u );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNearestUnitId_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 1, typeof(M.PathFinding.Unit));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetNearestUnitId( _u, _x, _y );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleMap_InitRvo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _width = LuaAPI.xlua_tointeger(L, 4);
                    int _height = LuaAPI.xlua_tointeger(L, 5);
                    float _timeStep = (float)LuaAPI.lua_tonumber(L, 6);
                    float _neighborDist = (float)LuaAPI.lua_tonumber(L, 7);
                    int _maxNeighbors = LuaAPI.xlua_tointeger(L, 8);
                    float _timeHorizon = (float)LuaAPI.lua_tonumber(L, 9);
                    float _timeHorizonObst = (float)LuaAPI.lua_tonumber(L, 10);
                    
                    M.Battle.BattleLogicBridge.BattleMap_InitRvo( _simulator, _x, _y, _width, _height, _timeStep, _neighborDist, _maxNeighbors, _timeHorizon, _timeHorizonObst );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleAddUnitRvo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    int _id = LuaAPI.xlua_tointeger(L, 4);
                    int _teamId = LuaAPI.xlua_tointeger(L, 5);
                    int _side = LuaAPI.xlua_tointeger(L, 6);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 7);
                    bool _isMelee = LuaAPI.lua_toboolean(L, 8);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 9);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 10);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 11);
                    float _stopDis = (float)LuaAPI.lua_tonumber(L, 12);
                    float _avoidanceRatio = (float)LuaAPI.lua_tonumber(L, 13);
                    int _groupId = LuaAPI.xlua_tointeger(L, 14);
                    LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 15, typeof(LuaArrAccess));
                    int _agentType = LuaAPI.xlua_tointeger(L, 16);
                    
                    M.Battle.BattleLogicBridge.BattleAddUnitRvo( _simulator, _x, _y, _id, _teamId, _side, _isVirtual, _isMelee, _speed, _attackRange, _radius, _stopDis, _avoidanceRatio, _groupId, _luaArray, _agentType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleAddVirtualHeroRvo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    int _id = LuaAPI.xlua_tointeger(L, 4);
                    int _teamId = LuaAPI.xlua_tointeger(L, 5);
                    int _side = LuaAPI.xlua_tointeger(L, 6);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 7);
                    bool _isMelee = LuaAPI.lua_toboolean(L, 8);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 9);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 10);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 11);
                    float _stopDis = (float)LuaAPI.lua_tonumber(L, 12);
                    float _avoidanceRatio = (float)LuaAPI.lua_tonumber(L, 13);
                    LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 14, typeof(LuaArrAccess));
                    int _agentType = LuaAPI.xlua_tointeger(L, 15);
                    float _neighborDist = (float)LuaAPI.lua_tonumber(L, 16);
                    int _maxNeighbors = LuaAPI.xlua_tointeger(L, 17);
                    float _timeHorizon = (float)LuaAPI.lua_tonumber(L, 18);
                    
                    M.Battle.BattleLogicBridge.BattleAddVirtualHeroRvo( _simulator, _x, _y, _id, _teamId, _side, _isVirtual, _isMelee, _speed, _attackRange, _radius, _stopDis, _avoidanceRatio, _luaArray, _agentType, _neighborDist, _maxNeighbors, _timeHorizon );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleMapRvoTick_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    
                    M.Battle.BattleLogicBridge.BattleMapRvoTick( _simulator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetTargetId_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    bool _isFollow = LuaAPI.lua_toboolean(L, 4);
                    bool _isFirst = LuaAPI.lua_toboolean(L, 5);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetTargetId( _simulator, _id, _targetId, _isFollow, _isFirst );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    bool _isFollow = LuaAPI.lua_toboolean(L, 4);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetTargetId( _simulator, _id, _targetId, _isFollow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetTargetId( _simulator, _id, _targetId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattleLogicBridge.BattleRvoSetTargetId!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoStopMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.BattleRvoStopMove( _simulator, _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitsInRadiusRvo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitsInRadiusRvo( _simulator, _agentId, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitsInCircleRvo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 6);
                    bool _containDead = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitsInCircleRvo( _simulator, _x, _y, _radius, _side, _isVirtual, _containDead );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitsInCircleRvo( _simulator, _x, _y, _radius, _side, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetUnitsInCircleRvo( _simulator, _x, _y, _radius, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattleLogicBridge.GetUnitsInCircleRvo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUnitDeadRvo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.SetUnitDeadRvo( _simulator, _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentAttackRange_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    int _range = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.SetAgentAttackRange( _simulator, _id, _range );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentStopDis_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _stopDis = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleLogicBridge.SetAgentStopDis( _simulator, _id, _stopDis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetTargetSpeed_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetTargetSpeed( _simulator, _id, _speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetTargetAvoidRatio_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _ratio = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetTargetAvoidRatio( _simulator, _id, _ratio );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetTargetPosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetTargetPosition( _simulator, _id, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetAgentRatio_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _ratio = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetAgentRatio( _simulator, _id, _ratio );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoGetAgentsDistance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentIdA = LuaAPI.xlua_tointeger(L, 2);
                    int _agentIdB = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsDistance( _simulator, _agentIdA, _agentIdB );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoIsAgentValid_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoIsAgentValid( _simulator, _agentId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoBuildAgentTree_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    
                    M.Battle.BattleLogicBridge.BattleRvoBuildAgentTree( _simulator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoAgentRevive_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    
                    M.Battle.BattleLogicBridge.BattleRvoAgentRevive( _simulator, _agentId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoDeadKdTreeUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    
                    M.Battle.BattleLogicBridge.BattleRvoDeadKdTreeUpdate( _simulator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoAfterDeadAgentRevive_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    
                    M.Battle.BattleLogicBridge.BattleRvoAfterDeadAgentRevive( _simulator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetAgentPosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetAgentPosition( _simulator, _agentId, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoSetAgentAIType_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    int _aiType = LuaAPI.xlua_tointeger(L, 3);
                    
                    M.Battle.BattleLogicBridge.BattleRvoSetAgentAIType( _simulator, _agentId, _aiType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoAgentEncircled_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoAgentEncircled( _simulator, _agentId, _attackRange );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoNeedChangeNearestAgent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoNeedChangeNearestAgent( _simulator, _agentId, _targetId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoGetAgentsInRotatedSector_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 9&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    bool _containDead = LuaAPI.lua_toboolean(L, 9);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedSector( _simulator, _x, _y, _radius, _angle, _angleRad, _side, _isVirtual, _containDead );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedSector( _simulator, _x, _y, _radius, _angle, _angleRad, _side, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedSector( _simulator, _x, _y, _radius, _angle, _angleRad, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedSector( _simulator, _x, _y, _radius, _angle, _angleRad );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedSector!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BattleRvoGetAgentsInRotatedRect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 9&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    bool _containDead = LuaAPI.lua_toboolean(L, 9);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedRect( _simulator, _x, _y, _width, _height, _angleRad, _side, _isVirtual, _containDead );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedRect( _simulator, _x, _y, _width, _height, _angleRad, _side, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedRect( _simulator, _x, _y, _width, _height, _angleRad, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<RVO.Simulator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    RVO.Simulator _simulator = (RVO.Simulator)translator.GetObject(L, 1, typeof(RVO.Simulator));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedRect( _simulator, _x, _y, _width, _height, _angleRad );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattleLogicBridge.BattleRvoGetAgentsInRotatedRect!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAngleFromPoints_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 1);
                    float _y = (float)LuaAPI.lua_tonumber(L, 2);
                    float _x1 = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y1 = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = M.Battle.BattleLogicBridge.GetAngleFromPoints( _x, _y, _x1, _y1 );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
