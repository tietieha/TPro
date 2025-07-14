using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BigWorldRender;
using TEngine;
using UnityEngine;
using UW;
using XLua;

namespace World
{
	[LuaCallCSharp]
	public class BigWorldRender : MonoBehaviour
	{
		public bool isDebug;
		public Vector3 mapOffset;
		public ResRenderData resRenderData;
		public Camera mainCamera;

		public int testLod;

		[SerializeField] private List<string> _shadowCasterPassShaderControl;
		[SerializeField] private bool _shadowCasterEnable = true;

		private BigWorldRenderManager _bigWorldRenderManager;
		private bool isInit = false;

		void Awake()
		{
			if (_bigWorldRenderManager == null)
			{
				_bigWorldRenderManager = new BigWorldRenderManager();
				_bigWorldRenderManager.Init();
			}

			if (resRenderData)
			{
				Init(resRenderData, Vector3.zero, LoadWorldRenderDataFinishCallBack);
			}
		}

		private void OnEnable()
		{
		}

		void Update()
		{
			if (_isLoadComplete)
			{
				_isLoadComplete = false;
				_callBak?.Invoke();
				_callBak = null;
			}
			if (isInit)
			{
				_bigWorldRenderManager.Update(Time.deltaTime * 1000f);
				_bigWorldRenderManager.SetRenderLod(testLod);
#if UNITY_EDITOR
				_bigWorldRenderManager.SetDebug(isDebug);
#endif
			}
		}
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (isInit)
			{
				_bigWorldRenderManager.DrawGizmos();
			}
		}
#endif

		private void OnDisable()
		{
		}

		public void LoadMap(ResRenderData data, Action callback = null)
		{
			Init(data, mapOffset, callback);
		}

		public void Init(ResRenderData data, Vector3 offset, Action callback = null)
		{
			var time = Time.realtimeSinceStartup;
			_bigWorldRenderManager.InitData(data, offset, ()=>
			{
				Log.Info($"map render init complete use time {Time.realtimeSinceStartup - time}s");
				if (callback != null)
					callback();
			});
			_bigWorldRenderManager.SetWorldCamera(mainCamera);
			_bigWorldRenderManager.ConfigMaterialPassEnable(_shadowCasterPassShaderControl, "ShadowCaster",
				_shadowCasterEnable);
			// mainCamera.depthTextureMode |= DepthTextureMode.Depth;
			isInit = true;
		}

		public void SetWorldLoop(bool b)
		{
			_bigWorldRenderManager.SetWorldLoop(b);
		}

		public void SetShadowCasterPassEnable(bool enable)
		{
			if (_shadowCasterEnable != enable)
			{
				_shadowCasterEnable = enable;
				_bigWorldRenderManager.ConfigMaterialPassEnable(_shadowCasterPassShaderControl, "ShadowCaster",
					_shadowCasterEnable);
			}
		}

		public void SwitchShadowCasterPassEnable()
		{
			_shadowCasterEnable = !_shadowCasterEnable;
			_bigWorldRenderManager.ConfigMaterialPassEnable(_shadowCasterPassShaderControl, "ShadowCaster",
				_shadowCasterEnable);
		}

		public void UnInit()
		{
			_bigWorldRenderManager.Release();
			isInit = false;
		}
		//
		// public float GetAnalyzeProgress()
		// {
		// 	return _bigWorldRenderManager.GetStaticRenderDataLoadProgress();
		// }

		private void OnApplicationQuit()
		{
			UnInit();
		}

		private void LoadWorldRenderDataFinishCallBack()
		{
			Debug.Log("加载完成");
		}

		public WorldZoneMapData MapData;

		private Action _callBak;
		private bool _isLoadComplete = false;
		
		private void OnValidate()
		{
			if (isInit)
				_bigWorldRenderManager.ConfigMaterialPassEnable(_shadowCasterPassShaderControl, "ShadowCaster",
					_shadowCasterEnable);
		}

		public void LoadMapData(TextAsset textAsset, Action callBack)
		{
			_isLoadComplete = false;
			_callBak = callBack;
			if (textAsset != null)
			{
				var bytes = textAsset.bytes;
				using (var memStream = new MemoryStream(bytes))
				{
					var br = new BinaryReader(memStream);
					MapData = new WorldZoneMapData();
					MapData.Load(br);
					br.Close();

					WorldModuleZone.Instance.InitData(MapData);
				}
				_isLoadComplete = true;
				callBack?.Invoke();
			}
		}

		public void LoadMapData(string filePath, Action callBak)
		{
			_isLoadComplete = false;
			_callBak = callBak;
			var time = Time.realtimeSinceStartup;
			GameModule.Resource.LoadAsset<TextAsset>(filePath, (TextAsset textAsset) =>
			{
				if (textAsset != null)
				{
					var bytes = textAsset.bytes;
					using (var memStream = new MemoryStream(bytes))
					{
						var br = new BinaryReader(memStream);
						MapData = new WorldZoneMapData();
						MapData.Load(br);
						br.Close();

						WorldModuleZone.Instance.InitData(MapData);
					}
					_isLoadComplete = true;
					callBak?.Invoke();
				}
				Log.Info($"map data init complete use time {Time.realtimeSinceStartup-time}s");
			});
		}
	}
}