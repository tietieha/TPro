using System.Collections.Generic;
using FixPoint;
using XLua;

/// <summary>
/// 战场初始化设置，创建并赋值，传给BattleMap
/// </summary>
namespace M.PathFinding
{
    [LuaCallCSharp]
    public class UnitConfig
    {
        //public int _extend;
        public FixInt2 _worldPos;
        public int _id;
        public int _teamId;
        public int _side;
        public bool _isHero;
        public bool _isVirtual;
        public bool _isInstrument;
        public bool _isMelee;
        public int _speed;
        public int _attackRange;
        public int _radius;
        public int _extend;
        public int _greenCircleRadius;
        public int _meleeStandNum; // 近战接敌标准数
        public int _rangeStandNum; // 远程接敌标准数
        public LuaArrAccess _luaDatAccess;
        public UnitConfig(FixInt2 worldPos, int id, int teamId, int side, bool isHero, bool isVirtual, bool isInstrument, bool isMelee, int speed, int attackRange, int radius,
            int extend, int greenCircleRadius, int meleeStandNum, int rangeStandNum, LuaArrAccess luaArray)
        {
            //_extend = extend;
            _worldPos = worldPos;
            _id = id;
            _teamId = teamId;
            _side = side;
            _isHero = isHero;
            _isVirtual = isVirtual;
            _isInstrument = isInstrument;
            _isMelee = isMelee;
            _speed = speed;
            _attackRange = attackRange;
            _radius = radius;
            _extend = extend;
            _greenCircleRadius = greenCircleRadius;
            _meleeStandNum = meleeStandNum;
            _rangeStandNum = rangeStandNum;
            _luaDatAccess = luaArray;
        }
    }

    [LuaCallCSharp]
    public class BattleMapConfig
    {
        private List<int> teamIdList = new List<int>();
        private List<UnitConfig> unitList = new List<UnitConfig>();

        public int PathRequestPerFrame = 60; //20;//每帧路径请求数量
        // public int HeroMaxTestNode = 20; //英雄最大探查节点数量
        // public int SoliderMaxTestNode = 20; //小兵最大探查节点数量

        //建议英雄攻击距离是4

        //增加Team    
        public void AddTeam(int teamId)
        {
            Utils.Assert(teamIdList.FindIndex(i => i == teamId) == -1);
            teamIdList.Add(teamId);
        }

        //增加Unit
        public void AddUnit(UnitConfig unit)
        {
            Utils.Assert(unitList.FindIndex(u => u._id == unit._id) == -1);
            unitList.Add(unit);
        }

        public List<int> GetTeamIdList()
        {
            return teamIdList;
        }

        public List<UnitConfig> GetUnitList()
        {
            return unitList;
        }

        public void ClearTeamList()
        {
            teamIdList.Clear();
        }

        public void ClearUnitList()
        {
            unitList.Clear();
        }

    }
}