//Binary space partitioning
//基于Grid的2叉空间划分树状结构
//用来快速找到某位置周围一定半径内的单位

using System;
using System.Collections.Generic;
using FixPoint;
using Priority_Queue;

namespace M.PathFinding
{
    public class BSPTreeNode : FastPriorityQueueNode
    {
        private BattleMap _map = null;
        private BSPTree _tree = null;
        //位于[min, max)之间
        public int minGridX = -1;
        public int minGridY = -1;
        public int maxGridX = -1;
        public int maxGridY = -1;

        //世界坐标
        public FixInt minWorldX = -1;
        public FixInt minWorldY = -1;
        public FixInt maxWorldX = -1;
        public FixInt maxWorldY = -1;

        //这个节点下Unit的总数
        public int TotalUnitCount { get; set; } = 0;

        public int MaxUnitRadius { get; set; } = 0;

        //左子节点
        public BSPTreeNode LeftChild { get; set; } = null;
        //右子节点
        public BSPTreeNode RightChild { get; set; } = null;
        //父节点
        public BSPTreeNode Parent { get; set; } = null;

        //只有叶子节点才有Unit
        public HashSet<Unit> UnitSet { get; set; } = new HashSet<Unit>();
        //只有叶子节点才有FortificationUnit Node
        public HashSet<Node> FortificationNodeSet { get; set; } = new HashSet<Node>();

        //初始化BSPTree
        public void Init(BSPTree tree, int minGridX, int minGridY, int maxGridX, int maxGridY)
        {
            _tree = tree;
            _map = _tree.Map;
            this.minGridX = minGridX;
            this.minGridY = minGridY;
            this.maxGridX = maxGridX;
            this.maxGridY = maxGridY;

            this.minWorldX = _map.GetOrigiPos().x + _map.GetGridSize() * this.minGridX;
            this.maxWorldX = _map.GetOrigiPos().x + _map.GetGridSize() * this.maxGridX;
            this.minWorldY = _map.GetOrigiPos().y + _map.GetGridSize() * this.minGridY;
            this.maxWorldY = _map.GetOrigiPos().y + _map.GetGridSize() * this.maxGridY;

            //X方向比较大，还是Y方向比较大
            int xSize = maxGridX - minGridX;
            int ySize = maxGridY - minGridY;
            if (xSize > ySize)
            {
                if (xSize > _tree.MaxLeafSize)
                {
                    //沿着X方向切割
                    int xMiddle = minGridX + (maxGridX - minGridX) / 2;
                    LeftChild = _map.EntityPoolManager.getBSPTreeNodeObj();
                    LeftChild.Init(_tree, minGridX, minGridY, xMiddle, maxGridY);
                    LeftChild.Parent = this;
                    RightChild = _map.EntityPoolManager.getBSPTreeNodeObj();
                    RightChild.Init(_tree, xMiddle, minGridY, maxGridX, maxGridY);
                    RightChild.Parent = this;
                }
            }
            else
            {
                if (ySize > _tree.MaxLeafSize)
                {
                    //沿着Y方向切割
                    int yMiddle = minGridY + (maxGridY - minGridY) / 2;
                    LeftChild = _map.EntityPoolManager.getBSPTreeNodeObj();
                    LeftChild.Init(_tree, minGridX, minGridY, maxGridX, yMiddle);
                    LeftChild.Parent = this;
                    RightChild = _map.EntityPoolManager.getBSPTreeNodeObj();
                    RightChild.Init(_tree, minGridX, yMiddle, maxGridX, maxGridY);
                    RightChild.Parent = this;
                }
            }
        }

        public void Reset()
        {
            _map = null;
            _tree = null;
        }

        public bool IsWorldPosInMe(FixInt2 worldPos)
        {
            return worldPos.x >= minWorldX.Value && worldPos.x < maxWorldX.Value &&
                worldPos.y >= minWorldY.Value && worldPos.y < maxWorldY.Value;
        }

