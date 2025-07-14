using System.Collections.Generic;
using System.Text;
using UnityEngine;


/// <summary>
/// 配置表工具类
/// </summary>
public class ConfigUtils
{
    public const int Building = 100000;//建筑表
    public const int CombineBuilding = 200000;//建筑蓝图表
    public const int CombineObject = 300000;//合成物表
    public const int CombineResource = 400000;//合成物消耗表
    public const int CombineFactory = 500000;//合成物工厂表

    /// <summary>
    /// 获得建筑的配置表ID
    /// <param name="buildingId">建筑ID，对应BuildingId</param>
    /// </summary>
    public static string GetBuildingConfigId(int buildingId, int level)
    {
        // 目前建筑配置表每个建筑只配置一行，没有区分等级，
        // 后期如果区分等级，再通过 buildingId+level;
        return buildingId.ToString();
    }

    /// <summary>
    /// 获得建筑蓝图的配置表ID
    /// <param name="buildingId">建筑ID，对应BuildingId</param>
    /// <param name="level">建筑等级</param>
    /// </summary>
    public static string GetCombineBuildingConfigId(int buildingId, int level)
    {
        //var offset = CombineBuilding - Building;
        return (buildingId + level).ToString();
    }

    /// <summary>
    /// 获得合成物的配置表ID
    /// <param name="objectId">合成物ID，对应CombineObjectId</param>
    /// <param name="level">合成物等级</param>
    /// </summary>
    public static string GetCombineObjectConfigId(int objectId, int level)
    {
        return (objectId + level).ToString();
    }

    /// <summary>
    /// 获得合成消耗的配置表ID
    /// <param name="objectConfigId">合成物ID，对应combine_object表的id</param>
    /// <param name="level">合成物等级</param>
    /// </summary>
    public static string GetCombineResourceConfigId(string objectConfigId)
    {
        var offset = CombineResource - CombineObject;
        return (int.Parse(objectConfigId) + offset).ToString();
    }

    /// <summary>
    /// 获得合成物工厂的配置表ID
    /// <param name="buildingId">建筑ID，对应BuildingId</param>
    /// <param name="level">建筑等级</param>
    /// </summary>
    public static string GetCombineFactoryConfigId(int buildingId, int level)
    {
        var offset = CombineFactory - Building;
        return (buildingId + level + offset).ToString();
    }

    /// <summary>
    /// 获得区域建筑配置
    /// <param name="areaId">区域ID</param>
    /// <param name="buildingId">建筑ID，对应BuildingId</param>
    /// </summary>
    //public static BaseXmlData GetAreaBuildingConfig(int areaId, int buildingId)
    //{
    //    //TODO 遍历整张表的效率比较低，最好是ID有规则可以换算，或者让服务端把id保存在建筑数据中，一起下发。
    //    //var list = GameEntry.DataTable.GetAllDataRows(GameDefines.NspTableName.AreaBuild);
    //    //foreach (var e in list)
    //    //{
    //    //    if (e.TryGetInt("areaId") == areaId && e.TryGetInt("buildingId") == buildingId)
    //    //    {
    //    //        return e;
    //    //    }
    //    //}
    //    //Log.Error("区域建筑表找不到配置，areaId=" + areaId + ", buildingId=" + buildingId);
    //    return null;
    //}
}
