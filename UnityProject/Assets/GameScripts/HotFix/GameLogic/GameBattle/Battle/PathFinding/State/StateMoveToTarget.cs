using System;
using FixPoint;
namespace M.PathFinding
{
    public class StateMoveToTarget: State
    {
        private int _moveToTargetId;
        private int _pathTestNodeMax = 50;
        private int _noPathWaitFrame = 0;
        private int _waitingFrameAccum = 0;
        private readonly int _pathRecomputeInterval = 10;
        private ERequestPriority _pathPriority = ERequestPriority.Low;
        private Unit _targetInAttactRange = null;
        public StateMoveToTarget(Unit unit)
        {
            _unit = unit;
        }
        public void Enter(int targetId)
        {
            _unit.SetPathFindingBlocked((int)PathFindingBlockType.None);
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();
            _targetInAttactRange = null;
            _moveToTargetId = targetId;
        }
        public override void Tick()
        {
            if (_moveToTargetId == 0)
                return;
            if (_targetInAttactRange != null)
                return;
            int stopDis = Math.Max(_unit.GetRadius(), 500) + _unit.GetAttackRange();
            Unit target = _unit.GetMap().GetUnit(_moveToTargetId);
            if (_unit.IsAttackTargetInMyRange(target))
            {
                _SetTargetAttackUnit(target);
                return;
            }
            var waitingPathId = _unit.PathState.GetCurrentWaitingPathId();
            var path = _unit.PathState.GetCurrentPath();
            //当前没有寻路路径
            if (path.Count == 0)
            {
                //当前没有等待的寻路请求
                if (waitingPathId < 0)
                {
                    // Unit target = _unit.GetMap().GetUnit(_moveToTargetId);
                    if (target.IsDead())
                    {
                        _moveToTargetId = 0;
                        _unit.SetPathFindingBlocked((int)PathFindingBlockType.NoPath);
                        return;
                    }
                    _unit.SendPathFindingRequest_OneUnit(target, stopDis, _pathTestNodeMax, _pathPriority);
                    _pathPriority = ERequestPriority.Low;
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
                                _moveToTargetId = 0;
                                _unit.SetPathFindingBlocked((int)PathFindingBlockType.Blocked);
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

                    // Unit target = _unit.GetMap().GetUnit(_moveToTargetId);
                    if (_unit.IsAttackTargetInMyRange(target) && _targetInAttactRange == null)
                    {
                        _SetTargetAttackUnit(target);
                        return;
                    }
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval &&
                    waitingPathId < 0)
                {
                    // Unit target = _unit.GetMap().GetUnit(_moveToTargetId);
                    _unit.SendPathFindingRequest_OneUnit(target, stopDis,
                            _pathTestNodeMax, ERequestPriority.Low);
                }
            }
        }

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

        public override void Exit()
        {
            Reset();
        }

        public override void Reset()
        {
            _targetInAttactRange = null;
            _moveToTargetId = 0;
            _unit.SetPathFindingBlocked((int)PathFindingBlockType.None);
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
        }

        [XLua.BlackList]
        public int GetTargetId()
        {
            return _moveToTargetId;
        }
    }
}