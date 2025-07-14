//虚拟武将
using FixPoint;

namespace M.PathFinding
{
    public class StateMoveWithTurnAngle : State, ISetMoveTargetPosition, ISetMoveTargetUnit, IGetEnemyInRange
    {
        FixInt2 _moveTargetPos = FixInt2.zero;
        bool _isMoveToPosReachTarget = false;

        Unit _moveTargetUnit = null;
        //射程内的敌人
        private Unit _targetInAttactRange = null;

        FixFraction _movingDirInRadian = FixFraction.zero;
        FixFraction _turnAngleSpeedRadianPerSecend = FixFraction.zero;

        FixInt _virtualHeroCircleRadius;

        public StateMoveWithTurnAngle(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(FixInt2 target, int initDirInRadian, int turnAngleSpeedRadianPerSecend, int circleRadius)
        {
            //_unit.Log("Enter State: StateMoveWithTurnAngle : ({0}, {1}), {2}, {3}", target.x, target.y, initDirInRadian, turnAngleSpeedRadianPerSecend);

            target = _unit.GetMap().ClampWorldPosInMap(target);

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();
            
            _movingDirInRadian = new FixFraction(initDirInRadian, 10000L);
            _turnAngleSpeedRadianPerSecend = new FixFraction(turnAngleSpeedRadianPerSecend, 10000L);
            _virtualHeroCircleRadius = circleRadius;

            SetMoveTargetPosition(target);
        }

        //是否移动到了目标地点
        public bool IsReachTarget()
        {
            return _isMoveToPosReachTarget;
        }

        public EReachTargetSt GetReachTargetSt()
        {
            return _isMoveToPosReachTarget ? EReachTargetSt.Normal : EReachTargetSt.None;
        }

        public void SetMoveTargetPosition(FixInt2 targetPos)
        {
            _moveTargetUnit = null;
            targetPos = _unit.GetMap().ClampWorldPosInMap(targetPos);

            _moveTargetPos = targetPos;
            _isMoveToPosReachTarget = false;
        }

        public void SetMoveTargetUnit(Unit targetUnit)
        {
            _moveTargetUnit = targetUnit;
            _isMoveToPosReachTarget = false;
        }

        private void SetTargetAttackUnit(Unit u)
        {
            // if(u != null)
            //     Utils.Log("StateMoveWithTurnAngle SetTargetAttackUnit!!!");
            if(u != null)
                _unit.Log("SetTargetAttackUnit: {0}", u.GetId());
            else
                _unit.Log("SetTargetAttackUnit: null");
            _targetInAttactRange = u;
            _unit.SetWillAttackTarget(u);
        }

        public Unit GetEnemyInRange()
        {
            if (_targetInAttactRange != null && _targetInAttactRange.IsDead())
                return null;
            // if(_targetInAttactRange != null)
            //     Utils.Log("StateMoveWithTurnAngle GetEnemyInRange!!!");
            // else
            //     Utils.Log("StateMoveWithTurnAngle GetEnemyInRange null");
            return _targetInAttactRange;
        }

        public override void Tick()
        {
            if (_moveTargetUnit == null && _isMoveToPosReachTarget)
                return;

            if (_moveTargetUnit != null)
            {
                //查询现在目标是否在射程内，如果在则不进行任何寻路
                if (_targetInAttactRange != null)
                {
                    if (_unit.IsAttackTargetInMyRange(_targetInAttactRange) && !_targetInAttactRange.IsDead())
                        return;
                    else
                        SetTargetAttackUnit(null);
                }
            }

            var posTarget = _moveTargetPos;
            if (_moveTargetUnit != null)
                posTarget = _moveTargetUnit.WorldPos;
            
            if (posTarget == _unit.WorldPos)
            {
                if (_moveTargetUnit != null)
                {
                    SetTargetAttackUnit(_moveTargetUnit);
                }
                return;
            }
            var vecToTarget = posTarget - _unit.WorldPos;
            var len = vecToTarget.magnitude;
            var vecToTargetN = vecToTarget;
            vecToTargetN.Normalize();
            var vecCurrentDir1000 = new FixInt2(
               (int)FixMath.Cos(_movingDirInRadian.nominal, _movingDirInRadian.denominal).nominal,
               (int)FixMath.Sin(_movingDirInRadian.nominal, _movingDirInRadian.denominal).nominal
            ) / 10;
            long radianOffset10000 = FixInt2.RadianInt(vecCurrentDir1000, vecToTargetN).nominal;
            long crossProduct = FixInt2.CrossLong(vecCurrentDir1000, vecToTargetN);
            //var k = crossProduct > 0 ? 1 : -1;
            if (_virtualHeroCircleRadius.Value > len )
            {
                // 半径内
                _unit.SetFaceDirToTargetVec2(posTarget);
                _unit.SysPos(posTarget);
                _movingDirInRadian = FixMath.Atan2(vecToTargetN.y, vecToTargetN.x);
                _isMoveToPosReachTarget = true;
                _unit.TurnDirection = crossProduct > 0 ? ETurnDirection.Left : ETurnDirection.Right;
            }
            else
            {
                // 判断夹角是否大于配置值
                if (radianOffset10000 > 7854)// 31416/4 45°
                {
                    // 计算出当前点位置
                    var targetPos = _unit.GetWorldPos() + new FixInt2(
                        vecToTargetN.x * _virtualHeroCircleRadius.Value / FixInt2.Scale,
                        vecToTargetN.y * _virtualHeroCircleRadius.Value / FixInt2.Scale);

                    _unit.SetFaceDirToTargetVec2(posTarget);
                    _unit.SysPos(targetPos);
                    _movingDirInRadian = FixMath.Atan2(vecToTargetN.y, vecToTargetN.x);
                    _isMoveToPosReachTarget = false;
                    _unit.TurnDirection = crossProduct > 0 ? ETurnDirection.Left : ETurnDirection.Right;
                }
                else
                {
                    //弧度进行转弯处理
                    var posCurrentTarget = _unit.CalculateMovingTargetWithTurnAngleSpeed(
                        _turnAngleSpeedRadianPerSecend, posTarget, ref _movingDirInRadian);
                    bool moveOk = _unit.TryToMove(
                        posCurrentTarget, 
                        ref _isMoveToPosReachTarget, 
                        FixFraction.one, 
                        null,
                        true);
                }

            }

            if (_moveTargetUnit != null)
                UpdateTargetInAttackRange();

            _unit.CurrFaceRad = (int)_movingDirInRadian.nominal;

            //var r = FixMath.Atan2(posCurrentTarget.y, posCurrentTarget.x);
            //Utils.Log("SetFaceDirToTargetVec2: {0}, {1}", r.nominal, r.denominal);

        }

        

        private void UpdateTargetInAttackRange()
        {
            var minDis = Utils.MAX_DISTANCE;
            int attackRange = _unit.GetAttackRange();

            Unit resultTarget = null;
            var u = _moveTargetUnit;
            var d2 = _unit.GetAttackDistanceToTarget(u);
            int realAttackRange = attackRange;

            if (d2 < realAttackRange)
                resultTarget = u;

            SetTargetAttackUnit(resultTarget);
        }

        public override void Exit()
        {
            _unit.TurnDirection = ETurnDirection.None;
        }

        public override void Reset()
        {
            _unit.TurnDirection = ETurnDirection.None;
            _moveTargetPos = FixInt2.zero;
            _isMoveToPosReachTarget = false;

            _moveTargetUnit = null;
            _targetInAttactRange = null;

            _movingDirInRadian = FixFraction.zero;
            _turnAngleSpeedRadianPerSecend = FixFraction.zero;

            _virtualHeroCircleRadius = 0;
        }
    }
}