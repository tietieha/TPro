using System;
using FixPoint;
using XLua;

namespace M.PathFinding
{

    public partial class Unit
    {
        public BSPTree MyBspTree { get; set; } = null;
        public BSPTree EnemyBspTree { get; set; } = null;


        private static bool FilterNonVirtual(Unit u)
        {
            return !u.IsVirtual();
        }
        //获取距离最近的敌人
        public Unit GetMinDistanceEnemy()
        {
            var result = EnemyBspTree.FindNearestUnit(WorldPos, FilterNonVirtual);

            return result;
        }

        private static bool FilterNonVirtualAndAlive(Unit u)
        {
            return !u.IsVirtual() && !u.IsDead();
        }
        //获取距离最近没有死亡的敌人
        public Unit GetMinDistanceAliveEnemy()
        {
            var result = EnemyBspTree.FindNearestUnit(WorldPos, FilterNonVirtualAndAlive);

            return result;
        }
        //获取距离最近的友军（不在本军团的友军）
        public Unit GetMinDistanceFriend()
        {
            var result = MyBspTree.FindNearestOtherTeamUnit(this, WorldPos, FilterNonVirtual);

            return result;
        }


        //获取目标军团中士兵距离最近的敌人
        public Unit GetTargetTeamMinDistanceEnemy(Team enemyTeam)
        {
            Unit result = null;
            if (enemyTeam == null)
                return result;

            var minDisSqr = Utils.MAX_DISTANCE;
            foreach (var u in enemyTeam.AllUnitList)
            {
                if (u.IsDead() || u.IsVirtual() || !u.CanTarget())
                    continue;

                long disSqr = (WorldPos - u.WorldPos).sqrMagnitude;
                if (disSqr < minDisSqr)
                {
                    minDisSqr = disSqr;
                    result = u;
                }
            }
            return result;
        }
    }
}