#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GEngine.MapEditor
{
    [System.Serializable]
    public class HexagonSave
    {
        public int index;
        public int x;
        public int y;
        public int zone;
        public int block;
    }

    [System.Serializable]
    public class ZoneSave
    {
        public int index;
        public int level;
        public int pos;
        public int posType;
    }

    public class HexMapSave
    {
        [SerializeField] public ZoneSave[] zones;

        [SerializeField] public HexagonSave[] hexagons;
    }

    //六边形类
    public class Hexagon
    {
        public int index = 0;
        public int x;
        public int y;

        public int q, r;

        private float _hegiht = -1;
        private Vector3 _pos;
        public Vector3 Pos
        {
            get
            {
                if (_pos == Vector3.zero)
                    _pos = Hex.OffsetToWorld(x, y);
                return _pos;
            }
        }

        public float Height {
            get
            {
                if (_hegiht < 0)
                {
                    // _hegiht = Hex.GetHeight(x, y);
                    Ray ray = new Ray(Pos + new Vector3(0, 100, 0), Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("ObjRaycast")))
                    {
                        _hegiht = hit.point.y;
                    }
                    else
                    {
                        _hegiht = 0;
                    }
                }

                return _hegiht;
            }
        }

        public Hexagon[] neigbours = new Hexagon[6];
        public Zone zone;
        public bool bEdge = false;
        public int edgeIndex = -1;

        public MOVETYPE movetype = GlobalDef.S_DefaultMoveType;
        // 地格 可选的movetype合集，随机深浅海用
        public int movetypeMark = GlobalDef.S_AllMark;
        public int movetypeMarkRandomSea = GlobalDef.S_AllMark;

        public int ruinDepth = 0;
        public BlockFlag blockFlag = BlockFlag.None;
        public FishType fishType = FishType.None;
        private HexMap _map;
        public HexAttribute attribute = HexAttribute.Idle;
        
        //判断交界点使用的临时哈希
        HashSet<int> zoneIndexesHashSet = new HashSet<int>();

        // fogofwar ID
        public int fogOfWarId = FogOfWarDataMan.DefaultFogOfWarId;

        // 遍历使用的临时变量
        public bool Dirty = false;

        public Hexagon(HexMap map, int idx, int _x, int _y)
        {
            _map = map;
            index = idx;
            x = _x;
            y = _y;
            edgeIndex = -1;
            movetypeMark = ~(~0 << (int) MOVETYPE.COUNT);
            r = _y;
            q = _x - (_y - (_y&1)) / 2;
        }
        

        public int GetZoneIndex()
        {
            return zone?.index ?? -1;
        }

        public bool IsEdge(Hexagon dst, int edgeIndex = 0)
        {
            if (edgeIndex == 0)
            {
                return (dst == null || dst.zone != zone) ? true : false;
            }

            if (dst != null && dst.edgeIndex >= 0 && dst.edgeIndex < edgeIndex)
            {
                return true;
            }

            return false;
        }

        public int getEdges(int edgeIndex = 0)
        {
            int ret = 0;
            for (int i = 0; i < 6; i++)
            {
                if (IsEdge(neigbours[i], edgeIndex))
                {
                    ret++;
                }
            }

            return ret;
        }

        public int getExpandEdges(bool isDevour = false)
        {
            int ret = 0;
            for (int i = 0; i < 6; i++)
            {
                if (neigbours[i] != null)
                {
                    if (neigbours[i].zone == null ||
                        (isDevour && neigbours[i].zone.level <= zone.level && neigbours[i].zone.index != zone.index))
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }

        public void getNeigbourZones(ref List<Zone> zones)
        {
            for (int i = 0; i < 6; i++)
            {
                if (neigbours[i] == null)
                    continue;

                if (neigbours[i].zone != null && neigbours[i].zone != zone)
                {
                    if (!zones.Contains(neigbours[i].zone))
                    {
                        zones.Add(neigbours[i].zone);
                    }
                }
            }
        }

        public int GetNeigbourIndex(Hexagon hex)
        {
            if (hex != null)
            {
                for (int i = 0; i < neigbours.Length; i++)
                {
                    if (neigbours[i] == hex)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        // 获取边界的范围
        public int GetEdgeBegin(int dir, Hexagon last)
        {
            int ret = -1;
            int idx = 0;
            int lastIndex = (last != null) ? GetNeigbourIndex(last) : -1;

            for (int i = 0; i < 7; i++)
            {
                idx = dir * i;
                if (idx < 0) idx += 6;
                if (idx > 5) idx -= 6;

                //if (IsEdge(neigbours[idx]))
                if (neigbours[idx] == null || neigbours[idx].zone != zone)
                {
                    if (lastIndex >= 0)
                    {
                        if (dir > 0 && ret != lastIndex)
                            continue;
                    }

                    if (ret >= 0)
                    {
                        return ret;
                    }
                }
                else
                {
                    ret = idx;
                }
            }

            return -1;
        }

        public int GetEdgeEnd(int dir, int begin)
        {
            int ret = -1;
            int idx = 0;
            for (int i = 0; i < 7; i++)
            {
                idx = begin + dir * i;
                if (idx < 0) idx += 6;
                if (idx > 5) idx -= 6;

                //if (IsEdge(neigbours[idx]))
                if (neigbours[idx] == null || neigbours[idx].zone != zone)
                {
                    ret = idx;
                }
                else
                {
                    if (ret >= 0)
                    {
                        return idx;
                    }
                }
            }

            return -1;
        }

        public List<Hexagon> GetAllEdgeHex(int edgeIndex = 0)
        {
            var rets = new List<Hexagon>();
            for (int i = 0; i < 6; i++)
            {
                if (neigbours[i] != null && neigbours[i].zone == zone && neigbours[i].edgeIndex == -1 &&
                    neigbours[i].getEdges(edgeIndex) > 0)
                {
                    rets.Add(neigbours[i]);
                }
            }

            return rets;
        }

        // 获取下一个边界的六角格
        public Hexagon GetEdgeHex(List<Hexagon> edges, Hexagon last, int dir, int edgeIndex = 0)
        {
            if (last == null)
            {
                for (int i = 0; i < neigbours.Length; i++)
                {
                    if (neigbours[i] != null && neigbours[i].zone == zone && neigbours[i].edgeIndex == -1 &&
                        neigbours[i].getEdges(edgeIndex) > 0)
                    {
                        return neigbours[i];
                    }
                }

                return null;
            }

            Hexagon ret1 = null;
            Hexagon ret2 = null;
            var tdx = GetNeigbourIndex(last);
            for (int i = 1; i <= 6; i++)
            {
                var idx = tdx + i * dir;
                if (idx < 0) idx += 6;
                if (idx > 5) idx -= 6;

                if (neigbours[idx] != null && neigbours[idx].zone == zone && neigbours[idx].edgeIndex == -1 &&
                    neigbours[idx].getEdges(edgeIndex) > 0)
                {
                    if (ret1 == null)
                    {
                        ret1 = neigbours[idx];
                    }

                    if (ret2 == null)
                    {
                        if (!edges.Contains(neigbours[idx]))
                        {
                            ret2 = neigbours[idx];
                        }
                    }
                }
            }

            if (ret2 != null)
                return ret2;

            return ret1;
        }

        //判断Hexagon是否在n个及其以上Zone的交界处
        public bool IsMultiZonesHexagon(int n) 
        {
            
            bool result = false;
            int count = 0;
            zoneIndexesHashSet.Add(this.zone.index);
            count++;
            for (int i = 0; i < neigbours.Length; i++)
            {
                if (neigbours[i] != null)
                {
                    if (neigbours[i].zone != null)
                    {
                        if (zoneIndexesHashSet.Contains(neigbours[i].zone.index) == false)
                        {
                            count++;
                            zoneIndexesHashSet.Add(neigbours[i].zone.index);
                        }
                        
                        if (count >= n)
                        {
                            result = true;
                            break;
                        }
                    }
                    else 
                    {
                        Debug.LogError("Hexagon's zone is null." + "hexagon x: " + neigbours[i].x + " hexagon y: " + neigbours[i].y);
                    }
                }
            }
            zoneIndexesHashSet.Clear();
            return result;
        }


        // 判断是否为尖角，是则抹平
        public bool DoSmooth(out Zone smoothZone)
        {
            int findCount = 0;
            smoothZone = null;
            for (int i = 0; i < neigbours.Length + 3; i++)
            {
                var idx = i < 6 ? i : i - 6;
                if (neigbours[idx] != null && neigbours[idx].zone != zone)
                {
                    if (smoothZone == null || smoothZone != neigbours[idx].zone)
                    {
                        smoothZone = neigbours[idx].zone;
                        findCount = 1;
                    }
                    else
                    {
                        findCount++;
                        if (findCount >= 4)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    findCount = 0;
                }
            }

            return false;
        }

        public void RemoveFromZone()
        {
            if (zone == null)
                return;

            if (zone.towers.Count == 0)
            {
                if (zone.towers.ContainsKey(this))
                {
                    zone.towers.Remove(this);
                }
            }
            
            zone = null;
        }

        List<Hexagon> _expands = new List<Hexagon>();
        List<Hexagon> _newExpands = new List<Hexagon>();

        public void GetRoundHexagons(ref List<Hexagon> rets, int round = 1)
        {
            rets.Clear();
            rets.Add(this);

            _expands.Clear();
            _expands.Add(this);

            for (int k = 0; k < round; k++)
            {
                _newExpands.Clear();
                foreach (var t in _expands)
                {
                    for (int i = 0; i < t.neigbours.Length; i++)
                    {
                        if (t.neigbours[i] != null && !rets.Contains(t.neigbours[i]))
                        {
                            rets.Add(t.neigbours[i]);
                            _newExpands.Add(t.neigbours[i]);
                        }
                    }
                }

                _expands.Clear();
                _expands.AddRange(_newExpands);
            }
        }

        public void GetNearSameHexagons(ref List<Hexagon> rets)
        {
            rets.Clear();
            rets.Add(this);
            GetNeigbourSames(this, ref rets);
        }

        private void GetNeigbourSames(Hexagon hex, ref List<Hexagon> rets)
        {
            if (hex == null || hex.zone == null)
                return;

            foreach (var nb in hex.neigbours)
            {
                if (nb != null && nb.zone != null && nb.zone.index == hex.zone.index && !rets.Contains(nb))
                {
                    rets.Add(nb);
                    GetNeigbourSames(nb, ref rets);
                }
            }
        }
        
        public void GetRoundEarth(ref HashSet<int> earth)
        {
            int ret = 0;
            for (int i = 0; i < 6; i++)
            {
                if (neigbours[i] != null && neigbours[i].movetype == MOVETYPE.DISABLE)
                {
                    if (earth.Contains(neigbours[i].index) == false)
                    {
                        earth.Add(neigbours[i].index);
                    }
                }
            }
        }

        //是否在商圈交界处
        public bool IsBusinessEdge()
        {
            for (int i = 0; i < 6; i++)
            {
                if (neigbours[i] != null)
                {
                    if (neigbours[i].zone.businessZone != zone.businessZone)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Color GetFogColor()
        {
            if (_map == null)
                return Color.black;

            if (_map.fogOfWarDataMan.FogOfWarDatas.TryGetValue(fogOfWarId, out var fogOfWarData))
            {
                return fogOfWarData.FogColor;
            }
            else
            {
                return _map.fogOfWarDataMan.defaultFogOfWarData.FogColor;
            }
        }

        public void Reset()
        {
            movetype = GlobalDef.S_DefaultMoveType;
            movetypeMark = GlobalDef.S_AllMark;
            movetypeMarkRandomSea = GlobalDef.S_AllMark;
            
            blockFlag = BlockFlag.None;
            zone = null;
            fogOfWarId = 0;
        }
    }
}
#endif