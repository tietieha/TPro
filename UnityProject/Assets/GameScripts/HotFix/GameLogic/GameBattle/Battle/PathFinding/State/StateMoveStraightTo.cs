
using FixPoint;

namespace M.PathFinding
{

    public class StateMoveStraightTo : State, ISetMoveTargetPosition, ISetMoveTargetUnit
    {
        FixInt2 _moveTarget = FixInt2.zero;
        bool _isReachTarget = false;
        Unit _moveUnit = null;

        public StateMoveStraightTo(Unit unit)
        {
            _unit = unit;
        }

        //骑兵
            //虚拟武将
            //骑兵

        //进入这个状态
        public void Enter(FixInt2 target)
        {
            _unit.Log("Enter State: StateMoveStraightTo");

            target = _unit.GetMap().ClampWorldPosInMap(target);

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();
            
            SetMoveTargetPosition(target);
            _moveUnit = null;
        }

        public bool IsReachTarget()
        {
            return _isReachTarget;
        }

        public EReachTargetSt GetReachTargetSt()
        {
            return _isReachTarget ? EReachTargetSt.Normal : EReachTargetSt.None;
        }

        public void SetMoveTargetPosition(FixInt2 target)
        {
            target = _unit.GetMap().ClampWorldPosInMap(target);
            
            _moveTarget = target;
            _isReachTarget = false;
            _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)EReachTargetSt.None);
        }

        public override void Tick()
        {
            if (_isReachTarget)
                return;
            if (_moveUnit != null)
            {
                _moveTarget = _moveUnit.GetWorldPos();
            }

            _unit.TryToMove(_moveTarget, ref _isReachTarget,FixFraction.one, null, true);
            //if (!moveOk)
            //{
            //    //此状态下没有站位，所以不能移动只能是被障碍物阻挡、到边界等
            //    _isReachTarget = true;
            //}
            if (_isReachTarget)
            {
                _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)EReachTargetSt.Normal);
            }
        }

        public override void Exit()
        {

        }

        public override void Reset()
        {
            _moveTarget = FixInt2.zero;
            _isReachTarget = false;
            _moveUnit = null;
        }

        public void SetMoveTargetUnit(Unit u)
        {
            _moveUnit = u;
        }
    }
}