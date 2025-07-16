// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-10-08       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GEngine.MapEditor
{
    [HideMonoScript]
    [DisallowMultipleComponent]
    public class PrefabPolygonEx : MonoBehaviour
    {
        public List<Vector3> waypoints = new List<Vector3>();

        public bool IsPointInPolygon(Vector3 point)
        {
            return IsPointInPolygon(point, waypoints);
        }

        // 判断一个点是否在多边形内
        public bool IsPointInPolygon(Vector3 point, List<Vector3> polygon)
        {
            if (polygon.Count < 3)
                return false;
            int intersections = 0;
            int n = polygon.Count;
            float x = point.x;
            float z = point.z;

            for (int i = 0; i < n; i++)
            {
                Vector3 v1 = polygon[i];
                Vector3 v2 = polygon[(i + 1) % n];

                float v1x = v1.x, v1z = v1.z;
                float v2x = v2.x, v2z = v2.z;

                // 检查点是否在顶点上（直接返回 true）
                if ((x == v1x && z == v1z) || (x == v2x && z == v2z))
                    return true;

                // 检查射线是否与边相交
                if ((v1z > z) != (v2z > z)) // 点是否在边的 Y 范围内
                {
                    // 计算交点 X 坐标
                    float intersectX = (z - v1z) * (v2x - v1x) / (v2z - v1z) + v1x;

                    // 如果点在边的左侧，计数 +1
                    if (x <= intersectX)
                        intersections++;
                }
            }

            // 奇数交点 = 点在内部
            return (intersections % 2) == 1;
        }
    }
}