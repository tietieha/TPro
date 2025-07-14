using UnityEngine;
using System.Collections.Generic;
using XLua;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace M.Battle
{
    [LuaCallCSharp]
    public class FieldGridDrawer : MonoBehaviour
    {
        // 格子尺寸，单位为世界坐标单位
        public float gridSize = 1f;

        // 记录格子的数量和坐标
        private List<Vector3> gridPositions = new List<Vector3>();
        private List<int> gridIds = new List<int>();

        // 格子的标识符和文本样式
        private GUIStyle labelStyle;

        // 通过Lua传入的数据
        public void SetGridData(List<Vector3> positions, List<int> ids, float newGridSize)
        {
            gridSize = newGridSize;
            // gridPositions = positions;
            gridPositions.Clear();
            gridIds = ids;

            foreach (var pos in positions)
            {
                gridPositions.Add(pos);
            }
        }

        // 只有在编辑器模式下绘制
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (gridPositions.Count == 0 || gridIds.Count == 0)
                return;

            // 设置绘制颜色
            Gizmos.color = Color.green;
            Vector3 offset = transform.position;
            // 绘制每个格子的边框和编号
            for (int i = 0; i < gridPositions.Count; i++)
            {
                Vector3 gridPosition = offset + gridPositions[i];
                int gridId = gridIds[i];

                // 绘制格子边框
                Gizmos.DrawWireCube(gridPosition, new Vector3(gridSize, 0.1f, gridSize));

                // 绘制格子编号
                DrawGridLabel(gridPosition, gridId);
            }
        }

        // 绘制格子编号
        private void DrawGridLabel(Vector3 position, int id)
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle();
                labelStyle.fontSize = 22;
                labelStyle.normal.textColor = Color.white;
                labelStyle.alignment = TextAnchor.MiddleCenter;
            }

            // 使用Handles.Label绘制文本
            Handles.Label(position + new Vector3(0, 0.1f, 0), id.ToString(), labelStyle);
        }
#endif
    }
}