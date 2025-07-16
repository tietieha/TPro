#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
    public class IslandWindow : EditorWindow
    {
        private static IslandWindow _window;
        // [MenuItem("地图编辑/岛屿模板列表", false, 200)]
        public static void ShowIslandList()
        {
            if (_window == null)
            {
                _window = GetWindow<IslandWindow>(true, "岛屿模板");
                _window.minSize = new Vector2(200, 500);
                _window.ShowUtility();
            }
            else
            {
                _window.Close();
            }
			
        }
        
        public static void Hide()
        {
            if (_window != null)
                _window.Close();
        }

        public static void UpdateUI()
        {
            if (_window != null)
                _window.Repaint();
        }

        private void OnGUI()
        {
            if (MapRender.instance == null)
                return;

            var map = MapRender.instance.GetMap();
            if (map == null)
                return;
            
            GUILayout.BeginVertical();

            var islands = map.islandTemplates;
            var remove = new List<int>();
            foreach (var kv in islands)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"岛屿 {kv.Key}");
                if (GUILayout.Button("编辑"))
                {
                    MapRender.instance.islandTemplate = kv.Value;
                    MapToolBar.SetSelect(12);
                }
                if (GUILayout.Button("移除"))
                {
                    remove.Add(kv.Key);
                }
                GUILayout.EndHorizontal();
            }

            foreach (var key in remove)
            {
                map.RemoveIsland(key);
            }

            GUILayout.Space(18);
            if (GUILayout.Button("退出编辑"))
            {
                MapRender.instance.islandTemplate = null;
            }
            
            GUILayout.Space(18);
            if (GUILayout.Button("加载岛屿模板"))
            {
                MapRender.instance.ExportIslandTemplate();
            }
            
            GUILayout.Space(18);
            if (GUILayout.Button("保存岛屿模板"))
            {
                MapRender.instance.ImportIslandTemplate();
            }
            
            GUILayout.EndVertical();
        }
    }
}
#endif