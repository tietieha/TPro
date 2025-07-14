using System.Collections.Generic;
using FixPoint;
using Priority_Queue;

namespace M.PathFinding
{
    public partial class BattleMap
    {
        //请求缓存
        static private int PathRequstQueueCahceSize = 1000;
        static private int MaxPathPosDis = 10*1000*10*1000; //路点距离起点到终点直线的最大垂直距离
        private Queue<PathRequest> _pathRequestCache = new Queue<PathRequest>(PathRequstQueueCahceSize);
        public PathRequest GetPathRequestFromCache()
        {
            var q = _pathRequestCache.Dequeue();
            return q;
        }

        public void ReturnPathRequestToCache(PathRequest r)
        {
            _pathRequestCache.Enqueue(r);
        }

        //寻路请求队列
        private readonly List<Queue<PathRequest>> _pathRequestQ = new List<Queue<PathRequest>>((int)ERequestPriority.MaxNum);

        private void InitPathRequest()
        {
            for (int i = 0; i < (int)ERequestPriority.MaxNum; i++)
            {
                var queue = new Queue<PathRequest>(500);
                _pathRequestQ.Add(queue);
            }
            for (int i = 0; i < PathRequstQueueCahceSize; i++)
            {
                _pathRequestCache.Enqueue(EntityPoolManager.getPathRequestObj());
            }
        }

        //四方向查找
        private readonly List<Integer2> _dir4 = new List<Integer2>()
        {
            new Integer2(1, 0),
            new Integer2(-1, 0),
            new Integer2(0, -1),
            new Integer2(0, 1),
        };

        //挤开四方向查找
        private readonly List<Integer2> _findDir4 = new List<Integer2>()
        {
            new Integer2(0, -1),
            new Integer2(0, 1),
            new Integer2(-1, 0),
            new Integer2(1, 0),
        };
        //当前帧计算的路径数量
        private int _processedPathThisFrame = 0;
        //当前帧总探查的grid数量
        private int _processedNodeThisFrame = 0;

        public int ProcessedPathThisFrame { get => _processedPathThisFrame; }
        public int ProcessedNodeThisFrame { get => _processedNodeThisFrame; }

        //让某个单位占住某个位置
        public void Internal_AddUnitOccupy_GridPos(Unit u, int gridX, int gridY, EUnitType unitType)
        {
            int start = 0;
            int end = (int)EUnitType.Count;
            if (unitType != EUnitType.All)
            {
                start = (int)unitType;
                end = (int)unitType + 1;
            }

            for (int layerIndex = start; layerIndex < end; layerIndex++)
            {
                //更新每一层的可行走范围
                //总的扩张
                var e = u.GetRealExtendOnLayer((EUnitType)layerIndex);
                if (e < 0)
                    continue;

                int totalExtend = e;
                var gridPosList = MathCache.GetGridInCircle(totalExtend);
                for (int i = 0; i < gridPosList.Count; i++)
                {
                    var p = gridPosList[i];
                    var node = GetNode(gridX + p.x, gridY + p.y);
                    if (node != null)
                        node.AddOccupied((EUnitType)layerIndex);
                }
            }
        }

        //让某个单位取消某个位置的占据
        public void Internal_RemoveUnitOccupy_GridPos(Unit u, int gridX, int gridY, EUnitType unitType)
        {
            //int unitExtend = u.GetExtend();

            int start = 0;
            int end = (int)EUnitType.Count;
            if (unitType != EUnitType.All)
            {
                start = (int)unitType;
                end = (int)unitType + 1;
            }

            for (int layerIndex = start; layerIndex < end; layerIndex++)
            {
                //更新每一层的可行走范围
                //总的扩张
                var e = u.GetRealExtendOnLayer((EUnitType)layerIndex);
                if (e < 0)
                    continue;

                int totalExtend = e;
                var gridPosList = MathCache.GetGridInCircle(totalExtend);
                for (int i = 0; i < gridPosList.Count; i++)
                {
                    var p = gridPosList[i];
                    var node = GetNode(gridX + p.x, gridY + p.y);
                    if (node != null)
                        node.RemoveOccupied((EUnitType)layerIndex);
                }
            }
        }

