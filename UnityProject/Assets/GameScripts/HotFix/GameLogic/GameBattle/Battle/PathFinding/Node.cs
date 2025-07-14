using Priority_Queue;

using System.Collections.Generic;
using FixPoint;

/// <summary>
/// 每个格子是一个Node
/// </summary>
namespace M.PathFinding
{
    /// <summary>
    /// 结点状态
    /// </summary>
    public enum ENodeState
    {
        New = 0,    //新结点
        Open,       //接下来要分析的
        Close       //已经探查过的
    }

    public class Node : FastPriorityQueueNode
    {
        //地图
        private BattleMap _map = null;
        //位置
        public Integer2 _pos;

        private int _index = -1;

        //各种不同类型单位可以看到的占用层
        public int[] _occupyLayerForUnit = new int[(int)EUnitType.Count];
        public int[] _layerState = new int[(int)ELayerType.Count];

        //寻路计算中用到的属性。每次寻路结束后会被清空
        //累积路径代价
        private int _pathAccumCost = -1;
        //前一个路径点
        private Node _pathPreNode = null;
        //距离终点的距离
        private long _disToEnd = Utils.MAX_DISTANCE;
        //状态
        private ENodeState _state = ENodeState.New;

        private int _obstacledCount = 0;

        public int PathAccumCost { get => _pathAccumCost; set => _pathAccumCost = value; }
        public Node PathPreNode { get => _pathPreNode; set => _pathPreNode = value; }
        public ENodeState State { get => _state; set => _state = value; }
        public long DisToEnd { get => _disToEnd; set => _disToEnd = value; }

        public int Index { get { return _index; } set { _index = value; } }

        public Node()
        {
            Reset();
        }

        public void InitWithData(BattleMap map, int x, int y)
        {
            _map = map;
            _pos.x = x;
            _pos.y = y;
        }

        public void Reset()
        {
            _map = null;
            _pos.x = 0;
            _pos.y = 0;
            _index = -1;
            for (int i = 0; i < _occupyLayerForUnit.Length; i++)
                _occupyLayerForUnit[i] = 0;

            for (int i = 0; i < _layerState.Length; i++)
                _layerState[i] = 0;

            ClearPathFindingData();
            _map = null;
        }

        public void ClearPathFindingData()
        {
            _pathAccumCost = -1;
            _pathPreNode = null;
            _disToEnd = Utils.MAX_DISTANCE;
            _state = ENodeState.New;
        }

        public BattleMap GetMap()
        {
            return _map;
        }

        public Integer2 GetPos()
        {
            return _pos;
        }
        public int GetX() { return _pos.x; }
        public int GetY() { return _pos.y; }


        public FixInt2 GetCenterWorldPos()
        {
            return _map.GetGridCenter(_pos.x, _pos.y);
        }



        //增加占用计数
        public void AddOccupied(EUnitType unitType)
        {
            _occupyLayerForUnit[(int)unitType]++;
            _map.NodeOccupyChanged(this);
        }

        //减少占用计数
        public void RemoveOccupied(EUnitType unitType)
        {
            //if(_occupyLayerForUnit[(int)unitType] > 0)
            //为啥不用判断，因为在ProcessOneRequest移除时，并没有判断有没有占用当前层
            _occupyLayerForUnit[(int)unitType]--;

            //TODO
            //Utils.Assert(_occupyLayerForUnit[(int)unitType] >= 0);
            _map.NodeOccupyChanged(this);
        }

        //是否被占用（某一层）
        public bool IsOccupied(EUnitType unitType)
        {
            return _occupyLayerForUnit[(int)unitType] > 0;
        }

        public bool IsOccupiedOrLayer(EUnitType unitType, ELayerType layerType)
        {
            return (_occupyLayerForUnit[(int)unitType] > 0 || _layerState[(int)layerType] > 0);
        }

        public bool IsObstacled()
        {
            return _layerState[(int)ELayerType.Normal] > 0 || _layerState[(int)ELayerType.Height] >0;
        }

        // 是否被占用多于指定值
        public bool IsOccupiedMoreValue(EUnitType unitType, int value)
        {
            return _occupyLayerForUnit[(int)unitType] > value;
        }

        //地图层增加占用计数
        public void AddLayerOccupied(ELayerType layerType)
        {
            _layerState[(int)layerType]++;
            //_map.NodeOccupyChanged(this);
        }

        //洼地减少占用计数
        public void RemoveLayerOccupied(ELayerType layerType)
        {
            _layerState[(int)layerType]--;
            //_map.NodeOccupyChanged(this);
        }

        public bool IsCanMove(EUnitState state)
        {
            if (Unit.StateLayoutArray[(int)state, (int)ELayerType.Low] < 0 && _layerState[(int)ELayerType.Low]>0)
            {
                return false;
            }
            if (Unit.StateLayoutArray[(int)state, (int)ELayerType.Normal] < 0 && _layerState[(int)ELayerType.Normal] > 0)
            {
                return false;
            }
            if (Unit.StateLayoutArray[(int)state, (int)ELayerType.Height] < 0 && _layerState[(int)ELayerType.Height] > 0)
            {
                return false;
            }
            return true;
        }


    }
}