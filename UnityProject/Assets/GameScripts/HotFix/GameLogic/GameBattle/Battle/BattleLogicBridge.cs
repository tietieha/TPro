/* BattleLogicBridge.cs
 * caijunjie@topjoy.com
 * 2019/9/3 10:33:23
 * desc
 */

using System;
using System.Collections.Generic;
using FixPoint;
using M.PathFinding;
using RVO;
using TEngine;
using XLua;

namespace M.Battle
{
    [LuaCallCSharp]
    public class BattleLogicBridge
    {
        public static void BattleMap_Init(BattleMap battleMap,
            BattleEntityPoolManager mgr,
            int x,
            int y,
            int width,
            int height,
            int gridSize,
            int frameTime,
            int maxPathPosDis)
        {
            battleMap.Init(mgr, x, y, width, height, gridSize, frameTime, maxPathPosDis);
        }

        public static void InitUnitAndTeam(BattleMap battleMap, BattleMapConfig mapConfig)
        {
            battleMap.InitUnitAndTeam(mapConfig);
        }

        public static void BeforeBattle(BattleMap battleMap, int randSeed)
        {
            battleMap.BeforeBattle(randSeed);
        }

        public static void ReleaseMap(BattleMap battleMap)
        {
            battleMap.ReleaseMap();
        }

        public static void InitSingleTeam(BattleMap battleMap, int side, int teamIdx)
        {
            battleMap.InitSingleTeam(side, teamIdx);
        }

        public static void BattleMapTick(BattleMap battleMap)
        {
            battleMap.Tick();
        }

        public static void SetUnitSoldier(BattleMap battleMap, Unit unit)
        {
            battleMap.SetUnitSoldier(unit);
        }

        public static Unit GetUnit(BattleMap battleMap, int id)
        {
            return battleMap.GetUnit(id);
        }

        public static void SetUnitUpgradeHero(BattleMap battleMap, Unit unit)
        {
            battleMap.SetUnitHero(unit);
        }

        public static void BattleMapConfigAddUnit(BattleMapConfig mapConfig, int x, int y,
            int id, int teamId, int side, bool isHero, bool isVirtual, bool isInstrument, bool isMelee, double speed,
            double attackRange,
            double radius, int extend, int greenCircleRadius, int meleeStandNum, int rangeStandNum,
            LuaArrAccess luaArray)
        {
            UnitConfig unitcfg = new UnitConfig(new FixInt2(x, y), id, teamId, side, isHero, isVirtual, isInstrument,
                isMelee,
                (int)(speed * FixInt2.Scale),
                (int)(attackRange * FixInt2.Scale),
                (int)(radius * FixInt2.Scale),
                extend,
                (int)(greenCircleRadius * FixInt2.Scale),
                meleeStandNum,
                rangeStandNum,
                luaArray);
            mapConfig.AddUnit(unitcfg);
        }

        public static void ReviveUnit(Unit unit, int newId, out int posx, out int posy)
        {
            unit.ReviveUnit(newId, out posx, out posy);
        }


        public static void ReviveVirtualHero(Unit unit, out int posx, out int posy)
        {
            unit.ReviveVirtualHero(out posx, out posy);
        }

        public static void ClearBattleMapConfig(BattleMapConfig mapConfig)
        {
            mapConfig.ClearTeamList();
            mapConfig.ClearUnitList();
        }

        public static void AddNewTeamByBattleMapConfig(BattleMap battleMap, BattleMapConfig mapConfig)
        {
            battleMap.AddExtenTeam(mapConfig);
        }

        public static void SetUnitAttackRange(Unit unit, int range)
        {
            unit.SetAttackRange(range);
        }

        public static int IsUnitAttackTargetInRange(Unit unit, Unit target)
        {
            if (unit.IsAttackTargetInMyRange(target))
                return 1;
            return 0;
        }

        public static int GetUnitAttackRange(Unit unit)
        {
            return unit.GetAttackRange();
        }

        public static void SetUnitFaceDirToTarget(Unit unit, int x, int y)
        {
            unit.SetFaceDirToTarget(x, y);
        }

        public static int unitCurrentStateIs(Unit unit, EUnitState st)
        {
            if (unit.GetCurrentStateEnum() == st)
                return 1;
            return 0;
        }

        public static void SetUnitSpeed(Unit unit, double speed)
        {
            unit.SetSpeed((int)(speed * FixInt2.Scale));
        }

