#if UNITY_EDITOR
// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-11-01 11:20 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace GEngine.MapEditor
{
	public static class GlobalDef
	{
		public static string S_UWMapExt = ".uwmap";
		public static string S_RuntimeWorkSpace = "Assets/GameAssets/Scenes/Campaign";
		public static string S_WORLDRESRENDERDATA_NAME = "WorldResRenderData";

		public static Dictionary<int, string> S_LODS = new Dictionary<int, string>()
		{
			{ 1, "LOD_1" },
			{ 2, "LOD_2" },
			{ 3, "LOD_3" },
			{ 4, "LOD_4" },
			{ 5, "LOD_5" },
		};

        public static MOVETYPE S_DefaultMoveType = MOVETYPE.SHALLOWSEA;
        public static int S_AllMark = ~(~0 << (int)MOVETYPE.COUNT);
        public static int S_NormalMark = 1 << (int) MOVETYPE.NORMAL;
        public static int S_DisableMark = 1 << (int) MOVETYPE.DISABLE;

		public static string[] S_ZoneLandformDes = typeof(ZoneLandform).GetDescriptions<ZoneLandform>().ToArray();
		public static string[] S_ZoneLandTypeDes = typeof(ZoneLandType).GetDescriptions<ZoneLandType>().ToArray();
		public static string[] S_BlockFlagDes = typeof(BlockFlag).GetDescriptions<BlockFlag>().ToArray();
		public static string[] S_MoveTypeDes = typeof(MOVETYPE).GetDescriptions<MOVETYPE>().ToArray();
		public static string[] S_MapTypeDes = typeof(MapType).GetDescriptions<MapType>().ToArray();
		public static string[] S_MapToolOP = typeof(MapToolOP).GetDescriptions<MapToolOP>().ToArray();
		public static string[] S_HexAttributeDes = typeof(HexAttribute).GetDescriptions<HexAttribute>().ToArray();
		// 控制显示的工具
		public static string[] S_MapToolOP_Show = new string[]
		{
			MapToolOP.CreateMap.GetDescription(),
			MapToolOP.EditTerritory.GetDescription(),
			MapToolOP.EditCity.GetDescription(),
			MapToolOP.EditBlock.GetDescription(),
			MapToolOP.EditInteract.GetDescription(),
			MapToolOP.ExportSetting.GetDescription(),
			MapToolOP.Export.GetDescription(),
		};

		public static Color S_WaterShallow = new Color(0.25f, 0, 0);
		public static Color S_WaterNormal = new Color(0.5f, 0, 0);
		public static Color S_WaterDeep = new Color(1f, 0, 0);

		public static MapEditorType S_MapEditorType = MapEditorType.EBigWorld;
	}

	public enum MapEditorType
	{
		[Description("PVE地图")] EPVE,
		[Description("大世界地图")] EBigWorld,
	}

	public enum MOVETYPE
	{
        [Description("陆地")] DISABLE,        // 不可到达
		[Description("正常海")] NORMAL,       // 常规海
		[Description("浅海")] SHALLOWSEA,     // 浅海
		[Description("深海")] DEEPSEA,        // 深海
		[Description("数量")] COUNT,
	}

	public enum ZoneLandform
	{
		[Description("水域")] BLOCK,
		[Description("地貌1")] LANDFORM1,
		[Description("地貌2")] LANDFORM2,
		[Description("地貌3")] LANDFORM3,
		[Description("地貌4")] LANDFORM4,
	}

	public enum ZoneLandType
	{
		[Description("海水")] Sea,
		[Description("陆地")] Land,
	}

	public enum HexAttribute
	{
		[Description("无标识")] Idle,
		[Description("航道点")] SailPoint, //航道点
		[Description("海岛")] Island,    // 放置港口的海岛
		[Description("VIP停靠点")] VipStop,    // 港口VIP停靠点
		[Description("普通停靠点")] NormalStop,    // 港口普通停靠点
	}

	public enum BlockFlag
	{
		[Description("无标识")] None, // 无标识
		[Description("阻挡")] Block, // 阻挡
		[Description("非阻挡")] NotBlock // 不是阻挡
	}

	public enum FishType
	{
		[Description("无标识")] None, // 无标识
		[Description("鱼群")] Fish, // 鱼群
		[Description("垂钓点")] Seat // 垂钓点
	}
}
#endif