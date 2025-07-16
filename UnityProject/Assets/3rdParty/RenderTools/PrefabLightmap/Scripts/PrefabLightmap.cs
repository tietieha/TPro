// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-02-28 16:23 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace RenderTools.PrefabLightmap
{
	[ExecuteAlways]
	public class PrefabLightmap : MonoBehaviour
	{
		#region 引用计数
		[ListDrawerSettings]
		private static Dictionary<CustomLightmapData, int> S_LightmapDataRefenceCount = new Dictionary<CustomLightmapData, int>();
		public static void Clear()
		{
			S_LightmapDataRefenceCount.Clear();
		}
		#endregion

		[SerializeField] CustomLightmapData[] m_PrefabLightMapData;
		[SerializeField] RendererInfo[] m_RendererInfo;

		[SerializeField] LightInfo[] m_LightInfo;
		// [SerializeField] Texture2D[] m_Lightmaps;
		// [SerializeField] Texture2D[] m_LightmapsDir;
		// [SerializeField] Texture2D[] m_ShadowMasks;

		void Awake()
		{
			Init();
		}

		void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		// called when the game is terminated
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		void OnDestroy()
		{
			foreach (var data in m_PrefabLightMapData)
			{
				if (S_LightmapDataRefenceCount.TryGetValue(data, out var count))
				{
					count--;
					S_LightmapDataRefenceCount.Remove(data);
					if (count == 0)
					{
						var lightmaps = LightmapSettings.lightmaps;
						for (int i = 0; i < lightmaps.Length; i++)
						{
							if (data.IsEqual(lightmaps[i]))
							{
								lightmaps[i].lightmapColor = null;
								lightmaps[i].lightmapDir = null;
								lightmaps[i].shadowMask = null;
							}
						}

						LightmapSettings.lightmaps = lightmaps;
					}
					else
					{
						S_LightmapDataRefenceCount.Add(data, count);
					}
				}
			}
		}

		// called second
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Init();
		}

		void Init()
		{
			if (m_RendererInfo == null || m_RendererInfo.Length == 0)
				return;

			int lightdirCount = 0;
			int lightmapCount = 0;
			var lightmaps = LightmapSettings.lightmaps;
			List<LightmapData> lightmapList = new List<LightmapData>(lightmaps);
			int[] offsetindexes = new int[m_PrefabLightMapData.Length];

			for (int i = 0; i < m_PrefabLightMapData.Length; i++)
			{
				var lightMapIndex = -1;
				var nullIndex = -1;

				if (m_PrefabLightMapData[i].LightmapDir != null)
					lightdirCount++;
				if (m_PrefabLightMapData[i].LightmapColor != null)
					lightmapCount++;

				//
				LightmapData currentData = null;
				for (int j = lightmapList.Count - 1; j >= 0; j--)
				{
					var lightmap = lightmapList[j];
					if (m_PrefabLightMapData[i].IsEqual(lightmap))
					{
						lightMapIndex = j;
						currentData = lightmap;
					}

					if (lightmap.lightmapColor == null &&
					    lightmap.lightmapDir == null &&
					    lightmap.shadowMask == null)
					{
						nullIndex = j;
					}
				}

				// 没找到，看往哪塞
				if (lightMapIndex == -1)
				{
					currentData = m_PrefabLightMapData[i].GetLightmapData();
					if (nullIndex == -1)
					{
						lightmapList.Add(currentData);
						lightMapIndex = lightmapList.Count - 1;
					}
					else
					{
						lightmapList[nullIndex] = currentData;
						lightMapIndex = nullIndex;
					}
				}

				offsetindexes[i] = lightMapIndex;
			}
			
			foreach (var data in this.m_PrefabLightMapData)
			{
				if (!S_LightmapDataRefenceCount.TryGetValue(data, out var count))
				{
					S_LightmapDataRefenceCount.Add(data, 1);
				}
				else
				{
					S_LightmapDataRefenceCount[data]++;
				}
			}


			bool directional = lightdirCount > 0;

			LightmapSettings.lightmapsMode = (lightmapCount == lightdirCount && directional)
				? LightmapsMode.CombinedDirectional
				: LightmapsMode.NonDirectional;
			ApplyRendererInfo(m_RendererInfo, offsetindexes, m_LightInfo);
			LightmapSettings.lightmaps = lightmapList.ToArray();
		}

		static void ApplyRendererInfo(RendererInfo[] infos, int[] lightmapOffsetIndex, LightInfo[] lightsInfo)
		{
			bool shadowmask = false;
			for (int i = 0; i < lightsInfo.Length; i++)
			{
				LightBakingOutput bakingOutput = new LightBakingOutput();
				bakingOutput.isBaked = true;
				bakingOutput.lightmapBakeType = (LightmapBakeType) lightsInfo[i].lightmapBaketype;
				bakingOutput.mixedLightingMode = (MixedLightingMode) lightsInfo[i].mixedLightingMode;

				lightsInfo[i].light.bakingOutput = bakingOutput;
				if (!shadowmask)
				{
					shadowmask = bakingOutput.mixedLightingMode != MixedLightingMode.IndirectOnly;
				}
			}
			
			for (int i = 0; i < infos.Length; i++)
			{
				var info = infos[i];

				if (info.renderer == null)
					continue;
				info.renderer.lightmapIndex = lightmapOffsetIndex[info.lightmapIndex];
				info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;
				if (Application.isPlaying && shadowmask)
				{
					info.renderer.shadowCastingMode = ShadowCastingMode.Off;
				}
				// You have to release shaders.
				// Material[] mat = info.renderer.sharedMaterials;
				// for (int j = 0; j < mat.Length; j++)
				// {
				//     if (mat[j] != null && Shader.Find(mat[j].shader.name) != null)
				//         mat[j].shader = Shader.Find(mat[j].shader.name);
				// }
			}


		}

		// ============================================== 属性类 ========================================================
		#region 属性类
		[System.Serializable]
		struct RendererInfo
		{
			public Renderer renderer;
			public int lightmapIndex;
			public Vector4 lightmapOffsetScale;
		}

		[System.Serializable]
		struct LightInfo
		{
			public Light light;
			public int lightmapBaketype;
			public int mixedLightingMode;
		}

		[System.Serializable]
		struct CustomLightmapData
		{
			/// <summary>
			/// The color for lightmap.
			/// </summary>
			public Texture2D LightmapColor;

			/// <summary>
			/// The dir for lightmap.
			/// </summary>
			public Texture2D LightmapDir;

			/// <summary>
			/// The shadowmask for lightmap.
			/// </summary>
			public Texture2D ShadowMask;

			/// <summary>
			/// Initializes a new instance of the <see cref="CustomLightmapData"/> struct.
			/// </summary>
			/// <param name="data">lightmapdata.</param>
			public CustomLightmapData(LightmapData data)
			{
				this.LightmapColor = data.lightmapColor;
				this.LightmapDir = data.lightmapDir;
				this.ShadowMask = data.shadowMask;
			}

			public bool IsEqual(LightmapData data)
			{
				return this.LightmapColor == data.lightmapColor &&
				       this.LightmapDir == data.lightmapDir &&
				       this.ShadowMask == data.shadowMask;
			}

			public LightmapData GetLightmapData()
			{
				LightmapData data = new LightmapData();
				data.lightmapColor = this.LightmapColor ? this.LightmapColor : default(Texture2D);
				data.lightmapDir = this.LightmapDir ? this.LightmapDir : default(Texture2D);
				data.shadowMask = this.ShadowMask ? this.ShadowMask : default(Texture2D);
				return data;
			}

#if UNITY_EDITOR
			public void DeleteOldTex()
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(LightmapColor));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(LightmapDir));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(ShadowMask));
			}