        public static double GetUnitSpeed(Unit unit)
        {
            return unit.GetSpeed() * FixInt2.InverseScale;
        }

        //设置单位的目标地点。仅对某些State有效
        public static void SetUnitTarget(Unit unit, int x, int y)
        {
            unit.SetTarget(x, y);
        }


        //设置单位的目标单位。仅对某些State有效
        public static void SetUnitTargetUnit(Unit unit, Unit target)
        {
            unit.SetTargetUnit(target);
        }

        public static void SetUnitDead(Unit unit, int isDie)
        {
            unit.SetDead(isDie == 1);
        }

        /// <summary>
        /// 给小兵设置移动目标
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="targetId"></param>
        public static void ChangeState_MoveToTargetById(Unit unit, int targetId)
        {
            unit.ChangeState_MoveToTargetById(targetId);
        }

        public static void ChangeState_MoveStraight(Unit unit, int x, int y)
        {
            unit.ChangeState_MoveStraight(x, y);
        }

        public static void ChangeState_MoveTo(Unit unit, int x, int y, int speed, int isMoveToPos)
        {
            unit.ChangeState_MoveTo(x, y, speed, isMoveToPos == 1);
        }

        public static void ChangeState_FollowHero(Unit unit)
        {
            unit.ChangeState_FollowHero();
        }

        public static void ChangeState_SearchEnemy(Unit unit, int teamIdx)
        {
            unit.ChangeState_SearchEnemy(teamIdx);
        }

        public static void ChangeState_Stop(Unit unit)
        {
            unit.ChangeState_Stop();
        }

        public static void ChangeState_Stop_Attack(Unit unit, int targetUnitIndex)
        {
            unit.ChangeState_StopAttackTo(targetUnitIndex);
        }

        public static void ChangeState_WaitVirtualHeroAssignTarget(Unit unit)
        {
            unit.ChangeState_WaitVirtualHeroAssignTarget();
        }

        public static void ChangeState_Attack(Unit unit, int targetUnitIndex)
        {
            unit.ChangeState_Attack(targetUnitIndex);
        }

        public static void ChangeState_JumpTo(Unit unit, int x, int y)
        {
            unit.ChangeState_JumpTo(x, y);
        }

        public static void GetJumpTarget(Unit unit, out int x, out int y)
        {
            var target = unit.GetJumpTarget();
            x = target.x;
            y = target.y;
        }

        public static int IsJumpReachTarget(Unit unit)
        {
            if (unit.IsJumpReachTarget())
                return 1;
            return 0;
        }

        public static void ChangeState_StopVoid(Unit unit)
        {
            unit.ChangeState_StopVoid();
        }

        public static int GetTeamRadius(Unit unit)
        {
            return unit.GetTeamRadius();
        }

        public static List<FixInt2> GetSurroundingPositions(Unit unit, int x, int y, int radius, int angleStart,
            int angle,
            int minNum)
        {
            return unit.GetSurroundPositions(x, y, radius, angleStart, angle, minNum);
        }

        // public static void ChangeState_ForceSlip(Unit unit, int sourceId, int x, int y, double force,
        //     double acceleration, double mass)
        // {
        //     unit.ChangeState_ForceSlip(sourceId, x, y, force, acceleration, mass);
        // }

        // public static int IsForceSlipBlockByOther(Unit unit)
        // {
        //     if (unit.IsForceSlipBlockByOther())
        //         return 1;
        //     return 0;
        // }

        // public static int IsForceSlipReachTarget(Unit unit)
        // {
        //     if (unit.IsForceSlipReachTarget())
        //         return 1;
        //     return 0;
        // }

        public static int IsMoveStraightReachTarget(Unit unit)
        {
            return unit.IsMoveStraightReachTarget() ? 1 : 0;
        }

        public static int GetReachTargetSt(Unit unit)
        {
            return unit.GetReachTargetSt();
        }

        public static int GetTargetInRange_TargetTeam(Unit unit)
        {
            var targetunit = unit.GetTargetInRange_TargetTeam();
            if (targetunit == null)
            {
                return -1;
            }

            return targetunit.GetId();
        }

        public static int GetTargetInRange(Unit unit)
        {
            var targetUnit = unit.GetTargetInRange();
            if (targetUnit == null)
            {
                return -1;
            }

            return targetUnit.GetId();
        }

