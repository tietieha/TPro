// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-16 18:00 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************


using System;
using System.Collections.Generic;
using TEngine;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace BigWorldRender
{
	public class BigWorldRenderData
	{
		private const int CULLING_COLLIDER_MAX = 1000;

		public int currentRenderLod = 1;

		// 地图的偏移值
		public Vector3 MapOffset;

		// setting
		public bool isLoopX = true;
		public bool isLoopY = true;

		// Camera
		public Camera renderCamera;
		public Transform renderCameraTrans;

		private Vector4 _terrainTileSize;

		// Leon-TODO: 裁切面放到job内
		//相机的裁剪面
		private NativeList<float4> _frustumPlanes;

		// 资源 ========================================================================================================
		// Leon-TODO: 将资源容器分装到各个工厂内部
		// 资源列表 key 资源名字 value mesh mat  静态资源
		public Dictionary<int, RegionAssetData> StaticRegionAssetDic;

		// 数据 ========================================================================================================
		public ResRenderData ResRenderData;
		public Action StaticLoadFinishCallback;
		private int totalRegionNum;

		// 动态数据 =====================================================================================================
		/// 全部建筑数据
		public NativeList<WorldColliderStructData> worldCullingColliderStructDataNativeList;

		/// 空数据索引
		public NativeQueue<int> empWorldCulingColliderDataNativeQueue;

		/// 使用的数据索引
		public NativeList<int> useWorldCulingDataNativeList;

		/// 记录 裁剪建筑物后渲染数据Index
		public NativeList<int> worldCullingColliderStructDataCullingNativeList;
		// 动态数据 =====================================================================================================

		public bool Init(ResRenderData resRenderData, Vector3 mapOffset, Action staticLoadFinishCallback)
		{
			Reset();
			resRenderData.InitData();
			ResRenderData = resRenderData;
			MapOffset = mapOffset;
			StaticLoadFinishCallback = staticLoadFinishCallback;

			var worldRegionTotalScriptableObject = resRenderData.worldRegionTotalScriptableObject;
			if (worldRegionTotalScriptableObject == null)
			{
				Debug.LogError("[BigWorldRender] worldRegionTotalScriptableObject = null");
				return false;
			}

			totalRegionNum = worldRegionTotalScriptableObject.regionRowNum *
			                 worldRegionTotalScriptableObject.regionColNum;
			_terrainTileSize =
				new Vector4(
					worldRegionTotalScriptableObject.regionTotalWidth,
					worldRegionTotalScriptableObject.regionTotalHeight,
					mapOffset.x, mapOffset.z);
			
			Shader.SetGlobalVector("_LowTerrainTileSize", _terrainTileSize);
			
			if (resRenderData.UVScaleData != null)
			{
				Shader.SetGlobalVectorArray("_UVScales", resRenderData.UVScaleData.uvs);
			}

			// =========================================================================================================
			// 资源列表
			var regionResResDataList = worldRegionTotalScriptableObject.regionResResDataList;
			int resCount = worldRegionTotalScriptableObject.regionResResDataList.Count;
			StaticRegionAssetDic = new Dictionary<int, RegionAssetData>(resCount);
			for (int index = 0; index < regionResResDataList.Count; index++)
			{
				string meshName = regionResResDataList[index].meshName;
				string matName = regionResResDataList[index].materialName;
				int renderType = regionResResDataList[index].renderType;
				int id = regionResResDataList[index].resID;
				int lod = regionResResDataList[index].renderLod;
				int cullingGrade = regionResResDataList[index].cullingGrade;
				int subMeshIndex = regionResResDataList[index].subMeshIndex;
				if (StaticRegionAssetDic.ContainsKey(id))
				{
					Debug.LogError("[BigWorldRender] regionAssetDic 存在 " + id);
					continue;
				}

				RegionAssetData tempRegionAssetData = new RegionAssetData();
				Mesh mesh = ResRenderData.GetAssetMesh(meshName);
				if (mesh == null)
				{
					Debug.LogError($"[BigWorldRender] {meshName} == null");
					mesh = new Mesh();
				}

				tempRegionAssetData.mesh = mesh; //Resources.Load(meshName) as Mesh;
				Material mat = ResRenderData.GetAssetMaterial(matName);

				if (mat == null)
				{
					Debug.LogError($"[BigWorldRender] {matName} == null");
					continue;
				}

				//假如是Instancing渲染类型
				if (!mat.enableInstancing && renderType == 2)
					Debug.LogError($"[BigWorldRender] {mat.name} 不支持instance绘制");


				tempRegionAssetData.material = mat; //Resources.Load(matName) as Material;
				tempRegionAssetData.renderType = renderType;
				tempRegionAssetData.lod = lod;
				tempRegionAssetData.cullingGrade = cullingGrade;
				tempRegionAssetData.subMeshIndex = subMeshIndex;
				StaticRegionAssetDic.Add(id, tempRegionAssetData);
			}

			// =================== 动态绘制容器 =========================================================================
			worldCullingColliderStructDataCullingNativeList = new NativeList<int>(totalRegionNum, Allocator.Persistent);
			worldCullingColliderStructDataNativeList =
				new NativeList<WorldColliderStructData>(CULLING_COLLIDER_MAX, Allocator.Persistent);
			useWorldCulingDataNativeList = new NativeList<int>(CULLING_COLLIDER_MAX, Allocator.Persistent);
			empWorldCulingColliderDataNativeQueue = new NativeQueue<int>(Allocator.Persistent);
			for (int index = 0; index < CULLING_COLLIDER_MAX; index++)
			{
				empWorldCulingColliderDataNativeQueue.Enqueue(index);
				worldCullingColliderStructDataNativeList.Add(new WorldColliderStructData());
			}

			_frustumPlanes = new NativeList<float4>(6, Allocator.Persistent);
			for (int i = 0; i < 6; i++)
			{
				_frustumPlanes.Add(float4.zero);
			}

			return true;
		}

		public void Reset()
		{
			currentRenderLod = 1;
			if (worldCullingColliderStructDataNativeList.IsCreated)
				worldCullingColliderStructDataNativeList.Dispose();
			if (worldCullingColliderStructDataCullingNativeList.IsCreated)
				worldCullingColliderStructDataCullingNativeList.Dispose();
			if (empWorldCulingColliderDataNativeQueue.IsCreated)
				empWorldCulingColliderDataNativeQueue.Dispose();
			if (useWorldCulingDataNativeList.IsCreated)
				useWorldCulingDataNativeList.Dispose();
			if (_frustumPlanes.IsCreated)
				_frustumPlanes.Dispose();
			if (StaticRegionAssetDic != null)
				StaticRegionAssetDic.Clear();
			ResRenderData = null;
		}

		public void Exit()
		{
			Reset();

			if (BigWorldRenderManager.DebugConf.IsLog)
				Log.Info($"[BigWorldRender] BigWorldRenderData - Exit");
		}

		public NativeList<float4> GetFrustumPlanes()
		{
			return _frustumPlanes;
		}

		private Plane[] _planes = new Plane[6];

		public void SetFrustumPlanes(float4x4 worldProjectionMatrix)
		{
			//_frustumPlanes.Clear();
			//计算视景平面
			GeometryUtility.CalculateFrustumPlanes(worldProjectionMatrix, _planes);

			for (int i = 0; i < 6; ++i)
			{
				_frustumPlanes[i] = new float4(_planes[i].normal, _planes[i].distance);
			}
		}

		public void SetWorldLoop(bool loopX, bool loopY)
		{
			isLoopX = loopX;
			isLoopY = loopY;
		}

		public void SetWorldCamera(Camera camera)
		{
			renderCamera = camera;
			renderCameraTrans = camera.transform;
		}

		public void ConfigMaterialPassEnable(List<string> filterShader, string pass, bool enable)
		{
			if (ResRenderData != null)
			{
				ResRenderData.ConfigMaterialPassEnable(filterShader, pass, enable);
			}
		}
	}
}