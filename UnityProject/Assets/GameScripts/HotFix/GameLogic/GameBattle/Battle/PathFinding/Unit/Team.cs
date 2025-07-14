using System.Collections;
using System.Collections.Generic;
using FixPoint;
using XLua;

namespace M.PathFinding
{
    [LuaCallCSharp]
    public class Team
    {
        public int Id { get; set; } = 0;
        public Unit Hero { get; set; } = null;
        public List<Unit> AllUnitList { get; set; } = new List<Unit>(20);

        //本队正要攻击的目标单位集合
        public HashSet<Unit> WillAttackTargetUnitSet { get; set; } = new HashSet<Unit>();

        //正在攻击本队的目标单位集合
        public HashSet<Unit> WillAttackSourceUnitSet { get; set; } = new HashSet<Unit>();

        //本队正在攻击的目标队伍集合。int是单位数量
        public Dictionary<Team, int> WillAttackTargetTeamSet { get; set; } = new Dictionary<Team, int>();

        //正在攻击本队的目标集合
        public Dictionary<Team, int> WillAttackSourceTeamSet { get; set; } = new Dictionary<Team, int>();

        public void Reset()
        {
            Hero = null;
            AllUnitList.Clear();
            WillAttackTargetUnitSet.Clear();
            WillAttackTargetTeamSet.Clear();
            WillAttackSourceUnitSet.Clear();
            WillAttackSourceTeamSet.Clear();
        }

        public void AddWillAttackUnit(Unit u)
        {
            WillAttackTargetUnitSet.Add(u);
            var targetTeam = u.Team;
            if (WillAttackTargetTeamSet.ContainsKey(targetTeam))
                WillAttackTargetTeamSet[targetTeam] += 1;
            else
                WillAttackTargetTeamSet[targetTeam] = 1;
        }

        public void RemoveWillAttackUnit(Unit u)
        {
            WillAttackTargetUnitSet.Remove(u);
            var targetTeam = u.Team;
            WillAttackTargetTeamSet[targetTeam] -= 1;
            if (WillAttackTargetTeamSet[targetTeam] == 0)
                WillAttackTargetTeamSet.Remove(targetTeam);
        }

        public void AddWillAttackSourceUnit(Unit u)
        {
            WillAttackSourceUnitSet.Add(u);
            var sourceTeam = u.Team;
            if (WillAttackSourceTeamSet.ContainsKey(sourceTeam))
                WillAttackSourceTeamSet[sourceTeam] += 1;
            else
                WillAttackSourceTeamSet[sourceTeam] = 1;
        }

        public void RemoveWillAttackSourceUnit(Unit u)
        {
            WillAttackSourceUnitSet.Remove(u);
            var sourceTeam = u.Team;
            WillAttackSourceTeamSet[sourceTeam] -= 1;
            if (WillAttackSourceTeamSet[sourceTeam] == 0)
                WillAttackSourceTeamSet.Remove(sourceTeam);
        }

        public bool IsLive()
        {
            bool isLive = false;
            foreach (var u in AllUnitList)
            {
                if (u.IsVirtual())
                {
                    continue;
                }

                if (!u.IsDead())
                {
                    isLive = true;
                    break;
                }
            }

            return isLive;
        }

        
    }
}