        public static int GetWillAttackTargetId(Unit unit)
        {
            var target = unit.WillAttackTarget;
            if (target == null)
            {
                return -1;
            }

            return target.GetId();
        }

        public static int GetTargetInRangeNotVirtualHero(Unit unit)
        {
            var targetUnit = unit.GetTargetInRange();
            if (targetUnit == null)
            {
                return -1;
            }

            if (targetUnit.IsVirtual())
            {
                return -1;
            }

            return targetUnit.GetId();
        }

        // targetUnitIndex 为目标兵团的虚拟队长或者英雄
        public static void ChangeState_VirtualHeroMovePathTo(Unit unit, int targetUnitIndex)
        {
            unit.ChangeState_VirtualHeroMovePathTo(targetUnitIndex);
        }

        public static void ChangeState_VirtualHeroAssignTarget(Unit unit, int attackTargetVirtualHeroId)
        {
            unit.ChangeState_VirtualHeroAssignTarget(attackTargetVirtualHeroId);
        }

        public static void ChangeState_SoldierFollowVirtualHero(Unit unit)
        {
            unit.ChangeState_SoldierFollowVirtualHero();
        }

        // targetUnitIndex 为目标兵团的虚拟队长或者英雄
        public static void ChangeState_SoldierSearchEnemy(Unit unit, int targetUnitIndex)
        {
            unit.ChangeState_SoldierSearchEnemy(targetUnitIndex);
        }

        public static List<int> Team_GetAttackSource(BattleMap battleMap, int teamIndex)
        {
            Team t = battleMap.GetTeam(teamIndex);
            return battleMap.Team_GetAttackSource(t);
        }

        public static int Team_GetAttackSourceCount(BattleMap battleMap, int teamIndex)
        {
            Team t = battleMap.GetTeam(teamIndex);
            return battleMap.Team_GetAttackSourceCount(t);
        }

        //带转向速度限制的移动（骑兵队长使用）
        public static void ChangeState_MoveWithTurnAngle(Unit unit, int targetX, int targetY, int initDirInRadian,
            int turnAngleSpeedRadianPerSecend, int circleRadius)
        {
            unit.ChangeState_MoveWithTurnAngle(new FixInt2(targetX, targetY), initDirInRadian,
                turnAngleSpeedRadianPerSecend, circleRadius);
        }

        //带转向速度的小兵跟随武将移动（骑兵使用）
        public static void ChangeState_HorseFollowVirtualHero(Unit unit, int initDirInRadian,
            int turnAngleSpeedRadianPerSecend)
        {
            unit.ChangeState_HorseFollowVirtualHero(initDirInRadian, turnAngleSpeedRadianPerSecend);
        }

        //英雄寻路移动到目标
        public static void ChangeState_HeroMovePathTo(Unit unit, int targetUnitId)
        {
            unit.ChangeState_HeroMovePathTo(targetUnitId);
        }

        public static void ChangeState_HeroMoveStraightTo(Unit unit, int x, int y)
        {
            unit.ChangeState_HeroMoveStraightTo(x, y);
        }

        //英雄开始自由寻敌
        public static void ChangeState_HeroSearchEnemy(Unit unit, int targetUnitIndex)
        {
            unit.ChangeState_HeroSearchEnemy(targetUnitIndex);
        }

        //小兵开始冲锋
        public static void ChangeState_SoldierDashMove(Unit unit, int faceOffSet, int dashRadius,
            int damageRadius, int dashJumpDamgeTime,
            int dashHeroExtraW, int dashHeroExtraH,
            int maxSlidDur)
        {
            unit.ChangeState_DashMove(faceOffSet, dashRadius, damageRadius, dashJumpDamgeTime,
                dashHeroExtraW, dashHeroExtraH, maxSlidDur);
        }

        //切换为被裹挟
        // public static void ChangeState_StateCoerced(Unit unit, int ownIdx, int offsetDist, int coercedRadius, int speed)
        // {
        //     unit.ChangeState_StateCoerced(ownIdx, offsetDist, coercedRadius, speed);
        // }

        public static void CleanTargetByTeamIndex(BattleMap battleMap, int teamIndex)
        {
            Team t = battleMap.GetTeam(teamIndex);
            foreach (var u in t.AllUnitList)
            {
                u.ClearWillAttackTarget();
            }
        }

