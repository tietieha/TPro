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
    public class MPathFindingUnitWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.PathFinding.Unit);
			Utils.BeginObjectRegister(type, L, translator, 0, 136, 22, 20);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsMelee", _m_IsMelee);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitWithData", _m_InitWithData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMap", _m_GetMap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRealExtend", _m_GetRealExtend);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWorldPos", _m_GetWorldPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWorldPosX", _m_GetWorldPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWorldPosY", _m_GetWorldPosY);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGridPos", _m_GetGridPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetId", _m_GetId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsHero", _m_IsHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTeamId", _m_GetTeamId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsDead", _m_IsDead);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetDead", _m_SetDead);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSide", _m_GetSide);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAttackRange", _m_SetAttackRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAttackRangeRateOpen", _m_SetAttackRangeRateOpen);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsOpenDynamicAttackRange", _m_IsOpenDynamicAttackRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackRangeRate", _m_GetAttackRangeRate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackRange", _m_GetAttackRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPathFindingBlocked", _m_GetPathFindingBlocked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPathFindingBlocked", _m_SetPathFindingBlocked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPosRelativeToHero", _m_SetPosRelativeToHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosRelativeToHero", _m_GetPosRelativeToHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRadius", _m_GetRadius);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSqrRadius", _m_GetSqrRadius);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGreenCircleRadius", _m_GetGreenCircleRadius);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGreenCircleRadiusSqr", _m_GetGreenCircleRadiusSqr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsVirtual", _m_IsVirtual);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetVirtual", _m_SetVirtual);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnitType", _m_GetUnitType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFaceDirToTarget", _m_SetFaceDirToTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFaceDirToTargetVec2", _m_SetFaceDirToTargetVec2);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Tick", _m_Tick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSpeed", _m_SetSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClampPos", _m_ClampPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSpeed", _m_GetSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CanTarget", _m_CanTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CanSelect", _m_CanSelect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsMoveStraightReachTarget", _m_IsMoveStraightReachTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetReachTargetSt", _m_GetReachTargetSt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsJumpReachTarget", _m_IsJumpReachTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetJumpTarget", _m_GetJumpTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_SetHero", _m_Internal_SetHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StandAtGridCenter", _m_StandAtGridCenter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Log", _m_Log);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackSourceDict", _m_GetAttackSourceDict);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddLongAttackSource", _m_AddLongAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveLongAttackSource", _m_RemoveLongAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddMeleeAttackSource", _m_AddMeleeAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveMeleeAttackSource", _m_RemoveMeleeAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMeleeAttackSourceCount", _m_GetMeleeAttackSourceCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTargetAttackRangeAddByHisSourceCount", _m_GetTargetAttackRangeAddByHisSourceCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsAttackTargetInMyRange", _m_IsAttackTargetInMyRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsAttackNodeInMyRange", _m_IsAttackNodeInMyRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackDistanceToNode", _m_GetAttackDistanceToNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackDistanceToTarget", _m_GetAttackDistanceToTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAttackDistanceToTargetByIdx", _m_GetAttackDistanceToTargetByIdx);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsMeInVirtualHeroGreenCircle", _m_IsMeInVirtualHeroGreenCircle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsPositionInMyGreenCircle", _m_IsPositionInMyGreenCircle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsPositionInHeroGreenCircle", _m_IsPositionInHeroGreenCircle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateMovingTargetWithTurnAngleSpeed", _m_CalculateMovingTargetWithTurnAngleSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReviveUnit", _m_ReviveUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReviveVirtualHero", _m_ReviveVirtualHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFacePos", _m_GetFacePos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeTarget", _m_ChangeTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddFrontPriorityTarget", _m_AddFrontPriorityTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AppendPriorityTarget", _m_AppendPriorityTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AppendAndCheckPriorityTarget", _m_AppendAndCheckPriorityTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DequeuePriorityTarget", _m_DequeuePriorityTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PeekPriorityTarget", _m_PeekPriorityTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemovePriorityTargetById", _m_RemovePriorityTargetById);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CleanPriorityTarget", _m_CleanPriorityTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTeamRadius", _m_GetTeamRadius);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSurroundPositions", _m_GetSurroundPositions);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNearestUnitId", _m_GetNearestUnitId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetWillAttackTarget", _m_SetWillAttackTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearWillAttackTarget", _m_ClearWillAttackTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMeleeWillAttackSource", _m_GetMeleeWillAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLongRangeWillAttackSource", _m_GetLongRangeWillAttackSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRealExtendOnLayer", _m_GetRealExtendOnLayer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsOccupiedGird", _m_IsOccupiedGird);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOccupiedGrid", _m_GetOccupiedGrid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_RemoveCurrentGridOccupy", _m_Internal_RemoveCurrentGridOccupy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_ChangeOccupyGrid", _m_Internal_ChangeOccupyGrid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InternalEnableOccupy_Stand", _m_InternalEnableOccupy_Stand);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_RemoveCurrentGridOccupy_Temp", _m_Internal_RemoveCurrentGridOccupy_Temp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Internal_AddCurrentGridOccupy_Temp", _m_Internal_AddCurrentGridOccupy_Temp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsCurrStateCanMove", _m_IsCurrStateCanMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendPathFindingRequest_OneUnit", _m_SendPathFindingRequest_OneUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendPathFindingRequest_Team", _m_SendPathFindingRequest_Team);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendPathFindingRequest_Position", _m_SendPathFindingRequest_Position);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMinDistanceEnemy", _m_GetMinDistanceEnemy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMinDistanceAliveEnemy", _m_GetMinDistanceAliveEnemy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMinDistanceFriend", _m_GetMinDistanceFriend);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTargetTeamMinDistanceEnemy", _m_GetTargetTeamMinDistanceEnemy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ComputeMoveDisInOneFrame", _m_ComputeMoveDisInOneFrame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ComputeMoveDisInOneFrameBySpeed", _m_ComputeMoveDisInOneFrameBySpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryToMove", _m_TryToMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryToMoveBySpeed", _m_TryToMoveBySpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryToMoveAndReach", _m_TryToMoveAndReach);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SysPos", _m_SysPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryToMove_JumpTo", _m_TryToMove_JumpTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_Stop", _m_ChangeState_Stop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_StopAttackTo", _m_ChangeState_StopAttackTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_Attack", _m_ChangeState_Attack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_SearchEnemy", _m_ChangeState_SearchEnemy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTargetInRange_TargetTeam", _m_GetTargetInRange_TargetTeam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMoveTargetTeamId_TargetTeam", _m_GetMoveTargetTeamId_TargetTeam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_MoveTo", _m_ChangeState_MoveTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_StopVoid", _m_ChangeState_StopVoid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_MoveStraight", _m_ChangeState_MoveStraight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_JumpTo", _m_ChangeState_JumpTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_FollowHero", _m_ChangeState_FollowHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_VirtualHeroMovePathTo", _m_ChangeState_VirtualHeroMovePathTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_VirtualHeroAssignTarget", _m_ChangeState_VirtualHeroAssignTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_WaitVirtualHeroAssignTarget", _m_ChangeState_WaitVirtualHeroAssignTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RequestVirtualHeroAssignTarget", _m_RequestVirtualHeroAssignTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetVirtualHeroAssignTarget", _m_GetVirtualHeroAssignTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_SoldierFollowVirtualHero", _m_ChangeState_SoldierFollowVirtualHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_SoldierSearchEnemy", _m_ChangeState_SoldierSearchEnemy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_MoveWithTurnAngle", _m_ChangeState_MoveWithTurnAngle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_HorseFollowVirtualHero", _m_ChangeState_HorseFollowVirtualHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_HeroMoveStraightTo", _m_ChangeState_HeroMoveStraightTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_HeroMovePathTo", _m_ChangeState_HeroMovePathTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_HeroSearchEnemy", _m_ChangeState_HeroSearchEnemy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_DashMove", _m_ChangeState_DashMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState_MoveToTargetById", _m_ChangeState_MoveToTargetById);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentStateEnum", _m_GetCurrentStateEnum);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentStateObject", _m_GetCurrentStateObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTargetInRange", _m_GetTargetInRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget", _m_SetTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTargetUnit", _m_SetTargetUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDashEnemyList", _m_GetDashEnemyList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDashDamageEnemyList", _m_GetDashDamageEnemyList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFortificationNode", _m_GetFortificationNode);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldPos", _g_get_WorldPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldLogicPos", _g_get_WorldLogicPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldPosLastFrame", _g_get_WorldPosLastFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsMoving", _g_get_IsMoving);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FaceDirNormalized", _g_get_FaceDirNormalized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrFaceRad", _g_get_CurrFaceRad);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PosRelativeToHero", _g_get_PosRelativeToHero);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Team", _g_get_Team);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaArray", _g_get_LuaArray);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TurnDirection", _g_get_TurnDirection);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MeleeAttackSourceStandNum", _g_get_MeleeAttackSourceStandNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LongRangeAttackSourceStandNum", _g_get_LongRangeAttackSourceStandNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_BSPTreeNode", _g_get__BSPTreeNode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CanMove", _g_get_CanMove);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WillAttackTarget", _g_get_WillAttackTarget);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WillAttackSourceSet", _g_get_WillAttackSourceSet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PathState", _g_get_PathState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MyBspTree", _g_get_MyBspTree);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnemyBspTree", _g_get_EnemyBspTree);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnitState", _g_get_UnitState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_isMelee", _g_get__isMelee);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_attackRange", _g_get__attackRange);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "WorldPos", _s_set_WorldPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WorldLogicPos", _s_set_WorldLogicPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WorldPosLastFrame", _s_set_WorldPosLastFrame);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsMoving", _s_set_IsMoving);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FaceDirNormalized", _s_set_FaceDirNormalized);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CurrFaceRad", _s_set_CurrFaceRad);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PosRelativeToHero", _s_set_PosRelativeToHero);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Team", _s_set_Team);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaArray", _s_set_LuaArray);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TurnDirection", _s_set_TurnDirection);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_BSPTreeNode", _s_set__BSPTreeNode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CanMove", _s_set_CanMove);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WillAttackTarget", _s_set_WillAttackTarget);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WillAttackSourceSet", _s_set_WillAttackSourceSet);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PathState", _s_set_PathState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MyBspTree", _s_set_MyBspTree);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnemyBspTree", _s_set_EnemyBspTree);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnitState", _s_set_UnitState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_isMelee", _s_set__isMelee);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_attackRange", _s_set__attackRange);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StateLayoutArray", M.PathFinding.Unit.StateLayoutArray);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new M.PathFinding.Unit();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.Unit constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsMelee(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsMelee(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitWithData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMap));
                    FixPoint.FixInt2 _worldPos;translator.Get(L, 3, out _worldPos);
                    int _id = LuaAPI.xlua_tointeger(L, 4);
                    int _teamId = LuaAPI.xlua_tointeger(L, 5);
                    int _side = LuaAPI.xlua_tointeger(L, 6);
                    bool _isHero = LuaAPI.lua_toboolean(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    bool _isInstrument = LuaAPI.lua_toboolean(L, 9);
                    bool _isMelee = LuaAPI.lua_toboolean(L, 10);
                    int _speed = LuaAPI.xlua_tointeger(L, 11);
                    int _attackRange = LuaAPI.xlua_tointeger(L, 12);
                    int _radius = LuaAPI.xlua_tointeger(L, 13);
                    int _extend = LuaAPI.xlua_tointeger(L, 14);
                    int _greenCircleRadius = LuaAPI.xlua_tointeger(L, 15);
                    int _meleeStandNum = LuaAPI.xlua_tointeger(L, 16);
                    int _rangeStandNum = LuaAPI.xlua_tointeger(L, 17);
                    LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 18, typeof(LuaArrAccess));
                    
                    gen_to_be_invoked.InitWithData( _map, _worldPos, _id, _teamId, _side, _isHero, _isVirtual, _isInstrument, _isMelee, _speed, _attackRange, _radius, _extend, _greenCircleRadius, _meleeStandNum, _rangeStandNum, _luaArray );
                    
                    
                    
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
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMap(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRealExtend(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetRealExtend(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWorldPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetWorldPos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWorldPosX(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetWorldPosX(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWorldPosY(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetWorldPosY(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetGridPos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetId(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetId(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsHero(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTeamId(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTeamId(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsDead(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsDead(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetDead(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _isDead = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetDead( _isDead );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSide(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSide(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAttackRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _r = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetAttackRange( _r );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAttackRangeRateOpen(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _isOpen = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetAttackRangeRateOpen( _isOpen );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsOpenDynamicAttackRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsOpenDynamicAttackRange(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackRangeRate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetAttackRangeRate(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetAttackRange(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPathFindingBlocked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPathFindingBlocked(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPathFindingBlocked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _blockNum = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetPathFindingBlocked( _blockNum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPosRelativeToHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _v;translator.Get(L, 2, out _v);
                    
                    gen_to_be_invoked.SetPosRelativeToHero( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosRelativeToHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPosRelativeToHero(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRadius(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetRadius(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSqrRadius(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSqrRadius(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGreenCircleRadius(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetGreenCircleRadius(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGreenCircleRadiusSqr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetGreenCircleRadiusSqr(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsVirtual(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsVirtual(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetVirtual(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetVirtual( _isVirtual );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnitType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetUnitType(  );
                        translator.PushMPathFindingEUnitType(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFaceDirToTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetFaceDirToTarget( _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFaceDirToTargetVec2(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _target;translator.Get(L, 2, out _target);
                    
                    gen_to_be_invoked.SetFaceDirToTargetVec2( _target );
                    
                    
                    
                    return 0;
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
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Tick(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _speed = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetSpeed( _speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClampPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _pos;translator.Get(L, 2, out _pos);
                    
                        var gen_ret = gen_to_be_invoked.ClampPos( _pos );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSpeed(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CanTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.CanTarget(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CanSelect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.CanSelect(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsMoveStraightReachTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsMoveStraightReachTarget(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetReachTargetSt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetReachTargetSt(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsJumpReachTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsJumpReachTarget(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetJumpTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetJumpTarget(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_SetHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _isHero = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.Internal_SetHero( _isHero );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StandAtGridCenter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StandAtGridCenter(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Log(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _format = LuaAPI.lua_tostring(L, 2);
                    object[] _args = translator.GetParams<object>(L, 3);
                    
                    gen_to_be_invoked.Log( _format, _args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackSourceDict(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetAttackSourceDict(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddLongAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Team _t = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
                    
                    gen_to_be_invoked.AddLongAttackSource( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveLongAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Team _t = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
                    
                    gen_to_be_invoked.RemoveLongAttackSource( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddMeleeAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.AddMeleeAttackSource(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveMeleeAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RemoveMeleeAttackSource(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMeleeAttackSourceCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMeleeAttackSourceCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetAttackRangeAddByHisSourceCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _target = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = gen_to_be_invoked.GetTargetAttackRangeAddByHisSourceCount( _target );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsAttackTargetInMyRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<M.PathFinding.Unit>(L, 2)) 
                {
                    M.PathFinding.Unit _target = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = gen_to_be_invoked.IsAttackTargetInMyRange( _target );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<M.PathFinding.Unit>(L, 2)) 
                {
                    M.PathFinding.Unit _target = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    long _outDis;
                    
                        var gen_ret = gen_to_be_invoked.IsAttackTargetInMyRange( _target, out _outDis );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushint64(L, _outDis);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.Unit.IsAttackTargetInMyRange!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsAttackNodeInMyRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Node _targetNode = (M.PathFinding.Node)translator.GetObject(L, 2, typeof(M.PathFinding.Node));
                    
                        var gen_ret = gen_to_be_invoked.IsAttackNodeInMyRange( _targetNode );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackDistanceToNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Node _n = (M.PathFinding.Node)translator.GetObject(L, 2, typeof(M.PathFinding.Node));
                    
                        var gen_ret = gen_to_be_invoked.GetAttackDistanceToNode( _n );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackDistanceToTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                        var gen_ret = gen_to_be_invoked.GetAttackDistanceToTarget( _u );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttackDistanceToTargetByIdx(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAttackDistanceToTargetByIdx( _targetUnitIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsMeInVirtualHeroGreenCircle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsMeInVirtualHeroGreenCircle(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsPositionInMyGreenCircle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _pos;translator.Get(L, 2, out _pos);
                    
                        var gen_ret = gen_to_be_invoked.IsPositionInMyGreenCircle( _pos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsPositionInHeroGreenCircle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _pos;translator.Get(L, 2, out _pos);
                    
                        var gen_ret = gen_to_be_invoked.IsPositionInHeroGreenCircle( _pos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateMovingTargetWithTurnAngleSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixFraction _turnAngleSpeedRadianPerSecend;translator.Get(L, 2, out _turnAngleSpeedRadianPerSecend);
                    FixPoint.FixInt2 _posTarget;translator.Get(L, 3, out _posTarget);
                    FixPoint.FixFraction _refMovingDirInRadian;translator.Get(L, 4, out _refMovingDirInRadian);
                    
                        var gen_ret = gen_to_be_invoked.CalculateMovingTargetWithTurnAngleSpeed( _turnAngleSpeedRadianPerSecend, _posTarget, ref _refMovingDirInRadian );
                        translator.Push(L, gen_ret);
                    translator.Push(L, _refMovingDirInRadian);
                        translator.Update(L, 4, _refMovingDirInRadian);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReviveUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _newId = LuaAPI.xlua_tointeger(L, 2);
                    int _x;
                    int _y;
                    
                    gen_to_be_invoked.ReviveUnit( _newId, out _x, out _y );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReviveVirtualHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x;
                    int _y;
                    
                    gen_to_be_invoked.ReviveVirtualHero( out _x, out _y );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFacePos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetFacePos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _newIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeTarget( _newIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddFrontPriorityTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _newIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AddFrontPriorityTarget( _newIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AppendPriorityTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _newIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AppendPriorityTarget( _newIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AppendAndCheckPriorityTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _newIdx = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AppendAndCheckPriorityTarget( _newIdx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DequeuePriorityTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.DequeuePriorityTarget(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PeekPriorityTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PeekPriorityTarget(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemovePriorityTargetById(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.RemovePriorityTargetById( _targetId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CleanPriorityTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CleanPriorityTarget(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTeamRadius(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTeamRadius(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSurroundPositions(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _centerX = LuaAPI.xlua_tointeger(L, 2);
                    int _centerY = LuaAPI.xlua_tointeger(L, 3);
                    int _radius = LuaAPI.xlua_tointeger(L, 4);
                    int _angleStart = LuaAPI.xlua_tointeger(L, 5);
                    int _angle = LuaAPI.xlua_tointeger(L, 6);
                    int _minNum = LuaAPI.xlua_tointeger(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.GetSurroundPositions( _centerX, _centerY, _radius, _angleStart, _angle, _minNum );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNearestUnitId(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetNearestUnitId( _x, _y );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetWillAttackTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.SetWillAttackTarget( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearWillAttackTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearWillAttackTarget(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMeleeWillAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMeleeWillAttackSource(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLongRangeWillAttackSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetLongRangeWillAttackSource(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRealExtendOnLayer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.EUnitType _t;translator.Get(L, 2, out _t);
                    
                        var gen_ret = gen_to_be_invoked.GetRealExtendOnLayer( _t );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsOccupiedGird(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsOccupiedGird(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOccupiedGrid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetOccupiedGrid(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_RemoveCurrentGridOccupy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Internal_RemoveCurrentGridOccupy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_ChangeOccupyGrid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.Internal_ChangeOccupyGrid( _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InternalEnableOccupy_Stand(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InternalEnableOccupy_Stand(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_RemoveCurrentGridOccupy_Temp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.EUnitType _unitType;translator.Get(L, 2, out _unitType);
                    
                    gen_to_be_invoked.Internal_RemoveCurrentGridOccupy_Temp( _unitType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Internal_AddCurrentGridOccupy_Temp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.EUnitType _unitType;translator.Get(L, 2, out _unitType);
                    
                    gen_to_be_invoked.Internal_AddCurrentGridOccupy_Temp( _unitType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsCurrStateCanMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.ELayerType _layerType;translator.Get(L, 2, out _layerType);
                    
                        var gen_ret = gen_to_be_invoked.IsCurrStateCanMove( _layerType );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendPathFindingRequest_OneUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _targetUnit = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    int _stopDis = LuaAPI.xlua_tointeger(L, 3);
                    int _maxNodeTest = LuaAPI.xlua_tointeger(L, 4);
                    M.PathFinding.ERequestPriority _priority;translator.Get(L, 5, out _priority);
                    
                    gen_to_be_invoked.SendPathFindingRequest_OneUnit( _targetUnit, _stopDis, _maxNodeTest, _priority );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendPathFindingRequest_Team(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _teamId = LuaAPI.xlua_tointeger(L, 2);
                    int _stopDis = LuaAPI.xlua_tointeger(L, 3);
                    int _maxNodeTest = LuaAPI.xlua_tointeger(L, 4);
                    M.PathFinding.ERequestPriority _priority;translator.Get(L, 5, out _priority);
                    
                    gen_to_be_invoked.SendPathFindingRequest_Team( _teamId, _stopDis, _maxNodeTest, _priority );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendPathFindingRequest_Position(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    int _stopDis = LuaAPI.xlua_tointeger(L, 3);
                    int _maxNodeTest = LuaAPI.xlua_tointeger(L, 4);
                    M.PathFinding.ERequestPriority _priority;translator.Get(L, 5, out _priority);
                    
                    gen_to_be_invoked.SendPathFindingRequest_Position( _targetPos, _stopDis, _maxNodeTest, _priority );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMinDistanceEnemy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMinDistanceEnemy(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMinDistanceAliveEnemy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMinDistanceAliveEnemy(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMinDistanceFriend(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMinDistanceFriend(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetTeamMinDistanceEnemy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Team _enemyTeam = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
                    
                        var gen_ret = gen_to_be_invoked.GetTargetTeamMinDistanceEnemy( _enemyTeam );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComputeMoveDisInOneFrame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ComputeMoveDisInOneFrame(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComputeMoveDisInOneFrameBySpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _speed = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.ComputeMoveDisInOneFrameBySpeed( _speed );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryToMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<FixPoint.FixInt2>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<FixPoint.FixFraction>(L, 4)&& translator.Assignable<System.Func<FixPoint.FixInt2, bool>>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    bool _isReachTarget = LuaAPI.lua_toboolean(L, 3);
                    FixPoint.FixFraction _disScale;translator.Get(L, 4, out _disScale);
                    System.Func<FixPoint.FixInt2, bool> _PosConstrainFunc = translator.GetDelegate<System.Func<FixPoint.FixInt2, bool>>(L, 5);
                    bool _needFace = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.TryToMove( _targetPos, ref _isReachTarget, _disScale, _PosConstrainFunc, _needFace );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _isReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 5&& translator.Assignable<FixPoint.FixInt2>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<FixPoint.FixFraction>(L, 4)&& translator.Assignable<System.Func<FixPoint.FixInt2, bool>>(L, 5)) 
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    bool _isReachTarget = LuaAPI.lua_toboolean(L, 3);
                    FixPoint.FixFraction _disScale;translator.Get(L, 4, out _disScale);
                    System.Func<FixPoint.FixInt2, bool> _PosConstrainFunc = translator.GetDelegate<System.Func<FixPoint.FixInt2, bool>>(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.TryToMove( _targetPos, ref _isReachTarget, _disScale, _PosConstrainFunc );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _isReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.Unit.TryToMove!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryToMoveBySpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<FixPoint.FixInt2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<FixPoint.FixFraction>(L, 5)&& translator.Assignable<System.Func<FixPoint.FixInt2, bool>>(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    int _speed = LuaAPI.xlua_tointeger(L, 3);
                    bool _isReachTarget = LuaAPI.lua_toboolean(L, 4);
                    FixPoint.FixFraction _disScale;translator.Get(L, 5, out _disScale);
                    System.Func<FixPoint.FixInt2, bool> _PosConstrainFunc = translator.GetDelegate<System.Func<FixPoint.FixInt2, bool>>(L, 6);
                    bool _needFace = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.TryToMoveBySpeed( _targetPos, _speed, ref _isReachTarget, _disScale, _PosConstrainFunc, _needFace );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _isReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<FixPoint.FixInt2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<FixPoint.FixFraction>(L, 5)&& translator.Assignable<System.Func<FixPoint.FixInt2, bool>>(L, 6)) 
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    int _speed = LuaAPI.xlua_tointeger(L, 3);
                    bool _isReachTarget = LuaAPI.lua_toboolean(L, 4);
                    FixPoint.FixFraction _disScale;translator.Get(L, 5, out _disScale);
                    System.Func<FixPoint.FixInt2, bool> _PosConstrainFunc = translator.GetDelegate<System.Func<FixPoint.FixInt2, bool>>(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.TryToMoveBySpeed( _targetPos, _speed, ref _isReachTarget, _disScale, _PosConstrainFunc );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _isReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.Unit.TryToMoveBySpeed!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryToMoveAndReach(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<FixPoint.FixInt2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<M.PathFinding.EReachTargetSt>(L, 4)&& translator.Assignable<FixPoint.FixFraction>(L, 5)&& translator.Assignable<System.Func<FixPoint.FixInt2, bool>>(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    int _speed = LuaAPI.xlua_tointeger(L, 3);
                    M.PathFinding.EReachTargetSt _eReachTarget;translator.Get(L, 4, out _eReachTarget);
                    FixPoint.FixFraction _disScale;translator.Get(L, 5, out _disScale);
                    System.Func<FixPoint.FixInt2, bool> _PosConstrainFunc = translator.GetDelegate<System.Func<FixPoint.FixInt2, bool>>(L, 6);
                    bool _needFace = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.TryToMoveAndReach( _targetPos, _speed, ref _eReachTarget, _disScale, _PosConstrainFunc, _needFace );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _eReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 6&& translator.Assignable<FixPoint.FixInt2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<M.PathFinding.EReachTargetSt>(L, 4)&& translator.Assignable<FixPoint.FixFraction>(L, 5)&& translator.Assignable<System.Func<FixPoint.FixInt2, bool>>(L, 6)) 
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    int _speed = LuaAPI.xlua_tointeger(L, 3);
                    M.PathFinding.EReachTargetSt _eReachTarget;translator.Get(L, 4, out _eReachTarget);
                    FixPoint.FixFraction _disScale;translator.Get(L, 5, out _disScale);
                    System.Func<FixPoint.FixInt2, bool> _PosConstrainFunc = translator.GetDelegate<System.Func<FixPoint.FixInt2, bool>>(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.TryToMoveAndReach( _targetPos, _speed, ref _eReachTarget, _disScale, _PosConstrainFunc );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _eReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.Unit.TryToMoveAndReach!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SysPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    
                    gen_to_be_invoked.SysPos( _targetPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryToMove_JumpTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _targetPos;translator.Get(L, 2, out _targetPos);
                    bool _isReachTarget = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.TryToMove_JumpTo( _targetPos, ref _isReachTarget );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _isReachTarget);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeState_Stop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_StopAttackTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_StopAttackTo( _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_Attack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_Attack( _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SearchEnemy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _teamIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_SearchEnemy( _teamIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetInRange_TargetTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTargetInRange_TargetTeam(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMoveTargetTeamId_TargetTeam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMoveTargetTeamId_TargetTeam(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _worldPosX = LuaAPI.xlua_tointeger(L, 2);
                    int _worldPosY = LuaAPI.xlua_tointeger(L, 3);
                    int _speed = LuaAPI.xlua_tointeger(L, 4);
                    bool _isMoveToPos = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.ChangeState_MoveTo( _worldPosX, _worldPosY, _speed, _isMoveToPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_StopVoid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeState_StopVoid(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveStraight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _worldPosX = LuaAPI.xlua_tointeger(L, 2);
                    int _worldPosY = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.ChangeState_MoveStraight( _worldPosX, _worldPosY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_JumpTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _worldPosX = LuaAPI.xlua_tointeger(L, 2);
                    int _worldPosY = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.ChangeState_JumpTo( _worldPosX, _worldPosY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_FollowHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeState_FollowHero(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_VirtualHeroMovePathTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_VirtualHeroMovePathTo( _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_VirtualHeroAssignTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _attackTargetVirtualHeroId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_VirtualHeroAssignTarget( _attackTargetVirtualHeroId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_WaitVirtualHeroAssignTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeState_WaitVirtualHeroAssignTarget(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RequestVirtualHeroAssignTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.RequestVirtualHeroAssignTarget( _unitId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVirtualHeroAssignTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetVirtualHeroAssignTarget( _unitId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SoldierFollowVirtualHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeState_SoldierFollowVirtualHero(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_SoldierSearchEnemy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_SoldierSearchEnemy( _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveWithTurnAngle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt2 _target;translator.Get(L, 2, out _target);
                    int _initDirInRadian = LuaAPI.xlua_tointeger(L, 3);
                    int _turnAngleSpeedRadianPerSecend = LuaAPI.xlua_tointeger(L, 4);
                    int _circleRadius = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.ChangeState_MoveWithTurnAngle( _target, _initDirInRadian, _turnAngleSpeedRadianPerSecend, _circleRadius );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HorseFollowVirtualHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _initDirInRadian = LuaAPI.xlua_tointeger(L, 2);
                    int _turnAngleSpeedRadianPerSecend = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.ChangeState_HorseFollowVirtualHero( _initDirInRadian, _turnAngleSpeedRadianPerSecend );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HeroMoveStraightTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _worldPosX = LuaAPI.xlua_tointeger(L, 2);
                    int _worldPosY = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.ChangeState_HeroMoveStraightTo( _worldPosX, _worldPosY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HeroMovePathTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_HeroMovePathTo( _targetUnitId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_HeroSearchEnemy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetUnitIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_HeroSearchEnemy( _targetUnitIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_DashMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FixPoint.FixInt _faceOffSet;translator.Get(L, 2, out _faceOffSet);
                    FixPoint.FixInt _dashRadius;translator.Get(L, 3, out _dashRadius);
                    int _damageRadius = LuaAPI.xlua_tointeger(L, 4);
                    int _dashJumpDamgeTime = LuaAPI.xlua_tointeger(L, 5);
                    int _dashHeroExtraW = LuaAPI.xlua_tointeger(L, 6);
                    int _dashHeroExtraH = LuaAPI.xlua_tointeger(L, 7);
                    FixPoint.FixInt _maxSlidDur;translator.Get(L, 8, out _maxSlidDur);
                    
                    gen_to_be_invoked.ChangeState_DashMove( _faceOffSet, _dashRadius, _damageRadius, _dashJumpDamgeTime, _dashHeroExtraW, _dashHeroExtraH, _maxSlidDur );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState_MoveToTargetById(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _targetId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState_MoveToTargetById( _targetId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentStateEnum(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetCurrentStateEnum(  );
                        translator.PushMPathFindingEUnitState(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentStateObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetCurrentStateObject(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetInRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTargetInRange(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetTarget( _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTargetUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.SetTargetUnit( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDashEnemyList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetDashEnemyList(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDashDamageEnemyList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetDashDamageEnemyList(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFortificationNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetFortificationNode(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WorldPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldLogicPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WorldLogicPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldPosLastFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WorldPosLastFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsMoving(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsMoving);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FaceDirNormalized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FaceDirNormalized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrFaceRad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurrFaceRad);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PosRelativeToHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PosRelativeToHero);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Team(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Team);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaArray);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TurnDirection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TurnDirection);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MeleeAttackSourceStandNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MeleeAttackSourceStandNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LongRangeAttackSourceStandNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LongRangeAttackSourceStandNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__BSPTreeNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._BSPTreeNode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CanMove(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CanMove);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WillAttackTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WillAttackTarget);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WillAttackSourceSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WillAttackSourceSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PathState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PathState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MyBspTree(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MyBspTree);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnemyBspTree(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EnemyBspTree);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnitState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                translator.PushMPathFindingEUnitState(L, gen_to_be_invoked.UnitState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__isMelee(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked._isMelee);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__attackRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._attackRange);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WorldPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.WorldPos = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WorldLogicPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.WorldLogicPos = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WorldPosLastFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.WorldPosLastFrame = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsMoving(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsMoving = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FaceDirNormalized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.FaceDirNormalized = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CurrFaceRad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CurrFaceRad = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PosRelativeToHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.PosRelativeToHero = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Team(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Team = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaArray = (LuaArrAccess)translator.GetObject(L, 2, typeof(LuaArrAccess));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TurnDirection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                M.PathFinding.ETurnDirection gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.TurnDirection = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__BSPTreeNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._BSPTreeNode = (M.PathFinding.BSPTreeNode)translator.GetObject(L, 2, typeof(M.PathFinding.BSPTreeNode));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CanMove(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CanMove = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WillAttackTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WillAttackTarget = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WillAttackSourceSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WillAttackSourceSet = (System.Collections.Generic.HashSet<M.PathFinding.Unit>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<M.PathFinding.Unit>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PathState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PathState = (M.PathFinding.PathState)translator.GetObject(L, 2, typeof(M.PathFinding.PathState));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MyBspTree(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MyBspTree = (M.PathFinding.BSPTree)translator.GetObject(L, 2, typeof(M.PathFinding.BSPTree));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnemyBspTree(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnemyBspTree = (M.PathFinding.BSPTree)translator.GetObject(L, 2, typeof(M.PathFinding.BSPTree));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnitState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                M.PathFinding.EUnitState gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.UnitState = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__isMelee(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._isMelee = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__attackRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Unit gen_to_be_invoked = (M.PathFinding.Unit)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._attackRange = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