        //找到这个位置应该属于哪个叶子节点
        public BSPTreeNode FindLeafNode(FixInt2 worldPos)
        {
            Utils.Assert(IsWorldPosInMe(worldPos));
            if (this.LeftChild == null)
                return this;
            else
            {
                if (LeftChild.IsWorldPosInMe(worldPos))
                    return LeftChild.FindLeafNode(worldPos);
                else
                    return RightChild.FindLeafNode(worldPos);
            }
        }

        //从底向上修正UnitCount
        public void ChangeUnitCountBottomUp(int countChange)
        {
            TotalUnitCount += countChange;
            Utils.Assert(TotalUnitCount >= 0);

            if (Parent != null)
                Parent.ChangeUnitCountBottomUp(countChange);
        }

        public void UpdateUnitMaxRadiusBottomUp()
        {
            if (LeftChild == null)
            {
                MaxUnitRadius = 0;
                foreach (var u in UnitSet)
                {
                    if (u.GetRadius() > MaxUnitRadius)
                        MaxUnitRadius = u.GetRadius();
                }

                if (FortificationNodeSet.Count > 0 && _map.GetGridHalfSize() > MaxUnitRadius)
                    MaxUnitRadius = _map.GetGridHalfSize();
            }
            else
            {
                MaxUnitRadius = Math.Max(LeftChild.MaxUnitRadius, RightChild.MaxUnitRadius);
            }
            if (Parent != null)
                Parent.UpdateUnitMaxRadiusBottomUp();
        }

        //计算这个节点包围盒距离世界坐标的最近距离的平方
        public long CalculateMinSqrDistanceToPoint(FixInt2 p)
        {
            FixInt2 c = new FixInt2();
            c.x = Math.Max(Math.Min(p.x, maxWorldX.Value), minWorldX.Value);
            c.y = Math.Max(Math.Min(p.y, maxWorldY.Value), minWorldY.Value);
            long s = (c - p).sqrMagnitude;
            //if (s > long.MaxValue)
            //    s = long.MaxValue;
            return s;
        }

        //计算这个节点包围盒距离世界坐标的最近距离
        public long CalculateMinDistanceToPoint(FixInt2 p)
        {
            FixInt2 c = new FixInt2();
            c.x = Math.Max(Math.Min(p.x, maxWorldX.Value), minWorldX.Value);
            c.y = Math.Max(Math.Min(p.y, maxWorldY.Value), minWorldY.Value);
            long s = (c - p).magnitude;
            //if (s > long.MaxValue)
            //    s = long.MaxValue;
            return s;
        }
    }

    public class BSPTree
    {

        public BattleMap Map { get; set; } = null;
        public int MaxLeafSize { get; set; } = 2;
        //根节点
        public BSPTreeNode Root { get; set; }

        //创建BSP树。输入最大叶子的大小，1x1、2x2等等
        static public BSPTree Create(BattleMap map, int maxLeafSize)
        {
            var b = new BSPTree();
            b.Map = map;
            b.MaxLeafSize = maxLeafSize;
            b.Root = b.CreateRootNode(0, 0, map.GetWidth(), map.GetHeight());
            return b;
        }

        private BSPTreeNode CreateRootNode(int minGridX, int minGridY, int maxGridX, int maxGridY)
        {
            var n = Map.EntityPoolManager.getBSPTreeNodeObj();
            n.Init(this, minGridX, minGridY, maxGridX, maxGridY);
            return n;
        }

        //根据世界坐标，找到属于哪个子节点
        private BSPTreeNode FindLeafNode(FixInt2 worldPos)
        {
            return Root.FindLeafNode(worldPos);
        }

        //移除单位
        public void RemoveUnit(Unit u)
        {
            var leaf = u._BSPTreeNode;
            Utils.Assert(leaf != null);

            leaf.UnitSet.Remove(u);
            u._BSPTreeNode = null;

            leaf.ChangeUnitCountBottomUp(-1);
            leaf.UpdateUnitMaxRadiusBottomUp();
        }

