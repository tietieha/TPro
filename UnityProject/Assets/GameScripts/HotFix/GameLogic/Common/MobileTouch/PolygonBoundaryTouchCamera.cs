// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-05-23       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System.Collections.Generic;
using UnityEngine;
using XLua;
namespace BitBenderGames
{
    [RequireComponent(typeof(TouchInputController))]
    [RequireComponent(typeof(Camera))]
    [LuaCallCSharp]
    public class PolygonBoundaryTouchCamera : MobileTouchCamera
    {
        [Header("Polygon Boundary Settings")] [SerializeField]
        private List<Vector2> polygonBoundaryVertices = new List<Vector2>();

        [SerializeField] private bool usePolygonBoundary = false;

        public List<Vector2> PolygonBoundaryVertices
        {
            get => polygonBoundaryVertices;
            set
            {
                polygonBoundaryVertices = value;
                ComputeCamBoundaries();
            }
        }

        public bool UsePolygonBoundary
        {
            get => usePolygonBoundary;
            set
            {
                usePolygonBoundary = value;
                ComputeCamBoundaries();
            }
        }

        /// <summary>
        /// waypoints 两个一组
        /// </summary>
        /// <param name="points"></param>
        public void SetCameraBoundary(int[] points)
        {
            List<Vector2> list = new List<Vector2>();
            for (int i = 0; i < points.Length; i += 2)
            {
                list.Add(new Vector2(points[i], points[i + 1]));
            }

            usePolygonBoundary = true;
            PolygonBoundaryVertices = list;
        }

        protected override void ComputeCamBoundaries()
        {
            if (usePolygonBoundary && polygonBoundaryVertices.Count > 0)
            {
                // // 计算多边形的最小包围矩形用于优化
                // Vector2 min = polygonBoundaryVertices[0];
                // Vector2 max = polygonBoundaryVertices[0];
                //
                // foreach (var vertex in polygonBoundaryVertices)
                // {
                //     min = Vector2.Min(min, vertex);
                //     max = Vector2.Max(max, vertex);
                // }

                boundaryMin = new Vector2(-1000, -1000);
                boundaryMax = new Vector2(1000, 1000);
                base.ComputeCamBoundaries();
            }
            else
            {
                // 调用父类的矩形边界计算
                base.ComputeCamBoundaries();
            }
        }

        public override bool GetIsBoundaryPosition(Vector3 testPosition)
        {
            if (!usePolygonBoundary || polygonBoundaryVertices.Count < 3)
            {
                return base.GetIsBoundaryPosition(testPosition);
            }

            Vector2 testPos2D = ProjectVector3(testPosition);
            return !IsPointInPolygon(testPos2D, polygonBoundaryVertices);
        }

        public override Vector3 GetClampToBoundaries(Vector3 newPosition, bool includeSpringBackMargin = false)
        {
            if (!usePolygonBoundary || polygonBoundaryVertices.Count < 3)
            {
                return base.GetClampToBoundaries(newPosition, includeSpringBackMargin);
            }

            Vector2 testPos2D = ProjectVector3(newPosition);
            if (IsPointInPolygon(testPos2D, polygonBoundaryVertices))
            {
                return newPosition;
            }

            // 找到多边形内最近的点
            Vector2 closestPoint = FindClosestPointOnPolygon(testPos2D, polygonBoundaryVertices);
            Vector3 clampedPosition = UnprojectVector2(closestPoint, newPosition.y);

            // 如果当前位置超出边界，调整 AutoScroll 速度方向，使其沿边界滑动
            if (IsDragging)
            {
                Vector3 adjustedVelocity = (clampedPosition - transform.position).normalized * cameraScrollVelocity.magnitude;
                cameraScrollVelocity = adjustedVelocity;
            }
            // touchInputController.RestartDrag();
            return clampedPosition;
        }

        #region Polygon Utility Methods

        // 射线法判断点是否在多边形内
        private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
        {
            bool inside = false;
            int j = polygon.Count - 1;

            for (int i = 0; i < polygon.Count; j = i++)
            {
                // 检查是否为水平边，避免除零错误
                if (Mathf.Approximately(polygon[i].y, polygon[j].y))
                {
                    continue; // 跳过水平边
                }
                if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                    (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) /
                        (polygon[j].y - polygon[i].y) + polygon[i].x))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        // 找到多边形边界上最近的点
        private Vector2 FindClosestPointOnPolygon(Vector2 point, List<Vector2> polygon)
        {
            Vector2 closestPoint = polygon[0];
            float minDistance = Vector2.Distance(point, closestPoint);

            // 检查顶点
            for (int i = 1; i < polygon.Count; i++)
            {
                float distance = Vector2.Distance(point, polygon[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = polygon[i];
                }
            }

            // 检查边上的最近点
            for (int i = 0; i < polygon.Count; i++)
            {
                int j = (i + 1) % polygon.Count;
                Vector2 segmentClosest = GetClosestPointOnSegment(point, polygon[i], polygon[j]);
                float segmentDistance = Vector2.Distance(point, segmentClosest);

                if (segmentDistance < minDistance)
                {
                    minDistance = segmentDistance;
                    closestPoint = segmentClosest;
                }
            }

            return closestPoint;
        }

        // 找到线段上最近的点
        private Vector2 GetClosestPointOnSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            Vector2 segment = segmentEnd - segmentStart;
            float segmentLength = segment.magnitude;
            Vector2 segmentDir = segment.normalized;

            float projection = Vector2.Dot(point - segmentStart, segmentDir);
            projection = Mathf.Clamp(projection, 0f, segmentLength);

            return segmentStart + segmentDir * projection;
        }

        #endregion

        // #region Editor Visualization
        //
        // private void OnDrawGizmosSelected()
        // {
        //     if (!usePolygonBoundary || polygonBoundaryVertices.Count < 3)
        //     {
        //         base.OnDrawGizmosSelected();
        //         return;
        //     }
        //
        //     // 绘制多边形边界
        //     Gizmos.color = Color.green;
        //     for (int i = 0; i < polygonBoundaryVertices.Count; i++)
        //     {
        //         int j = (i + 1) % polygonBoundaryVertices.Count;
        //         Vector3 start = UnprojectVector2(polygonBoundaryVertices[i], 0);
        //         Vector3 end = UnprojectVector2(polygonBoundaryVertices[j], 0);
        //         Gizmos.DrawLine(start, end);
        //     }
        //
        //     // 绘制顶点
        //     Gizmos.color = Color.red;
        //     foreach (var vertex in polygonBoundaryVertices)
        //     {
        //         Gizmos.DrawSphere(UnprojectVector2(vertex, 0), 0.2f);
        //     }
        // }
        //
        // #endregion

        #region Debug

        private void OnValidate()
        {
            ComputeCamBoundaries();
        }

        #endregion
    }
}