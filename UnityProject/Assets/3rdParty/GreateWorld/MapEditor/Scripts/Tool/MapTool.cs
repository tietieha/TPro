// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-06-21 15:15 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DG.Tweening.Plugins.Core.PathCore;
using TEngine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Path = System.IO.Path;

namespace GEngine.MapEditor
{
    public partial class MapTool : EditorWindow
    {
        static MapTool _window = null;

        public static MapTool Instance
        {
            get { return _window; }
        }

        bool _wantsRepaint = false;
        private Vector2Int _focusCood;
        private int _focusZone;


        public MapToolOP tool = MapToolOP.CreateMap;
        private HexMap _map;

        // ================================================UW Map=====================================================
        private string currentMap = "无";
        private Vector2 _scrollMaplstPos;
        private List<string> _mapLst = new List<string>();
        private List<string> _mapPathLst = new List<string>();
        private int currentMapId = -1;
        private int currentPage = 0;
        private int itemsPerPage = 5;

        // =================================================场景=======================================================
        private const string WORLD_RES_FOLDER = "Assets/GameAssets/Scenes/Campaign";

        private const string WORLD_RES_RENDER_DATA_PATH =
            "Assets/GameAssets/Scenes/Campaign/{0}/prefabs/WorldResRenderData.prefab";

        private const string WORLD_RES_Prefab_PATH =
            "Assets/GameAssets/Scenes/Campaign/{0}/Prefab/{0}.prefab";

        private const string WORLD_MAP_Prefab_PATH =
            "Assets/GameAssets/Scenes/Campaign/{0}/prefabs/{0}_map.prefab";
        private const string WORLD_PROP_Prefab_PATH =
            // "Assets/GameAssets/Scenes/Campaign/Campaign01/prefabs/Campaign01_prop.prefab"
            "Assets/_Test/Scenes/Campaign/{0}/prefabs/{0}_prop.prefab";

        // 编写场景列表
        private List<string> _prefabSearchList = new List<string>()
        {
            WORLD_MAP_Prefab_PATH,
            WORLD_PROP_Prefab_PATH
        };

        private string currentScene = "无";
        private GameObject _currentMapGO;
        private Vector2 _scrollScenelstPos;
        private Vector2 _scrollZoneLastPos;
        private List<string> _sceneLst = new List<string>();

        // =================================================Attribute===============================================
        private int _selectHex = -1;
        private int _selectZone = -1;
        private int _changeZoneIndex = -1;

        private Color _color;

        // ==========================================================================================================
        // 地图创建 宽高
        private Vector2Int _mapsize = new Vector2Int(0, 0);
        private Vector2Int _mapMeterSize = new Vector2Int(600, 600);
        private string _mapName = string.Empty;
        private int _mapId = 0;

        // =================================================编辑陆地=====================================================
        // 设计图
        private Texture2D _mapTex;
        public MOVETYPE _EditLand_MoveType { get; private set; }
        public int _Tool_BrushRound { get; private set; } = 5;

        // =================================================海域分区=====================================================
        private AreaScatterSetting areaCenter;
        private AreaScatterSetting area1;
        private AreaScatterSetting area2;
        private AreaScatterSetting area3;

        private Zone _curZone;

        private Zone _mergeZone;

        // =================================================深浅海=====================================================
        public MOVETYPE _EditSea_MoveType
        {
            get { return _editsea_movetypes[_editsea_movetype_select]; }
        }

        private int _editsea_movetype_select = 0;

        private MOVETYPE[] _editsea_movetypes = new MOVETYPE[]
        {
            MOVETYPE.SHALLOWSEA,
            MOVETYPE.DEEPSEA
        };

        private string[] _editsea_movetypes_des = new[]
        {
            "浅海",
            "深海"
        };

        private int _landAffectRount = 0;
        private MovetypeScatterSetting _shallowScatterSetting;

        private MovetypeScatterSetting _deepScatterSetting;

        private int _postypeSplitEdgeExpandWidth = 3;

        // =================================================阻挡=====================================================
        private Texture2D _blockTex;
        public BlockFlag _Edit_BlockFlag { get; private set; }

        // =================================================地貌=====================================================
        public ZoneLandform _EditLand_Form { get; private set; }


