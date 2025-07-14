//跟随英雄，但是带转向速度

using FixPoint;

namespace M.PathFinding
{

    public class StateHorseFollowVirtualHero : State
    {
        FixFraction _movingDirInRadian = FixFraction.zero;
        FixFraction _turnAngleSpeedRadianPerSecend = FixFraction.zero;
        Unit _hero;

        public StateHorseFollowVirtualHero(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter(int initDirInRadian, int turnAngleSpeedRadianPerSecend)
        {
            //Utils.Assert(!_unit.IsHero());
            _unit.Log("Enter State: StateHorseFollowVirtualHero {0}, {1}", initDirInRadian, turnAngleSpeedRadianPerSecend);
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();

            _hero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;

            _movingDirInRadian = new FixFraction(initDirInRadian, 10000L);
            _turnAngleSpeedRadianPerSecend = new FixFraction(turnAngleSpeedRadianPerSecend, 10000L);
        }

        public override void Tick()
        {
            var p = _unit.GetPosRelativeToHero();

            FixInt2 d = _hero.FaceDirNormalized;
            int newX = (p.x * d.x - p.y * d.y) / FixInt2.Scale;
            int newY = (p.x * d.y + p.y * d.x) / FixInt2.Scale;

            var t = new FixInt2(newX, newY) + _hero.GetWorldPos();
            t = _unit.GetMap().ClampWorldPosInMap(t);
            bool isReachTarget = false;

            //加速追赶武将
            var moveDisScale = new FixFraction(15000, 10000);

            var posCurrentTarget = _unit.CalculateMovingTargetWithTurnAngleSpeed(_turnAngleSpeedRadianPerSecend, t, ref _movingDirInRadian);

            bool moveOk = _unit.TryToMove(posCurrentTarget, ref isReachTarget, moveDisScale, null, true);

            //_unit.SetSpeed(oldSpeed);
        }

        public override void Exit()
        {

        }
        public override void Reset()
        {
            _movingDirInRadian = FixFraction.zero;
            _turnAngleSpeedRadianPerSecend = FixFraction.zero;
            _hero = null;
        }
    }
}