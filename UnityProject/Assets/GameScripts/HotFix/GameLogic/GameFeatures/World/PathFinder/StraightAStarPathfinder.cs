using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using XLua;

namespace World.PathFinder
{
    [LuaCallCSharp]
    public class StraightAStarPathfinder
    {
        public ArrayList path = new();
        public int pathDistance = -1;
        public FindResult findResult = FindResult.DO_NOT_FIND;
        public int sIndex;
        public int eIndex;
        public int[] eQr;
        public WorldZoneMapData mapData;
        public FindPathNodeNote endNote;
        public PriorityQueue<FindPathNodeNote> openList = new();
        public HashSet<int> closeSet = new();
        public HashSet<int> cannotGoStraightSet = new();
        public void Init(WorldZoneMapData data)
        {
            this.mapData = data;
        }

        public ArrayList ToFindPathByQr(int q, int r, int q1, int r1)
        {
            try
            {
                var xy1 = WorldCoordinateConvert.qrToXy(q, r);
                var xy2 = WorldCoordinateConvert.qrToXy(q1, r1);
                var temp = ToFindPathByXy(xy1[0],xy1[1],xy2[0],xy2[1]);
                return temp;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Clear()
        {
            path.Clear();
            pathDistance = -1;
            findResult = FindResult.DO_NOT_FIND;
            sIndex = 0;
            eIndex = 0;
            eQr = null;
            endNote = null;
            openList.Clear();
            closeSet.Clear();
            cannotGoStraightSet.Clear();
        }
        public ArrayList ToFindPathByXy(int x, int y, int x1, int y1)
        {
            Clear();
            this.sIndex = mapData.XyToIndex(x,y);
            this.eIndex =mapData.XyToIndex(x1,y1);
            FindPath();
            if (findResult == FindResult.SUCCESS)
            {
                return path;
            }
            else
            {
                //失败了
                Log.Error("寻路失败");
            }
            return null;
        }
        public void FindPath()
        {
            if ((mapData.grids[sIndex] == (byte)GridType.DISABLE) || (mapData.grids[eIndex] == (byte)GridType.DISABLE))
            {
                this.findResult = FindResult.POS_ERROR;
                return;
            }

            var sXy = mapData.IndexToXy(sIndex);
            var eXy = mapData.IndexToXy(eIndex);
            if (!mapData.XyInMap(sXy) || !mapData.XyInMap(eXy))
            {
                this.findResult = FindResult.POS_ERROR;
                return;
            }

            var sQr = WorldCoordinateConvert.xyToQr(sXy);
            this.eQr = WorldCoordinateConvert.xyToQr(eXy);
            FindPathNodeNote sNote = new FindPathNodeNote(sIndex, sQr)
            {
                // 起点，需要扫描所有邻居
                g = 0F,
                h = WorldCoordinateConvert.DistanceByQr(sQr, eQr)
            };
            sNote.f = sNote.g + sNote.h;
            openList.Enqueue(sNote);
            closeSet.Add(sNote.index);
            while (true)
            {
                FindPathNodeNote curNote = openList.Dequeue();
                if (curNote == null)
                {
                    findResult = FindResult.BLOCKING;
                    break;
                }

                TryGoStraight(curNote);
                if (findResult == FindResult.SUCCESS)
                {
                    break;
                }
            }

            BuildPath();
        }


        /**
        * 先从当前点直线走到终点，如果可达即寻路成功；不可达则将射线的最后一个点的邻居们加入openList
        *
        * @param curNote
        */
        public void TryGoStraight(FindPathNodeNote curNote)
        {
            var curIntQr = WorldCoordinateConvert.xyToQr(mapData.IndexToXy(curNote.index));
            int distance = WorldCoordinateConvert.DistanceByQr(curIntQr, eQr);
            float[] curQr = { curIntQr[0], curIntQr[1], -curIntQr[0] - curIntQr[1] };
            float[] diffQrs =
                { eQr[0] - curIntQr[0], eQr[1] - curIntQr[1], -eQr[0] - eQr[1] + curIntQr[0] + curIntQr[1] };
            int lastCheckIndex = curNote.index;
            int[] lastCheckQr = curIntQr;
            int nowCheckDistance = 1;
            for (; nowCheckDistance <= distance; nowCheckDistance++)
            {
                int[] nowCheckQrs = WorldCoordinateConvert.VectorCubeRound(curQr, diffQrs, distance
                    , nowCheckDistance);
                var nowCheckXy = WorldCoordinateConvert.qrToXy(nowCheckQrs);
                if (!mapData.XyInMap(nowCheckXy))
                {
                    break;
                }

                var nowCheckIndex = mapData.XyToIndex(nowCheckXy);
                if (cannotGoStraightSet.Contains(nowCheckIndex))
                {
                    break;
                }

                if (mapData.grids[nowCheckIndex] == (byte)GridType.DISABLE)
                {
                    break;
                }

                lastCheckIndex = nowCheckIndex;
                lastCheckQr = nowCheckQrs;
            }

            if (lastCheckIndex != eIndex)
            {
                cannotGoStraightSet.Add(lastCheckIndex);
            }

            int lastCheckDistance = nowCheckDistance - 1;
            FindPathNodeNote lastCheckNote;
            if (lastCheckIndex == curNote.index)
            {
                lastCheckNote = curNote;
            }
            else
            {
                lastCheckNote = new FindPathNodeNote(lastCheckIndex, lastCheckQr)
                {
                    parent = curNote,
                    g = curNote.g + lastCheckDistance,
                    h = WorldCoordinateConvert.DistanceByQr(lastCheckQr, eQr)
                };
                lastCheckNote.f = lastCheckNote.g + lastCheckNote.h;
            }

            if (lastCheckIndex == eIndex)
            {
                findResult = FindResult.SUCCESS;
                endNote = lastCheckNote;
                pathDistance = (int)endNote.g;
                return;
            }

            AddNeighbors(lastCheckNote);
        }

        public void AddNeighbors(FindPathNodeNote curNote)
        {
            var cubeRings = WorldCoordinateConvert.CubeRings(curNote.qr, 1, 1);
            foreach (var neighborQr in cubeRings)
            {
                var neighborXy = WorldCoordinateConvert.qrToXy(neighborQr);
                if (!mapData.XyInMap(neighborXy))
                {
                    continue;
                }

                var neighborIndex = mapData.XyToIndex(neighborXy);
                if (mapData.grids[neighborIndex] == (byte)GridType.DISABLE)
                {
                    continue;
                }

                var newAdd = closeSet.Add(neighborIndex);
                if (!newAdd)
                {
                    continue;
                }

                FindPathNodeNote neighborNote = new FindPathNodeNote(neighborIndex, neighborQr)
                {
                    parent = curNote,
                    g = curNote.g + 1,
                    h = WorldCoordinateConvert.DistanceByQr(neighborQr, eQr)
                };
                neighborNote.f = neighborNote.g + neighborNote.h;
                openList.Enqueue(neighborNote);
            }
        }

        public void BuildPath()
        {
            if (findResult != FindResult.SUCCESS)
            {
                return;
            }

            endNote.BuildPath(path);
        }
    }
}
