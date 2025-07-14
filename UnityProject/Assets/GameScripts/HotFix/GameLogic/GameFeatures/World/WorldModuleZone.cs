using System.Collections.Generic;
using System.Linq;
using GameBase;
using UnityEngine;
using World;
using World.PathFinder;

public enum WZLoadState
{
    None = 0,
    StartLoad = 1,
    Update = 2,
    Loaded = 3,
}

public class WorldModuleZone : Singleton<WorldModuleZone>
{
    public Vector3 InitScale { get; private set; } = Vector3.one;

    private WorldZoneTreeNode findNode;
    public WorldZoneMapData MapData;
    public Dictionary<int, WorldZoneInfo> WorldZones { get; private set; } = new Dictionary<int, WorldZoneInfo>();

    private Dictionary<int, WorldCityColor> worldCityColors = new Dictionary<int, WorldCityColor>();
    public StraightAStarPathfinder WorldPathFinder = new StraightAStarPathfinder();
    public void Init()
    {
        InitWorldCityColor();
    }

    public void Reset()
    {
        city2Point.Clear();
        // WorldZoneInfo.Clear();
        // WorldZoneTreeNode.Clear();
        // WorldZoneData.Clear();
    }


    #region 获取

    public WorldZoneInfo GetWorldZoneInfoById(int zoneId)
    {
        if (WorldZones.TryGetValue(zoneId, out var info))
        {
            return info;
        }

        return null;
    }

    // public WorldZoneData GetZoneDataByZoneId(int zoneId)
    // {
    //     return GameEntry.World.State.curWorld.zone.GetZoneDataByZoneId(zoneId);
    // }
    //
    // public WorldZoneData GetZoneDataByPointId(int pointId, bool containsHide = false)
    // {
    //     return GameEntry.World.State.curWorld.zone.GetZoneDataByPointId(pointId, containsHide);
    // }
    //
    // public short GetZoneIdByPointId(int pointId)
    // {
    //     return GameEntry.World.State.curWorld.zone.GetZoneIdByPointId(pointId);
    // }
    //
    // public bool IsBornZoneByPointId(int pointId)
    // {
    //     return GameEntry.World.State.curWorld.zone.IsBornZoneByPointId(pointId);
    // }

    #endregion


    #region 加载地图Zone

    private WZLoadState loadState = WZLoadState.None;

    public bool IsInited()
    {
        return loadState == WZLoadState.Loaded;
    }


    public void InitData(WorldZoneMapData mapData)
    {
        if (IsInited())
            return;
      
        if (mapData == null)
            return;
        
        MapData = mapData;
        WorldPathFinder.Init(MapData);
        var zoneScale = mapData.scale;
        if (zoneScale < 1) zoneScale = 1;
        InitScale = new Vector3(1f / zoneScale, 1f / zoneScale, 1f / zoneScale);
        findNode = new WorldZoneTreeNode(new RectInt(0, 0, mapData.width, mapData.height));
        findNode.CrateSubNodes(1);

        WorldZones.Clear();
        foreach (var zone in mapData.zones.Values)
        {
            var zoneInfo = new WorldZoneInfo(zone);
            WorldZones.Add(zoneInfo.index, zoneInfo);
            findNode.AddZone(zoneInfo);
        }

        var oldState = loadState;
        if (oldState == WZLoadState.Update)
        {
            loadState = WZLoadState.Loaded;
            UpdateAllZoneOwner();
        }

        loadState = WZLoadState.Loaded;
    }

    public void ClearData()
    {
        // foreach (var zone in WorldZones.Values)
        // {
        //     WorldZoneInfo.Return(zone);
        // }
        WorldZones.Clear();

        // if (findNode != null)
        // {
        //     WorldZoneTreeNode.Return(findNode);
        // }
        findNode = null;

        loadState = WZLoadState.None;
    }

    public void ResetZone()
    {
        foreach (var zone in WorldZones.Values)
        {
            zone.Reset();
        }
    }

    #endregion


    #region Load City colors