#endif
		}
		#endregion

		// ============================================== EDITOR =======================================================
		#region Editor
#if UNITY_EDITOR
#if ODIN_INSPECTOR
		[PropertySpace(SpaceBefore = 50)]
		[InfoBox("使用说明：\n" +
		         "1.烘焙时确保场景中，只有当前物体显示\n" +
		         "2.光图保存目录默认为预制体的同目录\n" +
		         "3.注意控制Mixed 和 Realtime灯光的数量")]
		[Button("保存LightMap信息", ButtonSizes.Large)]
#endif
		void SaveLightmapInfo()
		{
#if UNITY_2018_3_OR_NEWER
			var prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
#else
			var prefab = UnityEditor.PrefabUtility.GetPrefabParent(gameObject);
#endif
			if (prefab == null)
			{
				if (EditorUtility.DisplayDialog("警告", "当前物体不是预制体, 无法保存LightMap信息", "确认"))
				{
					return;
				}
			}

			PrefabLightmap[] prefabs = gameObject.GetComponentsInChildren<PrefabLightmap>(true);

			// 保存 lightmap 信息
			foreach (var instance in prefabs)
			{
				var gameObject = instance.gameObject;

				var rendererInfos = new List<RendererInfo>();
				var lightmaps = new List<Texture2D>();
				var lightmapsDir = new List<Texture2D>();
				var shadowMasks = new List<Texture2D>();
				var lightsInfos = new List<LightInfo>();
				var prefabLightmapData = new List<CustomLightmapData>();

				GenerateLightmapInfo(gameObject, rendererInfos, lightmaps, lightmapsDir, shadowMasks, lightsInfos);

				// 设置数据
				// instance.m_Lightmaps = lightmaps.ToArray();
				// instance.m_LightmapsDir = lightmapsDir.ToArray();
				// instance.m_ShadowMasks = shadowMasks.ToArray();
				instance.m_LightInfo = lightsInfos.ToArray();
				instance.m_RendererInfo = rendererInfos.ToArray();

				// 保存光照贴图，保存光照信息
#if UNITY_2018_3_OR_NEWER
				var prefabSource = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance.gameObject);
				var prefabSourcePath = AssetDatabase.GetAssetPath(prefabSource);
				var prefabname = instance.gameObject.name;
#else
				var prefabSource = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) as GameObject;
				var prefabSourcePath = AssetDatabase.GetAssetPath(prefabSource);
				var prefabname = prefab.name;
