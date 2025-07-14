/* StateDashMove.cs
 * caijunjie@topjoy.com
 * 2020/4/9 14:24:12
 * desc
*/

using FixPoint;
using System.Collections.Generic;

namespace M.PathFinding
{

    public class StateDashMove : State, IGetDashEnemy, IGetDashDamageEnemy
    {
        FixInt _faceOffSet;
        FixInt _dashRadius = 0;
        FixInt _damageRadius = 0;
        int _dashJumpDamgeTime = 0;
        FixInt _dashHeroExtraW = 0;
        FixInt _dashHeroExtraH = 0;
        List<int> _currDashEnemyArry = new List<int>();
        List<int> _currDamageEnemyArry = new List<int>();
        Unit _hero;
        bool _isDashJump = false;
        bool _isDashEnd = false;
        FixInt _maxSlidDurPow = 0;
        FixFraction _movingDirInRadian = FixFraction.zero;
        FixFraction _turnAngleSpeedRadianPerSecend = FixFraction.zero;

        public StateDashMove(Unit unit)
        {
            _unit = unit;
        }

        public List<int> GetDashEnemy()
        {
            SearchDashEnemy();
            return _currDashEnemyArry;
        }

        public List<int> GetDashDamageEnemy()
        {
            SearchDamageEnemy();
            return _currDamageEnemyArry;
        }

        //骑兵冲撞状态

        //faceOffSet 提前判断
        //进入这个状态
        public void Enter(FixInt faceOffSet, FixInt dashRadius, 
            FixInt damageRadius, int dashJumpDamgeTime, 
            int dashHeroExtraW, int dashHeroExtraH, 
            FixInt maxSlidDur)
        {
            _unit.Log("Enter State: StateDashMove");

            _faceOffSet = faceOffSet;
            _dashRadius = dashRadius;
            _damageRadius = damageRadius;
            _dashJumpDamgeTime = dashJumpDamgeTime;
            _maxSlidDurPow = maxSlidDur*maxSlidDur;
            _dashHeroExtraW = dashHeroExtraW;
            _dashHeroExtraH = dashHeroExtraH;
            _currDashEnemyArry.Clear();
            _currDamageEnemyArry.Clear();
            if (_hero == null)
                _hero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;

            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();
            _isDashJump = false;
            _isDashEnd = false;
            _unit.LuaArray.SetInt((int)UnitShareIndex.IsDashEnd, 0);
            _unit.LuaArray.SetInt((int)UnitShareIndex.DashStartJump, 0);

            _movingDirInRadian = new FixFraction(_unit.CurrFaceRad.Value, 10000L);
            _turnAngleSpeedRadianPerSecend = new FixFraction(1300, 10000L);
        }

        public override void Tick()
        {
            if (_hero.UnitState == EUnitState.StopVoid)
            {
                //判断自己是否需要继续往前冲
                if (_isDashEnd)
                {
                    // 进行转弯，回到虚拟队长附近
                    var p = _unit.GetPosRelativeToHero();

                    FixInt2 d = _hero.FaceDirNormalized;
                    int newX = (p.x * d.x - p.y * d.y) / FixInt2.Scale;
                    int newY = (p.x * d.y + p.y * d.x) / FixInt2.Scale;

                    var t = new FixInt2(newX, newY) + _hero.GetWorldPos();

                    ////加速追赶武将
                    //var moveDisScale = new FixFraction(15000, 10000);

                    var posCurrentTarget = _unit.CalculateMovingTargetWithTurnAngleSpeed(
                        _turnAngleSpeedRadianPerSecend, t, ref _movingDirInRadian);
                    Moveing(posCurrentTarget);
                    return;
                }
                var dirPosTarget = _unit.WorldPos + new FixInt2(_unit.FaceDirNormalized.x * 10,
                    _unit.FaceDirNormalized.y * 10);
                Moveing(dirPosTarget);

                //判断是否撞击了人
                if (_isDashJump)
                {
                    if (_dashJumpDamgeTime <= 0)
                    {
                        _isDashEnd = true;
                    }
                }
                else
                {
                    //判断是否超过了距离
                    if ((_unit.WorldPos-_hero.WorldPos).sqrMagnitude >= _maxSlidDurPow.Value)
                    {
                        _isDashEnd = true;
                    }
                }
                if (_isDashEnd)
                {
                    _unit.LuaArray.SetInt((int)UnitShareIndex.IsDashEnd, 1);
                }
            }
            else
            {
                var p = _unit.GetPosRelativeToHero();

                FixInt2 d = _hero.FaceDirNormalized;
                int newX = (p.x * d.x - p.y * d.y) / FixInt2.Scale;
                int newY = (p.x * d.y + p.y * d.x) / FixInt2.Scale;

                var t = new FixInt2(newX, newY) + _hero.GetWorldPos();
                
                Moveing(t);
            }
        }