    void InitWorldCityColor()
    {
        // if (worldCityColors.Count == 0)
        // {
        //     var table = GameEntry.Table.GetTable<LF.Worldcity_colorTable>();
        //     var datas = table.GetAllData();
        //     foreach (var item in datas)
        //     {
        //         var color = new WorldCityColor(item.Value);
        //         worldCityColors[color.id] = color;
        //     }
        // }
    }

    public WorldCityColor GetWorldCityColor(int index)
    {
        if (worldCityColors.TryGetValue(index, out var ret))
        {
            return ret;
        }
        return null;
    }

    public int RandomWorldCityColor()
    {
        var keys = worldCityColors.Keys.ToList();
        return keys[Random.Range(0, keys.Count)];
    }

    #endregion


    // 更新城点的所属联盟
    public List<int> UpdateAllZoneOwner()
    {
        List<int> changes = new List<int>();
        if (!IsInited())
        {
            loadState = WZLoadState.Update;
            return changes;
        }

        // var isLegionOpen = GameEntry.World.State.curWorld.CheckLegionOpen;
        // var legionId = GameEntry.Controller.Legion.Uuid;
        // var selfAllianceSnapshot = GameEntry.World.State.curWorld.city.GetSelfAllianceSnapshot();
        //
        //string oldAllianceId;
        // AllianceSnapshot info;
        // var mapData = GameEntry.World.State.curWorld.zone.MapData;
        // if (mapData == null) return changes;
        // foreach (var zone in mapData.zones.Values)
        // {
        //     oldAllianceId = zone.AllianceId;
        //     info = GameEntry.World.State.curWorld.city.GetZoneOwnerAllianceSnapshot(zone.CityPos);
        //     if (info == null)
        //     {
        //         zone.Reset();
        //         if (!oldAllianceId.Equals(zone.AllianceId))
        //             changes.Add(zone.ZoneId);
        //         continue;
        //     }
        //
        //     //被占领
        //     var infoAllianceId = info.allianceId;
        //     if (info.IsCityBeFull(zone.CityPos))
        //     {
        //         if (info.FullAllianceInfo != null)
        //         {
        //             infoAllianceId = info.FullAllianceInfo.Id;
        //         }
        //     }
        //
        //     zone.AllianceId = infoAllianceId;
        //     zone.color = (info.color % worldCityColors.Count) + 1;
        //     zone.outlineColor = -1;
        //
        //     if (isLegionOpen)
        //     {
        //         // 同军团
        //         if (!string.IsNullOrEmpty(legionId) && legionId == info.LegionId)
        //         {
        //             if (selfAllianceSnapshot != null)
        //             {
        //                 zone.color = (selfAllianceSnapshot.color % worldCityColors.Count) + 1;
        //                 zone.outlineColor = 1;
        //             }
        //         }
        //     }
        //
        //     if (!oldAllianceId.Equals(infoAllianceId))
        //         changes.Add(zone.ZoneId);
        //
        // }

        return changes;
    }

    public void OnClip(RectInt rect, ref List<WorldZoneInfo> zones)
    {
        if (IsInited() && findNode != null)
        {
            int count = 0;
            zones.Clear();
            findNode.FindZone(rect, ref zones, ref count);
            //Debug.LogFormat("findNode => find = {0}, count = {1}", zones.Count, count);
        }
    }

    /// <summary>
    /// 是否是自己领地
    /// </summary>
    /// <param name="pointId"></param>
    /// <returns></returns>
    public bool IsSelfZone(int pointId)
    {
        // WorldZoneData zoneData = GameEntry.World.State.curWorld.zone.GetZoneDataByPointId(pointId);
        // if (zoneData == null)
        //     return false;
        // var alliance = GameEntry.World.State.oriWorld.city.GetZoneOwnerAllianceId(zoneData.CityPos);
        // if (!alliance.IsNullOrEmpty() && GameEntry.Controller.Player.User.GetAllianceId() == alliance)
        // {
        //     return true;
        // }
        // alliance = GameEntry.World.State.curWorld.city.GetZoneOwnerAllianceId(zoneData.CityPos);
        // if (!alliance.IsNullOrEmpty() && GameEntry.Controller.Player.User.GetAllianceId() == alliance)
        // {
        //     return true;
        // }
        return false;
    }

    private Dictionary<int, int> city2Point = new Dictionary<int, int>();


  
}