using XLua;
using System;
using System.Collections.Generic;
using System.Numerics;

[LuaCallCSharp]
public static class HexagonGridUtil
{
    private static float c_OuterRadius = 1f;

    private static float c_InnerRadius = c_OuterRadius * 0.866025404f;

    private static double _tan30 = Math.Tan(30f * Math.PI / 180f);

    public static float GetHexagonOuterRadius(float gridSize)
    {
        return c_OuterRadius * gridSize;
    }

    public static float GetHexagonInnerRadius(float gridSize)
    {
        return c_InnerRadius * gridSize;
    }

    public static float HexagonXDistance(bool pointyTop, float gridSize)
    {
        return pointyTop
            ? GetHexagonInnerRadius(gridSize) * 2
            : GetHexagonOuterRadius(gridSize) * 1.5f;
    }
    

    public static float HexagonYDistance(bool pointyTop, float gridSize)
    {
        return pointyTop
            ? GetHexagonOuterRadius(gridSize) * 2f -
              (float)(GetHexagonInnerRadius(gridSize) *
                      _tan30)
            : GetHexagonInnerRadius(gridSize) * 2f;
    }

    public static float GetHexagonMeshMiddlePositionX(bool pointyTop, int rowNum, int colNum, float gridSize)
    {
        int centerX = rowNum / 2;
        int centerY = colNum / 2;
        float positionX;
        if (pointyTop)
        {
            positionX = (centerX + (centerY * 0.5f - centerY / 2f)) *
                        (GetHexagonInnerRadius(gridSize) * 2f);
        }
        else
        {
            positionX = centerX * (GetHexagonOuterRadius(gridSize) * 1.5f);
        }

        return positionX;
    }

    public static float GetHexagonMeshMiddlePositionY(bool pointyTop, int rowNum, int colNum, float gridSize)
    {
        int centerX = rowNum / 2;
        int centerY = colNum / 2;
        float positionY;
        if (pointyTop)
        {
            positionY = -centerY *
                        (GetHexagonInnerRadius(gridSize) * 1.5f);
        }
        else
        {
            positionY = -(centerY + (centerX * 0.5f - centerX / 2f)) *
                        (GetHexagonInnerRadius(gridSize) * 2f);
        }

        return positionY;
    }

    public static float GetGridMiddlePositionX(bool pointyTop, int x, int z, float hexagonXDis)
    {
        return pointyTop ? (x + (z * 0.5f - z / 2)) * hexagonXDis : x * hexagonXDis;
    }

    public static float GetGridMiddlePositionY(bool pointyTop, int x, int z, float hexagonYDis)
    {
        return pointyTop ? -z * hexagonYDis : -(z + (x * 0.5f - x / 2)) * hexagonYDis;
    }
}