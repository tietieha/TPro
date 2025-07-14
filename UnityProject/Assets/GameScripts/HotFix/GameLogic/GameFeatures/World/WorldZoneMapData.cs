using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using XLua;

namespace World
{
    [LuaCallCSharp]
    public class WorldZoneMapData
    {
        public int width;
        public int height;
        public int scale;
        public int version;
        public float hexRadius;

        public Dictionary<int, WorldZoneData> zones;
        public short[] grid2zone;
        public byte[] grids;

        public WorldBusinessEdgeData[] businessEdgeDatas;

        public void Load(BinaryReader br)
        {
            version = br.ReadInt32();
            width = br.ReadInt32();
            height = br.ReadInt32();
            hexRadius = br.ReadSingle();

            int zoneCount = br.ReadInt32();
            if (zones == null)
            {
                zones = new Dictionary<int, WorldZoneData>(zoneCount);
                grid2zone = new short[width * height];
                grids = new byte[width * height];
            }

            for (int i = 0; i < zoneCount; i++)
            {
                var zoneData = new WorldZoneData();
                zoneData.Load(br);
                zones.Add(zoneData.ZoneId, zoneData);
            }

            int begin;
            short len, sdx;
            var count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                begin = br.ReadInt32();
                len = br.ReadInt16();
                sdx = br.ReadInt16();
                for (int k = 0; k < len; k++)
                {
                    grid2zone[begin + k] = sdx;
                }
            }

            int moveType;
            count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                begin = br.ReadInt32();
                len = br.ReadInt16();
                moveType = br.ReadInt32();
                for (int k = 0; k < len; k++)
                {
                    grids[begin + k] = (byte)moveType;
                }
            }
            
            // if(version >= 2)
            // {
	           //  // 商圈mesh
	           //  count = br.ReadInt32();
	           //  businessEdgeDatas = new WorldBusinessEdgeData[count];
	           //  for (int i = 0; i < count; i++)
	           //  {
		          //   var businessEdgeData = new WorldBusinessEdgeData();
		          //   businessEdgeData.Load(br);
		          //   businessEdgeData.index = i;
		          //   businessEdgeDatas[i] = businessEdgeData;
	           //  }
            // }
        }

        public bool InBlockingArea(int q, int r)
        {
            var xy = WorldCoordinateConvert.qrToXy(q, r);
            return InBlockingAreaByXy(xy[0], xy[1]);
        }

        public byte GetGridType(int q, int r)
        {
            var xy = WorldCoordinateConvert.qrToXy(q, r);
            var index = xy[1] * width + xy[0];
            if (index < 0 || index >= grids.Length)
            {
                return 0;
            }

            return grids[index];
        }
        
        public bool InBlockingAreaByXy(int x, int y)
        {
            var index = y * width + x;
            if (index < 0 || index >= grids.Length)
            {
                return true;
            }

            return grids[index] == (byte)GridType.DISABLE;
        }

        public short GetGridZone(int q, int r)
        {
            var qr = WorldCoordinateConvert.qrToXy(q, r);
            var index = qr[1] * width + qr[0];
            if (index < 0 || index >= grid2zone.Length)
            {
                return -1;
            }

            return grid2zone[index];
        }

        public WorldZoneData GetGridZoneByIndex(int index)
        {
            if (index < 0 || index >= grid2zone.Length)
            {
                return null;
            }
            return zones.TryGetValue(grid2zone[index], out var zone) ? zone : null;
        }


        public int GetPortIndex(int zoneId)
        {
            if (zones.ContainsKey(zoneId))
            {
                return zones[zoneId].PortIndex;
            }
            
            return -1;
        }
        
        //目标点是否在驻港点
        // public bool IsStationAtPort(int q, int r)
        // {
        //     var zId = GetGridZone(q, r);
        //
        //     if (zones.ContainsKey(zId))
        //     {
        //         var qr = WorldCoordinateConvert.qrToXy(q, r);
        //         var index = qr[1] * width + qr[0];
        //
        //         return zones[zId].NormalStation.Contains(index) || zones[zId].VipStation.Contains(index);
        //     }
        //
        //     return false;
        // }

        //目标点是否在vip驻港点
        // public bool IsStationAtPortVip(int q, int r)
        // {
        //     var zId = GetGridZone(q, r);
        //
        //     if (zones.ContainsKey(zId))
        //     {
        //         var qr = WorldCoordinateConvert.qrToXy(q, r);
        //         var index = qr[1] * width + qr[0];
        //
        //         return zones[zId].VipStation.Contains(index);
        //     }
        //
        //     return false;
        // }

        //返回vip驻港点
        // public int[] GetVipStation(int zoneId)
        // {
        //     if (zones.ContainsKey(zoneId))
        //     {
        //         return zones[zoneId].VipStation.ToArray();
        //     }
        //
        //     return null;
        // }

        //返回普通驻港点
        // public int[] GetNormalStation(int zoneId)
        // {
        //     if (zones.ContainsKey(zoneId))
        //     {
        //         return zones[zoneId].NormalStation.ToArray();
        //     }
        //
        //     return null;
        // }

        public int[] IndexToXy(int index)
        {
            var y = index / width;
            var x = index - y * width;
            return new[] { x, y };
        }

        public int XyToIndex(int[] xy)
        {
            return XyToIndex(xy[0], xy[1]);
        }

        public int QrToIndex(int[] qr)
        {
            return QrToIndex(qr[0], qr[1]);
        }

        public int QrToIndex(int q, int r)
        {
            return XyToIndex(WorldCoordinateConvert.qrToXy(q, r));
        }

        public int XyToIndex(int x, int y)
        {
            return x + y * width;
        }

        public int DistanceByIndex(int leftIndex, int rightIndex)
        {
            return WorldCoordinateConvert.DistanceByXy(IndexToXy(leftIndex), IndexToXy(rightIndex));
        }

        public bool XyInMap(int[] xy)
        {
            if (xy[0] < 0 || xy[0] >= width ||
                xy[1] < 0 || xy[1] >= height)
            {
                return false;
            }

            return true;
        }

        public bool CouldStraightGoByQr(int[] sQr, int[] eQr)
        {
            var distance = WorldCoordinateConvert.DistanceByQr(sQr, eQr);
            float[] sFloatQr = { sQr[0], sQr[1], sQr[2] };
            float[] diffQrs = { eQr[0] - sQr[0], eQr[1] - sQr[1], -eQr[0] - eQr[1] + sQr[0] + sQr[1] };
            for (int i = 0; i <= distance; i++)
            {
                int[] qrs = WorldCoordinateConvert.VectorCubeRound(sFloatQr, diffQrs, distance
                    , i);
                var xy = WorldCoordinateConvert.qrToXy(qrs);
                if (!XyInMap(xy))
                {
                    return false;
                }
                if(InBlockingAreaByXy(xy[0],xy[1]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

public enum GridType : int
{
    DISABLE = 0, // 不可到达
    NORMAL, // 常规
    SPEEDUP, // 加速区
    SPEEDDOWN // 减速区
}