        //找最近下脚点的格子
        private FastPriorityQueue<Node> _priorityQ_nearest;
        private List<Node> _processedNode_nearest;
        //找到某个位置最近的空位置，可以踩下去
        public Integer2 FindNearestNotOccupiedNode(int x, int y, EUnitType unitType, int maxTestNum = 500)
        {
            Utils.Assert(_priorityQ_astar.Count == 0);
            Utils.Assert(_priorityQ_nearest.Count == 0);
            Utils.Assert(x >= 0 && x < GetWidth());
            Utils.Assert(y >= 0 && y < GetHeight());

            Integer2 result = new Integer2();
            result.Set(-1, -1);

            var startNode = GetNode(x, y);

            //Utils.Assert(startNode != null, "X: " + x.ToString() + " Y: " + y.ToString());

            //如果输入的位置可以站人，直接返回
            if (!startNode.IsOccupied(unitType))
                result.Set(x, y);
            else
            {
                _priorityQ_nearest.Clear();
                _priorityQ_nearest.Enqueue(startNode, 0);
                startNode.State = ENodeState.Close;
                _processedNode_nearest.Clear();

                int testedNum = 0;
                while (true)
                {
                    if (_priorityQ_nearest.Count == 0)
                        break;

                    Node t = _priorityQ_nearest.Dequeue();
                    if (!t.IsOccupied(unitType))
                    {
                        result = t.GetPos();
                        break;
                    }

                    _processedNode_nearest.Add(t);

                    //测试的数量超过最大值，则退出
                    testedNum++;
                    if (testedNum > maxTestNum)
                        break;

                    //找与T邻接的节点加入Queue
                    if (unitType == EUnitType.Soldier_Left || unitType == EUnitType.Soldier_Right)
                    {
                        for (int i = 0; i < _findDir4.Count; i++)
                        {
                            var dir = _findDir4[i];
                            Integer2 p = t.GetPos() + dir;
                            var n = GetNode(p.x, p.y);
                            if (n != null && n.State != ENodeState.Close)
                            {
                                int dis = MathCache.GetGridDistance(p, startNode.GetPos());
                                _priorityQ_nearest.Enqueue(n, dis);
                                n.State = ENodeState.Close;

                                _processedNode_nearest.Add(n);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _dir4.Count; i++)
                        {
                            var dir = _dir4[i];
                            Integer2 p = t.GetPos() + dir;
                            var n = GetNode(p.x, p.y);
                            if (n != null && n.State != ENodeState.Close)
                            {
                                int dis = MathCache.GetGridDistance(p, startNode.GetPos());
                                _priorityQ_nearest.Enqueue(n, dis);
                                n.State = ENodeState.Close;

                                _processedNode_nearest.Add(n);
                            }
                        }
                    }
                }
                //还原格子的信息
                _priorityQ_nearest.Clear();
                for (int i = 0; i < _processedNode_nearest.Count; i++)
                {
                    var n = _processedNode_nearest[i];
                    n.State = ENodeState.New;
                    _priorityQ_nearest.ResetNode(n);
                }
                _processedNode_nearest.Clear();

            }

            return result;
        }

        //添加请求到队列
        public void PushPathRequest(PathRequest request)
        {
            var p = request.GetPriority();
            _pathRequestQ[(int)p].Enqueue(request);
        }

        //处理队列中的N个请求
        private readonly List<Unit> _restoreMovingUnitOccupyList = new List<Unit>(300);
        private void ProcessRequestInQ(int maxRequestNum)
        {
            int processedNum = 0;
            int currentPriority = 0;
            int maxPriority = (int)ERequestPriority.MaxNum;

            //临时移除所有正在移动的单位
            _restoreMovingUnitOccupyList.Clear();
            foreach (var kv in _unitDict)
            {
                var u = kv.Value;
                if (u.IsVirtual() || !u.IsOccupiedGird() || u.IsDead()) // || !u.IsMoving  u.GetCurrentState() == EUnitState.Stop
                    continue;

                if (u.IsMoving || u.GetCurrentStateEnum() == EUnitState.SoldierFollowVirtualHero || u.GetCurrentStateEnum() == EUnitState.HorseFollowVirtualHero)
                    u.Internal_RemoveCurrentGridOccupy();
                _restoreMovingUnitOccupyList.Add(u);
            }

            while (true)
            {
                if (currentPriority >= maxPriority)
                    break;
                var q = _pathRequestQ[currentPriority];
                if (q.Count == 0)
                {
                    currentPriority++;
                    continue;
                }

                if (processedNum >= maxRequestNum)
                    break;

                var r = q.Dequeue();

                //查看寻路请求是否还有效
                var startU = r.GetStartUnit();
                if (startU.PathState.GetCurrentWaitingPathId() != r.GetUID())
                {
                    ReturnPathRequestToCache(r);
                    continue;
                }

                ProcessOneRequest(r);
                //ReturnPathRequestToCache(r);

                processedNum++;
                _processedPathThisFrame++;

            }

            //把所有正在移动的单位加回来
            for (int i = 0; i < _restoreMovingUnitOccupyList.Count; i++)
            {
                var u = _restoreMovingUnitOccupyList[i];
                u.Internal_ChangeOccupyGrid(u.GetGridPos().x, u.GetGridPos().y);
            }
        }

        #region 寻路中的临时变量
        private FixInt2 _startWorldPos;
        private Integer2 _startGridPos;
        private List<FixInt2> _targetWorldPos;// = new List<FixInt2>(20);
        private List<Integer2> _targetGridPos;// = new List<Integer2>(20);
        private List<FixInt2> _resultPath;// = new List<FixInt2>(300);
        private List<FixInt2> _resultPathSimplified;// = new List<FixInt2>(300);
        private List<Unit> _restoreGridOccupyUnitList;// = new List<Unit>(20);

        #endregion
        //处理一条请求
        private void ProcessOneRequest(PathRequest request)
        {
            _targetGridPos.Clear();
            _targetWorldPos.Clear();
            _restoreGridOccupyUnitList.Clear();

            EPathRequestType t = request.GetRequestType();
            Unit startU = request.GetStartUnit();

            _startWorldPos = startU.GetWorldPos();
            _startGridPos = startU.GetGridPos();

            //需要寻路结束后回复占据格子的单位
            if (startU.IsOccupiedGird())
            {
                var gridPos = startU.GetGridPos();
                if (startU.GetOccupiedGrid() != gridPos)
                {
                    //var error = "";
                }

                //保证站在格子上
                startU.InternalEnableOccupy_Stand();

                //仅仅移除当前layerIndex
                startU.Internal_RemoveCurrentGridOccupy_Temp(startU.GetUnitType());
                _restoreGridOccupyUnitList.Add(startU);
            }

            // if (startU.IsOccupiedGird())
            // {

            //     Utils.Assert(startU.GetOccupiedGrid() == gridPos);
            //     Node startNode = GetNode(gridPos.x, gridPos.y);
            //     bool isOccu = startNode.IsOccupied(startU.GetExtend());
            //     if (isOccu)
            //     {
            //         var error = "";
            //     }
            //     Utils.Assert(!isOccu);
            // }

            if (t == EPathRequestType.ToOneUnit)
            {
                var targetU = request.GetTargetUnit();
                var v = targetU.GetWorldPos();
                var gridPos = WorldPosToGrid(v);
                _targetWorldPos.Add(v);
                _targetGridPos.Add(gridPos);
                if (targetU.IsOccupiedGird())
                {
                    targetU.Internal_RemoveCurrentGridOccupy_Temp(startU.GetUnitType());

                    //TODO 移除想忽略的单位

                    _restoreGridOccupyUnitList.Add(targetU);
                }
            }
            if (t == EPathRequestType.ToTeam)
            {
                var teamId = request.GetTargetTeamId();
                var team = _teamDict[teamId];

                void AddTargetUnit(Unit targetUnit)
                {
                    var v = targetUnit.GetWorldPos();
                    var gridPos = WorldPosToGrid(v);
                    _targetWorldPos.Add(v);
                    _targetGridPos.Add(gridPos);
                    if (targetUnit.IsOccupiedGird())
                    {
                        targetUnit.Internal_RemoveCurrentGridOccupy_Temp(startU.GetUnitType());
                        _restoreGridOccupyUnitList.Add(targetUnit);
                    }
                }

                for (int i = 0; i < team.AllUnitList.Count; i++)
                {
                    var targetU = team.AllUnitList[i];
                    if (targetU.IsDead() || targetU.IsVirtual())
                        continue;
                    AddTargetUnit(targetU);
                }

                // //活着的小兵数量
                // int livingSoldierNum = 0;
                // for (int i = 0; i < team.AllUnitList.Count; i++)
                // {
                //     var targetU = team.AllUnitList[i];
                //     if (!targetU.IsDead() && !targetU.IsHero())
                //         livingSoldierNum++;
                // }
                // for (int i = 0; i < team.AllUnitList.Count; i++)
                // {
                //     var targetU = team.AllUnitList[i];
                //     if (targetU.IsDead())
                //         continue;
                //     //如果自己是小兵，则只找敌方的小兵
                //     if (!startU.IsHero() && livingSoldierNum > 0 && targetU.IsHero())
                //         continue;

                //     //如果是武将，只找武将
                //     if (startU.IsHero() && !targetU.IsHero())
                //         continue;

                //     AddTargetUnit(targetU);
                // }

            }
            else if (t == EPathRequestType.ToPosition)
            {
                var v = request.GetTargetPos();
                var gridPos = WorldPosToGrid(v);
                _targetWorldPos.Add(v);
                _targetGridPos.Add(gridPos);
            }

            //开始进行寻路

            // var maxTestNodeNum = GetMapConfig().SoliderMaxTestNode;
            // if (startU.IsHero())
            //     maxTestNodeNum = GetMapConfig().HeroMaxTestNode;;
            var foundPath = FindPath(request, startU.GetUnitType(), ELayerType.Low, request.GetStopDis(), request.GetMaxNodeTestNum());

            //回复格子的占用
            for (int i = 0; i < _restoreGridOccupyUnitList.Count; i++)
            {
                var u = _restoreGridOccupyUnitList[i];
                //var gp = u.GetGridPos();
                u.Internal_AddCurrentGridOccupy_Temp(startU.GetUnitType());
            }

            //把寻路结果告诉Unit
            startU.PathState.OnPathFindingFinished(request, _resultPathSimplified, foundPath);
        }


        private bool _isPathTooFarAway(Node startNode, Node endNode)
        {
            bool isTooFar = false;
            if (startNode == null || endNode == null)
                return isTooFar;

            var startPos = startNode.GetPos();
            var endPos = endNode.GetPos();
            var a = MathCache.GetGridDistance(startPos, endPos);
            if (a<1)
                return isTooFar;

            if (_resultPath.Count > 1)
            {
                for (int i = 0; i < _resultPathSimplified.Count; i++)
                {
                    var gridPos = WorldPosToGrid(_resultPathSimplified[i]);
                    var b = MathCache.GetGridDistance(startPos, gridPos);
                    var c = MathCache.GetGridDistance(endPos, gridPos);
                    var p = (a+b+c)/2;
                    var powS = p * (p - a) * (p - b) * (p - c); //FixMath.Sqrt(p*(p-a)*(p-b)*(p-c));
                    if (4* powS / (a*a) >= MaxPathPosDis)//(2*s/a >= MaxPathPosDis)
                    {
                        isTooFar = true;
                        break;
                    }
                }
            }
            return isTooFar;
        }

        //粗略计算到目标的最近距离
        long ComputeMinDisToTargetList(Integer2 myPos)
        {
            long minDis = Utils.MAX_DISTANCE;
            for (int i = 0; i < _targetGridPos.Count; i++)
            {
                var pos = _targetGridPos[i];
                var myDis = MathCache.GetGridDistance(pos, myPos);
                if (myDis < minDis)
                    minDis = myDis;
            }
            return minDis;
        }

        //寻路A*用的优先级队列。Priority数值低的会优先被拉出来
        private readonly FastPriorityQueue<Node> _priorityQ_astar = new FastPriorityQueue<Node>(300);
        private readonly List<Node> _processedNode_astar = new List<Node>(500);

        //在这个寻路过程中，可走信息不再会改变了
        //返回是否找到可走的路。探查的所有节点中，是否有比起始位置更优的几点
        public bool FindPath(
            PathRequest request,
            EUnitType layerIndex, //在哪一层寻
            ELayerType layerType,
            int stopDis, //停止距离
            int maxTestedNodeCount //最大探查数量
        )
        {
            bool result = false;

            //Utils.Assert(_priorityQ_astar.Count == 0);
            //Utils.Assert(_priorityQ_nearest.Count == 0);
            //A*寻路
            //进入终止距离即退出，遍历的格子数量达到上限也退出，超出边界的格子不会去考虑
            //多算一条直走可以到达的最远路径。A*与直走两者距离终点最近的那条会被采纳返回（直走的可能没有什么用）
            Node startNode = GetNode(_startGridPos.x, _startGridPos.y);

            //TODO:暂时关闭
            //Utils.Assert(!startNode.IsOccupied(layerIndex));

            _processedNode_astar.Clear();
            _resultPath.Clear();
            _resultPathSimplified.Clear();

            _priorityQ_astar.Clear();
            _priorityQ_astar.Enqueue(startNode, 0);
            startNode.State = ENodeState.Close;
            startNode.PathAccumCost = 0;
            startNode.PathPreNode = null;
            startNode.DisToEnd = ComputeMinDisToTargetList(startNode.GetPos());

            _processedNode_astar.Add(startNode);

            //int testedNum = 0;

            //结尾的结点
            Node endNode = null;
            long endNodeDistance = Utils.MAX_DISTANCE;

            while (true)
            {
                if (_priorityQ_astar.Count == 0)
                    break;

                if (_processedNode_astar.Count > maxTestedNodeCount)
                    break;

                Node thisNode = _priorityQ_astar.Dequeue();

                Integer2 tPos = thisNode.GetPos();

                long dis = (thisNode.DisToEnd + 1414) * _gridSize / FixInt2.Scale;
                if (dis < stopDis)
                {
                    endNode = thisNode;
                    endNodeDistance = dis;
                    break;
                }

                //testedNum++;

                //找与T邻接的节点加入Queue
                for (int i = 0; i < _dir4.Count; i++)
                {
                    var dir = _dir4[i];
                    Integer2 p = tPos + dir;
                    var newNode = GetNode(p.x, p.y);

                    if (newNode == null || newNode.IsOccupiedOrLayer(layerIndex, layerType))
                        continue;

                    var newAccumCost = thisNode.PathAccumCost + 1; //如果每个点都有不同损耗的话，需要修改这里
                    //如果有必要，修正这个节点的父亲、总路径消耗
                    if (newNode.State == ENodeState.Close)
                    {
                        if (newNode.PathAccumCost > newAccumCost)
                        {
                            newNode.PathPreNode = thisNode;
                            newNode.PathAccumCost = newAccumCost;
                        }
                    }
                    else
                    {
                        newNode.PathPreNode = thisNode;
                        newNode.PathAccumCost = newAccumCost;
                        _processedNode_astar.Add(newNode);

                        //float dis = MathCache.GetGridDistance(p, startNode.GetPos());
                        //距离多个目标的最近距离
                        long minDis = ComputeMinDisToTargetList(p);
                        newNode.DisToEnd = minDis;
                        _priorityQ_astar.Enqueue(newNode, (int)(minDis + newAccumCost));
                        newNode.State = ENodeState.Close;

                        if (minDis < endNodeDistance)
                        {
                            endNodeDistance = minDis;
                            endNode = newNode;
                        }
                    }
                }

            }

            //从EndNode反推整个路径
            {
                //路径的终点比当前的起点，距离目标点还要远，则路径返回空
                if (endNode != null && endNode.DisToEnd >= startNode.DisToEnd)
                {
                    //request.GetStartUnit().Log("Path finding failed");
                    _resultPath.Clear();
                }
                else
                {
                    //找到终点距离最近的目标点
                    if (endNode != null)
                    {
                        // var stopDisSqr = stopDis * stopDis;
                        // foreach (var targetP in _targetWorldPos)
                        // {
                        //     var disSqr = (targetP - endNode.GetCenterWorldPos()).sqrMagnitude;
                        //     if (_targetWorldPos.Count == 1 || disSqr < stopDisSqr)
                        //     {
                        //         _resultPath.Add(targetP);
                        //     }
                        // }
                    }

                    Node node = endNode;
                    while (node != null)
                    {
                        _resultPath.Add(node.GetCenterWorldPos());
                        node = node.PathPreNode;
                        result = true;
                    }
                    _resultPath.Add(_startWorldPos); //加入起始的世界坐标
                }

            }

            //对路径做简化
            if (_resultPath.Count > 2)
                SimplifyPath(_resultPath, _resultPathSimplified, layerIndex, layerType);
            else
            {
                _resultPathSimplified.Clear();
                for (int i = 0; i < _resultPath.Count; i++)
                {
                    _resultPathSimplified.Add(_resultPath[i]);
                }
            }

            //TODO: 判断是否超出Y轴方向。
            // 首先构造起始点到目标点的直线（如果目标点有多个，就选他们的中心点），
            // 然后判断_resultPathSimplified上面所有的点距离这条直线的距离是否大于10（策划设置的值）
            // 如果超过这个值，就标记路径无效
            bool isPathTooFarAway = _isPathTooFarAway(startNode, endNode);
            if (isPathTooFarAway)
            {
                result = false;
                _resultPath.Clear();
                _resultPathSimplified.Clear();
            }

            //_resultPathSimplified.Add(_startWorldPos); //加入起始的世界坐标

            // var result = _resultPathSimplified;
            // if(!request.GetStartUnit().IsHero())
            // {
            //     Utils.Log("aaa");
            // }

            _processedNodeThisFrame += _processedNode_astar.Count;
            //清空
            _priorityQ_astar.Clear();
            for (int i = 0; i < _processedNode_astar.Count; i++)
            {
                var node = _processedNode_astar[i];
                node.ClearPathFindingData();
                _priorityQ_astar.ResetNode(node);
            }
            _processedNode_astar.Clear();

            return result;
        }

        //简化路径
        private void SimplifyPath(List<FixInt2> input, List<FixInt2> output, EUnitType layerIndex, ELayerType layerType)
        {
            Utils.Assert(input.Count >= 2);
            output.Clear();
            if (input.Count == 2)
            {
                output.Add(input[0]);
                output.Add(input[1]);
                return;
            }

            output.Add(input[0]);

            int startIndex = 0;
            int endIndex = input.Count - 1;
            while (true)
            {
                while (true)
                {
                    FixInt2 startPos = input[startIndex];
                    FixInt2 endPos = input[endIndex];
                    Integer2 gridPos = new Integer2(-1, -1);

                    //如果只有一段，则直接返回
                    if (startIndex + 1 == endIndex)
                    {
                        output.Add(endPos);
                        startIndex = endIndex;
                        endIndex = input.Count - 1;
                        break;
                    }

                    bool hasIntersect = Raycast(layerIndex, layerType, startPos, endPos, ref gridPos);
                    if (!hasIntersect)
                    {
                        output.Add(endPos);
                        startIndex = endIndex;
                        endIndex = input.Count - 1;
                        break;
                    }
                    else
                    {
                        endIndex = (startIndex + endIndex) / 2;
                        if (endIndex == startIndex)
                            endIndex++;
                    }

                }
                if (startIndex == endIndex)
                    break;
            }

        }

    }

}