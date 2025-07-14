using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{

    public class StateSearchEnemy : State, IGetEnemyInRange, ISetMoveTargetTeam
    {
        private int _pathTestNodeMax = 20;
        private int _targetTeamId = -1;

        private readonly int _pathRecomputeInterval = 10;

        //射程内的敌人
        private Unit _targetInAttactRange = null;

        private bool _needFastPath = false;

        private int _waitingAttackFrame = 0;
        private int _waitingAttackFrameTotal = 0;

        public StateSearchEnemy(Unit unit)
        {
            _unit = unit;
        }
        //进入这个状态
        public void Enter(int targetTeamIndex)
        {
            _unit.Log("Enter State: StateSearchEnemy: {0}", targetTeamIndex);

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();

            _targetTeamId = targetTeamIndex;
            _SetTargetAttackUnit(null);
            _needFastPath = false;

            _waitingAttackFrame = 0;
            _waitingAttackFrameTotal = _unit.GetMap().GetBattleRandom().Next(0, 5);
        }

        private void _SetTargetAttackUnit(Unit u)
        {
            if (u != null)
                _unit.Log("SetTargetAttackUnit: {0}", u.GetId());
            else
                _unit.Log("SetTargetAttackUnit: null");
            // if(u!= null)
            //     _unit.StandAtGridCenter();
            _targetInAttactRange = u;
            _unit.SetWillAttackTarget(u);
        }

        public void SetMoveTargetTeam(int teamIndex)
        {
            _targetTeamId = teamIndex;
        }

        public Unit GetEnemyInRange()
        {
            if (_targetInAttactRange != null && _targetInAttactRange.IsDead())
            {
                return null;
            }
            return _targetInAttactRange;
        }

        // private float GetUnitDistance_Squared(Unit u1, Unit u2)
        // {
        //     return (u1.GetWorldPos() - u2.GetWorldPos()).LengthSquared();
        // }

        // private float GetUnitDistance_Rough(Unit u1, Unit u2)
        // {
        //     var grid1 = u1.GetGridPos();
        //     var grid2 = u2.GetGridPos();
        //     var dis = MathCache.GetGridDistance(grid1.x - grid2.x, grid1.y - grid2.y);
        //     return dis;
        // }

        public override void Tick()
        {
            Utils.Assert(_unit.IsOccupiedGird());

            //如果是远程小兵
            if (_targetInAttactRange == null && !_unit.IsMelee()) //&& !_unit.IsHero()
            {
                _waitingAttackFrame++;
                if (_waitingAttackFrame > _waitingAttackFrameTotal)
                {
                    if (_unit.IsHero())
                    {
                        var enemyTeam = _unit.GetMap().GetTeam(_targetTeamId);
                        var targetU = enemyTeam.Hero;
                        if (targetU.CanTarget())
                            _SetTargetAttackUnit(targetU);
                    }
                    else
                    {
                        var targetU = _unit.GetMap().QueryAttackTarget_HighPriority(_unit.GetTeamId(), _targetTeamId);
                        _SetTargetAttackUnit(targetU);
                    }

                }
                return;
            }

            //查询现在目标是否在射程内，如果在则不进行任何寻路
            if (_targetInAttactRange != null)
            {
                // var attackDis = _unit.GetAttackDistanceToTarget(_targetInAttactRange);
                // var attackRange = _unit.GetAttackRange();
                if (_unit.IsAttackTargetInMyRange(_targetInAttactRange) && !_targetInAttactRange.IsDead())
                {
                    return;
                }
                else
                {
                    _SetTargetAttackUnit(null);
                }
            }

            int stopDis = Math.Max(_unit.GetRadius(), 500);
            var waitingPathId = _unit.PathState.GetCurrentWaitingPathId();
            var path = _unit.PathState.GetCurrentPath();

            if (path.Count == 0 && waitingPathId < 0)
            {
                UpdateTargetInAttackRange();
                if (_targetInAttactRange == null)
                {
                    //如果没有路，并且没有在等待路径返回。说明是在发呆
                    if (_needFastPath)
                    {
                        _unit.SendPathFindingRequest_Team(_targetTeamId, stopDis, _pathTestNodeMax, ERequestPriority.Medium);
                        _needFastPath = false;
                    }
                    else
                    {
                        _unit.SendPathFindingRequest_Team(_targetTeamId, stopDis, _pathTestNodeMax, ERequestPriority.Low);
                    }
                }
            }
            else
            {
                if (path.Count > 0)
                {
                    //如果路径的结尾，它的值比现在的值还要查，那
                    FixFraction disScale = FixFraction.one;
                    int moveDisInOneFrame = _unit.ComputeMoveDisInOneFrame();
                    int movedDisAccum = 0;
                    while (true)
                    {
                        if (path.Count <= 0)
                            break;
                        var startPos = _unit.GetWorldPos();
                        var targetPos = path.Peek();
                        bool isReachTarget = false;
                        bool moveOk = _unit.TryToMove(targetPos, ref isReachTarget, disScale, null, true);
                        if (!moveOk)
                        {
                            _unit.PathState.ClearPath();
                            _needFastPath = true;
                            break;
                        }

                        //达到了这个路点，从路径中移除
                        if (isReachTarget)
                        {
                            int movedDis = (targetPos - startPos).magnitude;
                            movedDisAccum += movedDis;
                            disScale = FixFraction.one - new FixFraction(movedDisAccum, moveDisInOneFrame); // movedDisAccum / moveDisInOneFrame;
                            path.Dequeue();
                        }
                        else
                            break;
                    }

                    UpdateTargetInAttackRange();
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval && waitingPathId < 0)
                {
                    _unit.SendPathFindingRequest_Team(_targetTeamId, stopDis, _pathTestNodeMax, ERequestPriority.Medium);
                }
            }
        }

        private readonly List<Unit> _targetHeroList = new List<Unit>(20);
        private readonly List<Unit> _targetSoldierList = new List<Unit>(100);
        private void UpdateTargetInAttackRange()
        {
            //更新敌人信息，找到敌对部队中在射程内的单位
            var enemyTeam = _unit.GetMap().GetTeam(_targetTeamId);

            var minDis = Utils.MAX_DISTANCE;
            int attackRange = _unit.GetAttackRange();
            Unit resultTarget = null;

            _targetHeroList.Clear();
            _targetSoldierList.Clear();

            for (int i = 0; i < enemyTeam.AllUnitList.Count; i++)
            {
                var u = enemyTeam.AllUnitList[i];
                if (u.IsDead())
                    continue;
                if (u.IsHero())
                    _targetHeroList.Add(u);
                else
                    _targetSoldierList.Add(u);
            }

            List<Unit> targetList;
            if (_unit.IsHero())
                targetList = _targetHeroList.Count > 0 ? _targetHeroList : _targetSoldierList;
            else
                targetList = _targetSoldierList.Count > 0 ? _targetSoldierList : _targetHeroList;

            for (int i = 0; i < targetList.Count; i++)
            {
                var u = targetList[i];
                var d2 = _unit.GetAttackDistanceToTarget(u);

                int realAttackRange = attackRange;
                realAttackRange += _unit.GetTargetAttackRangeAddByHisSourceCount(u);
                // if (!_unit.IsHero() && !u.IsHero())
                // {
                //     realAttackRange += 
                //     var count = u.GetMeleeAttackSourceCount();
                //     if (count >= 5)
                //         realAttackRange += 1.0f;
                // }

                if (d2 > realAttackRange)
                    continue;
                if (d2 < minDis)
                {
                    resultTarget = u;
                    minDis = d2;
                    break;
                }
            }

            //如果是近战小兵
            if (resultTarget != null && _unit.IsMelee())
            {
                resultTarget.AddMeleeAttackSource();
            }

            _SetTargetAttackUnit(resultTarget);
        }

        public override void Exit()
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _targetTeamId = -1;
        }

        public override void Reset()
        {
            _pathTestNodeMax = 20;
            _targetTeamId = -1;
            
             _targetInAttactRange = null;

            _needFastPath = false;

            _waitingAttackFrame = 0;
            _waitingAttackFrameTotal = 0;
        }
    }
}