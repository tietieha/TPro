using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{

    //寻路状态
    public class PathState
    {
        private Unit _unit = null;
        //等待哪条路径返回
        private int _waitingPathId = -1;
        //当前路点
        private readonly Queue<FixInt2> _path = new Queue<FixInt2>(100);
        //当前路径的存活帧数
        private int _pathLifeFrame = 0;

        private bool _foundPath = false;

        //返回的路径的PathRequest
        private PathRequest _returnedPathRequest = null;

        public PathState(Unit unit)
        {
            _unit = unit;
        }


        public void Reset()
        {
            _waitingPathId = -1;
            _pathLifeFrame = 0;
            _foundPath = false;
            _returnedPathRequest = null;
            _path.Clear();
        }

        public int GetCurrentWaitingPathId() { return _waitingPathId; }
        public void SetCurrentWaitingPathId(int id) { _waitingPathId = id; }

        public Queue<FixInt2> GetCurrentPath() { return _path; }
        public int GetPathLife() { return _pathLifeFrame; }

        public bool IsFoundPath() { return _foundPath; }

        public void CancelPathFinding()
        {
            _waitingPathId = -1;
            if (_returnedPathRequest != null)
            {
                _unit.GetMap().ReturnPathRequestToCache(_returnedPathRequest);
                _returnedPathRequest = null;
            }
        }

        public void ClearPath()
        {
            _path.Clear();
            if (_returnedPathRequest != null)
            {
                _unit.GetMap().ReturnPathRequestToCache(_returnedPathRequest);
                _returnedPathRequest = null;
            }
        }

        public void TickPathLife()
        {
            _pathLifeFrame++;
        }

        //寻路结束，返回反向路径
        public void OnPathFindingFinished(PathRequest req, List<FixInt2> pathResultReverse, bool foundPath)
        {
            Utils.Assert(req.GetUID() == _waitingPathId);
            //归还老的req
            if (_returnedPathRequest != null)
            {
                _unit.GetMap().ReturnPathRequestToCache(_returnedPathRequest);
                _returnedPathRequest = null;
            }

            _foundPath = foundPath;
            _returnedPathRequest = req;
            _waitingPathId = -1;
            _path.Clear();

            for (int i = pathResultReverse.Count - 2; i >= 0; i--) //最后一个点是当前位置，不用加入
            {
                _path.Enqueue(pathResultReverse[i]);
            }
            _pathLifeFrame = 0;

            if (Utils.IsDebug() && Utils.IsLogUnit(_unit.GetId()))
            {
                string pathStr = "";
                foreach (var p in _path)
                {
                    pathStr += $"[{p.x.ToString()}, {p.y.ToString()}] ";
                }

                _unit.Log("ResultPath: {0}", pathStr);
            }

            _unit.GetCurrentStateObject().OnPathFindFinished(this);

            if (_unit.GetMap().GetMapDebugger() != null && Utils.IsDebug())
            {
                // var posList = new List<FixInt2>();
                // posList.Add(_unit.WorldPos);
                //
                // foreach (var p in _path)
                // {
                //     posList.Add(p);
                // }

                // var unitSide = _unit.GetSide();
                // _unit.GetMap().GetMapDebugger().GeneratePath(unitSide, posList);
            }
        }
    }

    public partial class Unit
    {
        public PathState PathState { set; get; } = null;

        // //等待哪条路径返回
        // private int _waitingPathId = -1;
        // //当前路点
        // private readonly Queue<FixInt2> _path = new Queue<FixInt2>(100);
        // //当前路径的存活帧数
        // private int _pathLifeFrame = 0;

        public void SendPathFindingRequest_OneUnit(Unit targetUnit, int stopDis, int maxNodeTest, ERequestPriority priority)
        {
            var r = GetMap().GetPathRequestFromCache();
            r.Init_oneTarget(this, targetUnit, stopDis, maxNodeTest, priority);
            //new PathRequest(this, targetUnit, stopDis, priority);
            _map.PushPathRequest(r);
            PathState.SetCurrentWaitingPathId(r.GetUID());
        }

        public void SendPathFindingRequest_Team(int teamId, int stopDis, int maxNodeTest, ERequestPriority priority)
        {
            var r = GetMap().GetPathRequestFromCache();
            r.Init_team(this, teamId, stopDis, maxNodeTest, priority);
            _map.PushPathRequest(r);
            PathState.SetCurrentWaitingPathId(r.GetUID());
        }

        public void SendPathFindingRequest_Position(FixInt2 targetPos, int stopDis, int maxNodeTest, ERequestPriority priority)
        {
            var r = GetMap().GetPathRequestFromCache();
            r.Init_targetPos(this, targetPos, stopDis, maxNodeTest, priority);
            //new PathRequest(this, targetPos, stopDis, priority);
            _map.PushPathRequest(r);
            PathState.SetCurrentWaitingPathId(r.GetUID());
        }
    }
}