        public static int GetAttackDistanceToTarget(Unit unit, Unit tarUnit)
        {
            return unit.GetAttackDistanceToTarget(tarUnit);
        }

        #region 单位攻击来源

        //获取所有正在打我的单位ID
        public List<int> Unit_GetWillAttackSource_All(BattleMap map, Unit unit)
        {
            map.OutUnitIdList.Clear();
            foreach (var u in unit.WillAttackSourceSet)
            {
                map.OutUnitIdList.Add(u.GetId());
            }

            return map.OutUnitIdList;
        }

        //正在打我的近战
        public static List<int> Unit_GetWillAttackSource_Melee(BattleMap map, Unit unit)
        {
            map.OutUnitIdList.Clear();
            foreach (var u in unit.WillAttackSourceSet)
            {
                if (u.IsMelee())
                    map.OutUnitIdList.Add(u.GetId());
            }

            return map.OutUnitIdList;
        }

        public static int Unit_GetWillAttackSource_Melee_AnyOne(Unit unit)
        {
            foreach (var u in unit.WillAttackSourceSet)
            {
                if (u.IsMelee() && u.CanTarget())
                    return u.GetId();
            }

            return -1;
        }

        //正在打我的远程
        public static List<int> Unit_GetWillAttackSource_LongRange(BattleMap map, Unit unit)
        {
            map.OutUnitIdList.Clear();
            foreach (var u in unit.WillAttackSourceSet)
            {
                if (!u.IsMelee())
                    map.OutUnitIdList.Add(u.GetId());
            }

            return map.OutUnitIdList;
        }

        #endregion

        #region 军团攻击来源

        //正在打本军团的单位
        public static List<int> Team_GetWillAttackSource_All(BattleMap map, int teamId)
        {
            map.OutUnitIdList.Clear();
            Team team = map.GetTeam(teamId);
            if (team == null)
                return map.OutUnitIdList;

            foreach (var u in team.WillAttackSourceUnitSet)
            {
                map.OutUnitIdList.Add(u.GetId());
            }

            return map.OutUnitIdList;
        }

        //正在打本军团的近战单位
        public static List<int> Team_GetWillAttackSource_Melee(BattleMap map, int teamId)
        {
            map.OutUnitIdList.Clear();
            Team team = map.GetTeam(teamId);
            if (team == null)
                return map.OutUnitIdList;

            foreach (var u in team.WillAttackSourceUnitSet)
            {
                if (u.IsMelee())
                    map.OutUnitIdList.Add(u.GetId());
            }

            return map.OutUnitIdList;
        }

        //正在打本军团的近战单位,返回第一个
        public static int Team_GetWillAttackSource_MeleeOne(BattleMap map, int teamId)
        {
            Team team = map.GetTeam(teamId);
            if (team == null)
                return -1;

            foreach (var u in team.WillAttackSourceUnitSet)
            {
                if (u.IsMelee())
                    return u.GetId();
            }

            return -1;
        }

        public static int Team_GetWillAttackSource_Melee_Count(BattleMap map, int teamId)
        {
            return BattleLogicBridge.Team_GetWillAttackSource_Melee(map, teamId).Count;
        }

        public static List<int> Team_GetWillAttackSource_Melee_InRange(BattleMap map, int teamId)
        {
            map.OutUnitIdList.Clear();
            Team team = map.GetTeam(teamId);
            if (team == null)
                return map.OutUnitIdList;

            foreach (var u in team.WillAttackSourceTeamSet)
            {
                var atkTeam = u.Key;
                if (atkTeam.Hero.IsMelee() && atkTeam.Hero.CanTarget() &&
                    team.Hero.IsAttackTargetInMyRange(atkTeam.Hero))
                    map.OutUnitIdList.Add(atkTeam.Id);
            }

            return map.OutUnitIdList;
        }

        public static int Unit_Is_Target_InRange(BattleMap map, int uId, int targetVirtualIndex)
        {
            Unit u = map.GetUnit(uId);
            if (u == null)
                return 0;

            Unit targetU = map.GetUnit(targetVirtualIndex);
            if (targetU == null)
                return 0;

            if (u.IsAttackTargetInMyRange(targetU))
                return 1;

            return 0;
        }