        private void Moveing(FixInt2 tPos)
        {
            tPos = _unit.GetMap().ClampWorldPosInMap(tPos);

            EReachTargetSt eReachTarget = EReachTargetSt.None;
            bool moveOk = _unit.TryToMoveAndReach(tPos, _unit.GetSpeed(), ref eReachTarget, FixFraction.one, null, true);
            if (!_isDashJump)
            {
                if (eReachTarget == EReachTargetSt.BeIntercept)
                {
                    _unit.LuaArray.SetInt((int)UnitShareIndex.DashStartJump, 1);
                    _isDashJump = true;
                }
                else
                {
                    _isDashJump = isWillDash();
                }
            }
            else
            {
                _dashJumpDamgeTime -= _unit.GetMap().GetFrameDeltaMs();
            }
        }

        private bool isWillDash()
        {
            var jumpPos = _unit.WorldPos + new FixInt2(_unit.FaceDirNormalized.x * _faceOffSet.Value / FixInt2.Scale,
                    _unit.FaceDirNormalized.y * _faceOffSet.Value / FixInt2.Scale);

            Node fortifiNode;
            var target = _unit.EnemyBspTree.FindUnitIdInRadiusForDash(jumpPos, _dashRadius.Value, 
                _dashHeroExtraW.Value,_dashHeroExtraH.Value,
                out fortifiNode,
                (u) =>
                {
                    return !u.IsVirtual() && !u.IsDead();
                });

            if (fortifiNode != null || target != null)
            {
                _unit.LuaArray.SetInt((int)UnitShareIndex.DashStartJump, 1);
                return true;
            }
            else
                return false;
        }

        private bool SearchDashEnemy()
        {
            _unit.EnemyBspTree.FindUnitIdInRadiusForDash(_unit.WorldPos, _dashRadius.Value, 
                _dashHeroExtraW.Value, _dashHeroExtraH.Value, ref _currDashEnemyArry,
                (u) =>
                {
                    return !u.IsVirtual() && !u.IsDead();
                });

            return _currDashEnemyArry.Count > 0 ? true : false;
        }
        private void SearchDamageEnemy()
        {
             _unit.EnemyBspTree.FindUnitIdInRadiusForDash(_unit.WorldPos, _damageRadius.Value, 
                 _dashHeroExtraW.Value, _dashHeroExtraH.Value,ref _currDamageEnemyArry,
                (u) =>
                {
                    return !u.IsVirtual() && !u.IsDead();
                });
        }

        public override void Exit()
        {

        }

        public override void Reset()
        {
            _faceOffSet = 0;
            _dashRadius = 0;
            _damageRadius = 0;
            _dashJumpDamgeTime = 0;
            _dashHeroExtraW = 0;
            _dashHeroExtraH = 0;
            _currDashEnemyArry.Clear();
            _currDamageEnemyArry.Clear();
            if (_hero != null)
            {
                _hero = null;
            }
            _isDashJump = false;
            _isDashEnd = false;
            _maxSlidDurPow = 0;
            _movingDirInRadian = FixFraction.zero;
            _turnAngleSpeedRadianPerSecend = FixFraction.zero;
        }
    }
}
