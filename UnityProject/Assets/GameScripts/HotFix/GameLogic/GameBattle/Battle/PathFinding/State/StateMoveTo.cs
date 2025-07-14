
using FixPoint;

namespace M.PathFinding
{

    public class StateMoveTo : State, ISetMoveTargetPosition, ISetMoveTargetUnit
    {
        FixInt2 _moveTarget = FixInt2.zero;
        Unit _moveUnit = null;
        private Node _nearestNode = null;
        private bool _isMoveToPos = false;// 是否移动到单位点位置

        EReachTargetSt _reachTargetSt = EReachTargetSt.None;
        int _moveSpeed = 0;

        public StateMoveTo(Unit unit)
        {
            _unit = unit;
        }

        //骑兵
        //虚拟武将
        //骑兵

        //进入这个状态
        public void Enter(FixInt2 target, int speed, bool isMoveToPos)
        {
            _unit.Log("Enter State: StateMoveTo");

            target = _unit.GetMap().ClampWorldPosInMap(target);

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();

            _moveSpeed = speed;
            SetMoveTargetPosition(target);
            _isMoveToPos = isMoveToPos;
            _moveUnit = null;
        }

        public bool IsReachTarget()
        {
            return _reachTargetSt != EReachTargetSt.None;
        }

        public EReachTargetSt GetReachTargetSt()
        {
            return _reachTargetSt;
        }

        public void SetMoveTargetPosition(FixInt2 target)
        {
            target = _unit.GetMap().ClampWorldPosInMap(target);

            _moveTarget = target;
            _reachTargetSt = EReachTargetSt.None;
            _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
        }

        public override void Tick()
        {
            if (IsReachTarget())
                return;
            if (_moveUnit != null)
            {
                if (_moveUnit.IsDead())
                {
                    _reachTargetSt = EReachTargetSt.TargetDisappear;
                    _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
                    return;
                }
                else
                {
                    if (_isMoveToPos)
                    {
                        _moveTarget = _moveUnit.GetWorldPos();
                    }
                    else
                    {
                        var dis = _unit.GetAttackDistanceToTarget(_moveUnit);
                        if (dis < _unit.GetAttackRange())
                        {
                            _reachTargetSt = EReachTargetSt.Normal;
                            _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
                            return;
                        }
                        // 目标的前方
                        //计算出两者的连线方向
                        FixInt2 toTarget = _unit.WorldPos - _moveUnit.WorldPos;
                        toTarget.Normalize();
                        var offseLen = _moveUnit.GetRadius() + _unit.GetAttackRange() + _unit.GetRadius();

                        _moveTarget = _moveUnit.GetWorldPos();
                        _moveTarget.x += offseLen * toTarget.x / FixInt2.Scale;
                        _moveTarget.y += offseLen * toTarget.y / FixInt2.Scale;
                    }
                }
            }

            _unit.TryToMoveAndReach(_moveTarget, _moveSpeed, ref _reachTargetSt, FixFraction.one, null, true);
            if (_moveUnit != null)
            {
                var dis = _unit.GetAttackDistanceToTarget(_moveUnit);
                if (dis < _unit.GetAttackRange())
                {
                    _reachTargetSt = EReachTargetSt.Normal;
                }
            }

            if (_reachTargetSt != EReachTargetSt.None)
            {
                _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
                //站下格子
                if (!_unit.IsOccupiedGird())
                {
                    _unit.InternalEnableOccupy_Stand();
                }
            }
        }

        public override void Exit()
        {

        }

        public override void Reset()
        {
            _moveTarget = FixInt2.zero;
            _moveUnit = null;
            _nearestNode = null;

            _reachTargetSt = EReachTargetSt.None;
            _moveSpeed = 0;
        }

        public void SetMoveTargetUnit(Unit u)
        {
            _moveUnit = u;
        }

    }
}