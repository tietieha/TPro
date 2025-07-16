#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using FileMode = System.IO.FileMode;
using Random = UnityEngine.Random;

namespace GEngine.MapEditor
{
    public enum MapToolOP
    {
        [Description("创建/打开")] CreateMap,
        [Description("编辑区域")] EditTerritory,
        [Description("编辑城点")] EditCity,
        [Description("编辑阻挡")] EditBlock,
        [Description("编辑交互物")] EditInteract,
        [Description("编辑撒点")] EditScatter,
        [Description("编辑相机")] EditCamera,
        [Description("编辑迷雾")] EditFogOfWar,
        [Description("导出设置")] ExportSetting,
        [Description("导出")] Export,
    }


    public enum OperationType
    {
        Unknown = -1,
        EditNull,
        EditColor, //调整地格颜色
        EditFill, //填充地格颜色
        EditZone, //合并区域
        EditCut, //切分区域
        EditCity, //调整中立城位置
        EditTower, //设置哨塔
        EditPattern, //使用模具
        EditLandform, //编辑地貌
        EditBlock, //编辑阻挡
        EditFishery, //编辑渔场
        EditSailLine, //编辑航线
        EditIsland, //编辑岛屿模板
        EditInteraction, //编辑交互物
    }

    public class SeekInfo
    {
        public int width = 2400;
        public int height = 1200;
        public int startOffsetX = 0; //首个种子相对于区域原点的偏移
        public int startOffsetY = 0;
        public int seekOffset = 10; //取样种子的间隔距离
        public int seekAdjustRate = 35; //每个种子获得随机调整的概率
        public int seekAdjustOffset = 3; //种子调整的偏移幅度
        public int seekDelRandomMin = 0; //删除种子的最小间隔数量
        public int seekDelRandomMax = 10; //删除种子的最大间隔数量
        public int posType = 1; //所处区域的等级
        public int level = 1; //生成的城市等级
    }

    [System.Serializable]
    public class ZoneImageInfo
    {
        public short zoneId;
        public short x;
        public short y;
        public short w;
        public short h;
        public short cx;
        public short cy;
        public List<ushort> neigbours = new List<ushort>();
        public List<ushort> edgeNeigbours = new List<ushort>();
        public Zone zone;

        public ZoneImageInfo(Zone zone, int x, int y, int w, int h)
        {
            this.zone = zone;
            this.zoneId = (short)zone.index;
            this.x = (short)x;
            this.y = (short)y;
            this.w = (short)w;
            this.h = (short)h;
            this.cx = (short)(zone.hexagon.x - x);
            this.cy = (short)(zone.hexagon.y - y);

            neigbours.Clear();
            foreach (var t in zone.neigbourZones)
            {
                neigbours.Add((ushort)(t.index));
            }

            edgeNeigbours.Clear();
            foreach (var t in zone.edgeNeigbours)
            {
                edgeNeigbours.Add((ushort)t);
            }
        }

        public void Save(BinaryWriter bw)
        {
            // 城点数据
            zone.ExportCommon(bw);

            // 城点区域描边图位置信息
            bw.Write(x);
            bw.Write(y);
            bw.Write(w);
            bw.Write(h);
            bw.Write(cx);
            bw.Write(cy);

            //bw.Write(neigbours.Count);
            //for(int i=0; i<neigbours.Count; i++)
            //{
            //    bw.Write(neigbours[i]);
            //}

            // 边缘Mesh数据。
            bw.Write(zone.vertexs.Count);
            foreach (var p in zone.vertexs)
            {
                bw.Write((int)(p.x * 10f));
                bw.Write((int)(p.z * 10f));
            }

            bw.Write(zone.triangles.Count);
            foreach (var idx in zone.triangles)
            {
                bw.Write((ushort)idx);
            }

            bw.Write(zone.edgeNeigbours.Count);
            foreach (var idx in zone.edgeNeigbours)
            {
                bw.Write((ushort)idx);
            }

            // vertexSorts
            // var str = "save : ";
            // for(int i=0; i<zone.vertexSorts.Count; i++)
            // {
            //     str += zone.vertexSorts[i];
            // }
            // Debug.Log(str);

            var need = (zone.vertexSorts.Count / 8) + 1;
            bw.Write(need);

            byte[] bytes = new byte[need];

            int tdx = 0;
            for (int i = 0; i < need; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tdx = i * 8 + j;
                    if (tdx >= zone.vertexSorts.Count) break;
                    if (zone.vertexSorts[tdx] == 2)
                    {
                        bytes[i] |= (byte)(1 << j);
                    }
                }

                if (tdx >= zone.vertexSorts.Count) break;
            }

            bw.Write(bytes, 0, need);
        }
    }

    [System.Serializable]
    public class SaveImageConfig
    {
        public int width;
        public int height;

        public int scale;

        public List<ZoneImageInfo> images = new List<ZoneImageInfo>();

        public void Save(HexMap map, BinaryWriter bw)
        {
            bw.Write(width);
            bw.Write(height);
            bw.Write(scale);

            bw.Write(images.Count);
            for (int i = 0; i < images.Count; i++)
            {
                images[i].Save(bw);
            }

            map.ExportHexagongs(bw);

            int ver = 1;
            bw.Write(ver); // int 版本
            map.ExportToBlock(bw);

            bw.Write((int)map.mapType);
            if (map.mapType == MapType.ESmallWorld)
            {
                map.WriteSmallMapData(bw);
            }

            //map.WriteFisheriesData(bw);
        }
    }

