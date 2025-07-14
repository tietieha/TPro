using FixPoint;

namespace M.PathFinding
{

    public class StateFollowHero : State
    {
        public StateFollowHero(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter()
        {
            _unit.Log("Enter State: StateFollowHero");
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.Internal_RemoveCurrentGridOccupy();
            _unit.ClearWillAttackTarget();
        }

        public override void Tick()
        {
            Utils.Assert(!_unit.IsHero());

            var hero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;
            var p = _unit.GetPosRelativeToHero();

            FixInt2 d = hero.FaceDirNormalized;
            int newX = (p.x * d.x - p.y * d.y) / FixInt2.Scale;
            int newY = (p.x * d.y + p.y * d.x) / FixInt2.Scale;

            var t = new FixInt2(newX, newY) + hero.GetWorldPos();
            t = _unit.GetMap().ClampWorldPosInMap(t);
            bool isReachTarget = false;
            bool moveOk = _unit.TryToMove(t, ref isReachTarget, FixFraction.one, null, true);
        }

        public override void Exit()
        {

        }

        public override void Reset()
        {

        }
    }
}