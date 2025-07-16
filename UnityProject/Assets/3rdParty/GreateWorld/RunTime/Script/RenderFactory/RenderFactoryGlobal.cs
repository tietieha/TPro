// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-17 11:44 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System.Collections.Generic;
using GEgineRunTime;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace BigWorldRender
{
	public class RenderFactoryGlobal : RenderFactoryBase
	{
		/// 记录裁剪数据
		public NativeList<WorldStaticRenderStructData> worldGlobalRenderStructDataNativeList; //分块数  

		/// 记录 裁剪后的需要渲染数据的 index
		public NativeList<int> filterGlobalRenderDataNativeList;

		public RenderFactoryGlobal(BigWorldRenderData bigWorldRenderData) : base(bigWorldRenderData)
		{
		}

		protected override void InitLogic()
		{
			var resRenderData = _bigWorldRenderData.ResRenderData;
			var worldRegionTotalScriptableObject = resRenderData.worldRegionTotalScriptableObject;
			var regionAssetDic = _bigWorldRenderData.StaticRegionAssetDic;

			// Leon-TODO: 统计global的总数量,便于初始化容器
			worldGlobalRenderStructDataNativeList =
				new NativeList<WorldStaticRenderStructData>(1000, Allocator.Persistent);
			
			
			_renderResCount = 0;
			
			//读取配置文件
			foreach (var data in worldRegionTotalScriptableObject.worldGlobalRenderDataList)
			{
				int resId = data.resId;

				if (!regionAssetDic.ContainsKey(resId))
				{
					Debug.LogError("regionAssetDic not ContainsKey" + resId);
					continue;
				}

				_renderResCount++;
				int cullingGrade = regionAssetDic[resId].cullingGrade;

				List<Vector3> posList = data.posList;
				List<Vector3> eulerList = data.eulerList;
				List<Vector3> scaleList = data.scaleList;
				List<float> boundRadIuList = data.boundRadiusList;

				_renderDataCount += posList.Count;

				for (int index = 0; index < posList.Count; index++)
				{
					WorldStaticRenderStructData worldStaticRenderStructData = new WorldStaticRenderStructData();
					worldStaticRenderStructData.assetId = data.resId;
					worldStaticRenderStructData.boundRadius = boundRadIuList[index];
					worldStaticRenderStructData.cullingGrade = cullingGrade;

					if (!regionAssetDic.ContainsKey(data.resId))
					{
						Debug.LogError("regionAssetDic 不存在 " + data.resId);
						worldStaticRenderStructData.renderLod = 1;
					}
					else
					{
						worldStaticRenderStructData.renderLod = regionAssetDic[data.resId].lod;
					}

					worldStaticRenderStructData.renderMatrix = Matrix4x4.TRS(
						posList[index] + _bigWorldRenderData.MapOffset, Quaternion.Euler(eulerList[index]),
						scaleList[index]);

					worldGlobalRenderStructDataNativeList.Add(worldStaticRenderStructData);
				}
			}

			filterGlobalRenderDataNativeList = new NativeList<int>(_renderDataCount, Allocator.Persistent);
		}

		private JobHandle cullingJobJandle;

		protected override void CullLogic()
		{
			cullingJobJandle = ScheduleCullingJob();
			cullingJobJandle.Complete();
		}

		protected override void DrawLogic()
		{
			int length = filterGlobalRenderDataNativeList.Length;
			for (int index = 0; index < length; index++)
			{
				WorldStaticRenderStructData worldStaticRenderStructData =
					worldGlobalRenderStructDataNativeList[filterGlobalRenderDataNativeList[index]];
				
				Render(worldStaticRenderStructData);
			}
		}

		protected override void ExitLogic()
		{
			if (worldGlobalRenderStructDataNativeList.IsCreated)
				worldGlobalRenderStructDataNativeList.Dispose();

			if (filterGlobalRenderDataNativeList.IsCreated)
				filterGlobalRenderDataNativeList.Dispose();
		}

		protected override void DrawGizmosLogic()
		{
		}

		public override bool NeedRender()
		{
			// 全局渲染，先关掉
			return false;
			return _renderDataCount > 0;
		}

		protected override string getType()
		{
			return "RenderFactoryGlobal";
		}


		public JobHandle ScheduleCullingJob()
		{
			filterGlobalRenderDataNativeList.Clear();
			var frustumPlanes = _bigWorldRenderData.GetFrustumPlanes();
			return new FilterViewFrustumGlobalCullingRenderDataJob
			{
				_worldStaticRenderStructDatas = worldGlobalRenderStructDataNativeList,
				_frustumPlanes = frustumPlanes,
				currentRenderLod = _bigWorldRenderData.currentRenderLod,
				toprenderlod = BigWorldRenderManager.Consts.TOP_RENDER_LOD
			}.ScheduleAppend(filterGlobalRenderDataNativeList, worldGlobalRenderStructDataNativeList.Length, 16);
		}
	}
}