        //增加单位到树中
        public void AddUnit(Unit u)
        {
            //找到叶子节点，扔进去
            var leaf = FindLeafNode(u.WorldPos);
            Utils.Assert(leaf != null);

            leaf.UnitSet.Add(u);
            u._BSPTreeNode = leaf;

            leaf.ChangeUnitCountBottomUp(1);
            leaf.UpdateUnitMaxRadiusBottomUp();
        }

        //更新单位位置
        public void UpdateUnitPosition(Unit unit)
        {
            if (unit._BSPTreeNode != null)
                RemoveUnit(unit);
            AddUnit(unit);
        }

        private readonly FastPriorityQueue<BSPTreeNode> _searchQ = new FastPriorityQueue<BSPTreeNode>(300);


        #region 寻路索敌相关
        //找到中心距离p最近的Unit
        public Unit FindNearestUnit(FixInt2 p, Func<Unit, bool> FilterFun)
        {
            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            int minDisSqr = int.MaxValue;
            Unit result = null;
            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;
                    if (left.TotalUnitCount > 0)
                    {
                        var leftDisSqr = left.CalculateMinSqrDistanceToPoint(p);
                        if (leftDisSqr <= minDisSqr) //只放入距离有可能在我附近的节点
                            _searchQ.Enqueue(left, leftDisSqr);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var rightDisSqr = right.CalculateMinSqrDistanceToPoint(p);
                        if (rightDisSqr <= minDisSqr)
                            _searchQ.Enqueue(right, rightDisSqr);
                    }

                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        if (!u.CanTarget())
                            continue;
                        var disSqr = (u.WorldPos - p).sqrMagnitude;
                        if (disSqr > int.MaxValue)
                            disSqr = int.MaxValue;

                        if (disSqr < minDisSqr)
                        {
                            if (FilterFun != null && !FilterFun(u))
                                continue;
                            result = u;
                            minDisSqr = (int)disSqr;
                        }
                    }
                }
            }
            _searchQ.Clear();
            //Utils.Log("FindNearestUnit iter: {0}", iterCount);
            return result;
        }

        public Unit FindNearestOtherTeamUnit(Unit owner, FixInt2 p, Func<Unit, bool> FilterFun)
        {
            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            int minDisSqr = int.MaxValue;
            Unit result = null;
            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;
                    if (left.TotalUnitCount > 0)
                    {
                        var leftDisSqr = left.CalculateMinSqrDistanceToPoint(p);
                        if (leftDisSqr <= minDisSqr) //只放入距离有可能在我附近的节点
                            _searchQ.Enqueue(left, leftDisSqr);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var rightDisSqr = right.CalculateMinSqrDistanceToPoint(p);
                        if (rightDisSqr <= minDisSqr)
                            _searchQ.Enqueue(right, rightDisSqr);
                    }

                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        if (!u.CanSelect())
                            continue;

                        var disSqr = (u.WorldPos - p).sqrMagnitude;
                        if (disSqr > int.MaxValue)
                            disSqr = int.MaxValue;

                        if (disSqr < minDisSqr)
                        {
                            if (u.Team == owner.Team)
                                continue;
                            if (FilterFun != null && !FilterFun(u))
                                continue;
                            result = u;
                            minDisSqr = (int)disSqr;
                        }
                    }
                }
            }
            _searchQ.Clear();
            //Utils.Log("FindNearestUnit iter: {0}", iterCount);
            return result;
        }

        //找到任意射程内的敌人
        public Unit FindOneUnitInAttackRange(Unit unit, bool findNearest, Func<Unit, bool> FilterFun)
        {
            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            var p = unit.WorldPos;

            int minDis = int.MaxValue;

            Unit result = null;
            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;
                    if (left.TotalUnitCount > 0)
                    {
                        var dis = left.CalculateMinDistanceToPoint(p);
                        dis = Math.Max(0, dis - unit.GetRadius() - left.MaxUnitRadius);
                        if (dis <= unit.GetAttackRange())
                            _searchQ.Enqueue(left, dis);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var dis = right.CalculateMinDistanceToPoint(p);
                        dis = Math.Max(0, dis - unit.GetRadius() - right.MaxUnitRadius);
                        if (dis <= unit.GetAttackRange())
                            _searchQ.Enqueue(right, dis);
                    }

                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        long distance = 0;
                        var isInRange = unit.IsAttackTargetInMyRange(u, out distance);
                        if (isInRange)
                        {
                            if (FilterFun != null && !FilterFun(u))
                                continue;

                            if (!findNearest)
                            {
                                result = u;
                                break;
                            }
                            else
                            {
                                if (distance < minDis)
                                {
                                    result = u;
                                    minDis = (int)distance;
                                }
                            }
                        }
                    }
                }

                if (!findNearest)
                {
                    if (result != null)
                        break;
                }
            }
            _searchQ.Clear();

            //Utils.Log("FindOneUnitInAttackRange iter: {0}", iterCount);
            return result;
        }
