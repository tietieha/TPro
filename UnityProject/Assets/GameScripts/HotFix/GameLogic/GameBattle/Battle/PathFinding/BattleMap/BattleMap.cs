using System;
using System.Collections;
using System.Collections.Generic;
using FixPoint;
using XLua;

namespace M.PathFinding
{
    //调试器
    public interface IBattleMapDebugger
    {
        void ClearAll();

        //更新Node数据
        void UpdateNode(HashSet<Node> nodeSet);

        //生成Path
        void GeneratePath(int unitId, Queue<FixInt2> path);
        void RemovePath(int unitId);
        void UpdateVirtualHero();
    }

    [LuaCallCSharp]
    public partial class BattleMap
    {
        // private static readonly int PATH_REQUEST_PER_FRAME = 20;//10;
        // private static readonly int HERO_MAX_TESTED_NODE = 100;
        // private static readonly int SOLDIER_MAX_TESTED_NODE = 100;
        //private static readonly int MAX_DIS_TO_BOUND_RECT = 3 * FixInt2.Scale;

        private IBattleMapDebugger _debugger = null;

        private int _width = 146; //宽度
        private int _height = 50; //高度
        private int _gridSize = 1 * FixInt2.Scale + 400; //格子大小，正方形
        private int _halfGridSize = 700;
        private FixInt2 _originPos = FixInt2.zero; //原点位置，左下角
        private int _frameDeltaMs = 100; //帧间隔，毫秒

        private int _maxPathPosDis = 100;
        private Node[,] _grid = null; //格子

        private readonly HashSet<Node> _changedNodeInFrame = new HashSet<Node>(); //当前帧更新的Node数量
        private bool _enableRecordNodeChange = false;

        //NOTE: Lua在做攻击来源查询的时候，不要持有并缓存下列函数的返回值对象（即这个List<int>）。
        // 因为下面所有的函数都会把结果放在outUnitIdList中然后返回之（为了减少GC内存分配）
        // 持有这个List的话下一次查询后结果就会被覆盖了。
        // 获取攻击来源的List后，请立刻遍历List、把里面的Unit ID存下来。
        public List<int> OutUnitIdList = new List<int>(100);

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public int GetGridSize()
        {
            return _gridSize;
        }

        public int GetGridHalfSize()
        {
            return _halfGridSize;
        }

        public FixInt2 GetOrigiPos()
        {
            return _originPos;
        }

        public int GetFrameDeltaMs()
        {
            return _frameDeltaMs;
        }

        public Node[,] GetNode2D()
        {
            return _grid;
        }

        private int _minX = 0;
        private int _maxX = 0;
        private int _minY = 0;
        private int _maxY = 0;

        private int _MinWorldPosX = 0;
        private int _MinWorldPosY = 0;
        private int _MaxWorldPosX = 0;
        private int _MaxWorldPosY = 0;

        public int MinWorldPosX
        {
            get { return _minX; }
        }

        public int MinWorldPosY
        {
            get { return _minY; }
        }

        public int MaxWorldPosX
        {
            get { return _maxX; }
        }

        public int MaxWorldPosY
        {
            get { return _maxY; }
        }

        private BattleRandom _random = null;

        public BattleRandom GetBattleRandom()
        {
            return _random;
        }

        //左方单位的BSPTree
        public BSPTree BspTreeLeft { get; set; } = null;

        //右方单位的BSPTree
        public BSPTree BspTreeRight { get; set; } = null;

        public BattleEntityPoolManager EntityPoolManager { get; set; } = null;

        //初始化地图
        public bool Init(BattleEntityPoolManager mgr, int orginX, int orginY, int width, int height,
            int gridSizeFixIntScale, int frameDeltaMs, int maxPathPosDis)
        {
            EntityPoolManager = mgr;
            MathCache.Init();

            _targetWorldPos = EntityPoolManager.getTargetWorldPos(); // = new List<FixInt2>(20);
            _targetGridPos = EntityPoolManager.getTargetGridPos(); // = new List<Integer2>(20);
            _resultPath = EntityPoolManager.getResultPath(); // = new List<FixInt2>(300);
            _resultPathSimplified = EntityPoolManager.getResultPathSimplified(); // = new List<FixInt2>(300);
            _restoreGridOccupyUnitList = EntityPoolManager.getRestoreGridOccupyUnitList(); // = new List<Unit>(20);

            _originPos = new FixInt2((int)orginX * FixInt2.Scale, (int)orginY * FixInt2.Scale);
            _width = width;
            _height = height;
            _gridSize = gridSizeFixIntScale;
            _halfGridSize = _gridSize / 2;
            _frameDeltaMs = frameDeltaMs;

            int offset = 10;
            _minX = _originPos.x + offset;
            _maxX = _originPos.x + _width * _gridSize - offset;
            _minY = _originPos.y + offset;
            _maxY = _originPos.y + _height * _gridSize - offset;

            _maxPathPosDis = maxPathPosDis;

            //初始化格子
            _grid = new Node[width, height];
            int nodeIdx = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var n = EntityPoolManager.getNodeObj();
                    n.InitWithData(this, x, y);
                    n.Index = nodeIdx++;
                    _grid[x, y] = n;
                }
            }

            InitPathRequest();

            _priorityQ_nearest = EntityPoolManager.getFastPriorityQueue();
            _processedNode_nearest = EntityPoolManager.getProcessedNodeNearest();

