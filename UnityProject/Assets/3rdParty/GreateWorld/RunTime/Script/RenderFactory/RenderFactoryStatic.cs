// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-13 18:07 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.GZip;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BigWorldRender
{
	/// <summary>
	/// 静态渲染工厂
	/// 只绘制LOD 6以下的物体，即羊皮卷以下的
	/// </summary>
	public class RenderFactoryStatic : RenderFactoryBase
	{
		//相机内最大渲染数量
		private const int RENDER_DRAW_DEFAULT = 2000;
		
		private int _dealCount = 0;
		private bool _dealRegionDataFinish = false;
		
		//读取资源数据
		private ResRenderData _resRenderData;
		// 资源列表 key 资源名字 value mesh mat  静态资源
		private Dictionary<int, RegionAssetData> _staticRegionAssetDic;
		
		//记录total渲染数据的开始index
		private int worldRegionBeginIndex;
		//记录total渲染数据的结束index
		private int worldRegionEndIndex;
		
		//世界地块总数量
		private int totalRegionNum;
		//地块行数
		private int regionRowNum;
		//地块列数
		private int regionColNum;
		//地图宽
		private float totalMapWidth;
		//地图高
		private float totalMapHeight;
		//地块宽
		private float regionWidth;
		//半宽
		private float halfRegionWidth;
		//地块高
		private float regionHeight;
		//半高
		private float halfRegionHeight;
		//总渲染数量
		private int totalWorldRenderNum;
		
		/// ============================================================================================================
		// 分块的渲染数据   length 分块数
		public NativeList<WorldRegionStaticRenderHeadStructData> worldStaticStructNativeList;//分块数  
		
		// 全部的渲染数据 length total渲染数量
		public NativeList<WorldStaticRenderStructData> worldStaticTotalNativeList;//全部静态渲染资源
		
		// 记录 地块裁剪后的index length 地块数量
		public NativeList<int> filterRegionDataNativeList;
		
		// 记录 需要进行裁剪的渲染数据 index 
		public NativeList<int> filterCullingDataNativeList;
		
		// 记录 裁剪后的需要渲染数据的 index
		public NativeList<int> filterRegionRenderDataNativeList;
		
		//裁剪建筑数据Job
		private JobHandle _cullingDynaColliderJob;
		//裁剪渲染数据Job
		private JobHandle _cullJobHandle;
		
		/// ============================================================================================================
		
		// 记录地块偏移
		private NativeList<int> regionOffsetNativeList;
		/// ============================================================================================================
		
		public RenderFactoryStatic(BigWorldRenderData bigWorldRenderData) : base(bigWorldRenderData)
		{
		}

		protected override void InitLogic()
		{

			_dealCount = 0;
			_dealRegionDataFinish = false;
			_resRenderData = _bigWorldRenderData.ResRenderData;
			_staticRegionAssetDic = _bigWorldRenderData.StaticRegionAssetDic;
			if (_resRenderData.worldRegionDetailScriptableObjectList == null)
			{
				Debug.LogError("_resRenderData.worldRegionDetailScriptableObjectList == null");
				return;
			}

			var worldRegionTotalScriptableObject = _resRenderData.worldRegionTotalScriptableObject;
			if (_resRenderData.worldRegionTotalScriptableObject == null)
			{
				Debug.LogError("_resRenderData.worldRegionTotalScriptableObject == null");
				return;
			}
			
			worldRegionBeginIndex = 0;
			totalMapWidth = worldRegionTotalScriptableObject.regionTotalWidth;
			totalMapHeight = worldRegionTotalScriptableObject.regionTotalHeight;
			regionRowNum = worldRegionTotalScriptableObject.regionRowNum;
			regionColNum = worldRegionTotalScriptableObject.regionColNum;
			regionWidth = worldRegionTotalScriptableObject.regionTotalWidth / worldRegionTotalScriptableObject.regionColNum;
			halfRegionWidth = regionWidth / 2;
			regionHeight = worldRegionTotalScriptableObject.regionTotalHeight / worldRegionTotalScriptableObject.regionRowNum;
			halfRegionHeight = regionHeight / 2;
			totalWorldRenderNum = worldRegionTotalScriptableObject.regionObjTotalNum;
			// totalRegionNum = regionRowNum * regionColNum;
			totalRegionNum = _resRenderData.worldRegionDetailScriptableObjectList.Count;
			
			
			// =========================================================================================================
			worldStaticStructNativeList = new NativeList<WorldRegionStaticRenderHeadStructData>(regionRowNum * regionColNum, Allocator.Persistent);
			worldStaticTotalNativeList = new NativeList<WorldStaticRenderStructData>(totalWorldRenderNum, Allocator.Persistent);
			
			filterRegionDataNativeList = new NativeList<int>(regionRowNum * regionColNum, Allocator.Persistent);
			filterCullingDataNativeList = new NativeList<int>(RENDER_DRAW_DEFAULT, Allocator.Persistent);
			filterRegionRenderDataNativeList = new NativeList<int>(regionRowNum * regionColNum, Allocator.Persistent);
			
			// =========================================================================================================
			regionOffsetNativeList = new NativeList<int>(regionRowNum, Allocator.Persistent);
			for (int index = 0; index < regionRowNum; index++)
			{
				regionOffsetNativeList.Add(0);
			}

			// =========================================================================================================
			_renderResCount = worldRegionTotalScriptableObject.regionResResDataList.Count;
		}

		protected override void UpdateLogic()
		{
			if (!_dealRegionDataFinish)
			{
				DealWorldRegionDetailData();
			}
		}

		protected override void CullLogic()
		{
			CalCameraOffset();
			CullingRegion();
			FilterRegionData();
			_cullingDynaColliderJob = ScheduleCullingDynaColliderJob();
			//裁剪需要渲染的数据
			_cullJobHandle = ScheduleCullingStaticDataJob(_cullingDynaColliderJob);
			_cullJobHandle.Complete();
			
			// ArchitectureCulling();
		}

		protected override void DrawLogic()
		{
			unsafe
			{
				var worldStaticTotalNativeListPtr = (WorldStaticRenderStructData*)worldStaticTotalNativeList.GetUnsafePtr();
				var filterCullingDataNativeListPtr = (int*)filterCullingDataNativeList.GetUnsafePtr();
				var filterRegionRenderDataNativeListPtr = (int*)filterRegionRenderDataNativeList.GetUnsafePtr();
				for (int index = 0; index < filterRegionRenderDataNativeList.Length; index++)
				{
					int filterRegionRenderDataNativeListIndex = filterRegionRenderDataNativeListPtr[index];
					int filterCullingDataNativeListIndex = filterCullingDataNativeListPtr[filterRegionRenderDataNativeListIndex];
					WorldStaticRenderStructData worldStaticRenderStructData = worldStaticTotalNativeListPtr[filterCullingDataNativeListIndex];
					Render(worldStaticRenderStructData);
				}
				
			}
		}

		protected override void ExitLogic()
		{
			_dealRegionDataFinish = false;
			_dealCount = 0;
			if (worldStaticStructNativeList.IsCreated)
			{
				worldStaticStructNativeList.Dispose();
			}
			if (worldStaticTotalNativeList.IsCreated)
			{
				worldStaticTotalNativeList.Dispose();
			}
			if (filterRegionDataNativeList.IsCreated)
			{
				filterRegionDataNativeList.Dispose();
			}
			if (filterCullingDataNativeList.IsCreated)
			{
				filterCullingDataNativeList.Dispose();
			}
			if (filterRegionRenderDataNativeList.IsCreated)
			{
				filterRegionRenderDataNativeList.Dispose();
			}
			if (regionOffsetNativeList.IsCreated)
			{
				regionOffsetNativeList.Dispose();
			}
		}



		protected override void DrawGizmosLogic()
		{
			unsafe
			{
				var WorldRegionStaticRenderHeadStructDataPtr = (WorldRegionStaticRenderHeadStructData*)worldStaticStructNativeList.GetUnsafePtr();
				for (int index = 0; index < worldStaticStructNativeList.Length; index++)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(WorldRegionStaticRenderHeadStructDataPtr[index].regionBounds.center, WorldRegionStaticRenderHeadStructDataPtr[index].regionBounds.size);
					var pos = WorldRegionStaticRenderHeadStructDataPtr[index].regionPos;
					var halfWidth = regionWidth / 2;
					var halfHeight = regionHeight / 2;
					Gizmos.color = Color.green;
					Gizmos.DrawLine(new Vector3(pos.x - halfWidth, 0, pos.z - halfHeight), new Vector3(pos.x + halfWidth, 0, pos.z - halfHeight));
					Gizmos.DrawLine(new Vector3(pos.x + halfWidth, 0, pos.z - halfHeight), new Vector3(pos.x + halfWidth, 0, pos.z + halfHeight));
					Gizmos.DrawLine(new Vector3(pos.x + halfWidth, 0, pos.z + halfHeight), new Vector3(pos.x - halfWidth, 0, pos.z + halfHeight));
					Gizmos.DrawLine(new Vector3(pos.x - halfWidth, 0, pos.z + halfHeight), new Vector3(pos.x - halfWidth, 0, pos.z - halfHeight));
				}

				// var worldStaticTotalNativeListPtr = (WorldStaticRenderStructData*)worldStaticTotalNativeList.GetUnsafePtr();
				// var filterCullingDataNativeListPtr = (int*)filterCullingDataNativeList.GetUnsafePtr();
				// var filterRegionRenderDataNativeListPtr = (int*)filterRegionRenderDataNativeList.GetUnsafePtr();
				// for (int index = 0; index < filterRegionRenderDataNativeList.Length; index++)
				// {
				// 	int filterRegionRenderDataNativeListIndex = filterRegionRenderDataNativeListPtr[index];
				// 	int filterCullingDataNativeListIndex = filterCullingDataNativeListPtr[filterRegionRenderDataNativeListIndex];
				// 	WorldStaticRenderStructData worldStaticRenderStructData = worldStaticTotalNativeListPtr[filterCullingDataNativeListIndex];
				// 	Gizmos.DrawWireSphere(worldStaticRenderStructData.GetRenderPos(), worldStaticRenderStructData.boundRadius);
				// }

				
			}
		}

		protected override string getType()
		{
			return "RenderFactoryStatic";
		}

		public override bool NeedRender()
		{
			return _dealRegionDataFinish && 
			       totalWorldRenderNum > 0 &&
			       _bigWorldRenderData.currentRenderLod < BigWorldRenderManager.Consts.TOP_RENDER_LOD;
		}


		/// <summary>
        /// 加载分块数据
        /// </summary>
        public void DealWorldRegionDetailData()
		{
			int loaditeration = (int) Math.Ceiling(totalRegionNum / 16f);
            
            for (int index = 0; index < loaditeration; index++)
            {
	            if (_dealCount >= totalRegionNum)
		            break;
                LoadMapData();
            }
            
            if (_dealCount >= totalRegionNum)
            {
	            _dealRegionDataFinish = true;

                if (_bigWorldRenderData.StaticLoadFinishCallback != null)
                {
	                _bigWorldRenderData.StaticLoadFinishCallback();
                    _bigWorldRenderData.StaticLoadFinishCallback = null;
                }
            }
        }

        private void LoadMapData()
        {
	        var mapOffset = _bigWorldRenderData.MapOffset;
	        var worldRegionDetailScriptableObjectList =
		        _bigWorldRenderData.ResRenderData.worldRegionDetailScriptableObjectList;
            var worldRegionDetailScriptableObject = worldRegionDetailScriptableObjectList[_dealCount];
            int col = worldRegionDetailScriptableObject.regionCol;
            int row = worldRegionDetailScriptableObject.regionRow;
            var bounds = worldRegionDetailScriptableObject.regionBounds;

            WorldRegionStaticRenderHeadStructData worldRegionStaticRenderHeadStructData = new WorldRegionStaticRenderHeadStructData();
            worldRegionStaticRenderHeadStructData.beginIndex = worldRegionBeginIndex;
            worldRegionStaticRenderHeadStructData.regionPos =
                new Vector3(row * regionWidth + regionWidth / 2, 0, -(col * regionHeight + regionHeight / 2)) + mapOffset;
            worldRegionStaticRenderHeadStructData.regionX = row;
            worldRegionStaticRenderHeadStructData.regionBounds = bounds;

            foreach (var renderRes in worldRegionDetailScriptableObject.renderResList)
            {
                int resId = renderRes.resId;

                if (!_staticRegionAssetDic.ContainsKey(resId))
                {
                    continue;
                }


                int cullingGrade = _staticRegionAssetDic[resId].cullingGrade;

                List<Vector3> posList = renderRes.posList;
                List<Vector3> eulerList = renderRes.eulerList;
                List<Vector3> scaleList = renderRes.scaleList;
                List<float> boundRadIuList = renderRes.boundRadiusList;

                for (int index = 0; index < posList.Count; index++)
                {

                    WorldStaticRenderStructData worldStaticRenderStructData = new WorldStaticRenderStructData();
                    worldStaticRenderStructData.assetId = renderRes.resId;
                    worldStaticRenderStructData.boundRadius = boundRadIuList[index];
                    worldStaticRenderStructData.regionRow = row;
                    //worldStaticRenderStructData.logicPos = posList[index] + mapOffsetVec3;
                    worldStaticRenderStructData.cullingGrade = cullingGrade;

                    if (!_staticRegionAssetDic.ContainsKey(renderRes.resId))
                    {
                        Debug.LogError("regionAssetDic 不存在 " + renderRes.resId);
                        worldStaticRenderStructData.renderLod = 1;
                    }
                    else
                    {
                        worldStaticRenderStructData.renderLod = _staticRegionAssetDic[renderRes.resId].lod;
                    }

                    worldStaticRenderStructData.renderMatrix = Matrix4x4.TRS(posList[index] + mapOffset, Quaternion.Euler(eulerList[index]), scaleList[index]);


                    worldStaticTotalNativeList.Add(worldStaticRenderStructData);
                    
                }

                worldRegionBeginIndex += posList.Count;

            }
            worldRegionStaticRenderHeadStructData.endIndex = worldRegionBeginIndex - 1;
            worldStaticStructNativeList.Add(worldRegionStaticRenderHeadStructData);
            _dealCount++;
        }
        
        
        /// <summary>
        /// 裁剪地块 地块数量不多没有使用JOB优化
        /// </summary>
        public void CullingRegion()
        {
	        var frustumPlanes = _bigWorldRenderData.GetFrustumPlanes();
	        
	        filterRegionDataNativeList.Clear();
	        for (int worldStaticStructNativeListIndex = 0; worldStaticStructNativeListIndex < worldStaticStructNativeList.Length; worldStaticStructNativeListIndex++)
	        {
		        if (FilterViewFrustumSingleSizeCullingRegion(frustumPlanes, worldStaticStructNativeList, worldStaticStructNativeListIndex))
		        {
			        filterRegionDataNativeList.Add(worldStaticStructNativeListIndex);
		        }
	        }
        }
        
        /// <summary> 
        /// 是否需要裁剪   地块数量不多没有使用JOB优化 
        /// <returns></returns>
        private  bool FilterViewFrustumSingleSizeCullingRegion(NativeList<float4> frustumPlanes, NativeList<WorldRegionStaticRenderHeadStructData> worldStaticStructNativeList, int i)
        {
	        unsafe
	        {
		        var FrustumPlanePtr = (float4*)frustumPlanes.GetUnsafePtr();
		        var worldStaticStructNativeListPtr = (WorldRegionStaticRenderHeadStructData*)worldStaticStructNativeList.GetUnsafePtr();
		        
		        // 有些块上的物体可能在边缘，往外扩点
		        // Leon-TODO: 按地块真正的boudns裁切
		        float3 extents = worldStaticStructNativeListPtr[i].regionBounds.extents;
		        float3 positions = worldStaticStructNativeListPtr[i].regionBounds.center;
		        for (int j = 0; j < 6; j++)
		        {
			        float3 planeNormal = FrustumPlanePtr[j].xyz;

			        float planeConstant = FrustumPlanePtr[j].w;

			        if (math.dot(extents, math.abs(planeNormal)) +
			            math.dot(planeNormal, positions) +
			            planeConstant <= 0f)
				        return false;
		        }
		        //ttList[i].transform.position = positions;
		        return true;

	        }
        }
        
        /// <summary>
        /// 获取需要裁剪的数据
        /// </summary>
        public void FilterRegionData()
        {
	        unsafe
	        {
		        var worldStaticStructNativeListPtr = (WorldRegionStaticRenderHeadStructData*)worldStaticStructNativeList.GetUnsafePtr();
		        var filterRegionDataNativeListPtr = (int*)filterRegionDataNativeList.GetUnsafePtr();
		        filterCullingDataNativeList.Clear();
		        for (int filterRegionDataNativeListIndex = 0; filterRegionDataNativeListIndex < filterRegionDataNativeList.Length; filterRegionDataNativeListIndex++)
		        {
			        WorldRegionStaticRenderHeadStructData tempWorldRegionData = worldStaticStructNativeListPtr[filterRegionDataNativeListPtr[filterRegionDataNativeListIndex]];
			        int beginIndex = tempWorldRegionData.beginIndex;
			        int endIndex = tempWorldRegionData.endIndex;

			        for (int worldStaticTotalNativeListIndex = beginIndex; worldStaticTotalNativeListIndex <= endIndex; worldStaticTotalNativeListIndex++)
			        {
				        filterCullingDataNativeList.Add(worldStaticTotalNativeListIndex);
			        }
		        }

	        }

        }
        
        /// <summary>
        /// 裁剪建筑数据
        /// </summary>
        /// <returns></returns>
        public JobHandle ScheduleCullingDynaColliderJob()
        {
	        var worldCullingColliderStructDataNativeList = _bigWorldRenderData.worldCullingColliderStructDataNativeList;
	        var useWorldCulingDataNativeList = _bigWorldRenderData.useWorldCulingDataNativeList;
	        _bigWorldRenderData.worldCullingColliderStructDataCullingNativeList.Clear();

	        if (worldCullingColliderStructDataNativeList.Length == 0)
	        {
		        return default;
	        }

	        return new FilterViewCullingColliderRenderDataJob()
	        {
		        _cullingDataNativeList = useWorldCulingDataNativeList,
		        _worldCullingColliderStructDatas = worldCullingColliderStructDataNativeList,
		        _frustumPlanes = _bigWorldRenderData.GetFrustumPlanes()
	        }.ScheduleAppend(_bigWorldRenderData.worldCullingColliderStructDataCullingNativeList, useWorldCulingDataNativeList.Length, 16);

        }
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="CullingDataJobHandle"></param>
		/// <returns></returns>
        public JobHandle ScheduleCullingStaticDataJob(JobHandle CullingDataJobHandle = default)
        {
	        filterRegionRenderDataNativeList.Clear();
	        return new FilterViewFrustumCullingRenderDataJob
	        {
		        _cullingDataNativeList = filterCullingDataNativeList,
		        _worldStaticRenderStructDatas = worldStaticTotalNativeList,
		        _frustumPlanes = _bigWorldRenderData.GetFrustumPlanes(),
		        _regionW = new float3(regionWidth,0,0),
		        _regionOffsetNativeArray = regionOffsetNativeList,
		        currentRenderLod = _bigWorldRenderData.currentRenderLod,
	        }.ScheduleAppend(filterRegionRenderDataNativeList, filterCullingDataNativeList.Length, 16, CullingDataJobHandle);

            
        }
		
		/// <summary>
        /// 碰撞体裁剪 裁剪数量较少 没有使用Job
        /// </summary>
        public void ArchitectureCulling()
        {
            //获取裁剪对象
            var worldCullingColliderStructDataCullingNativeList = _bigWorldRenderData.worldCullingColliderStructDataCullingNativeList;
            var worldCullingColliderStructDataNativeList = _bigWorldRenderData.worldCullingColliderStructDataNativeList;
            var useWorldCulingDataNativeList = _bigWorldRenderData.useWorldCulingDataNativeList;
            if (worldCullingColliderStructDataCullingNativeList.Length == 0)
            {
                return;
            }
            unsafe
            {
                var worldCullingColliderStructDataNativeListPtr = (WorldColliderStructData*)worldCullingColliderStructDataNativeList.GetUnsafePtr();
                var worldCullingColliderStructDataCullingNativeListPtr = (int*)worldCullingColliderStructDataCullingNativeList.GetUnsafePtr();
                var filterRegionRenderDataNativeListPtr = (int*)filterRegionRenderDataNativeList.GetUnsafePtr();
                var filterCullingDataNativeListPtr = (int*)filterCullingDataNativeList.GetUnsafePtr();
                var useWorldCulingDataNativeListPtr = (int*)useWorldCulingDataNativeList.GetUnsafePtr();
                var worldStaticTotalNativeListPtr = (WorldStaticRenderStructData*)worldStaticTotalNativeList.GetUnsafePtr();
                float distance;
                for (int filterRegionRenderDataIndex = filterRegionRenderDataNativeList.Length - 1; filterRegionRenderDataIndex >= 0; filterRegionRenderDataIndex--)
                {
                    //获取filterCullingDataNativeList的index
                    int filterRegionRenderDataNativeListIndex = filterRegionRenderDataNativeListPtr[filterRegionRenderDataIndex];
                    //获取worldStaticTotalNativeList的Index
                    int filterCullingDataIndex = filterCullingDataNativeListPtr[filterRegionRenderDataNativeListIndex];
                    WorldStaticRenderStructData worldStaticRenderStructData = worldStaticTotalNativeListPtr[filterCullingDataIndex];
                    float rafius = worldStaticRenderStructData.boundRadius;
                    float3 pos = worldStaticRenderStructData.GetRenderPos();
                    int assetId = worldStaticRenderStructData.assetId;


                    for (int cullingColliderIndex = 0; cullingColliderIndex < worldCullingColliderStructDataCullingNativeList.Length; cullingColliderIndex++)
                    {
                        int worldCullingColliderStructDataIndex = worldCullingColliderStructDataCullingNativeListPtr[cullingColliderIndex];
                        int useWorldCulingDataIndex = useWorldCulingDataNativeListPtr[worldCullingColliderStructDataIndex];
                        WorldColliderStructData tempWorldCullingColliderStructData =
                            worldCullingColliderStructDataNativeListPtr[useWorldCulingDataIndex];


                        distance = Vector3.Distance(pos, tempWorldCullingColliderStructData.GetColliderPos());
                        if (distance < (rafius + tempWorldCullingColliderStructData.GetColliderBound()) /*&& rafius < tempWorldCullingColliderStructData.GetColliderBound()*/)
                        {
                            filterRegionRenderDataNativeList.RemoveAtSwapBack(filterRegionRenderDataIndex);
                            break;
                        }
                    }
                }
            }
        }
		
        
        
        /// <summary>
        /// 循环相机 地块偏移计算
        /// </summary>
        private void CalCameraOffset()
        {
            if (!_bigWorldRenderData.isLoopX)
                return;
            if (_bigWorldRenderData.renderCameraTrans == null)
	            return;
	        var cameratrans = _bigWorldRenderData.renderCameraTrans;

            var regionOffset = cameratrans.position;
            float offsetNum = regionOffset.x;
            int offsetVec;
            int rowOffset;
            int regionff;
            int bb;
            if (offsetNum >= 0)
            {
                //偏了几格                                                                                           // 0 1 2 3 4 5 6 7 8  9 10 11 12 13 14 15  // 14 15 0 1 2 3 4 5 6 7 8 9 10 11 12 13
                offsetVec = (int)offsetNum / (int)regionWidth;                                                   // 9 10 11 12 13 14 15 0 1 2 3 4 5 6 7 8  15-7= 8 16 -7 9 1-8 -7
                                                                                                                     //整体偏移
                rowOffset = (offsetVec / regionRowNum) * regionRowNum;

                //相机处于第几块
                bb = offsetVec % regionColNum;
                

            }
            else
            {
                //offsetNum +=
                //偏了几格                                                                                           
                offsetVec = (int)offsetNum / (int)regionWidth;

                regionff = -offsetVec / regionRowNum ;

                bb = (offsetVec + (regionff+1)*(int)totalMapWidth) % regionColNum;

                if (bb == 0)
                    rowOffset = -regionff * regionRowNum;
                else
                    rowOffset = -regionff * regionRowNum - regionRowNum;

            }
            for (int index = 0; index < regionRowNum; index++)
            {
                var tmp = index - bb;
                if (tmp < regionRowNum / 2 && tmp > -regionRowNum / 2)
                {
                    regionOffsetNativeList[index] = rowOffset;
                }
                else if (tmp <= -regionRowNum / 2)
                {
                    regionOffsetNativeList[index] = rowOffset + regionColNum;
                }
                else
                {
                    regionOffsetNativeList[index] = rowOffset - regionColNum;
                }
            }
        }
        
        
        private float3 GetRegionOffset(WorldRegionStaticRenderHeadStructData regionData)
        {
	        return new float3(regionOffsetNativeList[regionData.regionX] * regionWidth, 0, 0);
        }
	}
}