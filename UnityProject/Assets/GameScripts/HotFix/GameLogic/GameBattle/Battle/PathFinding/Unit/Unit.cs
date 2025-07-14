using System;
using System.Collections.Generic;
using FixPoint;
using XLua;

//单位
namespace M.PathFinding
{
    //单位类型
    [LuaCallCSharp]
    public enum EUnitType
    {
        Soldier_Left = 0,
        Hero_Left = 1,
        Virtual_Hero_Left = 2,
        Instrument_Hero_Left = 3,

        Soldier_Right = 4,
        Hero_Right = 5,
        Virtual_Hero_Right = 6,
        Instrument_Hero_Riht = 7,

        Count,

        All = 999,
    }

    //地图层
    public enum ELayerType
    {
        Low, //洼地
        Normal, //中间障碍
        Height, //高度障碍
        Count
    }

    public enum UnitShareIndex
    {
        PosX = 1,
        PosY = 2,
        DirX = 3,
        DirY = 4,

        RelativeHeroX = 5,
        RelativeHeroY = 6,

        CurrRad = 7,
        Speed = 8,
        IsMoving = 9,

        State = 10,
        CanTarget = 11,
        CanSelect = 12,
        ReachTargetSt = 13,

        //动态攻击范围倍率, 默认1
        DynamicAttackRangeRate = 14,

        //是否开启攻击范围倍率
        IsOpenDynamicAttackRange = 15,

        // 是否寻路被阻塞
        IsPathFindingBlocked = 16,

        // 要攻击的目标单位ID
        WillAttackTargetId = 17,
        DashStartJump = 20,
        IsDashEnd = 21,
        TryScaleAtkRange = 22,
        NearestTargetID = 23,


        All = 50,
    }

    public enum PathFindingBlockType
    {
        // 无障碍
        None = 0,

        // 找不到路径
        NoPath = 1,

        // 寻路过程被堵住
        Blocked = 2,
    }

    public enum ETurnDirection
    {
        None,
        Left,
        Right
    }

    [LuaCallCSharp]
    public partial class Unit
    {
        //地图
        private BattleMap _map = null;

        private EUnitType _unitType;

        private static readonly int[,] Extend_Modifier_Array2D = new int[(int)EUnitType.Count, (int)EUnitType.Count]
        {
            //Soldier_Left 眼里，其它单位的大小修正
            { 0, 0, -99, 0, 0, 0, -99, 0 },
            //Hero_Left 眼里，其它单位的大小修正
            { -99, 0, -99, 0, -99, 0, -99, 0 },
            //Virtual_Hero_Left 眼里，其它单位的大小修正
            { -99, -99, 0, -99, -99, -99, 0, -99 },
            //Instrument_Hero_Left 眼里，其它单位的大小修正
            { -99, 0, -99, 0, -99, 0, -99, 0 },
            //Soldier_Right 眼里，其它单位的大小修正
            { 0, 0, -99, 0, 0, 0, -99, 0 },
            // Hero_Right 眼里，其它单位的大小修正
            { -99, 0, -99, 0, -99, 0, -99, 0 },
            // Virtual_Hero_Right 眼里，其它单位的大小修正
            { -99, -99, 0, -99, -99, -99, 0, -99 },
            //Instrument_Hero_Riht 眼里，其它单位的大小修正
            { -99, 0, -99, 0, -99, 0, -99, 0 },
        };

        //TODO:这个值要走单位的配表走
        private FixInt _greenCircleRadius = 10000;
        private FixInt _greenCircleRadiusSqr = 10000 * 10000;

        private readonly int minTurnAngleLen = 15000; //1 *fixscale

        //地图坐标，与世界坐标保持一致
        private Integer2 _gridPos = new Integer2(0, 0);

        private void SetGridPos(Integer2 pos)
        {
            _gridPos = pos;
            WorldLogicPos = _map.GetGridCenter(pos.x, pos.y);
        }

        public bool _isMelee;

        //是否已经死亡
        private bool _isDead;

        //射程
        public int _attackRange = 4;

        //自己的ID
        private int _id; //复活会改变Id

        //是否是英雄
        private bool _isHero;

        //Team
        private int _teamId;

        //敌我双边
        private int _side;

        //半径
        private int _radius = 0;
        private long _sqrRadius = 0;

        private int _realExtend = 0;

        //是否是虚拟单位（仅对武将有效）
        private bool _isVirtual = false;

        //是否是器械单位（仅对武将有效）
        private bool _isInstrument = false;

        private FixInt2 _worldPos;

        private bool _canMove = false;

