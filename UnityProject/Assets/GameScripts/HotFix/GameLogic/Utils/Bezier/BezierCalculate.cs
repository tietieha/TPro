using System;
using UnityEngine;


//贝塞尔曲线计算
public static class BezierCalculate
{
	/// <summary>
	/// 二次贝塞尔曲线，根据T值，计算贝塞尔曲线上面相对应的点
	/// </summary>
	/// <param name="t"></param>T值
	/// <param name="p0"></param>起始点
	/// <param name="p1"></param>控制点
	/// <param name="p2"></param>目标点
	/// <returns></returns>根据T值计算出来的贝赛尔曲线点
	public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
 
		Vector3 p = uu * p0;
		p += 2 * u * t * p1;
		p += tt * p2;
 
		return p;
	}

    //三阶贝塞尔曲线获取0<=t<=1之间的点    需要4个点 p0---起点 p1，p2---控制点 ,p3---终点
    public static Vector3 BezierPos_Three_Pos(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 resultPos = new Vector3();
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        resultPos = uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
        return resultPos;
    }

    //三阶贝塞尔曲线获取0<=t<=1之间的切线    需要4个点 p0---起点 p1，p2---控制点 ,p3---终点
    public static Vector3 BezierPos_Three_Tangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 resultTangent = new Vector3();
        float u = 1 - t;
        float uu = u * u;
        float tu = t * u;
        float tt = t * t;
        resultTangent = p0 * 3 * uu * (-1.0f) + p1 * 3 * (uu - 2 * tu) + p2 * 3 * (2 * tu - tt) + p3 * 3 * tt;
        return resultTangent.normalized;
    }

    //返回贝塞尔采样10个点位置
    public static Vector3[] BezierLength_Three(Vector3 pStart, Vector3 pEnd)
    {
        int s_COUNT = 11;
        float t = 0;
        float dt = 1.0f / (s_COUNT - 1);
        float ControlPercent = 0.25f;
        //计算p2,p3控制点位置 路口转向
        Vector3 p2 = new Vector3(pStart.x + (pEnd.x - pStart.x) * ControlPercent, 0, pStart.z);
        Vector3 p3 = new Vector3(pEnd.x, 0, pEnd.z - (pEnd.z - pStart.z) * ControlPercent);
        //Debug.DrawLine(p2, p3, Color.green, 1000000f);
        var bezierPosVec = new Vector3[s_COUNT];
        Vector3 p = BezierPos_Three_Pos(0, pStart, p2, p3, pEnd);
        bezierPosVec[0] = p;
        for (int i = 1; i < s_COUNT; i++)
        {
            t += dt;
            p = BezierPos_Three_Pos(t, pStart, p2, p3, pEnd);
            bezierPosVec[i] = p;
        }

        return bezierPosVec;
    }

    public static float BezierLength_Three_Length(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        int s_COUNT = 11;
        float result = 0;
        if (p0 == p1 && p2 == p3)
        {
            return 1.0f;
        }

        float t = 0;
        float dt = 1.0f / (s_COUNT - 1);
        var bezierPosVec = new Vector3[s_COUNT];
        Vector3 p = BezierPos_Three_Pos(0, p0, p1, p2, p3);
        bezierPosVec[0] = p;
        for (int i = 1; i < s_COUNT; i++)
        {
            t += dt;
            p = BezierPos_Three_Pos(t, p0, p1, p2, p3);
            bezierPosVec[i] = p;
        }

        for (int i = 1; i < bezierPosVec.Length; i++)
        {
            var dx = bezierPosVec[i].x - bezierPosVec[i - 1].x;
            var dz = bezierPosVec[i].z - bezierPosVec[i - 1].z;
            result += (float) Math.Sqrt(dx * dx + dz * dz);
        }

        return result;
    }
}