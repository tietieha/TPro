using System;
using UnityEngine;
using System.Collections.Generic;

public class EmbattleHexagonUtils
{
    // 邻接点的偏移量
    // https://www.redblobgames.com/grids/hexagons/#neighbors-offset
    private static readonly Vector2Int[][] NeighborOffset =
    {
        // even cols
        new[]
        {
            new Vector2Int(+1, 0), new Vector2Int(+1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(0, +1)
        },

        // odd cols
        new[]
        {
            new Vector2Int(+1, +1), new Vector2Int(+1, 0), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, +1), new Vector2Int(0, +1)
        }
    };

    public static Vector2Int CubeToOffset(Vector3Int cube)
    {
        int x = cube.x;
        int z = cube.z + (cube.x - (cube.x & 1)) / 2;
        return new Vector2Int(x, z);
    }

    public static Vector3Int Cube_round(Vector3 cube)
    {
        //三个轴先进行round 然后对diff最大的进行校正
        int rx = Mathf.RoundToInt(cube.x);
        int ry = Mathf.RoundToInt(cube.y);
        int rz = Mathf.RoundToInt(cube.z);

        float x_diff = Mathf.Abs(rx - cube.x);
        float y_diff = Mathf.Abs(ry - cube.y);
        float z_diff = Mathf.Abs(rz - cube.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector3Int(rx, ry, rz);
    }

    public static int HexagonDistance(EmbattleHexagonCoordinates left, EmbattleHexagonCoordinates right)
    {
        int a = Mathf.Abs(left.X - right.X);
        int b = Mathf.Abs(left.X + left.Y - right.X - right.Y);
        int c = Mathf.Abs(left.Y - right.Y);

        return (a + b + c) / 2;
    }

    public static Vector3 OffsetToWorld(Vector2Int offset)
    {
        float x = EmbattleHexagonMetrics.GetOuterRadius() * 3.0f / 2.0f * offset.x;
        float y = EmbattleHexagonMetrics.GetOuterRadius() * EmbattleHexagonMetrics.Sqrt3 *
                  (offset.y + 0.5f * (offset.x & 1));

        return new Vector3(x, 0, -y);
    }

    /// <summary>
    /// 计算邻接点
    /// </summary>
    public static Vector2Int OddROffsetNeighbor(Vector2Int pos, int dir)
    {
        int parity = pos.y & 1;
        Vector2Int d = NeighborOffset[parity][dir];
        return new Vector2Int(pos.x + d.x, pos.y + d.y);
    }

    public static List<int> CalculateNeedStandGrid(int rowNum, int colNum, int standColNum,
        int standRowNum, int space)
    {
        int middleRow = (int)Math.Floor((rowNum / 2f));
        int middleCol = (int)Math.Floor((colNum / 2f));

        int startRow = middleRow - (int)Math.Floor((space / 2f) + standRowNum);
        int startCol = middleCol - (int)Math.Floor((standColNum / 2f));
        int rightStartRow = startRow + standRowNum + space;

        return new List<int>
        {
            startCol, startCol + standColNum, startRow,
            startRow + standRowNum, rightStartRow, rightStartRow + standRowNum
        };
    }
    /// <summary>
    /// 计算箭头六边格的顶点
    /// </summary>
    /// <returns></returns>
    // public static List<Vector2> CalculatePointyTopOrientation()
    // {
    // }
}