        // ================================================航道点=====================================================
        private static string newSailPointX = string.Empty;
        private static string newSailPointY = string.Empty;
        private static string newHarborIndex = string.Empty;
        private static string newHarborSailIndex = string.Empty;

        // =================================================商圈=====================================================
        //商圈编辑
        private Zone _curBusinessZone;

        // =================================================撒点=====================================================


        // =================================================迷雾=====================================================
        public int _EditFog_ID = -1;

        [MenuItem("地图编辑/工具窗口")]
        public static void ShowWindow()
        {
            _window = GetWindow<MapTool>();
            _window.titleContent = new GUIContent("地图编辑");
            _window.Show();

            GUIStyle styleApply = new GUIStyle("ObjectPickerBackground");
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

        private void OnEnable()
        {
            RefreshMap();
            RefreshScene();
        }

        #region MapTool

        void RefreshMap()
        {
            if (GlobalDef.S_MapEditorType == MapEditorType.EPVE)
            {
                RefreshMapPVE();
            }
            else
            {
                // RefreshMapAll();
                RefreshMapWorld();
            }
        }

        void RefreshMapAll()
        {
            _mapLst.Clear();
            _mapPathLst.Clear();
            if (MapRender.instance.CheckWorkSpaceValid())
            {
                DirectoryInfo rootFolder = new DirectoryInfo(MapRender.instance.WorkSpace);
                var files = rootFolder.GetFiles($"*{GlobalDef.S_UWMapExt}",
                    SearchOption.AllDirectories);
                foreach (var fileInfo in files)
                {
                    _mapLst.Add(fileInfo.Name.Replace(GlobalDef.S_UWMapExt, ""));
                    _mapPathLst.Add(fileInfo.FullName);
                }
            }
        }

        void RefreshMapWorld()
        {
            _mapLst.Clear();
            _mapPathLst.Clear();

            if (MapRender.instance.CheckWorkSpaceValid())
            {
                DirectoryInfo rootFolder = new DirectoryInfo(MapRender.instance.WorkSpace);
                var files = rootFolder.GetFiles($"*{GlobalDef.S_UWMapExt}", SearchOption.AllDirectories);

                foreach (var fileInfo in files)
                {
                    string mapName = fileInfo.Name.Replace(GlobalDef.S_UWMapExt, "");
                    if (mapName.Contains("BigWorld"))
                    {
                        _mapLst.Add(mapName);
                        _mapPathLst.Add(fileInfo.FullName);
                    }
                }
            }
        }

        void RefreshMapPVE()
        {
            if (MapRender.instance.CheckWorkSpaceValid())
            {
                if (MapRender.instance.CampaignMapList.Count == 0)
                {
                    Log.Error("没有PVE地图信息，请先加载PVE地图 Campaign_Map_Info.xlsx");
                    return;
                }
                DirectoryInfo rootFolder = new DirectoryInfo(MapRender.instance.WorkSpace);
                var files = rootFolder.GetFiles($"*{GlobalDef.S_UWMapExt}",
                    SearchOption.AllDirectories);
                foreach (var fileInfo in files)
                {
                    var sceneName = fileInfo.Name.Replace(GlobalDef.S_UWMapExt, "");
                    // 检查MapRender.instance.CampaignMapList里面是否有对象的name为sceneName, 如果有设置路径
                    var campaignMapInfo = MapRender.instance.CampaignMapList
                        .Find(m=> m != null && m.Name == sceneName);
                    if (campaignMapInfo != null)
                    {
                        campaignMapInfo.MapUWFilePath = fileInfo.FullName;
                        campaignMapInfo.PrefabFilePath = string.Format(WORLD_MAP_Prefab_PATH, sceneName);
                        campaignMapInfo.RenderDataFilPath = string.Format(WORLD_RES_RENDER_DATA_PATH, sceneName);
                    }
                }
            }
        }

        private void OpenPVEMap(int index)
        {
            if (MapRender.instance.CampaignMapList.Count == 0)
            {
                Log.Error("没有PVE地图信息，请先加载PVE地图 Campaign_Map_Info.xlsx");
                return;
            }

            if (MapRender.instance.CampaignMapList[index] == null)
            {
                EditorUtility.DisplayDialog("Error", "确认地图存在", "OK");
                return;
            }

            //加载uuwmap文件
            var mapInfo = MapRender.instance.CampaignMapList[index];
            if (File.Exists(mapInfo.MapUWFilePath))
                MapRender.instance.LoadMap(mapInfo.MapUWFilePath, mapInfo.Id);
            else
            {
                if (currentMap == mapInfo.Name)
                {
                    Log.Info("当前地图已加载，无需重复加载");
                }
            }
            currentMap = mapInfo.Name;
            var oldMapId = currentMapId;
            currentMapId = mapInfo.Id;
            MapRender.instance.CurrentMapId = mapInfo.Id;
            try
            {
                if (MapRender.instance.worldTestRoot != null)
                    MapRender.instance.worldTestRoot.UnInit();

                if (_currentMapGO != null)
                    DestroyImmediate(_currentMapGO);

                EditorUtility.DisplayProgressBar("加载场景", "load res ", 0.1f);

                string mapName = mapInfo.Name;
                string mapPath = mapInfo.RenderDataFilPath;
                if (File.Exists(mapPath))
                {
                    currentScene = mapName;
                    // load res render data
                    ResRenderData resRenderData =
                        AssetDatabase.LoadAssetAtPath<ResRenderData>(mapPath);
                    EditorUtility.DisplayProgressBar("加载场景", "load res ", 0.4f);
                    // initlogic
                    // isLoading = true;
                    MapRender.instance.worldTestRoot.Init(
                        resRenderData,
                        new Vector3(0, 0, 0),
                        () =>
                        {
                            // isLoading = false;
                            EditorUtility.ClearProgressBar();
                        });
                    string mapBasePath = mapInfo.PrefabFilePath;
                    if (File.Exists(mapBasePath))
                    {
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapBasePath);
                        if (prefab != null)
                        {
                            _currentMapGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                        }
                    }
                }
                else
                {
                    foreach (var prefabPath in
                             _prefabSearchList)
                    {
                        string formattedPath = string.Format(prefabPath, mapName);
                        if (File.Exists(formattedPath))
                        {
                            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(formattedPath);
                            if (prefab != null)
                            {
                                _currentMapGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // isLoading = false;

                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                if (oldMapId > 0 && oldMapId != currentMapId)
                {
                    MapEditorEventCenter.SendEvent(MapEditorEvent.InteractionReloadEvent);
                }
            }
        }

        private void OpenMap(int index)
        {
            if (index >= _mapPathLst.Count)
            {
                EditorUtility.DisplayDialog("Error", "确认地图存在", "OK");
                return;
            }

            MapRender.instance.LoadMap(_mapPathLst[index]);
            currentMap = _mapLst[index];
            if (MapRender.instance.map.mapType == MapType.EGreatWorld)
            {
                // 弹窗确认是否加载三色图
                bool loadTerritory = EditorUtility.DisplayDialog(
                    "加载提示",
                    "是否加载地貌图？",
                    "加载",
                    "取消"
                );

                if (loadTerritory)
                {
                    var territoryPath = "Assets/_Test/Scenes/BigWorld/Map/Prefab/WorldTerritory.prefab";
                    if (File.Exists(territoryPath))
                    {
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(territoryPath);
                        if (prefab != null)
                        {
                            var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                            go.transform.SetParent(MapRender.instance.worldTestRoot.transform);
                            var scale = go.transform.localScale;
                            go.transform.localPosition = new Vector3(scale.x / 0.2f, 0, -scale.z / 0.2f);
                        }
                    }
                }
            }
        }

        void RefreshScene()
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

        private void OpenScene(int index)
        {
            try
            {
                if (MapRender.instance.worldTestRoot != null)
                    MapRender.instance.worldTestRoot.UnInit();

                if (_currentMapGO != null)
                    DestroyImmediate(_currentMapGO);

                EditorUtility.DisplayProgressBar("加载场景", "load res ", 0.1f);

                string mapName = _sceneLst[index];
                string mapPath = string.Format(WORLD_RES_RENDER_DATA_PATH, mapName);
                if (File.Exists(mapPath))
                {
                    currentScene = _sceneLst[index];
                    // load res render data
                    ResRenderData resRenderData =
                        AssetDatabase.LoadAssetAtPath<ResRenderData>(mapPath);
                    EditorUtility.DisplayProgressBar("加载场景", "load res ", 0.4f);
                    // initlogic
                    // isLoading = true;
                    MapRender.instance.worldTestRoot.Init(
                        resRenderData,
                        new Vector3(0, 0, 0),
                        () =>
                        {
                            // isLoading = false;
                            EditorUtility.ClearProgressBar();
                        });
                    string mapBasePath = string.Format(WORLD_MAP_Prefab_PATH, mapName);
                    if (File.Exists(mapBasePath))
                    {
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(mapBasePath);
                        if (prefab != null)
                        {
                            _currentMapGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                        }
                    }
                }
                else
                {
                    foreach (var prefabPath in
                             _prefabSearchList)
                    {
                        string formattedPath = string.Format(prefabPath, mapName);
                        if (File.Exists(formattedPath))
                        {
                            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(formattedPath);
                            if (prefab != null)
                            {
                                _currentMapGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                                break;
                            }
                        }
                    }
                    // GameObject prefab =
                    //     AssetDatabase.LoadAssetAtPath<GameObject>(
                    //         string.Format(WORLD_RES_Prefab_PATH, mapName));
                    // _currentMapGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }
            }
            catch (Exception e)
            {
                // isLoading = false;

                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        #endregion


        private void OnGUI()
        {
            if (MapRender.instance == null)
                return;


            EditorGUILayout.BeginHorizontal();
            {
                DrawToolbar();
                GUILayout.Box("",
                    GUILayout.Width(10),
                    GUILayout.ExpandHeight(true));
                DrawTool();
                GUILayout.Box("",
                    GUILayout.Width(10),
                    GUILayout.ExpandHeight(true));
                DrawAttribute();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnInspectorUpdate()
        {
            if (_wantsRepaint)
            {
                _wantsRepaint = false;
                Repaint();
            }
        }

        void DrawToolbar()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();

            int toolbarIndex = (int)tool;

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.SelectionGrid(toolbarIndex, GlobalDef.S_MapToolOP, 1,
                    GUILayout.Width(100));
            }

            if (EditorGUI.EndChangeCheck())
            {
                SetTool((MapToolOP)toolbarIndex);
            }

            EditorGUILayout.EndVertical();
        }

        void DrawTool()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            DrawHelperGUI();
            DrawEditTool();
            GUILayout.FlexibleSpace();
            DrawDebugTool();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 全局工具
        /// </summary>
        void DrawDebugTool()
        {
            Color preColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("保存地图", GUILayout.Height(40)))
            {
                if (CheckValid()) MapRender.instance.SaveMap();
            }

            GUI.color = preColor;
            if (GUILayout.Button("打开地图(自选路径)"))
            {
                if (CheckValid()) MapRender.instance.OnMenu_LoadMap();
            }

            GUILayout.Box("",
                GUILayout.ExpandWidth(true),
                GUILayout.Height(10));

            EditorGUILayout.BeginHorizontal();
            _focusCood = EditorGUILayout.Vector2IntField("跳转坐标", _focusCood);
            if (GUILayout.Button("跳转"))
            {
                MapRender.instance.FocusHexagon(_focusCood.x, _focusCood.y);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _focusZone = EditorGUILayout.IntField("跳转区域", _focusZone);
            if (GUILayout.Button("跳转"))
            {
                MapRender.instance.FocusZone(_focusZone);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            MapRender.instance.ShowGrid = GUILayout.Toggle(MapRender.instance.ShowGrid, "显示网格");
            MapRender.instance.ShowGridGround = GUILayout.Toggle(MapRender.instance.ShowGridGround, "显示地面网格");
            if (MapRender.instance.ShowGrid)
                MapRender.instance.fHexagonAlpha =
                    EditorGUILayout.Slider("网格透明度", MapRender.instance.fHexagonAlpha, 0, 1);
            EditorGUILayout.EndHorizontal();

            // MapRender.instance.bShowCity = GUILayout.Toggle(MapRender.instance.bShowCity, "显示城点");
            MapRender.instance.bShowEdge = GUILayout.Toggle(MapRender.instance.bShowEdge, "显示边界");
            // MapRender.instance.bShowBusinessEdge =
                // GUILayout.Toggle(MapRender.instance.bShowBusinessEdge, "显示商圈边界");
            MapRender.instance.bShowWireframe =
                GUILayout.Toggle(MapRender.instance.bShowWireframe, "显示边线");
            MapRender.instance.bShowBlock = GUILayout.Toggle(MapRender.instance.bShowBlock, "显示阻挡");
            // MapRender.instance.bShowTreasure =
            //     GUILayout.Toggle(MapRender.instance.bShowTreasure, "显示宝藏点");
            MapRender.instance.bShowSeaQuadTreeBranch =
                GUILayout.Toggle(MapRender.instance.bShowSeaQuadTreeBranch, "显示四叉树分支");

            if (GUILayout.Button("刷新等级标签"))
            {
                MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateAllEvent);
            }
        }

        /// <summary>
        /// 工具简介，快捷键提示
        /// </summary>
        void DrawHelperGUI()
        {
            // EditorGUILayout.HelpBox($"{tool.GetDescription()}", MessageType.Info);
            switch (tool)
            {
                case MapToolOP.CreateMap:
                    // EditorGUILayout.HelpBox($"快捷键提示", MessageType.Info);
                    break;
                case MapToolOP.EditTerritory:
                    break;
            }
        }


        private void DrawBrushSetting()
        {
            GUILayout.Label("笔刷");
            EditorGUILayout.HelpBox($"笔刷工具：Control + 鼠标左键", MessageType.Info);
            _Tool_BrushRound = EditorGUILayout.IntSlider("笔刷大小", _Tool_BrushRound, 1, 50);
        }

        private void DrawHarborSetting()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var map = MapRender.instance.GetMap();
                if (map != null)
                {
                    GUILayout.Label("港口停泊点");
                    GUILayout.BeginHorizontal();

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("1-2级港口");
                        map.Level2BelowCircles =
                            EditorGUILayout.IntField("占领圈数", map.Level2BelowCircles);
                        map.VipCount4TwoCircle =
                            EditorGUILayout.IntField("vip停泊点数量", map.VipCount4TwoCircle);
                        map.CallRadius4TwoCircle =
                            EditorGUILayout.IntField("停靠半径", map.CallRadius4TwoCircle);
                    }

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("3-7级港口");
                        map.Level2BeyondCircles =
                            EditorGUILayout.IntField("占领圈数", map.Level2BeyondCircles);
                        map.VipCount4ThreeCircle =
                            EditorGUILayout.IntField("vip停泊点数量", map.VipCount4ThreeCircle);
                        map.CallRadius4ThreeCircle =
                            EditorGUILayout.IntField("停靠半径", map.CallRadius4ThreeCircle);
                    }

                    GUILayout.EndHorizontal();
                }
            }
        }

        private void DrawNewbieSetting()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var map = MapRender.instance.GetMap();
                if (map != null)
                {
                    map.ExportNewbieMapZoneMargin =
                        GUILayout.Toggle(map.ExportNewbieMapZoneMargin > 0, "新手地图") ? 10000 : 0;

                    if (map.ExportNewbieMapZoneMargin > 0)
                    {
                        map.Level2BelowCircles = 2;
                        map.Level2BeyondCircles = 3;
                    }
                    else
                    {
                        map.Level2BelowCircles = 3;
                        map.Level2BeyondCircles = 4;
                    }
                }
            }
        }

        private void DrawAreaScatterSetting(AreaScatterSetting areaScatterSetting)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label(areaScatterSetting.name);

                if (areaScatterSetting.isCenterZone)
                {
                    areaScatterSetting.expandCount =
                        EditorGUILayout.IntField("几圈", areaScatterSetting.expandCount);
                }
                else
                {
                    areaScatterSetting.width =
                        EditorGUILayout.IntField("宽", areaScatterSetting.width);
                    areaScatterSetting.height =
                        EditorGUILayout.IntField("高", areaScatterSetting.height);
                }

                areaScatterSetting.level = EditorGUILayout.IntField("等级", areaScatterSetting.level);
                areaScatterSetting.count =
                    EditorGUILayout.IntField("区域数量", areaScatterSetting.count);
            }
        }