//找到任意射程内的敌人
        public void FindUnitsInCircleRange(FixInt2 p, int radius,List<int> ret)
        {
            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            while (true)
            {
                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;
                    if (left.TotalUnitCount > 0)
                    {
                        var dis = left.CalculateMinDistanceToPoint(p);
                        if (dis <= radius)
                            _searchQ.Enqueue(left, dis);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var dis = right.CalculateMinDistanceToPoint(p);
                        if (dis <= radius)
                            _searchQ.Enqueue(right, dis);
                    }

                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        if (u.IsHero() || u.IsDead())
                            continue;

                        var disBetweenCenter = (u.WorldLogicPos - p).magnitude;
                        if (disBetweenCenter <= radius)
                        {
                            ret.Add(u.GetId());
                        }
                    }
                }
            }
            _searchQ.Clear();

        }
        #endregion


        #region 通用单位筛选函数
        //找到射程内的随机敌人
        private List<Unit> outList = new List<Unit>(100);
        public Unit FindOneUnitInAttackRangeRandom(Unit unit, Func<Unit, bool> FilterFun)
        {
            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);
            outList.Clear();
            var p = unit.WorldPos;

            Unit result = null;
            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;
                    if (left.TotalUnitCount > 0)
                    {
                        var dis = left.CalculateMinDistanceToPoint(p);
                        dis = Math.Max(0, dis - unit.GetRadius() - left.MaxUnitRadius);
                        if (dis <= unit.GetAttackRange())
                            _searchQ.Enqueue(left, dis);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var dis = right.CalculateMinDistanceToPoint(p);
                        dis = Math.Max(0, dis - unit.GetRadius() - right.MaxUnitRadius);
                        if (dis <= unit.GetAttackRange())
                            _searchQ.Enqueue(right, dis);
                    }

                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        if (!u.CanTarget())
                            continue;

                        long distance = 0;
                        var isInRange = unit.IsAttackTargetInMyRange(u, out distance);
                        if (isInRange)
                        {
                            if (FilterFun != null && !FilterFun(u))
                                continue;

                        }
                        outList.Add(u);
                    }
                }


                if (result != null)
                    break;
            }
            _searchQ.Clear();

            if (outList.Count == 1)
            {
                result = outList[0];
            }
            else if (outList.Count > 1)
            {
                var idx = unit.GetMap().GetBattleRandom().Next(0, outList.Count);
                result = outList[idx];
            }

            return result;
        }
        //找到圆圈内的单位
        public List<Unit> FindUnitInRadius(FixInt2 p, int radius, Func<Unit, bool> FilterFun)
        {
            outList.Clear();

            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            long radiusSqr = (long)radius * (long)radius;

            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;

                    if (left.TotalUnitCount > 0)
                    {
                        var leftDisSqr = left.CalculateMinSqrDistanceToPoint(p);
                        if (leftDisSqr <= radiusSqr) //只放入距离有可能在我附近的节点
                            _searchQ.Enqueue(left, leftDisSqr);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var rightDisSqr = right.CalculateMinSqrDistanceToPoint(p);
                        if (rightDisSqr <= radiusSqr)
                            _searchQ.Enqueue(right, rightDisSqr);
                    }
                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        if (!u.CanTarget())
                            continue;

                        var disSqr = (u.WorldPos - p).sqrMagnitude;
                        if (disSqr < radiusSqr)
                        {
                            if (FilterFun != null && !FilterFun(u))
                                continue;
                            outList.Add(u);
                        }
                    }
                }
            }
            _searchQ.Clear();
            //Utils.LogUtils.Log("FindUnitInRadius iter: {0}", iterCount);
            return outList;
        }

        //private static List<int> outListInt = new List<int>(100);
        //找到圆圈内的单位id
        public void FindUnitIdInRadius(FixInt2 p, int radius, ref List<int> result, Func<Unit, bool> FilterFun)
        {
            result.Clear();

            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            long radiusSqr = (long)radius * (long)radius;

            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;

                    if (left.TotalUnitCount > 0)
                    {
                        var leftDisSqr = left.CalculateMinSqrDistanceToPoint(p);
                        if (leftDisSqr <= radiusSqr) //只放入距离有可能在我附近的节点
                            _searchQ.Enqueue(left, leftDisSqr);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var rightDisSqr = right.CalculateMinSqrDistanceToPoint(p);
                        if (rightDisSqr <= radiusSqr)
                            _searchQ.Enqueue(right, rightDisSqr);
                    }
                }
                else
                {
                    foreach (var u in node.UnitSet)
                    {
                        if (!u.CanTarget())
                            continue;

                        var disSqr = (u.WorldPos - p).sqrMagnitude;
                        if (disSqr < radiusSqr)
                        {
                            if (FilterFun != null && !FilterFun(u))
                                continue;
                            result.Add(u.GetId());
                        }
                    }
                }
            }
            _searchQ.Clear();
            //Utils.LogUtils.Log("FindUnitInRadius iter: {0}", iterCount);
        }

        //冲撞特殊处理找到圆圈内的任一单位
        public Unit FindUnitIdInRadiusForDash(FixInt2 p, int radius, int heroExtraWidth, int heroExtraHeight, out Node outNode, Func<Unit, bool> FilterFun)
        {
            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);
            outNode = null;

            long radiusSqr = (long)radius * (long)radius;
            long extrRadius = radiusSqr + (long)heroExtraHeight * (long)heroExtraHeight / 4;

            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;

                    if (left.TotalUnitCount > 0)
                    {
                        var leftDisSqr = left.CalculateMinSqrDistanceToPoint(p);
                        if (leftDisSqr <= extrRadius) //只放入距离有可能在我附近的节点
                            _searchQ.Enqueue(left, leftDisSqr);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var rightDisSqr = right.CalculateMinSqrDistanceToPoint(p);
                        if (rightDisSqr <= extrRadius)
                            _searchQ.Enqueue(right, rightDisSqr);
                    }
                }
                else
                {
                    //策划为了精准控制冲撞单位数，这里使用的矩形不带方向，并且与圆判断直接加上圆半径
                    //优先判断是否有工事阻拦
                    foreach (var n in node.FortificationNodeSet)
                    {
                        var disSqr = (n.GetCenterWorldPos() - p).sqrMagnitude;
                        if (disSqr < radiusSqr)
                        {
                            outNode = n;
                            return null;
                        }
                    }
                    foreach (var u in node.UnitSet)
                    {
                        if (u.IsHero())
                        {
                            FixInt2 pos1, pos2, pos3, pos4;
                            GetRectPoss(u.WorldPos, heroExtraWidth + radius, heroExtraHeight + radius,
                                out pos1, out pos2, out pos3, out pos4);
                            if (IsSimpleContain(pos1, pos2, pos3, pos4, p))
                            {
                                if (FilterFun != null && !FilterFun(u))
                                    continue;
                                return u;
                            }
                        }
                        else
                        {
                            var disSqr = (u.WorldPos - p).sqrMagnitude;
                            if (disSqr < radiusSqr)
                            {
                                if (FilterFun != null && !FilterFun(u))
                                    continue;
                                return u;
                            }
                        }
                    }
                }
            }
            _searchQ.Clear();
            return null;
        }

        //冲撞特殊处理找到圆圈内的单位id,英雄采用半径距离
        public void FindUnitIdInRadiusForDash(FixInt2 p, int radius, int heroExtraWidth, int heroExtraHeight, ref List<int> result, Func<Unit, bool> FilterFun)
        {
            result.Clear();

            _searchQ.Clear();
            _searchQ.Enqueue(Root, 0);

            long radiusSqr = (long)radius * (long)radius;
            long extrRadius = radiusSqr + (long)heroExtraHeight * (long)heroExtraHeight / 4;

            int iterCount = 0;
            while (true)
            {
                iterCount++;

                if (_searchQ.Count == 0)
                    break;

                var node = _searchQ.Dequeue();

                if (node.LeftChild != null)
                {
                    //如果不是叶子节点，则把左右两个儿子都放入队列
                    var left = node.LeftChild;
                    var right = node.RightChild;

                    if (left.TotalUnitCount > 0)
                    {
                        var leftDisSqr = left.CalculateMinSqrDistanceToPoint(p);
                        if (leftDisSqr <= extrRadius) //只放入距离有可能在我附近的节点
                            _searchQ.Enqueue(left, leftDisSqr);
                    }
                    if (right.TotalUnitCount > 0)
                    {
                        var rightDisSqr = right.CalculateMinSqrDistanceToPoint(p);
                        if (rightDisSqr <= extrRadius)
                            _searchQ.Enqueue(right, rightDisSqr);
                    }
                }
                else
                {
                    //策划为了精准控制冲撞单位数，这里使用的矩形不带方向，并且与圆判断直接加上圆半径
                    foreach (var u in node.UnitSet)
                    {
                        if (u.IsHero())
                        {
                            FixInt2 pos1, pos2, pos3, pos4;
                            GetRectPoss(u.WorldPos, heroExtraWidth + radius, heroExtraHeight + radius,
                                out pos1, out pos2, out pos3, out pos4);
                            if (IsSimpleContain(pos1, pos2, pos3, pos4, p))
                            {
                                if (FilterFun != null && !FilterFun(u))
                                    continue;
                                result.Add(u.GetId());
                            }
                        }
                        else
                        {
                            var disSqr = (u.WorldPos - p).sqrMagnitude;
                            if (disSqr < radiusSqr)
                            {
                                if (FilterFun != null && !FilterFun(u))
                                    continue;
                                result.Add(u.GetId());
                            }
                        }
                    }
                }
            }
            _searchQ.Clear();
            //Utils.LogUtils.Log("FindUnitInRadius iter: {0}", iterCount);
        }

        //找到扇形内的单位

        //找到AABB内的单位

        //找到OBB内的单位

        //获取基于点水平矩形
        public void GetRectPoss(FixInt2 centerPos, FixInt width, FixInt height, out FixInt2 pos1, out FixInt2 pos2, out FixInt2 pos3, out FixInt2 pos4)
        {
            var halfW = width.Value / 2;
            var halfH = height.Value / 2;
            pos1 = centerPos + new FixInt2(halfW, halfH);
            pos2 = centerPos + new FixInt2(halfW, -halfH);
            pos3 = centerPos + new FixInt2(-halfW, -halfH);
            pos4 = centerPos + new FixInt2(-halfW, halfH);
        }
        public bool IsSimpleContain(FixInt2 mp1, FixInt2 mp2, FixInt2 mp3, FixInt2 mp4, FixInt2 p)
        {
            int minX = Math.Min(Math.Min(mp1.x, mp2.x), Math.Min(mp3.x,mp4.x));
            int maxX = Math.Max(Math.Max(mp1.x, mp2.x), Math.Max(mp3.x, mp4.x));
            int minY = Math.Min(Math.Min(mp1.y, mp2.y), Math.Min(mp3.y, mp4.y));
            int maxY = Math.Max(Math.Max(mp1.y, mp2.y), Math.Max(mp3.y, mp4.y));
            if (p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY)
                return true;
            return false;
        }
        // 得出矩形当前朝向4个点
        public void GetRectRotatePoss(FixInt2 centerPos, FixInt width, FixInt height, FixInt rad, out FixInt2 pos1, out FixInt2 pos2, out FixInt2 pos3, out FixInt2 pos4)
        {
            var halfW = width.Value / 2;
            var halfH = height.Value / 2;
            FixInt2 p1 = centerPos + new FixInt2(halfW, halfH);
            FixInt2 p2 = centerPos + new FixInt2(halfW, -halfH);
            FixInt2 p3 = centerPos + new FixInt2(-halfW, -halfH);
            FixInt2 p4 = centerPos + new FixInt2(-halfW, halfH);
            int sinrad = FixMath.Sin(rad.Value);
            int cosrad = FixMath.Cos(rad.Value);
            pos1 = new FixInt2((p1.x- centerPos.x)* cosrad/ FixInt2.Scale - (p1.y - centerPos.y) * sinrad / FixInt2.Scale + centerPos.x,
                (p1.x - centerPos.x) * sinrad / FixInt2.Scale + (p1.y - centerPos.y) * cosrad / FixInt2.Scale + centerPos.y);
            pos2 = new FixInt2((p2.x - centerPos.x) * cosrad / FixInt2.Scale - (p2.y - centerPos.y) * sinrad / FixInt2.Scale + centerPos.x,
                (p2.x - centerPos.x) * sinrad / FixInt2.Scale + (p2.y - centerPos.y) * cosrad / FixInt2.Scale + centerPos.y);
            pos3 = new FixInt2((p3.x - centerPos.x) * cosrad / FixInt2.Scale - (p3.y - centerPos.y) * sinrad / FixInt2.Scale + centerPos.x,
                (p3.x - centerPos.x) * sinrad / FixInt2.Scale + (p3.y - centerPos.y) * cosrad / FixInt2.Scale + centerPos.y);
            pos4 = new FixInt2((p4.x - centerPos.x) * cosrad / FixInt2.Scale - (p4.y - centerPos.y) * sinrad / FixInt2.Scale + centerPos.x,
                (p4.x - centerPos.x) * sinrad / FixInt2.Scale + (p4.y - centerPos.y) * cosrad / FixInt2.Scale + centerPos.y);
        }


        //矩形判断
        //只需要判断：|P2P|×|P1P2|*|P3P|×|P3P4|<=0 And |P1P|×|P1P4|*|P2P|×|P2P3|<=0

        public bool IsContain(FixInt2 mp1, FixInt2 mp2, FixInt2 mp3, FixInt2 mp4, FixInt2 p)
        {
            if (Multiply(p, mp1, mp2) * Multiply(p, mp4, mp3) <= 0
                && Multiply(p, mp4, mp1) * Multiply(p, mp3, mp2) <= 0)
                return true;
            return false;
        }
        // 计算叉乘 |P0P1| × |P0P2|
        private double Multiply(FixInt2 p1, FixInt2 p2, FixInt2 p0)
        {
            return ((p1.x - p0.x) * (p2.y - p0.y) - (p2.y - p0.y) * (p1.y - p0.y));
        }


        #endregion
    }

}