        //正在打本军团的远程单位
        public static List<int> Team_GetWillAttackSource_LongRange(BattleMap map, int teamId)
        {
            map.OutUnitIdList.Clear();
            Team team = map.GetTeam(teamId);
            if (team == null)
                return map.OutUnitIdList;

            foreach (var u in team.WillAttackSourceUnitSet)
            {
                if (!u.IsMelee())
                    map.OutUnitIdList.Add(u.GetId());
            }

            return map.OutUnitIdList;
        }

        public static int Team_GetWillAttackSource_LongRange_Count(BattleMap map, int teamId)
        {
            return BattleLogicBridge.Team_GetWillAttackSource_LongRange(map, teamId).Count;
        }

        #endregion


        public static List<int> Unit_GetDashEnemyList(BattleMap map, int unitId)
        {
            return map.GetUnit(unitId).GetDashEnemyList();
        }

        public static List<int> Unit_GetDashDamageEnemyList(BattleMap map, int unitId)
        {
            return map.GetUnit(unitId).GetDashDamageEnemyList();
        }

        public static void Unit_GetFacePos(Unit u, ref float x, ref float y)
        {
            var tpos = u.GetFacePos();
            x = tpos.x;
            y = tpos.y;
        }

        public static int Get_Unit_newHeroTarget(Unit u, int warningDis)
        {
            // 策划提出警戒范围以圆心判断
            int idx = -1;
            long sqrMagnit = warningDis * warningDis;
            foreach (var s in u.WillAttackSourceSet)
            {
                if (s.IsVirtual() || s.IsDead() || !s.IsHero() || !s.CanTarget())
                {
                    continue;
                }

                if ((u.WorldLogicPos - s.WorldLogicPos).sqrMagnitude < sqrMagnit)
                {
                    idx = s.GetId();
                    break;
                }
            }

            return idx;
        }

        public static int Get_Unit_isHeroWarningDis(Unit u, int target, int warningDis)
        {
            int bRet = 0;
            long sqrMagnit = warningDis * warningDis;
            var t = u.GetMap().GetUnit(target);
            if (t.IsHero() && (u.WorldPos - t.WorldPos).sqrMagnitude < sqrMagnit)
                bRet = 1;

            return bRet;
        }

        public static int Team_GetNearestTeam(Unit u)
        {
            return u.GetMap().GetNearestTeam(u);
        }

        public static void Unit_ChangeTarget(Unit u, int target)
        {
            u.ChangeTarget(target);
        }

        public static int Unit_PeekPriorityTarget(Unit u)
        {
            var tar = u.PeekPriorityTarget();
            if (tar != null)
            {
                return tar.GetId();
            }

            return -1;
        }

        public static void Unit_RemovePriorityTargetById(Unit u, int targetId)
        {
            u.RemovePriorityTargetById(targetId);
        }

        public static void Unit_AddFrontPriorityTarget(Unit u, int target)
        {
            u.AddFrontPriorityTarget(target);
        }

        public static void Unit_CleanPriorityTarget(Unit u)
        {
            u.CleanPriorityTarget();
        }

        public static void Unit_AddSkillWillAttack(Unit u, int tarIdx)
        {
            if (!u.GetMap().GetUnitDict().ContainsKey(tarIdx))
                return;

            var t = u.GetMap().GetUnit(tarIdx);
            if (t.IsDead())
                return;

            u.AppendAndCheckPriorityTarget(tarIdx);
        }

        public static int Unit_PeekPriorityTarget_WillAttackSource_Melee_AnyOne(Unit u)
        {
            int tid = Unit_PeekPriorityTarget(u);
            if (tid == -1)
            {
                return Unit_GetWillAttackSource_Melee_AnyOne(u);
            }

            return tid;
        }

        public static int Unit_PeekPriorityTarget_WillAttackSource_InRange_AnyOne(Unit u)
        {
            //先查优先攻击目标
            int tid = Unit_PeekPriorityTarget(u);

            if (tid == -1)
            {
                //再查范围内目标
                var t = u.GetTargetInRange();
                if (t != null)
                {
                    tid = t.GetId();
                }
            }

            if (tid == -1)
            {
                //再查攻击我的近战目标，最好是攻击我的远程目标
                var temp = -1;
                tid = Unit_GetWillAttackSource_Melee_AnyOne(u);
                foreach (var t in u.WillAttackSourceSet)
                {
                    if (t.IsMelee())
                    {
                        if (t.CanTarget())
                        {
                            tid = t.GetId();
                            break;
                        }
                    }
                    else
                    {
                        if (t.CanTarget() && temp == -1)
                        {
                            temp = t.GetId();
                        }
                    }
                }

                if (tid == -1 && temp != -1)
                {
                    tid = temp;
                }
            }

            return tid;
        }