        private void InitAreaScatterSetting()
        {
            var areaCenterCount = 0;
            var areaLevelCount = new int[7];

            var area1Count = 0;
            var area2Count = 0;
            var area3Count = 0;
            var map = MapRender.instance.map;
            if (map != null && map.zones.Count > 0)
            {

                foreach (var zone in map.zones)
                {
                    switch (zone.Value.level)
                    {
                        case 8:
                            areaCenterCount += 1;
                            break;
                        default:
                            if (zone.Value.level > 0 && zone.Value.level < 8)
                            {
                                areaLevelCount[zone.Value.level - 1] += 1;
                            }
                            break;
                    }
                }
            }
            else
            {
                areaCenterCount = 1;
            }

            if (areaCenter == null)
                areaCenter = new AreaScatterSetting
                {
                    name = "王城", isCenterZone = true, expandCount = 30, level = 8, postype = 0,
                    count = areaCenterCount
                };
            if (area1 == null)
                area1 = new AreaScatterSetting
                    { name = "内圈", width = 300, height = 300, level = 6, postype = 1, count = area1Count };
            if (area2 == null)
                area2 = new AreaScatterSetting
                    { name = "中圈", width = 700, height = 700, level = 4, postype = 2, count = area2Count };
            if (area3 == null)
                area3 = new AreaScatterSetting
                {
                    name = "外圈", width = 1201, height = 1201, level = 1, postype = 3, count = area3Count
                };
        }

