#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-11-03 10:30 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

namespace GEngine.MapEditor
{
	public class SceneTool : EditorWindow
	{
		private const string HIGH_WORLD_RES_RENDER_DATA_PATH =
			"Assets/GameAssets/Scenes/Campaign/{0}/Prefab/WorldResRenderData.prefab";
		private const string LOW_WORLD_RES_RENDER_DATA_PATH =
			"Assets/GameAssets/Scenes/Campaign/{0}/Prefab/LOW_WorldResRenderData.prefab";

		private const string WORLD_RES_FOLDER = "Assets/GameAssets/Scenes/Campaign/";

		// data
		private List<string> _sceneLst = new List<string>();
		private bool isLoading;
		private string current = string.Empty;

		// ui
		private static SceneTool _window;
		private Vector2 _scrollPos;

		// [MenuItem("地图编辑/场景工具", false, 210)]
		public static void Show()
		{
			if (_window == null)
			{
				_window = GetWindow<SceneTool>(true, "场景");
				_window.minSize = new Vector2(200, 500);
				_window.ShowUtility();
				_window.InitData();
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

		private void InitData()
		{
			_sceneLst.Clear();

			DirectoryInfo rootFolder = new DirectoryInfo(WORLD_RES_FOLDER);
			DirectoryInfo[] subFolders = rootFolder.GetDirectories();
			foreach (var dicInfo in subFolders)
			{
				if (!dicInfo.Name.ToLower().Contains("common"))
				{
					_sceneLst.Add(dicInfo.Name);
				}
			}
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField($"当前：{current}");
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField($"共{_sceneLst.Count}个");
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("刷新", GUILayout.Width(60)))
				{
					InitData();
				}
			}
			EditorGUILayout.EndHorizontal();
			using (new EditorGUILayout.ScrollViewScope(_scrollPos, EditorStyles.helpBox))
			{
				for (int i = 0; i < _sceneLst.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField(_sceneLst[i], GUILayout.Width(200));
						if (GUILayout.Button("高配"))
						{
							OpenScene(_sceneLst[i]);
						}
						if (GUILayout.Button("低配"))
						{
							OpenScene(_sceneLst[i], false);
						}
					}
					EditorGUILayout.EndHorizontal();
					
				}
			}

			if (isLoading)
				EditorUtility.DisplayProgressBar("加载场景", "解析 res data",
					0.5f + MapRender.instance.worldTestRoot.GetAnalyzeProgress() * 0.5f);
		}

		private void OpenScene(string worldDataName, bool highQuality = true)
		{
			try
			{
				if (MapRender.instance.worldTestRoot == null)
					return;

				MapRender.instance.worldTestRoot.UnInit();
				EditorUtility.DisplayProgressBar("加载场景", "load res ", 0.1f);
				// load res render data
				ResRenderData resRenderData = AssetDatabase.LoadAssetAtPath<ResRenderData>(
					string.Format(highQuality ? HIGH_WORLD_RES_RENDER_DATA_PATH : LOW_WORLD_RES_RENDER_DATA_PATH,
						worldDataName));

				current = $"{worldDataName} - {(highQuality ? "高配" : "低配")}";
					
				EditorUtility.DisplayProgressBar("加载场景", "load res ", 0.4f);
				// initlogic
				isLoading = true;
				MapRender.instance.worldTestRoot.Init(
					resRenderData, 
					new Vector3(2079.5f, -0.05f, -1201.5f),
					() =>
					{
						isLoading = false;
						EditorUtility.ClearProgressBar();
					});
				
			}
			catch (Exception e)
			{
				isLoading = false;
				EditorUtility.ClearProgressBar();
				Debug.LogError(e);
			}
		}
	}
}
#endif