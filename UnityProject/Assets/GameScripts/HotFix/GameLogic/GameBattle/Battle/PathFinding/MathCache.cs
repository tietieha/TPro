using System;
 
using System.Collections.Generic;
using System.Collections;
using FixPoint;

/// <summary>
/// 缓存数学计算结果，加速运算
/// 1，一定半径内的格子坐标
/// 2，任意两个格子中心间的距离
/// </summary>
namespace M.PathFinding
{
    public static class MathCache
    {
        private static readonly int _maxExtend = 8;
        private static List<List<Integer2>> _gridInCircleData;
        private static readonly int _maxGridDistance = 120;
        private static long[,] _gridDistanceData;

        private static bool _isInited = false;

        public static void Init()
        {
            if(_isInited)
                return;
            Init_GridInCircle();
            Init_GridDistance();
        }

        //取得一定半径内的所有格子
        public static List<Integer2> GetGridInCircle(int extend)
        {
            Utils.Assert(extend < _gridInCircleData.Count && extend >= 0);
            return _gridInCircleData[extend];
        }

        //获取格子中心的距离
        public static long GetGridDistance(int xDiff, int yDiff)
        {
            xDiff = xDiff > 0 ? xDiff : -xDiff;//Math.Abs(xDiff);
            yDiff = yDiff > 0 ? yDiff : -yDiff;//Math.Abs(yDiff);
            //Utils.Assert(xDiff < _maxGridDistance && yDiff < _maxGridDistance);
            if (xDiff >= _maxGridDistance || yDiff >= _maxGridDistance)
                return Utils.MAX_DISTANCE;
            return _gridDistanceData[xDiff, yDiff];
        }

        public static int GetGridDistance(Integer2 a, Integer2 b)
        {
            return (int) GetGridDistance(a.x - b.x, a.y - b.y);
        }


        private static void Init_GridDistance()
        {
            if (_gridDistanceData != null)
            {
                return;
            }
            else
            {
                _gridDistanceData = new long[_maxGridDistance, _maxGridDistance];
                for (int x = 0; x < _maxGridDistance; x++)
                {
                    for (int y = 0; y < _maxGridDistance; y++)
                    {
                        _gridDistanceData[x, y] = FixMath.Sqrt((long)x * FixInt2.Scale * (long)x * FixInt2.Scale + (long)y * FixInt2.Scale * (long)y * FixInt2.Scale);
                    }
                }
            }
        }


        private static void Init_GridInCircle()
        {
            if (_gridInCircleData!=null)
                return;

            _gridInCircleData = new List<List<Integer2>>(_maxExtend + 1);
            //0
            {
                var l = new List<Integer2>(1)
                {
                    new Integer2(0, 0)
                };
                _gridInCircleData.Add(l);
            }
            //1
            {
                var l = new List<Integer2>(9);
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        l.Add(new Integer2(x, y));
                    }
                }
                _gridInCircleData.Add(l);
            }
            //2+，计算哪些点在圆里面，不用正方形了
            for (int e = 2; e <= _maxExtend; e += 1)
            {
                var l = new List<Integer2>();
                int threshold = 100;
                for (int x = -e; x <= e; x++)
                {
                    for (int y = -e; y <= e; y++)
                    {
                        int dis = FixMath.Sqrt((long)x * FixInt2.Scale * (long)x * FixInt2.Scale + (long)y * FixInt2.Scale * (long)y * FixInt2.Scale);
                        if (dis < e*FixInt2.Scale + threshold)
                        {
                            l.Add(new Integer2(x, y));
                        } 
                    }
                }
                _gridInCircleData.Add(l);
            }

        }



    }
}