#endif
				var prefabFolder = Path.GetDirectoryName(prefabSourcePath);
				var foldername = $"{prefabname}_LightMap";
				var lightmapSavePath = Path.Combine(prefabFolder, foldername);

				// 删除旧的光照贴图
				AssetDatabase.DeleteAsset(lightmapSavePath);
				AssetDatabase.CreateFolder(prefabFolder, foldername);
				// 避免漏删
				for (int i = 0; i < instance.m_PrefabLightMapData.Length; i++)
				{
					instance.m_PrefabLightMapData[i].DeleteOldTex();
				}

				// lightmap
				// 修改lightMapData格式
				for (int i = 0; i < lightmaps.Count; i++)
				{
					CustomLightmapData data = new CustomLightmapData();

					data.LightmapColor = Helpers.CopyTextureToFolder(lightmaps[i], lightmapSavePath);
					data.LightmapDir = Helpers.CopyTextureToFolder(lightmapsDir[i], lightmapSavePath);
					data.ShadowMask = Helpers.CopyTextureToFolder(shadowMasks[i], lightmapSavePath);

					prefabLightmapData.Add(data);
				}

				instance.m_PrefabLightMapData = prefabLightmapData.ToArray();

#if UNITY_2018_3_OR_NEWER
				var targetPrefab = prefabSource as GameObject;
				if (targetPrefab != null)
				{
					GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(instance.gameObject); // 根结点
					//如果当前预制体是是某个嵌套预制体的一部分（IsPartOfPrefabInstance）
					if (root != null)
					{
						GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance.gameObject);
						string rootPath = AssetDatabase.GetAssetPath(rootPrefab);
						//打开根部预制体
						PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root,
							PrefabUnpackMode.OutermostRoot);
						try
						{
							//Apply各个子预制体的改变
							PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
						}
						catch
						{
						}
						finally
						{
							//重新更新根预制体
							PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, InteractionMode.AutomatedAction);
						}
					}
					else
					{
						PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
					}
				}
