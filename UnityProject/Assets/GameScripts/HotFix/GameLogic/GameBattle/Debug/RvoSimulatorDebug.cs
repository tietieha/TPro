using UnityEngine;
using UnityEditor;
using RVO;
using System.Collections.Generic;

namespace M.Battle
{
    public class RvoSimulatorDebug : MonoBehaviour
    {
        public Simulator simulator;
        readonly float _drawLineTime = 0.1f;
        private readonly float _drawLineY = 0.3f;
        static public float _showTime = 0;
        static public List<Vector3> drawRectPoints = new List<Vector3>();

        private void OnDrawGizmos()
        {
            DrawRects();
            if (simulator == null || simulator.Agents == null)
            {
                return;
            }

            Vector3 offset = transform.position;
            DrawObstacles();
            foreach (var agent in simulator.Agents)
            {
                if (agent is SoldierAgent soldierAgent)
                {
                    // if (soldierAgent.IsVirtual)
                    //     continue;
                    var position = agent.Position;
                    var vec = new Vector3(position.x(), 0, position.y()) + offset;
                    // Gizmos.DrawSphere(vec, agent.radius_);
                    var color = soldierAgent.Side == 1 ? Color.green : Color.white;
                    var campColor = soldierAgent.Side == 1 ? Color.green : Color.white;
                    var virtualHeroColor = soldierAgent.Side == 1 ? Color.cyan : Color.yellow;
                    var targetPos = soldierAgent.TargetPos;
                    var pos1 = new Vector3(position.x(), _drawLineY, position.y()) + offset;
                    var pos2 = new Vector3(targetPos.x(), _drawLineY, targetPos.y()) + offset;
                    if (!soldierAgent.IsVirtual)
                        Debug.DrawLine(pos1, pos2, campColor, _drawLineTime);
                    DrawCircle(vec, soldierAgent.radius_, soldierAgent.IsVirtual ? virtualHeroColor : color);
                    // if (!soldierAgent.IsMelee)
                    //     DrawCircle(vec, soldierAgent.AttackRange, color);
#if UNITY_EDITOR
                    DrawGridLabel(vec, soldierAgent.Id, campColor);
#endif
                }
            }
        }
#if UNITY_EDITOR
        // 格子的标识符和文本样式
        private GUIStyle _labelStyle;
        private void DrawGridLabel(Vector3 position, int id, Color color)
        {
            if (_labelStyle == null)
            {
                _labelStyle = new GUIStyle();
                _labelStyle.fontSize = 22;
                // _labelStyle.normal.textColor = color;
                _labelStyle.alignment = TextAnchor.MiddleCenter;
            }

            _labelStyle.normal.textColor = color;
            // 使用Handles.Label绘制文本
            Handles.Label(position + new Vector3(0, 0.1f, 0), id.ToString(), _labelStyle);
        }
#endif
        public void SetSimulator(Simulator s)
        {
            this.simulator = s;
        }

        private void DrawObstacles()
        {
            if (simulator == null || simulator.Obstacles == null)
            {
                return;
            }

            Vector3 offset = transform.position;
            var oldColor = Gizmos.color;
            Gizmos.color = Color.red; // Set the color for obstacles

            foreach (var obstacle in simulator.Obstacles)
            {
                var start = new Vector3(obstacle.point_.x(), 0, obstacle.point_.y()) + offset;
                var next = obstacle.next_;
                if (next != null)
                {
                    var end = new Vector3(next.point_.x(), 0, next.point_.y()) + offset;
                    Gizmos.DrawLine(start, end);
                }
            }

            Gizmos.color = oldColor;
        }

        private void DrawCircle(Vector3 center, float radius, Color color, int segments = 36)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            float angle = 2 * Mathf.PI / segments;
            Vector3 prevPoint = center + new Vector3(radius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                float x = Mathf.Cos(angle * i) * radius;
                float z = Mathf.Sin(angle * i) * radius;
                Vector3 newPoint = center + new Vector3(x, 0, z);
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }

            Gizmos.color = oldColor;
        }

        private void DrawRects()
        {
            if (drawRectPoints.Count == 0) return;
            Vector3 offset = transform.position;
            Gizmos.color = Color.green; // S
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    Gizmos.color = Color.yellow;
                }

                int startIndex = i * 4;

                Gizmos.DrawLine(drawRectPoints[startIndex] + offset, drawRectPoints[startIndex + 1] + offset);
                Gizmos.DrawLine(drawRectPoints[startIndex + 1] + offset, drawRectPoints[startIndex + 2] + offset);
                Gizmos.DrawLine(drawRectPoints[startIndex + 2] + offset, drawRectPoints[startIndex + 3] + offset);
                Gizmos.DrawLine(drawRectPoints[startIndex + 3] + offset, drawRectPoints[startIndex] + offset);
            }

            _showTime = _showTime - Time.deltaTime;
            if (_showTime < 0)
            {
                drawRectPoints.Clear();
            }
        }
    }
}