        //世界坐标
        public FixInt2 WorldPos
        {
            get { return _worldPos; }
            set
            {
                _worldPos = value;
                LuaArray.SetInt((int)UnitShareIndex.PosX, _worldPos.x);
                LuaArray.SetInt((int)UnitShareIndex.PosY, _worldPos.y);
            }
        }

        private FixInt2 _worldLogicPos;

        public FixInt2 WorldLogicPos
        {
            get { return _worldLogicPos; }
            set { _worldLogicPos = value; }
        }

        //上一帧的世界坐标
        public FixInt2 WorldPosLastFrame { get; set; }
        private bool _isMoving = false;

        public bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                _isMoving = value;
                LuaArray.SetInt((int)UnitShareIndex.IsMoving, _isMoving ? 1 : 0);
            }
        }

        //是否是近战
        public bool IsMelee()
        {
            return _isMelee;
        }

        //朝向
        private FixInt2 _faceDirNormalized = FixInt2.zero;

        public FixInt2 FaceDirNormalized
        {
            get { return _faceDirNormalized; }
            set
            {
                _faceDirNormalized = value;
                LuaArray.SetInt((int)UnitShareIndex.DirX, _faceDirNormalized.x);
                LuaArray.SetInt((int)UnitShareIndex.DirY, _faceDirNormalized.y);
            }
        }

        private FixInt _currFaceRad = new FixInt(0);

        public FixInt CurrFaceRad
        {
            get { return _currFaceRad; }
            set
            {
                _currFaceRad = value;
                LuaArray.SetInt((int)UnitShareIndex.CurrRad, _currFaceRad.Value);
            }
        }

        //相对英雄的位置（小兵用）
        private FixInt2 _posRelativeToHero = new FixInt2();

        public FixInt2 PosRelativeToHero
        {
            get { return _posRelativeToHero; }
            set
            {
                _posRelativeToHero = value;
                LuaArray.SetInt((int)UnitShareIndex.RelativeHeroX, _posRelativeToHero.x);
                LuaArray.SetInt((int)UnitShareIndex.RelativeHeroY, _posRelativeToHero.y);
            }
        }

        //攻击来源统计，来自每个Team的攻击源计数
        private readonly Dictionary<Team, int> _attackSourceDict = new Dictionary<Team, int>();
        private int _meleeAttackSource = 0;

        public Team Team { get; set; } = null;

        public LuaArrAccess LuaArray { get; set; }

        public ETurnDirection TurnDirection { get; set; }

        public int MeleeAttackSourceStandNum { get; private set; } = 0;
        public int LongRangeAttackSourceStandNum { get; private set; } = 0;

        //优先攻击目标
        private List<Unit> _priorityTarget = new List<Unit>();

        public Unit()
        {
            PathState = new PathState(this);

            InitState();
        }

        public void InitWithData(BattleMap map, FixInt2 worldPos,
            int id, int teamId, int side, bool isHero, bool isVirtual, bool isInstrument, bool isMelee, int speed,
            int attackRange, int radius,
            int extend, int greenCircleRadius, int meleeStandNum, int rangeStandNum, LuaArrAccess luaArray)
        {
            _map = map;

            LuaArray = luaArray;
            WorldPos = map.ClampWorldPosInMap(worldPos);
            _id = id;
            _teamId = teamId;
            _side = side;
            _isHero = isHero;
            SetSpeed(speed);
            _isDead = false;

            _isVirtual = isVirtual;
            _isMelee = isMelee;
            _isInstrument = isInstrument;

            _greenCircleRadius = greenCircleRadius;
            _greenCircleRadiusSqr = _greenCircleRadius * _greenCircleRadius;

            MeleeAttackSourceStandNum = meleeStandNum;
            LongRangeAttackSourceStandNum = rangeStandNum;

            SetAttackRange(attackRange);

            if (side == 1)
            {
                if (isHero)
                {
                    if (_isVirtual)
                        _unitType = EUnitType.Virtual_Hero_Left;
                    else
                        _unitType = EUnitType.Hero_Left;
                }
                else
                {
                    _unitType = EUnitType.Soldier_Left;
                }
            }
            else
            {
                if (isHero)
                {
                    if (_isVirtual)
                        _unitType = EUnitType.Virtual_Hero_Right;
                    else
                        _unitType = EUnitType.Hero_Right;
                }
                else
                    _unitType = EUnitType.Soldier_Right;
            }

            SetGridPos(_map.WorldPosToGrid(WorldPos));
            if (!(_gridPos.x >= 0 && _gridPos.x < GetMap().GetWidth()) ||
                !(_gridPos.y >= 0 && _gridPos.y < GetMap().GetHeight()))
            {
                Utils.LogWarning(WorldPos.ToString(), _gridPos);
            }

            //Utils.Assert(_gridPos.x >= 0 && _gridPos.x < GetMap().GetWidth());
            //Utils.Assert(_gridPos.y >= 0 && _gridPos.y < GetMap().GetHeight());

            _radius = radius;
            _sqrRadius = radius * radius;
            _realExtend = extend;
            _canMove = true;
        }

        public void Reset()
        {
            _priorityTarget.Clear();
            ChangeState_StopVoid();
            if (!_isDead)
            {
                //清空自己想攻击的目标
                ClearWillAttackTarget();
                CleanPriorityTarget();
                //清空正在攻击自己的单位的攻击目标
                _tempUnitList.Clear();
                foreach (var u in WillAttackSourceSet)
                    _tempUnitList.Add(u);
                foreach (var u in _tempUnitList)
                    u.ClearWillAttackTarget();
                // 清除攻击来源信息
                _attackSourceDict.Clear();

                //从BSP树里面移除
                MyBspTree.RemoveUnit(this);
                _isDead = true;
            }

            // 重置个状态
            _stateStop.Reset();
            _stateSearchEnemy.Reset();
            // _stateSlipTo.Reset();
            // _stateForceSlip.Reset();
            _stateStopVoid.Reset();
            _stateMoveStraightTo.Reset();
            _stateJumpTo.Reset();
            _stateFollowHero.Reset();
            _stateAttack.Reset();

            _stateVirtualHeroMovePathTo.Reset();
            _stateSoldierFollowVirtualHero.Reset();
            _stateSoldierSearchEnemy.Reset();

            _stateMoveWithTurnAngle.Reset();
            _stateHeroMovePathTo.Reset();
            _stateHeroMoveStraightTo.Reset();
            _stateHeroSearchEnemy.Reset();
            _stateHorseFollowVirtualHero.Reset();
            _stateDashMove.Reset();
            // _stateCoerced.Reset();
            // _stateAttackFortifiction.Reset();
            // _stateHeroOrVirtualGuard.Reset();
            _stateMoveTo.Reset();
            // _stateAppointTarget.Reset();
            // _stateCharmSearch.Reset();
            // _stateHeroOrVirtualVigilance.Reset();
            // _stateGuardWallAttck.Reset();
            _stateVirtualHeroAssignTarget.Reset();
            _stateWaitVirtualHeroAssignTarget.Reset();
            _stateMoveToTarget.Reset();
            WorldPosLastFrame = FixInt2.zero;
            IsMoving = false;
            _faceDirNormalized = FixInt2.zero;
            _currFaceRad.Value = 0;
            _posRelativeToHero = FixInt2.zero;
            _attackSourceDict.Clear();
            _meleeAttackSource = 0;
            TurnDirection = ETurnDirection.None;

            _isOccupyGrid = false;
            _occupiedGrid.x = -1;
            _occupiedGrid.y = -1;
            _canMove = false;

            PathState.Reset();

            LuaArray = null;
            EnemyBspTree = null;
            MyBspTree = null;
            Team = null;
            _map = null;
        }

        public BattleMap GetMap()
        {
            return _map;
        }

        public int GetRealExtend()
        {
            // if (_unitType == EUnitType.Hero_Left || _unitType == EUnitType.Hero_Right)
            //     return GetMap().GetMapConfig().ExtendHero;
            // else
            //     return GetMap().GetMapConfig().ExtendSoldier;;
            return _realExtend;
        }

        public FixInt2 GetWorldPos()
        {
            return _worldPos;
        }

        public int GetWorldPosX()
        {
            return _worldPos.x;
        }

        public int GetWorldPosY()
        {
            return _worldPos.y;
        }

        public Integer2 GetGridPos()
        {
            return _gridPos;
        }

        public int GetId()
        {
            return _id;
        }

        public bool IsHero()
        {
            return _isHero;
        }

        public int GetTeamId()
        {
            return _teamId;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        readonly private List<Unit> _tempUnitList = new List<Unit>(20);

        public void SetDead(bool isDead)
        {
            if (isDead == _isDead)
                return;

            _isDead = isDead;
            //死亡后的清理
            if (_isDead)
            {
                //确保已经处在StopVoid状态
                //Utils.Assert(GetCurrentStateObject() == _stateStopVoid, "SetDead之前，单位必须处在StopVoid状态");

                //清空自己想攻击的目标
                ClearWillAttackTarget();
                CleanPriorityTarget();
                //清空正在攻击自己的单位的攻击目标
                _tempUnitList.Clear();
                foreach (var u in WillAttackSourceSet)
                    _tempUnitList.Add(u);
                foreach (var u in _tempUnitList)
                    u.ClearWillAttackTarget();
                // 清除攻击来源信息
                _attackSourceDict.Clear();

                //从BSP树里面移除
                MyBspTree.RemoveUnit(this);
            }
        }

        public int GetSide()
        {
            return _side;
        }

        public void SetAttackRange(int r)
        {
            _attackRange = r;
        }

        /// <summary>
        /// 设置动态攻击范围倍率
        /// </summary>
        /// <param name="isOpen"></param>
        public void SetAttackRangeRateOpen(bool isOpen)
        {
            LuaArray.SetInt((int)UnitShareIndex.IsOpenDynamicAttackRange, isOpen ? 1 : 0);
        }

        public bool IsOpenDynamicAttackRange()
        {
            var open = LuaArray.GetInt((int)UnitShareIndex.IsOpenDynamicAttackRange) == 1;
            var rateOk = LuaArray.GetInt((int)UnitShareIndex.DynamicAttackRangeRate) > 0;
            return open && rateOk;
        }

        public int GetAttackRangeRate()
        {
            return LuaArray.GetInt((int)UnitShareIndex.DynamicAttackRangeRate);
        }

        public int GetAttackRange()
        {
            if (LuaArray.GetInt((int)UnitShareIndex.IsOpenDynamicAttackRange) == 1)
                return _attackRange * GetAttackRangeRate();
            return _attackRange;
        }

        public int GetPathFindingBlocked()
        {
            return LuaArray.GetInt((int)UnitShareIndex.IsPathFindingBlocked);
        }

        public void SetPathFindingBlocked(int blockNum)
        {
            LuaArray.SetInt((int)UnitShareIndex.IsPathFindingBlocked, blockNum);
        }

        private void _SetWillAttackUnitId(int unitId)
        {
            LuaArray.SetInt((int)UnitShareIndex.WillAttackTargetId, unitId);
        }

        public void SetPosRelativeToHero(FixInt2 v)
        {
            PosRelativeToHero = v;
        }

        public FixInt2 GetPosRelativeToHero()
        {
            return _posRelativeToHero;
        }

        // public FixInt2 GetFaceDir() { return FaceDirNormalized; }
        public int GetRadius()
        {
            return _radius;
        }

        public long GetSqrRadius()
        {
            return _sqrRadius;
        }

        public FixInt GetGreenCircleRadius()
        {
            return _greenCircleRadius;
        }

        public FixInt GetGreenCircleRadiusSqr()
        {
            return _greenCircleRadiusSqr;
        }

        //public void GetFaceDirXY(out double x, out double y)
        //{
        //    x = FaceDirNormalized.x;
        //    y = FaceDirNormalized.y;
        //}

        public bool IsVirtual()
        {
            return _isVirtual;
        }

        public void SetVirtual(bool isVirtual)
        {
            _isVirtual = isVirtual;
        }

        public EUnitType GetUnitType()
        {
            return _unitType;
        }

        //目前单位位于BSP树的哪个节点节点。该属性仅供BSPTree树使用
        public BSPTreeNode _BSPTreeNode { get; set; } = null;

        //手工设置朝向。在Move和Search状态下Tick中会自动设置
        //public void SetFaceDir(FixInt2 dir)
        //{
        //    Utils.Assert(dir.sqrMagnitude > 0);
        //    //dir = Vector2.Normalize(dir);
        //    dir.Normalize();
        //    FaceDirNormalized = dir;
        //}
        //public void SetFaceDirXY(int x, int y)
        //{
        //    Utils.Assert(x != 0 || y != 0);

        //    FixInt2 dir = new FixInt2(x, y);
        //    dir.Normalize();
        //    FaceDirNormalized = dir;
        //}

        public void SetFaceDirToTarget(int x, int y)
        {
            FixInt2 target = new FixInt2(x, y);
            SetFaceDirToTargetVec2(target);
        }

        public void SetFaceDirToTargetVec2(FixInt2 target)
        {
            if (target == WorldPos)
                return;
            FixInt2 toTarget = target - WorldPos;
            toTarget.Normalize();
            FaceDirNormalized = toTarget;
        }

        //每逻辑帧更新
        public void Tick()
        {
            if (!_canMove)
                return;


            var oldGridPos = _gridPos;

            PathState.TickPathLife();
            //UnityEngine.Profiling.Profiler.BeginSample("unit_Tick");
            TickState();
            //UnityEngine.Profiling.Profiler.EndSample();

            //  if (IsHero() && !IsDead())
            //  {
            //      Internal_UpdateSoldierPositionNearHero();
            //  }

            //Clamp到战场内
            var pos = ClampPos(WorldPos);
            if (pos != WorldPos)
                WorldPos = pos;

            //刷新当前是否在移动
            IsMoving = (WorldPos != WorldPosLastFrame);
            WorldPosLastFrame = WorldPos;

            //格子发生改变了
            if (_gridPos != oldGridPos)
            {
                if (!IsDead())
                {
                    MyBspTree.UpdateUnitPosition(this);
                }
            }
        }

        //设置速度，可以每帧调用
        public void SetSpeed(int speed)
        {
            LuaArray.SetInt((int)UnitShareIndex.Speed, speed);
        }

        public FixInt2 ClampPos(FixInt2 pos)
        {
            //Clamp到战场内
            pos.x = Math.Max(_map.MinWorldPosX, pos.x);
            pos.x = Math.Min(_map.MaxWorldPosX, pos.x);
            pos.y = Math.Max(_map.MinWorldPosY, pos.y);
            pos.y = Math.Min(_map.MaxWorldPosY, pos.y);
            return pos;
        }


        public int GetSpeed()
        {
            return LuaArray.GetInt((int)UnitShareIndex.Speed);
        }

        public bool CanTarget()
        {
            return LuaArray.GetInt((int)UnitShareIndex.CanTarget) == 1;
        }

        public bool CanSelect()
        {
            return LuaArray.GetInt((int)UnitShareIndex.CanSelect) == 1;
        }

        public bool IsMoveStraightReachTarget()
        {
            ISetMoveTargetPosition s = _curState as ISetMoveTargetPosition;
            if (s != null)
                //Utils.Assert(_curState == _stateMoveStraightTo, GetCurrentState().ToString());
                return s.IsReachTarget();
            return false;
        }

        public int GetReachTargetSt()
        {
            ISetMoveTargetPosition s = _curState as ISetMoveTargetPosition;
            if (s != null)
                //Utils.Assert(_curState == _stateMoveStraightTo, GetCurrentState().ToString());
                return (int)s.GetReachTargetSt();
            return (int)EReachTargetSt.None;
        }

        /// <summary>
        /// 跳跃状态是否到达终点
        /// </summary>
        /// <returns></returns>
        public bool IsJumpReachTarget()
        {
            //Utils.Assert(_curState == _stateJumpTo, GetCurrentState().ToString());
            return _stateJumpTo.IsJumpReachTarget();
        }

        /// <summary>
        /// 获取跳跃目标点
        /// </summary>
        /// <returns></returns>
        public FixInt2 GetJumpTarget()
        {
            //Utils.Assert(_curState == _stateJumpTo, GetCurrentState().ToString());
            return _stateJumpTo.GetJumpTarget();
        }

        /// <summary>
        /// 是否已经滑到了目标点
        /// </summary>
        /// <returns></returns>
        // public bool IsSlipReachTarget()
        // {
        //     //Utils.Assert(_curState == _stateSlipTo, GetCurrentState().ToString());
        //     return _stateSlipTo.IsReachTarget();
        // }

        // public bool IsForceSlipReachTarget()
        // {
        //     //Utils.Assert(_curState == _stateForceSlip, GetCurrentState().ToString());
        //     return _stateForceSlip.IsReachTarget();
        // }

        /// <summary>
        /// 是否在滑动中被别人挡住（障碍物、别的单位等）
        /// </summary>
        /// <returns></returns>
        // public bool IsSlipBlockByOther()
        // {
        //     //Utils.Assert(_curState == _stateSlipTo, GetCurrentState().ToString());
        //     return _stateSlipTo.IsSlipBlockByOther();
        // }

        // public bool IsForceSlipBlockByOther()
        // {
        //     //Utils.Assert(_curState == _stateForceSlip, GetCurrentState().ToString());
        //     return _stateForceSlip.IsSlipBlockByOther();
        // }
        public void Internal_SetHero(bool isHero)
        {
            _isHero = isHero;
        } //供Map使用，外部别用

        //让小兵站在格子中心，效果可能更好
        public void StandAtGridCenter()
        {
            var center = _map.GetGridCenter(_gridPos.x, _gridPos.y);
            WorldPos = center;
        }

        //打印Unit的Log
        public void Log(string format, params object[] args)
        {
            if (Utils.IsDebug()) // && Utils.IsLogUnit(_id))
                Utils.Log(
                    $"#BattleUnit# [{_id.ToString()}] [{WorldPos.x.ToString()}, {WorldPos.y.ToString()}]" + format,
                    args);
        }

        public Dictionary<Team, int> GetAttackSourceDict()
        {
            return _attackSourceDict;
        }

        //增加攻击来源
        public void AddLongAttackSource(Team t)
        {
            if (_attackSourceDict.ContainsKey(t))
                _attackSourceDict[t]++;
            else
                _attackSourceDict[t] = 1;
        }

        //减少攻击来源
        public void RemoveLongAttackSource(Team t)
        {
            //Utils.Assert(_attackSourceDict.ContainsKey(t) && _attackSourceDict[t] >= 1);
            if (_attackSourceDict.ContainsKey(t) && _attackSourceDict[t] >= 1)
                _attackSourceDict[t]--;
        }

        //增加近战来源
        public void AddMeleeAttackSource()
        {
            _meleeAttackSource++;
        }

        //减少近战来源
        public void RemoveMeleeAttackSource()
        {
            _meleeAttackSource--;
            //Utils.Assert(_meleeAttackSource >= 0);
            if (_meleeAttackSource < 0)
                _meleeAttackSource = 0;
        }

        //获取近战来源数量
        public int GetMeleeAttackSourceCount()
        {
            return _meleeAttackSource;
        }

        //根据攻击来源数量调整的射程大小
        public int GetTargetAttackRangeAddByHisSourceCount(Unit target)
        {
            // if (!IsHero() && !target.IsHero())
            // {
            //     var count = target.GetMeleeAttackSourceCount();
            //     if (count >= 5)
            //         return 1.0f;
            // }

            return 0;
        }

        //攻击目标是否在射程内
        public bool IsAttackTargetInMyRange(Unit target, out long outDis)
        {
            var dis = GetAttackDistanceToTarget(target); //- GetTargetAttackRangeAddByHisSourceCount(target);
            var attRange = GetAttackRange();
            outDis = dis;

            //如果格子的在正中间，认为是调整过占位的，增加射程
            //700 = 1400/2
            //if (WorldPos.x % 700 == 0 && WorldPos.y % 700 == 0)
            //全部采用攻击格子中心计算
            //attRange += 990;

            return attRange > dis;
        }

        public bool IsAttackTargetInMyRange(Unit target)
        {
            long outDis;
            return IsAttackTargetInMyRange(target, out outDis);
        }

        // 攻击点是否在射程之内
        public bool IsAttackNodeInMyRange(Node targetNode)
        {
            var dis = GetAttackDistanceToNode(targetNode);
            var attRange = GetAttackRange();

            //如果格子的在正中间，认为是调整过占位的，增加射程
            //700 = 1400/2
            //if (WorldPos.x % 700 == 0 && WorldPos.y % 700 == 0)
            //attRange += 990;

            return attRange > dis;
        }

        public int GetAttackDistanceToNode(Node n)
        {
            var disBetweenCenter = (WorldLogicPos - n.GetCenterWorldPos()).magnitude;
            var tr = GetMap().GetGridSize() / 2;
            var r = GetRadius();
            return disBetweenCenter - tr - r;
        }

        //获取两者间的攻击距离。中心距离减去双方半径
        public int GetAttackDistanceToTarget(Unit u)
        {
            var disBetweenCenter = (WorldLogicPos - u.WorldLogicPos).magnitude;
            var tr = u.GetRadius();
            var r = GetRadius();
            return disBetweenCenter - tr - r;
        }

        //根据targetidx 获取两者间的攻击距离。中心距离减去双方半径
        public int GetAttackDistanceToTargetByIdx(int targetUnitIndex)
        {
            var unitDict = this.GetMap().GetUnitDict();
            var tarToUnit = unitDict[targetUnitIndex];
            return GetAttackDistanceToTarget(tarToUnit);
        }

        //我是否在虚拟英雄的绿圈内。我是小兵
        public bool IsMeInVirtualHeroGreenCircle()
        {
            var myPos = WorldPos;
            var heroUnit = _map.GetTeam(_teamId).Hero;
            var virtualHeroPos = heroUnit.WorldPos;
            long disSqr = (myPos - virtualHeroPos).sqrMagnitude;
            var circleRadiusSqr = heroUnit.GetGreenCircleRadiusSqr().Value;
            return disSqr < circleRadiusSqr;
        }

        //世界坐标是否在我的绿圈范围内，我是英雄
        public bool IsPositionInMyGreenCircle(FixInt2 pos)
        {
            var virtualHeroPos = WorldPos;
            long disSqr = (pos - virtualHeroPos).sqrMagnitude;
            return disSqr < GetGreenCircleRadiusSqr().Value;
        }

        //世界坐标是否在我的英雄的绿圈范围内，我是小兵
        public bool IsPositionInHeroGreenCircle(FixInt2 pos)
        {
            var heroUnit = _map.GetTeam(_teamId).Hero;
            var virtualHeroPos = heroUnit.WorldPos;
            long disSqr = (pos - virtualHeroPos).sqrMagnitude;
            return disSqr < heroUnit.GetGreenCircleRadiusSqr().Value;
        }

        bool turnRight = false;

        //每帧改变方向的移动
        public FixInt2 CalculateMovingTargetWithTurnAngleSpeed(FixFraction turnAngleSpeedRadianPerSecend,
            FixInt2 posTarget,
            ref FixFraction refMovingDirInRadian)
        {
            var vecToTarget = posTarget - this.WorldPos;
            var len = vecToTarget.magnitude;
            var scale = ComputeMoveDisInOneFrame();
            if (len < 100) //0.1*1000
            {
                return posTarget;
            }
            // 由于速度可能很快，但是在转弯需要做个限制，不然会转出地图
            //todo


            var vecCurrentDir1000 = new FixInt2(
                (int)FixMath.Cos(refMovingDirInRadian.nominal, refMovingDirInRadian.denominal).nominal,
                (int)FixMath.Sin(refMovingDirInRadian.nominal, refMovingDirInRadian.denominal).nominal) / 10;

            var vecToTargetN = vecToTarget;
            vecToTargetN.Normalize();
            //long radianOffset10000 = FixMath.Acos(FixInt2.DotLong(vecCurrentDir1000, vecToTargetN) * 10, 10000L).nominal;
            long radianOffset10000 = FixInt2.RadianInt(vecCurrentDir1000, vecToTargetN).nominal;

            long radianChangePerFrame10000 =
                turnAngleSpeedRadianPerSecend.nominal * this.GetMap().GetFrameDeltaMs() / 1000;

            bool isFaceToTargetDirectly = false;
            if (radianOffset10000 <= radianChangePerFrame10000)
            {
                vecCurrentDir1000 = vecToTarget;
                refMovingDirInRadian = FixMath.Atan2(vecCurrentDir1000.y, vecCurrentDir1000.x);

                //Utils.Log("_movingDirInRadian 1: {0}, {1}", refMovingDirInRadian.nominal, refMovingDirInRadian.denominal);

                isFaceToTargetDirectly = true;

                //TurnDirection = ETurnDirection.None;
            }
            else
            {
                long crossProduct = FixInt2.CrossLong(vecCurrentDir1000, vecToTargetN);
                bool turnLeft = crossProduct > 0;

                long offSetVirtual10000 = radianChangePerFrame10000;

                if (turnLeft)
                {
                    refMovingDirInRadian += new FixFraction(offSetVirtual10000, 10000L);
                    turnRight = false;

                    TurnDirection = ETurnDirection.Left;
                }
                else
                {
                    refMovingDirInRadian -= new FixFraction(offSetVirtual10000, 10000L);
                    turnRight = true;

                    TurnDirection = ETurnDirection.Right;
                }
                //Utils.Log("_movingDirInRadian 2: {0}, {1}", refMovingDirInRadian.nominal, refMovingDirInRadian.denominal);

                vecCurrentDir1000 = new FixInt2(
                    (int)FixMath.Cos(refMovingDirInRadian.nominal, refMovingDirInRadian.denominal).nominal,
                    (int)FixMath.Sin(refMovingDirInRadian.nominal, refMovingDirInRadian.denominal).nominal) / 10;
            }

            var posCurrentTarget = this.WorldPos + vecCurrentDir1000 * scale / 1000;
            if (isFaceToTargetDirectly)
                posCurrentTarget = posTarget;
            // 进行合法性判断

            return ClampPos(posCurrentTarget);
        }

        public void ReviveUnit(int newId, out int x, out int y)
        {
            x = 0;
            y = 0;
            if (IsHero())
            {
                if (!IsVirtual())
                {
                }
            }
            else
            {
                //由于复活id是变更了，需要做索引处理
                GetMap().GetUnitDict().Remove(this._id);
                this._id = newId;
                GetMap().GetUnitDict().Add(newId, this);
                var rand = this.GetMap().GetBattleRandom();
                var dirX = rand.Next(1, 10) % 2 == 0 ? 1 : -1;
                var dirY = rand.Next(1, 10) % 2 == 0 ? 1 : -1;
                // 找到可以站下的位置
                var newPos = this.Team.Hero.WorldPos +
                             new FixInt2(rand.Next(200, 1400) * dirX, rand.Next(200, 800) * dirY);
                SetGridPos(_map.WorldPosToGrid(newPos));
                WorldPos = newPos;
                InternalEnableOccupy_Stand();
                MyBspTree.AddUnit(this);
                _isDead = false;
                x = WorldPos.x;
                y = WorldPos.y;
            }
        }

        public void ReviveVirtualHero(out int x, out int y)
        {
            x = 0;
            y = 0;
            if (!IsVirtual())
                return;

            InternalEnableOccupy_Stand();
            MyBspTree.AddUnit(this);
            _isDead = false;
            x = WorldPos.x;
            y = WorldPos.y;
        }

        public FixInt2 GetFacePos()
        {
            return this.GetWorldPos() + new FixInt2(FaceDirNormalized.x,
                FaceDirNormalized.y);
        }

        public void ChangeTarget(int newIdx)
        {
            var newTar = GetMap().GetUnit(newIdx);
            SetWillAttackTarget(newTar);
        }

        public void AddFrontPriorityTarget(int newIdx)
        {
            _priorityTarget.Insert(0, GetMap().GetUnit(newIdx));
        }

        public void AppendPriorityTarget(int newIdx)
        {
            _priorityTarget.Add(GetMap().GetUnit(newIdx));
        }

        public void AppendAndCheckPriorityTarget(int newIdx)
        {
            var t = GetMap().GetUnit(newIdx);
            if (!_priorityTarget.Contains(t))
            {
                _priorityTarget.Add(t);
            }
        }

        public Unit DequeuePriorityTarget()
        {
            if (_priorityTarget.Count <= 0)
            {
                return null;
            }

            do
            {
                var ret = _priorityTarget[0];
                _priorityTarget.RemoveAt(0);
                if (!ret.IsDead())
                {
                    return ret;
                }
            } while (_priorityTarget.Count > 0);

            return null;
        }

        public Unit PeekPriorityTarget()
        {
            if (_priorityTarget.Count <= 0)
            {
                return null;
            }

            do
            {
                var ret = _priorityTarget[0];
                if (!ret.IsDead() && ret.CanTarget())
                {
                    return ret;
                }
                else
                {
                    _priorityTarget.RemoveAt(0);
                }
            } while (_priorityTarget.Count > 0);

            return null;
        }

        public void RemovePriorityTargetById(int targetId)
        {
            foreach (var u in _priorityTarget)
            {
                if (u.GetId() == targetId)
                {
                    _priorityTarget.Remove(u);
                    break;
                }
            }
        }

        public void CleanPriorityTarget()
        {
            _priorityTarget.Clear();
        }

        public bool CanMove
        {
            get { return _canMove; }
            set { _canMove = value; }
        }

        /// <summary>
        /// 根据当前队伍的阵型 计算其半径
        /// 其实是找一个离中心点最远的
        /// </summary>
        /// <returns></returns>
        public int GetTeamRadius()
        {
            Team team = GetMap().GetTeam(GetTeamId());
            int radius = 0;
            foreach (var u in team.AllUnitList)
            {
                if (u.IsVirtual() || u.IsDead())
                {
                    continue;
                }

                var disBetweenCenter = (WorldLogicPos - u.WorldLogicPos).magnitude;
                if (disBetweenCenter > radius)
                {
                    radius = disBetweenCenter;
                }
            }

            return radius;
        }


        public List<FixInt2> GetSurroundPositions(int centerX, int centerY, int radius, int angleStart, int angle,
            int minNum)
        {
            List<FixInt2> ret = new List<FixInt2>();
            Team team = GetMap().GetTeam(GetTeamId());
            int num = 0;
            foreach (var u in team.AllUnitList)
            {
                if (u.IsVirtual() || u.IsDead())
                {
                    continue;
                }

                num += 1;
            }

            double angleRad = angle * Math.PI / 180.0;
            double minAngleRad = angleRad / minNum;
            double startAngleRad = angleStart * Math.PI / 180.0;
            int i = 0;
            int count = 0;
            while (count < num)
            {
                // 计算每个小兵的角度
                double theta = startAngleRad + i * minAngleRad;

                // 将极坐标转换为笛卡尔坐标
                double x = radius * Math.Cos(theta) + centerX;
                double y = radius * Math.Sin(theta) + centerY;
                i++;
                count++;

                FixInt2 pos = new FixInt2((int)Math.Round(x), (int)Math.Round(y));
                ret.Add(pos);
                if (i == minNum - 1)
                {
                    radius += GetRadius() * 3;
                    minNum += 3;
                    minAngleRad = angleRad / minNum;
                    i = 0;
                }
            }

            return ret;
        }

        //找到距离位置最近的单兵ID
        public int GetNearestUnitId(int x, int y)
        {
            FixInt2 pos = new FixInt2(x, y);
            Team team = GetMap().GetTeam(GetTeamId());
            double minDis = 9999999;
            int id = 0;
            foreach (var u in team.AllUnitList)
            {
                if (u.IsVirtual() || u.IsDead())
                {
                    continue;
                }

                double dis = (u._worldPos - pos).magnitude;
                if (dis < minDis)
                {
                    id = u.GetId();
                    minDis = dis;
                }
            }
            return id;
        }
    }
}