        // public static void ChangeState_StateAttackFortification(Unit u, int x, int y)
        // {
        //     u.ChangeState_StateAttackFortification(x, y);
        // }

        public static int IsUnitAttackNodeInRange(Unit unit, int x, int y)
        {
            Node n = unit.GetMap().GetNode(x, y);
            if (n == null)
                return 0;

            if (unit.IsAttackNodeInMyRange(n))
                return 1;
            else
                return 0;
        }

        // public static void ChangeState_StateHeroOrVirtualGuard(Unit u)
        // {
        //     u.ChangeState_StateHeroOrVirtualGuard();
        // }
        //
        // public static void ChangeState_StateHeroOrVirtualVigilance(Unit u, int toPosX, int toPosY)
        // {
        //     u.ChangeState_StateHeroOrVirtualVigilance(toPosX, toPosY);
        // }
        //
        // public static void ChangeState_StateGuardWallAttack(Unit u)
        // {
        //     u.ChangeState_StateGuardWallAttack();
        // }

        public static void GetGridNodePos(BattleMap map, int x, int y, out int ox, out int oy)
        {
            var n = map.GetNode(x, y);
            if (n == null)
            {
                ox = -1;
                oy = -1;
            }
            else
            {
                var pos = n.GetCenterWorldPos();
                ox = pos.x;
                oy = pos.y;
            }
        }

        // 指定目标
        // public static void ChangeState_UnitAppointSearch(Unit u, int uId)
        // {
        //     u.ChangeState_UnitAppointSearch(uId);
        // }

        public static void UnitCanMove(Unit u, int bCan)
        {
            u.CanMove = bCan == 1;
        }

        // public static void ChangeState_UnitCharmSearch(Unit u)
        // {
        //     u.ChangeState_UnitCharmSearch();
        // }

        public static int GetMoveTargetTeamId_TargetTeam(Unit unit)
        {
            return unit.GetMoveTargetTeamId_TargetTeam();
        }

        public static List<int> GetUnitsInRadius(BattleMap map, Unit unit, int radius)
        {
            return map.GetUnitIdsInRange(unit, radius);
        }

        public static List<int> GetUnitsInCircle(BattleMap map, int x, int y, int radius, int side)
        {
            return map.GetUnitsInCircle(x, y, radius, side);
        }

        /// <summary>
        /// 计算U2在U的那个角度方位
        /// </summary>
        /// <param name="u"></param>
        /// <param name="u2"></param>
        /// <returns></returns>
        public static int GetTwoUnitAngle(Unit u, Unit u2)
        {
            double value = Math.Atan2(u2.WorldPos.y - u.WorldPos.y,
                u2.WorldPos.x - u.WorldPos.x);
            return (int)Math.Round(value * 180 / Math.PI);
        }

        public static FixInt2 GetWorldPos(Unit u)
        {
            return u.WorldPos;
        }

        public static int GetNearestUnitId(Unit u, int x, int y)
        {
            return u.GetNearestUnitId(x, y);
        }

        /**************************** ROV2 ********************************/

        public static void BattleMap_InitRvo(Simulator simulator,
            int x,
            int y,
            int width,
            int height,
            float timeStep,
            float neighborDist,
            int maxNeighbors,
            float timeHorizon,
            float timeHorizonObst)
        {
            // Log.Info($"Map x {x} y {y} width {width} height {height} timeStep {timeStep} neighborDist {neighborDist} maxNeighbors {maxNeighbors} timeHorizon {timeHorizon} timeHorizonObst {timeHorizonObst}");
            // _CreateWall(simulator, x, y, width, height);
            // simulator.SetAgentDefaults(1, 8, 1f, 1f, 1f, 0.2f, new Vector2(0, 0));
            // simulator.SetAgentDefaults(15f, 10, 0.5f, 1f, 1f, 0.2f, new Vector2(0, 0));
            simulator.SetAgentDefaults(neighborDist, maxNeighbors, timeHorizon,
                timeHorizonObst, 1f, 0.2f, new Vector2(0, 0));
            simulator.SetTimeStep(timeStep);
            // simulator.SetTimeStep(0.2f);
        }

