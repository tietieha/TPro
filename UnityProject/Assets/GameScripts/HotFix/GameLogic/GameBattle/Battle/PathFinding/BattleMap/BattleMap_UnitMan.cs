using System.Collections.Generic;
using FixPoint;
using XLua;

namespace M.PathFinding
{

    public partial class BattleMap
    {

        private readonly Dictionary<int, Unit> _unitDict = new Dictionary<int, Unit>(); //所有单位的Map

        private readonly Dictionary<int, Team> _teamDict = new Dictionary<int, Team>(); //所有Team的Map
        private BattleMapConfig _mapConfig = null;

        public BattleMapConfig GetMapConfig() { return _mapConfig; }

        public Dictionary<int, Unit> GetUnitDict() { return _unitDict; }
        public Team GetTeam(int teamId) { return _teamDict[teamId]; }
        public Unit GetUnit(int unitId)
        {
            return _unitDict[unitId];
        }

        //以Team的方式初始化战场；小兵的初始位置即为衣架位置
        public void InitUnitAndTeam(BattleMapConfig c)
        {
            _mapConfig = c;

            foreach (var teamId in c.GetTeamIdList())
            {
                var t = EntityPoolManager.getTeamObj();
                t.Id = teamId ;
                _teamDict.Add(t.Id, t);
            }

            foreach (var u in c.GetUnitList())
            {
                AddUnit(u._worldPos, u._id, u._teamId, u._side, u._isHero, u._isVirtual, u._isInstrument, u._isMelee, u._speed, u._attackRange, u._radius,
                    u._extend, u._greenCircleRadius,u._meleeStandNum, u._rangeStandNum, u._luaDatAccess);
            }
        }

        // 战场中添加新的兵团
        public void AddExtenTeam(BattleMapConfig c)
        {
            foreach (var teamId in c.GetTeamIdList())
            {
                var t = EntityPoolManager.getTeamObj();
                t.Id = teamId;
                _teamDict.Add(t.Id, t);
            }

            foreach (var u in c.GetUnitList())
            {
                AddUnit(u._worldPos, u._id, u._teamId, u._side, u._isHero, u._isVirtual, u._isInstrument, u._isMelee, u._speed, u._attackRange, u._radius,
                    u._extend, u._greenCircleRadius, u._meleeStandNum, u._rangeStandNum, u._luaDatAccess);
            }
        }

        //1, 加入单位
        private Unit AddUnit(FixInt2 worldPos,
            int id, int teamId, int side, bool isHero, bool isVirtual, bool isInstrument, bool isMelee, int speed, int attackRange,
            int radius, int extend, int greenCircleUnit, int meleeStandNum, int rangeStandNum, LuaArrAccess luaArray)
        {
            //Utils.Assert(extend < _extendLayerNum);

            Unit u = EntityPoolManager.getUnitObj();
            u.InitWithData(this, worldPos, id, teamId, side, isHero, isVirtual, isInstrument, isMelee, speed, attackRange, radius, extend, greenCircleUnit, meleeStandNum, rangeStandNum, luaArray);
            _unitDict.Add(u.GetId(), u);

            Utils.Assert(_teamDict.ContainsKey(teamId));
            var team = _teamDict[teamId];

            u.Team = team;

            team.AllUnitList.Add(u);
            if (u.IsHero())
            {
                Utils.Assert(team.Hero == null);
                team.Hero = u;
            }

            return u;
        }

        //初始化所有单位初始的格子占据信息
        public void InitAllUnitGridData()
        {
            var zoneX = (_maxX + _minX) / 2;
            //设置每个单位的衣架位置
            foreach (var kv in _teamDict)
            {
                var team = kv.Value;
                var hero = team.Hero;
                foreach (var u in team.AllUnitList)
                {
                    if (u == hero)
                    {
                        if (u.WorldPos.x<= zoneX)
                        {
                            u.CurrFaceRad = 0;
                        }
                        else
                        {
                            u.CurrFaceRad = (int)FixFraction.pi.nominal;
                        }
                        continue;
                    }

                    if (u.WorldPos.x <= zoneX) //向着X正向
                    {
                        u.SetPosRelativeToHero(u.GetWorldPos() - hero.GetWorldPos());
                        u.CurrFaceRad = 0;
                    }
                    else
                    {
                        FixInt2 v = u.GetWorldPos() - hero.GetWorldPos();
                        //转180度
                        int length = v.magnitude;
                        FixFraction radian = FixMath.Atan2(v.y, v.x);
                        radian = radian + FixFraction.pi;
                        u.CurrFaceRad = (int)FixFraction.pi.nominal;

                        v.x = length * FixMath.Cos(radian.nominal, radian.denominal);
                        v.y = length * FixMath.Sin(radian.nominal, radian.denominal);

                        u.SetPosRelativeToHero(v);
                    }
                }
            }

            foreach (var kv in _unitDict)
            {
                var u = kv.Value;
                //u.ChangeState_MoveStraight(u.GetWorldPos());
                u.ChangeState_StopVoid();
            }

            //创建BSPTree并把单位全部加入
            BspTreeLeft = BSPTree.Create(this, 4);
            BspTreeRight = BSPTree.Create(this, 4);
            foreach (var item in _unitDict)
            {
                var u = item.Value;
                if(u.GetSide() == 1)
                {
                    BspTreeLeft.AddUnit(u);
                    u.MyBspTree = BspTreeLeft;
                    u.EnemyBspTree = BspTreeRight;
                }
                else
                {
                    BspTreeRight.AddUnit(u);
                    u.MyBspTree = BspTreeRight;
                    u.EnemyBspTree = BspTreeLeft;
                }
            }

        }

