#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class MapToolBar : EditorWindow
	{
		private static EditorWindow _window;

		[MenuItem("地图编辑/迷雾开关", false, -2)]
		public static void CloudSwitch()
		{
			var fogOn = Shader.GetGlobalFloat("G_PVEFogOfWar_On");
			if (fogOn > 0)
			{
				Shader.SetGlobalFloat("G_PVEFogOfWar_On", 0);
			}
			else
			{
				Shader.SetGlobalFloat("G_PVEFogOfWar_On", 1);
			}
		}
		[MenuItem("地图编辑/地图格子开关", false, -1)]
		public static void HexGridSwitch()
		{
			var hexGridOn = Shader.IsKeywordEnabled("ENABLE_HEX_GRID");
			if (hexGridOn)
			{
				Shader.DisableKeyword("ENABLE_HEX_GRID");
			}
			else
			{
				Shader.EnableKeyword("ENABLE_HEX_GRID");
			}
		}


		[MenuItem("地图编辑/打开编辑器", false, -1)]
		public static void OpenTool()
		{
			// GlobalDef.S_MapEditorType = MapEditorType.EBigWorld;
			EditorPrefs.SetInt("MapEditorType", (int)MapEditorType.EBigWorld);
			EditorSceneManager.OpenScene("Assets/3rdParty/GreateWorld/MapEditor/MapEditor.unity");
			EditorApplication.isPlaying = true;
		}

		[MenuItem("地图编辑/打开PVE地图编辑器", false, -1)]
		public static void OpenPVEMapEditor()
		{
			// GlobalDef.S_MapEditorType = MapEditorType.EPVE;
			EditorPrefs.SetInt("MapEditorType", (int)MapEditorType.EPVE);
			EditorSceneManager.OpenScene("Assets/3rdParty/GreateWorld/MapEditor/MapEditor.unity");
			EditorApplication.isPlaying = true;
		}

		// [MenuItem("地图编辑/打开地图", false, 1)]
		public static void LoadMap()
		{
			if (CheckValid()) MapRender.instance.OnMenu_LoadMap();
		}

		[MenuItem("地图编辑/保存地图", false, 2)]
		public static void SaveMap()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SaveMap();
		}

		// [MenuItem("地图编辑/导出/导出线图")]
		public static void SavePictures()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SavePictures();
		}

		// [MenuItem("地图编辑/导出/导出整体线图")]
		public static void ExportBigImage()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportBigImage();
		}

		// [MenuItem("地图编辑/导出/导出像素图")]
		public static void SavePictures1()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SavePictures1();
		}

		[MenuItem("地图编辑/导出/导出像素全图")]
		public static void SavePictures2()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SavePictures2();
		}

		/*[MenuItem("地图编辑/导出/导出地貌散图")]
		public static void SaveLandformPicture()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SaveLandformPicture();
		}*/

		// [MenuItem("地图编辑/导出/导出地貌全图")]
		public static void SaveFullLandformPicture()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SaveFullLandformPicture();
		}

		[MenuItem("地图编辑/导出/导出阻挡图")]
		public static void SaveBlockPicture()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SaveBlockPicture();
		}

		/*[MenuItem("地图编辑/导出/导出末日实验室标记图")]
		public static void SaveZoneSubtypePicture()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SaveZoneSubtypePicture();
		}*/

		[MenuItem("地图编辑/导出/导出Server")]
		public static void ExportToServer()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportToServer();
		}

		[MenuItem("地图编辑/导出/导出Client")]
		public static void ExportToClient()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportToClient();
		}

		// [MenuItem("地图编辑/导出/导出世界CSV")]
		public static void ExportToCSV()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportToCSV();
		}

		// [MenuItem("地图编辑/导出/导出渔场CSV")]
		public static void ExportToFisheryCSV()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportToFisheryCSV();
		}

		// [MenuItem("地图编辑/导出/导出海港CSV")]
		public static void ExportCityToCSV()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportCityToCSV();
		}

		/*[MenuItem("地图编辑/导出/导出Json")]
		public static void ExportToJson()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportToJson();
		}*/

		// [MenuItem("地图编辑/导出/导出全部所需文件")]
		public static void ExportAll()
		{
			if (CheckValid()) MapRender.instance.OnMenu_ExportAll();
		}

		// [MenuItem("地图编辑/加载Json地图", false, 3)]
		public static void LoadFromJson()
		{
			if (CheckValid()) MapRender.instance.OnMenu_LoadFromJson();
		}

		// [MenuItem("地图编辑/加载模具", false, 300)]
		public static void LoadPattern()
		{
			if (CheckValid())
			{
				MapRender.instance.OnMenu_LoadPattern();
				if (!PatternTool.IsVisible()) PatternTool.ShowPatternTool();
			}
		}

		// [MenuItem("地图编辑/加载旧版模具", false, 300)]
		public static void LoadOldPattern()
		{
			if (CheckValid())
			{
				MapRender.instance.OnMenu_LoadOldPattern();
				if (!PatternTool.IsVisible()) PatternTool.ShowPatternTool();
			}
		}

		// [MenuItem("地图编辑/保存模具", false, 301)]
		public static void SavePattern()
		{
			if (CheckValid()) MapRender.instance.OnMenu_SavePattern();
		}

		// [MenuItem("地图编辑/检查飞地", false, 100)]
		public static void CheckEnclave()
		{
			if (CheckValid()) MapRender.instance.CheckEnclave();
		}

		private static bool CheckValid()
		{
			if (MapRender.instance == null)
			{
				EditorUtility.DisplayDialog("地图编辑", "请先启动 点击 Play !!!", "确定");
				return false;
			}

			return true;
		}


		// [MenuItem("地图编辑/编辑工具", false, 202)]
		public static void ShowToolBar()
		{
			if (_window == null)
			{
				_window = GetWindow<MapToolBar>(true, "编辑工具");
				_window.minSize = new Vector2(120, 650);
				//_window.position = new Rect(50, 50, 120, 650);
				_window.ShowUtility();
			}
			else
			{
				_window.Close();
			}
		}

		[MenuItem("地图编辑/TerrainToMesh", false, 101)]
		[MenuItem("GameObject/TerrainToMesh", priority = 12)]
		public static void TerrainToMesh()
		{
			EditorApplication.ExecuteMenuItem("Window/Amazing Assets/Terrain To Mesh");
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

		private string inputValue = "";
		private static int _selected = 0;
		private string[] _buttons = new[]
			{"移动地图", "编辑地格", "填充地格", "合并区域", "拆分区域", "调整主城", "设置哨塔", "使用模具", "编辑地貌", "编辑阻挡", "设置渔场", "设置航线", "编辑岛屿模板"};

		private OperationType GetOperationBySelect(int index)
		{
			OperationType ret = OperationType.Unknown;
			switch (index)
			{
				case 1:
					ret = OperationType.EditColor;
					break;
				case 2:
					ret = OperationType.EditFill;
					break;
				case 3:
					ret = OperationType.EditZone;
					break;
				case 4:
					ret = OperationType.EditCut;
					break;
				case 5:
					ret = OperationType.EditCity;
					break;
				case 6:
					ret = OperationType.EditTower;
					break;
				case 7:
					ret = OperationType.EditPattern;
					break;
				case 8:
					ret = OperationType.EditLandform;
					break;
				case 9:
					ret = OperationType.EditBlock;
					break;
				case 10:
					ret = OperationType.EditFishery;
					break;
				case 11:
					ret = OperationType.EditSailLine;
					break;
				case 12:
					ret = OperationType.EditIsland;
					break;
			}

			return ret;
		}

		public static void SetSelect(int sel)
		{
			_selected = sel;
		}

		private void OnGUI()
		{
			if (MapRender.instance == null)
				return;

			var map = MapRender.instance.GetMap();
			if (map == null)
				return;

			GUILayout.BeginVertical();

			GUILayout.Space(20);
			_selected = GUILayout.SelectionGrid(_selected, _buttons, 1, GUILayout.Height(200));
			MapRender.instance.Operation = GetOperationBySelect(_selected);
			GUILayout.Space(20);

			OnHelperGUI();

			GUILayout.Space(20);

			MapRender.instance.fHexagonAlpha = EditorGUILayout.Slider("网格透明度", MapRender.instance.fHexagonAlpha, 0, 1);
			MapRender.instance.ShowGrid = GUILayout.Toggle(MapRender.instance.ShowGrid, "显示网格");
			MapRender.instance.ShowGridGround = GUILayout.Toggle(MapRender.instance.ShowGridGround, "网格是否贴地");
			MapRender.instance.bShowCity = GUILayout.Toggle(MapRender.instance.bShowCity, "显示城点");
			MapRender.instance.bShowEdge = GUILayout.Toggle(MapRender.instance.bShowEdge, "显示边界");
			MapRender.instance.bShowWireframe = GUILayout.Toggle(MapRender.instance.bShowWireframe, "显示边线");
			EditorGUILayout.BeginHorizontal();
			{
				MapRender.instance.bShowBlock = GUILayout.Toggle(MapRender.instance.bShowBlock, "显示阻挡");
				EditorGUILayout.BeginVertical();
				{
					if (GUILayout.Button("重新解析阻挡图"))
					{
						map.ResetBlock();
					}
					if (GUILayout.Button("统计阻挡数量"))
					{
						map.CountAllBlock();
					}
					GUILayout.Label($"全图阻挡数量:{map.BlockCount}");
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(20);

			GUILayout.Label("城点起始ID");
			inputValue = GUILayout.TextField(inputValue);
			if (GUILayout.Button("重刷城点ID"))
			{
				var startId = int.Parse(inputValue);
				Debug.Log("input is : " + startId);
				if (startId <= 0)
				{
					Debug.LogError("输入的起始ID无效！！！");
				}
				else
				{
					map.ResetZoneId(startId);
				}
			}

			GUILayout.Space(20);

			if (GUILayout.Button("刷新边界", GUILayout.Height(30)))
			{
				MapRender.instance.CalcEdges();
			}

			if (GUILayout.Button("检查飞地", GUILayout.Height(30)))
			{
				MapRender.instance.CheckEnclave();
			}

			GUILayout.EndVertical();
			OnRefreshNeedAdjustmentZonesEditGUI();
			OnEditGUI();
			OnMapAttributeGUI();
		}

		private void OnMapAttributeGUI()
		{
			HexMap hexMap = MapRender.instance.GetMap();
			hexMap.mapType = (MapType)EditorGUILayout.Popup((int)hexMap.mapType, GlobalDef.S_MapTypeDes);
			if (hexMap.mapType == MapType.ESmallWorld)
			{
				hexMap.startHexPos = EditorGUILayout.Vector2IntField("小地图开始坐标", hexMap.startHexPos);
				hexMap.endHexPos = EditorGUILayout.Vector2IntField("小地图结束坐标", hexMap.endHexPos);
			}
		}

		private static int selRotateInt = 0;
		private static string[] rotateString = {"0°", "60°", "120°", "180°", "240°", "300°"};
		private void OnHelperGUI()
		{
			if (MapRender.instance.Operation == OperationType.EditLandform)
			{
				EditorGUILayout.HelpBox($"编辑地貌\n" +
				                        $"左ctrl + 左键点击 快速设置地貌 TO {MapRender.instance.landform.GetDescription()}",
					MessageType.Info);
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField("快捷设置");
					MapRender.instance.landform =
						(ZoneLandform) EditorGUILayout.Popup((int) MapRender.instance.landform,
							GlobalDef.S_ZoneLandformDes);
				}
				EditorGUILayout.EndHorizontal();
			}
			else if (MapRender.instance.Operation == OperationType.EditBlock)
			{
				EditorGUILayout.HelpBox($"编辑阻挡\n" +
				                        $"左ctrl  + 左键点击 快速设置选中地格\n" +
				                        $"左ctrl  + alt + 左键点击 快速设置选中地格所属的zone\n" +
				                        $"左shift + 左键点击拖动 快速设置选中区域地格", MessageType.Info);
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField("快捷设置");
					MapRender.instance.blockFlag = (BlockFlag) EditorGUILayout.Popup((int) MapRender.instance.blockFlag,
						GlobalDef.S_BlockFlagDes);
				}
				EditorGUILayout.EndHorizontal();
			}
			else if (MapRender.instance.Operation == OperationType.EditFill)
			{

				EditorGUILayout.HelpBox("先Alt+左键选中地块，然后按住Ctrl拖拽鼠标使用笔刷。", MessageType.Info);

			}
			else if (MapRender.instance.Operation == OperationType.EditSailLine)
			{
				EditorGUILayout.HelpBox("先Ctrl+左键航道点, 然后按住Ctrl拖动到另外一个航道点。", MessageType.Info);
			}
			else if (MapRender.instance.Operation == OperationType.EditIsland)
			{
				if (MapRender.instance.islandTemplate != null)
				{
					EditorGUILayout.HelpBox("红色地格为岛屿中心点,不可取消,默认占地,岛屿旋转基于此地格。 ", MessageType.Info);
					EditorGUILayout.HelpBox("Ctrl+鼠标左键标记岛屿占地地格, Ctrl+鼠标右键标记可放置港口的地格。 ", MessageType.Info);
					EditorGUILayout.HelpBox("重复操作可撤销。 ", MessageType.Info);
					EditorGUILayout.HelpBox("旋转的时候不支持增减地格。 ", MessageType.Warning);

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("模板ID");
					MapRender.instance.islandTemplate._Id = EditorGUILayout.TextField($"{MapRender.instance.islandTemplate._Id}").ToInt();
					EditorGUILayout.EndHorizontal();

					if (GUILayout.Button("添加岛屿模板"))
					{
						var map = MapRender.instance.GetMap();
						var id = MapRender.instance.islandTemplate._Id;
						if (map.islandTemplates.ContainsKey(id))
						{
							EditorUtility.DisplayDialog("添加岛屿模板", $"模板ID重复!!!", "确定");
							return;
						}

						map.AddIsland(MapRender.instance.islandTemplate);
						MapRender.instance.islandTemplate = null;
					}

					EditorGUILayout.LabelField("旋转|模板不保存旋转 为了方便看效果");
					selRotateInt = GUILayout.SelectionGrid(selRotateInt, rotateString, 6);
					if (MapRender.instance.islandTemplate != null)
					{
						MapRender.instance.islandTemplate.Rotate(selRotateInt);
					}
				}
			}
		}


		public static bool isSetupNeedAdjustmentZones = true;


		private void OnRefreshNeedAdjustmentZonesEditGUI()
		{
			if (MapRender.instance.Operation == OperationType.EditTower)
			{

			}
			else
			{
				isSetupNeedAdjustmentZones = true;
			}
		}

		private void OnEditGUI()
		{
			if (MapRender.instance.Operation == OperationType.EditColor)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
				GUILayout.Label($"笔刷尺寸 {MapRender.instance.EditColorRange} 格");
				MapRender.instance.EditColorRange =
					(int) GUILayout.HorizontalSlider(MapRender.instance.EditColorRange, 1, 50);
				GUILayout.EndVertical();
			}
			else if (MapRender.instance.Operation == OperationType.EditPattern)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
				if (MapRender.instance.Pattern == null)
				{
					GUILayout.Label("未加载模具");
					if (GUILayout.Button("加载模具"))
					{
						if (!PatternTool.IsVisible()) PatternTool.ShowPatternTool();
					}
				}
				else
				{
					GUILayout.Label(MapRender.instance.Pattern.Name);
					GUILayout.Box(MapRender.instance.Pattern.Icon, GUILayout.Width(_window.position.width - 10),
						GUILayout.Height(_window.position.width - 10));
				}

				GUILayout.EndVertical();
			}
			else if (MapRender.instance.Operation == OperationType.EditTower)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
				if (isSetupNeedAdjustmentZones == true)
				{
					MapRender.instance.SetupNeedAdjustmentZones();
					isSetupNeedAdjustmentZones = false;
				}

				if (GUILayout.Button("快捷设置哨塔"))
				{
					if (EditorUtility.DisplayDialog("快捷设置哨塔", "此操作会重新生成所有哨塔，是否要继续", "继续", "取消"))
					{
						MapRender.instance.QuicklySetupTowers();
					}
					else
					{

					}
				}
				GUILayout.EndVertical();
			}
		}
	}
}
#endif