        void DrawAttribute()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUILayout.LabelField("属性");

            var hex = MapRender.instance.OperationHex;
            if (hex != null)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("格子属性");
                    GUILayout.Label($"地格 [{hex.x},{hex.y}]");
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label($"下标 [{hex.index}]");
                    GUILayout.Label($"QR [{hex.q},{hex.r}]");
                    if (GUILayout.Button("复制"))
                    {
                        GUIUtility.systemCopyBuffer = hex.index.ToString();
                    }

                    EditorGUILayout.EndHorizontal();

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("邻接地格");
                        EditorGUILayout.BeginHorizontal();
                        List<string> btns = new List<string>();
                        List<Hexagon> neighbours = new List<Hexagon>();
                        for (int i = 0; i < hex.neigbours.Length; i++)
                        {
                            if (hex.neigbours[i] != null)
                            {
                                neighbours.Add(hex.neigbours[i]);
                                btns.Add($"{hex.neigbours[i].x}, {hex.neigbours[i].y}");
                            }
                        }

                        var idx = GUILayout.SelectionGrid(_selectHex, btns.ToArray(), 6);
                        if (idx != _selectHex)
                        {
                            _selectHex = idx;
                            MapRender.instance.FocusHexagon(neighbours[idx], -1f);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("地势");
                        hex.movetype = (MOVETYPE)GUILayout.SelectionGrid(
                            (int)hex.movetype, GlobalDef.S_MoveTypeDes, 5);
                    }

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("可设置的地势(只读)");
                        EditorGUILayout.BeginHorizontal();
                        for (int i = 0; i < (int)MOVETYPE.COUNT; i++)
                        {
                            GUILayout.Toggle(
                                ((hex.movetypeMark & hex.movetypeMarkRandomSea) & (1 << i)) > 0,
                                GlobalDef.S_MoveTypeDes[i]);
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("阻挡");
                        hex.blockFlag = (BlockFlag)GUILayout.SelectionGrid(
                            (int)hex.blockFlag, GlobalDef.S_BlockFlagDes, 5);
                    }

