//小兵索敌移动
using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{
    using PositionConstrainFunc = Func<FixInt2, bool>;

    public class StateSoldierSearchEnemy : State, ISetMoveTargetUnit, IGetEnemyInRange
    {
        private int _pathTestNodeMax = 20;
        //射程内的敌人
        private Unit _targetInAttactRange = null;

        //移动目标，可能是自己的英雄
        Unit _currentMoveToUnit = null;

        Unit _moveEnemyUnit = null;
        Team _moveEnemyTeam = null;

        private ERequestPriority _pathPriority = ERequestPriority.Low;
        private readonly int _pathRecomputeInterval = 10;

        private int _waitingFrameAccum = 0;

        //FixFraction _movingDirInRadian = FixFraction.zero;
        //FixFraction _turnAngleSpeedRadianPerSecend = new FixFraction(13960 * 5, 10000L);

        //目标Unit。一般是敌对军团的Hero，可能是虚拟
        public StateSoldierSearchEnemy(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(int targetUnitIndex)
        {
            _unit.Log("Enter State: StateSoldierSearchEnemy: {0}", targetUnitIndex);
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.InternalEnableOccupy_Stand();
            _unit.ClearWillAttackTarget();
            _pathPriority = ERequestPriority.Low;
            var unitDict = _unit.GetMap().GetUnitDict();
            // if(!unitDict.ContainsKey(targetUnitIndex))
            // {
            //     Utils.Log("aa");
            // }
            _moveEnemyUnit = unitDict[targetUnitIndex];
            _moveEnemyTeam = _unit.GetMap().GetTeam(_moveEnemyUnit.GetTeamId());

            _currentMoveToUnit = _moveEnemyUnit;

            //_movingDirInRadian = FixMath.Atan2(_unit.FaceDirNormalized.y, _unit.FaceDirNormalized.x);
        }

        public void SetMoveTargetUnit(Unit u)
        {
            _moveEnemyUnit = u;
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

        public override void Tick()
        {
            //自动在周围找最近的单位打
            //找打了则返回target
            Utils.Assert(_unit.IsOccupiedGird());
            // 确保站在格子上
            _unit.InternalEnableOccupy_Stand();

            //查询现在目标是否在射程内，如果在则不进行任何寻路
            if (_targetInAttactRange != null)
            {
                // var attackDis = _unit.GetAttackDistanceToTarget(_targetInAttactRange);
                // var attackRange = _unit.GetAttackRange();
                if (_unit.IsAttackTargetInMyRange(_targetInAttactRange) && !_targetInAttactRange.IsDead())
                {
                    return;
                }
                else
                {
                    _SetTargetAttackUnit(null);
                }
            }

            int stopDis = Math.Max(_unit.GetRadius(), 500);
            var waitingPathId = _unit.PathState.GetCurrentWaitingPathId();
            var path = _unit.PathState.GetCurrentPath();

            //当前是否在绿圈之内
            bool isInHeroGreenCircle = _unit.IsMeInVirtualHeroGreenCircle();
            var myHero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;
            PositionConstrainFunc posConstrainFunc = null;
            /*
            如果正在朝着敌人移动，
                如果在圈内：保证不能移出圈外，一旦下一步跨出圈外就停下。清空路径，低优先级等下次寻路。
                如果在圈外：更改目的地，向着英雄移动
            如果正在朝着英雄移动
                如果在圈内：达到英雄周边一定范围内后，再次尝试向着敌人移动
                如果在圈外：一直试图向着英雄移动
            */

            if (_currentMoveToUnit == _moveEnemyUnit)
            {
                if (isInHeroGreenCircle)
                {
                    //posConstrainFunc = myHero.IsPositionInMyGreenCircle;
                }
                else
                {
                    _currentMoveToUnit = myHero;
                    _unit.PathState.CancelPathFinding();
                    _unit.PathState.ClearPath();
                    _unit.SendPathFindingRequest_OneUnit(_currentMoveToUnit, stopDis, _pathTestNodeMax, _pathPriority);
                    return;
                }
            }
            else
            {
                if (isInHeroGreenCircle)
                {
                    var distanceToMyHero = (myHero.WorldPos - _unit.WorldPos).magnitude;
                    if (distanceToMyHero < _unit.GetGreenCircleRadius().Value)
                    {
                        _currentMoveToUnit = _moveEnemyUnit;
                        _unit.PathState.CancelPathFinding();
                        _unit.PathState.ClearPath();
                        _unit.SendPathFindingRequest_OneUnit(_currentMoveToUnit, stopDis, _pathTestNodeMax, _pathPriority);
                        return;
                    }
                }
                else
                {
                    // 如果没有在寻路状态，则需要重新寻路走到绿圈内
                    if (path.Count == 0)
                    {
                        _unit.SendPathFindingRequest_OneUnit(_currentMoveToUnit, stopDis, _pathTestNodeMax, _pathPriority);
                        return;
                    }
                }
            }

            if (path.Count == 0)
            {
                //如果没有在等待路径计算
                if (waitingPathId < 0)
                {
                    if (isInHeroGreenCircle)
                        _UpdateTargetInAttackRange();

                    if (_targetInAttactRange == null)
                    {
                        //如果没有路，并且没有在等待路径返回。说明是在发呆
                        if (_currentMoveToUnit == _moveEnemyUnit)
                        {
                            // var t = _unit.GetMinDistanceAliveEnemy();
                            //_unit.SendPathFindingRequest_OneUnit(t, stopDis, _pathTestNodeMax, _pathPriority);
                            _unit.SendPathFindingRequest_Team(_moveEnemyTeam.Id, stopDis, _pathTestNodeMax, _pathPriority);
                        }
                        else
                            _unit.SendPathFindingRequest_OneUnit(_currentMoveToUnit, stopDis, _pathTestNodeMax, _pathPriority);
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

                        bool moveOk = _unit.TryToMove(targetPos, ref isReachTarget, disScale, posConstrainFunc, true);

                        if (!moveOk)
                        {
                            _waitingFrameAccum++;
                            if (_waitingFrameAccum >= 6)
                            {
                                _unit.PathState.ClearPath();
                                if (isInHeroGreenCircle)
                                    _UpdateTargetInAttackRange();
                                break;
                            }

                        }
                        else
                            _waitingFrameAccum = 0;

                        //达到了这个路点，从路径中移除
                        if (isReachTarget)
                        {
                            int movedDis = (targetPos - startPos).magnitude;
                            movedDisAccum += movedDis;
                            disScale = FixFraction.one - new FixFraction(movedDisAccum, moveDisInOneFrame); // movedDisAccum / moveDisInOneFrame;
                            path.Dequeue();
                        }
                        else
                            break;
                    }

                    if (isInHeroGreenCircle)
                        _UpdateTargetInAttackRange();
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval && waitingPathId < 0)
                {
                    if (_currentMoveToUnit == _moveEnemyUnit)
                    {
                        //var t = _unit.GetMinDistanceEnemy();
                        //_unit.SendPathFindingRequest_OneUnit(t, stopDis, _pathTestNodeMax, _pathPriority);
                        _unit.SendPathFindingRequest_Team(_moveEnemyTeam.Id, stopDis, _pathTestNodeMax, ERequestPriority.Low);
                    }
                    else
                        _unit.SendPathFindingRequest_OneUnit(_currentMoveToUnit, stopDis, _pathTestNodeMax, ERequestPriority.Low);
                }
            }
        }

        private void _UseFarAttackRangeFindTarget()
        {
            // TODO LZL 在这里判断一下, 策划需求是, 先动态增加小兵的攻击距离, 打一个最近的, 如果没有, 再找一个可以走过去的敌人, 走过去打就好了
            if (!_unit.IsOpenDynamicAttackRange() && _unit.IsMelee())
            {
                // 找最近的敌人
                var t = _unit.GetMinDistanceAliveEnemy();
                if (t != null)
                {
                    var dis = (_unit.GetWorldPos() - t.GetWorldPos()).magnitude;
                    // 这时候的攻击距离是不够的, 以为之前已经调用_UpdateTargetInAttackRange()找了一次了, 需要动态增加攻击距离
                    if (dis <= _unit.GetAttackRange() * _unit.GetAttackRangeRate())
                    {
                        Utils.Log($"BattleLog UID {_unit.GetId()}找到了最近的单位{t.GetId()} dis:{dis}, 开启五倍攻击");
                        _unit.SetAttackRangeRateOpen(true);
                        _SetTargetAttackUnit(t);
                    }
                }
            }
        }

        private void _UpdateTargetInAttackRange()
        {
            // bool isMelee = _unit.IsMelee();
            // if (!isMelee)
            // {
            //     var myHero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;
            //     if (!myHero.IsPositionInMyGreenCircle(_unit.WorldPos))
            //         _SetTargetAttackUnit(null);
            // }

            //var minDis = Utils.MAX_DISTANCE;
            int attackRange = _unit.GetAttackRange();

            Unit resultTarget = null;
            var mySide = _unit.GetSide();

            /*
            1、在“可移动范围+普攻距离范围”内，寻找3个直线距离最近的敌对单位，计算路径距离，选择最近路径距离的目标；若3个均无可到达路径，排除后，继续遍历；
            切换目标：
            1、目标死亡时
            2、当普攻距离内有敌方单位，且当前自身没有正在攻击的目标，则攻击该目标；
            */

            //近战的话，只找距离最近的
            if (_unit.IsMelee())
            {
                //用BSP树加速全局的单位查找
                resultTarget = _unit.EnemyBspTree.FindOneUnitInAttackRange(_unit, false,
                    (u) =>
                    {
                        return !u.IsVirtual() &&
                            !u.IsDead() &&
                            (u.UnitState == EUnitState.Attack ||
                            u.UnitState == EUnitState.SoldierSearchEnemy ||
                            u.UnitState == EUnitState.SearchEnemy ||
                            u.UnitState == EUnitState.HeroSearchEnemy ||
                            // u.UnitState == EUnitState.AttackFortification ||
                            // u.UnitState == EUnitState.SearchFortificationEnemy ||
                            u.UnitState == EUnitState.Stop
                            // || u.UnitState == EUnitState.StopAndOccupy
                            );
                    });
                _SetTargetAttackUnit(resultTarget);
            }
            else
            {

                /*
                目标为英雄
                1、当该目标达到远程接敌数标准值时；在其半径为4的圆形区域内，筛选3个的敌对目标，判断其远程接敌数，选择远程接敌数最高，且尚未达到标准值的单位为目标；不足3个，则有几个选择几个；
                2、若选择的单位，均已达到远程接敌数标准值，则排除后，继续遍历其他符合条件的单位；
                3、若符合距离条件的单位，均已达到标准值，则最后遍历的3个单位中，除敌对英雄外，随机1个；
                4、若无符合距离条件的其他单位存在，则以该英雄为目标
                */
                //如果目标是单个武将
                if (_moveEnemyUnit.IsHero() && !_moveEnemyUnit.IsVirtual())
                {
                    //找到半径为4的范围内的所有单位
                    var unitList = _unit.EnemyBspTree.FindUnitInRadius(_moveEnemyUnit.WorldPos, 4000,
                        (u) =>
                        {
                            return !u.IsVirtual() && !u.IsDead();
                        });

                    //筛选小于标准接敌数的单位，选择最大的
                    Unit resultUnit = null;
                    int critValue = -1;
                    foreach (var u in unitList)
                    {
                        if (u.IsDead())
                            continue;
                        var c = u.GetLongRangeWillAttackSource().Count;
                        if (c < u.LongRangeAttackSourceStandNum)
                        {
                            if (c > critValue)
                            {
                                resultUnit = u;
                                critValue = c;
                            }
                        }
                    }
                    if (resultUnit != null && _unit.IsAttackTargetInMyRange(resultUnit))
                    {
                        _SetTargetAttackUnit(resultUnit);
                        return;
                    }

                    //在剩下的单位中，选择接敌数最低的
                    critValue = 9999;
                    foreach (var u in unitList)
                    {
                        if (u.IsDead())
                            continue;
                        var c = u.GetLongRangeWillAttackSource().Count;
                        if (c >= u.LongRangeAttackSourceStandNum)
                        {
                            if (c < critValue)
                            {
                                resultUnit = u;
                                critValue = c;
                            }
                        }
                    }
                    if (resultUnit != null && _unit.IsAttackTargetInMyRange(resultUnit))
                    {
                        _SetTargetAttackUnit(resultUnit);
                        return;
                    }

                }
                /*
                目标为军团
                1、 在“ 可移动范围 + 普攻距离范围” 内， 寻找3个目标军团中直线距离最近的单位； 选择远程接敌数最高， 且尚未达到标准值的单位为目标； 不足3个， 则有几个选择几个；
                2、 若选择的单位， 均已达到远程接敌数标准值， 则排除后， 继续遍历其他符合距离条件的该军团单位；
                3、 若符合距离条件的该军团单位， 均已达到标准值， 在最后遍历的3个单位中， 随机1个；
                4、 若无符合距离条件的该军团单位存在， 则以该军团队长为圆心， 在其半径为4的圆形区域内， 筛选所有敌对目标； 还没有， 则不断扩大搜索半径；
                */
                //如果目标是小兵兵团
                else
                {
                    var unitList = _moveEnemyTeam.AllUnitList;
                    //筛选小于标准接敌数的单位，选择最大的
                    Unit resultUnit = null;
                    int critValue = -1;
                    foreach (var u in unitList)
                    {
                        if (u.IsVirtual() || u.IsDead())
                            continue;
                        var c = u.GetLongRangeWillAttackSource().Count;
                        if (c < u.LongRangeAttackSourceStandNum)
                        {
                            if (c > critValue)
                            {
                                resultUnit = u;
                                critValue = c;
                            }
                        }
                    }
                    if (resultUnit != null && _unit.IsAttackTargetInMyRange(resultUnit))
                    {
                        _SetTargetAttackUnit(resultUnit);
                        return;
                    }

                    //在剩下的单位中，选择接敌数最低的
                    critValue = 9999;
                    foreach (var u in unitList)
                    {
                        if(u.IsVirtual() || u.IsDead() || !u.CanTarget())
                            continue;
                        var c = u.GetLongRangeWillAttackSource().Count;
                        if (c >= u.LongRangeAttackSourceStandNum)
                        {
                            if (c < critValue)
                            {
                                resultUnit = u;
                                critValue = c;
                            }
                        }
                    }
                    if (resultUnit != null && _unit.IsAttackTargetInMyRange(resultUnit))
                    {
                        _SetTargetAttackUnit(resultUnit);
                        return;
                    }
                }
                _SetTargetAttackUnit(null);
            }
            // //遍历所有敌方单位
            // foreach (var item in _unit.GetMap().GetUnitDict())
            // {
            //     var u = item.Value;
            //     var side = u.GetSide();
            //     if (u.IsDead() || u.IsVirtual() || side == mySide)
            //         continue;
            //     if (_unit.IsAttackTargetInMyRange(u))
            //     {
            //         resultTarget = u;
            //         break;
            //     }
            // }
            // _SetTargetAttackUnit(resultTarget);
        }

        public override void Exit()
        {
            Reset();
        }

        public override void Reset()
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();

            _pathTestNodeMax = 20;
            //射程内的敌人
            _targetInAttactRange = null;

            //移动目标，可能是自己的英雄
            _currentMoveToUnit = null;

            _moveEnemyUnit = null;
            _moveEnemyTeam = null;

            _pathPriority = ERequestPriority.Low;
            _waitingFrameAccum = 0;

    }
    }
}