            return true;
        }

        //public void AddGridObstacled(int x, int y)
        //{
        //    _grid[x, y].AddObstacled();
        //}
        //public void RmGridObstacled(int x, int y)
        //{
        //    _grid[x, y].RemovObstacled();
        //}

        //在这个之前，调用InitUnitAndTeam
        //准备开始战斗，调用完这个后，可以开始Tick
        //这个调用完后可以设置Debugger
        public bool BeforeBattle(int randSeed)
        {
            _random = new BattleRandom(randSeed);
            //InitTeamData();
            InitAllUnitGridData();

            if (_debugger != null)
                _debugger.ClearAll();
            return true;
        }

        //每帧更新
        public void Tick()
        {
            _processedNodeThisFrame = 0;
            _processedPathThisFrame = 0;

            //处理寻路请求
            _enableRecordNodeChange = false;
            var maxReq = GetMapConfig().PathRequestPerFrame;
            ProcessRequestInQ(maxReq);
            if (maxReq < 300)
            {
                //逐渐扩大寻路,避免性能极速下降
                GetMapConfig().PathRequestPerFrame = maxReq + 5;
            }


            _enableRecordNodeChange = true;

            //Tick每个单位
            foreach (var kv in _unitDict)
            {
                var u = kv.Value;
                //if (u.IsDead())
                //    continue;
                u.Tick();
            }

            if (_debugger != null)
            {
                _debugger.UpdateVirtualHero();
                _debugger.UpdateNode(_changedNodeInFrame);
                _changedNodeInFrame.Clear();
            }
        }

        // 将战斗寻路资源回收
        public void ReleaseMap()
        {
            foreach (var kv in _unitDict)
            {
                var u = kv.Value;
                EntityPoolManager.backUnitObj(u);
            }

            foreach (var kv in _teamDict)
            {
                var t = kv.Value;
                EntityPoolManager.backTeamObj(t);
            }

            _teamDict.Clear();
            _unitDict.Clear();

            foreach (var kv in _grid)
            {
                EntityPoolManager.backNodeObj(kv);
            }

            _grid = null;

            while (_pathRequestCache.Count > 0)
            {
                EntityPoolManager.backPathRequestObj(_pathRequestCache.Dequeue());
            }

            _pathRequestCache = null;

            EntityPoolManager.backFastPriorityQueue(_priorityQ_nearest);
            EntityPoolManager.backProcessedNodeNearest(_processedNode_nearest);

            _priorityQ_nearest = null;
            _processedNode_nearest = null;

            EntityPoolManager.backTargetWorldPos(_targetWorldPos);
            _targetWorldPos = null;
            EntityPoolManager.backTargetGridPos(_targetGridPos);
            _targetGridPos = null;
            EntityPoolManager.backResultPath(_resultPath);
            _resultPath = null;
            EntityPoolManager.backResultPathSimplified(_resultPathSimplified);
            _resultPathSimplified = null;
            EntityPoolManager.backRestoreGridOccupyUnitList(_restoreGridOccupyUnitList);
            _restoreGridOccupyUnitList = null;
            EntityPoolManager.RestPathRequestUID();

            _processedNode_astar.Clear();
            _priorityQ_astar.Clear();
            _restoreMovingUnitOccupyList.Clear();
        }

        public void SetDebugger(IBattleMapDebugger debugger)
        {
            _debugger = debugger;
        }

        public void NodeOccupyChanged(Node n)
        {
            if (_debugger != null && _enableRecordNodeChange)
                _changedNodeInFrame.Add(n);
        }

        public FixInt2 ClampWorldPosInMap(FixInt2 worldPos)
        {
            FixInt2 r = worldPos;

            r.x = Math.Max(_minX, r.x);
            r.x = Math.Min(_maxX, r.x);
            r.y = Math.Max(_minY, r.y);
            r.y = Math.Min(_maxY, r.y);
            return r;
        }

        //世界坐标转换到格子坐标
        public Integer2 WorldPosToGrid(FixInt2 worldPos)
        {
            int x = worldPos.x - _originPos.x;
            int y = worldPos.y - _originPos.y;
            Integer2 r;
            r.x = (x) / _gridSize; // floor operation
            r.y = (y) / _gridSize;

            if (r.x < 0 || r.y < 0)
            {
                Utils.Assert(false, "fal");
            }

            return r;
        }

        //获取格子的中心
        public FixInt2 GetGridCenter(int x, int y)
        {
            FixInt2 r = FixInt2.zero;
            r.x = _originPos.x + x * _gridSize + _gridSize / 2;
            r.y = _originPos.y + y * _gridSize + _gridSize / 2;
            return r;
        }

        //格子是否在地图内
        public bool IsGridInMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }

        //获取节点
        public Node GetNode(int x, int y)
        {
            if (!IsGridInMap(x, y))
                return null;
            return _grid[x, y];
        }

        public IBattleMapDebugger GetMapDebugger()
        {
            return _debugger;
        }
        

        public List<int> GetUnitIdsInRange(Unit unit, int radius)
        {
            List<int> ret = new List<int>() { };
            BspTreeLeft.FindUnitsInCircleRange(unit.WorldLogicPos, radius, ret);
            BspTreeRight.FindUnitsInCircleRange(unit.WorldLogicPos, radius, ret);
            return ret;
        }

        public List<int> GetUnitsInCircle(int x, int y, int radius, int side)
        {
            List<int> ret = new List<int>() { };
            BSPTree tree = side == 1 ? BspTreeLeft : BspTreeRight;
            tree.FindUnitsInCircleRange(new FixInt2(x, y), radius, ret);
            return ret;
        }
    }
}