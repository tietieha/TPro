// #if UNITY_EDITOR
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace GEngine.MapEditor
// {
// 	public class CreateMapTool : EditorWindow
// 	{
// 		private static CreateMapTool _window;
//
// 		private SeekInfo seek1 = new SeekInfo();
// 		private SeekInfo seek2 = new SeekInfo();
// 		private SeekInfo seek3 = new SeekInfo();
//
// 		[MenuItem("地图编辑/新建地图", false, 0)]
// 		public static void ShowToolBar()
// 		{
// 			if (_window == null)
// 			{
// 				_window = GetWindow<CreateMapTool>(true, "创建地图");
// 				_window.minSize = new Vector2(420, 450);
// 				//_window.position = new Rect(50, 50, 420, 450);
// 				_window.Init();
// 				_window.ShowUtility();
// 			}
// 			else
// 			{
// 				_window.Close();
// 			}
// 		}
//
// 		public static void Hide()
// 		{
// 			if (_window != null)
// 				_window.Close();
// 		}
//
// 		public void Init()
// 		{
// 			// 默认的区域大小
// 			seek1.width = 2401;
// 			seek1.height = 1201;
// 			seek1.seekOffset = 60;
// 			seek1.startOffsetX = 30;
// 			seek1.startOffsetY = 30;
// 			seek1.seekAdjustRate = 33;
// 			seek1.seekAdjustOffset = 30;
// 			seek1.seekDelRandomMin = 3;
// 			seek1.seekDelRandomMax = 5;
// 			seek1.posType = 1;
// 			seek1.level = 1;
//
// 			seek2.width = 1000;
// 			seek2.seekOffset = 75;
// 			seek2.startOffsetX = 35;
// 			seek2.startOffsetY = 35;
// 			seek2.seekAdjustRate = 33;
// 			seek2.seekAdjustOffset = 30;
// 			seek2.seekDelRandomMin = 4;
// 			seek2.seekDelRandomMax = 6;
// 			seek2.posType = 2;
// 			seek2.level = 4;
//
// 			seek3.width = 600;
// 			seek3.seekOffset = 65;
// 			seek3.startOffsetX = 30;
// 			seek3.startOffsetY = 30;
// 			seek3.seekAdjustRate = 0;
// 			seek3.seekAdjustOffset = 0;
// 			seek3.posType = 3;
// 			seek3.level = 6;
// 		}
//
// 		private void OnGUI()
// 		{
// 			if (MapRender.instance == null)
// 				return;
//
// 			GUILayout.BeginVertical();
// 			GUILayout.BeginHorizontal();
//
// 			GUILayout.BeginVertical();
// 			GUILayout.Space(10);
// 			GUILayout.Label("关1 : " + seek1.width + "x" + seek1.height);
// 			// seek1.width = (int) GUILayout.HorizontalSlider(seek1.width, 100, 2400);
// 			// seek1.height = (int) GUILayout.HorizontalSlider(seek1.height, 100, 1200);
// 			seek1.width = EditorGUILayout.IntSlider(seek1.width, 100, 2401);
// 			seek1.height = EditorGUILayout.IntSlider(seek1.height, 100, 2401);
// 			
// 			GUILayout.Space(20);
// 			ShowSeekInfo(seek1, "关1");
// 			GUILayout.EndVertical();
//
// 			GUILayout.BeginVertical();
// 			GUILayout.Space(10);
// 			GUILayout.Label("关2 : " + seek2.width + "x" + seek2.width);
// 			seek2.width = (int) GUILayout.HorizontalSlider(seek2.width, 50, seek1.width);
// 			GUILayout.Space(20);
// 			ShowSeekInfo(seek2, "关2");
// 			GUILayout.EndVertical();
//
// 			GUILayout.BeginVertical();
// 			GUILayout.Space(10);
// 			GUILayout.Label("关3 : " + seek3.width + "x" + seek3.width);
// 			seek3.width = (int) GUILayout.HorizontalSlider(seek3.width, 10, seek2.width);
// 			GUILayout.Space(20);
// 			ShowSeekInfo(seek3, "关3");
// 			GUILayout.EndVertical();
//
// 			GUILayout.EndHorizontal();
//
// 			GUILayout.Space(20);
//
// 			GUILayout.BeginHorizontal();
// 			if (GUILayout.Button("立即创建地图", GUILayout.Height(30)))
// 			{
// 				if (_window != null)
// 					_window.Close();
//
// 				if (MapRender.instance != null)
// 				{
// 					MapRender.instance.CreateMap(seek1, seek2, seek3, false);
// 					Hide();
// 				}
// 				else
// 					EditorUtility.DisplayDialog("地图编辑", "请先启动 点击 Play !!!", "确定");
// 			}
//
// 			if (GUILayout.Button("创建空白地图", GUILayout.Height(30)))
// 			{
// 				if (MapRender.instance != null)
// 				{
// 					MapRender.instance.CreateEmpyMap(seek1.width, seek1.height);
// 					Hide();
// 				}
// 				else
// 					EditorUtility.DisplayDialog("地图编辑", "请先启动 点击 Play !!!", "确定");
// 			}
//
// 			// if (GUILayout.Button("单步创建地图",GUILayout.Height(30)))
// 			// {
// 			//     if (MapRender.instance != null)
// 			//         MapRender.instance.CreateMap(seek1, seek2, seek3, true);
// 			//     else
// 			//         EditorUtility.DisplayDialog("地图编辑", "请先启动 点击 Play !!!", "确定");
// 			// }
//
// 			// if (GUILayout.Button("地图单步扩散",GUILayout.Height(30)))
// 			// {
// 			//     if (MapRender.instance != null)
// 			//         MapRender.instance.DoStepExpand();
// 			//     else
// 			//         EditorUtility.DisplayDialog("地图编辑", "请先启动 点击 Play !!!", "确定");
// 			// } 
// 			GUILayout.EndHorizontal();
//
// 			GUILayout.EndVertical();
// 		}
//
// 		void ShowSeekInfo(SeekInfo seek, string tag)
// 		{
// 			GUILayout.BeginVertical();
// 			GUILayout.Label(tag + "取样间隔 : " + seek.seekOffset);
// 			seek.seekOffset = (int) GUILayout.HorizontalSlider(seek.seekOffset, 10, 100);
// 			GUILayout.Label("种子偏移X : " + seek.startOffsetX);
// 			seek.startOffsetX = (int) GUILayout.HorizontalSlider(seek.startOffsetX, 0, 100);
// 			GUILayout.Label("种子偏移Y : " + seek.startOffsetY);
// 			seek.startOffsetY = (int) GUILayout.HorizontalSlider(seek.startOffsetY, 0, 100);
// 			GUILayout.Label("取样间隔 : " + seek.seekOffset);
// 			seek.seekOffset = (int) GUILayout.HorizontalSlider(seek.seekOffset, 10, 100);
// 			GUILayout.Label("随机校正概率 : " + seek.seekAdjustRate);
// 			seek.seekAdjustRate = (int) GUILayout.HorizontalSlider(seek.seekAdjustRate, 0, 100);
// 			GUILayout.Label("随机校正幅度 : " + seek.seekAdjustOffset);
// 			seek.seekAdjustOffset =
// 				(int) GUILayout.HorizontalSlider(seek.seekAdjustOffset, 0, Mathf.FloorToInt(seek.seekOffset * 0.5f));
// 			GUILayout.Label("删除最小间隔 : " + seek.seekDelRandomMin);
// 			seek.seekDelRandomMin = (int) GUILayout.HorizontalSlider(seek.seekDelRandomMin, 0, seek.seekDelRandomMax);
// 			GUILayout.Label("删除最大间隔 : " + seek.seekDelRandomMax);
// 			seek.seekDelRandomMax = (int) GUILayout.HorizontalSlider(seek.seekDelRandomMax, seek.seekDelRandomMin, 10);
// 			GUILayout.EndVertical();
// 		}
// 	}
// }
// #endif