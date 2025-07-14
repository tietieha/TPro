using System;
using FixPoint;

namespace M.PathFinding
{
    [XLua.BlackList]
    public class StateWaitVirtualHeroAssignTarget : State, IGetEnemyInRange
    {
        // private int _attackTargetUnitId = 0;
        private bool _waitAssign = false;
        private int _attackTargetUnitId = 0;
        //找不到路的等待帧
        private int _noPathWaitFrame = 0;
        //找不到路的等待总帧数
        private const int _noPathWaitTotalFrame = 6;
        public StateWaitVirtualHeroAssignTarget(Unit unit)
        {
            _unit = unit;
        }

        public void Enter()
        {
            var myHero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;
            var targetId = myHero.GetVirtualHeroAssignTarget(_unit.GetId());
            if (targetId > 0)
                _attackTargetUnitId = targetId;
            else
                _waitAssign = true;
            // if (_attackTargetUnitId > 0)
            // {
            //     _SetTargetAttackUnit(_unit.GetMap().GetUnit(_attackTargetUnitId));
            // }
        }

        public override void Tick()
        {
            if (_waitAssign)
            {
                var myHero = _unit.Team.Hero;
                var targetId = myHero.GetVirtualHeroAssignTarget(_unit.GetId());
                if (targetId > 0)
                    _attackTargetUnitId = targetId;
                _waitAssign = false;
            }

            // if (_attackTargetUnitId > 0 && _targetInAttactRange == null)
            // {
                // _unit.ChangeState_Attack(_attackTargetUnitId);
                // _SetTargetAttackUnit(_unit.GetMap().GetUnit(_attackTargetUnitId));
            // }
            if (_attackTargetUnitId == 0)
            {
                if (!_waitAssign)
                    _waitAssign = true;
                return;
            }
            if (_targetInAttactRange != null && !_unit.IsAttackTargetInMyRange(_targetInAttactRange))
            {
                _SetTargetAttackUnit(null);
            }
            int stopDis = Math.Max(_unit.GetRadius(), 500);
            var waitingPathId = _unit.PathState.GetCurrentWaitingPathId();
            var path = _unit.PathState.GetCurrentPath();
            if (path.Count == 0)
            {
                //如果没有在等待路径计算
                if (waitingPathId < 0)
                {
                    Unit target = _unit.GetMap().GetUnit(_attackTargetUnitId);
                    if (target.IsDead())
                    {
                        _attackTargetUnitId = 0;
                        _waitAssign = true;
                        return;
                    }
                    if (_unit.IsAttackTargetInMyRange(target) && _targetInAttactRange == null)
                    {
                        _SetTargetAttackUnit(target);
                        return;
                    }
                    if (_targetInAttactRange == null || !_unit.IsAttackTargetInMyRange(target))
                    {
                        _noPathWaitFrame++;
                        var curPathTestNodeMax = _noPathWaitFrame < _noPathWaitTotalFrame ?
                            _pathTestNodeMax : _pathTestNodeMax + (_noPathWaitFrame -
                                _noPathWaitTotalFrame) * 10;
                        _unit.SendPathFindingRequest_OneUnit(target, stopDis, curPathTestNodeMax, _pathPriority);
                        _pathPriority = ERequestPriority.Low;
                    }
                }
                else //如果正在等待路径计算
                {

                    //啥都不干
                }
            }
            else
            {
                if (path.Count > 0)
                {
                    _noPathWaitFrame = 0;
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

                        bool moveOk = _unit.TryToMove(targetPos, ref isReachTarget,
                            disScale, null, true);

                        if (!moveOk)
                        {
                            _waitingFrameAccum++;
                            if (_waitingFrameAccum >= 6)
                            {
                                _unit.PathState.ClearPath();
                                _waitAssign = true;
                                return;
                            }
                        }
                        else
                            _waitingFrameAccum = 0;

                        //达到了这个路点，从路径中移除
                        if (isReachTarget)
                        {
                            int movedDis = (targetPos - startPos).magnitude;
                            movedDisAccum += movedDis;
                            disScale = FixFraction.one -
                                       new FixFraction(movedDisAccum,
                                           moveDisInOneFrame); // movedDisAccum / moveDisInOneFrame;
                            path.Dequeue();
                        }
                        else
                            break;
                    }

                    // if (isInHeroGreenCircle)
                    //     _UpdateTargetInAttackRange();
                    Unit target = _unit.GetMap().GetUnit(_attackTargetUnitId);
                    if (_unit.IsAttackTargetInMyRange(target) && _targetInAttactRange == null)
                    {
                        _SetTargetAttackUnit(target);
                        return;
                    }
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval &&
                    waitingPathId < 0)
                {
                    // if (_currentMoveToUnit == _moveEnemyUnit)
                    // {
                    //     //var t = _unit.GetMinDistanceEnemy();
                    //     //_unit.SendPathFindingRequest_OneUnit(t, stopDis, _pathTestNodeMax, _pathPriority);
                    //     _unit.SendPathFindingRequest_Team(_moveEnemyTeam.Id, stopDis,
                    //         _pathTestNodeMax, ERequestPriority.Low);
                    // }
                    // else
                    // {
                    Unit target = _unit.GetMap().GetUnit(_attackTargetUnitId);
                    _unit.SendPathFindingRequest_OneUnit(target, stopDis,
                            _pathTestNodeMax, ERequestPriority.Low);
                    // }
                }
            }
        }

        private int _pathTestNodeMax = 50;

        //射程内的敌人
        private Unit _targetInAttactRange = null;
        private ERequestPriority _pathPriority = ERequestPriority.Low;
        private readonly int _pathRecomputeInterval = 10;
        private int _waitingFrameAccum = 0;

        private void _SetTargetAttackUnit(Unit u)
        {
            if (Utils.IsDebug())
            {
                if (u != null)
                    _unit.Log("SetTargetAttackUnit: {0}", u.GetId());
                else
                    _unit.Log("SetTargetAttackUnit: null");
            }

            _targetInAttactRange = u;
            _unit.SetWillAttackTarget(u);
        }

        public Unit GetEnemyInRange()
        {
            if (_targetInAttactRange != null && _targetInAttactRange.IsDead())
                return null;
            return _targetInAttactRange;
        }

        public override void Exit()
        {
            Reset();
        }

        public override void Reset()
        {
            _attackTargetUnitId = 0;
            _waitAssign = false;
        }
    }
}