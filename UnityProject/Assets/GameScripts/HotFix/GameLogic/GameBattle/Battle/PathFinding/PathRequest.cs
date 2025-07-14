using FixPoint;
using Priority_Queue;

/// <summary>
/// 路径请求类。由Unit发给BattleMap
/// </summary>
namespace M.PathFinding
{
    public enum EPathRequestType
    {
        ToOneUnit,
        ToTeam,
        ToPosition,
        ToFortificationUnit,
    }

    public enum ERequestPriority
    {
        High = 0,
        Medium,
        Low,
        MaxNum
    }

    public class PathRequest
    {
        private Unit _startUnit;
        private EPathRequestType _type;
        private int _stopDistance; //终止距离
        //以下两个4选一
        private Unit _targetUnit;
        private int _targetTeamId;
        private FixInt2 _targetPos;

        private int _uid;
        private int _maxNodeTestNum = 0;

        private ERequestPriority _priority = 0;
        private BattleEntityPoolManager _mgr;

        public PathRequest(BattleEntityPoolManager mgr)
        {
            _mgr = mgr;
        }

        //初始单位
        public Unit GetStartUnit() { return _startUnit; }
        //请求类型
        public EPathRequestType GetRequestType() { return _type; }
        //目标单位
        public Unit GetTargetUnit() { return _targetUnit; }
        //目标队伍
        public int GetTargetTeamId() { return _targetTeamId; }
        //目标位置
        public FixInt2 GetTargetPos() { return _targetPos; }
        //停止距离
        public int GetStopDis() { return _stopDistance; }
        public ERequestPriority GetPriority() { return _priority; }

        public int GetMaxNodeTestNum() { return _maxNodeTestNum; }

        public int GetUID() { return _uid; }

        private int GenerateUID()
        {
            return _mgr.GeneratePathRequestUID();
        }

        public void Init_oneTarget(Unit startUnit, Unit targetUnit, int stopDis, int maxNodeTest, ERequestPriority priority)
        {
            _type = EPathRequestType.ToOneUnit;
            _stopDistance = stopDis;
            _startUnit = startUnit;
            _targetUnit = targetUnit;
            _stopDistance = stopDis;
            _targetTeamId = -1;
            _targetPos = FixInt2.zero;
            _priority = priority;
            _maxNodeTestNum = maxNodeTest;
            _uid = GenerateUID();
        }

        public void Init_team(Unit startUnit, int teamId, int stopDis, int maxNodeTest, ERequestPriority priority)
        {
            _type = EPathRequestType.ToTeam;
            _stopDistance = stopDis;
            _startUnit = startUnit;
            _targetTeamId = teamId;
            _stopDistance = stopDis;
            _targetUnit = null;
            _targetPos = FixInt2.zero;
            _priority = priority;
            _maxNodeTestNum = maxNodeTest;
            _uid = GenerateUID();
        }

        public void Init_targetPos(Unit startUnit, FixInt2 targetPos, int stopDis, int maxNodeTest, ERequestPriority priority)
        {
            _type = EPathRequestType.ToPosition;
            _stopDistance = stopDis;
            _startUnit = startUnit;
            _targetPos = targetPos;
            _stopDistance = stopDis;
            _targetUnit = null;
            _targetTeamId = -1;
            _priority = priority;
            _maxNodeTestNum = maxNodeTest;
            _uid = GenerateUID();
        }

    }

}