                    _map = MapRender.instance.GetMap();

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label("属性");
                        hex.attribute = (HexAttribute)GUILayout.SelectionGrid((int)hex.attribute,
                            GlobalDef.S_HexAttributeDes,
                            5);
                    }

                    if (hex.attribute == HexAttribute.SailPoint)
                    {
                        _map.AddSailPoint(hex.index);
                        var count = _map.connect[hex.index].Count + 1;
                        var removeIdxs = new HashSet<int>();
                        foreach (var idx in _map.connect[hex.index])
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("关联航道点");
                            GUILayout.Space(20);
                            var targetHex = _map.GetHexagon(idx);
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
                            _map.RemoveConnect(hex.index, idx);
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("X:");
                        newSailPointX = GUILayout.TextField(newSailPointX);
                        // GUILayout.Space(20);
                        GUILayout.Label("Y:");
                        newSailPointY = GUILayout.TextField(newSailPointY);
                        if (GUILayout.Button("新增关联航道点"))
                        {
                            _map.AddConnect(hex.index, newSailPointX.ToInt(),
                                newSailPointY.ToInt());
                            newSailPointX = string.Empty;
                            newSailPointY = string.Empty;
                        }

                        GUILayout.EndHorizontal();

                        var hexZone = hex.zone;
                        if (hexZone != null && hexZone._portSailIndex == hex.index)
                        {
                            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                            {
                                GUILayout.Label("海域主航道点");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("新海域主航道点下标:");
                                newHarborSailIndex = GUILayout.TextField(newHarborSailIndex);
                                if (GUILayout.Button("替换"))
                                {
                                    hexZone.ChangePortSailIndex(newHarborSailIndex.ToInt());
                                }

                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                    else
                    {
                        _map.RemoveSailPoint(hex.index);
                    }
                }

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("区域属性");
                    var zone = hex.zone;
                    if (zone != null && _map != null)
                    {
                        if (hex.index == zone._portIndex)
                        {
                            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                            {
                                GUILayout.Label("港口");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("新海域港口下标:");
                                newHarborIndex = GUILayout.TextField(newHarborIndex);
                                if (GUILayout.Button("替换"))
                                {
                                    zone.ChangePortIndex(newHarborIndex.ToInt());
                                }

                                GUILayout.EndHorizontal();
                            }
                        }

                        var color = _map.GetZoneColor(zone.color);
                        color.a = 1f;
                        GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label($"区域 [ {zone.index} ] ");
                            GUILayout.FlexibleSpace();
                            _changeZoneIndex =
                                EditorGUILayout.IntField(_changeZoneIndex, GUILayout.Width(50));
                            if (GUIHelper.Button("修改id", Color.green))
                            {
                                _map.ChangeZoneIndex(zone.index, _changeZoneIndex);
                            }

                            if (GUIHelper.Button("扩一圈", Color.red))
                            {
                                _map.ExpandZone(zone.index);
                            }
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            if (GUIHelper.Button("创建河流区域", Color.blue))
                            {
                                _map.CreateWaterZoneByHex(hex);
                            }

                            if (GUIHelper.Button("创建山脉区域", Color.yellow))
                            {
                                _map.CreateLandZoneByHex(hex);
                            }
                        }
                        GUILayout.EndHorizontal();
                        _color = EditorGUILayout.ColorField(color);
                        if (_color != color)
                        {
                            _map.SetZoneColor(zone.index, _color);
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
                                MapRender.instance.FocusHexagon(zone.neigbourZones[idx].hexagon,
                                    -1f);
                            }
                        }

                        GUILayout.Label("所属商圈");
                        GUILayout.Label($"商圈 [ {zone.businessZone} ] ");

                        GUILayout.Space(10);

                        GUILayout.Space(20);
                        GUILayout.Label($"城市位置 [{zone.hexagon.x},{zone.hexagon.y}]");
                        GUILayout.Label($"包含地格 {zone.hexagons.Count}");
                        //GUILayout.BeginHorizontal();
                        GUILayout.Label($"城市等级 {zone.level}");
                        zone.level = EditorGUILayout.IntSlider(zone.level, 0, Zone.S_MAX_ZoneLV);
                        // zone.level = (int) GUILayout.HorizontalSlider(zone.level, 0, 7);
                        //GUILayout.EndHorizontal();
                        GUILayout.Space(10);
                        zone.visible = GUILayout.Toggle(zone.visible == 1, "显示城点") ? 1 : 0;
                        zone.isGuanqia = GUILayout.Toggle(zone.isGuanqia == 1, "是否关卡") ? 1 : 0;
                        zone.isBorn = GUILayout.Toggle(zone.isBorn == 1, "是否出生城") ? 1 : 0;
                        zone.subType = GUILayout.Toggle(zone.subType == 1, "是否末日实验室") ? 1 : 0;
                        GUILayout.Space(20);


                        GUILayout.Space(20);

                        // GUILayout.Label("设置遗迹： " + zone.ruinId);
                        // zone.ruinId = (int) GUILayout.HorizontalSlider(zone.ruinId, 0, 3);
                        //GUILayout.EndHorizontal();
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            GUILayout.Label("区域类型");
                            // zone.landform = (ZoneLandform) EditorGUILayout.EnumPopup("地貌", zone.landform);
                            zone.landType = (ZoneLandType)GUILayout.SelectionGrid(
                                (int)zone.landType,
                                GlobalDef.S_ZoneLandTypeDes, 5);
                        }

                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            GUILayout.Label("地貌");
                            // zone.landform = (ZoneLandform) EditorGUILayout.EnumPopup("地貌", zone.landform);
                            zone.landform = (ZoneLandform)GUILayout.SelectionGrid(
                                (int)zone.landform,
                                GlobalDef.S_ZoneLandformDes, 5);
                        }

                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            GUILayout.Label("地域类型");
                            zone.posType = EditorGUILayout.IntField(zone.posType);
                        }

                        GUILayout.EndVertical();
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }


        internal static void DoRepaint()
        {
            if (_window != null)
                _window._wantsRepaint = true;
        }

        internal void SetTool(MapToolOP maptool, bool enableTool = true)
        {
            if (maptool == tool)
                return;

            if (maptool > MapToolOP.CreateMap)
            {
                if (!CheckValid())
                {
                    return;
                }
            }

            MapEditorEventCenter.SendEvent(MapEditorEvent.MapEditorToolChanged, maptool, tool);
            switch (maptool)
            {
                case MapToolOP.EditFogOfWar:
                    var map = MapRender.instance.GetMap();
                    map.fogOfWarDataMan.SetFogOfWarDataVisible(true);
                    break;
            }

            switch (tool)
            {
                case MapToolOP.EditFogOfWar:
                    var map = MapRender.instance.GetMap();
                    map.fogOfWarDataMan.SetFogOfWarDataVisible(false);
                    break;
            }

            tool = maptool;

            DoRepaint();
        }

        private static bool CheckValid()
        {
            if (!CheckEditorValid()) return false;

            var map = MapRender.instance.GetMap();
            if (map == null)
            {
                EditorUtility.DisplayDialog("Error", "没有地图 !!!", "确定");
                return false;
            }

            return true;
        }

        private static bool CheckEditorValid()
        {
            if (MapRender.instance == null)
            {
                EditorUtility.DisplayDialog("地图编辑", "请先启动 点击 Play !!!", "确定");
                return false;
            }

            return true;
        }

        public void EditSea_SetCurZone(Zone zone)
        {
            _curZone = zone;
        }

        public void EditSea_SetMergeZone(Zone zone)
        {
            if (_curZone != null && _curZone != zone)
            {
                _mergeZone = zone;
                EditSea_MergeZone();
            }
        }

        private void EditSea_MergeZone()
        {
            if (_curZone != null && _mergeZone != null && _curZone != _mergeZone)
            {
                if (!_curZone.AddZone(_mergeZone, out string error))
                {
                    // EditorUtility.DisplayDialog("提示", error, "OK");
                    Debug.LogError(error);
                }

                var map = MapRender.instance.GetMap();
                if (map != null)
                {
                    map.Del(_mergeZone);
                }

                _mergeZone = null;
            }
        }

        public void EditBusiness_SetCurZone(Zone zone)
        {
            _curBusinessZone = zone;
        }

        public void EditBusiness_SetMergeZone(Zone zone)
        {
            if (_curBusinessZone != null && _curBusinessZone != zone)
            {
                if (zone.neigbourZones.Contains(_curBusinessZone))
                {
                    zone.businessZone = _curBusinessZone.businessZone;
                    _curBusinessZone = zone;
                }
            }
        }

        private void DrawEditTreasure()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var map = MapRender.instance.GetMap();
                if (map != null)
                {
                    GUILayout.Label("宝藏点设置");
                    GUILayout.BeginVertical();

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        map.Treasure2PortDis =
                            EditorGUILayout.IntField("宝藏点距离港口限制(x)", map.Treasure2PortDis);
                        map.TreasureInterval =
                            EditorGUILayout.IntField("宝藏点之间的间隔区间(n)", map.TreasureInterval);
                        map.TreasureMaxPerPort =
                            EditorGUILayout.IntField("每个海域最大宝藏数量", map.TreasureMaxPerPort);
                    }

                    if (GUILayout.Button("生成宝藏点"))
                    {
                        map.GenerateTreasures();
                    }

                    if (GUILayout.Button("导出宝藏点配置"))
                    {
                        MapRender.instance.OnMenu_ExportTreasures();
                    }

                    GUILayout.EndVertical();
                }
            }
        }
    }
}

#endif