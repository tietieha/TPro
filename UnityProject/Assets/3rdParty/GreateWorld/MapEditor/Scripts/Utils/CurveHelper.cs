// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-04-11       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class CurveHelper
	{
		#region 路点连接

		public static List<Vector3> ConnectWaypoints(List<Vector3> waypoints)
		{
			List<Vector3> connectedPath = new List<Vector3>();

			// 选择起点和终点
			Vector3 startPoint = waypoints[0];
			Vector3 endPoint = waypoints[0];

			connectedPath.Add(startPoint);

			while (waypoints.Count > 0)
			{
				Vector3 nearestWaypoint = Vector3.zero;

				float shortestDistance = float.MaxValue;
				bool addToEnd = true;
				// 找到最近的路点
				foreach (Vector3 waypoint in waypoints)
				{
					// 1.跟尾巴比
					float distance2end = Vector3.Distance(endPoint, waypoint);
					if (distance2end < shortestDistance)
					{
						nearestWaypoint = waypoint;
						shortestDistance = distance2end;
						addToEnd = true;
					}

					// 2.跟头比
					float distance2start = Vector3.Distance(startPoint, waypoint);
					if (distance2start < shortestDistance)
					{
						nearestWaypoint = waypoint;
						shortestDistance = distance2start;
						addToEnd = false;
					}
				}

				if (addToEnd)
				{
					connectedPath.Add(nearestWaypoint);
					endPoint = nearestWaypoint;
				}
				else
				{
					connectedPath.Insert(0, nearestWaypoint);
					startPoint = nearestWaypoint;
				}

				waypoints.Remove(nearestWaypoint);
			}

			return connectedPath;
		}

		static Vector3 FindNearestWaypoint(Vector3 position, List<Vector3> waypoints)
		{
			Vector3 nearestWaypoint = waypoints[0];
			float shortestDistance = Vector3.Distance(position, nearestWaypoint);

			// 找到最近的路点
			foreach (Vector3 waypoint in waypoints)
			{
				float distance = Vector3.Distance(position, waypoint);
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					nearestWaypoint = waypoint;
				}
			}

			return nearestWaypoint;
		}

		#endregion

		#region 路点精简

		public static List<Vector3> SimplifyRoute(List<Vector3> route, float tolerance)
		{
			if (route == null || route.Count < 3)
				return route;

			List<Vector3> simplifiedRoute = new List<Vector3>();
			simplifiedRoute.Add(route[0]); // 添加起点
			Simplify(route, 0, route.Count - 1, tolerance, simplifiedRoute);
			simplifiedRoute.Add(route[route.Count - 1]); // 添加终点

			return simplifiedRoute;
		}

		private static void Simplify(List<Vector3> route, int start, int end, float tolerance, List<Vector3> simplifiedRoute)
		{
			float maxDistance = 0f;
			int index = 0;

			for (int i = start + 1; i < end; i++)
			{
				float distance = PerpendicularDistance(route[i], route[start], route[end]);
				if (distance > maxDistance)
				{
					maxDistance = distance;
					index = i;
				}
			}

			if (maxDistance > tolerance)
			{
				Simplify(route, start, index, tolerance, simplifiedRoute);
				simplifiedRoute.Add(route[index]);
				Simplify(route, index, end, tolerance, simplifiedRoute);
			}
		}

		private static float PerpendicularDistance(Vector3 point, Vector3 start, Vector3 end)
		{
			float area = Mathf.Abs(0.5f * (start.x * end.z + end.x * point.z + point.x * start.z - end.x * start.z - point.x * end.z - start.x * point.z));
			float bottom = Mathf.Sqrt(Mathf.Pow(end.x - start.x, 2f) + Mathf.Pow(end.z - start.z, 2f));
			float height = area / bottom * 2f;
			return height;
		}

		#endregion

		#region spline mesh

		/// <summary>
		/// 构建mesh
		/// </summary>
		/// <param name="route">曲线</param>
		/// <param name="width">网格宽度</param>
		/// <param name="segments">分段</param>
		public static Mesh GenerateMesh(List<Vector3> pathPoints, float width = 1f, int segments = 1, bool capEnds = false)
		{
			// 创建网格
			Mesh mesh = new Mesh();
			// 0 - 2 - 4 - 6
			// | / | / | / |
			// 1 - 3 - 5 - 7

			// 生成网格顶点、三角形和UV
			int pathControlPointCount = segments * pathPoints.Count;
			int vertexCount = 2 * pathControlPointCount;
			int triangleCount = 3 * 2 * (pathControlPointCount - 1);
			int[] triangles = new int[triangleCount];
			Vector3[] vertices = new Vector3[vertexCount];
			Vector3[] normals = new Vector3[pathPoints.Count];
			Vector2[] uv = new Vector2[vertexCount];

			for (int i = 0; i < pathPoints.Count; i++)
			{
				Vector3 startPoint;
				Vector3 endPoint;
				Vector3 direction;
				if (i == pathPoints.Count - 1)
				{
					Vector3 startBefore = pathPoints[i - 1];
					startPoint = pathPoints[i];
					direction = (startPoint - startBefore).normalized;
					endPoint = startPoint + direction;
				}
				else
				{
					startPoint = pathPoints[i];
					endPoint = pathPoints[i + 1];
					direction = (endPoint - startPoint).normalized;
				}

				// 计算右侧方向
				Vector3 vertexRight = Vector3.Cross(direction, Vector3.up);
				normals[i] = vertexRight;
				
				Vector3 preN;
				if (i > 0)
				{
					preN = normals[i - 1];
				}
				else
				{
					preN = vertexRight;
				}

				float reviseWidth = width;
				Vector3 normal = (preN + vertexRight).normalized;
				if (Vector3.Dot(preN, vertexRight) < 0.1f)
				{
					reviseWidth *= 1.5f;
				}

				Vector3 offset = normal * reviseWidth;
				for (int j = 0; j < segments; j++)
				{
					float t = (float) j / segments;
					Vector3 position = Vector3.Lerp(startPoint, endPoint, t);

					// 计算顶点位置
					Vector3 vertex1 = position - offset;
					Vector3 vertex2 = position + offset;

					int baseIndex = 2 * (i * segments + j);
					vertices[baseIndex] = vertex1;
					vertices[baseIndex + 1] = vertex2;

					// 计算UV
					uv[baseIndex] = new Vector2(t, 0);
					uv[baseIndex + 1] = new Vector2(t, 1);
				}
			}

			
			// 帽
			if (capEnds)
			{
				pathControlPointCount += 2;
				vertexCount += 4;
				triangleCount += 6 * 2;

				List<Vector3> capVertices = new List<Vector3>(triangleCount);
				// 头
				Vector3 pt0 = pathPoints[0];
				Vector3 pt1 = pathPoints[1];
				Vector3 dirHead = (pt1 - pt0).normalized;
				Vector3 headControlP = pt0 - dirHead * width;
				Vector3 headRight = normals[0];
				Vector3 headVerLeft = headControlP - headRight * width;
				Vector3 headVerRight = headControlP + headRight * width;

				// 尾
				Vector3 ptTail0 = pathPoints[^1];
				Vector3 ptTail1 = pathPoints[^2];
				Vector3 dirTail = (ptTail0 - ptTail1).normalized;
				Vector3 tailControlP = ptTail0 + dirTail * width;
				Vector3 tailRight = normals[0];
				Vector3 tailVerLeft = tailControlP - tailRight * width;
				Vector3 tailVerRight = tailControlP + tailRight * width;

			}
			
			// 所有控制点 最后一个点不用管
			for (int i = 0; i < pathControlPointCount - 1; i++)
			{
				int baseTriangleIndex = 6 * i;
				int baseVertexIndex = 2 * i;

				triangles[baseTriangleIndex + 0] = baseVertexIndex + 0;
				triangles[baseTriangleIndex + 1] = baseVertexIndex + 1;
				triangles[baseTriangleIndex + 2] = baseVertexIndex + 2;

				triangles[baseTriangleIndex + 3] = baseVertexIndex + 1;
				triangles[baseTriangleIndex + 4] = baseVertexIndex + 3;
				triangles[baseTriangleIndex + 5] = baseVertexIndex + 2;
			}

			// 设置网格数据并更新网格
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uv;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}
	}

	#endregion
}