        /// <summary>
        /// 初始化单兵团
        /// </summary>
        /// <param name="side"></param>
        /// <param name="teamIdx"></param>
        public void InitSingleTeam(int side, int teamIdx)
        {
            if (!_teamDict.ContainsKey(teamIdx))
                return;

            var zoneX = (_maxX + _minX) / 2;
            //设置每个单位的衣架位置
            var team = _teamDict[teamIdx];
            var hero = team.Hero;
            foreach (var u in team.AllUnitList)
            {
                u.ChangeState_StopVoid();
                if (u == hero)
                {
                    if (u.WorldPos.x <= zoneX)
                    {
                        u.CurrFaceRad = 0;
                    }
                    else
                    {
                        u.CurrFaceRad = (int)FixFraction.pi.nominal;
                    }
                    continue;
                }

                if (u.WorldPos.x <= zoneX) //向着X正向,不能取其身上的side,因为兵团有可能是在召唤中创建，而创建的时机可能会在对面
                {
                    u.SetPosRelativeToHero(u.GetWorldPos() - hero.GetWorldPos());
                    u.CurrFaceRad = 0;
                }
                else
                {
                    FixInt2 v = u.GetWorldPos() - hero.GetWorldPos();
                    //转180度
                    int length = v.magnitude;
                    FixFraction radian = FixMath.Atan2(v.y, v.x);
                    radian = radian + FixFraction.pi;
                    u.CurrFaceRad = (int)FixFraction.pi.nominal;

                    v.x = length * FixMath.Cos(radian.nominal, radian.denominal);
                    v.y = length * FixMath.Sin(radian.nominal, radian.denominal);

                    u.SetPosRelativeToHero(v);
                }
            }

            //把单位全部加入BSPTree
            foreach (var u in team.AllUnitList)
            {
                if (u.GetSide() == 1)
                {
                    BspTreeLeft.AddUnit(u);
                    u.MyBspTree = BspTreeLeft;
                    u.EnemyBspTree = BspTreeRight;
                }
                else
                {
                    BspTreeRight.AddUnit(u);
                    u.MyBspTree = BspTreeRight;
                    u.EnemyBspTree = BspTreeLeft;
                }
            }
        }

        //设置某个单位为英雄
        public void SetUnitHero(Unit u)
        {
            var t = _teamDict[u.GetTeamId()];
            //var oldHero = t.Hero;
            u.Internal_SetHero(true);
            t.Hero = u;
            //老的单位是否是英雄，这里不考虑了。
        }

        public void SetUnitSoldier(Unit u)
        {
            var t = _teamDict[u.GetTeamId()];
            //var oldHero = t.Hero;
            u.Internal_SetHero(false);
        }

        //建立攻击关系
        public void CreateLongAttackRelation(int srcTeamIndex, int dstUnitIndex)
        {
            var dstUnit = _unitDict[dstUnitIndex];
            dstUnit.AddLongAttackSource(_teamDict[srcTeamIndex]);
        }

        //解除攻击关系
        public void RemoveLongAttackReleation(int srcTeamIndex, int dstUnitIndex)
        {
            if (_unitDict.ContainsKey(dstUnitIndex))
            {
                var dstUnit = _unitDict[dstUnitIndex];
                dstUnit.RemoveLongAttackSource(_teamDict[srcTeamIndex]);
            }
        }

        //查询当前应该优先攻击的单位列表
        public Unit QueryAttackTarget_HighPriority(int srcTeamId, int dstTeamId)
        {
            int minAttackerCount = 99;
            Unit resultTarget = null;

            Team dstTeam = _teamDict[dstTeamId];
            Team srcTeam = _teamDict[srcTeamId];
            for (int i = 0; i < dstTeam.AllUnitList.Count; i++)
            {
                var u = dstTeam.AllUnitList[i];
                if (u.IsDead() || !u.CanTarget())
                    continue;
                var attackCount = 0;
                if (u.GetAttackSourceDict().ContainsKey(srcTeam))
                    attackCount = u.GetAttackSourceDict()[srcTeam];
                if (attackCount < minAttackerCount)
                {
                    minAttackerCount = attackCount;
                    resultTarget = u;
                }
            }
            if (resultTarget != null)
            {
                CreateLongAttackRelation(srcTeamId, resultTarget.GetId());
            }
            return resultTarget;
        }

        private List<Unit> resultUnitListForLua = new List<Unit>();
        private List<int> resultIntListForLua = new List<int>();

        //获取单位的攻击来源
        public List<Unit> Unit_GetAttackSource(Unit u)
        {
            resultUnitListForLua.Clear();
            foreach (var tu in u.WillAttackSourceSet)
            {
                resultUnitListForLua.Add(tu);
            }
            return resultUnitListForLua;
        }

        //获取队伍的攻击来源
        public List<int> Team_GetAttackSource(Team t)
        {
            resultIntListForLua.Clear();
            foreach (var i in t.WillAttackSourceTeamSet)
            {
                resultIntListForLua.Add(i.Key.Id);
            }
            return resultIntListForLua;
        }

        public int Team_GetAttackSourceCount(Team t)
        {
            return t.WillAttackSourceTeamSet.Count;
        }

        // 获取最近的team
        public int GetNearestTeam(Unit u)
        {
            int ret = -1;

            long dis = long.MaxValue;
            foreach (var kv in _teamDict)
            {
                var team = kv.Value;
                var hero = team.Hero;
                if (hero.IsDead() || hero.GetSide() == u.GetSide() || !hero.CanTarget())
                    continue;

                long tempDis = (u.WorldLogicPos - hero.WorldLogicPos).sqrMagnitude;
                if (tempDis < dis)
                {
                    dis = tempDis;
                    ret = team.Id;
                }
            }
            return ret;
        }

    }

}