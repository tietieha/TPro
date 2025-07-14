/// <summary>
/// 跳跃状态
/// </summary>


using FixPoint;

namespace M.PathFinding
{

    public class StateJumpTo : State
    {
        FixInt2 _moveTarget = FixInt2.zero;
        bool _isReachTarget = false;

        public StateJumpTo(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(FixInt2 target)
        {
            _unit.Log("Enter State: StateJumpTo");

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.ClearWillAttackTarget();
            
            target = _unit.GetMap().ClampWorldPosInMap(target);

            _unit.Internal_RemoveCurrentGridOccupy();
            Integer2 gp = _unit.GetMap().WorldPosToGrid(target);
            Integer2 gpNew = _unit.GetMap().FindNearestNotOccupiedNode(gp.x, gp.y, _unit.GetUnitType());
            _unit.Internal_ChangeOccupyGrid(gpNew.x, gpNew.y);    //占据落点

            //如果格子变更，那么目标点要变更，否则不正确
            if(gpNew != gp)
            {
                target = _unit.GetMap().GetGridCenter(gpNew.x, gpNew.y);
            }

            _moveTarget = target;
            _isReachTarget = false;
        }

        public override void Tick()
        {
            _unit.SetFaceDirToTargetVec2(_moveTarget);
            //逐步移动到目标位置
            bool moveOk = _unit.TryToMove_JumpTo(_moveTarget, ref _isReachTarget);
        }
        
        /// <summary>
        /// 跳跃是否到达目标点
        /// </summary>
        /// <returns></returns>
        public bool IsJumpReachTarget()
        {
            return _isReachTarget;
        }

        /// <summary>
        /// 获取跳跃目标点
        /// </summary>
        /// <returns></returns>
        public FixInt2 GetJumpTarget()
        {
            return _moveTarget;
        }

        public override void Exit()
        {

        }
        public override void Reset()
        {
            _moveTarget = FixInt2.zero;
            _isReachTarget = false;
        }
    }
}