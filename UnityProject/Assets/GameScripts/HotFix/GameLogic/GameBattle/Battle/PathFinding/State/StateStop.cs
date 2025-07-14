namespace M.PathFinding
{

    public class StateStop : State
    {
        int _targetAttackUnitId = 0;

        public StateStop(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(int targetUnitIndex)
        {
            _unit.Log("Enter State: StateStop");
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();
            // _unit.StandAtGridCenter();
            //_unit.GetMap().CreateAttackRelation(_unit.GetId(), targetUnitIndex);
            _targetAttackUnitId = targetUnitIndex;
        }

        public override void Tick()
        {

        }

        public override void Exit()
        {
            if (_targetAttackUnitId > 0)
            {
                _unit.GetMap().RemoveLongAttackReleation(_unit.GetTeamId(), _targetAttackUnitId);
                _targetAttackUnitId = 0;
            }

            if (_unit.IsMelee() && _targetAttackUnitId > 0)
            {
                var targetUnit = _unit.GetMap().GetUnit(_targetAttackUnitId);
                targetUnit.RemoveMeleeAttackSource();
            }
        }

        public override void Reset()
        {
            _targetAttackUnitId = 0;
        }
    }
}