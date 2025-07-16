using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

namespace GEngine.MapEditor
{
    public partial class MapTool
    {
        void DrawEditTool()
        {
            switch (tool)
            {
                case MapToolOP.CreateMap:
                    DrawCreateMapTool();
                    break;
                case MapToolOP.EditTerritory:
                    DrawEditTerritoryTool();
                    break;
                case MapToolOP.EditCity:
                    DrawEditCityTool();
                    break;
                case MapToolOP.EditBlock:
                    DrawEditBlockTool();
                    break;
                case MapToolOP.ExportSetting:
                    DrawExportSettingTool();
                    break;
                case MapToolOP.Export:
                    DrawExportTool();
                    break;
                case MapToolOP.EditInteract:
                    DrawEditInteractTool();
                    break;
                case MapToolOP.EditScatter:
                    DrawEditScatterTool();
                    break;
                case MapToolOP.EditCamera:
                    DrawEditCameraTool();
                    break;
                case MapToolOP.EditFogOfWar:
                    DrawEditFogOfWar();
                    break;
            }
        }

        private void DrawCreateMapTool()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                var envValid = MapRender.instance.CheckWorkSpaceValid();
                // EditorGUILayout.BeginHorizontal();
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUIHelper.Button(
                            envValid ? MapRender.instance.WorkSpace : "设置工作空间",
                            envValid ? Color.green : Color.red, GUILayout.Height(50)))
                    {
                        var folderName =
                            EditorUtility.OpenFolderPanel("设置工作空间", Application.dataPath,
                                "");
                        if (!string.IsNullOrEmpty(folderName))
                        {
                            MapRender.instance.WorkSpace = folderName.Replace("\\", "/");
                            RefreshMap();
                        }
                    }
                }

                // EditorGUILayout.EndHorizontal();
                // EditorGUILayout.BeginHorizontal();
                using (new EditorGUILayout.HorizontalScope())
                {
                    envValid = MapRender.instance.CheckConfigPathValid();
                    EditorGUILayout.LabelField("设置配置表路径", GUILayout.Width(100));
                    if (GUIHelper.Button(
                            envValid ? MapRender.instance.ConfigPath : "设置配置表路径",
                            envValid ? Color.green : Color.red, GUILayout.Height(30),
                            GUILayout.ExpandWidth(true)))
                    {
                        var folderName =
                            EditorUtility.OpenFolderPanel("设置配置表路径", Application.dataPath,
                                "");
                        if (!string.IsNullOrEmpty(folderName))
                        {
                            MapRender.instance.ConfigPath = folderName.Replace("\\", "/");
                            // RefreshMap();
                            var srcIndexExcelPath =
                                Path.Combine(MapRender.instance.ConfigPath,
                                    "Src_Index.xlsx");
                            HexMap.ImportSrcIndexExcel(srcIndexExcelPath);
                            var campaignMapInfoPath = Path.Combine(
                                MapRender.instance.ConfigPath, "Campaign_Map_Info.xlsx");
                            HexMap.ImportCampaignMapInfoExcel(campaignMapInfoPath);
                            var campaignFogInfoPath = Path.Combine(
                                MapRender.instance.ConfigPath, "Campaign_Fog.xlsx");
                            HexMap.ImportCampaignFogExcel(campaignFogInfoPath);
                        }
                    }
                }
                // EditorGUILayout.EndHorizontal();

                // 地图文件
                EditorGUILayout.LabelField($"当前地图：{currentMap}");
                // EditorGUILayout.BeginHorizontal();
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GlobalDef.S_MapEditorType == MapEditorType.EPVE)
                        EditorGUILayout.LabelField(
                            $"共{MapRender.instance.CampaignMapList.Count}个");
                    else
                    {
                        int bigWorldCount = _mapLst.Count(name => name.Contains("BigWorld"));

                        EditorGUILayout.LabelField($"共{bigWorldCount}个");
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("刷新", GUILayout.Width(60)))
                    {
                        RefreshMap();
                    }
                }
                // EditorGUILayout.EndHorizontal();

                // _scrollMaplstPos = EditorGUILayout.BeginScrollView(_scrollMaplstPos, EditorStyles.helpBox);
                using (var scrollViewScope =
                       new EditorGUILayout.ScrollViewScope(_scrollMaplstPos,
                           EditorStyles.helpBox))
                {
                    _scrollMaplstPos = scrollViewScope.scrollPosition;
                    if (GlobalDef.S_MapEditorType == MapEditorType.EPVE)
                    {
                        for (int i = 0; i < MapRender.instance.CampaignMapList.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                var mapInfo = MapRender.instance.CampaignMapList[i];
                                EditorGUILayout.LabelField(mapInfo.Id.ToString(),
                                    GUILayout.Width(60));
                                if (GUILayout.Button(mapInfo.Name))
                                {
                                    OpenPVEMap(i);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _mapLst.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(i.ToString(),
                                    GUILayout.Width(20));
                                if (GUILayout.Button(_mapLst[i]))
                                {
                                    OpenMap(i);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                // EditorGUILayout.EndScrollView();

                if (GlobalDef.S_MapEditorType != MapEditorType.EPVE && GlobalDef.S_MapEditorType != MapEditorType.EBigWorld)
                {
                    // 场景文件
                    GUILayout.Space(30);
                    EditorGUILayout.LabelField($"当前场景：{currentScene}");
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"共{_sceneLst.Count}个");
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("刷新", GUILayout.Width(60)))
                        {
                            RefreshScene();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    // _scrollScenelstPos = EditorGUILayout.BeginScrollView(_scrollScenelstPos, EditorStyles.helpBox);
                    using (var scrollViewScope =
                           new EditorGUILayout.ScrollViewScope(_scrollScenelstPos,
                               EditorStyles.helpBox))
                    {
                        _scrollScenelstPos = scrollViewScope.scrollPosition;
                        for (int i = 0; i < _sceneLst.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(i.ToString(),
                                    GUILayout.Width(20));
                                if (GUILayout.Button(_sceneLst[i]))
                                {
                                    OpenScene(i);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    // EditorGUILayout.EndScrollView();
                }
            }
            // EditorGUILayout.EndScrollView();
            // EditorGUILayout.EndVertical();


            // 新建场景
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            {
                var hexRadius = Hex.HexRadius;
                Hex.HexRadius = EditorGUILayout.FloatField("六边形半径", Hex.HexRadius);
                Hex.CenterUnit = new Vector3(Hex.HexRadius, 0,
                    Hex.HexRadius * Hex.Sqrt3 * 0.5f);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                _mapName = EditorGUILayout.TextField("地图名字", _mapName);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("地图尺寸");
                    _mapsize = EditorGUILayout.Vector2IntField("", _mapsize);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("地图尺寸：（米）");
                    _mapMeterSize = EditorGUILayout.Vector2IntField("", _mapMeterSize);
                }
                EditorGUILayout.EndHorizontal();
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("地图ID");
                    _mapId = EditorGUILayout.IntField("", _mapId);

                }

                if (GUIHelper.Button("新建地图", Color.yellow, GUILayout.Height(40)))
                {
                    if (!CheckEditorValid())
                        return;

                    if (string.IsNullOrEmpty(_mapName))
                    {
                        EditorUtility.DisplayDialog("Error", "名字不能为空", "OK");
                        return;
                    }

                    var lowerName = _mapName.ToLower();
                    if (_mapLst.Contains(lowerName))
                    {
                        EditorUtility.DisplayDialog("Error", "地图名已存在", "OK");
                        return;
                    }

                    if (_mapsize.x == 0 || _mapsize.y == 0)
                    {
                        if (_mapMeterSize.x == 0 || _mapMeterSize.y == 0)
                        {
                            EditorUtility.DisplayDialog("Error", "地图尺寸不能为空", "OK");
                            return;
                        }

                        var hexSide = Hex.HexRadius;
                        var hexW = hexSide * Hex.Sqrt3 / 2f; // 六边格一半宽
                        var hexH = hexSide * 0.5f;
                        _mapsize.x =
                            Mathf.CeilToInt((_mapMeterSize.x - hexW) / (hexW * 2));
                        _mapsize.y =
                            Mathf.CeilToInt((_mapMeterSize.y - hexH) / (hexSide * 1.5f));
                        // mapSizeW = width * hexW * 2 + hexW;
                        // mapSizeH = height * hexSide * 1.5f + hexSide * 0.5f;
                    }
                    currentMap = _mapName;
                    MapRender.instance.CreateMap(_mapsize.x, _mapsize.y, _mapName, _mapId);
                }
            }
            EditorGUILayout.EndVertical();
        }

        void DrawEditTerritoryTool()
        {
            InitAreaScatterSetting();
            EditorGUILayout.BeginHorizontal();
            DrawAreaScatterSetting(areaCenter);
            DrawAreaScatterSetting(area1);
            DrawAreaScatterSetting(area2);
            DrawAreaScatterSetting(area3);
            EditorGUILayout.EndHorizontal();
            if (GUIHelper.Button("划分海域", Color.green))
            {
                if (CheckValid())
                {
                    MapRender.instance.CreateZones(areaCenter,
                        new[] { area1, area2, area3 });
                }
            }

            // if (GUIHelper.Button("根据图片分割区域", Color.green))
            // {
            //     FileHelper.OpenFileDialog(
            //         "选择区域图",
            //         s =>
            //         {
            //             var tex = TextureUtils.LoadByIO(s);
            //             if (CheckValid())
            //             {
            //                 MapRender.instance.CreateZones(tex);
            //             }
            //         },
            //         new[] { "选择区域图", "png,jpg,jpeg,tga", "All files", "*" });
            // }
            var zoneLevelColorExcel = Path
                .Combine(Application.dataPath.Replace("/Assets", ""),
                    "map/BigWorld/Zone/zone_level_color.xlsx").Replace("\\", "/");
            EditorGUILayout.LabelField($"等级配置表路径: {zoneLevelColorExcel}",
                GUILayout.Height(30), GUILayout.ExpandWidth(true));
            if (GUIHelper.Button("根据图片以及等级配置表分割区域", Color.green))
            {
                var colorLevel = FileImportUtils.ImportLevelColor(zoneLevelColorExcel);
                FileHelper.OpenFileDialog(
                    "选择区域图",
                    s =>
                    {
                        var tex = TextureUtils.LoadByIO(s);
                        if (CheckValid())
                        {
                            MapRender.instance.CreateZones(tex, colorLevel);
                        }
                    },
                    new[] { "选择区域图", "png,jpg,jpeg,tga", "All files", "*" });
            }

            EditorGUILayout.Separator();
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("编辑区域");
                DrawBrushSetting();
                EditorGUILayout.HelpBox($"Ctrl + Shift + 鼠标左键 ==> 可锁定海洋陆地",
                    MessageType.Warning);
            }

            EditorGUILayout.Separator();
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("区域合并");
                EditorGUILayout.HelpBox($"合并工具：鼠标左键-(选择基准区域),Shift + 鼠标左键-(选择要合并的区域)",
                    MessageType.Info);
                string cur = _curZone == null ? "未选中" : _curZone.index.ToString();
                string mer = _mergeZone == null ? "未选中" : _mergeZone.index.ToString();
                GUILayout.Label($"当前区域:{cur} ==> {mer}");
            }

            EditorGUILayout.Separator();
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("区域切割");
                EditorGUILayout.HelpBox($"切割工具：Shift + Alt + 鼠标左键", MessageType.Info);
            }

            EditorGUILayout.Space(20);
            if (GUIHelper.Button("空格子检查", Color.yellow, GUILayout.Width(100)))
            {
                if (CheckValid())
                {
                    _map = MapRender.instance.GetMap();
                    _map.SewingHexagon();
                    _map.CalcEdges();
                }
            }

            if (GUIHelper.Button("导出陆地贴图", Color.green, GUILayout.Width(100)))
            {
                MapRender.instance.OnMenu_SaveLandAreaToPicture();
            }

            if (GUIHelper.Button("计算边界", Color.green, GUILayout.Width(100)))
            {
                if (CheckValid())
                {
                    _map = MapRender.instance.GetMap();
                    _map.CalcEdges();
                }
            }

            if (GUIHelper.Button("导出区域框图", Color.green, GUILayout.Width(100)))
            {
                // MapRender.instance.OnMenu_SaveZoneLineToPicture();
                var fileName =
                    EditorUtility.SaveFilePanel("导出区域框图", Application.dataPath, "",
                        "png");
                if (!string.IsNullOrEmpty(fileName))
                {
                    MapRender.instance.SaveSeaLineToPicture(fileName, false);
                }
            }
        }

        void DrawEditCityTool()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("修改城点");

                EditorGUILayout.HelpBox($"Ctrl + 鼠标左键 ==> 移动城点", MessageType.Warning);
            }
        }

        private void DrawEditBlockTool()
        {
            var map = MapRender.instance.GetMap();
            if (GUIHelper.Button("根据图片重置阻挡", Color.green))
            {
                FileHelper.OpenFileDialog(
                    "选择阻挡图",
                    s =>
                    {
                        var tex = TextureUtils.LoadByIO(s);
                        if (CheckValid())
                        {
                            map.ResetBlock(tex);
                        }
                    },
                    new[] { "Image files", "png,jpg,jpeg,tga", "All files", "*" });
            }

            if (GUIHelper.Button("清除阻挡数据", Color.red))
            {
                if (EditorUtility.DisplayDialog("清除阻挡数据", "确认清除掉阻挡图的数据", "OK"))
                {
                    if (CheckValid())
                    {
                        map.ResetBlock();
                    }
                }
            }

            if (GUIHelper.Button("计算阻挡", Color.green))
            {
                if (CheckValid())
                {
                    map.CalStaticBlock();
                }
            }

            EditorGUILayout.Separator();
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("阻挡笔刷");
                EditorGUILayout.HelpBox("无标识 - 根据图片阻挡来 \n阻挡 - 强制阻挡 \n非阻挡 - 强制非阻挡",
                    MessageType.Info);
                _Edit_BlockFlag = (BlockFlag)GUILayout.SelectionGrid((int)_Edit_BlockFlag,
                    GlobalDef.S_BlockFlagDes, 5);
                DrawBrushSetting();
            }
        }

        void DrawExportSettingTool()
        {
            DrawHarborSetting();
            DrawNewbieSetting();
        }

        void DrawExportTool()
        {
            if (GUIHelper.Button("打开导出目录", Color.green, GUILayout.Height(50)))
            {
                if (CheckValid())
                {
                    MapRender.instance.OpenExportFolder();
                }
            }

            if (GUILayout.Button("导出服务器"))
            {
                if (CheckValid())
                {
                    if (MapRender.instance.SaveMap(false))
                        MapRender.instance.ExportToServer();
                }
            }

            if (GUILayout.Button("导出客戶端"))
            {
                if (CheckValid())
                {
                    if (MapRender.instance.SaveMap(false))
                        MapRender.instance.ExportToClient();
                }
            }

            if (GUILayout.Button("导出区域图"))
            {
                if (CheckValid())
                {
                    if (MapRender.instance.SaveMap(false))
                        MapRender.instance.ExportZonePictures();
                }
            }

            if (GUILayout.Button("全部导出"))
            {
                if (CheckValid())
                {
                    if (MapRender.instance.SaveMap(false))
                        MapRender.instance.ExportAll();
                }
            }

            GUILayout.Space(28);

            if (GUILayout.Button("导出航线(服务器)"))
            {
                MapRender.instance.ExportSailLine();
            }

            if (GUILayout.Button("导出城点CSV"))
            {
                MapRender.instance.ExportCityToCSV();
            }
        }

        void DrawEditCameraTool()
        {
            EditorGUILayout.LabelField("Camera Boundary", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox($"Scene视图操作，Scene视图操作 ！！！", MessageType.Info);
            EditorGUILayout.HelpBox($"最多10个点，应该是够用的", MessageType.Info);
            EditorGUILayout.HelpBox($"Shift + 左键点击 添加路点", MessageType.Info);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (CheckValid())
                {
                    var map = MapRender.instance.map;
                    if (GUILayout.Button("清除"))
                    {
                        map.cameraBoundary.Clear();
                    }

                    for (int i = 0; i < map.cameraBoundary.Count; i++)
                    {
                        var point = map.cameraBoundary[i];
                        EditorGUILayout.Vector3Field(i.ToString(), point);
                    }

                    if (GUILayout.Button("复制数据"))
                    {
                        string result = "[" + string.Join(",",
                            map.cameraBoundary.Select(v => $"{v.x}, {v.z} ")) + "]";
                        GUIUtility.systemCopyBuffer = result;
                    }
                }
            }

            return;
            Camera camera = Camera.main;
            if (camera != null)
            {
                EditorGUILayout.LabelField("Camera Position", EditorStyles.boldLabel);
                camera.transform.position =
                    EditorGUILayout.Vector3Field("Position", camera.transform.position);
                camera.transform.rotation = Quaternion.Euler(
                    EditorGUILayout.Vector3Field("Rotation",
                        camera.transform.rotation.eulerAngles));
                camera.fieldOfView =
                    EditorGUILayout.Slider("Field of View", camera.fieldOfView, 1, 179);
                camera.nearClipPlane =
                    EditorGUILayout.FloatField("Near Clipping Plane",
                        camera.nearClipPlane);
                camera.farClipPlane =
                    EditorGUILayout.FloatField("Far Clipping Plane", camera.farClipPlane);

                if (GUILayout.Button("相机归位(调整为查看格子的角度)", GUILayout.Height(40)))
                {
                    camera.transform.rotation = Quaternion.Euler(90, 0, 0);
                }

                if (GUILayout.Button("记录相机参数", GUILayout.Height(40)))
                {
                    if (CheckValid()) MapRender.instance.SaveMap();
                }

                if (GUILayout.Button("恢复相机参数", GUILayout.Height(40)))
                {
                    var cameraSetting = MapRender.instance.GetMap().cameraSetting;
                    camera.transform.rotation = Quaternion.Euler(cameraSetting.rotation.x,
                        cameraSetting.rotation.y, cameraSetting.rotation.z);
                    camera.transform.position = cameraSetting.position;
                    camera.fieldOfView = cameraSetting.fieldOfView;
                    camera.nearClipPlane = cameraSetting.nearClipPlane;
                    camera.farClipPlane = cameraSetting.farClipPlane;
                }
            }
            else
            {
                EditorGUILayout.LabelField("No main camera found.");
            }
        }

        void DrawEditFogOfWar()
        {
            if (!CheckValid())
            {
                return;
            }

            var map = MapRender.instance.GetMap();
            var datas = map.fogOfWarDataMan.GetAllFog();
            EditorGUILayout.LabelField($"迷雾: {datas.Count}", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    // EditorGUILayout.BeginHorizontal();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        var fogOfWarData = datas[i];

                        EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(20));
                        if (GUIHelper.Button($"{fogOfWarData.FogOfWarId}", _EditFog_ID == fogOfWarData.FogOfWarId ? Color.green : GUI.color))
                        {
                            _EditFog_ID = fogOfWarData.FogOfWarId;
                            Selection.activeGameObject = fogOfWarData.FogOfWarGameObject;
                        }
                        fogOfWarData.FogColor = EditorGUILayout.ColorField(fogOfWarData.FogColor, GUILayout.Width(100));
                        if(GUIHelper.Button("X", Color.red, GUILayout.Width(20)))
                        {
                            MapRender.instance.RemoveCampaignFogInfo(MapRender.instance.CurrentMapId, fogOfWarData.FogOfWarId);
                            map.RemoveFogOfWarData(fogOfWarData.FogOfWarId);
                            continue;
                        }
                    }
                    // EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("添加迷雾数据"))
                {
                    _EditFog_ID = map.AddFogOfWarData();
                    CampaignFogInfo fogInfo = new CampaignFogInfo
                    {
                        Id = -1,
                        MapId = MapRender.instance.CurrentMapId,
                        FogId = _EditFog_ID,
                        FogPos = Vector3.zero,
                        Locked = "",
                        BubbleCon = string.Empty,
                        BubbleCoor = string.Empty,
                        UnlockCon = string.Empty,
                        TipInfo = string.Empty
                    };
                    MapRender.instance.AddCampaignFogInfo(fogInfo);
                }
            }

            DrawBrushSetting();
            if (GUIHelper.Button("复制数据", Color.green))
            {
                if (CheckValid())
                {
                    var data = map.fogOfWarDataMan.GetAllFog();
                    StringBuilder builder = new StringBuilder();
                    foreach (var fogData in data)
                    {
                        var transform = fogData.FogOfWarGameObject.transform;
                        builder.AppendLine($"{fogData.FogOfWarId}");
                        builder.AppendLine($"[{transform.localPosition.x}, 0, {transform.localPosition.z}]");
                        builder.Append("[");
                        for (int i = 0; i < fogData.FogOfWarMesh.vertices.Length; i++)
                        {
                            Vector3 pos = fogData.FogOfWarMesh.vertices[i];
                            if (i > 0)
                            {
                                builder.Append(", ");
                            }
                            builder.Append($"{pos.x}, {pos.z}");
                        }
                        builder.AppendLine("]");
                    }
                    GUIUtility.systemCopyBuffer = builder.ToString();
                }
            }
        }

        private int searchId = 0;
        private string curPointFileName = string.Empty;

        void DrawEditInteractTool()
        {
            GUILayout.Label("笔刷");
            EditorGUILayout.HelpBox($"笔刷工具：点击交互点的绘制阻挡Control + 鼠标左键, Control + 鼠标右键删除阻挡",
                MessageType.Info);
            // _Tool_BrushRound = EditorGUILayout.IntSlider("笔刷大小", _Tool_BrushRound, 1, 50);
            List<InteractionPoint> interactionPoints =
                MapRender.instance.CurrentMapInteractionPoints;
            if (interactionPoints == null || interactionPoints.Count == 0)
            {
                EditorGUILayout.HelpBox("当前地图没有交互点数据，请先添加交互点", MessageType.Warning);
            }
            else
            {
                int totalItems = interactionPoints.Count;
                int totalPages = Mathf.CeilToInt((float)totalItems / itemsPerPage);
                MapRender.instance.SelectInteractBlockId =
                    EditorGUILayout.IntField("编辑阻挡的交互物ID：",
                        MapRender.instance.SelectInteractBlockId);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(100)) && currentPage > 0)
                {
                    currentPage--;
                }

                GUILayout.Label($"{currentPage + 1}/{totalPages}", GUILayout.Width(200));
                if (GUILayout.Button("+", GUILayout.Width(100)) &&
                    currentPage < totalPages - 1)
                {
                    currentPage++;
                }

                // 搜索交互点ID
                searchId = EditorGUILayout.IntField("输入交互点ID进行查找", searchId);
                if (GUILayout.Button("搜索"))
                {
                    // 查找交互点在列表中的索引
                    int index = 0;
                    for (int i = 0; i < interactionPoints.Count; i++)
                    {
                        if (interactionPoints[i].id == searchId)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index == -1)
                    {
                        Debug.LogWarning("未找到该ID的交互点");
                        return;
                    }

                    int page = index / itemsPerPage;
                    currentPage = page; // 设置当前页码

                    totalPages =
                        Mathf.CeilToInt((float)interactionPoints.Count / itemsPerPage);

                    Debug.Log($"交互点 ID {searchId} 位于第 {page + 1} 页");
                }

                EditorGUILayout.EndHorizontal();

                int startIndex = currentPage * itemsPerPage;
                int endIndex = Mathf.Min(startIndex + itemsPerPage, totalItems);

                foreach (var point in interactionPoints.GetRange(startIndex,
                             endIndex - startIndex))
                {
                    bool foldoutOpen =
                        EditorPrefs.GetBool($"InteractionPointFoldout_{point.id}",
                            false); // 默认折叠打开

                    foldoutOpen =
                        EditorGUILayout.Foldout(foldoutOpen, $"交互点 {point.id} 设置");

                    EditorPrefs.SetBool($"InteractionPointFoldout_{point.id}",
                        foldoutOpen);

                    if (foldoutOpen)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUI.BeginChangeCheck();
                        int newId = EditorGUILayout.IntField("Event ID", point.id);

                        Vector2Int newPosition =
                            EditorGUILayout.Vector2IntField("网格坐标", point.position);
                        Vector3 newRotation =
                            EditorGUILayout.Vector3Field("旋转角度", point.rotation);
                        Vector3 newScale =
                            EditorGUILayout.Vector3Field("缩放", point.scale);
                        InteractionType newType =
                            (InteractionType)EditorGUILayout.EnumPopup("Type",
                                point.type);

                        // 如果有变化，更新交互点
                        // if (newId != point.id || newPosition != point.position ||
                        //     newRotation != point.rotation || newType != point.type ||
                        //     newScale != point.scale)
                        if (EditorGUI.EndChangeCheck())
                        {
                            point.id = newId;
                            point.position = newPosition;
                            point.rotation = newRotation;
                            point.scale = newScale;
                            point.type = newType;
                            MapEditorEventCenter.SendEvent(
                                MapEditorEvent.InteractionEditorUpdateEvent,
                                point);
                        }

                        GameObject newPrefab = (GameObject)EditorGUILayout.ObjectField(
                            "Prefab",
                            point.prefab, typeof(GameObject), false);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("资源路径", GUILayout.Width(80));
                        point.prefabPath = EditorGUILayout.TextField(point.prefabPath);
                        EditorGUILayout.EndHorizontal();

                        if (newPrefab != point.prefab)
                        {
                            point.prefab = newPrefab;
                            if (point.prefab != null)
                            {
                                MapEditorEventCenter.SendEvent(
                                    MapEditorEvent.InteractionAddEvent, point.id);
                            }
                        }

                        // EditorGUILayout.BeginHorizontal();
                        using(new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("删除", GUILayout.Width(60)))
                            {
                                MapEditorEventCenter.SendEvent(
                                    MapEditorEvent.InteractionRemoveEvent,
                                    point.id);
                                interactionPoints.Remove(point);
                            }

                            if (GUILayout.Button("定位", GUILayout.Width(60)))
                            {
                                if (point.prefab != null)
                                {
                                    var mainCamera = Camera.main;
                                    if (mainCamera != null)
                                    {
                                        var targetPosition =
                                            Hex.OffsetToWorld(point.position);
                                        Ray centerRay =
                                            mainCamera.ViewportPointToRay(new Vector3(
                                                0.5f,
                                                0.5f, 0f));
                                        float targetDistance = Vector3.Distance(
                                            targetPosition,
                                            mainCamera.transform.position);
                                        Vector3 newCameraPosition =
                                            targetPosition - centerRay.direction *
                                            targetDistance;
                                        newCameraPosition.y = 30;
                                        mainCamera.transform.position = newCameraPosition;
                                        Selection.activeGameObject =
                                            point.prefab.gameObject;
                                        MapEditorEventCenter.SendEvent(
                                            MapEditorEvent.InteractionFocusEvent, point.id);
                                    }
                                }
                            }

                            if (GUILayout.Button("绘制阻挡", GUILayout.Width(60)))
                            {
                                MapRender.instance.SelectInteractBlockId = point.id;
                            }
                        }
                        // EditorGUILayout.EndHorizontal();
                        var blockPoints = MapRender.instance.GetBlockPoints(point.id);
                        // if (MapRender.instance.CurrentMapInteractionPoints.TryGetValue(point.id,
                                // out. var blockPoints))
                        if (blockPoints != null)
                        {
                            // EditorGUILayout.BeginHorizontal();
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField("阻挡点数量",
                                    blockPoints.Count.ToString());
                                if (GUILayout.Button("清除阻挡", GUILayout.Width(80)))
                                {
                                    MapRender.instance.ClearInteractionBlocks(point.id);
                                }

                                // EditorGUILayout.BeginVertical();
                                using (new EditorGUILayout.VerticalScope())
                                    foreach (var points in blockPoints)
                                    {
                                        // EditorGUILayout.BeginHorizontal();
                                        using (new EditorGUILayout.HorizontalScope())
                                        {
                                            EditorGUILayout.LabelField($"阻挡区域 {points.x},{points.y}");
                                            if (GUILayout.Button("删除", GUILayout.Width(60)))
                                            {
                                                blockPoints.Remove(points);
                                            }
                                        }
                                        // EditorGUILayout.EndHorizontal();
                                    }

                            }

                            // EditorGUILayout.EndVertical();
                            // EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.EndVertical();
                    }
                }
            }

            // 新增交互物按钮
            if (GUILayout.Button("新增交互物", GUILayout.Height(30)))
            {
                interactionPoints.Add(new InteractionPoint()
                    { index = MapRender.instance.InteractionIndex });
            }

            // 同步列表按钮
            if (GUILayout.Button("同步列表", GUILayout.Height(30)))
            {
                MapEditorEventCenter.SendEvent(MapEditorEvent.InteractionSyncEvent);
            }

            // 导出Excel按钮
            if (GUILayout.Button("导出Excel", GUILayout.Height(30)))
            {
                string fileName = string.IsNullOrEmpty(curPointFileName)
                    ? "Campaign_Points.xlsx"
                    : curPointFileName + ".xlsx";
                string filePath = EditorUtility.SaveFilePanel("Save Interaction Points",
                    "",
                    fileName, "xlsx");
                if (!string.IsNullOrEmpty(filePath))
                {
                    MapRender.instance.map.ExportAllInteractionExcel(filePath);
                }
            }

            // 导入Excel按钮
            if (GUILayout.Button("导入Excel", GUILayout.Height(30)))
            {
                string filePath =
                    EditorUtility.OpenFilePanel("Open Interaction Points", "", "xlsx");
                if (!string.IsNullOrEmpty(filePath))
                {
                    // 读取Excel文件
                    MapRender.instance.map.ImportAllInteractionExcel(filePath);
                    // 把Excel文件名字保存起来
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    curPointFileName = fileName;
                }
            }
        }

        private bool _isUseEditorTool = true;
        private string _scatterPath = "Assets/GameAssets/Scenes";
        private GameObject _scatterGO;

        void DrawEditScatterTool()
        {
            // MapRender.instance.prefabPainter.DeActive();
            // _isUseEditorTool = EditorGUILayout.Toggle("是否使用撒点工具", _isUseEditorTool);
            // if (!_isUseEditorTool)
            // {
            EditorGUILayout.BeginVertical();
            _scatterGO = (GameObject)EditorGUILayout.ObjectField("自定义物体", _scatterGO,
                typeof(GameObject), false);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("导出路径:", _scatterPath);
            if (GUILayout.Button("选择路径", GUILayout.Width(100)))
            {
                _scatterPath = EditorUtility.SaveFolderPanel("保存路径：", _scatterPath, "");
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            // }
            // else
            // {
            //     EditorGUILayout.BeginVertical();
            //     MapRender.instance.prefabPainter.Active();
            //     MapRender.instance.prefabPainter.OnPrefabPainterGUI();
            //     EditorGUILayout.EndVertical();
            // }
            if (GUILayout.Button("导出", GUILayout.Height(40)))
            {
                if (CheckValid() && _scatterGO != null)
                {
                    if (!string.IsNullOrEmpty(_scatterPath))
                    {
                        string path =
                            _scatterPath.Replace(Application.dataPath, "Assets");
                        string dataPath = Path.Combine(path, "data");
                        string prefabPath = Path.Combine(path, "prefabs");
                        if (!Directory.Exists(dataPath))
                        {
                            Directory.CreateDirectory(dataPath);
                        }

                        if (!Directory.Exists(prefabPath))
                        {
                            Directory.CreateDirectory(prefabPath);
                        }

                        MapRender.instance.ExportMapObjects(_scatterGO.transform,
                            dataPath, prefabPath, true);
                    }
                }
            }

            if (GUILayout.Button("预览", GUILayout.Height(40)))
            {
                string path = _scatterPath.Replace(Application.dataPath, "Assets");
                string prefabPath =
                    Path.Combine(path, "prefabs/WorldResRenderData.prefab");
                if (File.Exists(prefabPath))
                {
                    // load res render data
                    ResRenderData resRenderData =
                        AssetDatabase.LoadAssetAtPath<ResRenderData>(prefabPath);
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
                }
            }
        }
    }
}
#endif