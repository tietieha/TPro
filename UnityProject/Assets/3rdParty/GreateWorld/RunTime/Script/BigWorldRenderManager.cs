// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-13 17:45 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace BigWorldRender
{
	/// <summary>
	/// 所有渲染工厂的运转
	/// </summary>
	public class BigWorldRenderManager// : Singleton<BigWorldRenderManager>
	{
		public static class DebugConf
		{
#if GAME_DEBUG
			public static bool IsLog = true;
#else
           public static bool IsLog = false;
#endif
			
			public static bool IsDebugGizmos;
		}
		
		public static class Consts
		{
			// 羊皮卷LOD
			public static int TOP_RENDER_LOD = 5;
		}

		// 裁剪timer
		public float CULLING_FRAME_TIME = 33.3f;
		private float _cullingTimer = 0f;

		// 渲染工厂
		private Dictionary<int, RenderFactoryBase> _renderFactories =
			new Dictionary<int, RenderFactoryBase>(RenderFactoryType.COUNT);

		// culling 优化
		private Camera _currentCamera;
		private Vector3 _lastCameraPos;
		private Transform _cameraTrans;
		
		// render lod
		private int _renderLOD;

		private bool _isInit;

		private BigWorldRenderData _bigWorldRenderData;

		public void Init()
		{
			if (BigWorldRenderManager.DebugConf.IsLog)
				Log.Info($"[BigWorldRender] Manager - Init");
			_bigWorldRenderData = new BigWorldRenderData();
			_cullingTimer = 0f;
			// 添加渲染工厂
			_renderFactories.Clear();
			_renderFactories.Add(RenderFactoryType.STATIC, new RenderFactoryStatic(_bigWorldRenderData));
			_renderFactories.Add(RenderFactoryType.GLOBAL, new RenderFactoryGlobal(_bigWorldRenderData));
		}

		public void InitData(ResRenderData resRenderData, Vector3 offset, Action callback = null)
		{
			if (BigWorldRenderManager.DebugConf.IsLog)
				Debug.Log($"[BigWorldRender] Manager - InitData");
			if (resRenderData == null)
			{
				Debug.LogError("[BigWorldRender] resRenderData == null");
				return;
			}
			
			_bigWorldRenderData.Init(resRenderData, offset, callback);

			// 初始化
			foreach (var factory in _renderFactories.Values)
			{
				factory.Init();
			}

			_isInit = true;
		}

		public void Update(float delta)
		{
			if (!_isInit)
				return;
			
			if (_currentCamera == null)
			{
				Debug.LogError("currentCamera == null");
				return;
			}
			
			foreach (var factory in _renderFactories.Values)
			{
				factory.Update();
			}
			
			// 裁剪
			_cullingTimer += delta;
			while (_cullingTimer >= CULLING_FRAME_TIME)
			{
				if (IsNeedCulling())
				{
					// 更新相机裁切面
					_bigWorldRenderData.SetFrustumPlanes(_currentCamera.cullingMatrix);
					
					foreach (var factory in _renderFactories.Values)
					{
						if (factory.NeedRender())
							factory.Cull();
					}
				}
			
				_cullingTimer -= CULLING_FRAME_TIME;
			}

			// 渲染
			foreach (var factory in _renderFactories.Values)
			{
				if (factory.NeedRender())
					factory.Draw();
			}
		}

		public void Release()
		{
			if (BigWorldRenderManager.DebugConf.IsLog)
				Log.Info($"[BigWorldRender] Manager - Release");

			_isInit = false;
			_cullingTimer = 0f;
			
			_currentCamera = null;
			_lastCameraPos = Vector3.zero;
			_cameraTrans = null;

			if (_bigWorldRenderData != null)
			{
				_bigWorldRenderData.Exit();
			}
			
			foreach (var factory in _renderFactories.Values)
			{
				factory.Exit();
			}
		}

		private bool IsNeedCulling()
		{
			return _lastCameraPos != _cameraTrans.position;
		}
		
		/// <summary>
		/// 进入大世界设置相机
		/// </summary>
		/// <param name="mainCamera"></param>
		public void SetWorldCamera(Camera camera)
		{
			if (camera == null)
			{
				Debug.LogError("[BigWorldRender] camera == null");
				return;
			}
			_currentCamera = camera;
			_cameraTrans = _currentCamera.transform;
			_lastCameraPos = Vector3.zero;
			_bigWorldRenderData.SetWorldCamera(camera);
		}

		public void SetWorldLoop(bool loopX = true, bool loopY = true)
		{
			_bigWorldRenderData.SetWorldLoop(loopX, loopY);
		}
		
		/// <summary>
		/// 设置LOD
		/// </summary>
		/// <param name="renderLod"></param>
		public void SetRenderLod(int renderLod)
		{
			_bigWorldRenderData.currentRenderLod = renderLod;
		}

		public void SetDebug(bool isDebug)
		{
			DebugConf.IsDebugGizmos = isDebug;
		}

		public void ConfigMaterialPassEnable(List<string> filterShader, string pass, bool enable)
		{
			_bigWorldRenderData.ConfigMaterialPassEnable(filterShader, pass, enable);
		}

#if UNITY_EDITOR
		public void DrawGizmos()
		{
			if (!DebugConf.IsDebugGizmos)
				return;
			foreach (var factory in _renderFactories.Values)
			{
				factory.DrawGizmos();
			}
		}
#endif
	}
}