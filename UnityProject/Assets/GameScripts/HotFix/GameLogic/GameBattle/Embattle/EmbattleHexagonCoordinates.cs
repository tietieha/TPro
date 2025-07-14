using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EmbattleHexagonCoordinates
{
    [SerializeField] private int x, z;
    public int X => x;
    public int Z => z;

    public int Y => -x - z;


    public EmbattleHexagonCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static EmbattleHexagonCoordinates FromOffsetCoordinates(int x, int z)
    {

        // offset to cube
        return new EmbattleHexagonCoordinates(x, z - (x - (x & 1)) / 2);
    }


    public static EmbattleHexagonCoordinates ToOffsetCoordinates(int x, int z)
    {
        // cube to offset
        return new EmbattleHexagonCoordinates(x, z + (x - (x & 1)) / 2);
    }

    public static EmbattleHexagonCoordinates FromPosition(Vector3 position)
    {
        position.z = -position.z;
        float x = 2f / 3f * position.x / EmbattleHexagonMetrics.GetOuterRadius();
        float z = (-1f / 3f * position.x + EmbattleHexagonMetrics.Sqrt3 / 3f * position.z) / EmbattleHexagonMetrics.GetOuterRadius();
        float y = -x - z;

        Vector3Int cubePosition = EmbattleHexagonUtils.Cube_round(new Vector3(x, y, z));
        Vector2Int offsetPosition = EmbattleHexagonUtils.CubeToOffset(cubePosition);
        return new EmbattleHexagonCoordinates(offsetPosition.x, offsetPosition.y);
    }
    public static Vector2Int WorldPosToXy(Vector3 position)
    {
        position.z = -position.z;
        float x = 2f / 3f * position.x / EmbattleHexagonMetrics.GetWorldGridRadius();
        float z = (-1f / 3f * position.x + EmbattleHexagonMetrics.Sqrt3 / 3f * position.z) / EmbattleHexagonMetrics.GetWorldGridRadius();
        float y = -x - z;

        Vector3Int cubePosition = EmbattleHexagonUtils.Cube_round(new Vector3(x, y, z));
        Vector2Int offsetPosition = EmbattleHexagonUtils.CubeToOffset(cubePosition);
        return new Vector2Int(offsetPosition.x, offsetPosition.y);
    }
    public static Vector3 ToPosition(EmbattleHexagonCoordinates coordinates)
    {
        float x = EmbattleHexagonMetrics.GetOuterRadius() * 3.0f/2.0f * coordinates.x;
        float y = EmbattleHexagonMetrics.GetOuterRadius() * (EmbattleHexagonMetrics.Sqrt3 / 2.0f * coordinates.x + EmbattleHexagonMetrics.Sqrt3 * coordinates.z);
        return new Vector3(x, 0, y);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}

/// <summary>
/// 全部搬入lua中  提升性能
/// </summary>
public class WorldCoordinateConvert
{
     public static Vector3 QR_To_V3(float q,float r)
     {
         var x = EmbattleHexagonMetrics.GetWorldGridRadius() * 3.0f/2.0f * q;
         var z = EmbattleHexagonMetrics.GetWorldGridRadius() * (EmbattleHexagonMetrics.Sqrt3 / 2.0f * q + EmbattleHexagonMetrics.Sqrt3 * r) * -1;//最后的乘以-1是为了转换坐标系
         return new Vector3(x, 0, z);
     }
     
     public static Vector3 QR_To_V3_Oddr(float q, float r)
     {
         var x = EmbattleHexagonMetrics.GetWorldGridRadius() * (EmbattleHexagonMetrics.Sqrt3 * q + EmbattleHexagonMetrics.Sqrt3 / 2.0f * r);
         var z = EmbattleHexagonMetrics.GetWorldGridRadius() * (3.0f / 2.0f * r) * -1;//最后的乘以-1是为了转换坐标系
         return new Vector3(x, 0, z);
     }

     public static int[] V3_To_QR(Vector3 v3)
     {
         var xy = V3_To_XY(v3);
         return xyToQr(xy);
     }

     public static int[] V3_To_XY(Vector3 position)
     {
         // position.z *= -1;
         var q = 2f / 3f * position.x / EmbattleHexagonMetrics.GetWorldGridRadius();
         var r = (-1f / 3f * position.x + EmbattleHexagonMetrics.Sqrt3 / 3f * position.z) /
                 EmbattleHexagonMetrics.GetWorldGridRadius();
         var s = -q - r;
         var cuberound = CubeRound(q, r, s);
         var xy = EmbattleHexagonUtils.CubeToOffset(new Vector3Int(cuberound[0], cuberound[1], cuberound[2]));
         return new[] { xy[0], xy[1] };
     }

     public static int[] xyToQr(int[] xy)
    {
        return xyToQr(xy[0], xy[1]);
    }
     public static int[] xyToQr(int x, int y)
     {
         int q = x;
         int r = y - (x - (x & 1)) / 2;
         return new int[]{q, r};
     }
     
     public static int[] xyToQr_Oddr(int x, int y)
     {
         int q = x - (y - (y & 1)) / 2;
         int r = y;
         return new int[]{q, r};
     }

     public static int DistanceByQr(int[] leftQr, int[] rightQr)
     {
         int a = Math.Abs(leftQr[0] - rightQr[0]);
         int b = Math.Abs(leftQr[0] + leftQr[1] - rightQr[0] - rightQr[1]);
         int c = Math.Abs(leftQr[1] - rightQr[1]);
         return (a + b + c) / 2;
     }

     public static int[] VectorCubeRound(float[] startNodeQrs, float[] totalDiffQrs, long totalDiffLength, long targetLength) {
         float segmentRate = (float)targetLength / (float)totalDiffLength;
         float tmpQ = startNodeQrs[0] + totalDiffQrs[0] * segmentRate;
         float tmpR = startNodeQrs[1] + totalDiffQrs[1] * segmentRate;
         float tmpS = startNodeQrs[2] + totalDiffQrs[2] * segmentRate;
         return CubeRound(tmpQ, tmpR, tmpS);
     }

     public static int[] CubeRound(float q, float r, float s) {
         //三个轴先进行round 然后对diff最大的进行校正
         int rx = (int)Math.Round(q);
         int ry = (int)Math.Round(r);
         int rz = (int)Math.Round(s);

         float x_diff = Math.Abs(rx - q);
         float y_diff = Math.Abs(ry - r);
         float z_diff = Math.Abs(rz - s);

         if (x_diff >= y_diff && x_diff >= z_diff)
             rx = -ry - rz;
         else if (y_diff >= z_diff)
             ry = -rx - rz;
         else
             rz = -rx - ry;

         return new int[]{rx, ry, rz};
     }
     public static int DistanceByXy(int[] leftXy, int[] rightXy)
     {
         return DistanceByQr(leftXy, rightXy);
     }
//
     public static int[] qrToXy(int q, int r)
     {
         int x = q;
         int y = r + (q - (q & 1)) / 2;
         return new int[]{x, y};
     }

     public static int[] qrToXy(int[] qr)
     {
         return qrToXy(qr[0], qr[1]);
     }
     
     public static int[] qrToXy_Oddr(int q, int r)
     {
         int x = q + (r - (r & 1)) / 2;
         int y = r;
         return new int[]{x, y};
     }

     public static List<int[]> CubeRings(int[] qr, int fromRange, int toRange) {
         if (fromRange < 0 || toRange < fromRange) {
             throw new Exception("cubeNeighbor,param error,fromRange:" + fromRange + ",toRange:" + toRange);
         }
         List<int[]> result = new ();
         int[] newQr = {qr[0], qr[1]};
         if (fromRange == 0) {
             result.Add(newQr);
             fromRange = 1;
         }
         for (int range = fromRange; range <= toRange; range++) {
             int[] hex = cubeDirNeighbor(newQr, 4, range);
             for (int loopDir = 0; loopDir < 6; loopDir++) {
                 for (int i = 0; i < range; i++) {
                     result.Add(hex);
                     hex = cubeDirNeighbor(hex, loopDir, 1);
                 }
             }
         }
         return result;
     }

     public static int[] cubeDirNeighbor(int[] qr, int dir, int range) {
         return new int[]{qr[0] + cube_direction_vectors[dir].q * range, qr[1] + cube_direction_vectors[dir].r * range};
     }

     public struct Cube
     {
         public int q;
         public int r;
         public int s;
         public Cube(int _q,int _r,int _s)
         {
             q = _q;
             r = _r;
             s = _s;
         }
         public Cube(int _q,int _r)
         {
             q = _q;
             r = _r;
             s = -q-r;
         }
     }

     public static  Cube[] cube_direction_vectors =
     {
         new Cube(0, -1, 1), new Cube(1, -1, 0), new Cube(1, 0, -1),
         new Cube(0, 1, -1), new Cube(-1, 1, 0), new Cube(-1, 0, 1),
     };

     public static Cube cube_add(Cube hex, Cube vec)
     {
         return new Cube(hex.q + vec.q, hex.r + vec.r, hex.s + vec.s);
     }

     public static Cube cube_direction(int direction)
     {
         return cube_direction_vectors[direction];
     }

     public static Cube cube_neighbor(Cube cube, int direction)
     {
         return cube_add(cube, cube_direction(direction));
     }

     public static Cube cube_scale(Cube hex, int factor)
     {
         return new Cube(hex.q * factor, hex.r * factor, hex.s * factor);
     }

     public static List<Cube> cube_ring(Cube center,int radius)
     {
         var results = new List<Cube>();
         var hex = cube_add(center, cube_scale(cube_direction(4), radius));
         for (int i = 0; i < 6; i++)
         {
             for (int j = 0; j < radius; j++)
             {
                 results.Add(hex);
                 hex = cube_neighbor(hex, i);
             }
         }
         return results;
     }

     public static List<Cube> cube_spiral(int q, int r, int radius)
     {
         var center = new Cube(q, r);
         var results = new List<Cube>();
         results.Add(center);
         for (int i = 1; i <= radius; i++)
         {
             var temp = cube_ring(center, i);

             foreach (var v in temp)
             {
                 results.Add(v);
             }
         }

         return results;
     }

 }