#else
	            var targetPrefab = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) as GameObject;
	            if (targetPrefab != null)
	            {
	                //UnityEditor.Prefab
	                UnityEditor.PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
	            }
#endif
			}
		}
		#if ODIN_INSPECTOR
		[Button("一键烘培 + 保存", ButtonSizes.Large)]
		#endif
		void GenerateLightmapInfo()
		{
			if (UnityEditor.Lightmapping.giWorkflowMode != UnityEditor.Lightmapping.GIWorkflowMode.OnDemand)
			{
				Debug.LogError(
					"ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
				return;
			}

			// PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();
			PrefabLightmap[] prefabs = gameObject.GetComponentsInChildren<PrefabLightmap>(true);
			if (prefabs.Length > 1)
			{
				if (!EditorUtility.DisplayDialog("警告", "子节点中有多个 PrefabLightmap 脚本", "确认烘焙", "取消"))
				{
					return;
				}
			}

			// TODO: 是否自动全部设成Static
			// GameObjectUtility.SetStaticEditorFlags(gameObject, StaticEditorFlags.ContributeGI);

			UnityEditor.Lightmapping.Bake();

			SaveLightmapInfo();
		}

		static void GenerateLightmapInfo(GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmaps,
			List<Texture2D> lightmapsDir, List<Texture2D> shadowMasks, List<LightInfo> lightsInfo)
		{
			var renderers = root.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in renderers)
			{
				if (renderer.lightmapIndex != -1)
				{
					RendererInfo info = new RendererInfo();
					info.renderer = renderer;

					if (renderer.lightmapScaleOffset != Vector4.zero)
					{
						//1ibrium's pointed out this issue : https://docs.unity3d.com/ScriptReference/Renderer-lightmapIndex.html
						if (renderer.lightmapIndex < 0 || renderer.lightmapIndex == 0xFFFE) continue;
						info.lightmapOffsetScale = renderer.lightmapScaleOffset;

						Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;
						Texture2D lightmapDir = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapDir;
						Texture2D shadowMask = LightmapSettings.lightmaps[renderer.lightmapIndex].shadowMask;

						info.lightmapIndex = lightmaps.IndexOf(lightmap);
						if (info.lightmapIndex == -1)
						{
							info.lightmapIndex = lightmaps.Count;
							lightmaps.Add(lightmap);
							lightmapsDir.Add(lightmapDir);
							shadowMasks.Add(shadowMask);
						}

						rendererInfos.Add(info);
					}
				}
			}

			var lights = root.GetComponentsInChildren<Light>(true);

			foreach (Light l in lights)
			{
				LightInfo lightInfo = new LightInfo();
				lightInfo.light = l;
				lightInfo.lightmapBaketype = (int) l.lightmapBakeType;
#if UNITY_2020_1_OR_NEWER
				lightInfo.mixedLightingMode = (int) UnityEditor.Lightmapping.lightingSettings.mixedBakeMode;
#elif UNITY_2018_1_OR_NEWER
            lightInfo.mixedLightingMode = (int)UnityEditor.LightmapEditorSettings.mixedBakeMode;
#else
            lightInfo.mixedLightingMode = (int)l.bakingOutput.lightmapBakeType;
#endif
				lightsInfo.Add(lightInfo);
			}
		}
#endif
		#endregion
	}
}