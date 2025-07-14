/* BattleEntityPoolManager.cs
 * caijunjie@topjoy.com
 * 2020/6/17 10:29:16
 * desc
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using FixPoint;
using XLua;

namespace M.PathFinding
{
    [LuaCallCSharp]
    public class BattleEntityPoolManager
    {
        private int _pathRequestUid = 0;
        private Queue<Unit> _unitPool;
        private Queue<Node> _nodePool;
        private Queue<Team> _teamPool;
        private Queue<BSPTreeNode> _bspTreeNodePool;
        private Queue<BSPTree> _bspTreePool;
        private Queue<PathRequest> _pathRequestPool;
        private Queue<FastPriorityQueue<Node>> _fastPriorityQueuesPool;
        private Queue<List<Node>> _processedNodeNearestPool;
        private Queue<List<FixInt2>> _targetWorldPosPool;// = new List<FixInt2>(20);
        private Queue<List<Integer2>> _targetGridPosPool;// = new List<Integer2>(20);
        private Queue<List<FixInt2>> _resultPathPool;// = new List<FixInt2>(300);
        private Queue<List<FixInt2>> _resultPathSimplifiedPool;// = new List<FixInt2>(300);
        private Queue<List<Unit>> _restoreGridOccupyUnitListPool;// = new List<Unit>(20);

        public BattleEntityPoolManager()
        {
            _unitPool = new Queue<Unit>();
            _nodePool = new Queue<Node>();
            _teamPool = new Queue<Team>();
            _bspTreeNodePool = new Queue<BSPTreeNode>();
            _bspTreePool = new Queue<BSPTree>();
            _pathRequestPool = new Queue<PathRequest>();
            _fastPriorityQueuesPool = new Queue<FastPriorityQueue<Node>>();
            _processedNodeNearestPool = new Queue<List<Node>>();
            _targetWorldPosPool = new Queue<List<FixInt2>>();
            _targetGridPosPool = new Queue<List<Integer2>>();
            _resultPathPool = new Queue<List<FixInt2>>();
            _resultPathSimplifiedPool = new Queue<List<FixInt2>>();
            _restoreGridOccupyUnitListPool = new Queue<List<Unit>>();

            //for (var i = 0; i < 2; i++)
            //{
            //    _bspTreePool.Enqueue(new BSPTree()); //todo 实现
            //}

            for (var i=0; i<400; i++)
            {
                _unitPool.Enqueue(new Unit());
            }

            for (var i = 0; i < 600; i++)
            {
                _bspTreeNodePool.Enqueue(new BSPTreeNode());
            }

            for (var i = 0; i < 4200; i++)
            {
                _nodePool.Enqueue(new Node());
            }

            for (var i = 0; i < 16; i++)
            {
                _teamPool.Enqueue(new Team());
            }

            for (var i = 0; i < 500; i++)
            {
                _pathRequestPool.Enqueue(new PathRequest(this));
            }

            _fastPriorityQueuesPool.Enqueue(new FastPriorityQueue<Node>(300));
            _processedNodeNearestPool.Enqueue(new List<Node>(300));

            _targetWorldPosPool.Enqueue(new List<FixInt2>(20));
            _targetGridPosPool.Enqueue(new List<Integer2>(20));
            _resultPathPool.Enqueue(new List<FixInt2>(300));
            _resultPathSimplifiedPool.Enqueue(new List<FixInt2>(300));
            _restoreGridOccupyUnitListPool.Enqueue(new List<Unit>(20));
        }

        public int GeneratePathRequestUID()
        {
            return _pathRequestUid++;
        }

        public void RestPathRequestUID()
        {
            _pathRequestUid = 0;
        }

        public Unit getUnitObj()
        {
            if (_unitPool.Count > 0)
                return _unitPool.Dequeue();
            else
                return new Unit();
        }

        public void backUnitObj(Unit u)
        {
            u.Reset();
            _unitPool.Enqueue(u);
        }

        public Node getNodeObj()
        {
            if (_nodePool.Count > 0)
                return _nodePool.Dequeue();
            else
                return new Node();
        }

        public void backNodeObj(Node n)
        {
            n.Reset();
            _nodePool.Enqueue(n);
        }

        public Team getTeamObj()
        {
            if (_teamPool.Count > 0)
                return _teamPool.Dequeue();
            else
                return new Team();
        }

        public void backTeamObj(Team t)
        {
            t.Reset();
            _teamPool.Enqueue(t);
        }

        public BSPTreeNode getBSPTreeNodeObj()
        {
            if (_bspTreeNodePool.Count > 0)
                return _bspTreeNodePool.Dequeue();
            else
                return new BSPTreeNode();
        }

        public void backBSPTreeNodeObj(BSPTreeNode n)
        {
            n.Reset();
            _bspTreeNodePool.Enqueue(n);
        }

        public PathRequest getPathRequestObj()
        {
            if (_pathRequestPool.Count > 0)
                return _pathRequestPool.Dequeue();
            else
                return new PathRequest(this);
        }

        public void backPathRequestObj(PathRequest t)
        {
            _pathRequestPool.Enqueue(t);
        }

        public FastPriorityQueue<Node> getFastPriorityQueue()
        {
            if (_fastPriorityQueuesPool.Count > 0)
                return _fastPriorityQueuesPool.Dequeue();
            else
                return new FastPriorityQueue<Node>(300);
        }

        public void backFastPriorityQueue(FastPriorityQueue<Node> t)
        {
            t.Clear();
            _fastPriorityQueuesPool.Enqueue(t);
        }

        public List<Node> getProcessedNodeNearest()
        {
            if (_processedNodeNearestPool.Count > 0)
                return _processedNodeNearestPool.Dequeue();
            else
                return new List<Node>(300);
        }

        public void backProcessedNodeNearest(List<Node> t)
        {
            t.Clear();
            _processedNodeNearestPool.Enqueue(t);
        }

        public List<FixInt2> getTargetWorldPos()
        {
            if (_targetWorldPosPool.Count > 0)
                return _targetWorldPosPool.Dequeue();
            else
                return new List<FixInt2>(20);
        }

        public void backTargetWorldPos(List<FixInt2> t)
        {
            t.Clear();
            _targetWorldPosPool.Enqueue(t);
        }
        public List<Integer2> getTargetGridPos()
        {
            if (_targetGridPosPool.Count > 0)
                return _targetGridPosPool.Dequeue();
            else
                return new List<Integer2>(20);
        }

        public void backTargetGridPos(List<Integer2> t)
        {
            t.Clear();
            _targetGridPosPool.Enqueue(t);
        }

        public List<FixInt2> getResultPath()
        {
            if (_resultPathPool.Count > 0)
                return _resultPathPool.Dequeue();
            else
                return new List<FixInt2>(300);
        }

        public void backResultPath(List<FixInt2> t)
        {
            t.Clear();
            _resultPathPool.Enqueue(t);
        }

        public List<FixInt2> getResultPathSimplified()
        {
            if (_resultPathSimplifiedPool.Count > 0)
                return _resultPathSimplifiedPool.Dequeue();
            else
                return new List<FixInt2>(300);
        }

        public void backResultPathSimplified(List<FixInt2> t)
        {
            t.Clear();
            _resultPathSimplifiedPool.Enqueue(t);
        }

        public List<Unit> getRestoreGridOccupyUnitList()
        {
            if (_restoreGridOccupyUnitListPool.Count > 0)
                return _restoreGridOccupyUnitListPool.Dequeue();
            else
                return new List<Unit>(20);
        }

        public void backRestoreGridOccupyUnitList(List<Unit> t)
        {
            t.Clear();
            _restoreGridOccupyUnitListPool.Enqueue(t);
        }
    }
}
