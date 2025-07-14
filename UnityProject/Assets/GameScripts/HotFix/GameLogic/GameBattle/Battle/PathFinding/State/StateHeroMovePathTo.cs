//英雄单独寻路
using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{
    public class StateHeroMovePathTo : State, ISetMoveTargetUnit, IGetEnemyInRange
    {
        private int _pathTestNodeMax = 50;
        //射程内的敌人
        private Unit _targetInAttactRange = null;

        //目标敌人
        Unit _moveToUnit = null;
        private ERequestPriority _pathPriority = ERequestPriority.High;
        private readonly int _pathRecomputeInterval = 10;

        public StateHeroMovePathTo(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(int targetUnitId)
        {
            _unit.Log("Enter State: StateHeroMovePathTo: {0}", targetUnitId);
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();
            _targetInAttactRange = null;

            _pathPriority = ERequestPriority.High;

            var unitDict = _unit.GetMap().GetUnitDict();
            this._moveToUnit = unitDict[targetUnitId];
        }

        public void SetMoveTargetUnit(Unit u)
        {
            _moveToUnit = u;
        }

        private void _SetTargetAttackUnit(Unit u)
        {
            if (u != null)
                _unit.Log("SetTargetAttackUnit: {0}", u.GetId());
            else
                _unit.Log("SetTargetAttackUnit: null");
            _targetInAttactRange = u;
            _unit.SetWillAttackTarget(u);
        }

        public Unit GetEnemyInRange()
        {
            if (_targetInAttactRange != null && _targetInAttactRange.IsDead())
                return null;
            return _targetInAttactRange;
        }

        public override void Tick()
        {
            //试图寻找一条路径到目标。
            //路上不会打别人。如果要自有攻击，请用StateHeroSearchEnemy

            Utils.Assert(_unit.IsOccupiedGird());

            //查询现在目标是否在射程内，如果在则不进行任何寻路
            if (_targetInAttactRange != null)
            {
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

            if (path.Count == 0)
            {
                if (waitingPathId < 0) //如果没有在等待路径计算
                {
                    UpdateTargetInAttackRange();
                    if (_targetInAttactRange == null)
                    {
                        //如果没有路，并且没有在等待路径返回。说明是在发呆
                        _unit.SendPathFindingRequest_OneUnit(_moveToUnit, stopDis, _pathTestNodeMax, _pathPriority);
                        _pathPriority = ERequestPriority.Medium;

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

                    UpdateTargetInAttackRange();
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval && waitingPathId < 0)
                {
                    _unit.SendPathFindingRequest_OneUnit(_moveToUnit, stopDis, _pathTestNodeMax, ERequestPriority.High);
                }
            }
        }

        private void UpdateTargetInAttackRange()
        {
            var minDis = Utils.MAX_DISTANCE;
            int attackRange = _unit.GetAttackRange();

            Unit resultTarget = null;
            var u = _moveToUnit;
            var d2 = _unit.GetAttackDistanceToTarget(u);
            int realAttackRange = attackRange;

            if (d2 < realAttackRange)
                resultTarget = u;
            if (resultTarget == null && _unit.GetMeleeAttackSourceCount()>0)
            {
                // 查找是否有近战打我
                foreach (var tar in _unit.WillAttackSourceSet)
                {
                    if (tar.IsDead() || !tar.CanTarget() || !tar.IsMelee() || tar.GetSide() == _unit.GetSide())
                        continue;

                    var d3 = _unit.GetAttackDistanceToTarget(tar);
                    if (d3 < realAttackRange)
                    {
                        resultTarget = tar;
                        break;
                    }
                }
            }

            _SetTargetAttackUnit(resultTarget);

        }

        private int GetAttackDistanceToTarget(Unit u)
        {
            return (_unit.GetWorldPos() - u.GetWorldPos()).magnitude - u.GetRadius() - _unit.GetRadius();
        }

        public override void Exit()
        {

        }

        public override void Reset()
        {
            _pathTestNodeMax = 50;
            _targetInAttactRange = null;

            _moveToUnit = null;
            _pathPriority = ERequestPriority.High;
        }
    }
}