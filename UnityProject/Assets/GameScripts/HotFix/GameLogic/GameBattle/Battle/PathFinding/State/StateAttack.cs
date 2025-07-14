

namespace M.PathFinding
{

    public class StateAttack : State
    {
        int _targetAttackUnitId = 0;

        public StateAttack(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(int targetUnitIndex)
        {
            _unit.Log("Enter State: StateAttack");
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            if (!_unit.IsOccupiedGird())
            {
                _unit.InternalEnableOccupy_Stand();
            }
            //_unit.StandAtGridCenter();

            _targetAttackUnitId = targetUnitIndex;

            if (_unit.IsMelee())
            {
                if (_unit.GetMap().GetUnitDict().ContainsKey(_targetAttackUnitId))
                {
                    var targetUnit = _unit.GetMap().GetUnit(_targetAttackUnitId);
                    targetUnit.AddMeleeAttackSource();
                }
            }
            else
            {
                _unit.GetMap().CreateLongAttackRelation(_unit.GetTeamId(), _targetAttackUnitId);
            }
        }

        public override void Tick()
        {
            if (!_unit.IsHero() && !_unit.IsVirtual())
            {
                var gridPos = _unit.GetGridPos();
                // 位置可能被武将占用
                var node = _unit.GetMap().GetNode(gridPos.x, gridPos.y);
                if (node.IsOccupiedMoreValue(_unit.GetUnitType(), 1))
                {
                    // 除了自己，还有别人站着，这时需要找到空闲地点
                    _unit.InternalEnableOccupy_Stand();
                }
            }
        }

        public override void Exit()
        {
            if(_unit.IsMelee() && _targetAttackUnitId > 0)
            {
                if (_unit.GetMap().GetUnitDict().ContainsKey(_targetAttackUnitId))
                {
                    var targetUnit = _unit.GetMap().GetUnit(_targetAttackUnitId);
                    targetUnit.RemoveMeleeAttackSource();
                }
            }
            if (_targetAttackUnitId >= 0)
            {
                _unit.GetMap().RemoveLongAttackReleation(_unit.GetTeamId(), _targetAttackUnitId);
                _targetAttackUnitId = 0;
            }
        }

        public override void Reset()
        {
            _targetAttackUnitId = 0;
        }
    }
}