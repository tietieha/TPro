using System;
using System.Collections.Generic;
using RVO;

namespace M.Battle
{
    public class BattleUtils
    {
        public static List<Vector2> GetCircleEightDirectionPoints(float radius, Vector2 origin)
        {
            var points = new List<Vector2>();
            int[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
            for (int i = 0; i < angles.Length; i++)
            {
                float angle = angles[i];
                var rad = angle / 180 * Math.PI;
                var x = radius * Math.Cos(rad);
                var y = radius * Math.Sin(rad);
                points.Add(new Vector2((float)x + origin.x(), (float)y + origin.y()));
            }

            return points;
        }

        public static Vector2 FindNearestTargetPoint(Vector2 currentPoint, Vector2 circumcenter,
            List<Vector2> eightDirectionPoints)
        {
            int index = -1;
            double minDistance = double.MaxValue;
            for (int i = 0; i < eightDirectionPoints.Count; i++)
            {
                var point = eightDirectionPoints[i];
                double dis = currentPoint * point;
                if (minDistance > dis)
                {
                    minDistance = dis;
                    index = i;
                }
            }

            if (index == -1)
            {
                return currentPoint;
            }
            return eightDirectionPoints[index];
        }
    }
}