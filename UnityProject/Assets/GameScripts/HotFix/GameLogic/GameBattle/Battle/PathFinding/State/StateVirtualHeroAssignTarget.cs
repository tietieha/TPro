using System.Collections.Generic;
using UnityEngine;

namespace M.PathFinding
{
    [XLua.BlackList]
    public class StateVirtualHeroAssignTarget: State
    {
        private int _attackTargetVirtualHeroId = 0;
        // 用于复用的集合
        private List<Unit> _enemyUnits = new List<Unit>();
        private List<Unit> _friendlyUnits = new List<Unit>();
        private HashSet<int> _assignedEnemies = new HashSet<int>(); // 使用ID集合避免引用
        // 小兵与目标敌人的映射 (使用ID映射)
        private Dictionary<int, int> _soldierToEnemyMapping = new Dictionary<int, int>();

        private List<int> _waitAssignUnitIds = new List<int>();
        public StateVirtualHeroAssignTarget(Unit unit)
        {
            _unit = unit;
        }

        public void Enter(int attackTargetVirtualHeroId)
        {
            _attackTargetVirtualHeroId = attackTargetVirtualHeroId;
            FirstAssignTarget();
        }

        public override void Tick()
        {
            if (_attackTargetVirtualHeroId <= 0)
                return;
            if (!_unit.Team.IsLive())
            {
                return;
            }
            var battleMap = _unit.GetMap();
            var targetVirtualHero = battleMap.GetUnit(_attackTargetVirtualHeroId);
            if (targetVirtualHero == null)
            {
                return;
            }
            var targetTeam = battleMap.GetTeam(targetVirtualHero.GetTeamId());
            if (targetTeam == null || !targetTeam.IsLive())
            {
                return;
            }

            if (_waitAssignUnitIds.Count > 0)
            {
                SortEnemyUnits();
                int enemyIndex = 0;
                foreach (var unitId in _waitAssignUnitIds)
                {
                    var enemyUnit = _enemyUnits[enemyIndex];
                    _soldierToEnemyMapping[unitId] = enemyUnit.GetId();
                    enemyIndex++;
                    if (enemyIndex >= _enemyUnits.Count)
                    {
                        enemyIndex = 0;
                    }
                }
                _waitAssignUnitIds.Clear();
            }
        }
        //soldier获取目标
        public int GetSoldierTarget(int soldierId)
        {
            if (_soldierToEnemyMapping.TryGetValue(soldierId, out var enemyId))
            {
                var targetUnit = _unit.GetMap().GetUnit(enemyId);
                if (targetUnit == null || targetUnit.IsDead())
                {
                    if (_soldierToEnemyMapping.ContainsKey(soldierId))
                        _soldierToEnemyMapping.Remove(soldierId);
                    _waitAssignUnitIds.Add(soldierId);
                    return 0;
                }
                return enemyId;
            }
            if (!_waitAssignUnitIds.Contains(soldierId))
                _waitAssignUnitIds.Add(soldierId);
            return 0;
        }

        private void SortEnemyUnits()
        {
            var targetVirtualHero = _unit.GetMap().GetUnit(_attackTargetVirtualHeroId);
            var myHero = _unit.Team.Hero;
            var myHeroPos = myHero.GetWorldPos();
            // 获取敌方团队的小兵列表，并按距离我方 VirtualHero 排序
            _enemyUnits.Clear();
            foreach (var enemy in targetVirtualHero.Team.AllUnitList)
            {
                if (!enemy.IsDead() && !enemy.IsHero())
                {
                    _enemyUnits.Add(enemy);
                }
            }
            _enemyUnits.Sort((a, b) => (myHeroPos - a.WorldPos).sqrMagnitude.CompareTo((myHeroPos - b.WorldPos).sqrMagnitude));
        }
        //首次分配目标
        private void FirstAssignTarget()
        {

            SortEnemyUnits();
            if (_enemyUnits.Count == 0)
            {
                return;
            }
            var targetVirtualHero = _unit.GetMap().GetUnit(_attackTargetVirtualHeroId);
            var tarHeroPos = targetVirtualHero.GetWorldPos();
            // 获取我方团队的小兵列表，并按距离敌方 VirtualHero 排序
            _friendlyUnits.Clear();
            _friendlyUnits.AddRange(_unit.Team.AllUnitList);
            _friendlyUnits.Sort((a, b) => (tarHeroPos - a.WorldPos).sqrMagnitude.CompareTo((tarHeroPos - b.WorldPos).sqrMagnitude));
            // 记录已分配的敌人小兵
            _soldierToEnemyMapping.Clear();
            int enemyIndex = 0;
            //给每个小兵分配目标
            foreach (var friendlyUnit in _friendlyUnits)
            {
                if (friendlyUnit.IsDead())
                {
                    continue;
                }
                if (friendlyUnit.IsHero())
                {
                    continue;
                }
                var enemyUnit = _enemyUnits[enemyIndex];
                // Debug.Log($"AddHeroAssignTarget: {friendlyUnit.GetId()} -> {enemyUnit.GetId()}");
                _soldierToEnemyMapping.Add(friendlyUnit.GetId(), enemyUnit.GetId());
                enemyIndex++;
                if (enemyIndex >= _enemyUnits.Count)
                {
                    enemyIndex = 0;
                }
            }
        }


        // 获取攻击次数最少的敌人
        private Unit GetLeastAttackedEnemy(Team enemyTeam)
        {
            Unit leastAttackedEnemy = null;
            int minAttackCount = int.MaxValue;

            foreach (var enemy in enemyTeam.AllUnitList)
            {
                int attackCount = enemy.GetMeleeAttackSourceCount();
                if (attackCount < minAttackCount)
                {
                    minAttackCount = attackCount;
                    leastAttackedEnemy = enemy;
                }
            }

            return leastAttackedEnemy;
        }

        public bool RequestAssignTarget(int unitId)
        {
            return true;
        }

        public override void Exit()
        {
            Reset();
        }

        public override void Reset()
        {
            _attackTargetVirtualHeroId = 0;
            _soldierToEnemyMapping.Clear();
            _assignedEnemies.Clear();
            _enemyUnits.Clear();
            _friendlyUnits.Clear();
        }
    }
}