//英雄单独寻路
using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{

    public class StateHeroSearchEnemy : State, IGetEnemyInRange
    {
        private int _pathTestNodeMax = 50;
        //射程内的敌人
        private Unit _targetInAttactRange = null;
        Team _moveEnemyTeam = null;
        
        private ERequestPriority _pathPriority = ERequestPriority.Medium;

        private readonly int _pathRecomputeInterval = 10;

        public StateHeroSearchEnemy(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(int targetUnitIndex)
        {
            _unit.Log("Enter State: StateHeroSearchEnemy");
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();
            var unitDict = _unit.GetMap().GetUnitDict();
            var moveToUnit = unitDict[targetUnitIndex];
            _moveEnemyTeam = _unit.GetMap().GetTeam(moveToUnit.GetTeamId());
            var prio = _unit.PeekPriorityTarget();
            if (prio != _targetInAttactRange)
            {
                _targetInAttactRange = null;
            }

        }
        
        private void _SetTargetAttackUnit(Unit u)
        {
            if (u != null)
            {
                _unit.Log("SetTargetAttackUnit: {0}", u.GetId());
                //_unit.StandAtGridCenter();
            }
            else
                _unit.Log("SetTargetAttackUnit: null");
            _targetInAttactRange = u;
            _unit.SetWillAttackTarget(u);
        }

        public Unit GetEnemyInRange()
        {
            if (_targetInAttactRange != null && (_targetInAttactRange.IsDead() || !_targetInAttactRange.CanTarget()))
                return null;
            return _targetInAttactRange;
        }

        public override void Tick()
        {
            //试图筛选出最近的目标
            //移动到目标去打
            //只要有单位在射程内，则立刻开始打

            Utils.Assert(_unit.IsOccupiedGird());

            //查询现在目标是否在射程内，如果在则不进行任何寻路
            if (_targetInAttactRange != null)
            {
                if (_targetInAttactRange.CanTarget() && _unit.IsAttackTargetInMyRange(_targetInAttactRange) && !_targetInAttactRange.IsDead())
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

            if (path.Count == 0)
            {
                if (waitingPathId < 0) //如果没有在等待路径计算
                {
                    _UpdateTargetInAttackRange();
                    if (_targetInAttactRange == null)
                    {
                        //如果没有路，并且没有在等待路径返回。说明是在发呆
                        Unit targetUnit = null;
                        if (_unit.IsMelee())
                            targetUnit = _unit.GetMinDistanceEnemy();
                        else
                            targetUnit = _unit.GetTargetTeamMinDistanceEnemy(_moveEnemyTeam);

                        if (targetUnit != null)
                        {
                            _unit.SendPathFindingRequest_OneUnit(targetUnit, stopDis, _pathTestNodeMax, _pathPriority);
                            _pathPriority = ERequestPriority.Low;
                        }
                    }
                }
                else //如果正在等待路径计算
                {

                }

            }
            else
            {
                if (path.Count > 0)
                {
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
                            _pathPriority = ERequestPriority.High;
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

                    _UpdateTargetInAttackRange();
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval && waitingPathId < 0)
                {
                    var targetUnit = GetPriorityTargetorMinDistanceEnemy();
                    if (targetUnit != null)
                        _unit.SendPathFindingRequest_OneUnit(targetUnit, stopDis, _pathTestNodeMax, ERequestPriority.Medium);
                }
            }
        }

        private void _UpdateTargetInAttackRange()
        {
            var minDis = Utils.MAX_DISTANCE;
            int attackRange = _unit.GetAttackRange();

            Unit resultTarget = null;
            var mySide = _unit.GetSide();

            //如果有优先目标，则只找到他
            var prioTarget = _unit.PeekPriorityTarget();
            if (prioTarget != null)
            {
                if (prioTarget.IsVirtual())
                {
                    foreach (var enemyUnit in prioTarget.Team.AllUnitList)
                    {
                        if (enemyUnit.IsVirtual() || enemyUnit.IsDead() || !enemyUnit.CanTarget())
                        {
                            continue;
                        }
                        if (_unit.IsAttackTargetInMyRange(enemyUnit))
                        {
                            resultTarget = enemyUnit;
                            _SetTargetAttackUnit(resultTarget);
                            return;
                        }
                    }
                }
                else
                {
                    if (_unit.IsAttackTargetInMyRange(prioTarget))
                    {
                        resultTarget = prioTarget;
                        _SetTargetAttackUnit(resultTarget);
                        return;
                    }
                }
            }


            //优先查找在打我的英雄
            foreach (var u in _unit.WillAttackSourceSet)
            {
                if (u.IsVirtual() || u.IsDead() || !u.IsHero() || !u.IsMelee() || !u.CanTarget())
                {
                    continue;
                }
                if (_unit.IsAttackTargetInMyRange(u))
                {
                    resultTarget = u;
                    _SetTargetAttackUnit(resultTarget);
                    return;
                }
            }

            // 如果目标在自己的射程内，直接返回
            if (_moveEnemyTeam != null)
            {
                foreach (var enemyUnit in _moveEnemyTeam.AllUnitList)
                {
                    if (enemyUnit.IsVirtual() || enemyUnit.IsDead() || !enemyUnit.CanTarget())
                    {
                        continue;
                    }
                    if(_unit.IsAttackTargetInMyRange(enemyUnit))
                    {
                        resultTarget = enemyUnit;
                        _SetTargetAttackUnit(resultTarget);
                        return;
                    }
                }
            }
            
            if (_unit.IsMelee())
            {
                //用BSP树加速全局的单位查找
                resultTarget = _unit.EnemyBspTree.FindOneUnitInAttackRange(_unit, true,
                    (u) =>
                    {
                        return !u.IsVirtual() && !u.IsDead();
                    });
                _SetTargetAttackUnit(resultTarget);
            }

            //遍历所有敌方单位
            // foreach (var item in _unit.GetMap().GetUnitDict())
            // {
            //     var u = item.Value;
            //     var side = u.GetSide();
            //     if (u.IsDead() || u.IsVirtual() || side == mySide)
            //         continue;
            //     if (_unit.IsAttackTargetInMyRange(u))
            //     {
            //         resultTarget = u;
            //         break;
            //     }
            // }
            // _SetTargetAttackUnit(resultTarget);

        }

        public override void Exit()
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
        }

        private Unit GetPriorityTargetorMinDistanceEnemy()
        {
            var proiTarget = _unit.PeekPriorityTarget();
            if (proiTarget != null)
            {
                return proiTarget;
            }
            if (_unit.IsMelee())
            {
                return _unit.GetMinDistanceEnemy();
            }

            return _unit.GetTargetTeamMinDistanceEnemy(_moveEnemyTeam);
        }

        public override void Reset()
        {
            _pathTestNodeMax = 50;
            _targetInAttactRange = null;
            _moveEnemyTeam = null;
            
            _pathPriority = ERequestPriority.Medium;
        }
    }
}