#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class PatternTool : EditorWindow
	{
		private static PatternTool _window;

		// [MenuItem("地图编辑/模具工具", false, 202)]
		public static void ShowPatternTool()
		{
			if (_window == null)
			{
				_window = GetWindow<PatternTool>(true, "模具");
				_window.minSize = new Vector2(300, 400);
				//_window.position = new Rect(50, 50, 300, 400);
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

		public static void UpdateUI()
		{
			if (_window != null)
				_window.Repaint();
		}

		Vector2 _scrollPos = Vector2.zero;

		private void OnGUI()
		{
			if (MapRender.instance == null)
				return;

			var manager = MapRender.instance.PatternManager;
			if (manager == null || manager.Patterns.Count == 0)
			{
				GUILayout.Label("没有模具数据!!!");
				if (GUILayout.Button("加载模具"))
				{
					MapRender.instance.OnMenu_LoadPattern();
				}

				return;
			}

			_scrollPos = GUILayout.BeginScrollView(_scrollPos);

			foreach (var pattern in manager.Patterns.Values)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Box(pattern.Icon, GUILayout.Width(150), GUILayout.Height(150));
				GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
				GUILayout.Label("名称:");
				pattern.Name = EditorGUILayout.TextField(pattern.Name);
				GUILayout.EndHorizontal();
				GUILayout.Label($"等级: {pattern.Level}");
				GUILayout.Label($"大小: {pattern.Count}");
				pattern.IsOddPos = GUILayout.Toggle(pattern.IsOddPos == 1, "位置校准") ? 1 : 0;
				GUILayout.BeginHorizontal();
				if (pattern.IsGuanqia == 1) GUILayout.Box("关卡");
				if (pattern.IsBorn == 1) GUILayout.Box("出生城");
				if (pattern.SubType == 1) GUILayout.Box("末日实验室");
				GUILayout.EndVertical();
				if (GUILayout.Button("加载", GUILayout.Height(30)))
				{
					MapRender.instance.Pattern = pattern;
					MapToolBar.UpdateUI();
				}

				if (GUILayout.Button("删除", GUILayout.Height(30)))
				{
					if (PatternManager.instance != null)
						PatternManager.instance.DelPatternZone(pattern.Guid);
				}

				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				GUILayout.Space(20);
			}

			GUILayout.EndScrollView();
		}
	}
}
#endif