        //创建边界墙
        private static void _CreateWall(Simulator simulator, int x, int y, int width, int height)
        {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            float thickness = 1f;

            // Create the left wall
            simulator.AddObstacle(new List<Vector2>
            {
                new Vector2(x - halfWidth - thickness, y - halfHeight),
                new Vector2(x - halfWidth, y - halfHeight),
                new Vector2(x - halfWidth, y + halfHeight),
                new Vector2(x - halfWidth - thickness, y + halfHeight)
            });

            // Create the right wall
            simulator.AddObstacle(new List<Vector2>
            {
                new Vector2(x + halfWidth, y - halfHeight),
                new Vector2(x + halfWidth + thickness, y - halfHeight),
                new Vector2(x + halfWidth + thickness, y + halfHeight),
                new Vector2(x + halfWidth, y + halfHeight)
            });

            // Create the bottom wall
            simulator.AddObstacle(new List<Vector2>
            {
                new Vector2(x - halfWidth - thickness, y - halfHeight - thickness),
                new Vector2(x + halfWidth + thickness, y - halfHeight - thickness),
                new Vector2(x + halfWidth + thickness, y - halfHeight),
                new Vector2(x - halfWidth - thickness, y - halfHeight)
            });

            // Create the top wall
            simulator.AddObstacle(new List<Vector2>
            {
                new Vector2(x - halfWidth - thickness, y + halfHeight),
                new Vector2(x + halfWidth + thickness, y + halfHeight),
                new Vector2(x + halfWidth + thickness, y + halfHeight + thickness),
                new Vector2(x - halfWidth - thickness, y + halfHeight + thickness)
            });

            // Process the obstacles so they are added to the simulation
            simulator.ProcessObstacles();
        }

        public static void BattleAddUnitRvo(Simulator simulator, float x, float y,
            int id, int teamId, int side, bool isVirtual, bool isMelee, float speed,
            float attackRange,
            float radius, float stopDis, float avoidanceRatio, int groupId,
            LuaArrAccess luaArray, int agentType)
        {
            float posX = x;
            float posY = y;
            // Log.Info(
            //     $"添加Soldier id {id}, teamId {teamId}, x {x}, y {y}, speed {speed}, radius {radius}, isVirtual {isVirtual}, isMelee {isMelee}, stopDis {stopDis}, attackRange {attackRange}");
            simulator.AddSoliderAgent(id, posX, posY, teamId, radius, side, speed,
                attackRange, isVirtual, isMelee, stopDis, avoidanceRatio, groupId, luaArray,
                (AgentType)agentType);
        }

        public static void BattleAddVirtualHeroRvo(Simulator simulator, float x, float y,
            int id, int teamId, int side, bool isVirtual, bool isMelee, float speed,
            float attackRange,
            float radius, float stopDis, float avoidanceRatio,
            LuaArrAccess luaArray, int agentType, float neighborDist, int maxNeighbors,
            float timeHorizon)
        {
            simulator.AddVirtualHeroAgent(id, x, y, teamId, radius, side, speed,
                attackRange, isVirtual, isMelee, stopDis, avoidanceRatio, luaArray,
                (AgentType)agentType, neighborDist, maxNeighbors, timeHorizon);
        }

        public static void BattleMapRvoTick(Simulator simulator)
        {
            simulator.DoStep();
        }

        public static void BattleRvoSetTargetId(Simulator simulator, int id, int targetId, bool isFollow = false,
            bool isFirst = false)
        {
            // Log.Info($"SetTargetId: {id} -> {targetId}");
            simulator.SetTargetId(id, targetId, isFollow, isFirst);
        }

        // 停止移动
        public static void BattleRvoStopMove(Simulator simulator, int id)
        {
            simulator.SetTargetId(id, 0);
        }

        public static List<int> GetUnitsInRadiusRvo(Simulator simulator, int agentId, float radius)
        {
            return simulator.GetAgentIdsInRange(agentId, radius);
        }

        public static List<int> GetUnitsInCircleRvo(Simulator simulator, int x, int y, float radius, int side,
            bool isVirtual = false, bool containDead = false)
        {
            return simulator.GetAgentsInCircle(x / FixInt2.Scale, y / FixInt2.Scale, radius / FixInt2.Scale, side,
                isVirtual, containDead);
        }

