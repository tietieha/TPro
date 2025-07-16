#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class AttributeDlg : EditorWindow
	{
		private static AttributeDlg _window;
		private Vector2 scrollPos = Vector2.zero;
		private static HexMap hexMap = null;
		// [MenuItem("地图编辑/属性工具", false, 200)]
		public static void ShowAttributeDlg()
		{
			if (_window == null)
			{
				_window = GetWindow<AttributeDlg>(true, "属性");
				_window.minSize = new Vector2(200, 500);
				//_window.position = new Rect(Screen.width - 300, 50, 200, 500);
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

		//private static string curSelectFisheryStr = string.Empty;
		public static OperationType curOperationType = OperationType.Unknown;
		private Vector2 fisheScrollPos = Vector2.zero;
		private Vector2 seatScrollPos = Vector2.zero;
		private static int curSelectFishIndex = -1;
		private static int curSelectSeatIndex = -1;
		private static string newSailPointX = string.Empty;
		private static string newSailPointY = string.Empty;
		private static List<Hexagon> fishHexagons = new List<Hexagon>();
		private static List<string> fishBtns = new List<string>();
		private static List<string> seatBtns = new List<string>();
		public static void UpdateFishery()
		{
			hexMap = MapRender.instance.GetMap();
			int fisheryHexIndexOfMap = hexMap.GetCurFisheryIndex();
			Hexagon hexagon = hexMap.GetHexagon(fisheryHexIndexOfMap);
			curSelectFishIndex = -1;
			if (fisheryHexIndexOfMap == -1)
			{
				//curSelectFisheryStr = "当前没有选中任何渔场";
			}
			else
			{


			}
			seatBtns.Clear();
			fishBtns.Clear();
			fishHexagons = hexMap.fishHexgaons;
			for (int i = 0; i < fishHexagons.Count; i++)
			{
				string fishHexPosStr = "(" + fishHexagons[i].x + "," + fishHexagons[i].y + ")";
				fishBtns.Add(fishHexPosStr);
			}

			for (int i = 0; i < fishHexagons.Count; i++)
			{
				if (fishHexagons[i].index == fisheryHexIndexOfMap)
				{
					List<int> seatIndexList = hexMap.fisheryDic[fishHexagons[i]].seatIndexList;
					curSelectFishIndex = i;
					for (int j = 0; j < seatIndexList.Count; j++)
					{
						Hexagon seatHex = hexMap.GetHexagon(hexMap.fisheryDic[fishHexagons[i]].seatIndexList[j]);
						string seatHexPosStr = "(" + seatHex.x + "," + seatHex.y + ")";
						seatBtns.Add(seatHexPosStr);
					}
					break;
				}
			}
		}


		private Color _color;
		private Color fishColor;
		private Color seatColor;

		private bool _isRuin = false;
		private string[] ruinBtns = new[] {"1号", "2号", "3号"};
		private int _selectZone = -1;
		private List<Zone> adjustmentZonesList = new List<Zone>();
		private int lastCount = -1;
		private int curCount = 0;
		private void OnGUI()
		{
			if (MapRender.instance == null)
				return;

			var map = MapRender.instance.GetMap();
			if (map == null)
				return;

			if (MapRender.instance.Operation == OperationType.EditTower)
			{
				if (curOperationType != OperationType.EditTower)
				{
					curOperationType = OperationType.EditTower;
				}
				var color = MapRender.instance.TowerColor;
				GUILayout.BeginVertical();
				GUILayout.Space(10);
				GUILayout.Label("哨塔");
				GUILayout.Space(10);
				_color = EditorGUILayout.ColorField(color);
				GUILayout.Space(10);

				if (MapRender.instance.OperationHex != null && MapRender.instance.OperationHex.zone != null)
				{
					GUILayout.Label($"Zone {MapRender.instance.OperationHex.zone.index}");
					GUILayout.Label($"当前哨塔数量 {MapRender.instance.OperationHex.zone.towers.Count}");
					GUILayout.Label($"实际需要哨塔数量 {MapRender.instance.OperationHex.zone.GetActualNeedsTowerCount()}");
				}

				GUILayout.Space(20);
				GUILayout.Label("Alt + 鼠标左键");

				scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
				adjustmentZonesList = map.GetAdjustmentZonesList();
				curCount = adjustmentZonesList.Count;
				if (lastCount != curCount)
				{
					_selectZone = -1;
					lastCount = curCount;
				}

				if (adjustmentZonesList.Count > 0)
				{
					GUILayout.Label("需要调整的区域");
					string[] btns = new string[adjustmentZonesList.Count];
					for (int i = 0; i < adjustmentZonesList.Count; i++)
					{
						btns[i] = adjustmentZonesList[i].index.ToString();
					}

					var idx = GUILayout.SelectionGrid(_selectZone, btns, 4);

					if (idx != _selectZone)
					{
						_selectZone = idx;
						MapRender.instance.FocusHexagon(adjustmentZonesList[idx].hexagon, -1f);
					}
				}
				EditorGUILayout.EndScrollView();

				GUILayout.EndVertical();

				if (_color != color)
				{
					MapRender.instance.TowerColor = _color;
				}
			}
			else if (MapRender.instance.Operation == OperationType.EditFishery)
			{

				var fishColor = MapRender.instance.FisheColor;
				var seatColor = MapRender.instance.SeatColor;
				GUILayout.BeginVertical();
				GUILayout.Space(10);
				GUILayout.Label("鱼群");
				GUILayout.Space(10);
				fishColor = EditorGUILayout.ColorField(fishColor);
				GUILayout.Space(10);
				GUILayout.Label("Alt + 鼠标左键，创建鱼群");

				GUILayout.Label("垂钓点");
				GUILayout.Space(10);
				seatColor = EditorGUILayout.ColorField(seatColor);
				GUILayout.Space(10);
				GUILayout.Label("Control + 鼠标左键，创建垂钓点");
				if (curOperationType != OperationType.EditFishery)
				{
					curSelectFishIndex = -1;
					curOperationType = OperationType.EditFishery;
					UpdateFishery();
					seatBtns.Clear();
					fishBtns.Clear();
					fishHexagons = MapRender.instance.GetMap().fishHexgaons;
					for (int i = 0; i < fishHexagons.Count; i++)
					{
						string fishHexPosStr = "(" + fishHexagons[i].x + "," + fishHexagons[i].y + ")";
						fishBtns.Add(fishHexPosStr);
					}

				}
				GUILayout.Space(10);
				//GUILayout.Label(curSelectFisheryStr,EditorStyles.wordWrappedLabel);
				GUILayout.Label("鱼群列表：");
				//鱼群按钮列表
				fisheScrollPos = EditorGUILayout.BeginScrollView(fisheScrollPos);
				var idxFish = GUILayout.SelectionGrid(curSelectFishIndex, fishBtns.ToArray(), 4);
				if (idxFish != curSelectFishIndex)
				{
					hexMap = MapRender.instance.GetMap();
					curSelectFishIndex = idxFish;
					MapRender.instance.FocusHexagon(fishHexagons[idxFish], -1f);
					hexMap.SetCurFisheryIndex(fishHexagons[idxFish].index);
					List<int> seatIndexList = hexMap.fisheryDic[fishHexagons[idxFish]].seatIndexList;
					seatBtns.Clear();
					for (int i = 0; i < seatIndexList.Count; i++)
					{
						Hexagon seatHex = hexMap.GetHexagon(hexMap.fisheryDic[fishHexagons[idxFish]].seatIndexList[i]);
						string seatHexPosStr = "(" + seatHex.x + "," + seatHex.y + ")";
						seatBtns.Add(seatHexPosStr);
					}
				}
				EditorGUILayout.EndScrollView();
				GUILayout.Label("垂钓点列表：");
				seatScrollPos = EditorGUILayout.BeginScrollView(seatScrollPos);
				var idxSeat = GUILayout.SelectionGrid(curSelectSeatIndex, seatBtns.ToArray(), 4);
				if (idxSeat != curSelectSeatIndex)
				{
					curSelectSeatIndex = idxSeat;
					int seatIndexOfMap = hexMap.fisheryDic[fishHexagons[idxFish]].seatIndexList[idxSeat];
					Hexagon seatHex = hexMap.GetHexagon(seatIndexOfMap);
					MapRender.instance.FocusHexagon(seatHex, -1f);
				}
			    EditorGUILayout.EndScrollView();

			}
			else if (MapRender.instance.OperationHex != null)
			{
				var zone = MapRender.instance.OperationHex.zone;
				if (zone != null)
				{
					var color = map.GetZoneColor(zone.color);
					color.a = 1f;
					GUILayout.BeginVertical();
					GUILayout.Label($"地格 [{MapRender.instance.OperationHex.x},{MapRender.instance.OperationHex.y}]");
					GUILayout.Label($"下标 [{MapRender.instance.OperationHex.index}]");
					using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
					{
						GUILayout.Label("阻挡");
						MapRender.instance.OperationHex.blockFlag = (BlockFlag)GUILayout.SelectionGrid(
							(int)MapRender.instance.OperationHex.blockFlag, GlobalDef.S_BlockFlagDes,
							GlobalDef.S_BlockFlagDes.Length);
					}

					GUILayout.Space(20);
					GUILayout.Label($"区域 [ {zone.index} ] ");
					_color = EditorGUILayout.ColorField(color);
					if (_color != color)
					{
						map.SetZoneColor(zone.index, _color);
					}

					GUILayout.Label("邻接区域");
					if (zone.neigbourZones != null && zone.neigbourZones.Count > 0)
					{
						string[] btns = new string[zone.neigbourZones.Count];
						for (int i = 0; i < zone.neigbourZones.Count; i++)
						{
							btns[i] = zone.neigbourZones[i].index.ToString();
						}

						var idx = GUILayout.SelectionGrid(_selectZone, btns, 4);
						if (idx != _selectZone)
						{
							_selectZone = idx;
							MapRender.instance.FocusHexagon(zone.neigbourZones[idx].hexagon, -1f);
						}
					}

					GUILayout.Space(10);

					GUILayout.Space(20);
					GUILayout.Label($"城市位置 [{zone.hexagon.x},{zone.hexagon.y}]");
					GUILayout.Label($"包含地格 {zone.hexagons.Count}");
					//GUILayout.BeginHorizontal();
					GUILayout.Label($"城市等级 {zone.level}");
					zone.level = (int)GUILayout.HorizontalSlider(zone.level, 0, 7);
					//GUILayout.EndHorizontal();
					GUILayout.Space(10);
					zone.visible = GUILayout.Toggle(zone.visible == 1, "显示城点") ? 1 : 0;
					zone.isGuanqia = GUILayout.Toggle(zone.isGuanqia == 1, "是否关卡") ? 1 : 0;
					zone.isBorn = GUILayout.Toggle(zone.isBorn == 1, "是否出生城") ? 1 : 0;
					zone.subType = GUILayout.Toggle(zone.subType == 1, "是否末日实验室") ? 1 : 0;
					GUILayout.Space(20);
					//GUILayout.BeginHorizontal();
					_isRuin = zone.ruinId > 0 ? true : false;
					_isRuin = GUILayout.Toggle(_isRuin, "是否遗迹");
					if (_isRuin)
					{
						zone.ruinId = math.clamp(zone.ruinId, 1, 1000);
						zone.ruinId = GUILayout.TextField(zone.ruinId.ToString()).ToInt();
						//zone.ruinId = GUILayout.SelectionGrid(zone.ruinId - 1, ruinBtns, 3) + 1;
					}
					else
					{
						zone.ruinId = 0;
					}

					GUILayout.Space(20);

					// GUILayout.Label("设置遗迹： " + zone.ruinId);
					// zone.ruinId = (int) GUILayout.HorizontalSlider(zone.ruinId, 0, 3);
					//GUILayout.EndHorizontal();
					using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
					{
						GUILayout.Label("地貌");
						// zone.landform = (ZoneLandform) EditorGUILayout.EnumPopup("地貌", zone.landform);
						zone.landform = (ZoneLandform)GUILayout.SelectionGrid((int)zone.landform,
							GlobalDef.S_ZoneLandformDes,
							GlobalDef.S_ZoneLandformDes.Length);
					}

					var hex = MapRender.instance.OperationHex;
					hexMap = MapRender.instance.GetMap();
					using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
					{
						GUILayout.Label("属性");
						hex.attribute = (HexAttribute)GUILayout.SelectionGrid((int)hex.attribute,
							GlobalDef.S_HexAttributeDes,
							5);
					}

					if (hex.attribute == HexAttribute.SailPoint)
					{
						hexMap.AddSailPoint(hex.index);
						var count = hexMap.connect[hex.index].Count + 1;
						var removeIdxs = new HashSet<int>();
						foreach (var idx in hexMap.connect[hex.index])
						{
							GUILayout.BeginHorizontal();
							GUILayout.Label("关联航道点");
							GUILayout.Space(20);
							var targetHex = hexMap.GetHexagon(idx);
							if (GUILayout.Button($"[{targetHex.x} , {targetHex.y}]"))
							{
								MapRender.instance.FocusHexagon(targetHex, -1f);
								MapRender.instance.SetOperationHex(targetHex);
							}
							if (GUILayout.Button("移除"))
							{
								removeIdxs.Add(idx);
							}
							GUILayout.EndHorizontal();
						}

						foreach (var idx in removeIdxs)
						{
							hexMap.RemoveConnect(hex.index, idx);
						}

						GUILayout.BeginHorizontal();
						GUILayout.Label("X:");
						newSailPointX = GUILayout.TextField(newSailPointX);
						// GUILayout.Space(20);
						GUILayout.Label("Y:");
						newSailPointY = GUILayout.TextField(newSailPointY);
						if (GUILayout.Button("新增关联航道点"))
						{

							hexMap.AddConnect(hex.index, newSailPointX.ToInt(), newSailPointY.ToInt());
							newSailPointX = string.Empty;
							newSailPointY = string.Empty;
						}
						GUILayout.EndHorizontal();
					}
					else
					{
						hexMap.RemoveSailPoint(hex.index);
					}

					GUILayout.EndVertical();
				}
			}

			if (MapRender.instance.OperationTargetZone != null)
			{
				GUILayout.Space(40);
				GUILayout.BeginVertical();
				GUILayout.Label($"编辑区域 [ {MapRender.instance.OperationTargetZone.index} ] ");

				GUILayout.BeginHorizontal();
				var color = map.GetZoneColor(MapRender.instance.OperationTargetZone.color);
				_color = EditorGUILayout.ColorField(color);
				if (_color != color)
				{
					map.SetZoneColor(MapRender.instance.OperationTargetZone.index, _color);
				}

				if (GUILayout.Button("添加模具"))
				{
					MapRender.instance.PatternManager.AddPatternZone(MapRender.instance.OperationTargetZone);
					PatternTool.UpdateUI();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
		}
	}
}
#endif