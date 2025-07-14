using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{

    public class StateHeroMoveStraightTo : State, ISetMoveTargetPosition, ISetMoveTargetUnit, IGetEnemyInRange
    {
        private int _pathTestNodeMax = 50;
        //射程内的敌人
        private Unit _targetInAttactRange = null;

        FixInt2 _moveTarget = FixInt2.zero;
        EReachTargetSt _reachTargetSt = EReachTargetSt.None;

        //目标敌人
        Unit _moveToUnit = null;
        private readonly int _pathRecomputeInterval = 10;

        public StateHeroMoveStraightTo(Unit unit)
        {
            _unit = unit;
        }

        //骑兵
            //虚拟武将
            //骑兵

        //进入这个状态
        public void Enter(FixInt2 target)
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();

            SetMoveTargetPosition(target);
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

            if (target == _moveTarget)
                return;

            _moveTarget = target;
            _reachTargetSt = EReachTargetSt.None;
            _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
        }


        public void SetMoveTargetUnit(Unit u)
        {
            if (_moveToUnit != u && u!=null)
            {
                _reachTargetSt = EReachTargetSt.None;
                _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
            }
            _moveToUnit = u;
        }


        private void _SetTargetAttackUnit(Unit u)
        {
            if(Utils.IsDebug())
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

        // }
        public override void Tick()
        {
            //试图寻找一条路径到目标。
            //路上不会打别人。如果要自有攻击，请用StateHeroSearchEnemy

            //Utils.Assert(_unit.IsOccupiedGird());

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

            if (IsReachTarget())
                return;

            bool moveOk = _unit.TryToMoveAndReach(_moveTarget, _unit.GetSpeed(), ref _reachTargetSt, FixFraction.one, null, true);
            if (_reachTargetSt != EReachTargetSt.None)
            {
                _unit.LuaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
            }
            _unit.Log("TryToMove st: {0}", moveOk);
            UpdateTargetInAttackRange();
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
            _moveTarget = FixInt2.zero;
            _reachTargetSt = EReachTargetSt.None;
            
            _moveToUnit = null;
        }
    }
}