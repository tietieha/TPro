using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GEngine.MapEditor
{
    [CustomEditor(typeof(PrefabPolygonEx))]
    public class PrefabPolygonExEditor : Editor
    {
        PrefabPolygonEx prefabPolygonEx;
        private bool isEditing = false;

        private Vector3 previewPoint = Vector3.zero; // 用于预览点

        private string _testLog;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("WayPoint Tool", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Shift + 左键点击 添加路点", MessageType.Info);

            if (GUILayout.Button(isEditing ? "Stop Editing" : "Start Editing"))
            {
                isEditing = !isEditing;
                SceneView.RepaintAll();
            }

            if (GUILayout.Button("Clear Waypoints"))
            {
                prefabPolygonEx.waypoints.Clear();
                EditorUtility.SetDirty(prefabPolygonEx);
                SceneView.RepaintAll();
            }

            GUILayout.Label("Waypoints Count: " + prefabPolygonEx.waypoints.Count);

            base.OnInspectorGUI();
            EditorGUILayout.Space();
            GUILayout.Label("测试", EditorStyles.boldLabel);
            if (GUILayout.Button("测试"))
            {
                bool inPolygon = prefabPolygonEx.IsPointInPolygon(prefabPolygonEx.transform.position);
                _testLog = inPolygon ? "在多边形内" : "不在";
            }
            GUILayout.Label(_testLog);

        }

        private void OnEnable()
        {
            prefabPolygonEx = (PrefabPolygonEx)target;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            // if (!isEditing) return;
            
            var waypoints = prefabPolygonEx.waypoints;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Event e = Event.current;

            // 捕捉鼠标点击事件，生成路点
            if (e.shift)
            {
                // 只有在按住Shift的情况下才将路点添加到列表
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (plane.Raycast(ray, out float distance))
                    {
                        previewPoint = ray.GetPoint(distance); // 更新预览点
                        previewPoint.y = 0;
                        waypoints.Add(previewPoint); // 添加路点
                        EditorUtility.SetDirty(prefabPolygonEx);
                        e.Use(); // 阻止事件传播
                    }
                }
            }

            // 绘制路点和连接线
            Handles.color = Color.green;
            for (int i = 0; i < waypoints.Count; i++)
            {
                // 为每个路点生成一个可移动的Handles
                Vector3 newWaypointPosition =
                    Handles.PositionHandle(waypoints[i], Quaternion.identity);
                if (newWaypointPosition != waypoints[i])
                {
                    waypoints[i] = newWaypointPosition; // 更新移动后的点
                    SceneView.RepaintAll();
                }

                // 显示序号
                Handles.Label(waypoints[i] + Vector3.up * 0.2f + new Vector3(0.2f, 0, 0.2f),
                    i.ToString()); // 显示序号
                Handles.SphereHandleCap(0, waypoints[i], Quaternion.identity, 0.3f,
                    EventType.Repaint); // 较小的句柄

                // 连接每个路点
                if (i > 0)
                {
                    Handles.DrawLine(waypoints[i - 1], waypoints[i]);
                }
            }

            // 绘制当前预览点和与已有点的连接线
            if (e.shift)
            {
                Handles.color = Color.yellow; // 预览点颜色

                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (plane.Raycast(ray, out float distance))
                {
                    previewPoint = ray.GetPoint(distance); // 更新预览点
                    Handles.SphereHandleCap(0, previewPoint, Quaternion.identity, 0.1f,
                        EventType.Repaint);

                    // 连接预览点和已有的路点
                    if (waypoints.Count > 1)
                    {
                        Handles.DrawLine(waypoints[waypoints.Count - 1], previewPoint);
                        Handles.DrawLine(previewPoint, waypoints[0]);
                    }
                }
            }
            // 绘制闭合路径
            else if (waypoints.Count >= 3)
            {
                Handles.DrawLine(waypoints[waypoints.Count - 1], waypoints[0]);
            }
        }
    }
}