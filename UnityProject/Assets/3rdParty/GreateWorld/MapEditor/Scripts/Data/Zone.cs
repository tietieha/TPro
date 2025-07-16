#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GEngine.MapEditor
{
    public class EdgeMeshInfo
    {
        public int zoneId;

        public List<Vector3> vertexs = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uvs = new List<Vector2>();

        public Dictionary<int, int> idDict = new Dictionary<int, int>();

        public EdgeMeshInfo(int index)
        {
            zoneId = index;
        }

        public void Calc(ref List<Vector3> allVertexs, ref List<Vector2> allUvs)
        {
            List<int> needVerts = new List<int>();
            for (int i = 0; i < triangles.Count; i++)
            {
                if (!needVerts.Contains(triangles[i]))
                {
                    needVerts.Add(triangles[i]);
                }
            }

            idDict.Clear();
            vertexs.Clear();
            uvs.Clear();
            for (int i = 0; i < needVerts.Count; i++)
            {
                var id = needVerts[i];
                vertexs.Add(allVertexs[id]);
                uvs.Add(allUvs[id]);
                idDict[id] = i;
            }

            var newTriangles = new List<int>();
            for (int i = 0; i < triangles.Count; i++)
            {
                newTriangles.Add(idDict[triangles[i]]);
            }

            triangles = newTriangles;
        }

        public void DrawMesh()
        {
            int idx = 0;
            while (true)
            {
                if (idx + 2 >= triangles.Count)
                    break;

                var p0 = vertexs[triangles[idx]];
                var p1 = vertexs[triangles[idx + 1]];
                var p2 = vertexs[triangles[idx + 2]];
                var uv1 = uvs[triangles[idx]];
                var uv2 = uvs[triangles[idx + 1]];
                var uv3 = uvs[triangles[idx + 2]];

                idx += 3;
                var f1 = Random.Range(0f, 1f);
                var f2 = Random.Range(0f, 1f);
                var f3 = Random.Range(0f, 1f);
                GL.Color(new Color(f1, f2, f3));
                GL.TexCoord2(uv1.x, uv1.y);
                GL.Vertex(p0);
                GL.TexCoord2(uv2.x, uv2.y);
                GL.Vertex(p1);
                GL.TexCoord2(uv3.x, uv3.y);
                GL.Vertex(p2);
            }
        }
    }

    public class Tower
    {
        public Zone zone;
        public Hexagon hexagon;

        public Tower(Zone t, Hexagon hex)
        {
            zone = t;
            hexagon = hex;
        }
    }

    public class Zone
    {
        public static int S_MAX_ZoneLV = 9;

        public int index;

        public int posType;

        //zone交界处六边形列表
        public List<Hexagon> junctionHexagons = new List<Hexagon>();

        public int level
        {
            get { return _level; }

            set
            {
                if (_map != null && _level >= 0 && _level <= Zone.S_MAX_ZoneLV)
                {
                    _map.zoneLvInfos[_level].Del(this);
                }

                var changed = _level != value;
                _level = value;

                if (_map != null && _level >= 0 && _level <= Zone.S_MAX_ZoneLV)
                {
                    _map.zoneLvInfos[_level].Add(this);
                }

                if (changed)
                    MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateEvent, index);
            }
        }

        public int visible
        {
            get { return _visible; }

            set
            {
                _visible = value;
                if (_map != null && _level >= 0 && _level <= Zone.S_MAX_ZoneLV)
                {
                    if (_visible == 1)
                        _map.zoneLvInfos[_level].Add(this);
                    else
                        _map.zoneLvInfos[_level].Del(this);
                }
            }
        }

        public int isGuanqia // 是否关卡, 只导出给CSV，策划填表用
        {
            get { return _isGuanqia; }

            set
            {
                _isGuanqia = value;
                if (_map != null && _level >= 0 && _level <= Zone.S_MAX_ZoneLV)
                {
                    if (_isGuanqia == 1)
                    {
                        _map.zoneLvInfos[_level].Del(this);
                        _map.guanqiaLvInfos[_level].Add(this);
                    }
                    else
                    {
                        _map.guanqiaLvInfos[_level].Del(this);
                        if (_visible == 1)
                            _map.zoneLvInfos[_level].Add(this);
                    }
                }
            }
        }

        public int SizePreset = -1;

        public int expandRound = 0;
        public int ExpandRound
        {
            get
            {
                if (expandRound == 0)
                {
                    // expandRound = Random.Range(2, 6);
                    expandRound = 2;
                }

                return expandRound;
            }
            set
            {
                expandRound = Random.Range(value - 2, value +2);
            }
        }

        // 1 : 末日实验室
        public int subType = 0;

        // 归属遗迹编号 0，非遗迹
        public int ruinId = 0;

        public int color; // 显示的颜色索引
        public Hexagon hexagon; // 城点位置
        public ZoneLandform landform = ZoneLandform.LANDFORM1;
        public ZoneLandType landType = ZoneLandType.Sea;
        // 商圈编号
        public int businessZone = -1;

        //哨塔列表
        public Dictionary<Hexagon, Tower> towers = new Dictionary<Hexagon, Tower>();

        //自动布置的哨塔字典
        public Dictionary<Hexagon, Tower> autoTowersDic = new Dictionary<Hexagon, Tower>();

        //自动布置的哨塔列表
        public List<Tower> autoTowersList = new List<Tower>();
        //实际布置的哨塔字典
        //public Dictionary<Hexagon, Tower> actualTowersDic = new Dictionary<Hexagon, Tower>();

        public int isBorn = 0; // 是否出生城，默认不是出生城
        public List<Hexagon> hexagons = new List<Hexagon>();
        public List<Hexagon> edges = new List<Hexagon>();
        public List<Hexagon> keyEdges = new List<Hexagon>();

        public List<Vector3> path = new List<Vector3>();

        // edge mesh data
        public List<Vector3> vertexs = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<int> vertexSorts = new List<int>();
        public List<int> edgeNeigbours = new List<int>();

        public Dictionary<int, EdgeMeshInfo> edgeMeshs = new Dictionary<int, EdgeMeshInfo>();

        private int _level;
        private int _visible = 1;
        private int _isGuanqia = 0;
        private HexMap _map;

        // 相邻的区域
        public List<Zone> neigbourZones = new List<Zone>();

        // 港口坐标
        public int _portIndex = -1;
        // 港口航道点坐标
        public int _portSailIndex = -1;
        // 海港岛屿坐标
        public int _islandIndex = -1;
        // 海盗模板ID
        public int _islandId = -1;
        // 海盗随机旋转
        public int _islandRotate = 0;
        // 港口占地圈数
        public int _portCircles
        {
            get
            {
                if (_level > 2)
                {
                    return _map.Level2BeyondCircles;
                }
                else
                {
                    return _map.Level2BelowCircles;
                }
            }
        }

        //宝藏点(生效)
        public List<int> _validTreasures = new List<int>();
        //宝藏点(失效)
        public List<int> _invalidTreasures = new List<int>();
        public Zone(HexMap map, int idx, Hexagon hex, int type, int lv, int c)
        {
            _map = map;
            index = idx;
            posType = type;
            _level = lv;
            color = c;
            hexagon = hex;

            isGuanqia = 0;
            isBorn = 0;
            subType = 0;
            ruinId = 0; // 遗迹编号
            landform = ZoneLandform.LANDFORM1;
            landType = ZoneLandType.Sea;

            if (hexagon != null)
            {
                hexagon.zone = this;
                hexagons.Add(hex);
            }
        }

        public void Reset()
        {
            foreach (var hex in hexagons)
            {
                hex.Reset();
            }

            hexagons.Clear();

            if (hexagon != null)
            {
                hexagon.zone = this;
                hexagons.Add(hexagon);
            }
        }

        public void Clear()
        {
            hexagons.Clear();
            edges.Clear();
            neigbourZones.Clear();
        }

        public void Destroy()
        {
            foreach (var hex in hexagons)
            {
                hex.RemoveFromZone();
            }

            Clear();
        }

        public Bounds CalcBounds()
        {
            Bounds bounds = new Bounds();
            Vector2Int min = new Vector2Int(_map.width, _map.width);
            Vector2Int max = Vector2Int.zero;
            foreach (var hex in hexagons)
            {
                if (hex.x > max.x) max.x = hex.x;
                if (hex.x < min.x) min.x = hex.x;
                if (hex.y > max.y) max.y = hex.y;
                if (hex.y < min.y) min.y = hex.y;
            }
            bounds.SetMinMax(new Vector3Int(min.x, min.y), new Vector3Int(max.x, max.y));
            return bounds;
        }

        public bool ToggleTower(Hexagon hex)
        {
            if (hex.zone != this)
                return false;

            if (towers.ContainsKey(hex))
            {
                towers.Remove(hex);

                if (this.IsNeedAdjustment() == true)
                {
                    this._map.AddAdjustmentZone(hex.zone);
                }
                else
                {
                    this._map.RemoveAdjustmentZone(hex.zone);
                }

                return true;
            }

            towers.Add(hex, new Tower(this, hex));

            if (this.IsNeedAdjustment() == true)
            {
                this._map.AddAdjustmentZone(hex.zone);
            }
            else
            {
                this._map.RemoveAdjustmentZone(hex.zone);
            }

            return true;
        }

        public string GetTowerText()
        {
            if (towers.Count == 0)
                return "";

            bool bFirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (var v in towers.Values)
            {
                if (bFirst)
                    bFirst = false;
                else
                    sb.Append(";");
                sb.Append(v.hexagon.x - v.zone.hexagon.x);
                sb.Append("|");
                sb.Append(v.hexagon.y - v.zone.hexagon.y);
            }

            return sb.ToString();
        }

        public bool AddZone(Zone zone, out string error)
        {
            error = String.Empty;
            if (landType == ZoneLandType.Land || zone.landType == ZoneLandType.Land)
            {
                error = "陆地区域不能合并";
                return false;
            }

            // 不相邻的区域不能合并
            if (!neigbourZones.Contains(zone))
            {
                error = "不相邻的区域不能合并";
                return false;
            }

            foreach (var hex in zone.hexagons)
            {
                hex.RemoveFromZone();
                hex.zone = this;
            }

            hexagons.AddRange(zone.hexagons);
            zone.Clear();

            CalcEdges();
            return true;
        }

        public void RemoveHexagon(Hexagon hex)
        {
            hex.RemoveFromZone();
            hex.zone = null;
            hex.bEdge = false;
            hexagons.Remove(hex);
        }

        public bool AddHexagon(Hexagon hex, bool bForce = false)
        {
            if (hex == null)
                return false;

            if (landType == ZoneLandType.Land)
                _map.OnLandZoneChanged();

            if (bForce)
            {
                hex.RemoveFromZone();
                hex.zone = this;
                hex.bEdge = false;
                hex.movetype = this.landType == ZoneLandType.Land ? MOVETYPE.DISABLE : GlobalDef.S_DefaultMoveType;
                hexagons.Add(hex);
                return true;
            }

            for (int i = 0; i < hex.neigbours.Length; i++)
            {
                if (hex.neigbours[i].zone == this)
                {
                    hex.RemoveFromZone();
                    hex.zone = this;
                    hex.bEdge = false;
                    if (!hexagons.Contains(hex))
                        hexagons.Add(hex);
                    return true;
                }
            }

            return false;
        }

        public void AddHexagon4Port(Hexagon hex)
        {
            if (hex == null) return;

            hex.RemoveFromZone();
            hex.zone = this;
            hex.bEdge = false;
            hexagons.Add(hex);
        }

        // 随机选择一个点扩散
        public bool ExpandEx(int round)
        {
            var expands = new List<Hexagon>();
            foreach (var hex in hexagons)
            {
                if (hex.getExpandEdges() > 0)
                {
                    expands.Add(hex);
                }
            }

            if (expands.Count == 0)
            {
                Debug.LogFormat("Zone [{0}] done hexagon = {1} ", index, hexagons.Count);
                return false;
            }

            var start = expands[Random.Range(0, expands.Count)];

            // 随机选择一个点扩散
            List<Hexagon> curExpands = new List<Hexagon>();
            List<Hexagon> newExpands = new List<Hexagon>();
            curExpands.Add(start);
            expands.Remove(start);

            for (int k = 0; k < round; k++)
            {
                newExpands.Clear();
                foreach (var hex in curExpands)
                {
                    for (int i = 0; i < hex.neigbours.Length; i++)
                    {
                        if (hex.neigbours[i] == null)
                            continue;

                        if (hex.neigbours[i].zone == null)
                        {
                            hex.neigbours[i].zone = this;
                            newExpands.Add(hex.neigbours[i]);
                        }
                    }
                }

                if (newExpands.Count == 0)
                {
                    break;
                }

                curExpands.Clear();
                curExpands.AddRange(newExpands);
                hexagons.AddRange(newExpands);
            }

            return true;
        }

        public void SetKeyEdge(Hexagon hex)
        {
            if (keyEdges.Contains(hex))
            {
                keyEdges.Remove(hex);
                return;
            }

            keyEdges.Add(hex);
            var list = new List<Hexagon>();
            foreach (var t in edges)
            {
                if (keyEdges.Contains(t))
                    list.Add(t);
            }

            keyEdges = list;
        }

        // 向四周扩散一周
        public bool Expand(bool isDevour = false)
        {
            var oldZones = new Dictionary<int, Zone>();
            var expands = new List<Hexagon>();
            foreach (var hex in hexagons)
            {
                if (hex.getExpandEdges(isDevour) > 0)
                {
                    expands.Add(hex);
                }
            }

            if (expands.Count == 0)
            {
                Debug.LogFormat("Zone [{0}] done hexagon = {1} ", index, hexagons.Count);
                return false;
            }

            List<Hexagon> newExpands = new List<Hexagon>();
            foreach (var hex in expands)
            {
                for (int i = 0; i < hex.neigbours.Length; i++)
                {
                    if (hex.neigbours[i] == null)
                        continue;

                    //无法使用Worley Noise，只能用Voronoi，因为控制点是任意分布的，无法栅格
                    if (isDevour || hex.neigbours[i].zone == null) //  && GetMinDist(hex.neigbours[i]) == this
                    {
                        if (hex.neigbours[i].zone != null)
                        {
                            var oldZone = hex.neigbours[i].zone;
                            if (oldZone.index == index || oldZone.level > level)
                                continue;
                            oldZone.hexagons.Remove(hex.neigbours[i]);
                            if (!oldZones.ContainsKey(oldZone.index))
                            {
                                oldZones.Add(oldZone.index, oldZone);
                            }
                        }

                        hex.neigbours[i].zone = this;
                        newExpands.Add(hex.neigbours[i]);
                    }
                }
            }

            if (newExpands.Count == 0)
            {
                Debug.LogFormat("Zone [{0}] done hexagon = {1} ", index, hexagons.Count);
            }
            else
            {
                hexagons.AddRange(newExpands);
            }

            return expands.Count > 0;
        }

        private Zone GetMinDist(Hexagon hexagon)
        {
            // 遍历所有zone城点
            float minDis = float.MaxValue;
            Zone minDisZone = null;
            foreach (var zone in _map.zones.Values)
            {
                float dis = Vector3.Distance(zone.hexagon.Pos, hexagon.Pos);
                if (dis < minDis)
                {
                    minDis = dis;
                    minDisZone = zone;
                }
            }

            return minDisZone;
        }

        // 检查是否有飞地
        public bool CheckEnclave(Action<string, float> onProgress = null)
        {
            //hexagon
            HexMap.ResetFind();
            var tmp1 = new List<Hexagon>(hexagons);
            var tmp2 = new List<Hexagon>();
            tmp2.Add(hexagon);
            int count = 0;
            int total = hexagons.Count;
            while (true)
            {
                RemoveByEdges(index, ref tmp2);
                count += tmp2.Count;
                onProgress?.Invoke($"Zone {index} {count}/{total}", (float) count / total);
                if (tmp2.Count == 0)
                    break;
            }

            // 没有遍历到的就是飞地
            Hexagon hexEnclave = null;
            foreach (var hex in hexagons)
            {
                if (!hex.Dirty)
                {
                    hexEnclave = hex;
                    break;
                }
            }

            if (hexEnclave != null)
            {
                var str = $"Zone {index} 有飞地，请检查！！！ [{hexEnclave.x},{hexEnclave.y}]";
                Debug.LogError(str);
                if (EditorUtility.DisplayDialog("检查飞地", str, "确定"))
                {
                    MapRender.instance.FocusHexagon(hexEnclave);
                }

                return true;
            }

            return false;
        }

        List<Hexagon> _tempHexes = new List<Hexagon>();

        private Hexagon RemoveByEdges(int zoneIndex, ref List<Hexagon> toChecks)
        {
            _tempHexes.Clear();
            foreach (var hex in toChecks)
            {
                foreach (var t in hex.neigbours)
                {
                    if (t == null || t.Dirty) continue;
                    t.Dirty = true;
                    if (t.zone != null && t.zone.index == zoneIndex)
                    {
                        _tempHexes.Add(t);
                    }
                }
            }

            toChecks.Clear();
            toChecks.AddRange(_tempHexes);
            return null;
        }

        private float minJunctionHexagonDistance = 6.0f;

        private List<Hexagon> tmpJunctionHexagons = new List<Hexagon>();

        //找到每块zone上的交界点
        public void FindJunctionHexagons()
        {
            junctionHexagons.Clear();
            for (int i = 0; i < edges.Count; i++)
            {
                bool isMapBorderHexagon = _map.IsMapBorderHexagon(edges[i]);
                //是地图边界六边形
                if (isMapBorderHexagon == true)
                {
                    bool isMultiZonesHexagon = edges[i].IsMultiZonesHexagon(2);
                    if (isMultiZonesHexagon == true)
                    {
                        FilterIntensiveJunctionHexagon(edges[i]);
                    }
                }
                else
                {
                    bool isMultiZonesHexagon = edges[i].IsMultiZonesHexagon(3);
                    if (isMultiZonesHexagon == true)
                    {
                        FilterIntensiveJunctionHexagon(edges[i]);
                    }
                }
            }
        }

        //去掉离得过近的交界处的点
        private void FilterIntensiveJunctionHexagon(Hexagon hexagon)
        {
            Vector2Int v1 = Vector2Int.zero;
            Vector2Int v2 = Vector2Int.zero;
            if (junctionHexagons.Count == 0)
            {
                junctionHexagons.Add(hexagon);
            }
            else
            {
                v1.x = junctionHexagons[junctionHexagons.Count - 1].x;
                v1.y = junctionHexagons[junctionHexagons.Count - 1].y;
                v2.x = hexagon.x;
                v2.y = hexagon.y;
                float distance = Vector2.Distance(v1, v2);
                if (distance > minJunctionHexagonDistance)
                {
                    junctionHexagons.Add(hexagon);
                }
            }
        }

        //每次自动哨塔迭代的步长
        public const int baseAutoStep = 10;

        //每次实际哨塔迭代的步长
        public const int baseActualStep = 20;
        public Vector2 tmpDeltaV2 = Vector2.zero;

        public Vector2Int tmpTowerPos = Vector2Int.zero;

        //自动布置哨塔
        public void AutoSetupTowers()
        {
            InitSearchDictionary();
            //生成Auto哨塔 作为后面计算的基准点
            for (int i = 0; i < junctionHexagons.Count; i++)
            {
                PlaceTargetAutoTower(junctionHexagons[i].x, junctionHexagons[i].y);
            }

            AdjustTowers();
        }

        public int GetActualNeedsTowerCount()
        {
            return this._map.cityLevel2TowerArray[level];
        }

        public class CompareTower
        {
            //哨塔到与它相邻的一个哨塔的距离(顺时针)
            public float dis;
            public Tower tower;
            public Tower nextTower;
        }

        //根据城市等级调整哨塔数目
        //补点规则，最多补点数小于等于compareTowersList中元素个数，即每条边只补一个点，不够的由策划手动添加
        private List<Vector2Int> tmpBaseTowerList = new List<Vector2Int>();
        private List<CompareTower> compareTowersList = new List<CompareTower>();

        public void AdjustTowers()
        {
            compareTowersList.Clear();
            tmpBaseTowerList.Clear();
            int autoTowerCount = autoTowersDic.Count;

            //获取对应zone上的哨塔数目
            int actualTowerCount = this._map.cityLevel2TowerArray[level];
            int reduceCount = 0;
            int addCount = 0;
            int curX = 0;
            int curY = 0;
            int nextX = 0;
            int nextY = 0;
            //构建哨塔与哨塔之间的关系列表
            for (int i = 0; i < autoTowersList.Count; i++)
            {
                CompareTower compareTower = new CompareTower();
                //尾部元素
                if (i == autoTowersList.Count - 1)
                {
                    curX = autoTowersList[i].hexagon.x;
                    curY = autoTowersList[i].hexagon.y;
                    nextX = autoTowersList[0].hexagon.x;
                    nextY = autoTowersList[0].hexagon.y;
                    compareTower.nextTower = autoTowersList[0];
                }
                else
                {
                    curX = autoTowersList[i].hexagon.x;
                    curY = autoTowersList[i].hexagon.y;
                    nextX = autoTowersList[i + 1].hexagon.x;
                    nextY = autoTowersList[i + 1].hexagon.y;
                    compareTower.nextTower = autoTowersList[i + 1];
                }

                int sum = (curX - nextX) * (curX - nextX) + (curY - nextY) * (curY - nextY);
                float dis = Mathf.Sqrt(sum);

                compareTower.tower = autoTowersList[i];
                compareTower.dis = dis;
                compareTowersList.Add(compareTower);
            }


            //自动布置的哨塔数大于实际需要哨塔数 需要减少自动布置的哨塔
            if (autoTowerCount > actualTowerCount)
            {
                reduceCount = autoTowerCount - actualTowerCount;

                //按距离升序排序
                compareTowersList.Sort((a, b) => { return a.dis <= b.dis ? -1 : 1; });

                for (int i = 0; i < compareTowersList.Count; i++)
                {
                    if (i + 1 > reduceCount)
                    {
                        //actualTowersDic.Add(compareTowersList[i].tower.hexagon, compareTowersList[i].tower);
                        if (towers.ContainsKey(compareTowersList[i].tower.hexagon))
                        {
                            Debug.LogError("Error");
                        }
                        else
                        {
                            towers.Add(compareTowersList[i].tower.hexagon, compareTowersList[i].tower);
                        }
                    }
                }
            }
            //自动布置的哨塔数等于实际需要哨塔数
            else if (autoTowerCount == actualTowerCount)
            {
                //actualTowersDic = autoTowersDic;
                towers = autoTowersDic;
            }
            //自动布置的哨塔数小于实际需要哨塔数
            else
            {
                //按距离降序排序
                compareTowersList.Sort((a, b) => { return a.dis <= b.dis ? 1 : -1; });
                //需要增加的哨塔数
                addCount = actualTowerCount - autoTowerCount;

                if (addCount > compareTowersList.Count)
                {
                    addCount = compareTowersList.Count;
                }

                for (int i = 0; i < compareTowersList.Count; i++)
                {
                    Vector2Int tmpPos = Vector2Int.zero;
                    if (i < addCount)
                    {
                        curX = compareTowersList[i].tower.hexagon.x;
                        curY = compareTowersList[i].tower.hexagon.y;
                        nextX = compareTowersList[i].nextTower.hexagon.x;
                        nextY = compareTowersList[i].nextTower.hexagon.y;
                        tmpPos.x = Mathf.FloorToInt((curX + nextX) / 2);
                        tmpPos.y = Mathf.FloorToInt((curY + nextY) / 2);
                        tmpBaseTowerList.Add(tmpPos);
                    }
                }

                //actualTowersDic = autoTowersDic;
                towers = autoTowersDic;
                //迭代新增加的点
                for (int i = 0; i < tmpBaseTowerList.Count; i++)
                {
                    PlaceTargetActualTower(tmpBaseTowerList[i].x, tmpBaseTowerList[i].y);
                }
            }
        }


        //扩散圈数
        private int roundCount = 3;

        //判断是否离边界点较近
        private bool IsNearbyJunction(Hexagon hexagon)
        {
            bool result = false;
            _tempHexes.Clear();
            hexagon.GetRoundHexagons(ref _tempHexes, roundCount);
            int index = hexagon.zone.index;
            for (int i = 0; i < _tempHexes.Count; i++)
            {
                if (_tempHexes[i].zone.index != index)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        //放置自动生成的哨塔
        //最大迭代次数
        private const int maxIterationCount = 3;
        int autoIterationCount = 0;

        private void PlaceTargetAutoTower(int posX, int posY)
        {
            //int step = baseStep - iterationCount * 2;
            int step = baseAutoStep;
            //超过迭代次数，还没有找到合适的坐标点
            if (autoIterationCount > maxIterationCount)
            {
                Debug.LogError("超过迭代次数还没有找到合适的自动哨塔点 Zone index: " + this.index + " autoIterationCount: " +
                               autoIterationCount + " position: " + tmpTowerPos.x + " " + tmpTowerPos.y);
                autoIterationCount = 0;
                return;
            }

            //算基准点的时候要先往里面推进
            int deltaX = hexagon.x - posX;
            int deltaY = hexagon.y - posY;
            tmpDeltaV2.x = deltaX;
            tmpDeltaV2.y = deltaY;
            tmpDeltaV2.Normalize();
            tmpDeltaV2.x = Mathf.FloorToInt(tmpDeltaV2.x * step);
            tmpDeltaV2.y = Mathf.FloorToInt(tmpDeltaV2.y * step);
            tmpTowerPos.x = posX + (int) tmpDeltaV2.x;
            tmpTowerPos.y = posY + (int) tmpDeltaV2.y;

            autoIterationCount++;

            //zone 中存在该六边形
            if (searchHexagonDic.ContainsKey(tmpTowerPos))
            {
                if (autoTowersDic.ContainsKey(searchHexagonDic[tmpTowerPos]) == false)
                {
                    bool isBlock = _map.IsBlock(searchHexagonDic[tmpTowerPos].x, searchHexagonDic[tmpTowerPos].y);

                    bool isNearbyJunction = IsNearbyJunction(searchHexagonDic[tmpTowerPos]);
                    //是阻挡或者是临近交界处
                    if (isBlock == true || isNearbyJunction == true)
                    {
                        PlaceTargetAutoTower(tmpTowerPos.x, tmpTowerPos.y);
                    }
                    else
                    {
                        Tower tower = new Tower(this, searchHexagonDic[tmpTowerPos]);
                        autoTowersDic.Add(searchHexagonDic[tmpTowerPos], tower);
                        autoTowersList.Add(tower);
                        autoIterationCount = 0;
                        return;
                    }
                }
            }
            else
            {
                PlaceTargetAutoTower(tmpTowerPos.x, tmpTowerPos.y);
            }
        }

        //放置实际的哨塔
        //距离中心城市的最小距离
        const float minDistance2City = 10.0f;
        Vector2Int cityPos = Vector2Int.zero;
        int actualIterationCount = 0;

        private void PlaceTargetActualTower(int posX, int posY)
        {
            //int step = baseStep - iterationCount * 2;
            int step = baseActualStep;
            //超过迭代次数，还没有找到合适的坐标点
            if (actualIterationCount > maxIterationCount)
            {
                Debug.LogError("超过迭代次数还没有找到合适的实际哨塔点 Zone index: " + this.index + " actualIterationCount: " +
                               actualIterationCount + " position: " + tmpTowerPos.x + " " + tmpTowerPos.y);
                actualIterationCount = 0;
                return;
            }

            cityPos.x = hexagon.x;
            cityPos.y = hexagon.y;
            tmpTowerPos.x = posX;
            tmpTowerPos.y = posY;


            if (Vector2Int.Distance(tmpTowerPos, cityPos) < minDistance2City)
            {
                //远离中心城市
                int deltaX = posX - cityPos.x;
                int deltaY = posY - cityPos.y;
                tmpDeltaV2.x = deltaX;
                tmpDeltaV2.y = deltaY;
                tmpDeltaV2.Normalize();
                tmpDeltaV2.x = Mathf.FloorToInt(tmpDeltaV2.x * step);
                tmpDeltaV2.y = Mathf.FloorToInt(tmpDeltaV2.y * step);
                tmpTowerPos.x = posX + (int) tmpDeltaV2.x;
                tmpTowerPos.y = posY + (int) tmpDeltaV2.y;
            }


            actualIterationCount++;

            //zone 中存在该六边形
            if (searchHexagonDic.ContainsKey(tmpTowerPos))
            {
                if (towers.ContainsKey(searchHexagonDic[tmpTowerPos]) == false)
                {
                    bool isBlock = _map.IsBlock(searchHexagonDic[tmpTowerPos].x, searchHexagonDic[tmpTowerPos].y);

                    bool isNearbyJunction = IsNearbyJunction(searchHexagonDic[tmpTowerPos]);
                    //是阻挡或者是临近交界处
                    if (isBlock == true || isNearbyJunction == true)
                    {
                        int deltaX = cityPos.x - posX;
                        int deltaY = cityPos.y - posY;
                        tmpDeltaV2.x = deltaX;
                        tmpDeltaV2.y = deltaY;
                        tmpDeltaV2.Normalize();
                        tmpDeltaV2.x = Mathf.FloorToInt(tmpDeltaV2.x * step / 10);
                        tmpDeltaV2.y = Mathf.FloorToInt(tmpDeltaV2.y * step / 10);
                        tmpTowerPos.x = posX + (int) tmpDeltaV2.x;
                        tmpTowerPos.y = posY + (int) tmpDeltaV2.y;
                        PlaceTargetActualTower(tmpTowerPos.x, tmpTowerPos.y);
                    }
                    else
                    {
                        Tower tower = new Tower(this, searchHexagonDic[tmpTowerPos]);
                        towers.Add(searchHexagonDic[tmpTowerPos], tower);
                        actualIterationCount = 0;
                        return;
                    }
                }
            }
            else
            {
                PlaceTargetActualTower(tmpTowerPos.x, tmpTowerPos.y);
            }
        }


        public Dictionary<Vector2, Hexagon> searchHexagonDic = new Dictionary<Vector2, Hexagon>();

        private Vector2 tmpSearchHexagonV2 = Vector2.zero;

        //使用字典按坐标存储六边形，（哈希结构，查找复杂度接近于O(1)）
        public void InitSearchDictionary()
        {
            searchHexagonDic.Clear();
            for (int i = 0; i < hexagons.Count; i++)
            {
                tmpSearchHexagonV2.x = hexagons[i].x;
                tmpSearchHexagonV2.y = hexagons[i].y;
                if (searchHexagonDic.ContainsKey(tmpSearchHexagonV2))
                {
                    Debug.LogError("Zone: " + this.index + " 的六边形列表中存在相同的六边形");
                }
                else
                {
                    searchHexagonDic.Add(tmpSearchHexagonV2, hexagons[i]);
                }
            }
        }

        public bool IsNeedAdjustment()
        {
            bool isNeedAdjustment = false;
            int actualTowerCount = _map.cityLevel2TowerArray[level];

            //if (actualTowersDic.Count != actualTowerCount)
            if (towers.Count != actualTowerCount)
            {
                isNeedAdjustment = true;
            }

            return isNeedAdjustment;
        }


        //清除自动布置哨塔数据
        public void ClearAutoTowers()
        {
            junctionHexagons.Clear();
            autoTowersDic.Clear();
            autoTowersList.Clear();
            //actualTowersDic.Clear();
            towers.Clear();
        }

        //

        // 测试边界算法
        // Hexagon _startHex = null;
        // Hexagon _curHex = null;
        // Hexagon _lastHex = null;
        // int _dir = 1;
        //
        // public void TestEdges()
        // {
        //     foreach (var hex in hexagons)
        //     {
        //         hex.bEdge = false;
        //     }
        //     edges.Clear();
        //
        //     if (hexagons.Count == 0)
        //         return;
        //
        //     _curHex = null;
        //
        //     for (int i = hexagons.Count - 1; i >= 0; i--)
        //     {
        //         if (hexagons[i].getEdges() > 0)
        //         {
        //             _curHex = hexagons[i];
        //             break;
        //         }
        //     }
        //
        //     if (_curHex == null)
        //     {
        //         Debug.LogError($"CalcEdges [{index}] error!!!");
        //         return;
        //     }
        //
        //     _startHex = _curHex;
        //     _lastHex = null;
        //     edges.Add(_curHex);
        //     var retHex = _curHex.GetEdgeHex(edges, null, 1);
        //     edges.Add(retHex);
        //
        //     _dir = -1;
        //     var idx = retHex.GetNeigbourIndex(_curHex);
        //     idx += 1;
        //     if (idx < 0) idx = 5;
        //     if (idx > 5) idx = 0;
        //     if (retHex.neigbours[idx] == null || retHex.neigbours[idx].zone != this)
        //     {
        //         _dir = 1;
        //     }
        //
        //     _lastHex = _curHex;
        //     _curHex = retHex;
        // }
        //
        // public void TestNextEdge()
        // {
        //     var retHex = _curHex.GetEdgeHex(edges, _lastHex, _dir);
        //     if (retHex == _startHex)
        //     {
        //         return;
        //     }
        //
        //     if (retHex == null || retHex == _curHex)
        //     {
        //         return;
        //     }
        //
        //     retHex.bEdge = true;
        //     edges.Add(retHex);
        //     _lastHex = _curHex;
        //     _curHex = retHex;
        // }


        // 计算区域边界的六角格
        public bool CalcEdges()
        {
            foreach (var hex in hexagons)
            {
                hex.edgeIndex = -1;
                hex.bEdge = false;
            }

            edges.Clear();

            if (hexagons.Count == 0)
                return true;

            Hexagon curHex = null;

            int i;
            for (i = hexagons.Count - 1; i >= 0; i--)
            {
                if (hexagons[i].getEdges() > 0)
                {
                    curHex = hexagons[i];
                    break;
                }
            }

            if (curHex == null)
            {
                Debug.LogWarning($"CalcEdges [{index}] no edges !!!");
                //EditorUtility.DisplayDialog("刷新边界", $"Zone {index} 没有边界！", "确定");
                return false;
            }

            Hexagon start = curHex;
            Hexagon last = null;
            edges.Add(curHex);

            // 找正方向的相邻边界点
            Hexagon retHex = null;
            var rets = curHex.GetAllEdgeHex();
            for (i = 0; i < rets.Count; i++)
            {
                retHex = rets[i];
                var idx = retHex.GetNeigbourIndex(curHex);
                idx += 1;
                if (idx < 0) idx = 5;
                if (idx > 5) idx = 0;
                //if (retHex.IsEdge(retHex.neigbours[i]))
                if (retHex.neigbours[idx] == null || retHex.neigbours[idx].zone != this)
                {
                    break;
                }
            }

            if (retHex == null)
            {
                Debug.LogError($"CalcEdges [{index}] error!!!");
                EditorUtility.DisplayDialog("刷新边界", $"Zone {index} 计算异常！", "确定");
                return false;
            }

            edges.Add(retHex);

            last = curHex;
            curHex = retHex;

            while (true)
            {
                retHex = curHex.GetEdgeHex(edges, last, 1);
                if (retHex == start)
                {
                    break;
                }

                if (retHex == null || retHex == curHex)
                {
                    break;
                }

                edges.Add(retHex);

                last = curHex;
                curHex = retHex;

                if (edges.Count > 10000)
                    break;
            }

            neigbourZones.Clear();
            foreach (var hex in edges)
            {
                hex.bEdge = true;
                hex.edgeIndex = 0;
                hex.getNeigbourZones(ref neigbourZones);
            }

            //CalcEdgeLine();
            CalcEdgeMesh();

            // 计算向内推一格的边界
            CalcEdges(out List<Hexagon> outEdges, 1);

            return true;
        }

        public void CalcEdges(out List<Hexagon> outEdges, int edgeIndex)
        {
            outEdges = new List<Hexagon>();

            foreach (var hex in hexagons)
            {
                if (hex.edgeIndex >= edgeIndex)
                    hex.edgeIndex = -1;
            }

            outEdges.Clear();

            if (hexagons.Count == 0)
                return;

            Hexagon curHex = null;

            int i;
            for (i = hexagons.Count - 1; i >= 0; i--)
            {
                if (hexagons[i].edgeIndex == -1 && hexagons[i].getEdges(edgeIndex) > 0)
                {
                    curHex = hexagons[i];
                    break;
                }
            }

            if (curHex == null)
            {
                Debug.LogError($"CalcEdges [{index}] error1!!!");
                return;
            }

            Hexagon start = curHex;
            Hexagon last = null;
            outEdges.Add(curHex);

            // 找正方向的相邻边界点
            Hexagon retHex = null;
            var rets = curHex.GetAllEdgeHex(edgeIndex);
            if (rets.Count == 0)
            {
                //Debug.LogError($"CalcEdges [{index}] error2!!!");
                return;
            }

            for (i = 0; i < rets.Count; i++)
            {
                retHex = rets[i];
                var idx = retHex.GetNeigbourIndex(curHex);
                idx += 1;
                if (idx < 0) idx = 5;
                if (idx > 5) idx = 0;
                if (retHex.IsEdge(retHex.neigbours[idx], edgeIndex))
                    //if (retHex.neigbours[idx] == null || retHex.neigbours[idx].zone != this)
                {
                    break;
                }
            }

            outEdges.Add(retHex);

            last = curHex;
            curHex = retHex;

            while (true)
            {
                retHex = curHex.GetEdgeHex(outEdges, last, 1, edgeIndex);
                if (retHex == start)
                {
                    break;
                }

                if (retHex == null || retHex == curHex)
                {
                    break;
                }

                outEdges.Add(retHex);
                last = curHex;
                curHex = retHex;

                if (outEdges.Count > 10000)
                    break;
            }

            foreach (var hex in outEdges)
            {
                hex.edgeIndex = edgeIndex;
            }
        }

        //// 平滑线条
        //var dels = new List<Hexagon>();

        //i = 0;
        //while (i < edges.Count)
        //{
        //    var cur = edges[i];
        //    if (i + 3 >= edges.Count)
        //        break;

        //    var nex1 = edges[i + 1];
        //    var nex2 = edges[i + 2];
        //    var idx1 = cur.GetNeigbourIndex(nex1);
        //    var idx2 = nex1.GetNeigbourIndex(nex2);
        //    var sub = idx1 - idx2;

        //    if (idx1 == idx2)
        //    {
        //        i += 2;
        //        continue;
        //    }

        //    var old1 = idx1;
        //    var old2 = idx2;

        //    if (Mathf.Abs(idx1 - idx2) == 1)
        //    {
        //        var oldNext = nex1;
        //        int index = i + 2;
        //        while (index + 1 < edges.Count)
        //        {
        //            index++;
        //            nex1 = nex2;
        //            nex2 = edges[index];
        //            idx1 = idx2;
        //            idx2 = nex1.GetNeigbourIndex(nex2);
        //            if (idx1 == old2 && idx2 == old1)
        //            {
        //                old1 = idx1;
        //                old2 = idx2;
        //                dels.Add(nex1);
        //            }
        //            else
        //                break;
        //        }
        //        if (index > i + 3)
        //        {
        //            dels.Add(oldNext);
        //        }
        //        i = index;
        //    }
        //    else
        //    {
        //        i++;
        //    }
        //}

        //foreach (var hex in dels)
        //{
        //    edges.Remove(hex);
        //}

        public void DoSmooth()
        {
            Dictionary<Hexagon, Zone> adjusts = new Dictionary<Hexagon, Zone>();
            foreach (var hex in hexagons)
            {
                if (hex.DoSmooth(out Zone smoothZone))
                {
                    adjusts[hex] = smoothZone;
                }
            }

            foreach (var it in adjusts)
            {
                var zone = it.Value;
                if (zone != null)
                {
                    hexagons.Remove(it.Key);
                    zone.AddHexagon(it.Key, true);
                }
            }
        }
        //
        // public void CalcEdgeLine()
        // {
        //     if (edges.Count <= 3)
        //         return;
        //
        //     path.Clear();
        //
        //     int begin;
        //     int end;
        //     Hexagon last = null;
        //     Hexagon cur = null;
        //     int idx, dir = 1;
        //
        //     for (int i = 0; i < edges.Count; i++)
        //     {
        //         cur = edges[i];
        //         begin = cur.GetEdgeBegin(dir, last);
        //         end = cur.GetEdgeEnd(dir, begin);
        //
        //         for (int k = 1; k < 6; k++)
        //         {
        //             idx = begin + k;
        //             if (idx > 5) idx -= 6;
        //             if (idx == end)
        //                 break;
        //
        //
        //             path.Add(_map.GetHexagonVertex(cur.x, cur.y, idx));
        //         }
        //
        //         last = cur;
        //     }
        // }

        // 计算边界mesh的数据
        public void CalcEdgeMesh()
        {
            if (edges.Count <= 3)
                return;

            vertexs.Clear();
            triangles.Clear();
            uvs.Clear();

            int begin;
            int end;
            Hexagon last = null;
            Hexagon cur = null;
            int idx, dir = 1;
            int startIdx = -1;
            int curIdx = -1;
            int lastEdgeIdx = -1;

            int vertCount1 = 0; // 内边界的点
            int vertCount2 = 0; // 外边界的点

            vertexSorts.Clear();
            edgeNeigbours.Clear();

            var edgeCount = 0;
            var neigbourZoneId = 0;

            for (int i = 0; i < edges.Count; i++)
            {
                cur = edges[i];
                begin = cur.GetEdgeBegin(dir, last);
                end = cur.GetEdgeEnd(dir, begin);

                startIdx = vertexs.Count;
                vertexs.Add(_map.GetHexagonPos(cur));
                Debug.Log($"-----------------------first index = {cur.index}, curIdx = {curIdx}");
                vertCount1++;
                vertexSorts.Add(1);

                if (vertexs.Count > 2)
                {
                    // 完成与上一个六角格相交的区域
                    triangles.Add(startIdx);
                    triangles.Add(curIdx);
                    triangles.Add(curIdx - 1 == startIdx ? curIdx - 2 : curIdx - 1);

                    idx = begin + 1;
                    if (idx > 5) idx -= 6;

                    neigbourZoneId = (cur.neigbours[idx] != null && cur.neigbours[idx].zone != null)
                        ? cur.neigbours[idx].zone.index
                        : -1;
                    edgeNeigbours.Add(neigbourZoneId);
                    if (neigbourZoneId == index)
                    {
                        int ttt1 = 0;
                    }
                }

                for (int k = 1; k < 6; k++)
                {
                    idx = begin + k;
                    if (idx > 5) idx -= 6;
                    if (idx == end)
                    {
                        // 与下一个六角格相邻的点
                        curIdx = vertexs.Count;
                        vertexs.Add(_map.GetHexagonVertex(cur.x, cur.y, idx));
                        vertCount1++;
                        vertexSorts.Add(1);

                        triangles.Add(startIdx);
                        triangles.Add(lastEdgeIdx);
                        triangles.Add(curIdx);
                        Debug.Log($"-----------------------lin index = {cur.index}, idx = {idx}, startIdx = {startIdx}, lastEdgeIdx = {lastEdgeIdx}, curIdx = {curIdx}, ");
                        if (edgeNeigbours.Count > 0)
                        {
                            neigbourZoneId = edgeNeigbours[edgeNeigbours.Count - 1];
                        }
                        else
                            neigbourZoneId = -1;

                        edgeNeigbours.Add(neigbourZoneId);
                        if (neigbourZoneId == index)
                        {
                            int ttt2 = 0;
                        }

                        break;
                    }

                    curIdx = vertexs.Count;
                    vertexs.Add(_map.GetHexagonVertex(cur.x, cur.y, idx));
                    vertCount2++;
                    vertexSorts.Add(2);
                    edgeCount++;

                    if (lastEdgeIdx >= 0)
                    {
                        triangles.Add(startIdx);
                        triangles.Add(lastEdgeIdx);
                        triangles.Add(curIdx);
                        Debug.Log($"-----------------------index = {cur.index} idx = {idx}, startIdx = {startIdx}, lastEdgeIdx = {lastEdgeIdx}, curIdx = {curIdx}");

                        neigbourZoneId = (cur.neigbours[idx] != null && cur.neigbours[idx].zone != null)
                            ? cur.neigbours[idx].zone.index
                            : -1;
                        edgeNeigbours.Add(neigbourZoneId);
                        if (neigbourZoneId == index)
                        {
                            int ttt3 = 0;
                        }
                    }

                    lastEdgeIdx = curIdx;
                }

                last = cur;
            }

            cur = edges[0];
            var lastIdx = vertexs.Count;
            vertexs.Add(_map.GetHexagonPos(cur));
            Debug.Log($"-----------------------index = {cur.index} curIdx = {curIdx}");
            vertCount1++;
            vertexSorts.Add(1);

            vertexs.Add(vertexs[1]);
            vertCount2++;
            vertexSorts.Add(2);
            Debug.Log($"-----------------------ffff index = {cur.index} vertexs[1]");

            triangles.Add(lastIdx);
            triangles.Add(lastEdgeIdx + 1);
            triangles.Add(lastEdgeIdx);
            Debug.Log($"-----------------------index = {cur.index} startIdx = {startIdx}, lastEdgeIdx = {lastEdgeIdx}, curIdx = {curIdx}");

            idx = cur.GetEdgeBegin(dir, null) + 1;

            if (idx < cur.neigbours.Length)
                neigbourZoneId = (cur.neigbours[idx] != null && cur.neigbours[idx].zone != null)
                    ? cur.neigbours[idx].zone.index
                    : -1;
            else
                neigbourZoneId = -1;

            edgeNeigbours.Add(neigbourZoneId);
            if (neigbourZoneId == index)
            {
                int ttt4 = 0;
            }

            triangles.Add(lastIdx);
            triangles.Add(lastEdgeIdx);
            triangles.Add(lastIdx + 1);

            edgeNeigbours.Add(neigbourZoneId);

            // 计算uv
            float step1 = 1f;
            float step2 = 1f;
            if (vertCount1 > vertCount2)
            {
                step1 = (float) vertCount2 / vertCount1;
            }
            else
            {
                step2 = (float) vertCount1 / vertCount2;
            }

            int num1 = 0;
            int num2 = 0;
            for (int i = 0; i < vertexSorts.Count; i++)
            {
                var type = vertexSorts[i];
                if (type == 1)
                {
                    uvs.Add(new Vector2(num1 * step1, 0f));
                    num1++;
                }
                else
                {
                    uvs.Add(new Vector2(num2 * step2, 1f));
                    num2++;
                }
            }

            // 按相邻的边界分成几段。
            EdgeMeshInfo curMesh = null;
            edgeMeshs.Clear();
            for (int i = 0; i < edgeNeigbours.Count; i++)
            {
                var zoneId = edgeNeigbours[i];
                if (zoneId == index)
                {
                    Debug.LogError("Calc zone edgeMesh error : " + zoneId);
                }

                if (curMesh == null || curMesh.zoneId != zoneId)
                {
                    if (!edgeMeshs.ContainsKey(zoneId))
                    {
                        curMesh = new EdgeMeshInfo(zoneId);
                        edgeMeshs[zoneId] = curMesh;
                    }
                    else
                    {
                        curMesh = edgeMeshs[zoneId];
                    }
                }

                curMesh.triangles.Add(triangles[i * 3]);
                curMesh.triangles.Add(triangles[i * 3 + 1]);
                curMesh.triangles.Add(triangles[i * 3 + 2]);
            }

            foreach (var mesh in edgeMeshs.Values)
            {
                mesh.Calc(ref vertexs, ref uvs);
            }

            Debug.LogFormat("zone {0} => subMesh : {1}", index, edgeMeshs.Count);
        }

        private void CalcBusinessMesh()
        {

        }

        //生成港口

        //周围陆地数量
        public void GeneratePort()
        {
            if (_portIndex > 0) return;
            if (landType == ZoneLandType.Land) return;
            if (_map.islandTemplates.Count  == 0) return;

            var earth = new HashSet<int>();
            foreach (var _hex in hexagons)
            {
                _hex.GetRoundEarth(ref earth);
            }

            if(earth.Count > 18)
            {
                GenerateEarthPort(ref earth);
            }
            else
            {
                GenerateSeaPort();
            }
        }

        private void GenerateEarthPort(ref HashSet<int> earth)
        {
            var array = earth.ToArray();
            //取最靠近海域中心的陆地
            float min = 100000f;
            var hex = GetZoneCenter();
            var center = _map.GetHexagonPos(hex.x, hex.y);
            for (int i = 0; i < array.Length; i++)
            {
                var pos = _map.GetHexagonPos(array[i]);
                var distance = Vector3.Distance(center, pos);
                if (distance < min)
                {
                    min = distance;
                    _portIndex = array[i];
                }
            }

            if (_portIndex > 0)
            {
                var portHex = _map.GetHexagon(_portIndex);
                var portAxial = new Axial(portHex.x, portHex.y);
                var ring = Axial.Ring4Port(_map, portAxial, 1);
                var water = new List<Axial>();
                foreach (var ax in ring)
                {
                    var axHex = _map.GetHexagon(ax.q, ax.r);
                    if (axHex.movetype != MOVETYPE.DISABLE)
                    {
                        water.Add(ax);
                    }
                }

                var spiral = Axial.Spiral4Port(_map, portAxial, _portCircles);
                foreach (var ax in spiral)
                {
                    var axHex = _map.GetHexagon(ax.q, ax.r);
                    //把港口占地区域归入海域
                    if (axHex.zone != this)
                    {
                        axHex.zone.hexagons.Remove(axHex);
                        AddHexagon(axHex, true);
                    }
                }

                var offsetSum = Vector3.zero;
                foreach (var ax in water)
                {
                    offsetSum += _map.GetHexagonPos(ax);
                }

                var average = offsetSum / water.Count;
                var portPos = _map.GetHexagonPos(_portIndex);
                float angle = CalSailAngle(average - portPos);
                CalPortSailPoint(angle);
            }
            else
            {
                Debug.LogError("生成陆地港口失败！");
            }
        }

        //获取海域中心世界坐标
        public Vector3 GetZoneCenterWp()
        {
            Vector3 sumOffset = Vector3.zero;
            foreach (var hex in hexagons)
            {
                sumOffset += _map.GetHexagonPos(hex);
            }

            return sumOffset / hexagons.Count;
        }

        //获取海域中心坐标
        public Vector2Int GetZoneCenter()
        {
            var offset = GetZoneCenterWp();
            return Hex.WorldToOffset(offset);
        }

        public void AssignIslandAfterLoad()
        {
            if (_islandIndex > 0)
            {
                var islandHex = _map.GetHexagon(_islandIndex);
                if (_map.islandTemplates.ContainsKey(_islandId))
                {
                    var template = _map.islandTemplates[_islandId];
                    template.Rotate(_islandRotate);
                    template.AssignMapIsland(_map, new Axial(islandHex.x, islandHex.y));
                }
                else
                {
                    Debug.LogError($"海域{index} 岛屿模板{_islandId} 丢失！！！");
                }
            }

            if (landType == ZoneLandType.Sea)
            {
                if(_portSailIndex > 0)
                {
                    var sailHex = _map.GetHexagon(_portSailIndex);
                    sailHex.attribute = HexAttribute.SailPoint;
                }
            }
        }

        private void GenerateSeaPort()
        {
            //取所有点平均值得出岛屿位置 根据岛屿模板以及旋转得出海港位置
            Vector2Int islandQr = GetZoneCenter();
            _islandIndex = _map.GetIndex(islandQr.x, islandQr.y);
            var islandIds = _map.islandTemplates.Keys.ToArray();
            _islandId = islandIds[Random.Range(0, islandIds.Length - 1)];
            _islandRotate = Random.Range(0, 5);
            var islandAxial = new Axial(islandQr.x, islandQr.y);

            var template = _map.islandTemplates[_islandId];
            template.Rotate(_islandRotate);
            template.AssignMapIsland(_map, islandAxial);

            var portAxial = template.GetPortIndex(_map, islandAxial);
            _portIndex = _map.GetIndex(portAxial.q, portAxial.r);

            var portHex = _map.GetHexagon(_portIndex);
            var islandHex = _map.GetHexagon(_islandIndex);

            var portPos = _map.GetHexagonPos(portHex);
            var islandPos = _map.GetHexagonPos(islandHex);

            var angle = CalSailAngle(portPos - islandPos);
            CalPortSailPoint(angle);
        }

        private float CalSailAngle(Vector3 vec)
        {
            var rotate = Quaternion.FromToRotation(Vector3.right, vec);
            return rotate.eulerAngles.y;
        }

        public void CalPortSailPoint(float angle)
        {
            var portHex = _map.GetHexagon(_portIndex);
            var portPos = _map.GetHexagonPos(_portIndex);
            var portAxial = new Axial(portHex.x, portHex.y);
            var ring = Axial.Ring4Port(_map, portAxial, 8);
            float min = 360.0f;
            foreach (var axial in ring)
            {
                var hex = _map.GetHexagon(axial.q, axial.r);
                if (hex != null && hex.zone == this && hex.movetype != MOVETYPE.DISABLE)
                {
                    var hexPos = _map.GetHexagonPos(hex);
                    var hexAngle = CalSailAngle(hexPos - portPos);
                    var sub = Math.Abs(hexAngle - angle);
                    if (sub < min)
                    {
                        _portSailIndex = hex.index;
                        min = sub;
                    }
                }

                if(hex == null)
                {
                    Debug.LogError($"CalPortSailPoint 超出地图了 = = ！！！！ Zone Index = {index}");
                }
            }

            var sailHex = _map.GetHexagon(_portSailIndex);
            sailHex.attribute = HexAttribute.SailPoint;
        }

        public void ChangePortIndex(int newPortIndex)
        {
            var newHex = _map.GetHexagon(newPortIndex);
            // if (newHex.zone != this && neigbourZones.Contains(newHex.zone) == false)
            // {
            // 	return;
            // }

            var oldHex = _map.GetHexagon(_portIndex);
            var cube1 = Axial.Axial2Cube(new Axial(newHex.x, newHex.y));
            var cube2 = Axial.Axial2Cube(new Axial(oldHex.x, oldHex.y));
            var sub = Cube.Subtract(cube1, cube2);
            if (sub.Distance2Origin() > 20)
            {
                return;
            }

            _portIndex = newPortIndex;

            var spiral = Axial.Spiral4Port(_map, new Axial(newHex.x, newHex.y), _portCircles);
            foreach (var ax in spiral)
            {
                var axHex = _map.GetHexagon(ax.q, ax.r);
                //把港口占地区域归入海域
                if (axHex.zone != this)
                {
                    axHex.zone.hexagons.Remove(axHex);
                    AddHexagon4Port(axHex);
                }
            }
        }

        public void ChangePortSailIndex(int sailIndex)
        {
            if (_portSailIndex == sailIndex) return;

            var newHex = _map.GetHexagon(sailIndex);
            if(newHex.zone != this) return;

            newHex.attribute = HexAttribute.SailPoint;

            var oldHex = _map.GetHexagon(_portSailIndex);
            oldHex.attribute = HexAttribute.Idle;

            _map.ReplaceSailPoint(_portSailIndex, sailIndex);

            _portSailIndex = sailIndex;
        }

        public void SetZoneHexagonsBlock(BlockFlag blockFlag)
        {
            foreach (var hex in hexagons)
            {
                hex.blockFlag = blockFlag;
            }
        }

        //获取循环地图相邻海域  左右
        public HashSet<Zone> GetLoopMapNeighborZones()
        {
            var set = new HashSet<Zone>();
            foreach (var hex in hexagons)
            {
                Zone neighborZone = null;
                if (hex.x == 0)
                {
                    var neighborHex = _map.GetHexagon(_map.width - 1, hex.y);
                    if (neighborHex != null)
                    {
                        if (neighborHex.zone.landType == ZoneLandType.Sea)
                        {
                            neighborZone = neighborHex.zone;
                        }
                    }
                }
                else if(hex.x == _map.width - 1)
                {
                    var neighborHex = _map.GetHexagon(0, hex.y);
                    if (neighborHex != null)
                    {
                        if (neighborHex.zone.landType == ZoneLandType.Sea)
                        {
                            neighborZone = neighborHex.zone;
                        }
                    }
                }

                if (neighborZone != null && set.Contains(neighborZone) == false)
                {
                    set.Add(neighborZone);
                }
            }

            return set;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(index);
            bw.Write(level);
            bw.Write(color);
            bw.Write(hexagon.index);
            bw.Write(visible);
            bw.Write(posType);
            bw.Write(isGuanqia);
            bw.Write(isBorn);
            bw.Write(subType);
            bw.Write(ruinId);

            bw.Write(keyEdges.Count);
            for (int i = 0; i < keyEdges.Count; i++)
            {
                bw.Write(keyEdges[i].index);
            }

            bw.Write(towers.Count);
            foreach (var it in towers)
            {
                bw.Write(it.Key.index);
            }

            bw.Write((int) landform);
            bw.Write((int) landType);

            bw.Write(_portIndex);
            bw.Write(_portSailIndex);
            bw.Write(_islandIndex);

            if (_islandIndex > 0)
            {
                bw.Write(_islandId);
                bw.Write(_islandRotate);
            }

            bw.Write(businessZone);

            bw.Write(_validTreasures.Count);
            foreach (var ti in _validTreasures)
            {
                bw.Write(ti);
            }

            bw.Write(_invalidTreasures.Count);
            foreach (var ti in _invalidTreasures)
            {
                bw.Write(ti);
            }

            //bw.Write(hexagons.Count);
            //for (int i = 0; i < hexagons.Count; i++)
            //{
            //    bw.Write(hexagons[i].index);
            //}
        }

        public void SaveImageInfo()
        {
        }

        public void Load(BinaryReader br, int ver)
        {
            index = br.ReadInt32();
            level = br.ReadInt32();
            color = br.ReadInt32();
            int idx = br.ReadInt32();
            hexagon = _map.GetHexagon(idx);
            visible = br.ReadInt32();
            posType = br.ReadInt32();

            isGuanqia = br.ReadInt32();
            isBorn = br.ReadInt32();


            subType = br.ReadInt32();


            ruinId = br.ReadInt32();


            var count = br.ReadInt32();
            keyEdges.Clear();
            for (int i = 0; i < count; i++)
            {
                idx = br.ReadInt32();
                keyEdges.Add(_map.GetHexagon(idx));
            }


            count = br.ReadInt32();
            towers.Clear();
            for (int i = 0; i < count; i++)
            {
                idx = br.ReadInt32();
                var hex = _map.GetHexagon(idx);
                towers[hex] = new Tower(this, hex);
            }


            landform = (ZoneLandform) br.ReadInt32();
            landType = (ZoneLandType) br.ReadInt32();

            _portIndex = br.ReadInt32();
            _portSailIndex = br.ReadInt32();
            _islandIndex = br.ReadInt32();
            if (_islandIndex > 0)
            {
                _islandId = br.ReadInt32();
                _islandRotate = br.ReadInt32();
            }

            if (landType == ZoneLandType.Land) level = 0;

            if (ver >= 4)
            {
                businessZone = br.ReadInt32();
            }

            if (ver >= 7)
            {
                int t1Count = br.ReadInt32();
                for (int i = 0; i < t1Count; i++)
                {
                    _validTreasures.Add(br.ReadInt32());
                }

                int t2Count = br.ReadInt32();
                for (int i = 0; i < t2Count; i++)
                {
                    _invalidTreasures.Add(br.ReadInt32());
                }
            }

            //int hexId = 0;
            //Hexagon hex = null;
            //hexagons.Clear();

            //int count = br.ReadInt32();
            //for (int i = 0; i < count; i++)
            //{
            //    hexId = br.ReadInt32();
            //    hex = _map.GetHexagon(hexId);
            //    if (hex != null && hex.zone == null)
            //    {
            //        hex.zone = this;
            //        hexagons.Add(hex);
            //    }
            //}
        }

        //导出停靠点信息
        public void ExportStops(BinaryWriter bw)
        {
            if (_portIndex < 0)
            {
                bw.Write(0);
                bw.Write(0);
                return;
            }
            var vipCount = _level > 2 ? _map.VipCount4ThreeCircle : _map.VipCount4TwoCircle;
            var radius = _level > 2 ? _map.CallRadius4ThreeCircle : _map.CallRadius4TwoCircle;

            //王座不导出驻港点
            if (_level == 7)
            {
                radius = 0;
                vipCount = 0;
            }

            var stopList = new List<int>();

            var portHex = _map.GetHexagon(_portIndex);
            var portPos = _map.GetHexagonPos(portHex);
            var portAxial = new Axial(portHex.x, portHex.y);
            var spiral = Axial.Spiral4Port(_map, portAxial, radius + 1);
            foreach (var axial in spiral)
            {
                var aHex = _map.GetHexagon(axial.q, axial.r);
                if (_map.GetMoveType(aHex) != MOVETYPE.DISABLE && aHex.zone == this && aHex.attribute != HexAttribute.SailPoint)
                {
                    stopList.Add(_map.GetIndex(axial.q, axial.r));
                }
            }

            stopList.Sort((int idx1, int idx2) =>
            {
                var hexPos1 = _map.GetHexagonPos(idx1);
                var hexPos2 = _map.GetHexagonPos(idx2);
                var dis1 = Vector3.Distance(portPos, hexPos1);
                var dis2 = Vector3.Distance(portPos, hexPos2);
                return dis1.CompareTo(dis2);
            });

            bw.Write(vipCount);
            for (int i = 0; i < vipCount; i++)
            {
                var hex = _map.GetHexagon(stopList[i]);
                hex.attribute = HexAttribute.VipStop;
                bw.Write(hex.index);
            }

            var allCount = stopList.Count;
            var normalCount = allCount - vipCount;
            bw.Write(normalCount);
            for (int i = vipCount; i < allCount; i++)
            {
                var hex = _map.GetHexagon(stopList[i]);
                hex.attribute = HexAttribute.NormalStop;
                bw.Write(hex.index);
            }
        }

        public void ExportCommon(BinaryWriter bw)
        {
            bw.Write((short) (index + _map.ExportNewbieMapZoneMargin));		// short	区域id
            bw.Write((short) level);		// short	区域等级  0-7级（0是陆地的区域）
            bw.Write(hexagon.index);
            // if (landType == ZoneLandType.Sea)
            // {
            // 	bw.Write(hexagon.index);
            // 	bw.Write(_portSailIndex);
            //
            // 	this.ExportStops(bw);
            // }
            // else
            // {
            // 	bw.Write(-1);
            // 	bw.Write(-1);
            // 	bw.Write(0);
            // 	bw.Write(0);
            // }
        }

        public void ExportServer(BinaryWriter bw)
        {
            bw.Write((short)businessZone);
        }

        // 导出服务器使用的配置
        public void ExportToServer(BinaryWriter bw)
        {
            ExportCommon(bw);
            ExportServer(bw);
        }

        // 导出给客户端使用的配置
        public void ExportToClient(BinaryWriter bw)
        {
            GetImageInfo(1, out int w, out int h, out int x, out int y);
            var image = new ZoneImageInfo(this, x, y, w, h);
            image.Save(bw);
            // bw.Write(edges.Count);
            // for (int i = 0; i < edges.Count; i++)
            // {
            // 	bw.Write(edges[i].index);
            // }
        }

        public void GetImageInfo(int scale, out int w, out int h, out int tx, out int ty)
        {
            Vector2Int min = new Vector2Int(_map.width, _map.width);
            Vector2Int max = Vector2Int.zero;
            foreach (var hex in hexagons)
            {
                if (hex.x > max.x) max.x = hex.x;
                if (hex.x < min.x) min.x = hex.x;
                if (hex.y > max.y) max.y = hex.y;
                if (hex.y < min.y) min.y = hex.y;
            }

            w = max.x - min.x + 5;
            h = max.y - min.y + 5;

            w *= scale;
            h *= scale;

            tx = min.x - 2;
            ty = min.y - 2;
        }

        public Texture2D GetImage(out int w, out int h, out int tx, out int ty)
        {
            GetImageInfo(1, out w, out h, out tx, out ty);
            Texture2D tex = new Texture2D(w, h);
            Color32[] colors = new Color32[w * h];
            Color32 dot1 = new Color32(255, 255, 255, 0);
            Color32 dot2 = new Color32(0, 0, 0, 255);
            Color32 dot3 = new Color32(0, 255, 255, 255);
            Color32 dot4 = new Color32(0, 0, 255, 255);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    colors[y * w + x] = dot1;
                }
            }

            foreach (var hex in hexagons)
            {
                var _x = hex.x - tx;
                var _y = h - (hex.y - ty) - 1;

                if (hex.edgeIndex == 0)
                {
                    colors[_y * w + _x] = dot3;
                }
                else if (hex.edgeIndex == 1)
                    colors[_y * w + _x] = dot4;
                else
                    colors[_y * w + _x] = dot2;
            }

            tex.SetPixels32(colors);
            tex.Apply();
            return tex;
        }

        public Texture2D GetOutLineImage(out int w, out int h, out int tx, out int ty)
        {
            GetImageInfo(1, out w, out h, out tx, out ty);
            Texture2D tex = new Texture2D(w, h);
            Color32[] colors = new Color32[w * h];
            Color32 dot1 = new Color32(255, 255, 255, 0);
            Color32 dot4 = new Color32(255, 255, 255, 255);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    colors[y * w + x] = dot1;
                }
            }

            foreach (var hex in hexagons)
            {
                var _x = hex.x - tx;
                var _y = h - (hex.y - ty) - 1;

                if (hex.edgeIndex == 1)
                    colors[_y * w + _x] = dot4;
            }

            tex.SetPixels32(colors);
            tex.Apply();
            return tex;
        }

        public void SaveImage(string filename, out int w, out int h, out int tx, out int ty)
        {
            var tex = GetImage(out w, out h, out tx, out ty);
            byte[] dataBytes = tex.EncodeToPNG();
            FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            Debug.Log($"SaveImage : {filename}");
        }

        public void SaveOutLineImage(string filename, out int w, out int h, out int tx, out int ty)
        {
            var tex = GetOutLineImage(out w, out h, out tx, out ty);
            byte[] dataBytes = tex.EncodeToPNG();
            FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
            fs.Write(dataBytes, 0, dataBytes.Length);
            fs.Flush();
            fs.Close();
            Debug.Log($"SaveImage : {filename}");
        }

        public bool IsPortAround(int index)
        {
            var hex = _map.GetHexagon(index);
            var cityIndex = hexagon.index;
            if (cityIndex > 0)
            {
                var offset = _map.GetOffset(cityIndex);
                var cube1 = Axial.Axial2Cube(new Axial(offset.x, offset.y));
                var cube2 = Axial.Axial2Cube(new Axial(hex.x, hex.y));
                var sub = Cube.Subtract(cube1, cube2);
                if (sub.Distance2Origin() < _portCircles)
                {
                    return true;
                }
            }

            return false;
        }

        //获取港口朝向
        public int GetPortOrient()
        {
            var portPos = _map.GetHexagonPos(_portIndex);
            var sailPos = _map.GetHexagonPos(_portSailIndex);
            float angle = CalSailAngle(sailPos - portPos);
            if (angle >= 0.0f && angle < 60.0f)
                return 1;
            else if(angle >= 60.0f && angle < 120.0f)
                return 2;
            else if(angle >= 120.0f && angle < 180.0f)
                return 3;
            else if(angle >= 180.0f && angle < 240.0f)
                return 4;
            else if(angle >= 240.0f && angle < 300.0f)
                return 5;
            else if(angle >= 300.0f && angle < 360.0f)
                return 6;
            else
            {
                Debug.LogError($"GetPortOrient unknown angle {angle}");
                return 6;
            }
        }

        //修复港口2圈3圈位置
        public void FixPort()
        {
            if (_portIndex > 0 && _level > 2)
            {
                //海港
                if (_islandId > 0)
                {
                    var template = _map.islandTemplates[_islandId];
                    template.Rotate(_islandRotate);

                    var islandHex = _map.GetHexagon(_islandIndex);
                    var portHex = _map.GetHexagon(_portIndex);
                    _portIndex = template.FixPortIndex(_map, new Axial(islandHex.x, islandHex.y), new Axial(portHex.x, portHex.y));
                }
                else
                {
                    var sailPos = _map.GetHexagonPos(_portSailIndex);
                    var portPos = _map.GetHexagonPos(_portIndex);
                    float angle = CalSailAngle(portPos - sailPos);

                    var portHex = _map.GetHexagon(_portIndex);
                    var portAxial = new Axial(portHex.x, portHex.y);
                    var ring = Axial.Ring4Port(_map, portAxial, 1);
                    float min = 360.0f;
                    foreach (var axial in ring)
                    {
                        var hex = _map.GetHexagon(axial.q, axial.r);
                        if (hex != null && hex.zone == this && hex.movetype == MOVETYPE.DISABLE)
                        {
                            var hexPos = _map.GetHexagonPos(hex);
                            var hexAngle = CalSailAngle(hexPos - portPos);
                            var sub = Math.Abs(hexAngle - angle);
                            if (sub < min)
                            {
                                _portIndex = hex.index;
                                min = sub;
                            }
                        }
                    }
                }
            }
        }


        //重置海港港口位置和主航道点位置
        public void ResetIslandPort()
        {
            //取所有点平均值得出岛屿位置 根据岛屿模板以及旋转得出海港位置
            Vector2Int islandQr = GetZoneCenter();
            var islandAxial = new Axial(islandQr.x, islandQr.y);

            var template = _map.islandTemplates[_islandId];
            template.Rotate(_islandRotate);

            var portAxial = template.GetPortIndex(_map, islandAxial);
            _portIndex = _map.GetIndex(portAxial.q, portAxial.r);

            var portHex = _map.GetHexagon(_portIndex);
            var islandHex = _map.GetHexagon(_islandIndex);

            var portPos = _map.GetHexagonPos(portHex);
            var islandPos = _map.GetHexagonPos(islandHex);

            var hex = _map.GetHexagon(_portSailIndex);
            hex.attribute = HexAttribute.Idle;
            _map.RemoveSailPoint(_portSailIndex);

            var angle = CalSailAngle(portPos - islandPos);
            CalPortSailPoint(angle);
        }

        public void ChangeCityHexagon(Hexagon hex)
        {
            hexagon = hex;
            MapEditorEventCenter.SendEvent(MapEditorEvent.LevelLabelUpdateEvent, index);
        }
    }
}
#endif