#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class RuinTool : EditorWindow
	{
		private static RuinTool _window;

		// [MenuItem("地图编辑/遗迹工具", false, 201)]
		public static void ShowRuinTool()
		{
			if (_window == null)
			{
				_window = GetWindow<RuinTool>(true, "遗迹");
				_window.minSize = new Vector2(200, 250);
				//_window.position = new Rect(50, 50, 200, 250);
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

		public static bool IsVisible()
		{
			return _window != null;
		}

		private int _selectZone = -1;

		private void OnGUI()
		{
			if (MapRender.instance == null)
				return;

			var map = MapRender.instance.GetMap();
			if (map == null)
				return;

			if (GUILayout.Button("刷新遗迹", GUILayout.Height(40)))
			{
				MapRender.instance.CheckRuins();
			}

			if (map.ruinDatas.Count == 0)
			{
				if (map.ruinDatas.Count == 0)
				{
					GUILayout.Label("暂无遗迹数据");
					return;
				}
			}

			GUILayout.BeginVertical();
			foreach (var t in map.ruinDatas.Values)
			{
				GUILayout.Space(10);
				GUILayout.Label($"{t.RuinId}号遗迹");
				GUILayout.Label($"区域数量 {t.Zones.Count}");
				string[] btns = new string[t.Zones.Count];
				for (int i = 0; i < t.Zones.Count; i++)
				{
					btns[i] = t.Zones[i].index.ToString();
				}

				var idx = GUILayout.SelectionGrid(_selectZone, btns, 3);
				if (idx != _selectZone)
				{
					_selectZone = idx;
					MapRender.instance.FocusHexagon(t.Zones[idx].hexagon, -1f);
				}

				GUILayout.Space(10);
			}

			GUILayout.Space(40);

			int k = 0;
			string[] _tmps = new string[map.ruinDatas.Count];
			foreach (var ruin in map.ruinDatas.Values)
			{
				_tmps[k++] = $"{ruin.RuinId}号遗迹";
			}

			MapRender.instance.CurRuinId = GUILayout.SelectionGrid(MapRender.instance.CurRuinId - 1, _tmps, 3) + 1;

			GUILayout.Space(20);
			GUILayout.Label($"显示深度(Depth = {MapRender.instance.CurRuinDepth})");
			MapRender.instance.CurRuinDepth = (int) GUILayout.HorizontalSlider(MapRender.instance.CurRuinDepth, 0, 200);

			GUILayout.EndVertical();
		}
	}
}
#endif