        public static void SetUnitDeadRvo(Simulator simulator, int id)
        {
            // Log.Info($"On Die id:{id}");
            simulator.DelAgent(id);
        }

        public static void SetAgentAttackRange(Simulator simulator, int id, int range)
        {
            // Log.Info($"SetAgentAttackRange: {id} -> {range}");
            simulator.SetAttackRange(id, range);
        }

        public static void SetAgentStopDis(Simulator simulator, int id, float stopDis)
        {
            simulator.SetStopDis(id, stopDis);
        }

        public static void BattleRvoSetTargetSpeed(Simulator simulator, int id, float speed)
        {
            simulator.SetAgentSpeed(id, speed);
        }

        public static void BattleRvoSetTargetAvoidRatio(Simulator simulator, int id, float ratio)
        {
            simulator.SetAgentAvoidRatio(id, ratio);
        }

        public static void BattleRvoSetTargetPosition(Simulator simulator, int id, float x, float y)
        {
            simulator.SetAgentTargetPosition(id, x, y);
        }

        public static void BattleRvoSetAgentRatio(Simulator simulator, int id, float ratio)
        {
            var agent = simulator.GetAgent(id);
            if (agent != null)
            {
                agent.AvoidanceRatio = ratio;
            }
        }

        public static float BattleRvoGetAgentsDistance(Simulator simulator, int agentIdA, int agentIdB)
        {
            var agent = simulator.GetAgent(agentIdA);
            var agentB = simulator.GetAgent(agentIdB);
            if (agent != null && agentB != null)
            {
                var dis = agent.Position.Distance(agentB.Position);
                return dis - agentB.StopDis;
            }

            return 0;
        }

        public static bool BattleRvoIsAgentValid(Simulator simulator, int agentId)
        {
            return simulator.IsAgentValid(agentId);
        }

        public static void BattleRvoBuildAgentTree(Simulator simulator)
        {
            simulator.BuildAgentTree();
        }

        public static void BattleRvoAgentRevive(Simulator simulator, int agentId)
        {
            simulator.AgentRevive(agentId);
        }

        public static void BattleRvoDeadKdTreeUpdate(Simulator simulator)
        {
            simulator.BuildDeadAgentTree();
        }

        public static void BattleRvoAfterDeadAgentRevive(Simulator simulator)
        {
            simulator.OnDeadAgentRevive();
        }

        public static void BattleRvoSetAgentPosition(Simulator simulator, int agentId, float x, float y)
        {
            simulator.SetAgentPosition(agentId, x / FixInt2.Scale, y / FixInt2.Scale);
        }

        public static void BattleRvoSetAgentAIType(Simulator simulator, int agentId, int aiType)
        {
            simulator.SetAIType(agentId, aiType);
        }

        public static int BattleRvoAgentEncircled(Simulator simulator, int agentId, float attackRange)
        {
            return simulator.IsAgentEncircle(agentId, attackRange);
        }

        public static int BattleRvoNeedChangeNearestAgent(Simulator simulator, int agentId, int targetId)
        {
            return simulator.NeedChangeNearestAgent(agentId, targetId);
        }

        public static List<int> BattleRvoGetAgentsInRotatedSector(Simulator simulator, float x, float y, float radius,
            float angle, float angleRad,
            int side = 0,
            bool isVirtual = false,
            bool containDead = false)
        {
            return simulator.GetAgentsInRotatedSector(x / FixInt2.Scale, y / FixInt2.Scale, radius / FixInt2.Scale,
                angle, angleRad, side, isVirtual, containDead);
        }

        public static List<int> BattleRvoGetAgentsInRotatedRect(Simulator simulator, float x, float y, float width,
            float height, float angleRad, int side = 0, bool isVirtual = false, bool containDead = false)
        {
            return simulator.GetAgentsInRotatedRect(x / FixInt2.Scale, y / FixInt2.Scale, width / FixInt2.Scale,
                height / FixInt2.Scale, angleRad, side,
                isVirtual, containDead);
        }

        public static float GetAngleFromPoints(float x, float y, float x1, float y1)
        {
            float angleRad = (float)Math.Atan2(y1 / FixInt2.Scale - y / FixInt2.Scale,
                x1 / FixInt2.Scale - x / FixInt2.Scale);
            if (angleRad == -2f * Math.PI)
            {
                angleRad = 0;
            }

            return angleRad;
        }
    }
}