//
    public class CityRenderInfo
    {
        public int level;
        public Color color;
        public int round; // 0: 1格  1: 7格   2：19格 ...
    }

    public class CampaignMapInfo
    {
        public int Id;
        public string Name;
        public string MapUWFilePath;
        public string PrefabFilePath;
        public string RenderDataFilPath;
    }


    public partial class MapRender : MonoBehaviour
    {
        public static MapRender instance;

        private const string S_WorkSpaceFlag = "UnityProject/map";

        public string WorkSpace
        {
            get => EditorPrefs.GetString("MapEditorSpace", String.Empty);
            set
            {
                if (!value.Contains(S_WorkSpaceFlag))
                {
                    EditorUtility.DisplayDialog("警告", $"策划SVN工程中的地图路径：{S_WorkSpaceFlag}", "OK");
                    return;
                }

                EditorPrefs.SetString("MapEditorSpace", value);
            }
        }

        public string ConfigPath
        {
            get => EditorPrefs.GetString("MapEditorConfigPath", String.Empty);
            set { EditorPrefs.SetString("MapEditorConfigPath", value); }
        }

        public WorldTestRoot worldTestRoot;
        public Camera mainCamera;
        public Rect cameraDragArea;
        public MapEditorUIManager uiManager;
        public Material waterMaterial;
        public GameObject parentForPrefabs;
        public GameObject prefabPainterCollider;

        private Texture2D blockData;

        public int CurRuinId;
        public int CurRuinDepth;

        // 操作数据
        public OperationType Operation = OperationType.Unknown; // 0 = 编辑六角格颜色  1 = 合并区域  2 = 调整城点位置
        public Hexagon OperationLastHex;
        public Hexagon OperationHex;
        public Zone OperationTargetZone;
        public int EditColorRange = 1;

        public PatternZone Pattern;

        public PatternManager PatternManager = new PatternManager();

        public float fHexagonAlpha = 1f;
        [SerializeField] private bool bShowGrid = true;
        [SerializeField] private bool bShowGridGround = false;

        public bool ShowGrid
        {
            get { return bShowGrid; }
            set
            {
                if (bShowGrid != value)
                {
                    bShowGrid = value;
                    lineMaterial.SetInt("_ZTest",
                        (int)(bShowGrid ? CompareFunction.Always : CompareFunction.LessEqual));
                }
            }
        }

        public bool ShowGridGround
        {
            get { return bShowGridGround; }
            set { bShowGridGround = value; }
        }

        public bool bShowBlock = false;
        public bool bShowCity = true;
        public bool bShowEdge = false;
        public bool bShowBusinessEdge = false;
        public bool bShowWireframe = true;
        public bool bShowSeaQuadTreeBranch = false;
        public bool bShowSeaQuadTreeLeaf = false;
        public bool bShowTreasure = false;


        public ZoneLandform landform = ZoneLandform.LANDFORM1;
        public BlockFlag blockFlag = BlockFlag.None;

        public static Color[] S_MoveTypeColor = new[]
        {
            Color.red, // disable
            Color.magenta, // normal
            Color.green, // speedup
            Color.cyan // speeddown
        };

        public Color TowerColor = new Color(0.9f, 1.0f, 0f, 1f);
        public Color FisheColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public Color SeatColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        private Material lineMaterial;
        public Material edgeMaterial;
        public Material hexMaterial;
        public Material businessEdgeMaterial;

        public static CityRenderInfo[] cityRenders;

        private float zoom = 200f;
        private float maxZoom = 1990f;
        private float minZoom = 2f;

        // bool bShowFileWnd = false;
        // bool bShowCreateWnd = false;
        // bool bShowEditWnd = false;
        // bool bShowRuinWnd = false;

        private Hexagon pointHexagon = null;
        private Hexagon pointHexagonLast = null;
        public HexMap map { get; private set; }

        //航道点
        private Hexagon beginHexagon = null;
        private Hexagon endHexagon = null;

        //岛屿模板
        public IslandTemplate islandTemplate = null;
        public List<Axial> islandHexCoords = null;
        public int islandOffset = 10; //岛屿中心偏移 确保q r都大于0

        public PrefabPainter prefabPainter;


        public List<InteractionPoint> interactionPoints { get; private set; }

        public Dictionary<string, string> PrefabsSrcIndex { get; set; }


        [Obsolete("废弃了, 使用InteractionPoint.BlockPoints")]
        public Dictionary<int, List<Vector2Int>> interactBlockPoints;


        private GameObject interactionLayer;


        private Vector3Int previewPoint = Vector3Int.zero; // 用于预览点
        public Mesh PveFogOfWarMesh;
        public float handleSize = 0.5f;
        public Color handleColor = Color.cyan;

        private void Awake()
        {
            if(EditorPrefs.HasKey("MapEditorType")) {
                GlobalDef.S_MapEditorType = (MapEditorType)EditorPrefs.GetInt("MapEditorType");
            }
            instance = this;
            prefabPainter = ScriptableObject.CreateInstance<PrefabPainter>();

            lineMaterial = new Material(Shader.Find("UI/Custom"));
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;

            // 城点显示配置
            string[] colors = new string[]
            {
                "#FFFFFF", "#FFFF00", "#00FF00", "#00FFFF", "#0000FF", "#FF00FF", "#FF0000",
                "#FF7F00", "#00BFFF", "#FFA500"
            };

            cityRenders = new CityRenderInfo[Zone.S_MAX_ZoneLV + 1];
            for (int i = 0; i <= Zone.S_MAX_ZoneLV; i++)
            {
                cityRenders[i] = new CityRenderInfo();
                cityRenders[i].level = i;
                ColorUtility.TryParseHtmlString(colors[i], out Color nowColor);
                cityRenders[i].color = nowColor;
                cityRenders[i].round = i / 3 + 1;
            }

            if (CheckConfigPathValid())
            {
                var mapInfoPath = Path.Combine(ConfigPath, "Campaign_Map_Info.xlsx");
                HexMap.ImportCampaignMapInfoExcel(mapInfoPath);
                var campaignFogInfoPath = Path.Combine(ConfigPath, "Campaign_Fog.xlsx");
                HexMap.ImportCampaignFogExcel(campaignFogInfoPath);
            }

            MapTool.ShowWindow();
            // MapToolBar.ShowToolBar();
            // AttributeDlg.ShowAttributeDlg();
            // SceneTool.Show();

            qt_select_ret_helper = new List<Hexagon>();
            // 后续优化
            // interactionPoints = new List<InteractionPoint>();

            // interactBlockPoints = new Dictionary<int, List<Vector2Int>>();
            PrefabsSrcIndex = new Dictionary<string, string>();
            if (CheckConfigPathValid())
            {
                var srcIndexExcelPath = Path.Combine(ConfigPath, "Src_Index.xlsx");
                HexMap.ImportSrcIndexExcel(srcIndexExcelPath);
            }
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public void OnDestroy()
        {
            MapTool.Hide();
            // SceneTool.Hide();
            // MapToolBar.Hide();
            // AttributeDlg.Hide();
            // RuinTool.Hide();
            // CreateMapTool.Hide();
            // PatternTool.Hide();
            IslandWindow.Hide();
        }

        Vector2 nativeSize = new Vector2(640, 480);

        void OnGUI()
        {
            if (saveZoneModel)
                return;

            if (map != null)
            {
                GUIStyle style = new GUIStyle();
                // style.fontSize = (int)(10.0f * ((float)Screen.width / (float)nativeSize.x));
                style.fontSize = 18;
                int cw = 150;
                GUI.Label(new Rect(Screen.width - cw * 4, 10, cw, 20),
                    "尺寸 : " + map.width + "x" + map.height, style);
                GUI.Label(new Rect(Screen.width - cw * 3, 10, cw, 20), "创建区域 : " + map.zones.Count,
                    style);
                GUI.Label(new Rect(Screen.width - cw * 2, 10, cw, 20),
                    "最少格数 : " + map.minZoneHexCount, style);
                GUI.Label(new Rect(Screen.width - cw * 1, 10, cw, 20),
                    "最多格数 : " + map.maxZoneHexCount, style);

                if (pointHexagon != null)
                {
                    GUI.Label(new Rect(Screen.width - cw * 4, 40, cw, 20),
                        "选中 : " + pointHexagon.x + "," + pointHexagon.y, style);
                    Vector3 pos = pointHexagon.Pos;

                    GUI.Label(new Rect(Screen.width - cw * 4, 60, cw, 20),
                        "坐标 : " + pos.x + "," + pos.z, style);
                    GUI.Label(new Rect(Screen.width - cw * 4, 80, cw, 20),
                        $"qr坐标:{pointHexagon.q}, {pointHexagon.r}", style);
                }

                int iy = 0;
                for (int i = 0; i <= Zone.S_MAX_ZoneLV; i++)
                {
                    var info = map.zoneLvInfos[i];
                    iy++;
                    GUI.Label(new Rect(Screen.width - cw * 2, 10 + iy * 30, cw * 2, 20),
                        $"{i}级城点: {info.zones.Count} \t{info.minHex} \t{info.maxHex}", style);

                    var guanqia = map.guanqiaLvInfos[i];
                    if (guanqia.zones.Count > 0)
                    {
                        iy++;
                        GUI.Label(new Rect(Screen.width - cw * 2, 10 + iy * 30, cw * 2, 20),
                            $"<color=#ff0000>{i}级关卡: {guanqia.zones.Count} \t{guanqia.minHex} \t{guanqia.maxHex}</color>",
                            style);
                    }
                }
            }

            if (GUILayout.Button("隐藏网格", GUILayout.Height(30)))
            {
                // Implement the logic to switch the game view here
                // SwitchGameView();
                // mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
                MapRender.instance.fHexagonAlpha = 0;
                MapRender.instance.ShowGrid = false;
            }

            if (GUILayout.Button("显示网格", GUILayout.Height(30)))
            {
                // Implement the logic to switch the game view here
                // SwitchGameView();
                // mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
                MapRender.instance.fHexagonAlpha = 1;
                MapRender.instance.ShowGrid = true;
            }

            if (GUILayout.Button("显示地面网格", GUILayout.Height(30)))
            {
                // Implement the logic to switch the game view here
                // SwitchGameView();
                // mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
                MapRender.instance.fHexagonAlpha = 1;
                MapRender.instance.ShowGrid = false;
                MapRender.instance.ShowGridGround = true;
            }

            if (GUILayout.Button("隐藏地面网格", GUILayout.Height(30)))
            {
                // Implement the logic to switch the game view here
                // SwitchGameView();
                // mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
                MapRender.instance.fHexagonAlpha = 0;
                MapRender.instance.ShowGrid = false;
                MapRender.instance.ShowGridGround = false;
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            switch (MapTool.Instance.tool)
            {
                case MapToolOP.EditCamera:
                    OnEditCameraSceneGUI(sceneView);
                    break;
                case MapToolOP.EditFogOfWar:
                    OnEditFogOfWarSceneGUI(sceneView);
                    break;
            }
        }

        #region WorkSpace

        string GetMapFolder()
        {
            if (CheckMapValid())
            {
                var folder = Path.Combine(WorkSpace, map.Name);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                return folder;
            }

            return null;
        }

        string GetMapFilePath()
        {
            if (CheckMapValid())
            {
                var folder = GetMapFolder();
                var filename = map.FileName;
                if (string.IsNullOrEmpty(filename))
                {
                    filename = Path.Combine(folder, $"{map.Name}{GlobalDef.S_UWMapExt}");
                }

                return filename;
            }

            return null;
        }

        string GetExportPath()
        {
            var exportFolder = GetMapFolder();
            var filename = Path.Combine(exportFolder, map.Name);
            return filename;
        }

        public void OpenExportFolder()
        {
            EditorUtility.RevealInFinder(GetExportPath());
        }

        private string _defaultDir = "";

        void SaveDefaultDirectory(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            _defaultDir = fi.Directory.FullName;
            PlayerPrefs.SetString("defaultDir", _defaultDir);
            PlayerPrefs.Save();
        }

        void GetOpenFileDialog(string title, System.Action<string> callback, string[] filters)
        {
            if (string.IsNullOrEmpty(_defaultDir))
            {
                _defaultDir = PlayerPrefs.GetString("defaultDir", Application.dataPath);
            }

            var fileName = EditorUtility.OpenFilePanelWithFilters(title, _defaultDir, filters);
            if (!string.IsNullOrEmpty(fileName))
            {
                SaveDefaultDirectory(fileName);
                callback?.Invoke(fileName);
            }
        }

        void GetSaveFileDialog(string title, System.Action<string> callback,
            string defaultName = "",
            string extension = "")
        {
            if (string.IsNullOrEmpty(_defaultDir))
                _defaultDir = Application.dataPath;

            var fileName = EditorUtility.SaveFilePanel(title, _defaultDir, defaultName, extension);
            if (!string.IsNullOrEmpty(fileName))
            {
                SaveDefaultDirectory(fileName);
                callback?.Invoke(fileName);
            }
        }

        #endregion

        public void LoadMap(string fileName)
        {
            LoadMap(fileName, -1);
        }

        public void LoadMap(string fileName, int mapId = -1)
        {
            prefabPainter?.Reset();
            uiManager?.Reset();
            if (map == null)
            {
                map = new HexMap();
            }
            else
            {
                map.Clear();
            }

            map.Name = Path.GetFileNameWithoutExtension(fileName);
            map.FileName = fileName;

            var br = new BinaryReader(new FileStream(fileName, FileMode.Open));
            var ret = map.Load(br, (s, f) => { EditorUtility.DisplayProgressBar("加载地图", s, f); }, mapId);
            br.Close();

            EditorUtility.ClearProgressBar();

            if (!ret)
            {
                EditorUtility.DisplayDialog("加载地图", $"加载地图[{fileName}]失败！！！", "确定");
            }
        }

        public bool SaveMap(bool notify = true)
        {
            if (!CheckMapValid())
            {
                return false;
            }

            if (!map.CheckMapValid((msg, value) => { EditorUtility.DisplayProgressBar("保存地图", msg, value); }))
            {
                return false;
            }

            var filename = GetMapFilePath();

            var bw = new BinaryWriter(new FileStream(filename, FileMode.Create));
            map.Save(bw, CurrentMapId);
            bw.Close();
            var fogFilePath = Path.Combine(instance.ConfigPath, "Campaign_Fog.xlsx");
            HexMap.ExportCampaignFogExcel(fogFilePath);
            EditorUtility.ClearProgressBar();

            if (notify)
                EditorUtility.DisplayDialog("保存地图", $"地图保存完成!!! \n{filename}", "确定");
            return true;
        }

        void SaveAsMap(string fileName)
        {
            if (!CheckMapValid())
            {
                return;
            }

            if (!map.CheckMapValid((msg, value) => { EditorUtility.DisplayProgressBar("保存地图", msg, value); }))
            {
                return;
            }

            var bw = new BinaryWriter(new FileStream(fileName, FileMode.Create));
            map.Save(bw);
            bw.Close();
            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayDialog("保存地图", $"地图保存完成!!! \n{fileName}", "确定");
        }

        string GetFileName(string fileName, string sExt)
        {
            var sPath = Path.GetDirectoryName(fileName);
            var sfile = Path.GetFileNameWithoutExtension(fileName);
            return sPath + "\\" + sfile + sExt;
        }


        void LoadFromJson(string fileName)
        {
            if (map == null)
            {
                map = new HexMap();
            }

            if (map.LoadFromJson(fileName))
            {
                EditorUtility.DisplayDialog("加载Json", $"通过Json加载地图完成! \n{fileName}", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("加载Json", $"加载Json失败!!!", "确定");
            }
        }


        void SaveClientMapData(string filename)
        {
            if (!map.CheckMapValid())
                return;

            // SaveImageConfig cfg = new SaveImageConfig();
            // cfg.width = map.width;
            // cfg.height = map.height;
            // cfg.scale = zoneScale;
            // cfg.images.Clear();
            //
            // foreach (var zone in map.zones.Values)
            // {
            // 	zone.GetImageInfo(1, out int w, out int h, out int x, out int y);
            //
            // 	var image = new ZoneImageInfo(zone, x, y, w, h);
            // 	cfg.images.Add(image);
            // }

            var sfile = GetFileName(filename, "_client.bytes");
            var bw = new BinaryWriter(new FileStream(sfile, FileMode.Create));
            map.ExportToClient(bw);
            // cfg.Save(map, bw);
            bw.Close();
        }

        void SavePictures(string filename)
        {
            if (!map.CheckMapValid())
                return;

            SaveImageConfig cfg = new SaveImageConfig();
            cfg.width = map.width;
            cfg.height = map.height;
            cfg.scale = zoneScale;
            cfg.images.Clear();

            foreach (var zone in map.zones.Values)
            {
                zone.GetImageInfo(1, out int w, out int h, out int x, out int y);

                var image = new ZoneImageInfo(zone, x, y, w, h);
                cfg.images.Add(image);
            }

            var sfile = GetFileName(filename, ".bytes");
            var bw = new BinaryWriter(new FileStream(sfile, FileMode.Create));
            cfg.Save(map, bw);
            bw.Close();
            StartCoroutine(SaveZoneLineImage(filename));
        }


        void SavePictures2(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveToPicture(filename);
            EditorUtility.DisplayDialog("导出像素全图", $"像素图导出完成！\n{filename}", "确定");
        }

        void SaveLandformPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveLandformToPicture(filename);
            EditorUtility.DisplayDialog("导出地貌散图", $"地貌图导出完成！\n{filename}", "确定");
        }

        void SaveFullLandformPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveFullLandformToPicture(filename);
            EditorUtility.DisplayDialog("导出地貌全图", $"地貌图导出完成！\n{filename}", "确定");
        }

        void SaveBlockPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveBlockPicture(filename);
            EditorUtility.DisplayDialog("导出阻挡图", $"阻挡图导出完成！\n{filename}", "确定");
        }

        void SaveZoneSubtypePicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveZoneSubtypePicture(filename);
            EditorUtility.DisplayDialog("导出末日实验室图", $"末日实验室图导出完成！\n{filename}", "确定");
        }

        void SaveLandAreaToPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveLandAreaToPicture(filename);
            EditorUtility.DisplayDialog("导出陆地区域图", $"陆地区域图图导出完成！\n{filename}", "确定");
        }

        void SaveIsLandAreaToPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveIsLandAreaToPicture(filename);
            EditorUtility.DisplayDialog("导出海岛区域图", $"海岛区域图图导出完成！\n{filename}", "确定");
        }


        void SaveSeaControlToPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveSeaControlToPicture(filename);
            EditorUtility.DisplayDialog("导出海水深浅控制图", $"海水深浅控制图导出完成！\n{filename}", "确定");
        }

        void SaveZoneLineToPicture(string filename)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveZoneLineToPicture(filename);
            EditorUtility.DisplayDialog("导出区域线图", $"区域线图导出完成！\n{filename}", "确定");
        }

        public void SaveSeaLineToPicture(string filename,
            bool isOnlyLine = false)
        {
            if (!map.CheckMapValid())
                return;

            map.SaveZoneLineToPicture(filename, true, isOnlyLine);
            EditorUtility.DisplayDialog("导出海洋线图", $"海洋线图导出完成！\n{filename}", "确定");
        }

        public void FocusZone(int zoneidx, float zm = 50f)
        {
            if (!CheckMapValid())
                return;
            var zone = map.zones[zoneidx];
            if (zone == null)
                return;

            var hex = zone.hexagon;
            FocusHexagon(hex.x, hex.y, zm);
        }

        public void FocusHexagon(Hexagon hex, float zm = 50f)
        {
            FocusHexagon(hex.x, hex.y, zm);
        }

        public void FocusHexagon(int x, int y, float zm = 50f)
        {
            var p = GetHexagonPos(x, y);
            zoom = zm > 0f ? zm : mainCamera.transform.position.y;
            p.y = zoom;
            mainCamera.transform.position = p;
        }

        public Vector3 GetHexagonPos(int x, int y)
        {
            if (map == null)
                return Vector3.zero;

            // return Hex.OffsetToWorld(new Vector2Int(x, y));
            if (MapRender.instance.ShowGridGround)
            {
                var pos = Hex.OffsetToWorld(new Vector2Int(x, y));
                Ray ray = new Ray(pos + new Vector3(0, 100, 0), Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("ObjRaycast")))
                {
                    pos.y = hit.point.y + 0.1f;
                }

                return pos;
            }
            else
            {
                return Hex.OffsetToWorld(new Vector2Int(x, y));
            }
        }

        public Vector3 GetHexagonPos(Hexagon hex)
        {
            if (map == null)
                return Vector3.zero;

            var x = hex.index % map.width;
            var y = (hex.index - x) / map.width;
            return GetHexagonPos(x, y);
        }

        Vector2Int PickHex(Vector3 screenPos)
        {
            var ray = mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                return Hex.WorldToOffset(hitInfo.point);
            }

            return Vector2Int.zero;
        }

        Vector3 lastPick;
        Vector3 downPos;

        bool bRectSelect;
        Vector3 rectStartPos;

        bool bBrushSelect;
        List<Zone> _changedZones = new List<Zone>();

        Hexagon GetHexagonByScreenPos(Vector3 screenPos)
        {
            if (map == null)
                return null;

            var ray = mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var pos = Hex.WorldToOffset(hitInfo.point);
                int x = pos.x; // + map.width / 2;
                int y = pos.y; // + map.width / 2;
                //Debug.Log($"Click world pos : {Input.mousePosition } => {hitInfo.point} => {x}x{y}");
                return map.GetHexagon(x, y);
            }

            return null;
        }

        public void SetOperationHex(Hexagon hex)
        {
            OperationLastHex = OperationHex;
            OperationHex = hex;
            // AttributeDlg.UpdateUI();
            // MapToolBar.UpdateUI();
            MapTool.DoRepaint();
        }

        private void SetOperationTargetZone(Zone zone)
        {
            OperationTargetZone = zone;
            // AttributeDlg.UpdateUI();
            // MapToolBar.UpdateUI();
            MapTool.DoRepaint();
        }

        private void Update()
        {
            QT_Select();
            pointHexagonLast = pointHexagon;
            pointHexagon = GetHexagonByScreenPos(Input.mousePosition);
            //如果鼠标选取的格子，超出了地图范围，则等于上次没超出范围的格子
            if (pointHexagon == null)
            {
                pointHexagon = pointHexagonLast;
            }

            // if (GUIUtility.hotControl != 0)
            // {
            //     return;
            // }

            switch (MapTool.Instance.tool)
            {
                case MapToolOP.EditTerritory:
                    OnEditTerritory(pointHexagon);
                    break;
                case MapToolOP.EditBlock:
                    OnEditBlock(pointHexagon);
                    break;
                case MapToolOP.EditCity:
                    OnEditCity(pointHexagon);
                    break;
                case MapToolOP.EditInteract:
                    OnEditInteraction(pointHexagon);
                    break;
                case MapToolOP.EditFogOfWar:
                    OnEditFogOfWar(pointHexagon);
                    break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                downPos = Input.mousePosition;
                lastPick = downPos;
                SetOperationHex(pointHexagon);

                if (Operation == OperationType.EditColor || Operation == OperationType.EditBlock)
                {
                    // 框选
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        bRectSelect = true;
                        rectStartPos = Input.mousePosition;
                    }
                }
                else if (Operation == OperationType.EditLandform)
                {
                    OnSelectLandform(pointHexagon);
                }
                else if (Operation == OperationType.EditSailLine)
                {
                    if (pointHexagon?.attribute == HexAttribute.SailPoint)
                        beginHexagon = pointHexagon;
                }

                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                {
                    if (pointHexagon != null)
                    {
                        SetOperationTargetZone(pointHexagon.zone);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // 框选
                bRectSelect = false;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    GetClipRect(rectStartPos, Input.mousePosition, out int nx1, out int nz1,
                        out int nx2, out int nz2);

                    switch (Operation)
                    {
                        case OperationType.EditColor:
                            OnEditColorByRect(nx1, nz1, nx2, nz2);
                            break;
                        case OperationType.EditBlock:
                            OnEditBlockByRect(nx1, nz1, nx2, nz2);
                            break;
                    }
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    switch (Operation)
                    {
                        case OperationType.EditSailLine:
                            OnEditSailLine(pointHexagon);
                            break;
                        case OperationType.EditIsland:
                            OnEditIsland(pointHexagon);
                            break;
                    }
                }
                else
                {
                    var offset = Input.mousePosition - downPos;
                    if (Mathf.Abs(offset.x) < 5 && Mathf.Abs(offset.y) < 5 && map != null)
                    {
                        var hex = pointHexagon;
                        if (hex != null)
                        {
                            /*Debug.LogFormat("pick : index = {0},  zone = {1}", hex.index,
                                hex.zone != null ? hex.zone.index : -1);*/
                            switch (Operation)
                            {
                                case OperationType.EditColor:
                                    OnEditColor(hex);
                                    break;
                                case OperationType.EditFill:
                                    OnEditFill(hex);
                                    break;
                                case OperationType.EditZone:
                                    OnEditZone(hex);
                                    break;
                                case OperationType.EditCut:
                                    OnEditCut(hex);
                                    break;
                                case OperationType.EditCity:
                                    OnEditCity(hex);
                                    break;
                                case OperationType.EditTower:
                                    OnEditTower(hex);
                                    break;
                                case OperationType.EditFishery:
                                    OnEditFishery(hex);
                                    break;
                                case OperationType.EditPattern:
                                    OnEditPattern(hex);
                                    break;
                                case OperationType.EditLandform:
                                    // TODO
                                    break;
                                case OperationType.EditBlock:
                                    OnEditBlock(hex);
                                    break;
                                case OperationType.EditInteraction:
                                    OnEditInteraction(hex);
                                    break;
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    switch (Operation)
                    {
                        case OperationType.EditIsland:
                            OnEditIslandPlace(pointHexagon);
                            break;
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (!bRectSelect &&
                    !Input.GetKey(KeyCode.LeftControl) &&
                    !Input.GetKey(KeyCode.RightControl) &&
                    !Input.GetKey(KeyCode.LeftAlt) &&
                    !Input.GetKey(KeyCode.RightAlt))
                {
                    var curPick = Input.mousePosition;
                    var detal = curPick - lastPick;
                    var sensit = zoom / 500f;
                    Vector3 p0 = mainCamera.transform.position;
                    Vector3 p01 = p0 - mainCamera.transform.right * detal.x * sensit *
                        Time.timeScale;
                    Vector3 p03 = p01 - mainCamera.transform.up * detal.y * sensit * Time.timeScale;

                    p03.x = Mathf.Clamp(p03.x, cameraDragArea.xMin, cameraDragArea.xMax);
                    p03.z = Mathf.Clamp(p03.z, cameraDragArea.yMin, cameraDragArea.yMax);

                    mainCamera.transform.position = p03;
                    lastPick = curPick;
                }

                if (Operation == OperationType.EditColor &&
                    (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                {
                    if (pointHexagonLast != pointHexagon)
                    {
                        OnEditColor(pointHexagon);
                    }
                }

                if (Operation == OperationType.EditSailLine && Input.GetKey(KeyCode.LeftControl))
                {
                    if (beginHexagon != null && pointHexagon != beginHexagon)
                    {
                        endHexagon = pointHexagon;
                    }
                }
            }

            if (Input.GetMouseButtonDown(2))
            {
                downPos = Input.mousePosition;
                lastPick = downPos;
            }

            if (Input.GetMouseButton(2))
            {
                // if (!bRectSelect &&
                //     !Input.GetKey(KeyCode.LeftControl) &&
                //     !Input.GetKey(KeyCode.RightControl) &&
                //     !Input.GetKey(KeyCode.LeftAlt) &&
                //     !Input.GetKey(KeyCode.RightAlt))
                {
                    var curPick = Input.mousePosition;
                    var detal = curPick - lastPick;
                    var sensit = zoom / 500f;
                    Vector3 p0 = mainCamera.transform.position;
                    Vector3 p01 = p0 - mainCamera.transform.right * detal.x * sensit *
                        Time.timeScale;
                    Vector3 p03 = p01 - mainCamera.transform.up * detal.y * sensit * Time.timeScale;
                    mainCamera.transform.position = p03;
                    lastPick = curPick;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0) //鼠标滚轮缩放功能
            {
                if (zoom >= minZoom && zoom <= maxZoom)
                {
                    zoom -= Input.GetAxis("Mouse ScrollWheel") * zoom;
                }

                zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
                var p = mainCamera.transform.position;
                p.y = zoom;
                mainCamera.transform.position = p;
                // Debug.Log("Scale = " + zoom);
            }
        }

        void OnSelectLandform(Hexagon hex)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                return;
            hex.zone.landform = MapRender.instance.landform;
            AttributeDlg.UpdateUI();
        }

        void OnEditTower(Hexagon hex)
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                hex.zone.ToggleTower(hex);
                AttributeDlg.UpdateUI();
            }
        }

        void OnEditFishery(Hexagon hex)
        {
            //设置选中的渔场索引
            if (hex == null)
            {
                this.map.SetCurFisheryIndex(-1);
            }


            //调整鱼群
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                this.map.ToggleFish(hex);
                AttributeDlg.UpdateUI();
                AttributeDlg.UpdateFishery();
            }

            //调整垂钓点
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                this.map.ToggleSeat(hex);
                AttributeDlg.UpdateUI();
                AttributeDlg.UpdateFishery();
            }
        }

        void OnEditFill(Hexagon hex)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                return;

            if (OperationTargetZone != null)
            {
                _tempHexes.Clear();
                hex.GetNearSameHexagons(ref _tempHexes);
                foreach (var t in _tempHexes)
                {
                    if (t.zone != OperationTargetZone)
                    {
                        if (t.zone != null)
                            t.zone.RemoveHexagon(t);

                        OperationTargetZone.AddHexagon(t, true);
                    }
                }
            }
        }

        void OnEditPattern(Hexagon hex)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                return;

            if (pointHexagon != null && Pattern != null)
            {
                Pattern.ApplyPattern(pointHexagon);
            }
        }

        void OnEditColor(Hexagon hex)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                return;

            if (OperationTargetZone != null)
            {
                if (hex == null)
                {
                    Debug.LogError("hex is null");
                }
                else
                {
                    hex.GetRoundHexagons(ref _tempHexes, EditColorRange - 1);
                }

                foreach (var t in _tempHexes)
                {
                    if (t.zone != OperationTargetZone)
                    {
                        if (t.zone != null)
                            t.zone.RemoveHexagon(t);

                        OperationTargetZone.AddHexagon(t, true);
                    }
                }
            }
        }

        void OnEditBlock(Hexagon hex)
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (hex == null)
                {
                    Debug.LogError("hex is null");
                }
                else
                {
                    hex.GetRoundHexagons(ref _tempHexes, MapTool.Instance._Tool_BrushRound - 1);
                }

                foreach (var t in _tempHexes)
                {
                    t.blockFlag = MapTool.Instance._Edit_BlockFlag;
                }
            }
        }

        Zone tmpZone;
        Hexagon tmpHex;
        Dictionary<int, Zone> tmpChangeZones = new Dictionary<int, Zone>();

        void OnEditColorByRect(int nx1, int nz1, int nx2, int nz2)
        {
            if (OperationTargetZone == null)
            {
                Debug.LogError("OperationTargetZone is null!");
                return;
            }

            for (int y = nz1; y <= nz2; y++)
            {
                for (int x = nx1; x <= nx2; x++)
                {
                    tmpHex = map.GetHexagon(x, y);
                    if (tmpHex != null)
                    {
                        //zone为空，错误情况，为hex重新赋值zone
                        if (tmpHex.zone == null)
                        {
                            tmpHex.zone = OperationTargetZone;
                        }
                        else
                        {
                            if (tmpHex.zone.index != OperationTargetZone.index)
                            {
                                tmpZone = tmpHex.zone;
                                tmpZone.RemoveHexagon(tmpHex);
                                tmpChangeZones[tmpZone.index] = tmpZone;
                            }
                        }

                        OperationTargetZone.AddHexagon(tmpHex, true);
                    }
                    else
                    {
                        Debug.LogError("TMPHex is null!");
                    }
                }
            }

            foreach (var t in tmpChangeZones.Values)
            {
                t.CalcEdges();
            }
        }

        void OnEditZone(Hexagon hex)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (hex != null && hex.zone != null && OperationTargetZone != null)
                {
                    OperationTargetZone.AddZone(hex.zone, out string error);
                }
            }
        }

        void OnEditCut(Hexagon hex)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                map.AddZone(hex);
            }
        }

        void OnEditCity(Hexagon hex)
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (hex.zone != null && hex.zone.hexagon != hex)
                {
                    hex.zone.ChangeCityHexagon(hex);
                }
            }
        }

        private void OnEditBlockByRect(int nx1, int nz1, int nx2, int nz2)
        {
            Hexagon hex;
            for (int y = nz1; y <= nz2; y++)
            {
                for (int x = nx1; x <= nx2; x++)
                {
                    hex = map.GetHexagon(x, y);
                    if (hex != null)
                    {
                        hex.blockFlag = blockFlag;
                    }
                }
            }
        }

        void OnEditLand(Hexagon hex)
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (hex == null)
                {
                    Debug.LogError("hex is null");
                }
                else
                {
                    hex.GetRoundHexagons(ref _tempHexes, MapTool.Instance._Tool_BrushRound - 1);
                }

                foreach (var t in _tempHexes)
                {
                    t.movetype = MapTool.Instance._EditLand_MoveType;
                }
            }
        }

        void OnEditTerritory(Hexagon hex)
        {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt) &&
                Input.GetKey(KeyCode.LeftShift))
            {
                if (hex != null && hex.zone != null)
                {
                    map.AddZone(hex);
                    Debug.Log("切割区域");
                }
            }
            else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (hex == null)
                {
                    Debug.LogError("hex is null");
                    return;
                }

                var zone = hex.zone;
                if (zone != null)
                {
                    hex.GetRoundHexagons(ref _tempHexes, MapTool.Instance._Tool_BrushRound - 1);
                    bool isClock = Input.GetKey(KeyCode.LeftShift);
                    foreach (var t in _tempHexes)
                    {
                        bool pass = false;
                        if (isClock)
                        {
                            if (hex.movetype == MOVETYPE.DISABLE && t.movetype != MOVETYPE.DISABLE)
                                pass = true;
                            else if (hex.movetype > MOVETYPE.DISABLE &&
                                     t.movetype == MOVETYPE.DISABLE)
                                pass = true;
                        }

                        if (pass)
                            continue;
                        if (t.zone != zone)
                        {
                            bBrushSelect = true;
                            SelectZone(t.zone);

                            if (t.zone != null) t.zone.RemoveHexagon(t);
                            zone.AddHexagon(t, true);
                            SelectZone(zone);
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
            {
                if (hex != null && hex.zone != null)
                {
                    MapTool.Instance.EditSea_SetMergeZone(hex.zone);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (hex != null && hex.zone != null)
                {
                    MapTool.Instance.EditSea_SetCurZone(hex.zone);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (bBrushSelect)
                {
                    foreach (var zone in _changedZones)
                    {
                        zone.CalcEdges();
                    }

                    bBrushSelect = false;
                    _changedZones.Clear();
                }
            }
        }

        void OnEditBusinessZone(Hexagon hex)
        {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
            {
                if (hex != null && hex.zone != null)
                {
                    MapTool.Instance.EditBusiness_SetMergeZone(hex.zone);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (hex != null && hex.zone != null)
                {
                    MapTool.Instance.EditBusiness_SetCurZone(hex.zone);
                }
            }
        }

        void OnEditSea(Hexagon hex)
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (hex == null)
                {
                    Debug.LogError("hex is null");
                    return;
                }

                hex.GetRoundHexagons(ref _tempHexes, MapTool.Instance._Tool_BrushRound - 1);

                int mark = 1 << (int)MapTool.Instance._EditSea_MoveType;
                foreach (var t in _tempHexes)
                {
                    if ((t.movetypeMark & t.movetypeMarkRandomSea & mark) > 0)
                        t.movetype = MapTool.Instance._EditSea_MoveType;
                }
            }
        }

        void GetClipRect(Vector3 screenPos1, Vector3 screenPos2, out int minX, out int minZ,
            out int maxX, out int maxZ)
        {
            var mapWidth = map != null ? map.width : 1200;
            var mapHeight = map != null ? map.height : 1200;
            var coord1 = PickHex(screenPos1);
            var coord2 = PickHex(screenPos2);

            minX = Mathf.Min(coord1.x, coord2.x); // + mapWidth / 2;
            maxX = Mathf.Max(coord1.x, coord2.x); // + mapWidth / 2;

            minZ = Mathf.Min(coord1.y, coord2.y); // + mapWidth / 2;
            maxZ = Mathf.Max(coord1.y, coord2.y); // + mapWidth / 2;

            if (minX < 0) minX = 0;
            if (maxX >= mapWidth) maxX = mapWidth - 1;
            if (minZ < 0) minZ = 0;
            if (maxZ >= mapHeight) maxZ = mapHeight - 1;
        }

        void SelectZone(Zone zone)
        {
            if (zone == null)
                return;
            if (_changedZones.Contains(zone))
                return;

            _changedZones.Add(zone);
        }


        //DrawSmallMapEdges
        private Vector2Int tmpIndex1 = Vector2Int.zero;
        private Vector2Int tmpIndex2 = Vector2Int.zero;

        private void DrawSmallMapEdges()
        {
            tmpIndex1.y = MapRender.instance.map.startHexPos.y;
            tmpIndex1.x = MapRender.instance.map.endHexPos.x;

            tmpIndex2.y = MapRender.instance.map.endHexPos.y;
            tmpIndex2.x = MapRender.instance.map.startHexPos.x;

            int y = tmpIndex1.y;
            int x = MapRender.instance.map.startHexPos.x;
            for (int i = x; i <= tmpIndex1.x; i++)
            {
                DrawHexagon(GetHexagonPos(x, y), Color.black, false);
                x = x + 1;
            }

            y = tmpIndex2.y;
            x = tmpIndex2.x;
            for (int i = x; i <= MapRender.instance.map.endHexPos.x; i++)
            {
                DrawHexagon(GetHexagonPos(x, y), Color.black, false);
                x = x + 1;
            }

            y = tmpIndex1.y + 1;
            x = tmpIndex1.x;
            for (int i = y; i <= MapRender.instance.map.endHexPos.y - 1; i++)
            {
                DrawHexagon(GetHexagonPos(x, y), Color.black, false);
                y = y + 1;
            }

            y = MapRender.instance.map.startHexPos.y + 1;
            x = MapRender.instance.map.startHexPos.x;
            for (int i = y; i <= tmpIndex2.y - 1; i++)
            {
                DrawHexagon(GetHexagonPos(x, y), Color.black, false);
                y = y + 1;
            }
        }

        void OnRenderObject()
        {
            if (lineMaterial == null || map == null || mainCamera == null)
                return;

            if (saveZoneModel)
            {
                OnRenderSaveZone();
                return;
            }

            lineMaterial.SetPass(0);

            GL.PushMatrix();
            //GL.LoadPixelMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            // 绘制
            // 绘制范围
            DrawRect(new Vector3(map.mapSizeW * 0.5f, 0f, -map.mapSizeH * 0.5f), map.mapSizeW, map.mapSizeH,
                Color.black);
            // 绘制城点
            DrawCity();

            var brushHexagons = new Dictionary<int, Hexagon>();
            // 笔刷
            switch (MapTool.Instance.tool)
            {
                case MapToolOP.EditTerritory:
                case MapToolOP.EditBlock:
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (pointHexagon == null)
                            return;
                        pointHexagon.GetRoundHexagons(ref _tempHexes, MapTool.Instance._Tool_BrushRound - 1);
                        foreach (var t in _tempHexes)
                        {
                            brushHexagons.Add(t.index, t);
                        }
                    }

                    // DrawRound(pointHexagon, Color.black, MapTool.Instance._Tool_BrushRound - 1);
                    break;
            }

            var curtool = MapTool.Instance.tool;
            // TODO draw 1.常显 2.对应工具辅助显示
            if (zoom > 300 && map.mapProgress > HexMap.MapProgress.None)
            {
                GetClipRect(new Vector3(0, 0, 0), new Vector3(Screen.width, Screen.height, 0),
                    out int nx1,
                    out int nz1,
                    out int nx2, out int nz2);

                if (curtool == MapToolOP.EditBlock || bShowBlock)
                {
                    for (int y = nz1; y <= nz2; y++)
                    {
                        for (int x = nx1; x <= nx2; x++)
                        {
                            //var color = blockData.GetPixel(x, mapWidth - y);
                            if (map.GetMoveType(x, y) == MOVETYPE.DISABLE)
                            {
                                DrawHexagon(GetHexagonPos(x, y), Color.red, true);
                            }
                        }
                    }
                }

                // 边界线
                foreach (var zone in map.zones.Values)
                {
                    if (zone.hexagons.Count > 0 && zone.edges.Count > 0)
                    {
                        var city = cityRenders[zone.level];
                        var color = zone.level > 0
                            ? city.color
                            : (zone.landType == ZoneLandType.Sea ? Color.black : Color.red);
                        DrawEdgeLine(zone, color);
                    }
                }
            }
            else
            {
                GetClipRect(new Vector3(0, 0, 0), new Vector3(Screen.width, Screen.height, 0),
                    out int nx1, out int nz1,
                    out int nx2, out int nz2);

                var blockColor = new Color(1f, 0f, 0f, 0.5f);
                var tw = hexW * 0.5f;


                for (int y = nz1; y <= nz2; y++)
                {
                    for (int x = nx1; x <= nx2; x++)
                    {
                        var hex = map.GetHexagon(x, y);
                        if (hex != null)
                        {
                            var zone = hex.zone;
                            //if (hex.bEdge && bShowEdge)
                            if (hex.edgeIndex == 0 && bShowEdge)
                            {
                                if (zone != null)
                                    DrawHexagon(GetHexagonPos(x, y),
                                        map.GetZoneFrameColor(zone.color), false);
                                else
                                    DrawHexagon(GetHexagonPos(x, y), Color.black, true);
                            }
                            else
                            {
                                if (zone != null)
                                {
                                    var index = y * map.width + x;
                                    var color = GetDrawColor(zone);
                                    if (brushHexagons.ContainsKey(index))
                                    {
                                        DrawHexagon(GetHexagonPos(x, y), Color.green);
                                    }
                                    else
                                    {
                                        DrawHexagon(GetHexagonPos(x, y), color, MapRender.instance.bShowGrid);
                                    }
                                }
                                else
                                    DrawHexagon(GetHexagonPos(x, y),
                                        hex.movetype == MOVETYPE.DISABLE ? Color.red : Color.black);
                            }

                            if (curtool == MapToolOP.EditBlock || bShowBlock)
                            {
                                // if (map.IsBlock(x, y))
                                if (map.GetMoveType(x, y) == MOVETYPE.DISABLE)
                                {
                                    // DrawBlockRect(GetHexagonPos(x, y), tw, tw, blockColor);
                                    DrawHexagon(GetHexagonPos(x, y), blockColor);
                                }
                            }

                            if (curtool == MapToolOP.EditInteract)
                            {
                                if (IsInteractBlock(hex))
                                {
                                    DrawHexagon(GetHexagonPos(x, y), Color.red);
                                }
                            }
                        }
                    }
                }
            }

            // 绘制选中区域的边界
            DrawSelectZoneEdge();

            // 矩形选框
            DrawRectSelect();


            // Debug
            DrawQT();

            GL.PopMatrix();
        }

        void DrawRectSelect()
        {
            if (!bRectSelect)
                return;

            var coord1 = PickHex(rectStartPos);
            var coord2 = PickHex(Input.mousePosition);
            DrawRect(Hex.OffsetToWorld(coord1), Hex.OffsetToWorld(coord2), Color.red);
        }

        List<Hexagon> _tempHexes = new List<Hexagon>();

        void DrawRound(Hexagon hex, Color fillColor, int round = 1)
        {
            if (hex == null)
                return;

            hex.GetRoundHexagons(ref _tempHexes, round);

            foreach (var t in _tempHexes)
            {
                DrawHexagon(GetHexagonPos(t), fillColor);
            }
        }

        void DrawRect(Vector3 lt, Vector3 rb, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            GL.Vertex3(lt.x, lt.y, lt.z);
            GL.Vertex3(rb.x, lt.y, lt.z);
            GL.Vertex3(rb.x, rb.y, rb.z);
            GL.Vertex3(lt.x, rb.y, rb.z);
            GL.Vertex3(lt.x, lt.y, lt.z);
            GL.End();
        }

        void DrawRect(Vector2 lt, Vector2 rb, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            GL.Vertex3(lt.x, 0, lt.y);
            GL.Vertex3(rb.x, 0, lt.y);
            GL.Vertex3(rb.x, 0, rb.y);
            GL.Vertex3(lt.x, 0, rb.y);
            GL.Vertex3(lt.x, 0, lt.y);
            GL.End();
        }

        void DrawRect(Vector3 p, float w, float h, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            var tw = w * 0.5f;
            var th = h * 0.5f;
            GL.Vertex3(p.x - tw, p.y, p.z - th);
            GL.Vertex3(p.x + tw, p.y, p.z - th);
            GL.Vertex3(p.x + tw, p.y, p.z + th);
            GL.Vertex3(p.x - tw, p.y, p.z + th);
            GL.Vertex3(p.x - tw, p.y, p.z - th);
            GL.End();
        }

        void DrawBlockRect(Vector3 p, float tw, float th, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            GL.Vertex3(p.x - tw, p.y, p.z - th);
            GL.Vertex3(p.x + tw, p.y, p.z - th);
            GL.Vertex3(p.x + tw, p.y, p.z + th);
            GL.Vertex3(p.x - tw, p.y, p.z + th);
            GL.Vertex3(p.x - tw, p.y, p.z - th);
            GL.End();
        }

        private float hexW;
        private float hexH;
        private float hexSide;

        void DrawHexagon(Vector3 p, Color color, bool bGrid = false)
        {
            if (map == null)
                return;

            hexSide = Hex.HexRadius;
            hexH = hexSide * 0.5f;
            hexW = hexSide * Hex.Sqrt3 / 2;
            // hexW =
            // hexH = 1;

            DrawHexagon(p, color, hexW, hexH, hexSide, bGrid);
            // DrawHexagonMesh(p, color, map.hexMesh);
        }

        Matrix4x4 renderMatrix;

        void DrawHexagonMesh(Vector3 p, Color color, Mesh mesh)
        {
            renderMatrix = Matrix4x4.TRS(p, Quaternion.identity, Vector3.one);
            Graphics.DrawMesh(mesh, renderMatrix, hexMaterial, 0);
        }

        void DrawHexagon(Vector3 p, Color color, float hexW, float hexH, float hexSide,
            bool bGrid = false)
        {
            color.a = MapRender.instance.fHexagonAlpha;

            if (bGrid)
            {
                // GL.Begin(GL.LINE_STRIP);
                // GL.Color(color);
                // GL.Vertex3(p.x - hexSide, p.y, p.z); // 左
                // GL.Vertex3(p.x - hexSide / 2, p.y, p.z - hexH); // 左下
                // GL.Vertex3(p.x + hexSide / 2, p.y, p.z - hexH); // 右下
                // GL.Vertex3(p.x + hexSide, p.y, p.z); // 右
                // GL.Vertex3(p.x + hexSide / 2, p.y, p.z + hexH); // 右上
                // GL.Vertex3(p.x - hexSide / 2, p.y, p.z + hexH); // 左上
                // GL.Vertex3(p.x - hexSide, p.y, p.z); // 左
                // GL.End();

                GL.Begin(GL.LINE_STRIP);
                GL.Color(color);
                GL.Vertex3(p.x - hexW, p.y, p.z - hexH);
                GL.Vertex3(p.x, p.y, p.z - hexSide);
                GL.Vertex3(p.x + hexW, p.y, p.z - hexH);
                GL.Vertex3(p.x + hexW, p.y, p.z + hexH);
                GL.Vertex3(p.x, p.y, p.z + hexSide);
                GL.Vertex3(p.x - hexW, p.y, p.z + hexH);
                GL.Vertex3(p.x - hexW, p.y, p.z - hexH);
                GL.End();
            }
            else
            {
                GL.Begin(GL.TRIANGLES);
                GL.Color(color);
                GL.Vertex3(p.x, p.y, p.z);
                // GL.Vertex3(p.x - hexSide, p.y, p.z);
                // GL.Vertex3(p.x - hexSide / 2, p.y, p.z - hexH);
                GL.Vertex3(p.x - hexW, p.y, p.z - hexH);
                GL.Vertex3(p.x, p.y, p.z - hexSide);

                GL.Vertex3(p.x, p.y, p.z);
                // GL.Vertex3(p.x - hexSide / 2, p.y, p.z - hexH);
                // GL.Vertex3(p.x + hexSide / 2, p.y, p.z - hexH);
                GL.Vertex3(p.x, p.y, p.z - hexSide);
                GL.Vertex3(p.x + hexW, p.y, p.z - hexH);

                GL.Vertex3(p.x, p.y, p.z);
                // GL.Vertex3(p.x + hexSide / 2, p.y, p.z - hexH);
                // GL.Vertex3(p.x + hexSide, p.y, p.z);
                GL.Vertex3(p.x + hexW, p.y, p.z - hexH);
                GL.Vertex3(p.x + hexW, p.y, p.z + hexH);

                GL.Vertex3(p.x, p.y, p.z);
                // GL.Vertex3(p.x + hexSide, p.y, p.z);
                // GL.Vertex3(p.x + hexSide / 2, p.y, p.z + hexH);
                GL.Vertex3(p.x + hexW, p.y, p.z + hexH);
                GL.Vertex3(p.x, p.y, p.z + hexSide);

                GL.Vertex3(p.x, p.y, p.z);
                // GL.Vertex3(p.x + hexSide / 2, p.y, p.z + hexH);
                // GL.Vertex3(p.x - hexSide / 2, p.y, p.z + hexH);
                GL.Vertex3(p.x, p.y, p.z + hexSide);
                GL.Vertex3(p.x - hexW, p.y, p.z + hexH);

                GL.Vertex3(p.x, p.y, p.z);
                // GL.Vertex3(p.x - hexSide / 2, p.y, p.z + hexH);
                // GL.Vertex3(p.x - hexSide, p.y, p.z);
                GL.Vertex3(p.x - hexW, p.y, p.z + hexH);
                GL.Vertex3(p.x - hexW, p.y, p.z - hexH);
                GL.End();
            }
        }

        void DrawEdgeMesh(Zone zone, Color color)
        {
            GL.Begin(GL.TRIANGLES);

            edgeMaterial.SetPass(0);

            foreach (var mesh in zone.edgeMeshs.Values)
            {
                mesh.DrawMesh();
                edgeMaterial.SetPass(0);
            }

            GL.End();
        }

        void DrawEdgeLine(Zone zone, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);

            var list = (zone.keyEdges.Count > 0) ? zone.keyEdges : zone.edges;
            Vector3 p;
            foreach (var hex in list)
            {
                p = GetHexagonPos(hex);
                GL.Vertex3(p.x, p.y, p.z);
            }

            p = GetHexagonPos(list[0]);
            GL.Vertex3(p.x, p.y, p.z);
            GL.End();
        }

        void DrawBusinessLine(Zone zone)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(Color.green);

            var list = (zone.keyEdges.Count > 0) ? zone.keyEdges : zone.edges;
            Vector3 p1 = Vector3.zero;

            foreach (var hex in list)
            {
                if (hex.IsBusinessEdge() == false)
                {
                    var p = GetHexagonPos(hex);
                    if (Vector3.Distance(p, p1) < 5)
                    {
                        GL.Vertex3(p.x, p.y, p.z);
                    }
                    else
                    {
                        GL.End();
                        GL.Begin(GL.LINE_STRIP);
                        GL.Color(Color.green);
                    }

                    p1 = p;
                }
            }

            GL.End();

            // GL.Begin(GL.LINE_STRIP);
            // GL.Color(Color.red);
            //
            // Vector3 p2 = Vector3.zero;
            // foreach (var hex in list)
            // {
            // 	if (hex.IsBusinessEdge())
            // 	{
            // 		var p = GetHexagonPos(hex);
            // 		if (Vector3.Distance(p, p2) < 5)
            // 		{
            // 			GL.Vertex3(p.x, p.y, p.z);
            // 		}
            // 		else
            // 		{
            // 			GL.End();
            // 			GL.Begin(GL.LINE_STRIP);
            // 			GL.Color(Color.red);
            // 		}
            // 		p2 = p;
            // 	}
            // }
            // GL.End();


            foreach (var points in map.businessEdgeWayPoints.Values)
            {
                GL.Begin(GL.LINE_STRIP);
                GL.Color(Color.red);
                foreach (var p in points)
                {
                    GL.Vertex3(p.x, p.y, p.z);
                }

                GL.End();
            }
        }

        void DrawCity()
        {
            if (bShowCity)
            {
                foreach (var zone in map.zones.Values)
                {
                    if (zone.visible == 1 && zone.hexagons.Count > 0 && zone.edges.Count > 0)
                    {
                        var city = cityRenders[zone.level];
                        DrawRound(zone.hexagon, city.color, city.round);
                    }
                }
            }
        }

        void DrawSelectZoneEdge()
        {
            if (bShowWireframe)
            {
                if (OperationHex != null && OperationHex.zone != null)
                {
                    if (OperationHex.zone.hexagons.Count > 0 && OperationHex.zone.edges.Count > 0)
                    {
                        var city = cityRenders[OperationHex.zone.level];
                        DrawEdgeMesh(OperationHex.zone, city.color);
                    }
                }
            }
        }

        void DrawBusinessEdge()
        {
            if (bShowBusinessEdge)
            {
                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.red);

                businessEdgeMaterial.SetPass(0);

                foreach (var mesh in map.businessEdgeMeshes.Values)
                {
                    int idx = 0;
                    var triangles = mesh.triangles;
                    var vertexs = mesh.vertices;
                    var uvs = mesh.uv;
                    while (true)
                    {
                        if (idx + 2 >= triangles.Length)
                            break;

                        var p0 = vertexs[triangles[idx]];
                        var p1 = vertexs[triangles[idx + 1]];
                        var p2 = vertexs[triangles[idx + 2]];
                        var uv1 = uvs[triangles[idx]];
                        var uv2 = uvs[triangles[idx + 1]];
                        var uv3 = uvs[triangles[idx + 2]];

                        idx += 3;

                        GL.TexCoord2(uv1.x, uv1.y);
                        GL.Vertex(p0);
                        GL.TexCoord2(uv2.x, uv2.y);
                        GL.Vertex(p1);
                        GL.TexCoord2(uv3.x, uv3.y);
                        GL.Vertex(p2);
                    }
                }

                GL.End();
            }
        }

        void DrawTreasures()
        {
            if (bShowTreasure)
            {
                GL.Begin(GL.TRIANGLES);
                GL.Color(Color.red);

                businessEdgeMaterial.SetPass(0);

                foreach (var mesh in map.businessEdgeMeshes.Values)
                {
                    int idx = 0;
                    var triangles = mesh.triangles;
                    var vertexs = mesh.vertices;
                    var uvs = mesh.uv;
                    while (true)
                    {
                        if (idx + 2 >= triangles.Length)
                            break;

                        var p0 = vertexs[triangles[idx]];
                        var p1 = vertexs[triangles[idx + 1]];
                        var p2 = vertexs[triangles[idx + 2]];
                        var uv1 = uvs[triangles[idx]];
                        var uv2 = uvs[triangles[idx + 1]];
                        var uv3 = uvs[triangles[idx + 2]];

                        idx += 3;

                        GL.TexCoord2(uv1.x, uv1.y);
                        GL.Vertex(p0);
                        GL.TexCoord2(uv2.x, uv2.y);
                        GL.Vertex(p1);
                        GL.TexCoord2(uv3.x, uv3.y);
                        GL.Vertex(p2);
                    }
                }

                GL.End();
            }
        }

        void OnEditSailLine(Hexagon hex)
        {
            if (beginHexagon != null && beginHexagon != hex)
            {
                hex.attribute = HexAttribute.SailPoint;
                map.AddSailPoint(hex.index);
                map.AddConnect(beginHexagon.index, hex.index);
            }

            beginHexagon = null;
            endHexagon = null;
        }

        #region 岛屿模板相关

        void OnEditIsland(Hexagon hex)
        {
            if (islandTemplate != null)
            {
                islandTemplate.AddOwnHex(hex.x - islandOffset, hex.y - islandOffset);
            }
        }

        bool IsInteractBlock(Hexagon hex)
        {
            if (SelectInteractBlockId != 0)
            {
                var interactionPoint = CurrentMapInteractionPoints.Find(m=>m.id == SelectInteractBlockId);
                if (interactionPoint != null && interactionPoint.IsBlockPosition(hex.x, hex.y))
                {
                    return true;
                }
                // if (interactBlockPoints.ContainsKey(SelectInteractBlockId))
                // {
                //     foreach (var point in interactBlockPoints[SelectInteractBlockId])
                //     {
                //         if (point.x == hex.x && point.y == hex.y)
                //         {
                //             return true;
                //         }
                //     }
                // }
            }

            return false;
        }

        void OnEditInteraction(Hexagon hex)
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                if (SelectInteractBlockId != 0)
                {
                    var interactionPoint = CurrentMapInteractionPoints.Find(m=>m.id == SelectInteractBlockId);
                    if (interactionPoint != null)
                    {
                        foreach (var point in interactionPoint.BlockPoints)
                        {
                            if (point.x == hex.x && point.y == hex.y)
                            {
                                return;
                            }
                        }

                        interactionPoint.BlockPoints.Add(new Vector2Int(hex.x, hex.y));
                    }
                    else
                    {
                        // interactBlockPoints[SelectInteractBlockId] = new List<Vector2Int>
                        //     { new Vector2Int(hex.x, hex.y) };
                    }
                }
            }

            if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))
            {
                if (SelectInteractBlockId != 0)
                {
                    var interactionPoint = CurrentMapInteractionPoints.Find(m=>m.id == SelectInteractBlockId);
                    if (interactionPoint != null)
                    {
                        interactionPoint.BlockPoints.RemoveAll(p => p.x == hex.x && p.y == hex.y);
                    }
                    // if (interactBlockPoints.ContainsKey(SelectInteractBlockId))
                    // {
                    //     interactBlockPoints[SelectInteractBlockId]
                    //         .Remove(new Vector2Int(hex.x, hex.y));
                    // }
                }
            }
        }

        void OnEditFogOfWar(Hexagon hex)
        {

        }

        void OnEditIslandPlace(Hexagon hex)
        {
            if (islandTemplate != null)
            {
                islandTemplate.AddPlaceHex(hex.x - islandOffset, hex.y - islandOffset);
            }
        }

        void SaveIslands(string filename)
        {
            var bw = new BinaryWriter(new FileStream(filename, FileMode.Create));
            map.WriteIslandData(bw);
            bw.Close();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("保存岛屿模板", $"模板保存完成!!! \n{filename}", "确定");
        }

        public void ImportIslandTemplate()
        {
            GetSaveFileDialog("保存岛屿模板", SaveIslands);
        }

        void LoadIslands(string filename)
        {
            var br = new BinaryReader(new FileStream(filename, FileMode.Open));
            map.ReadIslandData(br);
            br.Close();
            EditorUtility.DisplayDialog("保存岛屿模板", $"模板加载完成!!! \n{filename}", "确定");
        }

        #endregion

        void DrawSailLine()
        {
            if (Operation != OperationType.EditSailLine)
            {
                return;
            }

            var hasDraw = new HashSet<string>();
            foreach (var connect in map.connect)
            {
                var begin = connect.Key;
                foreach (var target in connect.Value)
                {
                    var key = begin < target ? $"{begin}-{target}" : $"{target}-{begin}";
                    if (hasDraw.Contains(key) == false)
                    {
                        hasDraw.Add(key);

                        var hex1 = GetHexagonPos(map.hexagons[begin]);
                        var hex2 = GetHexagonPos(map.hexagons[target]);
                        DrawStraightLine(hex1, hex2, Color.cyan);
                    }
                }
            }

            if (beginHexagon != null && endHexagon != null)
            {
                var hexBegin = GetHexagonPos(beginHexagon);
                var hexEnd = GetHexagonPos(endHexagon);
                DrawStraightLine(hexBegin, hexEnd, Color.cyan);
            }
        }

        void DrawStraightLine(Vector3 start, Vector3 end, Color color)
        {
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
        }

        #region 航线导出

        public bool CheckSailLine()
        {
            var pass = true;
            foreach (var idx in map.connect.Keys)
            {
                var hexagon = map.GetHexagon(idx);
                if (idx == hexagon.zone._portSailIndex)
                {
                    if (map.connect[idx].Count <= 0)
                    {
                        Debug.LogError($"主航道点没连接! {hexagon.x} , {hexagon.y}");
                        pass = false;
                    }
                }
                else
                {
                    if (map.connect[idx].Count < 2)
                    {
                        Debug.LogError($"辅助航道点连接异常! {hexagon.x} , {hexagon.y}");
                        pass = false;
                    }
                }

                foreach (var cIdx in map.connect[idx])
                {
                    if (map.connect[cIdx].Contains(idx) == false)
                    {
                        var cHexagon = map.GetHexagon(cIdx);
                        Debug.LogError(
                            $"航道点连接异常 不是互相连接关系! {hexagon.x} , {hexagon.y} - {cHexagon.x} , {cHexagon.y}");
                        pass = false;
                    }
                }
            }

            return pass;
        }

        #endregion

        bool saveZoneModel = false;
        Zone curSaveZone = null;
        Vector2Int saveDrawOffset = Vector2Int.zero;

        IEnumerator SaveZoneLineImage(string fileName)
        {
            saveZoneModel = true;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;

            var sfile = Path.GetFileName(fileName);
            var spath = Path.GetDirectoryName(fileName);

            foreach (var zone in map.zones.Values)
            {
                curSaveZone = zone;
                yield return new WaitForEndOfFrame();

                var filePath = Path.Combine(spath, sfile + "_" + zone.index + ".png");
                SaveZoneImage(curSaveZone, filePath);
                yield return new WaitForEndOfFrame();
            }

            mainCamera.clearFlags = CameraClearFlags.Skybox;
            saveZoneModel = false;

            yield return new WaitForSeconds(0.5f);

            EditorUtility.DisplayDialog("保存线图", $"保存线图完成，共{map.zones.Values.Count}张！", "确定");
        }

        IEnumerator SaveBigLineImage(string bigfile)
        {
            saveZoneModel = true;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            curSaveZone = null;

            yield return new WaitForEndOfFrame();

            int w = map.width * zoneScale;
            int h = map.height * zoneScale;
            int cw = Screen.width - zoneOffx * 2;
            int ch = Screen.height - zoneOffy * 2;

            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);

            int cx = Mathf.CeilToInt(w / cw) + 1;
            int cy = Mathf.CeilToInt(h / ch) + 1;

            for (int j = 0; j < cy; j++)
            {
                for (int i = 0; i < cx; i++)
                {
                    saveDrawOffset = new Vector2Int(-i * cw, -j * ch);
                    yield return new WaitForEndOfFrame();
                    tex.ReadPixels(new Rect(zoneOffx, zoneOffy, cw, ch), -saveDrawOffset.x,
                        -saveDrawOffset.y);
                    tex.Apply();
                    yield return new WaitForEndOfFrame();
                }
            }

            Color32 dot1 = new Color32(255, 255, 255, 0);
            Color32 dot2 = new Color32(0, 0, 0, 255);
            Color32[] colors = new Color32[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var c = tex.GetPixel(x, y);
                    if (c.r > 0.9f)
                    {
                        colors[y * w + x] = dot1;
                    }
                    else
                    {
                        colors[y * w + x] = dot2;
                    }
                }
            }

            tex.SetPixels32(colors);
            tex.Apply();

            byte[] dataBytes = tex.EncodeToPNG();
            FileStream fs = File.Open(bigfile, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            Debug.Log($"SaveBigImage : {bigfile}");

            yield return new WaitForEndOfFrame();

            mainCamera.clearFlags = CameraClearFlags.Skybox;
            saveZoneModel = false;

            yield return new WaitForSeconds(1f);

            EditorUtility.DisplayDialog("输出全图", $"保存全图完成！\n{bigfile}", "确定");
        }

        void OnRenderSaveZone()
        {
            GL.PushMatrix(); //保存当前Matirx
            lineMaterial.SetPass(0); //刷新当前材质
            GL.LoadPixelMatrix(); //设置pixelMatrix
            GL.Color(Color.white);

            if (curSaveZone != null)
            {
                DrawEdge(curSaveZone, Color.black);
            }
            else
            {
                DrawAllEdges(Color.black, saveDrawOffset);
            }

            GL.End();
            GL.PopMatrix(); //读取之前的Matrix
        }

        void OnRenderIslandTemplate()
        {
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            if (islandTemplate == null)
            {
                islandTemplate = new IslandTemplate();
            }

            if (islandHexCoords == null)
            {
                islandHexCoords = Axial.Spiral(new Axial(islandOffset, islandOffset),
                    IslandTemplate.Radius);
                FocusHexagon(map.GetHexagon(islandOffset, islandOffset), 30.0f);
            }

            foreach (var hex in islandHexCoords)
            {
                bool grid = true;
                Color color = Color.cyan;
                if (islandTemplate.ExistOwnHex(hex.q - islandOffset, hex.r - islandOffset, true))
                {
                    grid = false;
                    color = Color.blue;
                }

                if (islandTemplate.ExistPlaceHex(hex.q - islandOffset, hex.r - islandOffset, true))
                {
                    grid = false;
                    color = Color.magenta;
                }

                if (hex.q == islandOffset && hex.r == islandOffset)
                {
                    grid = false;
                    color = Color.red;
                }

                DrawHexagon(GetHexagonPos(hex.q, hex.r), color, grid);
            }

            GL.PopMatrix();
        }

        int zoneOffx = 5;
        int zoneOffy = 5;
        int zoneScale = 1;

        public void SaveZoneImage(Zone zone, string filename)
        {
            if (zone == null)
                return;

            zone.GetImageInfo(zoneScale, out int w, out int h, out int tx, out int ty);

            Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(zoneOffx, zoneOffy, w, h), 0, 0);
            tex.SetPixel(0, 0, Color.black);
            tex.SetPixel(w - 1, h - 1, Color.black);
            tex.Apply();

            byte[] dataBytes = tex.EncodeToPNG();
            FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            Debug.Log($"SaveImage : {filename}");
        }

        public void DrawEdge(Zone zone, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);

            zone.GetImageInfo(zoneScale, out int w, out int h, out int tx, out int ty);

            int _x;
            int _y;
            foreach (var hex in zone.edges)
            {
                _x = (hex.x - tx) * zoneScale;
                _y = h - (hex.y - ty + 1) * zoneScale;
                GL.Vertex3(_x + zoneOffx, _y + zoneOffy, 0);
            }

            var last = zone.edges[0];
            _x = (last.x - tx) * zoneScale;
            _y = h - (last.y - ty + 1) * zoneScale;
            GL.Vertex3(_x + zoneOffx, _y + zoneOffy, 0);
            GL.End();
        }

        public void DrawAllEdges(Color color, Vector2Int drawOffset)
        {
            foreach (var zone in map.zones.Values)
            {
                if (zone.hexagons.Count > 0 && zone.edges.Count > 0)
                {
                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);

                    int _x;
                    int _y;
                    foreach (var hex in zone.edges)
                    {
                        _x = hex.x * zoneScale;
                        _y = (map.height - (hex.y + 1)) * zoneScale;
                        GL.Vertex3(_x + zoneOffx + drawOffset.x, _y + zoneOffy + drawOffset.y, 0);
                    }

                    var last = zone.edges[0];
                    _x = last.x * zoneScale;
                    _y = (map.height - (last.y + 1)) * zoneScale;
                    GL.Vertex3(_x + zoneOffx + drawOffset.x, _y + zoneOffy + drawOffset.y, 0);
                    GL.End();

                    _x = zone.hexagon.x * zoneScale;
                    _y = (map.height - (zone.hexagon.y + 1)) * zoneScale;
                    var p = new Vector3(_x + zoneOffx + drawOffset.x, _y + zoneOffy + drawOffset.y,
                        0);

                    //DrawRect(pt, 6f, 6f, Color.black);

                    var hexW = map.hexW * 8f;
                    var hexH = map.hexH * 8f;
                    var hexSide = Hex.HexRadius * 8f;

                    GL.Begin(GL.LINE_STRIP);
                    GL.Color(color);
                    GL.Vertex3(p.x - hexW, p.y - hexH, p.z);
                    GL.Vertex3(p.x, p.y - hexSide, p.z);
                    GL.Vertex3(p.x + hexW, p.y - hexH, p.z);
                    GL.Vertex3(p.x + hexW, p.y + hexH, p.z);
                    GL.Vertex3(p.x, p.y + hexSide, p.z);
                    GL.Vertex3(p.x - hexW, p.y + hexH, p.z);
                    GL.Vertex3(p.x - hexW, p.y - hexH, p.z);
                    GL.End();
                }
            }
        }

        public void CreateMap(int w, int h, string mapName, int mapId = 0)
        {
            prefabPainter?.Reset();
            uiManager?.Reset();
            if (map != null)
            {
                map.Clear();
            }
            map = new HexMap()
            {
                Name = mapName,
            };
            try
            {
                map.Create(w, h,
                    (s, f) => { EditorUtility.DisplayProgressBar("创建地图", s, f); });
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                if (GlobalDef.S_MapEditorType == MapEditorType.EPVE)
                {
                    var campaignMapInfo = new CampaignMapInfo
                    {
                        Id = mapId!=0?mapId:GenerateNewMapId(),
                        Name = mapName,
                    };
                    CreateCampaignMap(campaignMapInfo);
                }
            }
        }

        public void IdentifyMap(Texture2D tex)
        {
            if (!CheckMapValid())
                return;

            map.IdentifyTex(tex, (s, f) => { EditorUtility.DisplayProgressBar("分析设计图", s, f); });
            EditorUtility.ClearProgressBar();
        }

        public void CreateLandZones()
        {
            return;
            if (!CheckMapValid())
                return;

            map.CreateLandZones((s, f) => { EditorUtility.DisplayProgressBar("生成陆地区域", s, f); });
            EditorUtility.ClearProgressBar();
        }

        public void CreateZones(AreaScatterSetting centerSetting,
            AreaScatterSetting[] settings)
        {
            if (!CheckMapValid())
                return;

            try
            {
                map.CreateZones(centerSetting, settings,
                    (s, f) => { EditorUtility.DisplayProgressBar("划分海域", s, f); });
                MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateAllEvent);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public void CreateZones(Texture2D tex, Dictionary<Color32, Vector3Int> colorLevelMap = null)
        {
            if (!CheckMapValid())
                return;

            try
            {
                map.CreateZoneByTex(tex, 1201, 1201, colorLevelMap,
                    (s, f) => { EditorUtility.DisplayProgressBar("划分海域", s, f); });
                MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateAllEvent);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public void CreateHarbors()
        {
            if (!CheckMapValid())
                return;

            map.CreatePorts((s, f) => { EditorUtility.DisplayProgressBar("生成港口", s, f); });
            EditorUtility.ClearProgressBar();
        }

        public void FixHarbors()
        {
            if (!CheckMapValid())
                return;

            map.FixPorts((s, f) => { EditorUtility.DisplayProgressBar("修复港口", s, f); });
            EditorUtility.ClearProgressBar();
        }

        // public void CreateEmpyMap(int w, int h)
        // {
        // 	map = new HexMap();
        // 	map.CreateEmpty(blockData, w, h);
        // }

        public void DoStepExpand()
        {
            if (CheckMapValid())
                map.DoStepExpand();
        }

        public void CheckRuins()
        {
            if (CheckMapValid())
                map.CheckRuins();
        }

        public void CalcEdges()
        {
            if (CheckMapValid())
            {
                map.CalcEdges((msg, value) => { EditorUtility.DisplayProgressBar("刷新边界", msg, value); });
                EditorUtility.ClearProgressBar();
            }
        }

        //设置需要调整哨塔的zone
        public void SetupNeedAdjustmentZones()
        {
            map.ClearAdjustmentZonesList();
            foreach (var zone in map.zones.Values)
            {
                //只有有主城的城市，需要检查哨塔个数
                if (zone.visible > 0 && zone.isGuanqia == 0 && zone.subType == 0)
                {
                    if (zone.IsNeedAdjustment() == true)
                    {
                        map.AddAdjustmentZone(zone);
                    }
                }
            }
        }

        //快捷设置哨塔
        public void QuicklySetupTowers()
        {
            foreach (var zone in map.zones.Values)
            {
                zone.ClearAutoTowers();
                //只有有主城的城市，需要设置哨塔
                //关卡、末日实验室，都不需要哨塔
                if (zone.visible > 0 && zone.isGuanqia == 0 && zone.subType == 0)
                {
                    zone.FindJunctionHexagons();
                    zone.AutoSetupTowers();
                    if (zone.IsNeedAdjustment() == true)
                    {
                        map.AddAdjustmentZone(zone);
                    }
                }
            }
        }

        public void CheckEnclave()
        {
            if (CheckMapValid())
            {
                map.CheckEnclave();
            }
        }

        public HexMap GetMap()
        {
            return map;
        }

        public bool CheckWorkSpaceValid()
        {
            if (string.IsNullOrEmpty(WorkSpace) || !WorkSpace.Contains(S_WorkSpaceFlag))
            {
                return false;
            }

            return true;
        }

        public bool CheckConfigPathValid()
        {
            if (string.IsNullOrEmpty(ConfigPath))
            {
                return false;
            }

            if (!Directory.Exists(ConfigPath) || !File.Exists(Path.Combine(ConfigPath, "Src_Index.xlsx")))
            {
                return false;
            }

            return true;
        }

        public bool CheckMapValid()
        {
            if (!CheckWorkSpaceValid())
            {
                EditorUtility.DisplayDialog("地图编辑", "工作空间设置错误!!!", "确定");
                return false;
            }

            if (map == null)
            {
                EditorUtility.DisplayDialog("地图编辑", "请先创建地图!!!", "确定");
                return false;
            }

            return true;
        }

        public static Color GetLandformColor(ZoneLandform landform)
        {
            switch (landform)
            {
                case ZoneLandform.LANDFORM1:
                    return Color.red;
                case ZoneLandform.LANDFORM2:
                    return Color.magenta;
                case ZoneLandform.LANDFORM3:
                    return Color.blue;
                case ZoneLandform.LANDFORM4:
                    return Color.green;
                case ZoneLandform.BLOCK:
                    return Color.cyan;
            }

            return Color.white;
        }

        public static Color GetLandformColor(ZoneLandType landType)
        {
            switch (landType)
            {
                case ZoneLandType.Sea:
                    return Color.cyan;
                case ZoneLandType.Land:
                    return Color.yellow;
            }

            return Color.white;
        }

        private Color GetDrawColor(Zone zone)
        {
            // if (MapTool.Instance.tool == MapToolOP.EditTerritory)
            // 	return GetLandformColor(zone.landType);
            if (zone.level == 0)
            {
                return zone.landType == ZoneLandType.Sea ? Color.black : Color.red;
            }

            return map.GetZoneColor(zone.color);
        }

        #region Gizmos

        private Color[] qtcolros = new[] { Color.red, Color.green, Color.blue };

        private void DrawQT()
        {
            if (!bShowSeaQuadTreeBranch)
                return;
            if (map == null)
                return;
            if (map.SeaHexagonQtArr == null)
                return;

            for (int i = 0; i < map.SeaHexagonQtArr.Length; i++)
            {
                _DrawBranch(map.SeaHexagonQtArr[i].root, qtcolros[i]);
            }
        }

        private List<Hexagon> qt_select_ret_helper;
        private List<QTAABB> cam_aabbs = new List<QTAABB>();

        private void OnDrawGizmos()
        {
            if (map != null && map.SeaHexagonQT != null)
            {
                QTAABB.GetCameraAABBs(mainCamera, cam_aabbs, 3, 5);
                foreach (var aabb in cam_aabbs)
                {
                    _DrawQTAABB(aabb, Color.red);
                }

                if (bShowSeaQuadTreeBranch) _DrawBranch(map.SeaHexagonQT.root, Color.cyan);
                if (bShowSeaQuadTreeLeaf) _DrawQTInFrustumLeavesAABBs();
            }
        }

        private void QT_Select()
        {
            if (map == null)
                return;
            if (map.SeaHexagonQT == null)
                return;
            QTAABB.GetCameraAABBs(mainCamera, cam_aabbs, 3, 5);
            // 概要筛选
            map.SeaHexagonQT.Select(cam_aabbs, qt_select_ret_helper, false);
        }

        private void _DrawBranch(QuadTree<Hexagon>.Branch branch, Color color)
        {
            if (branch == null)
            {
                return;
            }

            if (branch.leaveCount <= 0)
                return;

            Vector2Int indexXY =
                Hex.WorldToOffset(new Vector3(branch.aabb.center.x, 0, branch.aabb.center.y));
            Hexagon hexagon = map.GetHexagon(indexXY);
            // draw this branch
            // _DrawQTAABB(branch.aabb, color);
            // _DrawBoundsXZ(new Bounds(branch.aabb.center, Vector3.one*0.5f), Color.green);
            DrawRect(branch.aabb.top_left, branch.aabb.bottom_right, color);
            DrawHexagon(new Vector3(branch.aabb.center.x, 0, branch.aabb.center.y), color);
            // DrawRound(hexagon, color, branch.depth);

            // draw sub branches
            foreach (var b in branch.branches)
            {
                _DrawBranch(b, color / 1.2f);
            }
        }

        private void _DrawQTInFrustumLeavesAABBs()
        {
            if (map == null)
                return;
            if (map.SeaHexagonQT == null)
                return;
            if (qt_select_ret_helper != null)
            {
                Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
                foreach (var hex in qt_select_ret_helper)
                {
                    bounds.center = hex.Pos;
                    _DrawBoundsXZ(bounds, Color.black);
                }
            }
        }

        private void _DrawLeafsOfBrances(QuadTree<Hexagon>.Branch branch, Color color)
        {
            if (branch == null)
            {
                return;
            }

            foreach (var b in branch.branches)
            {
                if (b == null)
                {
                    continue;
                }

                foreach (var l in b.leaves)
                {
                    _DrawQTAABB(l.aabb, color);
                }

                _DrawLeafsOfBrances(b, color);
            }
        }

        private void _DrawBoundsXZ(Bounds bounds, Color color)
        {
            Gizmos.color = color;

            var min = bounds.min;
            var max = bounds.max;

            var start_pos = min;
            var end_pos = min;
            end_pos.x = max.x;

            Gizmos.DrawLine(start_pos, end_pos);

            start_pos = end_pos;
            end_pos = start_pos;
            end_pos.z = max.z;

            Gizmos.DrawLine(start_pos, end_pos);

            start_pos = end_pos;
            end_pos = start_pos;
            end_pos.x = min.x;

            Gizmos.DrawLine(start_pos, end_pos);

            start_pos = end_pos;
            end_pos = start_pos;
            end_pos.z = min.z;

            Gizmos.DrawLine(start_pos, end_pos);
        }

        private void _DrawQTAABB(QTAABB aabb, Color color)
        {
            Gizmos.color = color;

            var min = aabb.min;
            var max = aabb.max;

            var start_pos = new Vector3(min.x, 0, min.y);
            var end_pos = start_pos;
            end_pos.x = max.x;

            Gizmos.DrawLine(start_pos, end_pos);

            start_pos = end_pos;
            end_pos = start_pos;
            end_pos.z = max.y;

            Gizmos.DrawLine(start_pos, end_pos);

            start_pos = end_pos;
            end_pos = start_pos;
            end_pos.x = min.x;

            Gizmos.DrawLine(start_pos, end_pos);

            start_pos = end_pos;
            end_pos = start_pos;
            end_pos.z = min.y;

            Gizmos.DrawLine(start_pos, end_pos);
        }

        #endregion


        // ================================================= Prefabs =======================================================
        string GetMapObjectsDescPath()
        {
            return Path.Combine(GetMapFolder(), "MapObjectsDesc.json");
        }

        public void ClearMapObjects()
        {
            if (CheckMapValid())
            {
                map.ClearMapObjects();
            }
        }

        public void LoadMapObjects()
        {
            if (CheckMapValid())
            {
                map.LoadMapObjects(GetMapObjectsDescPath());
            }
        }

        public void SaveMapObjects()
        {
            if (CheckMapValid())
                map.SaveMapObjects(GetMapObjectsDescPath());
        }


        // ================================================= Water =======================================================
        public void UpdateWater(Texture2D tex, float mapSizeW, float mapSizeH)
        {
            // waterMaterial.SetTexture("_DistributMap", tex);
            // waterMaterial.SetTextureOffset("_DistributMap", Vector2.zero);
            // waterMaterial.SetTextureScale("_DistributMap", new Vector2(mapSizeW, mapSizeH));
        }
        // ================================================== UI =======================================================

        public void UpdateMapRect(float mapSizeW, float mapSizeH)
        {
            prefabPainterCollider.transform.localPosition =
                new Vector3(mapSizeW / 2, 0, -mapSizeH / 2);
            prefabPainterCollider.transform.localScale = new Vector3(mapSizeW, mapSizeH, 1);

            uiManager.canvas.GetComponent<RectTransform>().sizeDelta =
                new Vector2(mapSizeW, mapSizeH);

            cameraDragArea.width = mapSizeW + 100 * 2;
            cameraDragArea.height = mapSizeH + 100 * 2;
            cameraDragArea.center = new Vector2(mapSizeW / 2, -mapSizeH / 2);
        }

        // ================================================== Notification =======================================================
        public void ShowNotification(string content)
        {
            MapTool.Instance.ShowNotification(new GUIContent(content));
        }

        public void UpdateInteractions()
        {
        }

        // ================================================== SceneGUI =======================================================
        private void OnEditCameraSceneGUI(SceneView sceneView)
        {
            if (!CheckMapValid())
                return;

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Event e = Event.current;

            // 捕捉鼠标点击事件，生成路点
            if (e.shift)
            {
                // 只有在按住Shift的情况下才将路点添加到列表
                if (e.type == EventType.MouseDown && e.button == 0 && map.cameraBoundary.Count <= 10)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (plane.Raycast(ray, out float distance))
                    {
                        previewPoint = Vector3Int.FloorToInt(ray.GetPoint(distance)); // 更新预览点
                        previewPoint.y = 0;
                        map.cameraBoundary.Add(previewPoint); // 添加路点
                        e.Use(); // 阻止事件传播
                    }
                }
            }

            // 绘制路点和连接线
            Handles.color = Color.green;
            for (int i = 0; i < map.cameraBoundary.Count; i++)
            {
                // 为每个路点生成一个可移动的Handles
                Vector3Int newWaypointPosition = Vector3Int.FloorToInt(
                    Handles.PositionHandle(map.cameraBoundary[i], Quaternion.identity));
                if (newWaypointPosition != map.cameraBoundary[i])
                {
                    map.cameraBoundary[i] = newWaypointPosition; // 更新移动后的点]
                    sceneView.Repaint();
                    // SceneView.RepaintAll();
                }

                // 显示序号
                Handles.Label(map.cameraBoundary[i] + Vector3.up * 0.2f + new Vector3(0.2f, 0, 0.2f),
                    i.ToString()); // 显示序号
                Handles.SphereHandleCap(0, map.cameraBoundary[i], Quaternion.identity, 0.3f,
                    EventType.Repaint); // 较小的句柄

                // 连接每个路点
                if (i > 0)
                {
                    Handles.DrawLine(map.cameraBoundary[i - 1], map.cameraBoundary[i]);
                }
            }

            // 绘制当前预览点和与已有点的连接线
            if (e.shift)
            {
                Handles.color = Color.yellow; // 预览点颜色

                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (plane.Raycast(ray, out float distance))
                {
                    previewPoint = Vector3Int.FloorToInt(ray.GetPoint(distance)); // 更新预览点
                    Handles.SphereHandleCap(0, previewPoint, Quaternion.identity, 0.1f,
                        EventType.Repaint);

                    // 连接预览点和已有的路点
                    if (map.cameraBoundary.Count > 1)
                    {
                        Handles.DrawLine(map.cameraBoundary[map.cameraBoundary.Count - 1], previewPoint);
                        Handles.DrawLine(previewPoint, map.cameraBoundary[0]);
                    }
                }
            }
            // 绘制闭合路径
            else if (map.cameraBoundary.Count >= 3)
            {
                Handles.DrawLine(map.cameraBoundary[map.cameraBoundary.Count - 1], map.cameraBoundary[0]);
            }
        }

        private void OnEditFogOfWarSceneGUI(SceneView sceneView)
        {
            if (!CheckMapValid())
                return;

            var fogOfWarData = map.fogOfWarDataMan.GetFogOfWarData(MapTool.Instance._EditFog_ID);
            if (fogOfWarData == null) return;

            var verticesInt = fogOfWarData.FogOfWarMesh.vertices;
            if (verticesInt == null) return;

            // 获取网格变换
            Transform gridTransform = fogOfWarData.FogOfWarGameObject.transform;

            // 1. 先绘制网格线（确保在控制柄下方）
            DrawGridLines(verticesInt, gridTransform);

            EditorGUI.BeginChangeCheck();

            // 2. 绘制球体控制柄
            for (int i = 0; i < verticesInt.Length; i++)
            {
                // 转换为世界坐标
                Vector3 worldPos = gridTransform.TransformPoint(
                    new Vector3(verticesInt[i].x, verticesInt[i].y, verticesInt[i].z));

                float size = HandleUtility.GetHandleSize(worldPos) * handleSize * 0.5f; // 缩小球体尺寸

                // 设置颜色：半透明以便看到下方物体
                Handles.color = (i == verticesInt.Length / 2)
                    ? new Color(1, 1, 0, 0.7f)
                    : // 半透明黄色（中心点）
                    new Color(0, 1, 1, 0.5f); // 半透明青色（普通点）

                // 绘制球体控制柄
                Vector3 newWorldPos = Handles.FreeMoveHandle(
                    worldPos,
                    size,
                    Vector3.zero,
                    Handles.SphereHandleCap);

                // 检测位置变化
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Edit Grid Vertex");

                    // 转换回局部坐标并存储为Vector3Int
                    Vector3 localPos = gridTransform.InverseTransformPoint(newWorldPos);
                    verticesInt[i] = new Vector3Int(
                        Mathf.RoundToInt(localPos.x),
                        0,
                        Mathf.RoundToInt(localPos.z)
                    );

                    fogOfWarData.UpdateMeshFromIntVertices(verticesInt);
                    EditorGUI.BeginChangeCheck();
                }
            }
        }

        // 绘制网格线（不遮挡模型）
        private void DrawGridLines(Vector3[] vertices, Transform gridTransform)
        {
            int gridSize = Mathf.RoundToInt(Mathf.Sqrt(vertices.Length)); // 假设是方形网格

            // 使用更细的线条和较暗颜色
            Handles.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            float lineThickness = 1.5f;

            // 绘制水平线
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize - 1; x++)
                {
                    int index1 = z * gridSize + x;
                    int index2 = z * gridSize + x + 1;

                    Vector3 p1 = gridTransform.TransformPoint(
                        new Vector3(vertices[index1].x, vertices[index1].y, vertices[index1].z));
                    Vector3 p2 = gridTransform.TransformPoint(
                        new Vector3(vertices[index2].x, vertices[index2].y, vertices[index2].z));

                    Handles.DrawLine(p1, p2, lineThickness);
                }
            }

            // 绘制垂直线
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize - 1; z++)
                {
                    int index1 = z * gridSize + x;
                    int index2 = (z + 1) * gridSize + x;

                    Vector3 p1 = gridTransform.TransformPoint(
                        new Vector3(vertices[index1].x, vertices[index1].y, vertices[index1].z));
                    Vector3 p2 = gridTransform.TransformPoint(
                        new Vector3(vertices[index2].x, vertices[index2].y, vertices[index2].z));

                    Handles.DrawLine(p1, p2, lineThickness);
                }
            }
        }
    }
}
#endif