using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EmbattleHexagonMetrics
{
    /// <summary>
    /// 基准单位，最终大小和size相乘
    /// 1.154700538091836
    /// </summary>
    private static float c_OuterRadius = 1f;

    private static float c_InnerRadius = c_OuterRadius * 0.866025404f;

    private static float s_Size = 1.1547f;
    private const float WorldGridSize = 1.1547f;

    public const float Sqrt3 = 1.7320508f;

    public static Vector3[] Corners_FlatTop => _corners;
    public static Vector3[] Corners_PointyTop => _pointyCorners;
    public static Vector2[] Hexagon_UVs => _uvs.ToArray();
    private static int _numIcons = 6;

    private static Vector3[] _corners = null;
    private static Vector3[] _pointyCorners = null;

    /// <summary>
    /// 初始化时设置六边形大小
    /// </summary>
    /// <param name="hexagonSize"></param>
    public static void Initialize(float hexagonSize, int hexagonTexGridNum)
    {
        s_Size = hexagonSize > 0 ? hexagonSize : 1.0f;
        _numIcons = hexagonTexGridNum;
        List<Vector3> corners = new List<Vector3>
        {
            new Vector3(-0.5f * c_OuterRadius * s_Size, 0f, c_InnerRadius * s_Size),
            new Vector3(0.5f * c_OuterRadius * s_Size, 0f, c_InnerRadius * s_Size),
            new Vector3(c_OuterRadius * s_Size, 0f, 0f),
            new Vector3(0.5f * c_OuterRadius * s_Size, 0f, -c_InnerRadius * s_Size),
            new Vector3(-0.5f * c_OuterRadius * s_Size, 0f, -c_InnerRadius * s_Size),
            new Vector3(-c_OuterRadius * s_Size, 0f, 0f),
            new Vector3(-0.5f * c_OuterRadius * s_Size, 0f, c_InnerRadius * s_Size)
        };
        _corners = corners.ToArray();

        _pointyCorners = CalculatePointyTopCorners(hexagonSize, hexagonTexGridNum).ToArray();
    }

    public static List<Vector3> CalculatePointyTopCorners(float hexagonSize, int hexagonTexGridNum)
    {
        return new List<Vector3>
        {
            new Vector3(-c_InnerRadius * s_Size, 0f, 0.5f * c_OuterRadius * s_Size), // 顶点 1
            new Vector3(0, 0f, c_OuterRadius * s_Size), // 顶点 2
            new Vector3(c_InnerRadius * s_Size, 0f, 0.5f * c_OuterRadius * s_Size), // 顶点 3
            new Vector3(c_InnerRadius * s_Size, 0f, -0.5f * c_OuterRadius * s_Size), // 顶点 4
            new Vector3(0, 0f, -c_OuterRadius * s_Size), // 顶点 5
            new Vector3(-c_InnerRadius * s_Size, 0f, -0.5f * c_OuterRadius * s_Size), // 顶点 6
            new Vector3(-c_InnerRadius * s_Size, 0f, 0.5f * c_OuterRadius * s_Size) // 顶点 7 (和顶点 1 重合)
        };
    }

    private static List<Vector2> _uvs = new List<Vector2>()
    {
        new Vector2(0.25f, 0.078125f),
        new Vector2(0.75f, 0.078125f),
        new Vector2(0.9f, 0.5f),
        new Vector2(0.75f, 0.921875f),
        new Vector2(0.25f, 0.921875f),
        new Vector2(0.1f, 0.5f),
        new Vector2(0.25f, 0.078125f),
    };

    public static float GetWidth()
    {
        return c_OuterRadius;
    }

    public static float GetOuterRadius()
    {
        return c_OuterRadius * s_Size;
    }

    public static float GetWorldGridRadius()
    {
        return c_OuterRadius * WorldGridSize;
    }

    public static float GetInnerRadius()
    {
        return c_InnerRadius * s_Size;
    }

    public static Vector2 GetCenterUVByIndex(float index)
    {
        return new Vector2((index * 2 - 1) / (_numIcons * 2), 0.5f);
    }

    public static Vector2[] GetUVByIndex(float index)
    {
        float half = 1f / (_numIcons * 4);
        float start = (index - 1f) / _numIcons;
        //   Vector2 uicen = new Vector2(index / (_numIcons * 2), 0.5f);
        Vector2[] uvs = new Vector2[7];
        uvs[0] = new Vector2(half + start, 1f);
        uvs[1] = new Vector2(half * 3 + start, 1f);
        uvs[2] = new Vector2(half * 4 + start, 0.5f);
        uvs[3] = new Vector2(half * 3 + start, 0f);
        uvs[4] = new Vector2(half + start, 0f);
        uvs[5] = new Vector2(start, 0.5f);
        uvs[6] = new Vector2(half + start, 1f);
        return uvs;
    }
}