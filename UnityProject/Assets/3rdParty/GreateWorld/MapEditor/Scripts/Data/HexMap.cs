#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using GEgineRunTime;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Sirenix.OdinInspector;
using TEngine;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GEngine.MapEditor
{
    public enum MapType
    {
        [Description("大地图")] EGreatWorld = 0,
        [Description("小地图")] ESmallWorld = 1
    }

    public struct SeekPos
    {
        public Vector2Int pos;
        public int posType;
        public int lv;
    }

    public struct ZoneLineInfo
    {
        //开始六边形的索引
        public int begin;

        //长度
        public short len;

        //对应的zone index
        public int idx;
    }

    public class ZoneLvInfo
    {
        public List<Zone> zones = new();
        public int minHex = -1;
        public int maxHex = -1;

        public void Reset()
        {
            zones.Clear();
            minHex = -1;
            maxHex = -1;
        }

        public bool Add(Zone zone)
        {
            if (zone == null || zones.Contains(zone))
                return false;

            var hexCount = zone.hexagons.Count;
            if (minHex < 0 || hexCount < minHex)
                minHex = hexCount;
            if (maxHex < 0 || hexCount > maxHex)
                maxHex = hexCount;

            zones.Add(zone);
            return true;
        }

        public bool Del(Zone zone)
        {
            if (zone == null || !zones.Contains(zone))
                return false;

            zones.Remove(zone);
            minHex = -1;
            maxHex = -1;
            foreach (var t in zones)
            {
                var hexCount = t.hexagons.Count;
                if (minHex < 0 || hexCount < minHex)
                    minHex = hexCount;
                if (maxHex < 0 || hexCount > maxHex)
                    maxHex = hexCount;
            }

            return true;
        }
    }

    public struct CameraSetting
    {
        public Vector3 position;
        public Vector3 rotation;
        public float fieldOfView;
        public float nearClipPlane;
        public float farClipPlane;
    }


    public class HexMap
    {
        public enum MapProgress
        {
            None,
            Create,
            LandZones,
            SeaZones
        }

        public string Name;
        public string FileName;

        public string WaterDistributeTexPath =>
            Path.Combine(GlobalDef.S_RuntimeWorkSpace, Name, "Tex",
                "water_distribute.png");

        public string RuntimeMapPath
        {
            get
            {
                var path = Path.Combine(GlobalDef.S_RuntimeWorkSpace, Name);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public string RuntimeMapPrefabPath
        {
            get
            {
                var path = Path.Combine(RuntimeMapPath, "Prefab");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public string RuntimeMapDataPath
        {
            get
            {
                var path = Path.Combine(RuntimeMapPath, "Data");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        private Texture2D _waterDistributeDefault;

        public Texture2D WaterDistributeDefault
        {
            get
            {
                if (_waterDistributeDefault == null)
                    _waterDistributeDefault =
                        AssetDatabase.LoadAssetAtPath<Texture2D>(
                            "Assets/_Test/WorldSceneTest/water_distribute.png");

                return _waterDistributeDefault;
            }
        }

        private int ver = 1;
        private int exportVer = 1;
        public MapProgress mapProgress = MapProgress.None;
        public int width = 1;

        public int height = 1;


        //地图类型 标识该地图的地图类型
        public MapType mapType = MapType.EGreatWorld;

        //小地图类型开始六边形坐标
        public Vector2Int startHexPos = Vector2Int.zero;

        //小地图类型结束六边形坐标 默认151x151大小
        public Vector2Int endHexPos = new(150, 150);
        // public float hexSide = Hex.HexRadius;
        public float hexW;
        public float hexH;

        private Mesh _hexMesh;

        public Mesh hexMesh
        {
            get
            {
                if (_hexMesh == null) GenerateMesh();

                return _hexMesh;
            }
        }

        public float mapSizeW;
        public float mapSizeH;

        // 相机滑动区域
        public List<Vector3Int> cameraBoundary = new List<Vector3Int>(10);


        public QuadTree<Hexagon> SeaHexagonQT;
        public QuadTree<Hexagon>[] SeaHexagonQtArr;

        public Vector2Int mapPoints = Vector2Int.one;
        private Vector2Int[,] cityPoints;
        private int[,] coordZones;

        public SeekInfo seek1;
        public SeekInfo seek2;
        public SeekInfo seek3;

        public int BlockCount { get; private set; }
        public int minZoneHexCount { get; private set; }
        public int maxZoneHexCount { get; private set; }

        public Dictionary<int, RuinData> ruinDatas = new();
        private readonly Dictionary<int, List<Zone>> ruinZones = new();

        // 城点信息
        public ZoneLvInfo[] zoneLvInfos { get; } =
            new ZoneLvInfo[Zone.S_MAX_ZoneLV + 1];

        // 关卡信息
        public ZoneLvInfo[] guanqiaLvInfos { get; } =
            new ZoneLvInfo[Zone.S_MAX_ZoneLV + 1];

        public Hexagon[] hexagons;
        public Dictionary<int, Zone> zones = new();
        private readonly Dictionary<int, Zone> landzones = new();
        private readonly Dictionary<int, Zone> seazones = new();

        private List<SeekPos> lv1Seeks = new();
        private List<SeekPos> lv2Seeks = new();
        private List<SeekPos> lv3Seeks = new();

        private readonly List<Zone> delZones = new();
        private readonly List<Zone> expandZones = new();

        private Color[] zoneColors;
        private Color[] frameColors;

        public Texture2D blockData;
        private Zone centerZone;
        private Color[] blockColor;

        private byte[] blockBuff = null;
        private int[] blockAnalyze; //0: 还没解析 1: 阻挡 2:非阻挡

        //城市等级与哨塔的对应关系
        public int[] cityLevel2TowerArray = { 0, 4, 4, 6, 6, 6, 8, 8 };

        //航道点
        public Dictionary<int, HashSet<int>> connect;

        //岛屿占地&放置模板
        public Dictionary<int, IslandTemplate> islandTemplates;

        /// <summary>
        ///     商圈信息
        /// </summary>
        public Dictionary<int, Business> businesses = new();

        // <(商圈, 商圈), <(zone, zone)>>
        public Dictionary<(int, int), Dictionary<(int, int), List<Vector3>>>
            businessZoneEdgeWayPoints = new();

        public Dictionary<(int, int), List<Vector3>> businessEdgeWayPoints = new();

        public Dictionary<(int, int), Mesh> businessEdgeMeshes = new();

        // 商圈边线抽稀阈值
        public float businessEdgeTolerance = 2f;
        public float businessEdgeWidth = 2f;

        //
        private bool needResetMovetypeMark = true;

        public class Fishery
        {
            public int fishIndex;
            public List<int> seatIndexList = new();
        }

        //当前正在编辑的渔场HexIndex
        private int curFisheryHexIndex = -1;

        public Dictionary<Hexagon, Fishery> fisheryDic = new();

        //设为鱼群的格子列表
        public List<Hexagon> fishHexgaons = new();

        //导出阻挡信息 | 港口停靠点 的时候判断地格是否属于海港占地 海港占地2~3圈(3级以下区域2圈  3级及以上3圈)
        public bool CheckPortRange;

        //2级及以下港口占领圈数
        public int Level2BelowCircles = 3;

        //3级及以上港口占地圈数
        public int Level2BeyondCircles = 4;

        //导出港口停泊点设置
        //2级及以下港口的 vip停播数量
        public int VipCount4TwoCircle = 3;

        //2级及以下港口的停靠半径
        public int CallRadius4TwoCircle = 6;

        //3级及以上港口的 vip停播数量
        public int VipCount4ThreeCircle = 5;

        //3级及以上港口的停靠半径
        public int CallRadius4ThreeCircle = 8;

        //新手地图区域添加值
        public int ExportNewbieMapZoneMargin = 0;

        //宝藏点
        public int Treasure2PortDis = 12;

        //宝藏点间隔随机区间
        public int TreasureInterval = 6;

        //每个海域最多几个
        public int TreasureMaxPerPort = 2;

        public CameraSetting cameraSetting = new()
        {
            position = new Vector3(0, 0, 0),
            rotation = new Vector3(90, 0, 0),
            fieldOfView = 60,
            nearClipPlane = 0.3f,
            farClipPlane = 2000
        };

        public FogOfWarDataMan fogOfWarDataMan = new FogOfWarDataMan();
        //================================ 属性分割线 ========================================

        public void AddFishery(Hexagon hexagon, Fishery fishery)
        {
            if (!fisheryDic.ContainsKey(hexagon)) fisheryDic[hexagon] = fishery;
        }

        public void RemoveFishery(Hexagon hexagon)
        {
            fisheryDic.Remove(hexagon);
        }

        public bool ToggleFish(Hexagon hex)
        {
            //已经存在该渔场
            if (fisheryDic.ContainsKey(hex) && hex.fishType == FishType.Fish)
            {
                //移除渔场数据，包括鱼群，垂钓点
                hex.fishType = FishType.None;
                hex.blockFlag = BlockFlag.None;
                //重置垂钓点格子数据
                if (fisheryDic[hex].seatIndexList != null &&
                    fisheryDic[hex].seatIndexList.Count > 0)
                    for (var i = 0; i < fisheryDic[hex].seatIndexList.Count; i++)
                    {
                        var seatHex = GetHexagon(fisheryDic[hex].seatIndexList[i]);
                        seatHex.fishType = FishType.None;
                        seatHex.blockFlag = BlockFlag.None;
                    }

                RemoveFishery(hex);
                SetCurFisheryIndex(-1);
                fishHexgaons.Remove(hex);
            }
            //如果不存在创建渔场对象，并且该格子不是垂钓点，加入
            else
            {
                if (hex.fishType != FishType.Seat)
                {
                    var fishery = new Fishery();
                    fishery.fishIndex = hex.index;
                    hex.fishType = FishType.Fish;
                    hex.blockFlag = BlockFlag.Block;
                    AddFishery(hex, fishery);
                    if (fishHexgaons.Contains(hex) == false) fishHexgaons.Add(hex);

                    SetCurFisheryIndex(fishery.fishIndex);
                }
            }

            return true;
        }

        public bool ToggleSeat(Hexagon hex)
        {
            if (curFisheryHexIndex == -1) return false;

            var fisheryHex = GetHexagon(curFisheryHexIndex);
            //渔场存在,并且当前选中的格子不是鱼群
            if (fisheryDic.ContainsKey(fisheryHex) && hex.fishType != FishType.Fish)
            {
                if (fisheryDic[fisheryHex].seatIndexList.Contains(hex.index))
                {
                    hex.fishType = FishType.None;
                    hex.blockFlag = BlockFlag.None;
                    fisheryDic[fisheryHex].seatIndexList.Remove(hex.index);
                }
                else
                {
                    hex.fishType = FishType.Seat;
                    hex.blockFlag = BlockFlag.Block;
                    fisheryDic[fisheryHex].seatIndexList.Add(hex.index);
                }
            }

            return true;
        }

        public int GetCurFisheryIndex()
        {
            return curFisheryHexIndex;
        }

        public void SetCurFisheryIndex(int hexIndex)
        {
            curFisheryHexIndex = hexIndex;
        }

        public HexMap()
        {
            for (var i = 0; i <= Zone.S_MAX_ZoneLV; i++)
            {
                zoneLvInfos[i] = new ZoneLvInfo();
                guanqiaLvInfos[i] = new ZoneLvInfo();
            }

            connect = new Dictionary<int, HashSet<int>>();
            islandTemplates = new Dictionary<int, IslandTemplate>();
        }

        public static void ResetFind()
        {
            var map = MapRender.instance.GetMap();
            if (map != null)
                map.ResetHexFind();
        }

        public void ResetHexFind()
        {
            for (var i = 0; i < hexagons.Length; i++)
                hexagons[i].Dirty = false;
        }

        // public void CreateEmpty(Texture2D block, int w, int h)
        // {
        // 	CreateBase(w, h);
        // 	zones.Clear();
        // 	landzones.Clear();
        // 	seazones.Clear();
        // 	mapProgress = MapProgress.Create;
        // }

        public void Create(int w, int h, Action<string, float> onProgress = null)
        {
            onProgress?.Invoke($"尺寸 {w} x {h}", 0f);
            CreateBase(w, h);
            PostprocessData();
            mapProgress = MapProgress.Create;
        }

        public void CreateBase(int w, int h)
        {
            width = w;
            height = h;

            // var hexSide = 1f / Mathf.Cos(Mathf.Deg2Rad * 30f);
            var hexSide = Hex.HexRadius;
            hexW = hexSide * Hex.Sqrt3 / 2f;        // 六边格一半宽
            hexH = hexSide * 0.5f;

            mapSizeW = width * hexW * 2 + hexW;
            mapSizeH = height * hexSide * 1.5f + hexSide * 0.5f;

            Debug.Log($"hexSide = {hexSide}, hexW = {hexW}, hexH = {hexH}, sizeW = {mapSizeW}, sizeH = {mapSizeH}");


            InitColors();

            blockAnalyze = new int[w * h];
            hexagons = new Hexagon[w * h];
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var idx = y * width + x;
                    hexagons[idx] = new Hexagon(this, idx, x, y);
                }

            CreateNeigbours();
        }

        public void IdentifyTex(Texture2D tex, Action<string, float> onProgress = null)
        {
            if (tex == null)
                return;
            TextureUtils.CheckTexReadable(tex);
            var colors = tex.GetPixels32();

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var idx = y * width + x;
                onProgress?.Invoke("calc zone ... ", 1f * idx / hexagons.Length);

                var pos = Hex.OffsetToWorld(x, y);
                var pixelpos = new Vector2Int(
                    Mathf.RoundToInt(pos.x / mapSizeW * (tex.width - 1)), Mathf.RoundToInt(
                        -pos.z / mapSizeH *
                        (tex.height - 1)));

                var hexagon = GetHexagon(x, y);
                // Color src = tex.GetPixel(x, y);
                // Color32 c = colors[idx] - Color.red;
                // float sqrt = Mathf.Sqrt(c.r * c.r + c.g * c.g + c.b * c.b);
                // 上下翻转才和视觉一样
                var pixelidx = pixelpos.x + (tex.height - 1 - pixelpos.y) * tex.width;
                hexagon.movetype = colors[pixelidx].r > 0.5
                    ? MOVETYPE.DISABLE
                    : GlobalDef.S_DefaultMoveType;
            }
        }

        public void CreateZones(AreaScatterSetting centerSetting,
            AreaScatterSetting[] settings,
            Action<string, float> onProgress = null)
        {
            // 重置
            var keylist = new List<int>(zones.Keys);
            foreach (var key in keylist)
            {
                zones[key].Destroy();
                zones[key] = null;
            }

            zones.Clear();


            foreach (var hexagon in hexagons) hexagon.Reset();

            CreateCenterZone(centerSetting, onProgress);
            CreateSeaZones(settings, onProgress);
        }

        public void CreateLandZones(Action<string, float> onProgress = null)
        {
            if (mapProgress >= MapProgress.LandZones)
            {
                var keylist = new List<int>(landzones.Keys);
                foreach (var key in keylist)
                {
                    landzones[key].Destroy();
                    landzones[key] = null;
                }

                landzones.Clear();

                keylist = new List<int>(seazones.Keys);
                foreach (var key in keylist)
                {
                    seazones[key].Destroy();
                    seazones[key] = null;
                }

                seazones.Clear();
            }

            var landzoneidx = 0;
            int landzoneColor;
            var neigbourZoneIdx = new List<int>(6);

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var idx = y * width + x;
                onProgress?.Invoke("calc zone ... ", 1f * idx / hexagons.Length);

                var hexagon = GetHexagon(x, y);

                if (hexagon.movetype == MOVETYPE.DISABLE)
                {
                    // 是陆地
                    // 看邻居中有没有陆地的
                    //		没有
                    //			自己创建一个zone
                    //		有
                    //			必然有zone
                    //				加入	并且如果有多个zone 合并
                    //
                    neigbourZoneIdx.Clear();
                    foreach (var neigbour in hexagon.neigbours)
                        if (neigbour != null && neigbour.zone != null)
                            if (!neigbourZoneIdx.Contains(neigbour.zone.index))
                                neigbourZoneIdx.Add(neigbour.zone.index);

                    if (neigbourZoneIdx.Count <= 0)
                    {
                        landzoneColor = 1; //是个黄色
                        var zone = new Zone(this, landzoneidx, hexagon, 0, 0, landzoneColor);
                        zone.landType = ZoneLandType.Land;
                        landzones.Add(zone.index, zone);
                        landzoneidx++;
                    }
                    else
                    {
                        var zone = landzones[neigbourZoneIdx[0]];
                        zone.AddHexagon(hexagon, true);
                        for (var i = neigbourZoneIdx.Count - 1; i > 0; i--)
                        {
                            foreach (var h in landzones[neigbourZoneIdx[i]].hexagons)
                                zone.AddHexagon(h, true);

                            landzones.Remove(neigbourZoneIdx[i]);
                        }
                    }
                }
            }

            Debug.LogFormat("Create Land Zone : {0}", landzones.Count);

            var toDels = new List<int>();
            foreach (var landzone in landzones)
                if (landzone.Value.hexagons.Count < 10)
                    toDels.Add(landzone.Key);

            for (var i = 0; i < toDels.Count; i++)
            {
                landzones[toDels[i]].Destroy();
                landzones.Remove(toDels[i]);
            }

            DealLandZones();
            CalcEdges(onProgress);
            mapProgress = MapProgress.LandZones;
        }

        private void DealLandZones()
        {
            var zonecount = 1;
            foreach (var zone in landzones)
            {
                zone.Value.index = zonecount;
                zones.Add(zonecount, zone.Value);
                zonecount++;
            }
        }

        private void DealSeaZones()
        {
            var zonecount = 100;
            foreach (var zone in seazones)
            {
                zone.Value.index = zonecount;
                zones.Add(zonecount, zone.Value);
                zonecount++;
            }
        }

        public void CreateCenterZone(AreaScatterSetting setting,
            Action<string, float> onProgress = null)
        {
            if (setting.count <= 0) return;

            // 王座 999
            // 陆地区域 1-99
            // 海洋区域 101-
            var centerZoneIndex = 999;
            var hexCenterIdx = new Vector2Int(width / 2, height / 2);
            var hexCenter = GetHexagon(hexCenterIdx);
            centerZone = new Zone(this, centerZoneIndex, hexCenter, setting.postype, setting.level,
                centerZoneIndex);
            zones.Add(centerZone.index, centerZone);
            for (var i = 0; i < setting.expandCount; i++) centerZone.Expand();


            // RectInt rect = new RectInt(width / 2 - setting.width / 2,
            // 	height / 2 - setting.height / 2,
            // 	setting.width, setting.height);
            //
            // for (int col = rect.xMin; col <= rect.xMax; col++)
            // {
            // 	for (int row = rect.yMin; row <= rect.yMax; row++)
            // 	{
            // 		var hexagon = GetHexagon(col, row);
            // 		centerZone.AddHexagon(hexagon, true);
            // 	}
            // }
        }

        public void CreateSeaZones(AreaScatterSetting[] settings,
            Action<string, float> onProgress = null)
        {
            // CreateLandZones(onProgress);

            if (mapProgress >= MapProgress.SeaZones)
            {
                var keylist = new List<int>(seazones.Keys);
                foreach (var key in keylist)
                {
                    seazones[key].Destroy();
                    seazones[key] = null;
                }

                seazones.Clear();

                DealLandZones();
            }


            // 1内圈 -> 2中圈 -> 3外圈
            // 指定区域分四叉树
            if (SeaHexagonQtArr == null)
                SeaHexagonQtArr = new QuadTree<Hexagon>[settings.Length];
            for (var i = 0; i < settings.Length; i++)
            {
                if (settings[i].count > 0)
                {
                    settings[i].rect = new Rect(width / 2 - settings[i].width / 2,
                        height / 2 - settings[i].height / 2,
                        settings[i].width, settings[i].height);
                }
                else
                {
                    settings[i].rect = new Rect(width / 2 - settings[i].width / 2,
                        height / 2 - settings[i].height / 2,
                        0, 0);
                }

                var needDepth = GetQuadTreeNeedDepth(settings[i].count);
                var wh = Hex.GetBounds(settings[i].width, settings[i].height);
                var bounds = new Bounds(new Vector3(mapSizeW / 2, 0, -mapSizeH / 2),
                    new Vector3(wh.x, 0, wh.y));

                if (SeaHexagonQtArr[i] != null)
                    SeaHexagonQtArr[i].Clear();
                SeaHexagonQtArr[i] = new QuadTree<Hexagon>(bounds);
            }


            AreaScatterSetting setting;
            var idx = 0;
            foreach (var hexagon in hexagons)
            {
                onProgress?.Invoke("Cal Quad Tree ... ", 1f * idx / hexagons.Length);

                for (var i = 0; i < settings.Length; i++)
                {
                    setting = settings[i];
                    if (hexagon.movetype != MOVETYPE.DISABLE && hexagon.zone == null)
                        if (hexagon.x >= setting.rect.xMin && hexagon.x <= setting.rect.xMax &&
                            hexagon.y >= setting.rect.yMin && hexagon.y <= setting.rect.yMax)
                        {
                            var bounds_one = new Bounds(hexagon.Pos, Vector3.one * 0.5f);
                            SeaHexagonQtArr[i].Insert(hexagon, bounds_one);
                            break;
                        }
                }

                idx++;
            }

            expandZones.Clear();
            var points = new Hexagon[settings.Length][];
            var zoneIdx = 100;
            for (var i = 0; i < settings.Length; i++)
            {
                setting = settings[i];
                if (setting.count == 0)
                    continue;

                points[i] = new Hexagon[setting.count];
                // 获取当前的面积
                var area = setting.width * setting.height;
                var prearea = 0;
                // 获取上一个的面积
                if (i - 1 >= 0) prearea = settings[i - 1].width * settings[i - 1].height;

                // 全铺范围内的总数
                var maxCount = setting.count;
                if (area - prearea > 0) maxCount = setting.count * (area / (area - prearea));

                var needDepth = GetQuadTreeNeedDepth(maxCount);
                //
                var branches = new List<QuadTree<Hexagon>.Branch>();
                GetAllBranchesByDepth(SeaHexagonQtArr[i].root, needDepth, ref branches);
                Debug.Log(
                    $"area {i}: setting.count:{setting.count}, needDepth:{needDepth}, branch count:{branches.Count}");

                if (SeaHexagonQtArr[i].root.leaveCount == 0)
                {
                    Debug.LogError(
                        $"area {i}分支数量不够,setting.count：{setting.count}, leave count:{SeaHexagonQtArr[i].root.leaveCount}");
                    continue;
                }


                // 如果不够
                while (setting.count > branches.Count)
                    if (branches.Count * 2 > setting.count)
                    {
                        Debug.LogError("aaa" + branches.Count);
                        var ext = new List<QuadTree<Hexagon>.Branch>(branches);
                        ext = ext
                            .OrderByDescending(item => item.leaveCount).ToList();
                        ext = ext.Take(setting.count - branches.Count)
                            .ToList();
                        branches = branches.Concat(ext).ToList();
                        Debug.LogError("aaa" + branches.Count);
                    }
                    else
                    {
                        needDepth++;
                        branches.Clear();
                        GetAllBranchesByDepth(SeaHexagonQtArr[i].root, needDepth, ref branches);
                        Debug.Log(
                            $"area {i}分支数量不够,setting.count：{setting.count}, branch count:{branches.Count}, needDepth++:{needDepth}");
                    }

                int color;

                // 开撒
                for (var j = 0; j < setting.count; j++)
                {
                    var random = Random.Range(0, branches.Count - 1);
                    var branch = branches[random];
                    var hexagon = GetRandomLeafFromBranch(branch);
                    points[i][j] = hexagon;
                    branches.RemoveAt(random);

                    color = zoneIdx % zoneColors.Length;
                    var zone = new Zone(this, zoneIdx++, hexagon, setting.postype, setting.level,
                        color);
                    seazones.Add(zone.index, zone);
                    expandZones.Add(zone);
                }
            }

            DealSeaZones();
            // 扩散
            minZoneHexCount = -1;
            maxZoneHexCount = -1;

            onProgress?.Invoke("地图种子扩散处理...", 0.1f);
            DoExpand(onProgress);
        }

        // 识别完的land zone, 可能会漏
        public void SewingHexagon(Action<string, float> onProgress = null)
        {
            var dic = new Dictionary<Zone, int>();
            foreach (var hexagon in hexagons)
            {
                if (hexagon.zone != null)
                    continue;

                dic.Clear();

                foreach (var hex in hexagon.neigbours)
                    if (hex != null && hex.zone != null)
                    {
                        if (dic.TryGetValue(hex.zone, out var count))
                            dic[hex.zone]++;
                        else
                            dic.Add(hex.zone, 1);
                    }

                var zone = dic.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                zone.AddHexagon(hexagon, true);
            }
        }

        public void CreatePorts(Action<string, float> onProgress = null)
        {
            var idx = 0;
            foreach (var zone in zones.Values)
            {
                onProgress?.Invoke("create port ... ", 1f * idx / zones.Count);
                zone.GeneratePort();
                idx++;
            }

            EditorUtility.DisplayDialog("港口生成", "生成成功！!!", "确定");
        }

        public void FixPorts(Action<string, float> onProgress = null)
        {
            var idx = 0;
            foreach (var zone in zones.Values)
            {
                onProgress?.Invoke("fix port ... ", 1f * idx / zones.Count);
                zone.FixPort();
                idx++;
            }

            EditorUtility.DisplayDialog("港口修复", "修复成功！!!", "确定");
        }

        private void GetAllBranchesByDepth(QuadTree<Hexagon>.Branch branch, int needdepth,
            ref List<QuadTree<Hexagon>.Branch> branches)
        {
            if (branch == null)
                return;

            if (branch.depth == needdepth && branch.leaveCount > 0)
                branches.Add(branch);
            else
                foreach (var b in branch.branches)
                    GetAllBranchesByDepth(b, needdepth, ref branches);
        }

        private int GetQuadTreeNeedDepth(int count)
        {
            var depth = 0;
            while (Math.Pow(4, depth) < count) depth++;

            return depth;
        }

        private Hexagon GetRandomLeafFromBranch(QuadTree<Hexagon>.Branch branch)
        {
            if (branch == null)
                return null;

            if (branch.leaves.Count > 0)
                return branch.leaves[Random.Range(0, branch.leaves.Count)].value;

            var branches =
                branch.branches.Where(item => item != null).ToArray();
            return GetRandomLeafFromBranch(branches[Random.Range(0, branches.Length)]);
        }

        public void CreateVoronoiZones(Vector2Int points, Action<string, float> onProgress = null)
        {
            Debug.LogFormat("Create Voronoi Zones");
            mapPoints = points;

            cityPoints = new Vector2Int[mapPoints.x, mapPoints.y];
            coordZones = new int[mapPoints.x, mapPoints.y];
            zones.Clear();
            // 添加王城的区域
            var zoneIdx = 0;
            var hexCenterIdx = new Vector2Int(width / 2, height / 2);
            var hexCenter = GetHexagon(hexCenterIdx);
            var color = zoneIdx % zoneColors.Length;
            centerZone = new Zone(this, zoneIdx++, hexCenter, 4, 7, color);
            for (var i = 0; i < 2; i++) centerZone.Expand();

            zones.Add(centerZone.index, centerZone);


            var Xrange = width / mapPoints.x;
            var Yrange = height / mapPoints.y;
            for (var y = 0; y < mapPoints.y; y++)
            for (var x = 0; x < mapPoints.x; x++)
            {
                var p = new Vector2Int(Random.Range(0, Xrange), Random.Range(0, Yrange));
                var realp = p + new Vector2Int(x * Xrange, y * Yrange);
                if (Vector2Int.Distance(realp, hexCenterIdx) > 4)
                {
                    cityPoints[x, y] = p;
                    var hexagon = GetHexagon(realp);
                    color = zoneIdx % zoneColors.Length;
                    var zone = new Zone(this, zoneIdx++, hexagon, 1, 1, color);
                    zones.Add(zone.index, zone);

                    coordZones[x, y] = zone.index;
                }
            }

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                onProgress.Invoke("calc zone ... ", 1f * (y * width + x) / hexagons.Length);

                var hexagon = GetHexagon(x, y);
                if (hexagon.zone == null)
                {
                    var citypos = GetCloseCityCoord(x, y);
                    zones[coordZones[citypos.x, citypos.y]].AddHexagon(hexagon, true);
                }
            }

            Debug.LogFormat("Create Zone : {0}", zones.Count);

            CalcEdges(onProgress);
            mapProgress = MapProgress.SeaZones;
        }

        private Vector2Int getPoint(int x, int y, out Vector2Int coord)
        {
            var Xrange = width / mapPoints.x;
            var Yrange = height / mapPoints.y;

            if (y >= mapPoints.y) y = 0;
            if (y < 0) y = mapPoints.y - 1;

            if (x >= mapPoints.x) x = 0;
            if (x < 0) x = mapPoints.x - 1;

            var off = new Vector2Int(x * Xrange, y * Yrange);

            coord = new Vector2Int(x, y);
            return cityPoints[x, y] + off;
        }

        private Vector2Int GetCloseCity(int i, int j)
        {
            var cur = new Vector2Int(i, j);
            var closetCity = Vector2Int.zero;
            var cityCoord = Vector2Int.zero;

            var xgridsize = width / mapPoints.x;
            var ygridsize = height / mapPoints.y;
            var xindex = i / xgridsize;
            var yindex = j / ygridsize;

            var minDis = float.MaxValue;
            for (var y = -1; y <= 1; y++)
            for (var x = -1; x <= 1; x++)
            {
                var p = getPoint(xindex + x, yindex + y, out cityCoord);

                float dis = DistanceSqr(p, cur);
                if (dis < minDis)
                {
                    minDis = dis;
                    closetCity = p;
                }
            }

            return closetCity;
        }

        private Vector2Int GetCloseCityCoord(int i, int j)
        {
            var cur = new Vector2Int(i, j);
            var closetCity = Vector2Int.zero;
            var cityCoord = Vector2Int.zero;

            var xgridsize = width / mapPoints.x;
            var ygridsize = height / mapPoints.y;
            var xindex = i / xgridsize;
            var yindex = j / ygridsize;

            var minDis = float.MaxValue;
            for (var y = -1; y <= 1; y++)
            for (var x = -1; x <= 1; x++)
            {
                var p = getPoint(xindex + x, yindex + y, out cityCoord);

                float dis = DistanceSqr(p, cur);
                if (dis < minDis)
                {
                    minDis = dis;
                    closetCity = cityCoord;
                }
            }

            return closetCity;
        }

        private int DistanceSqr(Vector2Int a, Vector2Int b)
        {
            return (a - b).sqrMagnitude;
        }

        public void CreateBase(Texture2D block, int w, int h)
        {
            var hexSide = Hex.HexRadius;
            blockData = block;
            blockColor = blockData.GetPixels();
            width = w;
            height = h;

            hexSide = 1f / Mathf.Cos(Mathf.Deg2Rad * 30f);
            hexW = hexSide * 0.5f;
            hexH = 1f;

            mapSizeW = width * hexSide * 1.5f + hexSide * 0.5f;
            mapSizeH = 2 * height + hexW;

            Debug.Log(
                $"hexSide = {hexSide}, hexW = {hexW}, hexH = {hexH}, sizeW = {mapSizeW}, sizeH = {mapSizeH}");


            InitColors();

            blockAnalyze = new int[w * h];
            hexagons = new Hexagon[w * h];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var idx = y * width + x;
                hexagons[idx] = new Hexagon(this, idx, x, y);
            }

            CreateNeigbours();
        }

        bool IsSameColor(Color32 a, Color32 b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        }

        public void CreateLandZoneByHex(Hexagon hex)
        {
            var oldZone = hex.zone;
            oldZone?.RemoveHexagon(hex);
            if (!zones.ContainsKey(998))
            {
                var newZone = new Zone(this, 0, hex, 0, 0, 0);
                newZone.landType = ZoneLandType.Land;
                newZone.index = 998;
                newZone.level = 0;
                zones.Add(newZone.index, newZone);
                newZone.AddHexagon(hex);
                hex.zone = newZone;
            }
            else
            {
                zones[998].AddHexagon(hex);
                hex.zone = zones[998];
            }


            oldZone?.CalcEdges();
            zones[998].CalcEdges();
        }

        public void CreateWaterZoneByHex(Hexagon hex)
        {
            var oldZone = hex.zone;
            oldZone?.RemoveHexagon(hex);
            if (!zones.ContainsKey(999))
            {
                var newZone = new Zone(this, 0, hex, 0, 0, 0);
                newZone.landType = ZoneLandType.Sea;
                newZone.index = 999;
                newZone.level = 0;
                zones.Add(newZone.index, newZone);
                newZone.AddHexagon(hex);
                hex.zone = newZone;
            }
            else
            {
                zones[999].AddHexagon(hex);
                hex.zone = zones[999];
            }


            oldZone?.CalcEdges();
            zones[999].CalcEdges();
        }

        public void CreateZoneByTex(Texture2D zoneTex, int w, int h, Dictionary<Color32, Vector3Int> colorLevelMap = null, Action<string, float> onProgress = null)
        {
            if (zoneTex == null)
                return;
            TextureUtils.CheckTexReadable(zoneTex);
            var pixels = zoneTex.GetPixels32();

            Dictionary<Color32, List<Vector2Int>> colorGroups = new Dictionary<Color32, List<Vector2Int>>();
            onProgress?.Invoke($"识别颜色数量 {colorGroups.Count}", 0f);
            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % w;
                int y = i / w;

                Color32 originalColor = pixels[i];
                Color32 newColor = originalColor;

                // 处理边缘及左右颜色不一致情况
                if (y == 0 && y < h - 1)
                {
                    var below = pixels[i + w];
                    if (!IsSameColor(originalColor, below))
                        newColor = below;

                }
                else if (y == h - 1 && y > 0)
                {
                    var above = pixels[i - w];
                    if (!IsSameColor(originalColor, above))
                        newColor = above;
                }
                else if (x == 0 && x < w - 1)
                {
                    var right = pixels[i + 1];
                    if (!IsSameColor(originalColor, right))
                        newColor = right;
                }
                else if (x == w - 1 && x > 0)
                {
                    var left = pixels[i - 1];
                    if (!IsSameColor(originalColor, left))
                        newColor = left;
                }
                else if (x > 0 && x < w - 1 && y > 0 && y < h - 1)
                {
                    var left = pixels[i - 1];
                    var right = pixels[i + 1];
                    if (!IsSameColor(originalColor, left) && !IsSameColor(originalColor, right))
                        newColor = left;
                }

                if (!IsSameColor(originalColor, newColor))
                    pixels[i] = newColor;

                // 加入颜色分组
                if (!colorGroups.TryGetValue(newColor, out var list))
                {
                    list = new List<Vector2Int>();
                    colorGroups[newColor] = list;
                }
                list.Add(new Vector2Int(x, y));

                // 每行首像素，清理数量过少的颜色组
                if (x == 0 && colorGroups.Count > 1)
                {
                    var keysToRemove = new List<Color32>();
                    foreach (var kv in colorGroups)
                    {
                        if (kv.Value.Count <= 3)
                            keysToRemove.Add(kv.Key);
                    }

                    foreach (var key in keysToRemove)
                        colorGroups.Remove(key);
                }
                onProgress?.Invoke($"识别颜色数量 {colorGroups.Count}", 1f * i / pixels.Length);
            }

            onProgress?.Invoke($"一共识别出颜色数量 {colorGroups.Count}", 100);
            // 按照纹理的像素点颜色来创建区域
            zones.Clear();
            expandZones.Clear();
            delZones.Clear();
            width = w;
            height = h;
            frameColors = new Color[colorGroups.Count];

            for (int i = 0; i < colorGroups.Count; i++)
            {
                var color = colorGroups.ElementAt(i).Key;
                frameColors[i] = color;
                var hexPosGroup = colorGroups.ElementAt(i).Value;

                var level = 1;
                var zoneId = 1000 + i;
                var zoneSize = 1000;
                if (colorLevelMap != null && colorLevelMap.TryGetValue(color, out var levelValue))
                {
                    level = levelValue.x;
                    zoneId = levelValue.y;
                    zoneSize = levelValue.z;
                }
                var center = FindClosestToCenter(hexPosGroup);
                var centerHexagon = GetHexagon(center.x, center.y);
                var zone = new Zone(this, zoneId, centerHexagon, 0, level, i);
                zone.SizePreset = zoneSize;
                zones.Add(zone.index, zone);
                if (center.x == w / 2 && center.y == h / 2)
                {
                    int tSize = (int)Math.Floor(Math.Sqrt(zone.SizePreset) / 2);
                    for (int x = -tSize; x < tSize ; x++)
                    {
                        for (int y = -tSize; y < tSize; y++)
                        {
                            var hexPos = new Vector2Int(center.x + x, center.y + y);
                            var hexagon = GetHexagon(hexPos.x, hexPos.y);
                            hexagon.zone = zone;
                            zone.AddHexagon(hexagon);
                        }
                    }
                }
                else
                {

                    zone.ExpandRound = GetHexagonRingCount(zone.SizePreset);
                    expandZones.Add(zone);
                }

                onProgress?.Invoke($"创建区域：{zones.Count} / {colorGroups.Count}", 1f * zones.Count / colorGroups.Count);
            }
            // expandZones.Sort((a, b) => -a.level.CompareTo(b.level));

            DoExpand(onProgress);
        }

        int GetHexagonRingCount(int num)
        {
            if (num <= 1)
                return 0;

            float value = (num - 1) / 3f;
            float r = (-1f + Mathf.Sqrt(1f + 4f * value)) / 2f;
            return Mathf.CeilToInt(r);
        }

        Vector2Int FindCenter(List<Vector2Int> positions)
        {
            if (positions == null || positions.Count == 0)
                return Vector2Int.zero;

            int sumX = 0;
            int sumY = 0;

            foreach (var pos in positions)
            {
                sumX += pos.x;
                sumY += pos.y;
            }

            int avgX = Mathf.RoundToInt((float)sumX / positions.Count);
            int avgY = Mathf.RoundToInt((float)sumY / positions.Count);
            // 加上随机偏移 [-3, 3]
            if (avgX == width / 2 && avgY == width / 2)
                return new Vector2Int(avgX, avgY);
            avgX += Random.Range(-50, 51);
            // avgY += Random.Range(-50, 51);
            return new Vector2Int(avgX, avgY);
        }

        Vector2Int FindClosestToCenter(List<Vector2Int> positions)
        {
            if (positions == null || positions.Count == 0)
                return Vector2Int.zero;

            // 计算中心点
            var center = FindCenter(positions);

            // 找离中心点最近的实际点
            Vector2Int closest = positions[0];
            float minDist = Vector2.Distance(center, closest);

            foreach (var pos in positions)
            {
                float dist = Vector2.Distance(center, pos);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = pos;
                }
            }

            return closest;
        }

        public void InitColors()
        {
            zoneColors = new Color[100];
            for (var i = 0; i < zoneColors.Length; i++)
            {
                var f1 = Random.Range(0f, 1f);
                var f2 = Random.Range(0f, 1f);
                var f3 = Random.Range(0f, 1f);
                zoneColors[i] = new Color(f1, f2, f3);
            }

            zoneColors[1] = Color.yellow;
            zoneColors[2] = Color.blue;
            zoneColors[3] = Color.green;
            zoneColors[4] = Color.red;

            frameColors = new Color[zoneColors.Length];
            for (var i = 0; i < zoneColors.Length; i++)
            {
                var c = zoneColors[i];
                frameColors[i] = new Color(1f - c.r, 1f - c.g, 1f - c.b);
            }
        }

        #region Block

        public void CountAllBlock()
        {
            BlockCount = 0;
            var row = 0;

            EditorApplication.update = delegate
            {
                var isCancel =
                    EditorUtility.DisplayCancelableProgressBar("匹配资源中", "统计中。。。",
                        1f * row / height);

                for (var col = 0; col < width; col++)
                    if (IsBlock(row, col))
                        BlockCount++;

                row++;

                if (isCancel || row >= height)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    row = 0;
                }
            };
        }

        public bool IsBlock(int x, int y)
        {
            // leon
            // hexagon身上没有阻挡标识就用blockData的
            var idx = GetIndex(x, y);
            return IsBlock(idx);
        }

        public bool IsBlock(int idx)
        {
            // leon
            // hexagon身上没有阻挡标识就用blockData的
            if (hexagons[idx].blockFlag != BlockFlag.None)
                return hexagons[idx].blockFlag == BlockFlag.Block;

            if (blockAnalyze[idx] != 0) return blockAnalyze[idx] == 1;

            if (blockData != null)
            {
                if (idx >= blockColor.Length)
                    return false;
                //var c = blockData.GetPixel(x, height - y);
                var c = blockColor[idx];
                blockAnalyze[idx] = c.r == 1 ? 1 : 2;
                return c.r == 1 ? true : false;
            }

            return false;
        }

        // 根据场景objects计算阻挡
        public void CalStaticBlock()
        {
            var mapObjectsParent = MapRender.instance.parentForPrefabs;
            var parentTrans = mapObjectsParent.transform;

            var count = parentTrans.childCount;
            for (var i = 0; i < count; i++)
            {
                var tabTrans = parentTrans.GetChild(i);
                var tabChildCount = tabTrans.childCount;
                for (var j = 0; j < tabChildCount; j++)
                {
                    EditorUtility.DisplayProgressBar("计算阻挡中",
                        $"计算中...{tabTrans.name} - {tabChildCount}/{j}",
                        1f * j / tabChildCount * (i + 1));
                    var objectTrans = tabTrans.GetChild(j);
                    var prefabPolygonEx = objectTrans.GetComponent<PrefabPolygonEx>();
                    if (GetObjectWorldBounds(objectTrans.gameObject, out var bounds))
                    {
                        GetBoundsRect(bounds, out var minX, out var minZ, out var maxX,
                            out var maxZ);
                        for (var y = minZ; y <= maxZ; y++)
                        for (var x = minX; x <= maxX; x++)
                        {
                            var temHex = GetHexagon(x, y);
                            var pos = Hex.OffsetToWorld(x, y);
                            if (prefabPolygonEx.IsPointInPolygon(pos))
                            {
                                // temHex.blockFlag = BlockFlag.Block;
                                temHex.movetype = MOVETYPE.DISABLE;
                            }
                            else
                            {
                                var flag = 0;
                                for (var k = 0; k < 6; k++)
                                {
                                    var p = GetHexagonVertex(pos, k);
                                    if (prefabPolygonEx.IsPointInPolygon(p))
                                    {
                                        flag++;
                                        if (flag >= 2) temHex.movetype = MOVETYPE.DISABLE;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            EditorUtility.ClearProgressBar();
        }

        private void GetBoundsRect(Bounds bounds, out int minX, out int minZ, out int maxX,
            out int maxZ)
        {
            var coord1 = PickHex(bounds.min);
            var coord2 = PickHex(bounds.max);

            minX = Mathf.Min(coord1.x, coord2.x);
            maxX = Mathf.Max(coord1.x, coord2.x);

            minZ = Mathf.Min(coord1.y, coord2.y);
            maxZ = Mathf.Max(coord1.y, coord2.y);

            if (minX < 0) minX = 0;
            if (maxX >= width) maxX = width - 1;
            if (minZ < 0) minZ = 0;
            if (maxZ >= height) maxZ = height - 1;
        }

        private Ray ray = new(Vector3.zero, new Vector3(0, -100, 0));
        private readonly Vector3 rayOffset = new(0, 100, 0);

        private Vector2Int PickHex(Vector3 worldPos)
        {
            ray.origin = worldPos + rayOffset;
            if (Physics.Raycast(ray, out var hitInfo)) return Hex.WorldToOffset(hitInfo.point);

            return Vector2Int.zero;
        }

        private bool GetObjectWorldBounds(GameObject gameObject, out Bounds bounds)
        {
            var worldBounds = new Bounds();
            var found = false;

            PrefabPainter.Utility.ForAllInHierarchy(gameObject, go =>
            {
                if (!go.activeInHierarchy)
                    return;

                var renderer = go.GetComponent<Renderer>();
                SkinnedMeshRenderer skinnedMeshRenderer;
                RectTransform rectTransform;

                if (renderer != null)
                {
                    if (!found)
                    {
                        worldBounds = renderer.bounds;
                        found = true;
                    }
                    else
                    {
                        worldBounds.Encapsulate(renderer.bounds);
                    }
                }
                else if ((skinnedMeshRenderer = go.GetComponent<SkinnedMeshRenderer>()) != null)
                {
                    if (!found)
                    {
                        worldBounds = skinnedMeshRenderer.bounds;
                        found = true;
                    }
                    else
                    {
                        worldBounds.Encapsulate(skinnedMeshRenderer.bounds);
                    }
                }
                else if ((rectTransform = go.GetComponent<RectTransform>()) != null)
                {
                    var fourCorners = new Vector3[4];
                    rectTransform.GetWorldCorners(fourCorners);
                    var rectBounds = new Bounds();

                    rectBounds.center = fourCorners[0];
                    rectBounds.Encapsulate(fourCorners[1]);
                    rectBounds.Encapsulate(fourCorners[2]);
                    rectBounds.Encapsulate(fourCorners[3]);

                    if (!found)
                    {
                        worldBounds = rectBounds;
                        found = true;
                    }
                    else
                    {
                        worldBounds.Encapsulate(rectBounds);
                    }
                }
            });

            if (!found)
                bounds = new Bounds(gameObject.transform.position, Vector3.one);
            else
                bounds = worldBounds;

            return found;
        }

        public MOVETYPE GetMoveType(int x, int y)
        {
            var idx = GetIndex(x, y);
            return GetMoveType(idx);
        }


        public MOVETYPE GetMoveType(int idx)
        {
            var hexagon = hexagons[idx];
            return GetMoveType(hexagon);
        }

        public MOVETYPE GetMoveType(Hexagon hexagon)
        {
            if (hexagon.zone.landType == ZoneLandType.Land
                || IsBlock(hexagon.index))
                return MOVETYPE.DISABLE;

            return hexagon.movetype;
        }

        #endregion

        //是否是海港占地范围
        public bool IsPortAround(int index)
        {
            var hex = GetHexagon(index);
            return hex.zone.IsPortAround(index);
        }

        public bool IsMapBorderHexagon(Hexagon hexagon)
        {
            var result = false;
            if (hexagon.x >= width - 1 || hexagon.y >= height - 1 || hexagon.x <= 0 ||
                hexagon.y <= 0)
                result = true;
            else
                result = false;

            return result;
        }

        public void ResetBlock(Texture2D blocktex = null)
        {
            blockData = blocktex;
            blockAnalyze = new int[width * height];
            if (blocktex != null)
            {
                TextureUtils.CheckTexReadable(blocktex);
                blockColor = blocktex.GetPixels();
            }
            else
            {
                blockColor = null;
            }
        }

        public void ResetLandForm()
        {
            foreach (var zone in zones.Values) zone.landform = ZoneLandform.LANDFORM1;
        }

        public void ResetLandForm(ZoneLandform from)
        {
            foreach (var zone in zones.Values)
                if (zone.landform == from)
                    zone.landform = ZoneLandform.LANDFORM1;
        }

        public void SetLandForm(Texture2D tex, ZoneLandform landform)
        {
            if (tex == null)
            {
                EditorUtility.DisplayDialog("警告", "未选择图片", "OK");
                return;
            }

            var idx = 0;
            Debug.LogError($"图像大小:{tex.width},{tex.height}");
            // TextureUtils.CheckTexReadable(tex);
            var colors = tex.GetPixels32();
            foreach (var zone in zones.Values)
            {
                EditorUtility.DisplayProgressBar("设置地貌", $"计算港口位置[{zone.index}]",
                    1f * idx / zones.Count);
                idx++;
                var hexagon = zone.hexagon;
                if (zone._portIndex < 0)
                    continue;
                var pos = GetHexagonPos(zone._portIndex);
                var pixelpos = new Vector2Int(
                    Mathf.RoundToInt(pos.x / mapSizeW * (tex.width - 1)), Mathf.RoundToInt(-pos.z /
                        mapSizeH *
                        (tex.height - 1)));

                var pixelidx = pixelpos.x + (tex.height - 1 - pixelpos.y) * tex.width;
                // int pixelidx = pixelpos.x + pixelpos.y * tex.width;

                if (colors[pixelidx].r > 0.7f)
                    zone.landform = landform;
            }

            EditorUtility.ClearProgressBar();
        }

        public Color GetZoneColor(int idx)
        {
            return idx >= 0 ? zoneColors[idx % zoneColors.Length] : Color.white;
        }

        public void SetZoneColor(int idx, Color c)
        {
            if (idx < 0)
                return;

            idx = idx % zoneColors.Length;
            zoneColors[idx] = c;
            frameColors[idx] = new Color(1f - c.r, 1f - c.g, 1f - c.b);
        }

        public Color GetZoneFrameColor(int idx)
        {
            return idx >= 0 ? frameColors[idx % frameColors.Length] : Color.white;
        }

        public Zone GetZone(int zoneId)
        {
            if (zones.TryGetValue(zoneId, out var value)) return value;

            return null;
        }

        public int GetIndex(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height ? y * width + x : -1;
        }

        public Vector3 GetCube(int index)
        {
            var offset = GetOffset(index);
            return Hex.OffsetToCube(offset);
        }

        public Vector2Int GetOffset(int index)
        {
            var x = index % width;
            var y = (index - x) / width;
            return new Vector2Int(x, y);
        }

        public Vector3 GetHexagonPos(int x, int y)
        {
            return Hex.OffsetToWorld(new Vector2Int(x, y));
        }

        public Vector3 GetHexagonVertex(int x, int y, int index)
        {
            var p = Hex.OffsetToWorld(new Vector2Int(x, y));
            return GetHexagonVertex(p, index);
        }

        public Vector3 GetHexagonVertex(Vector3 p, int index)
        {
            // 	   2 一 1
            //	  /		 \
            //	 3		  0
            //	  \		 /
            // 	   4 一 5
            var hexSide = Hex.HexRadius;
            // switch (index)
            // {
            //     case 0: return new Vector3(p.x + hexSide, p.y, p.z); // 右
            //     case 1: return new Vector3(p.x + hexSide / 2, p.y, p.z + hexH); // 右上
            //     case 2: return new Vector3(p.x - hexSide / 2, p.y, p.z + hexH); // 左上
            //     case 3: return new Vector3(p.x - hexSide, p.y, p.z); // 左
            //     case 4: return new Vector3(p.x - hexSide / 2, p.y, p.z - hexH); // 左下
            //     case 5: return new Vector3(p.x + hexSide / 2, p.y, p.z - hexH); // 右下
            // }

            //    1      h = hexsize * 2 w = hexsize * sqrt(3)
            //   / \
            //  2   0
            //  |   |
            //  3   5
            //   \ /
            //    4
            switch (index)
            {
                case 0: return new Vector3(p.x + hexW, p.y, p.z + hexH);
                case 1: return new Vector3(p.x, p.y, p.z + hexSide);
                case 2: return new Vector3(p.x - hexW, p.y, p.z + hexH);
                case 3: return new Vector3(p.x - hexW, p.y, p.z - hexH);
                case 4: return new Vector3(p.x, p.y, p.z - hexSide);
                case 5: return new Vector3(p.x + hexW, p.y, p.z - hexH);
            }

            return Vector3.zero;
        }

        public Vector3 GetHexagonPos(Hexagon hex)
        {
            var x = hex.index % width;
            var y = (hex.index - x) / width;
            return GetHexagonPos(x, y);
        }

        public Vector3 GetHexagonPos(Axial axial)
        {
            return GetHexagonPos(axial.q, axial.r);
        }

        public Vector3 GetHexagonPos(int index)
        {
            var x = index % width;
            var y = (index - x) / width;
            return GetHexagonPos(x, y);
        }

        public Hexagon GetHexagon(int x, int y)
        {
            return GetHexagon(GetIndex(x, y));
        }

        public Hexagon GetHexagon(Vector2Int xy)
        {
            return GetHexagon(GetIndex(xy.x, xy.y));
        }

        public Hexagon GetHexagon(int idx)
        {
            return idx >= 0 && idx < hexagons.Length ? hexagons[idx] : null;
        }

        private void CreateNeigbours()
        {
            var cur = Vector2Int.zero;
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                cur.x = x;
                cur.y = y;
                var idx = y * width + x;

                var neighbors = Hex.OddRNeighborsOffset(cur);
                for (int i = 0; i < neighbors.Length; i++)
                {
                    hexagons[idx].neigbours[i] = GetHexagon(neighbors[i]);
                }
            }
        }

        // 1、在地图上每X个地格放置标记A。形成一个A的矩阵
        private void CreateSeek(SeekInfo seek, ref List<SeekPos> lastSeeks,
            ref List<SeekPos> curSeeks)
        {
            if (seek == null)
                return;

            var tx = (width - seek.width) / 2;
            var ty = (height - seek.height) / 2;

            var toDels = new List<SeekPos>();
            if (lastSeeks != null && lastSeeks.Count > 0)
            {
                // 先清除掉提前生成在这个区域的种子
                toDels.Clear();
                var rc = new Rect(tx, ty, seek.width, seek.height);
                foreach (var t in lastSeeks)
                    if (rc.Contains(t.pos))
                        toDels.Add(t);

                foreach (var t in toDels) lastSeeks.Remove(t);

                Debug.LogFormat($"lv[{seek.level}] Remove Seeks : {0}", toDels.Count);
            }

            for (var y = 0; y < seek.width; y++)
            {
                if ((y - seek.startOffsetY) % seek.seekOffset != 0 || y == 0)
                    continue;

                for (var x = 0; x < seek.height; x++)
                {
                    if ((x - seek.startOffsetX) % seek.seekOffset != 0 || x == 0)
                        continue;

                    var t = new SeekPos();
                    t.pos = new Vector2Int(tx + x, ty + y);
                    t.posType = seek.posType;
                    t.lv = seek.level;
                    curSeeks.Add(t);
                }
            }

            Debug.LogFormat($"lv[{seek.level}] Create Seeks : {0}", curSeeks.Count);

            //2、随机选取1/3的A，选取随机方向，进行1-3个地格的偏移，形成新的矩阵
            for (var i = 0; i < curSeeks.Count; i++)
            {
                if (Random.Range(0, 100) > seek.seekAdjustRate)
                    continue;

                var p = curSeeks[i];
                p.pos.x += Random.Range(-seek.seekAdjustOffset, seek.seekAdjustOffset);
                p.pos.y += Random.Range(-seek.seekAdjustOffset, seek.seekAdjustOffset);
                p.pos.x = Mathf.Clamp(p.pos.x, 0, width - 1);
                p.pos.y = Mathf.Clamp(p.pos.y, 0, height - 1);
                curSeeks[i] = p;
            }

            toDels.Clear();

            if (seek.level < 6)
            {
                //3、每隔3-5个A点，删除一个A
                var next = Random.Range(seek.seekDelRandomMin, seek.seekDelRandomMax);
                while (next < curSeeks.Count)
                {
                    toDels.Add(curSeeks[next]);
                    next += Random.Range(seek.seekDelRandomMin, seek.seekDelRandomMax);
                }
            }
            else if (seek.level == 6)
            {
                // 删除第2，5， 8个种子
                if (curSeeks.Count > 1) toDels.Add(curSeeks[1]);
                if (curSeeks.Count > 4) toDels.Add(curSeeks[4]);
                if (curSeeks.Count > 7) toDels.Add(curSeeks[7]);
            }

            foreach (var hex in toDels) curSeeks.Remove(hex);
        }

        private void CreateZones()
        {
            var allSeeks = new Dictionary<Hexagon, SeekPos>();
            for (var i = 0; i < lv1Seeks.Count; i++)
            {
                var hex = GetHexagon(lv1Seeks[i].pos.x, lv1Seeks[i].pos.y);
                if (hex != null && !allSeeks.ContainsKey(hex)) allSeeks.Add(hex, lv1Seeks[i]);
            }

            for (var i = 0; i < lv2Seeks.Count; i++)
            {
                var hex = GetHexagon(lv2Seeks[i].pos.x, lv2Seeks[i].pos.y);
                if (hex != null && !allSeeks.ContainsKey(hex)) allSeeks.Add(hex, lv2Seeks[i]);
            }

            for (var i = 0; i < lv3Seeks.Count; i++)
            {
                var hex = GetHexagon(lv3Seeks[i].pos.x, lv3Seeks[i].pos.y);
                if (hex != null && !allSeeks.ContainsKey(hex)) allSeeks.Add(hex, lv3Seeks[i]);
            }

            Debug.LogFormat("Last Seeks : {0}", allSeeks.Count);

            zones.Clear();

            // 添加王城的区域
            var zoneIdx = 1;
            var hexCenter = GetHexagon(width / 2, height / 2);
            var color = zoneIdx % zoneColors.Length;
            centerZone = new Zone(this, zoneIdx++, hexCenter, 4, 7, color);
            for (var i = 0; i < 2; i++) centerZone.Expand();

            zones.Add(centerZone.index, centerZone);

            foreach (var it in allSeeks)
            {
                color = zoneIdx % zoneColors.Length;
                var zone = new Zone(this, zoneIdx++, it.Key, it.Value.posType, it.Value.lv, color);
                zones.Add(zone.index, zone);
            }

            expandZones.Clear();
            expandZones.AddRange(zones.Values);

            Debug.LogFormat("Create Zone : {0}", zones.Count);
        }

        /*
         * 4、以每个A点为中心，向周围扩散，当遇到其他A的边界时，跳过此地格，
         * 直到所有扩散的地格都接触到其他A的边界停止。
         * 最终形成一个可控的地块划分。（类似多种不互溶的液体，滴到每个A点，然后无限扩散的过程）
         *
         * 算法修改为： 在已有的地盘上随机选取一个边界点，用这个点做多圈扩散。
         *
         */
        private void DoExpand(Action<string, float> onProgress = null)
        {
            // DoStepExpand(onProgress);
            while (true)
                if (DoStepExpand(onProgress))
                    break;
        }

        public bool DoStepExpand(Action<string, float> onProgress = null)
        {
            delZones.Clear();

            onProgress?.Invoke($"地图种子扩散中...剩余 {expandZones.Count} 个",
                (float)(zones.Count - expandZones.Count) / zones.Count);

            for (var i = 0; i < expandZones.Count; i++)
                // for (var j = 0; j < 2; j++)
                // var round = Random.Range(2, 6);
                // if (!expandZones[i].ExpandEx(round))
                if (!expandZones[i].Expand())
                {
                    var count = expandZones[i].hexagons.Count;
                    if (minZoneHexCount < 0 || minZoneHexCount > count) minZoneHexCount = count;

                        if (maxZoneHexCount < count) maxZoneHexCount = count;

                    delZones.Add(expandZones[i]);
                    break;
                }

            for (var i = 0; i < delZones.Count; i++) expandZones.Remove(delZones[i]);

            if (expandZones.Count == 0)
            {
                SewingHexagon();
                CalcEdges(onProgress);
                Debug.LogFormat("<color=#00ff00> Hexagon map done !!!</color>");
                mapProgress = MapProgress.SeaZones;
                return true;
            }

            return false;
        }

        public void ResetHexagonMovetypeMark(int round = 0)
        {
            var rets = new List<Hexagon>();
            var _expandEdge = new List<Hexagon>();
            var _newExpands = new List<Hexagon>();

            var mark = ~(~0 << (int)MOVETYPE.COUNT);
            var normalmark = 1 << (int)MOVETYPE.NORMAL;
            var disablemark = 1 << (int)MOVETYPE.DISABLE;

            // 重置海水的movetypemark
            foreach (var hexagon in hexagons)
            {
                hexagon.movetypeMark = mark;
                hexagon.movetypeMarkRandomSea = mark;
                // 海水全部改成默认
                if (hexagon.movetype >= MOVETYPE.NORMAL)
                    hexagon.movetype = GlobalDef.S_DefaultMoveType;
            }

            // 陆地		只能为disable
            // 近陆地	round格只能为normal
            var idx = 0;
            Hexagon hextmp;
            foreach (var zone in landzones.Values)
            {
                EditorUtility.DisplayProgressBar("计算陆地的辐射区域", $"计算陆地[{zone.index}]",
                    1f * idx / landzones.Count);
                idx++;

                rets.Clear();
                rets.AddRange(zone.hexagons);

                _expandEdge.Clear();
                _expandEdge.AddRange(zone.edges);

                // 陆地 只能为disable
                foreach (var hexagon in zone.hexagons) hexagon.movetypeMark = disablemark;

                // 沿陆地边缘 外扩round圈
                for (var k = 0; k < round; k++)
                {
                    _newExpands.Clear();
                    foreach (var t in _expandEdge)
                        for (var i = 0; i < t.neigbours.Length; i++)
                        {
                            hextmp = t.neigbours[i];

                            if (hextmp != null
                                && hextmp.movetypeMark != normalmark
                                && !rets.Contains(hextmp))
                            {
                                rets.Add(hextmp);
                                _newExpands.Add(hextmp);
                                hextmp.movetypeMark = normalmark;
                            }
                        }

                    _expandEdge.Clear();
                    _expandEdge.AddRange(_newExpands);
                }
            }


            EditorUtility.ClearProgressBar();

            needResetMovetypeMark = false;
        }

        /// <summary>
        ///     根据内 中 外圈生成分隔海
        /// </summary>
        /// <param name="expandRound"></param>
        public void GenerateSeaAreaWithPostype(int expandRound)
        {
            ResetHexagonMovetypeMark();
            // postype, zones
            var postypeZoneDic = new Dictionary<int, List<Zone>>();
            foreach (var zone in zones.Values)
            {
                if (!postypeZoneDic.TryGetValue(zone.posType, out var zoneLst))
                {
                    zoneLst = new List<Zone>();
                    postypeZoneDic.Add(zone.posType, zoneLst);
                }

                zoneLst.Add(zone);
            }

            foreach (var kv in postypeZoneDic)
            {
                var postype = kv.Key;
                var dealZones = kv.Value;

                // 找地域边界
                var postypeEdgeHexagons = new List<Hexagon>();
                foreach (var zone in dealZones)
                foreach (var edgeHexagon in zone.edges)
                {
                    if (edgeHexagon.movetype == MOVETYPE.DEEPSEA)
                        continue;
                    for (var i = 0; i < 6; i++)
                    {
                        var neighbour = edgeHexagon.neigbours[i];
                        if (neighbour != null &&
                            neighbour.movetype != MOVETYPE.DEEPSEA &&
                            neighbour.zone != null &&
                            neighbour.zone != zone &&
                            neighbour.zone.posType != postype)
                            postypeEdgeHexagons.Add(edgeHexagon);
                    }
                }

                // 边界扩散
                var expands = postypeEdgeHexagons;
                var round = 0;

                while (round <= expandRound)
                {
                    var newExpands = new List<Hexagon>();
                    foreach (var hex in expands)
                    {
                        hex.movetype = MOVETYPE.DEEPSEA;
                        for (var i = 0; i < hex.neigbours.Length; i++)
                        {
                            if (hex.neigbours[i] == null ||
                                hex.neigbours[i].movetype == MOVETYPE.DEEPSEA)
                                continue;

                            newExpands.Add(hex.neigbours[i]);
                        }
                    }

                    expands = new List<Hexagon>(newExpands);
                    round++;
                }
            }
        }

        /// <summary>
        ///     随机深浅海 TODO 构建一个area的概念
        /// </summary>
        /// <param name="landround"></param>
        /// <param name="shallowSetting"></param>
        /// <param name="deepSetting"></param>
        /// <param name="forceReset"></param>
        public void GenerateSeaAreaMoveType(int landround,
            MovetypeScatterSetting shallowSetting,
            MovetypeScatterSetting deepSetting,
            bool forceReset = false)
        {
            // 重置mark
            if (needResetMovetypeMark || forceReset)
                ResetHexagonMovetypeMark(landround);


            // 随机浅海
            RandomSeaAreaMoveType(MOVETYPE.SHALLOWSEA, shallowSetting, true);
            RandomSeaAreaMoveType(MOVETYPE.DEEPSEA, deepSetting, true);
            // 孤点
            CheckOrphan();
        }

        /// <summary>
        ///     随机点，并随机扩散生成深浅海区域
        /// </summary>
        /// <param name="movetype"></param>
        /// <param name="totalCount">区域数量</param>
        /// <param name="minExpand">随机扩散最小值</param>
        /// <param name="maxExpand">随机扩散最大值</param>
        /// <param name="round">round范围内只能是常规海</param>
        private void RandomSeaAreaMoveType(MOVETYPE movetype,
            MovetypeScatterSetting setting, bool affectRound)
        {
            var mark = 1 << (int)movetype;
            var normalmark = (1 << (int)MOVETYPE.NORMAL) | (1 << (int)movetype);
            var dic = new Dictionary<Hexagon, List<Hexagon>>();
            var seeds = hexagons
                .Where(item => (item.movetypeMark & item.movetypeMarkRandomSea & mark) > 0)
                .ToList();
            var max = Math.Min(seeds.Count, setting.count);
            while (dic.Count < max)
            {
                var random = Random.Range(0, seeds.Count);
                var hexagon = seeds[random];
                dic.Add(hexagon, new List<Hexagon>());
                hexagon.movetype = movetype;
                hexagon.movetypeMarkRandomSea = normalmark;
                seeds.RemoveAt(random);
            }

            // 扩张
            var rets = new List<Hexagon>();
            var curExpands = new List<Hexagon>();
            var newExpands = new List<Hexagon>();

            var idx = 0;
            foreach (var kv in dic)
            {
                EditorUtility.DisplayProgressBar("随机", $"随机中... {idx}/{dic.Count}",
                    1f * idx / dic.Count);
                idx++;
                rets = dic[kv.Key];
                rets.Add(kv.Key);

                curExpands.Clear();
                // curExpands.Add(kv.Key);

                Hexagon hextmp = null;
                // 扩几回
                var step = Random.Range(setting.minStep, setting.maxStep);
                for (var k = 0; k < step; k++)
                {
                    // newExpands.Clear();
                    // 随机获取一点，向外随机扩张
                    GetMoveTypeAreaEdgeHexagons(rets, ref curExpands);
                    if (curExpands.Count == 0)
                        break;
                    var start = curExpands[Random.Range(0, curExpands.Count)];

                    for (var i = 0; i < start.neigbours.Length; i++)
                    {
                        hextmp = start.neigbours[i];
                        if (hextmp != null
                            && hextmp.movetype == MOVETYPE.NORMAL
                            && (hextmp.movetypeMark & hextmp.movetypeMarkRandomSea & mark) > 0
                            && !rets.Contains(hextmp))
                        {
                            rets.Add(hextmp);
                            // newExpands.Add(hextmp);
                            hextmp.movetype = movetype;
                            hextmp.movetypeMarkRandomSea = normalmark;
                        }
                    }

                    // if (newExpands.Count == 0)
                    // 	break;
                    //
                    // curExpands.Clear();
                    // curExpands.AddRange(newExpands);
                }

                if (!affectRound)
                    continue;
                // 找到rets的边界
                GetMoveTypeAreaEdgeHexagons(rets, ref curExpands);

                // rets外扩round圈， 将mark改为normal

                for (var k = 0; k < setting.affectRound; k++)
                {
                    newExpands.Clear();
                    foreach (var hex in curExpands)
                        for (var i = 0; i < hex.neigbours.Length; i++)
                        {
                            hextmp = hex.neigbours[i];
                            if (hextmp == null)
                                continue;

                            if (hextmp.movetype == MOVETYPE.NORMAL
                                && hextmp.movetypeMarkRandomSea != normalmark
                                && (hextmp.movetypeMarkRandomSea & mark) > 0
                                && !rets.Contains(hextmp))
                            {
                                hextmp.movetypeMarkRandomSea = normalmark;
                                newExpands.Add(hextmp);
                            }
                        }

                    if (newExpands.Count == 0) break;

                    curExpands.Clear();
                    curExpands.AddRange(newExpands);
                }
            }

            EditorUtility.ClearProgressBar();
        }

        private void GetMoveTypeAreaEdgeHexagons(List<Hexagon> hexagons, ref List<Hexagon> results)
        {
            results.Clear();
            foreach (var hex in hexagons)
                for (var i = 0; i < hex.neigbours.Length; i++)
                    if (hex.neigbours[i] != null
                        && hex.neigbours[i].movetype != hex.movetype)
                    {
                        results.Add(hex);
                        break;
                    }
        }

        private void CheckOrphan()
        {
            var counts = new Dictionary<int, int>(2);
            foreach (var hexagon in hexagons)
            {
                counts.Clear();
                if (hexagon.movetype == MOVETYPE.NORMAL)
                    for (var h = 0; h < hexagon.neigbours.Length; h++)
                    {
                        var nei = hexagon.neigbours[h];
                        if (nei != null && (nei.movetype == MOVETYPE.DEEPSEA ||
                                            nei.movetype == MOVETYPE.SHALLOWSEA))
                        {
                            var type = (int)nei.movetype;
                            if (counts.ContainsKey(type))
                                counts[type]++;
                            else
                                counts.Add(type, 1);
                        }
                    }

                foreach (var kv in counts)
                    if (kv.Value >= 4)
                    {
                        hexagon.movetype = (MOVETYPE)kv.Key;
                        break;
                    }
            }
        }

        private int GetNewZoneId()
        {
            var zoneIdx = 1;
            foreach (var zone in zones.Values)
                if (zone.index > zoneIdx)
                    zoneIdx = zone.index;

            zoneIdx++;
            return zoneIdx;
        }

        public void NewZone(PatternZone pattern, Hexagon target, int posType = 1)
        {
            var zoneIdx = GetNewZoneId();
            if (target != null && target.zone != null)
                target.zone.RemoveHexagon(target);
            var newZone = new Zone(this, zoneIdx, target, posType, pattern.Level,
                zoneIdx % zoneColors.Length);
            newZone.isBorn = pattern.IsBorn;
            newZone.isGuanqia = pattern.IsGuanqia;
            newZone.subType = pattern.SubType;
            zones.Add(zoneIdx, newZone);
            var temps = new List<Hexagon>();
            pattern.GetAllHexagon(target, ref temps);
            foreach (var hex in temps)
            {
                if (hex == null) continue;
                if (hex.zone != null)
                    hex.zone.RemoveHexagon(hex);
                newZone.AddHexagon(hex, true);
            }

            newZone.CalcEdges();
        }

        // 在原有的区域内添加一个新的区域，两个区域在原有区域内通过扩散重新分配范围
        public void AddZone(Hexagon hex)
        {
            if (hex == null)
                return;

            var oldZone = hex.zone;
            if (oldZone == null)
                return;

            if (oldZone.landType == ZoneLandType.Land)
            {
                // EditorUtility.DisplayDialog("提示", "陆地区域不切割", "OK");
                Debug.LogError("陆地区域不切割");
                return;
            }

            oldZone.Reset();

            var zoneIdx = GetNewZoneId();
            var newZone = new Zone(this, zoneIdx, hex, oldZone.posType, oldZone.level,
                zoneIdx % zoneColors.Length);
            zones.Add(zoneIdx, newZone);

            var expands = new List<Zone>();
            expands.Add(oldZone);
            expands.Add(newZone);

            while (true)
            {
                delZones.Clear();
                for (var i = 0; i < expands.Count; i++)
                for (var j = 0; j < 2; j++)
                    if (!expands[i].Expand())
                    {
                        var count = expands[i].hexagons.Count;
                        if (minZoneHexCount < 0 || minZoneHexCount > count) minZoneHexCount = count;

                        if (maxZoneHexCount < count) maxZoneHexCount = count;

                        delZones.Add(expands[i]);
                        break;
                    }

                for (var i = 0; i < delZones.Count; i++) expands.Remove(delZones[i]);

                if (expands.Count == 0) break;
            }

            oldZone.CalcEdges();
            newZone.CalcEdges();

            MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateEvent, newZone.index);
            MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateEvent, oldZone.index);
        }

        public bool Del(Zone zone)
        {
            if (zone == null || !zones.ContainsKey(zone.index))
                return false;

            zones.Remove(zone.index);
            MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateEvent, zone.index);
            return true;
        }

        public void ResetZoneId(int startId)
        {
            var temps = new List<Zone>();
            foreach (var zone in zones.Values)
            {
                var newIdx = startId++;
                Debug.Log("zone " + zone.index + " => " + newIdx);
                zone.index = newIdx;
                zone.color = newIdx % zoneColors.Length;
                temps.Add(zone);
            }

            zones.Clear();
            foreach (var zone in temps) zones[zone.index] = zone;

            Debug.Log("重刷城点ID完成，数量: " + zones.Count);
        }

        // 刷新数据，去掉删除掉的区域，平滑区域边界，计算出区域边界
        public void CalcEdges(Action<string, float> onProgress = null)
        {
            for (var i = 0; i <= Zone.S_MAX_ZoneLV; i++)
            {
                zoneLvInfos[i].Reset();
                guanqiaLvInfos[i].Reset();
            }

            minZoneHexCount = int.MaxValue;
            maxZoneHexCount = 0;

            var toDels = new List<int>();
            foreach (var zone in zones.Values)
                if (zone.hexagons.Count <= 0)
                    toDels.Add(zone.index);

            foreach (var idx in toDels)
            {
                zones[idx].Destroy();
                zones.Remove(idx);
            }

            onProgress?.Invoke("CalcEdges ...", 0f);

            var count = 0;

            foreach (var zone in zones.Values)
            {
                zone.DoSmooth();

                if (onProgress != null)
                {
                    count++;
                    onProgress.Invoke($"DoSmooth ... {zone.index}",
                        0.5f * count / zones.Values.Count);
                }
            }

            count = 0;

            foreach (var zone in zones.Values)
            {
                zone.CalcEdges();

                if (onProgress != null)
                {
                    count++;
                    onProgress.Invoke($"CalcEdges ... {zone.index}",
                        0.5f + 0.5f * count / zones.Values.Count);
                }

                minZoneHexCount = Mathf.Min(minZoneHexCount, zone.hexagons.Count);
                maxZoneHexCount = Mathf.Max(maxZoneHexCount, zone.hexagons.Count);

                if (zone.isGuanqia == 1)
                    guanqiaLvInfos[zone.level].Add(zone);
                else if (zone.visible == 1)
                    zoneLvInfos[zone.level].Add(zone);
            }
        }

        /// <summary>
        ///     计算商圈边界
        /// </summary>
        /// <param name="tolerance"></param>
        /// <param name="edgeWidth"></param>
        /// <param name="onProgress"></param>
        public void CalcBusinessEdges(float tolerance = 2f, float edgeWidth = 2f,
            Action<string, float> onProgress = null)
        {
            onProgress?.Invoke("CalcBusinessEdges ...", 0f);

            var repeatWork = new Dictionary<int, bool>(zones.Count);
            var count = 0;
            businessZoneEdgeWayPoints.Clear();
            foreach (var kv in businesses)
            {
                var businessid = kv.Key;
                var business = kv.Value;

                if (onProgress != null)
                {
                    count++;
                    onProgress.Invoke($"CalcBusinessEdges ... 商圈:{businessid}",
                        0.5f * count / zones.Values.Count);
                }

                foreach (var zone in business.Zones)
                {
                    repeatWork.Add(zone.index, true);

                    var lastzone = -1;
                    List<Vector3> lastWayPoints = null;
                    foreach (var edgeHexagon in zone.edges)
                        for (var i = 0; i < 6; i++)
                        {
                            var neighbour = edgeHexagon.neigbours[i];
                            if (neighbour != null &&
                                neighbour.zone != null &&
                                neighbour.zone != zone &&
                                !repeatWork.ContainsKey(neighbour.zone.index) &&
                                neighbour.zone.businessZone != businessid)
                            {
                                List<Vector3> z1z2WayPoints;
                                // if (neighbour.zone.index == lastzone)
                                // {
                                // 	z1z2WayPoints = lastWayPoints;
                                // }
                                // else
                                {
                                    var b1_b2 = businessid > neighbour.zone.businessZone
                                        ? (neighbour.zone.businessZone, businessid)
                                        : (businessid, neighbour.zone.businessZone);
                                    if (!businessZoneEdgeWayPoints.TryGetValue(b1_b2,
                                            out var b1b2WayPoints))
                                    {
                                        b1b2WayPoints = new Dictionary<(int, int), List<Vector3>>();
                                        businessZoneEdgeWayPoints.Add(b1_b2, b1b2WayPoints);
                                    }

                                    var z1_z2 = zone.index > neighbour.zone.index
                                        ? (neighbour.zone.index, zone.index)
                                        : (zone.index, neighbour.zone.index);
                                    if (!b1b2WayPoints.TryGetValue(z1_z2, out z1z2WayPoints))
                                    {
                                        z1z2WayPoints = new List<Vector3>();
                                        b1b2WayPoints.Add(z1_z2, z1z2WayPoints);
                                    }

                                    lastWayPoints = z1z2WayPoints;
                                    lastzone = neighbour.zone.index;
                                }

                                var waypoint = GetBetweenPoint(neighbour.Pos, edgeHexagon.Pos);
                                z1z2WayPoints.Add(waypoint);
                            }
                        }
                }
            }

            // BusinessConnectEdgePoints1(onProgress);
            BusinessConnectEdgePoints2(tolerance, edgeWidth, onProgress);
            // TestBusinessEdge();
        }

        private GameObject _root;

        private void TestBusinessEdge()
        {
            if (_root != null)
                Object.Destroy(_root);

            _root = new GameObject("root");
            var _rootTrans = _root.transform;
            // foreach (var points in businessEdgeWayPoints.Values)
            // {
            // 	int count = 0;
            // 	foreach (var p in points)
            // 	{
            // 		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // 		cube.name = count.ToString();
            // 		cube.transform.parent = _rootTrans;
            // 		cube.transform.position = p;
            // 		count++;
            // 	}
            // 	break;
            // }

            foreach (var edgeinfo in businessEdgeMeshes)
            {
                var go = new GameObject($"{edgeinfo.Key.Item1}-{edgeinfo.Key.Item2}");
                go.transform.parent = _rootTrans;
                go.AddComponent<MeshFilter>().mesh = edgeinfo.Value;
                go.AddComponent<MeshRenderer>().sharedMaterial =
                    new Material(Shader.Find("UWUnlit/Opaque"));
            }
        }

        private void BusinessConnectEdgePoints1(Action<string, float> onProgress = null)
        {
            var count = 0;
            businessEdgeWayPoints.Clear();
            foreach (var b1b2 in businessZoneEdgeWayPoints)
            {
                var b1b2_key = b1b2.Key;
                if (onProgress != null)
                {
                    count++;
                    onProgress.Invoke($"CalcBusinessEdges ... 商圈:{b1b2_key}",
                        0.5f + 0.5f * count / businessZoneEdgeWayPoints.Count);
                }

                var b1b2WayPoints = b1b2.Value;
                var startkey = b1b2WayPoints.Keys.First();
                var startlist = b1b2WayPoints.Values.First();
                var startpos1 = startlist.First();
                var endpos1 = startlist.Last();

                var sortZ1Z2 = new List<(int, int)>(); // 排序
                var reverseZ1Z2 = new List<bool>(); // 是否反转
                sortZ1Z2.Add(startkey);
                reverseZ1Z2.Add(false);

                var checkCount = 0;
                while (sortZ1Z2.Count < b1b2WayPoints.Count)
                {
                    foreach (var z1z2 in b1b2WayPoints)
                    {
                        if (sortZ1Z2.Contains(z1z2.Key)) continue;
                        var startpos2 = z1z2.Value.First();
                        var endpos2 = z1z2.Value.Last();
                        var finded = false;
                        var reverse = false;
                        // 1.end1 - start2
                        if (Vector3.Distance(endpos1, startpos2) < 1.1)
                        {
                            sortZ1Z2.Add(z1z2.Key);
                            reverseZ1Z2.Add(false);
                            endpos1 = endpos2;
                            break;
                        }
                        // 2.end1 - end2

                        if (Vector3.Distance(endpos1, endpos2) < 1.1)
                        {
                            sortZ1Z2.Add(z1z2.Key);
                            reverseZ1Z2.Add(true);
                            endpos1 = startpos2;
                            break;
                        }
                        // 3.start1 - end2

                        if (Vector3.Distance(startpos1, endpos2) < 1.1)
                        {
                            sortZ1Z2.Insert(0, z1z2.Key);
                            reverseZ1Z2.Add(false);
                            startpos1 = startpos2;
                            break;
                        }
                        // 4.start1 - start2

                        if (Vector3.Distance(startpos1, startpos2) < 1.1)
                        {
                            sortZ1Z2.Insert(0, z1z2.Key);
                            reverseZ1Z2.Add(true);
                            startpos1 = endpos2;
                            break;
                        }
                    }

                    if (checkCount != sortZ1Z2.Count)
                    {
                        checkCount = sortZ1Z2.Count;
                    }
                    else
                    {
                        Debug.LogError($"{b1b2_key}边线计算错误, 没有连通");
                        break;
                    }
                }

                var waypoints = new List<Vector3>();
                for (var i = 0; i < sortZ1Z2.Count; i++)
                {
                    var key = sortZ1Z2[i];
                    var reverse = reverseZ1Z2[i];

                    var points = b1b2WayPoints[key];

                    if (reverse) points.Reverse();

                    waypoints.AddRange(points);
                }

                businessEdgeWayPoints.Add(b1b2_key, waypoints);
            }
        }

        private void BusinessConnectEdgePoints2(float tolerance = 2f, float edgeWidth = 2f,
            Action<string, float> onProgress = null)
        {
            var count = 0;
            var _builder = new StringBuilder();
            businessEdgeWayPoints.Clear();
            businessEdgeMeshes.Clear();
            foreach (var b1b2 in businessZoneEdgeWayPoints)
            {
                var name = b1b2.Key;
                if (onProgress != null)
                {
                    count++;
                    onProgress.Invoke($"CalcBusinessEdges ... 商圈:{name}",
                        0.5f + 0.5f * count / businessZoneEdgeWayPoints.Count);
                }

                var waypoints = new List<Vector3>();
                var b1b2WayPoints = b1b2.Value;

                foreach (var pts in b1b2WayPoints.Values) waypoints.AddRange(pts);

                waypoints = CurveHelper.ConnectWaypoints(waypoints);
                waypoints = CurveHelper.SimplifyRoute(waypoints, tolerance);
                businessEdgeWayPoints.Add(name, waypoints);
                businessEdgeMeshes.Add(name, CurveHelper.GenerateMesh(waypoints, edgeWidth));
            }
        }

        public void CheckEnclave()
        {
            EditorUtility.DisplayProgressBar("检查飞地", "", 0f);

            var count = 0;
            var unit = 1f / zones.Values.Count;
            foreach (var zone in zones.Values)
            {
                count++;
                var progress = (float)count / zones.Values.Count;
                if (zone.CheckEnclave((msg, value) =>
                    {
                        EditorUtility.DisplayProgressBar("检查飞地", $"zone {zone.index}",
                            progress + +value * unit);
                    }))
                    break;
            }

            EditorUtility.ClearProgressBar();

            if (count >= zones.Values.Count) EditorUtility.DisplayDialog("检查飞地", "未发现飞地!", "确定");
        }

        private void GetRuinZones(Zone start, ref List<Zone> zones)
        {
            if (start.ruinId <= 0)
                return;

            if (zones.Contains(start))
                return;

            zones.Add(start);
            foreach (var zone in start.neigbourZones)
                if (zone.ruinId == start.ruinId)
                    GetRuinZones(zone, ref zones);
        }

        // 更新遗迹的数据
        public void UpdateRuins()
        {
            ruinDatas.Clear();
            ruinZones.Clear();

            foreach (var zone in zones.Values)
            {
                if (zone.ruinId <= 0) continue;
                if (ruinZones.TryGetValue(zone.ruinId, out var list))
                {
                    list.Add(zone);
                }
                else
                {
                    list = new List<Zone>();
                    list.Add(zone);
                    ruinZones[zone.ruinId] = list;
                }
            }

            foreach (var v in ruinZones) ruinDatas[v.Key] = new RuinData(v.Key, v.Value);
        }

        // 检查遗迹是否符合规范，遗迹编号非零且相同的zone必须相邻。
        public bool CheckRuins()
        {
            UpdateRuins();

            var rets = new List<Zone>();
            var hexs = new List<Hexagon>();

            foreach (var v in ruinZones)
            {
                if (v.Value.Count == 0)
                    continue;

                rets.Clear();
                GetRuinZones(v.Value[0], ref rets);
                if (rets.Count != v.Value.Count)
                {
                    Zone outZone = null;
                    foreach (var t in v.Value)
                        if (!rets.Contains(t))
                        {
                            outZone = t;
                            break;
                        }

                    if (EditorUtility.DisplayDialog("遗迹", $"{v.Key}号遗迹包含的城点有飞地！！！", "前往"))
                        if (outZone != null && outZone.hexagon != null)
                            MapRender.instance.FocusHexagon(outZone.hexagon);

                    return false;
                }
            }

            return true;
        }

        public bool CheckMapValid(Action<string, float> onProgress = null)
        {
            try
            {
                // 小区域检查

                // 空格子检查
                SewingHexagon(onProgress);
                CalcEdges(onProgress);
                CalcBusinessEdges(businessEdgeTolerance, businessEdgeWidth, onProgress);
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("检查地图", $"边界数据计算异常:{e},", "确定");
                //MapRender.instance.ShowMessage($"边界数据计算异常:{e} ！！！");
                return false;
            }

            if (!CheckRuins())
                return false;

            return true;
        }

        private readonly List<Zone> adjustmentZonesList = new();

        //获得需要调整哨塔的Zone列表
        public List<Zone> GetAdjustmentZonesList()
        {
            return adjustmentZonesList;
        }

        public void ClearAdjustmentZonesList()
        {
            adjustmentZonesList.Clear();
        }

        public void RemoveAdjustmentZone(Zone zone)
        {
            adjustmentZonesList.Remove(zone);
        }

        public void AddAdjustmentZone(Zone zone)
        {
            if (adjustmentZonesList.Contains(zone) == false) adjustmentZonesList.Add(zone);
        }

        #region 航道点

        public void AddSailPoint(int index)
        {
            if (IsIdxValid(index) == false)
                return;

            if (connect.ContainsKey(index) == false) connect.Add(index, new HashSet<int>());
        }

        public void RemoveSailPoint(int index)
        {
            if (connect.ContainsKey(index))
                foreach (var idx in connect[index])
                    if (connect.ContainsKey(idx))
                    {
                        var hSet = connect[idx];
                        hSet.Remove(index);
                    }

            connect.Remove(index);
        }

        public void ReplaceSailPoint(int oldIndex, int newIndex)
        {
            AddSailPoint(newIndex);

            if (connect.ContainsKey(oldIndex))
            {
                var oSet = connect[oldIndex];
                var nSet = connect[newIndex];
                foreach (var idx in oSet)
                    if (nSet.Contains(idx) == false)
                    {
                        nSet.Add(idx);

                        var hSet = connect[idx];
                        if (hSet.Contains(newIndex) == false) hSet.Add(newIndex);
                    }

                RemoveSailPoint(oldIndex);
            }
        }

        private bool IsIdxValid(int idx)
        {
            var x = idx % width;
            var y = idx / width;
            return 0 <= x && x < width && 0 <= y && y < height;
        }

        public void AddConnect(int src, int x, int y)
        {
            var index = y * width + x;
            AddConnect(src, index);
        }

        public void AddConnect(int src, int dst)
        {
            AddSailPoint(src);

            if (IsIdxValid(dst) == false)
                return;

            if (hexagons[dst] != null && hexagons[dst].attribute == 0) return;

            if (connect[src].Contains(dst) == false)
            {
                connect[src].Add(dst);
                connect[dst].Add(src);
            }
        }

        public void RemoveConnect(int src, int dst)
        {
            if (connect[src].Contains(dst))
            {
                connect[src].Remove(dst);
                connect[dst].Remove(src);
            }
        }

        public void WriteSailData(BinaryWriter bw)
        {
            bw.Write(connect.Count);
            foreach (var key in connect.Keys)
            {
                bw.Write(key);

                var hashSet = connect[key];
                bw.Write(hashSet.Count);
                foreach (var idx in hashSet) bw.Write(idx);
            }
        }

        public void ReadSailData(BinaryReader br)
        {
            connect.Clear();
            var count = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = br.ReadInt32();
                var hashSet = new HashSet<int>();
                connect.Add(key, hashSet);

                if (hexagons[key] != null) hexagons[key].attribute = HexAttribute.SailPoint;

                var setCount = br.ReadInt32();
                for (var j = 0; j < setCount; j++)
                {
                    var idx = br.ReadInt32();
                    hashSet.Add(idx);
                }
            }
        }

        //导出航道点 num q r index
        public void ExportSailPoint2Server(BinaryWriter bw)
        {
            var num = connect.Count;
            bw.Write(num);
            //格式 q r index
            foreach (var idx in connect.Keys)
            {
                var cube = GetCube(idx);
                bw.Write(cube.x);
                bw.Write(cube.z);
                bw.Write(idx);
            }
        }

        //导出连接关系 count index num [index ... index] -1|0|1
        //offset x超出1680视为跨地图边界连接
        public void ExportSailConnect2Server(BinaryWriter bw)
        {
            var collect = new Dictionary<int, Dictionary<int, HashSet<int>>>();
            foreach (var kv in connect)
            {
                var list = new Dictionary<int, HashSet<int>>();
                collect.Add(kv.Key, list);

                var negativeHashSet = new HashSet<int>();
                var positiveHashSet = new HashSet<int>();
                var zeroHashSet = new HashSet<int>();
                list.Add(-1, negativeHashSet);
                list.Add(+0, zeroHashSet);
                list.Add(+1, positiveHashSet);

                var src = GetOffset(kv.Key);
                foreach (var idx in kv.Value)
                {
                    var dst = GetOffset(idx);
                    if (Math.Abs(src.x - dst.x) > 1680)
                    {
                        if (src.x < dst.x)
                            negativeHashSet.Add(idx);
                        else
                            positiveHashSet.Add(idx);
                    }
                    else
                    {
                        zeroHashSet.Add(idx);
                    }
                }
            }

            var num = 0;
            foreach (var dic in collect.Values)
            {
                num += dic[0].Count > 0 ? 1 : 0;
                num += dic[-1].Count > 0 ? 1 : 0;
                num += dic[1].Count > 0 ? 1 : 0;
            }

            bw.Write(num);

            foreach (var kv in collect)
                for (var i = -1; i <= 1; i++)
                {
                    var hashSet = kv.Value[i];
                    if (hashSet.Count > 0)
                    {
                        bw.Write(kv.Key);
                        bw.Write(hashSet.Count);
                        foreach (var idx in hashSet) bw.Write(idx);

                        bw.Write(i);
                    }
                }
        }

        #endregion

        #region 岛屿模板

        public void AddIsland(IslandTemplate island)
        {
            if (islandTemplates.ContainsKey(island._Id) == false)
                islandTemplates.Add(island._Id, island);
        }

        public void RemoveIsland(int id)
        {
            if (islandTemplates.ContainsKey(id)) islandTemplates.Remove(id);
        }

        public void ReadIslandData(BinaryReader br)
        {
            var count = br.ReadInt32();

            if (count == 0) return;
            islandTemplates.Clear();

            for (var i = 0; i < count; i++)
            {
                var island = new IslandTemplate();
                island.Load(br);
                AddIsland(island);
            }
        }

        public void WriteIslandData(BinaryWriter bw)
        {
            var count = islandTemplates.Count;
            bw.Write(count);
            foreach (var kv in islandTemplates) kv.Value.Save(bw);
        }

        public void AssignIsland()
        {
            foreach (var zone in zones.Values) zone.AssignIslandAfterLoad();
        }

        #endregion

        public void Save(BinaryWriter bw, int mapId = -1)
        {
            // 2.增加movetypemark
            // 3.删除movetypemark
            // 4.区域增加商圈信息
            // 5.保存阻挡图信息
            // 6.商圈信息
            // 7.宝藏点
            // 8.相机参数
            // 9.相机滑动区域，自定义多边形
            // 10.迷雾
            // 11.迷雾Mesh
            ver = 11;
            bw.Write(ver);
            bw.Write(width);
            bw.Write(height);
            bw.Write(Hex.HexRadius);
            bw.Write((int)mapProgress);

            bw.Write((int)mapType);
            if (mapType == MapType.ESmallWorld) WriteSmallMapData(bw);

            // Colors
            bw.Write(zoneColors.Length);
            for (var i = 0; i < zoneColors.Length; i++)
            {
                var c = zoneColors[i];
                bw.Write(c.r);
                bw.Write(c.g);
                bw.Write(c.b);
            }

            bw.Write(frameColors.Length);
            for (var i = 0; i < frameColors.Length; i++)
            {
                var c = frameColors[i];
                bw.Write(c.r);
                bw.Write(c.g);
                bw.Write(c.b);
            }

            // 区域的数量
            bw.Write(zones.Count);
            foreach (var zone in zones.Values) zone.Save(bw);

            bw.Write(hexagons.Length);

            var lines = GetZoneLines();
            bw.Write(lines.Count);

            for (var i = 0; i < lines.Count; i++)
            {
                bw.Write(lines[i].begin);
                bw.Write(lines[i].len);
                bw.Write((short)lines[i].idx);
            }

            /*
            // 按位导出阻挡信息
            var need = (hexagons.Length / 8) + 1;
            bw.Write(need);
            byte[] bytes = new byte[need];
            int idx = 0;
            int x, y;
            for (int i = 0; i < need; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    idx = i * 8 + j;
                    if (idx >= hexagons.Length) break;
                    x = idx % width;
                    y = idx / width;
                    //var color = blockData.GetPixel(x, height - y);
                    //if (color.r == 1)
                    if (IsBlock(x, y))
                    {
                        bytes[i] |= (byte) (1 << j);
                    }
                }

                if (idx >= hexagons.Length) break;
            }

            bw.Write(bytes, 0, need);
            */

            // hexagon block flag
            bw.Write(hexagons.Length);
            for (var i = 0; i < hexagons.Length; i++) bw.Write((int)hexagons[i].blockFlag);

            // hexagon movetype
            bw.Write(hexagons.Length);
            for (var i = 0; i < hexagons.Length; i++) bw.Write((int)hexagons[i].movetype);

            //渔场信息
            WriteFisheriesData(bw);

            //航线数据
            WriteSailData(bw);

            //岛屿模板数据
            WriteIslandData(bw);

            if (blockData != null)
            {
                var blockBytes = blockData.EncodeToPNG();
                bw.Write(blockBytes.Length);
                bw.Write(blockBytes);
            }
            else
            {
                bw.Write(0);
            }

            WriteBusinessDat(bw);
            WriteCameraParams(bw);
            WriteInteractionPoint(bw);
            WriteFogOfWar(bw, mapId);

            // SaveSeaControlToPicture(true);
            Debug.Log("Save Map finished!!");
        }

        private void WriteBusinessDat(BinaryWriter bw)
        {
            bw.Write(businesses.Count);
            foreach (var business in businesses.Values) business.SaveData(bw);
        }

        private void WriteInteractionPoint(BinaryWriter bw)
        {
            // bw.Write(interactionPoints.Count);
            // foreach (var point in interactionPoints)
            // {
            // 	bw.Write(point.id);
            // 	bw.Write(point.type);
            // 	bw.Write(point.pos.x);
            // 	bw.Write(point.pos.y);
            // 	bw.Write(point.pos.z);
            // }
        }

        private void WriteFogOfWar(BinaryWriter bw, int mapId = -1)
        {
            var allFogInfos = MapRender.instance.CampaignFogInfos;
            List<CampaignFogInfo> mapFogInfos = null;
            if (mapId > -1 && allFogInfos.ContainsKey(mapId))
            {
                mapFogInfos = allFogInfos[mapId];
            }
            fogOfWarDataMan.Save(bw, mapFogInfos);
            bw.Write(hexagons.Length);
            for (var i = 0; i < hexagons.Length; i++)
                bw.Write(hexagons[i].fogOfWarId);
        }

        private void WriteCameraParams(BinaryWriter bw)
        {
            // 保存相机滑动区域
            bw.Write(cameraBoundary.Count);
            for (int i = 0; i < cameraBoundary.Count; i++)
            {
                bw.Write(cameraBoundary[i].x);
                bw.Write(cameraBoundary[i].y);
                bw.Write(cameraBoundary[i].z);
            }

            // 保存相机区域
            var camera = Camera.main;
            if (camera)
            {
                bw.Write(camera.transform.position.x);
                bw.Write(camera.transform.position.y);
                bw.Write(camera.transform.position.z);
                bw.Write(camera.transform.rotation.eulerAngles.x);
                bw.Write(camera.transform.rotation.eulerAngles.y);
                bw.Write(camera.transform.rotation.eulerAngles.z);
                bw.Write(camera.fieldOfView);
                bw.Write(camera.nearClipPlane);
                bw.Write(camera.farClipPlane);

                cameraSetting.position = camera.transform.position;
                cameraSetting.rotation = camera.transform.rotation.eulerAngles;
                cameraSetting.fieldOfView = camera.fieldOfView;
                cameraSetting.nearClipPlane = camera.nearClipPlane;
                cameraSetting.farClipPlane = camera.farClipPlane;
            }
        }

        //加载策划配置的地图文件数据
        public bool Load(BinaryReader br, Action<string, float> onProgress = null, int mapId = -1)
        {
            onProgress?.Invoke("打开文件", 0f);

            var fileVer = br.ReadInt32();
            width = br.ReadInt32();
            height = br.ReadInt32();
            Hex.HexRadius = br.ReadSingle();
            Hex.CenterUnit = new Vector3(Hex.HexRadius * Hex.Sqrt3 * 0.5f, 0, Hex.HexRadius);
            mapProgress = (MapProgress)br.ReadInt32();

            //地图类型
            mapType = (MapType)br.ReadInt32();
            if (mapType == MapType.ESmallWorld)
            {
                var startIndex = br.ReadInt32();
                var startHexPosX = startIndex % width;
                var startHexPosY = startIndex / height;
                startHexPos.x = startHexPosX;
                startHexPos.y = startHexPosY;

                var endIndex = br.ReadInt32();
                var endHexPosX = endIndex % width;
                var endHexPosY = endIndex / height;
                endHexPos.x = endHexPosX;
                endHexPos.y = endHexPosY;
            }

            //Colors
            var count = br.ReadInt32();
            zoneColors = new Color[count];
            for (var i = 0; i < count; i++)
                zoneColors[i] = new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            onProgress?.Invoke("Zone Color", 0.1f);

            count = br.ReadInt32();
            frameColors = new Color[count];
            for (var i = 0; i < count; i++)
                frameColors[i] = new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            onProgress?.Invoke("Zone frame Color", 0.2f);

            CreateBase(width, height);

            onProgress?.Invoke("Create map Base", 0.4f);

            zones.Clear();
            landzones.Clear();
            seazones.Clear();
            count = br.ReadInt32();

            onProgress?.Invoke("Load zone ...", 0.5f);

            for (var i = 0; i < count; i++)
            {
                var zone = new Zone(this, -1, null, 0, 0, 0);
                zone.Load(br, fileVer);
                if (zones.TryGetValue(zone.index, out var zz))
                {
                    Debug.LogError($"index:{zone.index} {zone.hexagons.Count}");
                    Debug.LogError(
                        $"index:{zz.index} {zz.hexagons.Count} "); //(zz.hexagons[0].x},{zz.hexagons[0].y})
                    continue;
                }

                if (zone.index > -1)
                    zones.Add(zone.index, zone);

                if (zone.landType == ZoneLandType.Land)
                    landzones.Add(zone.index, zone);
                else if (zone.landType == ZoneLandType.Sea)
                    seazones.Add(zone.index, zone);
            }

            onProgress?.Invoke("Load Hexagon ...", 0.6f);

            count = br.ReadInt32(); // hexagons count
            count = br.ReadInt32(); // lines count

            for (var i = 0; i < count; i++)
            {
                var start = br.ReadInt32();
                int len = br.ReadInt16();
                int idx = br.ReadInt16();

                if (idx >= 0 && zones.ContainsKey(idx))
                    for (var j = 0; j < len; j++)
                        zones[idx].AddHexagon(hexagons[start + j], true);
            }

            // if (fileVer > 1)
            // {
            // 	int len = br.ReadInt32();
            // 	blockBuff = br.ReadBytes(len);
            // }

            // hexagon block flag
            count = br.ReadInt32(); // blockflag count = hexagon count
            for (var i = 0; i < count; i++) hexagons[i].blockFlag = (BlockFlag)br.ReadInt32();

            // movetype
            count = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                hexagons[i].movetype = (MOVETYPE)br.ReadInt32();
                if (fileVer == 2)
                    hexagons[i].movetypeMark = br.ReadInt32();
                else
                    needResetMovetypeMark = true;
            }

            ReadFisheriesData(br);
            ReadSailData(br);
            ReadIslandData(br);

            AssignIsland();

            ReadBlockData(br, fileVer);
            ReadBusinessData(br, fileVer);
            ReadCameraParams(br, fileVer);
            var allFogInfos = MapRender.instance.CampaignFogInfos;
            List<CampaignFogInfo> mapFogInfos = null;
            if (mapId > -1 && allFogInfos.ContainsKey(mapId))
            {
                mapFogInfos = allFogInfos[mapId];
            }
            ReadFogOfWarData(br, fileVer, mapFogInfos);

            onProgress?.Invoke("Check map valid ...", 0.8f);

            if (mapProgress == MapProgress.None)
                mapProgress = seazones.Count > 0 ? MapProgress.SeaZones : MapProgress.Create;

            PostprocessData();

            if (!CheckMapValid((msg, value) => { onProgress?.Invoke(msg, 0.8f + 0.2f * value); }))
            {
                Debug.Log("Load Map failed !!!");
                return false;
            }


            MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateAllEvent);
            Debug.Log("Load Map finished !!!");
            return true;
        }

        private void ReadCameraParams(BinaryReader br, int fileVer)
        {
            cameraBoundary.Clear();
            if (fileVer >= 9)
            {
                var count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    cameraBoundary.Add(new Vector3Int(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                }
            }

            if (fileVer >= 8)
            {
                var camera = Camera.main;
                if (camera)
                {
                    camera.transform.position =
                        new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    camera.transform.rotation = Quaternion.Euler(br.ReadSingle(), br.ReadSingle(),
                        br.ReadSingle());
                    camera.fieldOfView = br.ReadSingle();
                    camera.nearClipPlane = br.ReadSingle();
                    camera.farClipPlane = br.ReadSingle();
                    cameraSetting.position = camera.transform.position;
                    cameraSetting.rotation = camera.transform.rotation.eulerAngles;
                    cameraSetting.fieldOfView = camera.fieldOfView;
                    cameraSetting.nearClipPlane = camera.nearClipPlane;
                    cameraSetting.farClipPlane = camera.farClipPlane;
                }
            }
        }

        private void ReadBusinessData(BinaryReader br, int fileVer)
        {
            if (fileVer >= 6)
            {
                businesses.Clear();
                var count = br.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var business = new Business();
                    business.LoadData(br);
                    businesses.Add(business.ID, business);
                }

                foreach (var zone in zones.Values)
                    if (zone.businessZone > -1)
                    {
                        if (!businesses.TryGetValue(zone.businessZone, out var business))
                            Debug.LogError("商圈数据有误");

                        business.Zones.Add(zone);
                    }
            }
            else
            {
                CalcBusiness();
            }

            businesses = businesses.OrderBy(item => item.Key)
                .ToDictionary(o => o.Key, p => p.Value);
        }

        private void ReadBlockData(BinaryReader br, int fileVer)
        {
            if (fileVer >= 5)
            {
                var count = br.ReadInt32();
                if (count > 0)
                {
                    blockData = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                    blockData.filterMode = FilterMode.Point;
                    blockData.LoadImage(br.ReadBytes(count));
                    ResetBlock(blockData);
                }
            }
        }

        private void ReadFogOfWarData(BinaryReader br, int fileVer, List<CampaignFogInfo> fogInfos = null)
        {
            if (fileVer >= 10)
            {
                fogOfWarDataMan.Load(br, fileVer, fogInfos);
                var count = br.ReadInt32();
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        hexagons[i].fogOfWarId = br.ReadInt32();
                    }
                }
            }
        }

        /// <summary>
        ///     加载完地图数据，处理一下
        /// </summary>
        private void PostprocessData()
        {
            SetMapSetting();
            // 水设置 TODO
            // SetWater();
        }

        private void SetMapSetting()
        {
            MapRender.instance.UpdateMapRect(mapSizeW, mapSizeH);
        }

        private void SetWater()
        {
            // 1.获取海水分布图，没有新建
            // 2.设置tilling offset
            var tex = WaterDistributeDefault;
            if (File.Exists(WaterDistributeTexPath))
                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(WaterDistributeTexPath);

            MapRender.instance.UpdateWater(tex, mapSizeW, mapSizeH);
        }

        private void CalcBusiness()
        {
            businesses.Clear();
            foreach (var zone in zones.Values)
                if (zone.businessZone > -1)
                {
                    if (!businesses.TryGetValue(zone.businessZone, out var business))
                    {
                        business = new Business();
                        business.ID = zone.businessZone;
                        businesses.Add(zone.businessZone, business);
                    }

                    business.Zones.Add(zone);
                }
        }

        public bool LoadFromJson(string fileName)
        {
            InitColors();

            var hexMapSave = JsonUtility.FromJson<HexMapSave>(File.ReadAllText(fileName));

            width = 1201;
            height = 1201;

            CreateBase(blockData, width, height);

            zones.Clear();
            for (var i = 0; i < hexMapSave.zones.Length; i++)
            {
                var zone = new Zone(this, -1, null, 0, 0, 0);
                zone.index = hexMapSave.zones[i].index;
                zone.color = zone.index % zoneColors.Length;
                ;
                zone.level = hexMapSave.zones[i].level;
                zone.hexagon = GetHexagon(hexMapSave.zones[i].pos);
                zone.posType = hexMapSave.zones[i].posType;
                zones.Add(zone.index, zone);
            }

            for (var i = 0; i < hexMapSave.hexagons.Length; i++)
                zones[hexMapSave.hexagons[i].zone]
                    .AddHexagon(hexagons[hexMapSave.hexagons[i].index], true);

            CalcEdges();
            Debug.Log("Load Json Map finished!!");

            return true;
        }

        // public bool LoadFromServerData(BinaryReader br)
        // {
        //     int fileVer = br.ReadInt32();
        //     width = br.ReadInt32();
        //     height = br.ReadInt32();
        //
        //     InitColors();
        //     CreateBase(blockData, width, height);
        //
        //     zones.Clear();
        //     int count = br.ReadInt32();
        //     for (int i = 0; i < count; i++)
        //     {
        //         var zone = new Zone(this, -1, null, 0, 0, 0);
        //         zone.index = br.ReadInt16();
        //         zone.level = br.ReadInt32();
        //         zone.posType = br.ReadInt32();
        //         zone.hexagon = GetHexagon(br.ReadInt32());
        //         zone.visible = br.ReadInt32();
        //         zones.Add(zone.index, zone);
        //         int num = br.ReadInt32();
        //         for(int j = 0; j<num; j++)
        //         {
        //             var t = br.ReadInt16();
        //         }
        //     }
        //
        //     count = br.ReadInt32(); // hexagons count
        //     if (count != width * height)
        //     {
        //         EditorUtility.DisplayDialog("地图", $"地格数据异常: {count} != {width}x{height}", "确定");
        //         Debug.Log($"Load Map hexagon count err : {count}!!");
        //         return false;
        //     }
        //
        //     int visible = 1 << 15;
        //     for (int i=0; i<count; i++)
        //     {
        //         int val = br.ReadInt16();
        //         val &= ~visible;
        //         val = (short)val;
        //         if (val >= 0 && val < zones.Count)
        //         {
        //             zones[val].AddHexagon(hexagons[i], true);
        //         }
        //     }
        //
        //     CalcEdges();
        //
        //     Debug.Log($"Load Map finished!!");
        //     bInited = true;
        //     return true;
        // }

        // 导出给服务器

        //short 取值范围32767
        private const short maxShort = 32766;

        private List<ZoneLineInfo> GetZoneLines()
        {
            var cur = 0;
            var start = cur;
            var zoneIdx = hexagons[cur].zone != null ? hexagons[cur].zone.index : -1;
            var count = 0;

            var rets = new List<ZoneLineInfo>();
            while (true)
            {
                cur++;
                var val = hexagons[cur].zone != null ? hexagons[cur].zone.index : -1;

                count = cur - start;
                //不相同的zone，记录 或者 超过short最大值分段
                if (val != zoneIdx || count > maxShort)
                {
                    rets.Add(new ZoneLineInfo
                    {
                        begin = start,
                        len = (short)count,
                        idx = zoneIdx
                    });

                    zoneIdx = val;
                    start = cur;
                }

                if (cur >= hexagons.Length - 1)
                {
                    count = cur - start + 1;
                    rets.Add(new ZoneLineInfo
                    {
                        begin = start,
                        len = (short)count,
                        idx = zoneIdx
                    });
                    break;
                }
            }

            return rets;
        }

        private List<ZoneLineInfo> GetMovetypeLines()
        {
            var cur = 0;
            var start = cur;
            var movetype = GetMoveType(cur); //hexagons[cur].movetype;
            var count = 0;

            var rets = new List<ZoneLineInfo>();
            while (true)
            {
                cur++;
                var val = GetMoveType(hexagons[cur]);

                count = cur - start;
                //不相同的movetype，记录 或者 超过short最大值分段
                if (val != movetype || count > maxShort)
                {
                    rets.Add(new ZoneLineInfo
                    {
                        begin = start,
                        len = (short)count,
                        idx = (int)movetype
                    });

                    movetype = val;
                    start = cur;
                }

                if (cur >= hexagons.Length - 1)
                {
                    count = cur - start + 1;
                    rets.Add(new ZoneLineInfo
                    {
                        begin = start,
                        len = (short)count,
                        idx = (int)movetype
                    });
                    break;
                }
            }

            return rets;
        }

        // 线段式存储，获取fogof war数据，如果是Defaultfogid的不记录
        public Dictionary<int, List<ZoneLineInfo>> GetFogOfWarLines()
        {
            var cur = 0;
            var start = cur;
            var fogId = hexagons[cur].fogOfWarId;
            var count = 0;

            var rets = new List<ZoneLineInfo>();
            while (true)
            {
                cur++;
                var val = hexagons[cur].fogOfWarId;

                count = cur - start;
                //不相同的fogofwarid，记录 或者 超过short最大值分段
                if (val != fogId || count > maxShort)
                {
                    if (fogId != FogOfWarDataMan.DefaultFogOfWarId)
                    {
                        rets.Add(new ZoneLineInfo
                        {
                            begin = start,
                            len = (short)count,
                            idx = fogId
                        });
                    }

                    fogId = val;
                    start = cur;
                }

                if (cur >= hexagons.Length - 1)
                {
                    count = cur - start + 1;
                    if (fogId != FogOfWarDataMan.DefaultFogOfWarId)
                    {
                        rets.Add(new ZoneLineInfo
                        {
                            begin = start,
                            len = (short)count,
                            idx = fogId
                        });
                    }
                    break;
                }
            }

            //rets中同一个fogId的线段合并 dictionary<fogID, <start, len>>
            var mergedRets = new Dictionary<int, List<ZoneLineInfo>>();
            foreach (var line in rets)
            {
                if (!mergedRets.TryGetValue(line.idx, out var list))
                {
                    list = new List<ZoneLineInfo>();
                    mergedRets.Add(line.idx, list);
                }

                list.Add(line);
            }


            return mergedRets;
        }

        //写入小地图数据
        public void WriteSmallMapData(BinaryWriter bw)
        {
            //start index
            var startIndex = startHexPos.x + startHexPos.y * width;
            //end index
            var endIndex = endHexPos.x + endHexPos.y * width;
            bw.Write(startIndex);
            bw.Write(endIndex);
        }

        //写入渔场数据
        public void WriteFisheriesData(BinaryWriter bw)
        {
            bw.Write(fisheryDic.Count);
            foreach (var hexagon in fisheryDic.Keys)
            {
                bw.Write(fisheryDic[hexagon].fishIndex);
                var seatIndexList = fisheryDic[hexagon].seatIndexList;
                bw.Write(seatIndexList.Count);
                for (var i = 0; i < seatIndexList.Count; i++) bw.Write(seatIndexList[i]);
            }
        }

        public void ReadFisheriesData(BinaryReader br)
        {
            fisheryDic.Clear();
            fishHexgaons.Clear();
            var fisheryCount = br.ReadInt32();
            for (var i = 0; i < fisheryCount; i++)
            {
                var fishIndex = br.ReadInt32();
                var fishery = new Fishery();
                fishery.fishIndex = fishIndex;
                var seatCount = br.ReadInt32();
                for (var j = 0; j < seatCount; j++)
                {
                    var seatIndex = br.ReadInt32();
                    fishery.seatIndexList.Add(seatIndex);
                }

                fisheryDic.Add(hexagons[fishIndex], fishery);
                fishHexgaons.Add(hexagons[fishIndex]);
            }
        }

        private int GetSeaZoneCount()
        {
            var sum = 0;
            foreach (var zone in zones.Values)
                if (zone.landType == ZoneLandType.Sea && zone._portIndex > 0)
                    sum++;

            return sum;
        }

        private void ExportCommon(BinaryWriter bw)
        {
            // 2.增加商圈边界mesh信息
            exportVer = 2;
            bw.Write(exportVer); // int 版本
            bw.Write(width); // int 地图宽
            bw.Write(height); // int 地图高
            bw.Write(Hex.HexRadius); // float 六边格半径
        }

        public void ExportToClient(BinaryWriter bw)
        {
            ExportCommon(bw);

            //导出时判断地格是否属于海港
            CheckPortRange = true;

            bw.Write(zones.Count);
            foreach (var zone in zones.Values) zone.ExportToClient(bw);

            ExportHexagongs(bw);
            ExportMoveType(bw);
            CheckPortRange = false;

            // ExportBusiness(bw);
        }

        public void ExportToServer(BinaryWriter bw)
        {
            ExportCommon(bw);

            //导出时判断地格是否属于海港
            CheckPortRange = true;

            // 区域的数量
            // var seaZoneCount = GetSeaZoneCount();
            var seaZoneCount = zones.Count;
            bw.Write(seaZoneCount); // int
            // var seaZones =
            // 	zones.Values.Where(z => z.landType == ZoneLandType.Sea && z._portIndex > 0);
            foreach (var zone in zones.Values) zone.ExportToServer(bw);

            ExportHexagongs(bw);
            ExportMoveType(bw);

            CheckPortRange = false;
        }

        /// <summary>
        ///     导出商圈边界mesh
        /// </summary>
        /// <param name="bw"></param>
        private void ExportBusiness(BinaryWriter bw)
        {
            bw.Write(businessEdgeMeshes.Count);
            foreach (var kv in businessEdgeMeshes)
            {
                var key = kv.Key;
                var mesh = kv.Value;
                bw.Write(key.Item1);
                bw.Write(key.Item2);
                bw.Write(mesh.vertexCount);
                foreach (var vertex in mesh.vertices)
                {
                    bw.Write((int)(vertex.x * 10f));
                    bw.Write((int)(vertex.z * 10f));
                }

                bw.Write(mesh.triangles.Length);
                foreach (var idx in mesh.triangles) bw.Write((ushort)idx);
            }
        }

        public void ExportHexagongs(BinaryWriter bw)
        {
            // 线段式存储（六边格所属区域）
            var lines = GetZoneLines();
            bw.Write(lines.Count); // int		数量
            for (var i = 0; i < lines.Count; i++)
            {
                bw.Write(lines[i].begin); // int		起始idx
                bw.Write(lines[i].len); // short	长度
                bw.Write((short)(lines[i].idx + ExportNewbieMapZoneMargin)); // short	区域 id
            }
        }

        private void ExportMoveType(BinaryWriter bw)
        {
            // 线段式存储（六边格movetype）
            var lines = GetMovetypeLines();
            bw.Write(lines.Count); // int		数量
            for (var i = 0; i < lines.Count; i++)
            {
                bw.Write(lines[i].begin); // int		起始idx
                bw.Write(lines[i].len); // short	长度
                bw.Write(lines[i].idx); // int		movetype 枚举
            }
        }

        // 单独导出阻挡信息
        public void ExportToBlock(BinaryWriter bw)
        {
            bw.Write(width); // int 地图宽
            bw.Write(height); // int 地图高

            // 按位导出阻挡信息
            var need = hexagons.Length / 8 + 1;
            bw.Write(need);

            var bytes = new byte[need];

            var idx = 0;
            int x, y;
            for (var i = 0; i < need; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    idx = i * 8 + j;
                    if (idx >= hexagons.Length) break;
                    x = idx % width;
                    y = idx / width;
                    //var color = blockData.GetPixel(x, height - y);
                    //if (color.r == 1)
                    if (IsBlock(x, y)) bytes[i] |= (byte)(1 << j);
                }

                if (idx >= hexagons.Length) break;
            }

            bw.Write(bytes, 0, need);

            // ver = 2
            ExportRuinsToBlock(bw);
        }

        public void ExportRuinsToBlock(BinaryWriter bw)
        {
            bw.Write(ruinDatas.Count); // 遗迹数量
            foreach (var ruin in ruinDatas.Values) ruin.ExportToBlock(bw);
        }

        public void ExportRuinsToServer(BinaryWriter bw)
        {
            bw.Write(ruinDatas.Count); // 遗迹数量
            foreach (var ruin in ruinDatas.Values) ruin.ExportToServer(bw);
        }

        //
        public void ExportToCSV(string fileName)
        {
            FileStream fs;
            if (!File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                File.Delete(fileName);
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            var sw = new StreamWriter(fs, Encoding.UTF8);
            string[] headers =
            {
                "id", "posType", "nearBy", "isHide", "cityLv", "cityPos", "citySize", "isGuanqia",
                "isBorn", "tower",
                "subType", "ruinId"
            };
            for (var i = 0; i < headers.Length; i++)
            {
                sw.Write(headers[i]);
                if (i < headers.Length - 1)
                    sw.Write(",");
            }

            sw.Write(sw.NewLine);

            var sb = new StringBuilder();
            foreach (var zone in zones.Values)
            {
                sw.Write(zone.index); //id
                sw.Write(",");
                sw.Write(zone.posType); //posType
                sw.Write(",");

                sb.Clear();
                for (var j = 0; j < zone.neigbourZones.Count; j++)
                {
                    sb.Append(zone.neigbourZones[j].index);
                    if (j < zone.neigbourZones.Count - 1)
                        sb.Append("|");
                }

                sw.Write(sb.ToString()); // nearBy
                sw.Write(",");

                sw.Write(zone.visible); // isHide
                sw.Write(",");
                sw.Write(zone.level); // cityLv
                sw.Write(",");

                sb.Clear();
                sb.Append(zone.hexagon.x).Append("|").Append(zone.hexagon.y);
                sw.Write(sb.ToString()); // cityPos
                sw.Write(",");

                var info = MapRender.cityRenders[zone.level];
                sw.Write(info.round); //citySize
                sw.Write(",");

                sw.Write(zone.isGuanqia); //isGuanqia
                sw.Write(",");
                sw.Write(zone.isBorn); //isBorn
                sw.Write(",");
                sw.Write(zone.GetTowerText()); //tower
                sw.Write(",");
                if (zone.ruinId > 0)
                    zone.subType = 2; // 遗迹类型 subType = 2;
                sw.Write(zone.subType); //subType
                sw.Write(",");
                sw.Write(zone.ruinId); //ruinId

                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }


        public void ExportToFisheryCSV(string fileName)
        {
            FileStream fs;
            if (!File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                File.Delete(fileName);
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            var sw = new StreamWriter(fs, Encoding.UTF8);
            string[] headers =
            {
                "id", "mapName", "fishPos", "seatsPos"
            };
            for (var i = 0; i < headers.Length; i++)
            {
                sw.Write(headers[i]);
                if (i < headers.Length - 1)
                    sw.Write(",");
            }

            sw.Write(sw.NewLine);
            var sb = new StringBuilder();
            for (var i = 0; i < fishHexgaons.Count; i++)
            {
                //id
                sw.Write(i);
                sw.Write(",");
                //mapName
                var startIndex = fileName.LastIndexOf("/") + 1;
                var endIndex = fileName.LastIndexOf(".");
                var strLen = endIndex - startIndex;
                var mapName = fileName.Substring(startIndex, strLen);
                sw.Write(mapName);
                sw.Write(",");
                //fishPos
                sw.Write(fishHexgaons[i].x + ";" + fishHexgaons[i].y);
                sw.Write(",");
                //seatsPos
                sb.Clear();
                var fishery = fisheryDic[fishHexgaons[i]];
                if (fishery.seatIndexList != null && fishery.seatIndexList.Count > 0)
                    for (var j = 0; j < fishery.seatIndexList.Count; j++)
                    {
                        var seatHex = GetHexagon(fishery.seatIndexList[j]);
                        sb.Append(seatHex.x + ";" + seatHex.y);
                        if (j < fishery.seatIndexList.Count - 1) sb.Append("|");
                    }

                sw.Write(sb.ToString());
                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        //导出城市
        public void ExportCityToCsv(string fileName)
        {
            FileStream fs;
            if (!File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                File.Delete(fileName);
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            var sw = new StreamWriter(fs, Encoding.UTF8);
            string[] headers =
            {
                // "id", "pos", "level", "nearby", "num", "orient", "sail", "businessZone", "landform"
                "id", "pos", "level", "nearby", "num", "businessZone", "landform"
            };
            for (var i = 0; i < headers.Length; i++)
            {
                sw.Write(headers[i]);
                if (i < headers.Length - 1)
                    sw.Write(",");
            }

            sw.Write(sw.NewLine);

            var sb = new StringBuilder();
            foreach (var zone in zones.Values)
            {
                sw.Write(zone.index + ExportNewbieMapZoneMargin);
                sw.Write(",");

                sb.Clear();
                sb.Append("\"[");
                // var pos = GetOffset(zone._portIndex);
                sb.Append(zone.hexagon.x);
                sb.Append(",");
                sb.Append(zone.hexagon.y);
                sb.Append("]\"");
                sw.Write(sb);

                sw.Write(",");
                sw.Write(zone.level);
                sw.Write(",");

                sb.Clear();
                sb.Append("\"[");

                //去掉循环地图
                // var loopNeighborZones = zone.GetLoopMapNeighborZones();
                // foreach (var nz in loopNeighborZones)
                // {
                // 	sb.Append(nz.index + ExportNewbieMapZoneMargin);
                // 	sb.Append(",");
                // }

                var last = zone.neigbourZones.Last();

                foreach (var nz in zone.neigbourZones)
                {
                    sb.Append(nz.index + ExportNewbieMapZoneMargin);

                    if (nz != last) sb.Append(",");
                }

                sb.Append("]\"");
                sw.Write(sb);
                sw.Write(",");

                var count = 0;
                foreach (var hexagon in zone.hexagons)
                    if (!IsBlock(hexagon.index))
                        count++;

                sw.Write(count);
                // sw.Write(",");
                // sw.Write(zone.GetPortOrient());
                // sw.Write(",");

                // sb.Clear();
                // sb.Append("\"[");
                // var sailPos = GetOffset(zone._portSailIndex);
                // sb.Append(sailPos.x);
                // sb.Append(",");
                // sb.Append(sailPos.y);
                // sb.Append("]\"");
                // sw.Write(sb);

                sw.Write(",");
                sw.Write(zone.businessZone);
                sw.Write(",");

                sw.Write(zone.landform.ToString());
                sw.Write(",");

                sw.Write(sw.NewLine);
            }


            sw.Flush();
            sw.Close();
            fs.Close();
        }

        //划分商圈 类九宫格
        public void SplitBusinessZones()
        {
            var aabbs = new List<Rect>(11);
            var mapWidth3Of1 = mapSizeW / 3;
            var mapHeight3Of1 = mapSizeH / 3;
            var mapWidthHalf = mapSizeW / 2;

            aabbs.Add(new Rect(0, 0, mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(0, mapHeight3Of1, mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(0, 2 * mapHeight3Of1, mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(2 * mapWidth3Of1, 0, mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(2 * mapWidth3Of1, mapHeight3Of1, mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(2 * mapWidth3Of1, 2 * mapHeight3Of1, mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(mapWidth3Of1, 0, mapWidthHalf - mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(mapWidth3Of1, 2 * mapHeight3Of1, mapWidthHalf - mapWidth3Of1,
                mapHeight3Of1));
            aabbs.Add(new Rect(mapWidthHalf, 0, mapWidthHalf - mapWidth3Of1, mapHeight3Of1));
            aabbs.Add(new Rect(mapWidthHalf, 2 * mapHeight3Of1, mapWidthHalf - mapWidth3Of1,
                mapHeight3Of1));
            aabbs.Add(new Rect(mapWidth3Of1, mapHeight3Of1, mapWidth3Of1, mapHeight3Of1));

            foreach (var zone in zones.Values)
            {
                var wp = zone.hexagon.Pos;
                var pos = new Vector2(wp.x, -wp.z);
                // if (zone.landType == ZoneLandType.Sea)
                // {
                // 	var wp = GetHexagonPos(zone._portIndex);
                // 	pos = new Vector2(wp.x, -wp.z);
                // }
                // else
                // {
                // 	var wp = zone.GetZoneCenterWp();
                // 	pos = new Vector2(wp.x, -wp.z);
                // }

                var count = aabbs.Count;
                for (var idx = 0; idx < count; idx++)
                {
                    var rect = aabbs[idx];
                    if (rect.Contains(pos))
                    {
                        zone.businessZone = idx + 1;
                        break;
                    }
                }
            }

            CalcBusiness();
            CalcBusinessEdges(businessEdgeTolerance, businessEdgeWidth);
        }

        // 导出XML格式，服务器需要查错使用
        public bool ExportMapByJson(string fileName)
        {
            int i;
            var info = new HexMapSave();
            info.zones = new ZoneSave[zones.Count];
            i = 0;
            foreach (var zone in zones.Values)
            {
                info.zones[i] = new ZoneSave();
                info.zones[i].index = zone.index;
                info.zones[i].level = zone.level;
                info.zones[i].pos = zone.hexagon.index;
                info.zones[i].posType = zone.posType;
                i++;
            }

            info.hexagons = new HexagonSave[hexagons.Length];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                i = y * width + x;
                info.hexagons[i] = new HexagonSave();
                info.hexagons[i].index = hexagons[i].index;
                info.hexagons[i].x = hexagons[i].x;
                info.hexagons[i].y = hexagons[i].y;
                info.hexagons[i].zone = hexagons[i].zone != null ? hexagons[i].zone.index : 0;
                //var color = blockData.GetPixel(x, height - y);
                info.hexagons[i].block = IsBlock(x, y) ? 1 : 0;
            }

            File.WriteAllText(fileName, JsonUtility.ToJson(info));
            return true;
        }

        public void SaveToPicture(string filename)
        {
            Hexagon hex = null;
            var tex = new Texture2D(width, height);
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                hex = GetHexagon(x, y);
                tex.SetPixel(x, height - y, GetZoneColor(hex.zone.color));
            }

            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open(filename, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            Debug.Log($"SaveToPicture : {filename}");
        }

        public void SaveLandformToPicture(string filename)
        {
            Hexagon hex = null;
            var landforms = Enum.GetValues(typeof(ZoneLandform));
            foreach (ZoneLandform landform in landforms)
            {
                var tex = new Texture2D(width, height);
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    hex = GetHexagon(x, y);
                    if (hex.zone.landform == landform)
                        tex.SetPixel(x, height - y, MapRender.GetLandformColor(landform));
                }

                var dataBytes = tex.EncodeToPNG();
                var fs = File.Open($"{filename}_{landform}.png", FileMode.OpenOrCreate);
                fs.Write(dataBytes, 0, dataBytes.Length);
                fs.Flush();
                fs.Close();
            }

            Debug.Log($"SaveLandformToPicture : {filename}");
        }

        public void SaveFullLandformToPicture(string filename)
        {
            Hexagon hex = null;
            // int scale = 2;
            // int scaleW = scale * width + 1;
            // int scaleH = scale * height;
            // Texture2D tex = new Texture2D(scaleW, scaleH);
            // var colors = tex.GetPixels32();
            // for (int y = 0; y < height; y++)
            // {
            // 	for (int x = 0; x < width; x++)
            // 	{
            // 		int colIndex = x * scale + (y & 1) * 1;
            // 		int rowIndex = y * scale;
            // 		hex = GetHexagon(x, y);
            // 		for (int cellRow = 0; cellRow < scale; cellRow++)
            // 		{
            // 			for (int cellCol = 0; cellCol < scale; cellCol++)
            // 			{
            // 				// 边界处理
            // 				int col = colIndex + cellCol;
            // 				int row = rowIndex + cellRow;
            // 				if (col >= scaleW || row >= scaleH)
            // 					continue;
            // 				colors[col + row * scaleW] = MapRender.GetLandformColor(hex.zone.landform);
            // 				// tex.SetPixel(row, col, MapRender.GetLandformColor(hex.zone.landform));
            // 			}
            // 		}
            // 	}
            // }

            //   □   ⊙   □
            // □ □ ⊙ ⊙ □ □
            //   □ ◩ ⊙   □
            //   ◩ ◩ ◩
            //     ◩
            var texW = width * 2 + 1;
            var texH = width * 2 + 1;
            var tex = new Texture2D(texW, texH);

            var colors = new Color32[texW * texH];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                // 该死的上下翻转
                hex = GetHexagon(x, height - y - 1);
                var colIndex = x * 2 + 1 + (y & 1);
                var rowIndex = y * 2 + 1;
                var color = MapRender.GetLandformColor(hex.zone.landform);

                colors[colIndex + 0 + (rowIndex + 0) * texW] = color; // 中心
                colors[colIndex + 0 + (rowIndex - 1) * texW] = color; // 上
                colors[colIndex + 0 + (rowIndex + 1) * texW] = color; // 下
                colors[colIndex - 1 + (rowIndex + 0) * texW] = color; // 左
                colors[colIndex + 1 + (rowIndex + 0) * texW] = color; // 右
            }

            tex.SetPixels32(colors);
            tex.Apply();

            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open($"{filename}_landform.png", FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();

            Debug.Log($"SaveFullLandformToPicture : {filename}");
        }

        public void SaveZoneSubtypePicture(string filename)
        {
            Hexagon hex = null;
            //   □   ⊙   □
            // □ □ ⊙ ⊙ □ □
            //   □ ◩ ⊙   □
            //   ◩ ◩ ◩
            //     ◩
            var texW = width * 2 + 1;
            var texH = width * 2 + 1;
            var tex = new Texture2D(texW, texH);
            var colors = new Color32[texW * texH];

            foreach (var zone in zones.Values)
                if (zone.subType == 1)
                    foreach (var hexagon in zone.hexagons)
                    {
                        var x = hexagon.x;
                        var y = hexagon.y;
                        var colIndex = x * 2 + 1 + (y & 1);
                        var rowIndex = y * 2 + 1;

                        colors[colIndex + 0 + (rowIndex + 0) * texW] = Color.red; // 中心
                        colors[colIndex + 0 + (rowIndex - 1) * texW] = Color.red; // 上
                        colors[colIndex + 0 + (rowIndex + 1) * texW] = Color.red; // 下
                        colors[colIndex - 1 + (rowIndex + 0) * texW] = Color.red; // 左
                        colors[colIndex + 1 + (rowIndex + 0) * texW] = Color.red; // 右
                    }

            tex.SetPixels32(colors);
            tex.Apply();

            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open($"{filename}_subtype1.png", FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();

            Debug.Log($"SaveZoneSubtypePicture : {filename}");
        }

        public void SaveBlockPicture(string filename)
        {
            Hexagon hex = null;
            var scale = 2;
            var scaleW = scale * width + 1;
            var scaleH = scale * height;
            var tex = new Texture2D(scaleW, scaleH);
            var colors = tex.GetPixels32();
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var colIndex = x * scale + (y & 1) * 1;
                var rowIndex = y * scale;
                hex = GetHexagon(x, height - y - 1);
                for (var cellRow = 0; cellRow < scale; cellRow++)
                for (var cellCol = 0; cellCol < scale; cellCol++)
                {
                    // 边界处理
                    var col = colIndex + cellCol;
                    var row = rowIndex + cellRow;
                    if (col >= scaleW || row >= scaleH)
                        continue;
                    colors[col + row * scaleW] = hex.blockFlag == BlockFlag.Block
                        ? Color.red
                        : Color.black;
                    // tex.SetPixel(row, col, MapRender.GetLandformColor(hex.zone.landform));
                }
            }

            tex.SetPixels32(colors);
            tex.Apply();

            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open($"{filename}.png", FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();


            Debug.Log($"SaveBlockPicture : {filename}");
        }

        public void SaveLandAreaToPicture(string filename)
        {
            Hexagon hex = null;
            //   □   ⊙   □
            // □ □ ⊙ ⊙ □ □
            //   □ ◩ ⊙   □
            //   ◩ ◩ ◩
            //     ◩
            var texW = width * 2 + 1;
            var texH = height * 2 + 1;
            var tex = new Texture2D(texW, texH);

            var colors = new Color32[texW * texH];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                // 该死的上下翻转
                hex = GetHexagon(x, height - y - 1);
                var colIndex = x * 2 + 1;
                var rowIndex = y * 2 + 1 + (x & 1);
                var color = Color.red;

                // if (GetMoveType(hex) == MOVETYPE.DISABLE)
                if (hex.zone.landType == ZoneLandType.Land)
                {
                    colors[colIndex + 0 + (rowIndex + 0) * texW] = color; // 中心
                    colors[colIndex + 0 + (rowIndex - 1) * texW] = color; // 上
                    colors[colIndex + 0 + (rowIndex + 1) * texW] = color; // 下
                    colors[colIndex - 1 + (rowIndex + 0) * texW] = color; // 左
                    colors[colIndex + 1 + (rowIndex + 0) * texW] = color; // 右
                    colors[colIndex + 1 + (rowIndex - 1) * texW] = color; // 右上
                    colors[colIndex + 1 + (rowIndex + 1) * texW] = color; // 右下
                    colors[colIndex - 1 + (rowIndex - 1) * texW] = color; // 左上
                    colors[colIndex - 1 + (rowIndex + 1) * texW] = color; // 左下
                }
            }

            tex.SetPixels32(colors);
            tex.Apply();

            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open($"{filename}_land.png", FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();

            Debug.Log($"SaveLandAreaToPicture : {filename}");
        }

        public void SaveIsLandAreaToPicture(string filename)
        {
            Hexagon hex = null;
            //   □   ⊙   □
            // □ □ ⊙ ⊙ □ □
            //   □ ◩ ⊙   □
            //   ◩ ◩ ◩
            //     ◩
            var texW = width * 2 + 1;
            var texH = height * 2 + 1;
            var tex = new Texture2D(texW, texH);

            var colors = new Color32[texW * texH];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                // 该死的上下翻转
                hex = GetHexagon(x, height - y - 1);
                var colIndex = x * 2 + 1;
                var rowIndex = y * 2 + 1 + (x & 1);
                var color = Color.red;

                if (hex.attribute == HexAttribute.Island)
                {
                    colors[colIndex + 0 + (rowIndex + 0) * texW] = color; // 中心
                    colors[colIndex + 0 + (rowIndex - 1) * texW] = color; // 上
                    colors[colIndex + 0 + (rowIndex + 1) * texW] = color; // 下
                    colors[colIndex - 1 + (rowIndex + 0) * texW] = color; // 左
                    colors[colIndex + 1 + (rowIndex + 0) * texW] = color; // 右
                    colors[colIndex + 1 + (rowIndex - 1) * texW] = color; // 右上
                    colors[colIndex + 1 + (rowIndex + 1) * texW] = color; // 右下
                    colors[colIndex - 1 + (rowIndex - 1) * texW] = color; // 左上
                    colors[colIndex - 1 + (rowIndex + 1) * texW] = color; // 左下
                }
            }

            tex.SetPixels32(colors);
            tex.Apply();

            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open($"{filename}_island.png", FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();

            Debug.Log($"SaveIsLandAreaToPicture : {filename}");
        }

        public void SaveSeaControlToPicture(bool force = false)
        {
            SaveSeaControlToPicture(WaterDistributeTexPath, force);
        }

        public void SaveSeaControlToPicture(string filename, bool force = false)
        {
            Hexagon hex = null;
            //   □    ⊙   □
            // □ □ ⊙ ⊙ □ □
            //   □ ◩ ⊙   □
            //   ◩ ◩ ◩
            //     ◩
            var texW = width * 2 + 1;
            var texH = height * 2 + 1 + 1;
            var tex = new Texture2D(texW, texH);

            var normal = GlobalDef.S_WaterNormal;
            var shallow = GlobalDef.S_WaterShallow;
            var deep = GlobalDef.S_WaterDeep;
            var color = normal;

            var colors = new Color32[texW * texH];
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                // 该死的上下翻转
                hex = GetHexagon(x, height - y - 1);
                var colIndex = x * 2 + 1;
                var rowIndex = y * 2 + 1 + (x & 1);


                var movetype = GetMoveType(hex);
                // 浅海
                if (movetype == MOVETYPE.SHALLOWSEA)
                    color = shallow;
                // 深海
                else if (movetype == MOVETYPE.DEEPSEA)
                    color = deep;
                else
                    color = normal;

                colors[colIndex + 0 + (rowIndex + 0) * texW] = color; // 中心
                colors[colIndex + 0 + (rowIndex - 1) * texW] = color; // 上
                colors[colIndex + 0 + (rowIndex + 1) * texW] = color; // 下
                colors[colIndex - 1 + (rowIndex + 0) * texW] = color; // 左
                colors[colIndex + 1 + (rowIndex + 0) * texW] = color; // 右
            }

            tex.SetPixels32(colors);
            tex.Apply();
            MapRender.instance.UpdateWater(tex, mapSizeW, mapSizeH);

            var directoryPath = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directoryPath))
            {
                if (force)
                    Directory.CreateDirectory(directoryPath);
                else
                    return;
            }

            filename = filename.Replace($".{filename.GetExtensionName()}", ".png");
            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open(filename, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();

            Debug.Log($"SaveSeaControlToPicture : {filename}");
        }

        public void SaveZoneLineToPicture(string filename, bool isOnlySeaLand = false, bool isOnlyLine = false, bool force = false)
        {
            EditorUtility.DisplayProgressBar("导出区域线图", "创建中", 0);

            Hexagon hex = null;
            var texW = width * 2 + 1;
            var texH = height * 2 + 1;

            var tex = new Texture2D(texW, texH);

            var colors = new Color32[texW * texH];

            // EditorUtility.DisplayProgressBar("导出区域线图", "清空颜色", 0);
            int count = 0;
            foreach (var zone in zones.Values)
            {
                EditorUtility.DisplayProgressBar("导出区域等级块图", $"区域线图导出中..{count}/{zones.Values.Count}", (float)count / zones.Values.Count * 100);

                if (isOnlyLine)
                {
                    if (zone.hexagons.Count > 0 && zone.edgeNeigbours.Count > 0)
                    {
                        var city = MapRender.cityRenders[zone.level];
                        var color = city.color;
                        if (zone.level == 0)
                        {
                            color = zone.landType == ZoneLandType.Sea ? Color.black : Color.red;
                        }

                        foreach (var index in zone.edgeNeigbours)
                        {
                            var hexagon = GetHexagon(index);
                            var x = hexagon.x;
                            var y = hexagon.y;

                            var colIndex = x * 2 + 1 + (y & 1);
                            var rowIndex = y * 2 + 1;

                            colors[colIndex + 0 + (rowIndex + 0) * texW] = color; // 中心
                            colors[colIndex + 0 + (rowIndex - 1) * texW] = color; // 上
                            colors[colIndex + 0 + (rowIndex + 1) * texW] = color; // 下
                            colors[colIndex - 1 + (rowIndex + 0) * texW] = color; // 左
                            colors[colIndex + 1 + (rowIndex + 0) * texW] = color; // 右
                        }
                    }
                }
                else
                {
                    if (zone.hexagons.Count > 0)
                    {
                        if (isOnlySeaLand && zone.level > 0)
                            continue;
                        var color = GetZoneColor(zone.color);
                        if (zone.level == 0)
                        {
                            color = zone.landType == ZoneLandType.Sea ? Color.black : Color.red;
                        }
                        var list = zone.hexagons;
                        foreach (var hexagon in list)
                        {
                            var x = hexagon.x;
                            var y = hexagon.y;
                            var colIndex = x * 2 + 1 + (y & 1);
                            var rowIndex = texH - (y * 2 + 1);

                            colors[colIndex + 0 + (rowIndex + 0) * texW] = color; // 中心
                            colors[colIndex + 0 + (rowIndex - 1) * texW] = color; // 上
                            colors[colIndex + 0 + (rowIndex + 1) * texW] = color; // 下
                            colors[colIndex - 1 + (rowIndex + 0) * texW] = color; // 左
                            colors[colIndex + 1 + (rowIndex + 0) * texW] = color; // 右
                        }
                    }
                }
                count += 1;
            }

            tex.SetPixels32(colors);
            tex.Apply();

            var directoryPath = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directoryPath))
            {
                if (force)
                    Directory.CreateDirectory(directoryPath);
                else
                    return;
            }

            filename = filename.Replace($".{filename.GetExtensionName()}", ".png");
            var dataBytes = tex.EncodeToPNG();
            var fs = File.Open(filename, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            EditorUtility.ClearProgressBar();

        }

        private void GenerateMesh()
        {
            _hexMesh = new Mesh { hideFlags = HideFlags.HideAndDontSave };
            Vector3[] vertices =
            {
                Vector3.zero,
                GetHexagonVertex(Vector3.zero, 0),
                GetHexagonVertex(Vector3.zero, 1),
                GetHexagonVertex(Vector3.zero, 2),
                GetHexagonVertex(Vector3.zero, 3),
                GetHexagonVertex(Vector3.zero, 4),
                GetHexagonVertex(Vector3.zero, 5)
            };

            int[] tris =
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 5,
                0, 5, 6,
                0, 6, 1
            };
            _hexMesh.vertices = vertices;
            _hexMesh.triangles = tris;
        }

        /// <summary>
        ///     获取两点之间距离一定百分比的一个点
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="end">结束点</param>
        /// <param name="distance">起始点到目标点距离百分比</param>
        /// <returns></returns>
        private Vector3 GetBetweenPoint(Vector3 start, Vector3 end, float percent = 0.5f)
        {
            var normal = (end - start).normalized;
            var distance = Vector3.Distance(start, end);
            return normal * (distance * percent) + start;
        }

        public void OnLandZoneChanged()
        {
            needResetMovetypeMark = true;
        }

        //根据海港岛屿模板重置海港位置
        public void ResetIslandPort()
        {
            var seaZones = zones.Values.Where(z => z.landType == ZoneLandType.Sea);
            foreach (var zone in seaZones)
                if (zone._islandId > 0)
                    zone.ResetIslandPort();
        }

        public void GenerateTreasures()
        {
            var seaZones = zones.Values.Where(z => z.landType == ZoneLandType.Sea);
            foreach (var zone in seaZones)
            {
                zone._validTreasures.Clear();
                zone._invalidTreasures.Clear();
            }

            int x = 3, y = 3;
            var randomBase = 2 * Treasure2PortDis;
            for (x = 3; x < width;)
            {
                var random = Random.Range(1, TreasureInterval);
                x = x + randomBase + random;

                for (y = 3; y < height;)
                {
                    y = y + randomBase + random;

                    var hexagon = GetHexagon(x, y);
                    if (hexagon == null)
                        continue;
                    var zone = hexagon.zone;

                    if (GetMoveType(hexagon) == MOVETYPE.DISABLE)
                        continue;

                    Func<Zone, bool> valid = z =>
                    {
                        if (z.landType == ZoneLandType.Land) return true;

                        var portIndex = z._portIndex;
                        var pHex = GetHexagon(portIndex);
                        var cube1 = Axial.Axial2Cube(new Axial(pHex.x, pHex.y));
                        var cube2 = Axial.Axial2Cube(new Axial(hexagon.x, hexagon.y));
                        var sub = Cube.Subtract(cube1, cube2);
                        return sub.Distance2Origin() > Treasure2PortDis;
                    };

                    if (valid(zone) == false)
                        continue;

                    if (zone.landType == ZoneLandType.Sea)
                    {
                        var isValid = true;
                        var neighborZones = zone.neigbourZones;
                        foreach (var nz in neighborZones)
                            if (valid(nz) == false)
                            {
                                isValid = false;
                                break;
                            }

                        if (isValid) zone._invalidTreasures.Add(GetIndex(x, y));
                    }
                }
            }

            foreach (var zone in seaZones)
                for (var i = 0; i < TreasureMaxPerPort; i++)
                    if (zone._invalidTreasures.Count > 0)
                    {
                        var r1 = Random.Range(0, zone._invalidTreasures.Count - 1);
                        var tI = zone._invalidTreasures[r1];
                        zone._validTreasures.Add(tI);
                        zone._invalidTreasures.RemoveAt(r1);
                    }
        }

        public void ExportTreasures1(string fileName)
        {
            FileStream fs;
            var sfile = MapRender.instance;
            if (!File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                File.Delete(fileName);
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            var sw = new StreamWriter(fs, Encoding.UTF8);
            string[] headers =
            {
                "id", "pos", "port", "businessZone"
            };
            for (var i = 0; i < headers.Length; i++)
            {
                sw.Write(headers[i]);
                if (i < headers.Length - 1)
                    sw.Write(",");
            }

            sw.Write(sw.NewLine);

            var sb = new StringBuilder();
            var seaZones = zones.Values.Where(z => z.landType == ZoneLandType.Sea);
            foreach (var zone in seaZones)
            foreach (var index in zone._validTreasures)
            {
                sw.Write(index);
                sw.Write(",");
                sb.Clear();
                sb.Append("\"[");
                var pos = GetOffset(index);
                sb.Append(pos.x);
                sb.Append(",");
                sb.Append(pos.y);
                sb.Append("]\"");
                sw.Write(sb);
                sw.Write(",");
                sw.Write(zone.index);
                sw.Write(",");
                sw.Write(zone.businessZone);
                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public void ExportTreasures2(string fileName)
        {
            FileStream fs;
            var sfile = MapRender.instance;
            if (!File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                File.Delete(fileName);
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            var sw = new StreamWriter(fs, Encoding.UTF8);
            string[] headers =
            {
                "id", "pos", "port", "businessZone"
            };
            for (var i = 0; i < headers.Length; i++)
            {
                sw.Write(headers[i]);
                if (i < headers.Length - 1)
                    sw.Write(",");
            }

            sw.Write(sw.NewLine);

            var sb = new StringBuilder();
            var seaZones = zones.Values.Where(z => z.landType == ZoneLandType.Sea);
            foreach (var zone in seaZones)
            foreach (var index in zone._invalidTreasures)
            {
                sw.Write(index);
                sw.Write(",");
                sb.Clear();
                sb.Append("\"[");
                var pos = GetOffset(index);
                sb.Append(pos.x);
                sb.Append(",");
                sb.Append(pos.y);
                sb.Append("]\"");
                sw.Write(sb);
                sw.Write(",");
                sw.Write(zone.index);
                sw.Write(",");
                sw.Write(zone.businessZone);
                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public void ExportTreasures3(string fileName)
        {
            FileStream fs;
            var sfile = MapRender.instance;
            if (!File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                File.Delete(fileName);
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            }

            var sw = new StreamWriter(fs, Encoding.UTF8);
            string[] headers =
            {
                "port"
            };
            for (var i = 0; i < headers.Length; i++)
            {
                sw.Write(headers[i]);
                if (i < headers.Length - 1)
                    sw.Write(",");
            }

            sw.Write(sw.NewLine);

            var seaZones = zones.Values.Where(z => z.landType == ZoneLandType.Sea);
            foreach (var zone in seaZones)
                if (zone._validTreasures.Count == 0)
                {
                    sw.Write(zone.index);
                    sw.Write(sw.NewLine);
                }

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public void ChangeZoneIndex(int zoneIndex, int changeZoneIndex)
        {
            if (zoneIndex == changeZoneIndex)
            {
                EditorUtility.DisplayDialog("Error", "zoneIndex same", "OK");
                return;
            }

            if (!zones.ContainsKey(zoneIndex))
            {
                EditorUtility.DisplayDialog("Error", "zoneIndex not exist", "OK");
                return;
            }

            zones.Remove(zoneIndex, out var zone);
            zone.index = changeZoneIndex;
            zones.Add(zone.index, zone);
            zone.CalcEdges();
            foreach (var neigbourZone in zone.neigbourZones) neigbourZone.CalcEdges();
        }

        public void ExpandZone(int zoneIndex)
        {
            if (!zones.ContainsKey(zoneIndex))
            {
                EditorUtility.DisplayDialog("Error", "zoneIndex not exist", "OK");
                return;
            }

            var zone = zones[zoneIndex];
            EditorUtility.DisplayProgressBar("划分海域", "开始扩圈", 0);
            zone.Expand(true);
            EditorUtility.DisplayProgressBar("划分海域", "刷新区域", 40);
            zone.CalcEdges();

            int i = 0;
            foreach (var neigbourZone in zone.neigbourZones)
            {
                EditorUtility.DisplayProgressBar("划分海域", "刷新区域", 40 + (i / zone.neigbourZones.Count) * 60);
                neigbourZone.CalcEdges();
                i += 1;
            }
            EditorUtility.DisplayProgressBar("划分海域", "刷新区域", 100);
            EditorUtility.ClearProgressBar();
        }

        #region Interaction

        private const int InteractionExcelIndexColumn = 3;
        private const int InteractionExcelIdColumn = 6;
        private const int InteractionExcelTypeColumn = 7;
        private const int InteractionExcelPositionColumn = 8;
        private const int InteractionExcelPrefabPathColumn = 9;
        private const int InteractionExcelScaleColumn = 10;
        private const int InteractionExcelRotationColumn = 11;
        private const int InteractionExcelBlockPointsColumn = 12;
        private const int InteractionExcelMapIdColumn = 4;
        private const int InteractionExcelCommentsColumn  = 5;

        public void ExportAllInteractionExcel(string filePath)
        {
            #region DefineExcelHeader

            var clientServerFlagLine = 4;
            string[] clientServerFlags =
            {
                "CS", "C", "$", "CS", "CS", "CS", "C", "C", "C", "C"
            };
            var keyTypeLine = 5;
            string[] keyTypes =
            {
                "int", "int", "备注", "int", "int", "int[]", "string", "float", "float[]", "int[][]"
            };
            var keyDescLine = 6;
            string[] keyDescription =
            {
                "索引", "所属地图", "", "交互点ID", "交互点类型", "位置", "资源路径", "缩放比例", "旋转角度", "阻挡区域"
            };
            var keyNameLine = 7;
            string[] keyNames =
            {
                "id", "mapId", "", "interactionId", "type", "position", "src", "scale", "rotation", "blockPoints"
            };
            var valueStarLine = 7;

            #endregion

            using (var package = new ExcelPackage())
            {
                #region InitExcel

                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var worksheet = package.Workbook.Worksheets.Add(fileName);
                worksheet.Cells[1, 1].Value = 0;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor
                    .SetColor(System.Drawing.Color.Black);
                worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                var colCount = clientServerFlags.Length + 2;
                for (var row = 1; row < valueStarLine; row++)
                for (var col = 3; col <= colCount; col++)
                {
                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col].Style.Fill.BackgroundColor
                        .SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[row, col].Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                // worksheet.Cells[1, 1].

                worksheet.Cells[2, 3].Value = "Link:Interactions";

                worksheet.Cells[3, 3].Value = "Key";

                for (var i = 0; i < clientServerFlags.Length; i++)
                    worksheet.Cells[clientServerFlagLine, i + 3].Value = clientServerFlags[i];

                for (var i = 0; i < keyTypes.Length; i++)
                    worksheet.Cells[keyTypeLine, i + 3].Value = keyTypes[i];

                for (var i = 0; i < keyDescription.Length; i++)
                    worksheet.Cells[keyDescLine, i + 3].Value = keyDescription[i];

                for (var i = 0; i < keyNames.Length; i++)
                    worksheet.Cells[keyNameLine, i + 3].Value = keyNames[i];


                #endregion

                int index = 10000;
                int idx = 0;
                //按照地图顺序遍历
                foreach (var campaignMapInfo in MapRender.instance.CampaignMapList)
                {
                    if (!MapRender.instance.AllInteractionPoints.TryGetValue(campaignMapInfo.Id, out var mapInteractionList))
                    {
                        Debug.LogError($"地图 {campaignMapInfo.Id} 没有交互点数据");
                        continue;
                    }

                    // Add interaction points data
                    for (var i = 0; i < mapInteractionList.Count; i++)
                    {
                        var interaction = mapInteractionList[i];
                        idx++;
                        index++;
                        worksheet.Cells[valueStarLine + idx, InteractionExcelIndexColumn].Value = index;
                        worksheet.Cells[valueStarLine + idx, InteractionExcelIdColumn].Value = interaction.id;
                        // 保存注释信息
                        worksheet.Cells[valueStarLine + idx, InteractionExcelCommentsColumn].Value = interaction.comments;
                        worksheet.Cells[valueStarLine + idx, InteractionExcelTypeColumn].Value = (int)interaction.type;
                        worksheet.Cells[valueStarLine + idx, InteractionExcelPositionColumn].Value =
                            $"[{interaction.position.x},{interaction.position.y}]";
                        worksheet.Cells[valueStarLine + idx, InteractionExcelPrefabPathColumn].Value = GetSrcIndex(interaction.prefabPath);
                        worksheet.Cells[valueStarLine + idx, InteractionExcelScaleColumn].Value =
                            interaction.scale.x;
                        worksheet.Cells[valueStarLine + idx, InteractionExcelRotationColumn].Value =
                            $"[{interaction.rotation.x},{interaction.rotation.y},{interaction.rotation.z}]";
                        if (interaction.BlockPoints != null && interaction.BlockPoints.Count > 0)
                        {
                            var blockPoints = interaction.BlockPoints;
                            var blockPointsStr = new StringBuilder();
                            blockPointsStr.Append("[");
                            for (var j = 0; j < blockPoints.Count; j++)
                            {
                                blockPointsStr.Append("[");
                                blockPointsStr.Append(blockPoints[j].x);
                                blockPointsStr.Append(",");
                                blockPointsStr.Append(blockPoints[j].y);
                                blockPointsStr.Append("]");
                                if (j < blockPoints.Count - 1)
                                    blockPointsStr.Append(",");
                            }

                            blockPointsStr.Append("]");
                            worksheet.Cells[valueStarLine + idx, InteractionExcelBlockPointsColumn].Value = blockPointsStr.ToString();
                        }
                        if (interaction.mapId > 0)
                            worksheet.Cells[valueStarLine + idx, InteractionExcelMapIdColumn].Value = interaction.mapId;
                    }
                }
                MapRender.instance.ResetInteractionIndexId();
                // Save the Excel file
                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }

        public string GetSrcIndex(string path)
        {
            var srcIndex = MapRender.instance.PrefabsSrcIndex;
            foreach (var srcKeyValue in srcIndex)
                if (srcKeyValue.Value == path)
                    return srcKeyValue.Key;
            return path;
        }

        public void ImportAllInteractionExcel(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var package = new ExcelPackage(stream))
            {
                MapRender.instance.ClearCurrentMapInteractionPoints();
                var worksheet = package.Workbook.Worksheets[1];
                if (worksheet == null)
                {
                    Debug.LogError("Worksheet 'Interactions' not found");
                    return;
                }

                var valueStartLine = 8;
                var rowCount = worksheet.Dimension.Rows;
                for (var row = valueStartLine; row <= rowCount; row++)
                {
                    var interaction = new InteractionPoint();
                    //这其实是表id
                    interaction.index = Convert.ToInt32(worksheet.Cells[row, InteractionExcelIndexColumn].Value);
                    //这其实是事件id
                    interaction.id = Convert.ToUInt16(worksheet.Cells[row, InteractionExcelIdColumn].Value);
                    // 保存注释信息
                    if (worksheet.Cells[row, InteractionExcelCommentsColumn].Value != null)
                    {
                        interaction.comments = worksheet.Cells[row, InteractionExcelCommentsColumn].Value.ToString();
                    }
                    else
                    {
                        interaction.comments = string.Empty;
                    }
                    //交互点类型
                    interaction.type = (InteractionType)Convert.ToInt32(worksheet.Cells[row, InteractionExcelTypeColumn].Value);
                    //位置
                    if (worksheet.Cells[row, InteractionExcelPositionColumn].Value != null)
                    {
                        interaction.position =
                            ParseVector2(worksheet.Cells[row, InteractionExcelPositionColumn].Value.ToString());
                    }
                    //资源路径
                    if (worksheet.Cells[row, InteractionExcelPrefabPathColumn].Value != null)
                    {
                        interaction.prefabPath = worksheet.Cells[row, InteractionExcelPrefabPathColumn].Value.ToString();
                        var srcIndex = MapRender.instance.PrefabsSrcIndex;
                        if (srcIndex.TryGetValue(interaction.prefabPath, out var value))
                        {
                            interaction.prefabPath = value;
                        }
                    }
                    //缩放
                    var scale = Convert.ToSingle(worksheet.Cells[row, InteractionExcelScaleColumn].Value);
                    scale = scale == 0 ? 1 : scale;
                    interaction.scale = new Vector3(scale, scale, scale);
                    //旋转
                    if (worksheet.Cells[row, InteractionExcelRotationColumn].Value != null)
                    {
                        // Log.Info($"Id {interaction.id} Index {interaction.index} rotation {worksheet.Cells[row, InteractionExcelRotationColumn].Value.ToString()}");
                        interaction.rotation = ParseVector3(worksheet.Cells[row, InteractionExcelRotationColumn].Value.ToString());
                    }
                    //阻挡区域
                    if (worksheet.Cells[row, InteractionExcelBlockPointsColumn].Value != null)
                    {
                        var blockPointsStr = worksheet.Cells[row, InteractionExcelBlockPointsColumn].Value.ToString();
                        blockPointsStr = blockPointsStr.Replace(" ", "").Trim('[', ']');

                        interaction.BlockPoints = new List<Vector2Int>();

                        var parts = blockPointsStr.Split(new[] { "],[" }, System.StringSplitOptions.RemoveEmptyEntries);

                        foreach (var part in parts)
                        {
                            var cleanPart = part.Trim('[', ']');
                            var pointParts = cleanPart.Split(',');

                            if (pointParts.Length == 2 &&
                                int.TryParse(pointParts[0], out int x) &&
                                int.TryParse(pointParts[1], out int y))
                            {
                                interaction.BlockPoints.Add(new Vector2Int(x, y));
                            }
                            else
                            {
                                Debug.LogWarning($"Invalid point format: {cleanPart}");
                            }
                        }
                    }

                    //所属地图
                    if (worksheet.Cells[row, InteractionExcelMapIdColumn].Value != null)
                        interaction.mapId = worksheet.Cells[row, InteractionExcelMapIdColumn].Value.ToInt();
                    else
                    {
                        //必须要有地图, 没有地图就报错
                        Debug.LogError($"交互物 {interaction.id} 没有配置地图id, 请检查配置");
                        return;
                    }
                    MapRender.instance.AddInteractionPoint(interaction.mapId, interaction);
                }
                MapEditorEventCenter.SendEvent(MapEditorEvent.InteractionReloadEvent);
            }
        }

        public static void ImportSrcIndexExcel(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var package = new ExcelPackage(stream))
            {
                MapRender.instance.PrefabsSrcIndex.Clear();
                var worksheet = package.Workbook.Worksheets["Src_Index"];
                if (worksheet == null)
                {
                    Debug.LogError("Worksheet 'Src_Index' not found");
                    return;
                }

                var valueStartLine = 8;
                var rowCount = worksheet.Dimension.Rows;
                for (var row = valueStartLine; row <= rowCount; row++)
                {
                    var key = Convert.ToString(worksheet.Cells[row, 3].Value);
                    var value = Convert.ToString(worksheet.Cells[row, 7].Value);
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                    {
                        Debug.LogWarning($"Invalid key or value at row {row}");
                        continue;
                    }

                    if (MapRender.instance.PrefabsSrcIndex.ContainsKey(key))
                        continue;
                    MapRender.instance.PrefabsSrcIndex.Add(key, value);
                }
            }
        }

        public static void ImportCampaignMapInfoExcel(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var package = new ExcelPackage(stream))
            {
                // MapRender.instance.CampaignMapList.Clear();
                var worksheet = package.Workbook.Worksheets["Campaign_Map_Info"];
                if (worksheet == null)
                {
                    Debug.LogError("Worksheet 'Campaign_Map_Info' not found");
                    return;
                }
                var valueStartLine = 8;
                var rowCount = worksheet.Dimension.Rows;
                for (var row = valueStartLine; row <= rowCount; row++)
                {
                    var key = Convert.ToInt32(worksheet.Cells[row, 3].Value);
                    var value = Convert.ToString(worksheet.Cells[row, 14].Value);
                    if (string.IsNullOrEmpty(key.ToString()) || string.IsNullOrEmpty(value))
                    {
                        Debug.LogError($"Invalid key or value at row {row}");
                        continue;
                    }

                    var campaignMapInfo = new CampaignMapInfo()
                    {
                        Id = key,
                        Name = value,
                    };
                    // MapRender.instance.CampaignMapList.Add(campaignMapInfo);
                    MapRender.instance.AddCampaignMap(campaignMapInfo);
                }
            }
        }

        public static void ImportCampaignFogExcel(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets["Campaign_Fog"];
                    if (worksheet == null)
                    {
                        Debug.LogError("Worksheet 'Campaign_Map_Info' not found");
                        return;
                    }
                    var valueStartLine = 8;
                    var rowCount = worksheet.Dimension.Rows;
                    for (var row = valueStartLine; row <= rowCount; row++)
                    {
                        var id = Convert.ToInt32(worksheet.Cells[row, 3].Value);
                        if (id <= 0)
                            continue; // 跳过无效的ID
                        string tipInfo = Convert.ToString(worksheet.Cells[row, 4].Value);

                        var mapId = Convert.ToInt32(worksheet.Cells[row, 5].Value);
                        var fogId = Convert.ToInt32(worksheet.Cells[row, 6].Value);
                        var locked = Convert.ToString(worksheet.Cells[row, 7].Value);
                        var fogPosString = Convert.ToString(worksheet.Cells[row, 8].Value);
                        fogPosString = fogPosString.Replace(" ", "").Trim('[', ']');
                        var posParts = fogPosString.Split(',');
                        var fogPos = new Vector3(float.Parse(posParts[0]), float.Parse(posParts[1]), float.Parse(posParts[2]));
                        var fogGridString = Convert.ToString(worksheet.Cells[row, 9].Value);
                        var fogGrid = new List<Vector3>();
                        fogGridString = fogGridString.Replace(" ", "").Trim('[', ']');
                        var parts = fogGridString.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < parts.Length; i += 2)
                        {
                            fogGrid.Add(new Vector3(
                                float.Parse(parts[i]),
                                0,
                                float.Parse(parts[i + 1])));
                        }

                        string bubbleConString = Convert.ToString(worksheet.Cells[row, 10].Value);
                        string bubbleCoorString = Convert.ToString(worksheet.Cells[row, 11].Value);
                        string unlockConString = Convert.ToString(worksheet.Cells[row, 12].Value);
                        string unlockAnimationTime = Convert.ToString(worksheet.Cells[row, 13].Value);

                        var campaignFogInfo = new CampaignFogInfo
                        {
                            Id = id,
                            MapId = mapId,
                            FogId = fogId,
                            FogPos = fogPos,
                            FogGrid = fogGrid,
                            Locked = locked,
                            BubbleCon = bubbleConString,
                            BubbleCoor = bubbleCoorString,
                            UnlockCon = unlockConString,
                            TipInfo = tipInfo,
                            UnlockAnimationTime = unlockAnimationTime
                        };
                        MapRender.instance.AddCampaignFogInfo(campaignFogInfo);
                    }
                }
        }

        public static void ExportCampaignFogExcel(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                #region DefineExcelHeader

                var clientServerFlagLine = 4;
                string[] clientServerFlags =
                {
                    "CS", "", "CS", "C", "CS", "C", "C", "CS", "CS", "CS", "C"
                };
                var keyTypeLine = 5;
                string[] keyTypes =
                {
                    "int", "$", "int", "int", "int", "float[]", "float[]", "int[]", "int[]", "int[]", "float"
                };
                var keyDescLine = 6;
                string[] keyDescription =
                {
                    "索引", "说明", "归属关卡", "雾的预制体id", "底迷雾【不可解锁】", "迷雾格子", "迷雾格子", "气泡显示条件", "气泡显示位置", "解锁条件", "解锁动画时间"
                };
                var keyNameLine = 7;
                string[] keyNames =
                {
                    "id", "", "map", "fogId", "locked", "fogPos", "foggrid", "bubbleCon", "bubbleCoor", "unlockCon", "unlockAnimationTime"
                };
                var valueStarLine = 7;
                #endregion
                #region InitExcel

                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var worksheet = package.Workbook.Worksheets.Add(fileName);
                worksheet.Cells[1, 1].Value = 0;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor
                    .SetColor(System.Drawing.Color.Black);
                worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                var colCount = clientServerFlags.Length + 2;
                for (var row = 1; row <= valueStarLine; row++)
                for (var col = 3; col <= colCount; col++)
                {
                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, col].Style.Fill.BackgroundColor
                        .SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[row, col].Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                // worksheet.Cells[1, 1].

                worksheet.Cells[2, 5].Value = "Link:Campaign_Map_Info";
                worksheet.Cells[2, 10].Value = "Link:Campaign_Event";
                worksheet.Cells[2, 12].Value = "Link:Campaign_Event";

                worksheet.Cells[3, 3].Value = "Key";

                for (var i = 0; i < clientServerFlags.Length; i++)
                    worksheet.Cells[clientServerFlagLine, i + 3].Value = clientServerFlags[i];

                for (var i = 0; i < keyTypes.Length; i++)
                    worksheet.Cells[keyTypeLine, i + 3].Value = keyTypes[i];

                for (var i = 0; i < keyDescription.Length; i++)
                    worksheet.Cells[keyDescLine, i + 3].Value = keyDescription[i];

                for (var i = 0; i < keyNames.Length; i++)
                    worksheet.Cells[keyNameLine, i + 3].Value = keyNames[i];
                #endregion
                List<CampaignFogInfo> campaignFogInfos = new List<CampaignFogInfo>();
                foreach (var campaignFogMapInfoList in MapRender.instance.CampaignFogInfos)
                {
                    // campaignFogInfos.Add(campaignFogInfo.Value);
                    foreach (var campaignFogInfo in campaignFogMapInfoList.Value)
                    {
                        campaignFogInfos.Add(campaignFogInfo);
                    }
                }
                campaignFogInfos.Sort((a, b) => a.Id.CompareTo(b.Id));
                for (int i = 0; i < campaignFogInfos.Count; i++)
                {
                    var campaignFogInfo = campaignFogInfos[i];
                    worksheet.Cells[valueStarLine + i + 1, 3].Value = campaignFogInfo.Id;
                    worksheet.Cells[valueStarLine + i + 1, 4].Value = campaignFogInfo.TipInfo;
                    worksheet.Cells[valueStarLine + i + 1, 5].Value = campaignFogInfo.MapId;
                    worksheet.Cells[valueStarLine + i + 1, 6].Value = campaignFogInfo.FogId;
                    worksheet.Cells[valueStarLine + i + 1, 7].Value = campaignFogInfo.Locked;
                    worksheet.Cells[valueStarLine + i + 1, 8].Value =
                        $"[{campaignFogInfo.FogPos.x},{campaignFogInfo.FogPos.y},{campaignFogInfo.FogPos.z}]";
                    if (campaignFogInfo.FogGrid != null && campaignFogInfo.FogGrid.Count > 0)
                    {
                        var fogGridStr = new StringBuilder();
                        fogGridStr.Append("[");
                        for (var j = 0; j < campaignFogInfo.FogGrid.Count; j++)
                        {
                            // fogGridStr.Append("[");
                            fogGridStr.Append(campaignFogInfo.FogGrid[j].x);
                            fogGridStr.Append(",");
                            fogGridStr.Append(campaignFogInfo.FogGrid[j].z);
                            // fogGridStr.Append("]");
                            if (j < campaignFogInfo.FogGrid.Count - 1)
                                fogGridStr.Append(",");
                        }

                        fogGridStr.Append("]");
                        worksheet.Cells[valueStarLine + i + 1, 9].Value = fogGridStr.ToString();
                    }
                    worksheet.Cells[valueStarLine + i + 1, 10].Value = campaignFogInfo.BubbleCon;
                    worksheet.Cells[valueStarLine + i + 1, 11].Value = campaignFogInfo.BubbleCoor;
                    worksheet.Cells[valueStarLine + i + 1, 12].Value = campaignFogInfo.UnlockCon;
                    worksheet.Cells[valueStarLine + i + 1, 13].Value = campaignFogInfo.UnlockAnimationTime;
                }
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private Vector2Int ParseVector2(string value)
        {
            value = value.Trim('[', ']');
            var parts = value.Split(',');
            return new Vector2Int(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        private Vector3 ParseVector3(string value)
        {
            value = value.Trim('[', ']');
            var parts = value.Split(',');
            return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }

        #endregion


        #region MapObjects

        public void ClearMapObjects()
        {
            var parent = MapRender.instance.parentForPrefabs;
            var parentTrans = parent.transform;

            // 清空parentTrans 下的所有子物体
            ClearAllChildren(parentTrans);
        }

        private bool isLoadingObjects;

        public void LoadMapObjects(string mapPrefabObjectsDescPath)
        {
            if (isLoadingObjects)
            {
                MapRender.instance.ShowNotification("正在加载");
                return;
            }

            isLoadingObjects = true;
            var guid2GameObjects = new Dictionary<string, GameObject>();
            var tab2Trans = new Dictionary<string, Transform>();

            var mapObjectsDesc =
                JsonUtility.FromJson<MapObjectsDesc>(File.ReadAllText(mapPrefabObjectsDescPath));
            var parent = MapRender.instance.parentForPrefabs;
            var parentTrans = parent.transform;

            // 清空parentTrans 下的所有子物体
            ClearAllChildren(parentTrans);
            // 创建tab transform
            foreach (var mapObjects in mapObjectsDesc.mapObjects)
            {
                var tabname = mapObjects.tabname;
                var tabTrans = parentTrans.Find(tabname);
                if (tabTrans == null)
                {
                    tabTrans = new GameObject(tabname).transform;
                    tabTrans.SetParent(parentTrans);
                }

                tab2Trans.Add(tabname, tabTrans);
            }

            var preframeCount = 100;
            var currentTab = 0;
            var currentIndex = 0;

            void OnEditorUpdate()
            {
                try
                {
                    if (currentTab < mapObjectsDesc.mapObjects.Length)
                    {
                        var mapObjects = mapObjectsDesc.mapObjects[currentTab];
                        var tabname = mapObjects.tabname;

                        for (var i = 0; i < preframeCount; i++)
                            if (currentIndex < mapObjects.count)
                            {
                                var obj = mapObjects.objects[currentIndex];
                                if (!guid2GameObjects.TryGetValue(obj.guid, out var prefab))
                                {
                                    var assetPath = AssetDatabase.GUIDToAssetPath(obj.guid);
                                    prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                                    if (prefab == null)
                                    {
                                        Debug.LogError($"Prefab not exist:{obj.guid} - {obj.name}");
                                        prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    }

                                    guid2GameObjects.Add(obj.guid, prefab);
                                }

                                var instance =
                                    PrefabUtility.InstantiatePrefab(prefab, tab2Trans[tabname]) as
                                        GameObject;
                                instance.transform.localPosition = obj.T;
                                instance.transform.localEulerAngles = obj.R;
                                instance.transform.localScale = obj.S;
                                MapRender.instance.prefabPainter.AddObject(instance);
                                currentIndex++;
                            }
                            else
                            {
                                currentIndex = 0;
                                currentTab++;
                                break;
                            }
                    }
                    else
                    {
                        isLoadingObjects = false;
                        EditorApplication.update -= OnEditorUpdate;
                        MapRender.instance.ShowNotification("加载 Map Objects 成功");
                    }
                }
                catch (Exception e)
                {
                    isLoadingObjects = false;
                    EditorApplication.update -= OnEditorUpdate;
                    Debug.LogError(e);
                }
            }

            EditorApplication.update += OnEditorUpdate;
        }

        public void SaveMapObjects(string mapPrefabObjectsDescPath)
        {
            var desc = new MapObjectsDesc();

            var parent = MapRender.instance.parentForPrefabs;
            var parentTrans = parent.transform;

            var count = parentTrans.childCount;
            desc.mapObjects = new MapObjects[count];
            for (var i = 0; i < count; i++)
            {
                var tab = parentTrans.GetChild(i);
                var prefabObjectCount = tab.childCount;

                var objects = new List<MapObject>(prefabObjectCount);
                for (var j = 0; j < prefabObjectCount; j++)
                {
                    var obj = tab.GetChild(j);
                    var guid = FileHelper.GetPrefabAssetPath(obj.gameObject);
                    if (string.IsNullOrEmpty(guid))
                        continue;
                    objects.Add(new MapObject
                    {
                        name = obj.name,
                        guid = guid,
                        T = obj.localPosition,
                        R = obj.localEulerAngles,
                        S = obj.localScale
                    });
                }

                var mapObjects = new MapObjects();
                mapObjects.tabname = tab.name;
                mapObjects.objects = objects.ToArray();
                mapObjects.count = objects.Count;
                desc.mapObjects[i] = mapObjects;
            }

            File.WriteAllText(mapPrefabObjectsDescPath, JsonUtility.ToJson(desc));
            Debug.Log("保存Map Objects 成功");
            MapRender.instance.ShowNotification("保存Map Objects 成功");
        }

        public void ExportMapObjects(Transform parentTrans, string dataPath, string prefabPath, bool isUseGameObject = false)
        {
            int tilex = 8, tiley = 8;
            // var parent = MapRender.instance.parentForPrefabs;
            // var parentTrans = parent.transform;

            // 资源汇总
            var materailLst = new List<Material>();
            var meshLst = new List<Mesh>();
            // 资源列表，数据列表
            var regionResResDataList = new List<WorldRegionResSerializeData>();
            var regionDataLst = new List<WorldRegionDetailScriptableObject>(tilex * tiley);

            // total(资源，数据)
            var regionTotal =
                ScriptableObject.CreateInstance<WorldRegionTotalScriptableObject>();
            regionTotal.regionResResDataList = regionResResDataList;
            regionTotal.regionTotalWidth = mapSizeW;
            regionTotal.regionTotalHeight = mapSizeH;
            regionTotal.regionRowNum = tilex;
            regionTotal.regionColNum = tiley;

            var mapSize = new Vector2(mapSizeW, mapSizeH);
            var halfMapSize = mapSize / 2;
            var tileSize = mapSize / new Vector2(tilex, tiley);

            // Dictionary<string, >
            var regionDatas = new WorldRegionDetailScriptableObject[tilex, tiley];
            // tab
            var count = parentTrans.childCount;
            for (var i = 0; i < count; i++)
            {
                var tabProgress = (i + 1f) / count;
                // 遍历所有预制
                //	获取预制的导出模板，没有就构建
                //	计算物体的区块，使之保存在对应的区块
                var tab = parentTrans.GetChild(i);
                var objCount = tab.childCount;
                var tabname = tab.name;
                ExportMeshRes(objCount, ref tabProgress, tabname, tab, tileSize, tilex, tiley, ref regionDatas, ref regionDataLst, ref regionResResDataList, ref meshLst, ref materailLst, regionTotal, isUseGameObject);
            }

            // 删除旧数据
            var assetPaths = Directory.GetFiles(dataPath, "region_*.asset",
                SearchOption.TopDirectoryOnly);
            foreach (var assetPath in assetPaths)
            {
                var relativePath = assetPath.Replace(Application.dataPath, "").Replace("\\", "/");
                AssetDatabase.DeleteAsset(relativePath);
            }

            AssetDatabase.Refresh();

            // 保存asset
            var regionTotalDataPath = Path.Combine(dataPath, "region_total.asset");
            var regionTotalDataPath2 = regionTotalDataPath.Replace("\\", "/");
            AssetDatabase.CreateAsset(regionTotal, regionTotalDataPath2);
            for (var i = 0; i < regionDataLst.Count; i++)
            {
                var regiondata = regionDataLst[i];
                var regionDataPath = Path.Combine(dataPath,
                    $"region_{regiondata.regionCol}_{regiondata.regionRow}.asset");
                AssetDatabase.CreateAsset(regiondata, regionDataPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();

            // ResRender保存预制体
            var resRenderDataGo = new GameObject($"{GlobalDef.S_WORLDRESRENDERDATA_NAME}");
            var data = resRenderDataGo.AddComponent<ResRenderData>();
            data.materialList = materailLst.Distinct().ToList();
            data.meshList = meshLst.Distinct().ToList();
            data.worldRegionTotalScriptableObject = regionTotal;
            data.worldRegionDetailScriptableObjectList = regionDataLst;

            var relativePrefabPath = Path.Combine(prefabPath,
                $"{GlobalDef.S_WORLDRESRENDERDATA_NAME}.prefab");
            AssetDatabase.DeleteAsset(relativePrefabPath);
            PrefabUtility.SaveAsPrefabAsset(resRenderDataGo, relativePrefabPath);
            Object.DestroyImmediate(resRenderDataGo);
        }

        private void ClearAllChildren(Transform parent)
        {
            // 记录当前子物体数量
            var childCount = parent.childCount;

            // 从后往前遍历子物体，避免遍历过程中因销毁子物体导致索引失效
            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                Object.DestroyImmediate(child.gameObject);
            }
        }

        private void ExportMeshRes(int objCount, ref float tabProgress, string tabname,
            Transform tab, Vector2 tileSize, int tilex, int tiley,
            ref WorldRegionDetailScriptableObject[,] regionDatas,
            ref List<WorldRegionDetailScriptableObject> regionDataLst,
            ref List<WorldRegionResSerializeData> regionResResDataList, ref List<Mesh> meshLst,
            ref List<Material> materailLst, WorldRegionTotalScriptableObject regionTotal,
            bool isUseOwn)
        {
            for (var j = 0; j < objCount; j++)
            {
                EditorUtility.DisplayProgressBar("导出数据中", $"正在处理 {tabname}",
                        tabProgress * 1f * j / objCount);

                    var obj = tab.GetChild(j);
                    var guid = FileHelper.GetPrefabAssetPath(obj.gameObject);
                    // 不是预制跳过
                    if (string.IsNullOrEmpty(guid))
                    {
                        ExportMeshRes(obj.childCount, ref tabProgress, obj.name, obj, tileSize, tilex,
                            tiley, ref regionDatas, ref regionDataLst, ref regionResResDataList,
                            ref meshLst, ref materailLst, regionTotal, isUseOwn);
                        continue;
                    }

                    var bound = RenderUtil.GetBounds(obj.gameObject);
                    var tmpPos = bound.center;
                    var tileIndexX = (int)(tmpPos.x / tileSize.x);
                    var tileIndexY = (int)(-tmpPos.z / tileSize.y);

                    // if (tileIndexX >= tilex || tileIndexX < 0 ||
                    //     tileIndexY >= tiley || tileIndexY < 0)
                    //     continue;
                    tileIndexX = Mathf.Clamp(tileIndexX, 0, tilex - 1);
                    tileIndexY = Mathf.Clamp(tileIndexY, 0, tiley - 1);

                    var tilePos = new Vector3((tileIndexX + 0.5f) * tileSize.x, 0,
                        -(tileIndexY + 0.5f) * tileSize.y);

                    var boundsRadius = RenderUtil.GetBoundsRadius(bound);

                    WorldRegionDetailScriptableObject regionData = null;
                    if (regionDatas[tileIndexX, tileIndexY] == null)
                    {
                        regionData = ScriptableObject
                            .CreateInstance<WorldRegionDetailScriptableObject>();
                        regionData.regionRow = tileIndexX;
                        regionData.regionCol = tileIndexY;
                        regionData.regionBounds = new Bounds(tilePos, tileSize);
                        regionData.regionBounds.Encapsulate(bound);
                        regionDatas[tileIndexX, tileIndexY] = regionData;
                        regionDataLst.Add(regionData);
                    }
                    else
                    {
                        regionData = regionDatas[tileIndexX, tileIndexY];
                        regionData.regionBounds.Encapsulate(bound);
                    }

                    var variableLst = regionData.renderResList;


                    if (isUseOwn)
                    {
                        var offset = bound.center - obj.position;
                        offset.y = 0;
                        if (obj.GetComponent<MeshFilter>())
                        {
                            var meshFilter = obj.GetComponent<MeshFilter>();
                            var renderer = obj.GetComponent<MeshRenderer>();
                            if (meshFilter != null && renderer != null)
                            {
                                if (meshFilter.sharedMesh == null)
                                {
                                    Debug.LogWarning($"no sharedMesh {obj.name}");
                                    ExportMeshRes(obj.childCount, ref tabProgress, obj.name, obj, tileSize, tilex, tiley, ref regionDatas, ref regionDataLst, ref regionResResDataList, ref meshLst, ref materailLst, regionTotal, isUseOwn);
                                    continue;
                                }

                                var meshName = meshFilter.sharedMesh.name;
                                var sharedMats = renderer.sharedMaterials;
                                for (int subIndex = 0; subIndex < sharedMats.Length; subIndex++)
                                {
                                    var mat = sharedMats[subIndex];
                                    var matName = mat.name;

                                    // res
                                    var resId = regionResResDataList.FindIndex(item =>
                                        item.meshName.Equals(meshName) &&
                                        item.materialName.Equals(matName) &&
                                        item.subMeshIndex == subIndex &&
                                        item.renderLod == 3
                                    );
                                    if (resId == -1)
                                    {
                                        resId = regionResResDataList.Count;

                                        meshLst.Add(meshFilter.sharedMesh);
                                        materailLst.Add(mat);
                                        regionResResDataList.Add(
                                            new WorldRegionResSerializeData(
                                                resId,
                                                meshName,
                                                matName,
                                                mat.enableInstancing ? 2 : 1,
                                                3,
                                                1,
                                                subIndex));
                                    }

                                    WorldRenderVariable variable = null;
                                    var listId = variableLst.FindIndex(item => item.resId == resId);
                                    if (listId == -1)
                                    {
                                        variable = new WorldRenderVariable();
                                        variable.resId = resId;
                                        variableLst.Add(variable);
                                    }
                                    else
                                    {
                                        variable = variableLst[listId];
                                    }

                                    // 区域数据
                                    variable.boundRadiusList.Add(offset.magnitude > boundsRadius / 2
                                        ? boundsRadius * 2
                                        : boundsRadius);
                                    variable.eulerList.Add(obj.rotation.eulerAngles);
                                    variable.posList.Add(obj.position);
                                    variable.scaleList.Add(obj.lossyScale);
                                    regionTotal.regionObjTotalNum++;
                                }
                            }
                        }
                    }


                    // 之前工具的处理，先保留
                    // // 处理资源和数据
                    // // 如果资源列表中不存在，则添加
                    // // 位置旋转缩放记录
                    // foreach (var keypair in GlobalDef.S_LODS)
                    // {
                    //     var lod = keypair.Key;
                    //     var lodtransform = obj.Find(keypair.Value);
                    //     if (lodtransform != null)
                    //     {
                    //         // 包围盒中心点和实际位置的偏移
                    //         var offset = bound.center - lodtransform.position;
                    //         offset.y = 0;
                    //         var allChild = new List<Transform>();
                    //         lodtransform.GetComponentsInChildren(true, allChild);
                    //         foreach (var child in allChild)
                    //         {
                    //             var meshFilter = child.GetComponent<MeshFilter>();
                    //             var renderer = child.GetComponent<MeshRenderer>();
                    //
                    //             if (meshFilter != null && renderer != null)
                    //             {
                    //                 if (meshFilter.sharedMesh == null)
                    //                 {
                    //                     Debug.LogWarning($"no sharedMesh {child.name}");
                    //                     continue;
                    //                 }
                    //
                    //                 // Leon-TODO: 限制最大lod, GC
                    //                 var matName = renderer.sharedMaterial.name;
                    //                 var meshName = meshFilter.sharedMesh.name;
                    //
                    //                 // res
                    //                 var resId = regionResResDataList.FindIndex(item =>
                    //                     item.meshName.Equals(meshName) &&
                    //                     item.materialName.Equals(matName) &&
                    //                     item.renderLod == lod
                    //                 );
                    //                 if (resId == -1)
                    //                 {
                    //                     resId = regionResResDataList.Count;
                    //
                    //                     meshLst.Add(meshFilter.sharedMesh);
                    //                     materailLst.Add(renderer.sharedMaterial);
                    //                     regionResResDataList.Add(
                    //                         new WorldRegionResSerializeData(
                    //                             resId,
                    //                             meshName,
                    //                             matName,
                    //                             renderer.sharedMaterial.enableInstancing ? 2 : 1,
                    //                             lod,
                    //                             1));
                    //                 }
                    //
                    //                 WorldRenderVariable variable = null;
                    //                 var listId = variableLst.FindIndex(item => item.resId == resId);
                    //                 if (listId == -1)
                    //                 {
                    //                     variable = new WorldRenderVariable();
                    //                     variable.resId = resId;
                    //                     variableLst.Add(variable);
                    //                 }
                    //                 else
                    //                 {
                    //                     variable = variableLst[listId];
                    //                 }
                    //
                    //                 // 区域数据
                    //                 variable.boundRadiusList.Add(offset.magnitude > boundsRadius / 2
                    //                     ? boundsRadius * 2
                    //                     : boundsRadius);
                    //                 variable.eulerList.Add(child.rotation.eulerAngles);
                    //                 variable.posList.Add(child.position);
                    //                 variable.scaleList.Add(child.lossyScale);
                    //                 regionTotal.regionObjTotalNum++;
                    //             }
                    //         }
                    //     }
                    // }

                    ExportMeshRes(obj.childCount, ref tabProgress, obj.name, obj, tileSize, tilex, tiley, ref regionDatas, ref regionDataLst, ref regionResResDataList, ref meshLst, ref materailLst, regionTotal, isUseOwn);

            }
        }

        #endregion


        #region FogOfWar
        public void RemoveFogOfWarData(int fogOfWarId)
        {
            fogOfWarDataMan.RemoveFogOfWarData(fogOfWarId);
        }

        public int AddFogOfWarData()
        {
            return fogOfWarDataMan.AddFogOfWarData();
        }
        #endregion

        public void Clear()
        {
            fogOfWarDataMan.Reset();
        }
    }
}
#endif