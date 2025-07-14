

namespace M.PathFinding
{

    public class StateStopVoid : State
    {
        public StateStopVoid(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter()
        {
            _unit.Log("Enter State: StateStopVoid");

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();
        }

        public override void Tick()
        {

        }

        public override void Exit()
        {

        }

        public override void Reset()
        {
        }
    }
}