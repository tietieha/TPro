//虚拟英雄寻路移动到目标。用在虚拟英雄的寻路中
//会绕开兵线

using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{
    public class StateVirtualHeroMovePathTo : State, ISetMoveTargetUnit, IGetEnemyInRange
    {
        private int _pathTestNodeMax = 20;
        //射程内的敌人
        private Unit _targetInAttactRange = null;

        //目标敌人
        Unit _moveToUnit = null;
        private ERequestPriority _pathPriority = ERequestPriority.High;

        private readonly int _pathRecomputeInterval = 10;

        //目标Unit。一般是敌对军团的Hero，可能是虚拟
        public StateVirtualHeroMovePathTo(Unit unit)
        {
            _unit = unit;
        }

        //当寻路结果返回的时候
        public override void OnPathFindFinished(PathState ps)
        {
            if(!ps.IsFoundPath())
            {
                _pathTestNodeMax = 100;
            }
            //else
            //    _pathTestNodeMax = 20;
        }

        //进入这个状态
        public void Enter(int targetUnitId)
        {
            _unit.Log("Enter State: StateVirtualHeroMovePathTo: {0}", targetUnitId);
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();

            _pathPriority = ERequestPriority.High;

            var unitDict = _unit.GetMap().GetUnitDict();
            this._moveToUnit = unitDict[targetUnitId];
            _pathTestNodeMax = 20;
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
        }

        public Unit GetEnemyInRange()
        {
            if (_targetInAttactRange != null && _targetInAttactRange.IsDead())
                return null;
            return _targetInAttactRange;
        }

        public override void Tick()
        {
            Utils.Assert(_unit.IsOccupiedGird());

            // if(_unit.GetId() == 36)
            // {
            //     Utils.Log("aaa");
            // }

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
                    _UpdateTargetInAttackRange();
                    if (_targetInAttactRange == null)
                    {
                        //如果没有路，并且没有在等待路径返回。说明是在发呆
                        _unit.SendPathFindingRequest_OneUnit(_moveToUnit, stopDis, _pathTestNodeMax, _pathPriority);
                        _pathPriority = ERequestPriority.High;
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
                    _unit.SendPathFindingRequest_OneUnit(_moveToUnit, stopDis, _pathTestNodeMax, ERequestPriority.Medium);
                }
            }

        }
        private void _UpdateTargetInAttackRange()
        {
            var minDis = Utils.MAX_DISTANCE;
            int attackRange = _unit.GetAttackRange();

            Unit resultTarget = null;
            var u = _moveToUnit;
            var d2 = _unit.GetAttackDistanceToTarget(u);
            int realAttackRange = attackRange;

            if (d2 < realAttackRange)
                resultTarget = u;

            _SetTargetAttackUnit(resultTarget);
        }

        public override void Exit()
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _moveToUnit = null;
        }

        public override void Reset()
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();

            _pathTestNodeMax = 20;
            _targetInAttactRange = null;

            _moveToUnit = null;
            _pathPriority = ERequestPriority.High;

        }
    }
}