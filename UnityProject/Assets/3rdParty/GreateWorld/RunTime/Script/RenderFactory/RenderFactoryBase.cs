// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-13 17:52 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using TEngine;
using UnityEngine;

namespace BigWorldRender
{
	public static class RenderFactoryType
	{
		public static readonly int STATIC = 0; // 静态渲染
		public static readonly int GLOBAL = 1; // 全局渲染
		public static readonly int DYNAMIC = 2; // 动态渲染

		public static readonly int COUNT = 3; // count
	}

	/// <summary>
	/// 渲染工厂，比如 静态，动态，全局等
	/// </summary>
	public abstract class RenderFactoryBase
	{
		private const int RENDER_PROCESS_DEFAULT_COUNT = 200;

		protected bool _inited;
		protected int _renderResCount;
		protected int _renderDataCount;
		protected Dictionary<int, RenderProcessBase> _renderProcess; //key is asset id
		protected BigWorldRenderData _bigWorldRenderData;

		public RenderFactoryBase(BigWorldRenderData bigWorldRenderData)
		{
			_bigWorldRenderData = bigWorldRenderData;
		}

		public void Init()
		{
			if (BigWorldRenderManager.DebugConf.IsLog)
				Log.Info($"[BigWorldRender] {getType()} - Init");

			_renderResCount = 0;
			_renderDataCount = 0;
			ExitLogic();
			InitLogic();
			AfterInitLogic();

			_inited = true;
		}

		public void Update()
		{
			if (!_inited)
				return;
			UpdateLogic();
		}

		public void Cull()
		{
			if (!_inited)
				return;

			CullLogic();
		}

		public void Draw()
		{
			if (!_inited)
				return;
			DrawLogic();
			DrawRemainLogic();
		}

		public void Exit()
		{
			// if (!_inited)
			// 	return;
			if (BigWorldRenderManager.DebugConf.IsLog)
				Log.Info($"[BigWorldRender] {getType()} - Exit");
			ExitLogic();

			_inited = false;
			_renderDataCount = 0;
			_renderResCount = 0;
			if (_renderProcess != null)
			{
				foreach (var key in _renderProcess.Keys.ToList())
				{
					_renderProcess[key] = null;
				}

				_renderProcess.Clear();
			}
		}

		public void DrawGizmos()
		{
			if (!_inited)
				return;
			DrawGizmosLogic();
		}

		/// <summary>
		/// 初始化各工厂数据 容器
		/// </summary>
		protected abstract void InitLogic();

		/// <summary>
		/// 根据统计的资源数量，数据数量
		/// 创建各工厂流水线 single and instance render process
		/// </summary>
		protected virtual void AfterInitLogic()
		{
			if (_renderProcess == null)
				_renderProcess =
					new Dictionary<int, RenderProcessBase>(_renderResCount > 0
						? _renderResCount
						: RENDER_PROCESS_DEFAULT_COUNT);
		}

		protected abstract void CullLogic();

		protected virtual void UpdateLogic()
		{
		}

		protected abstract void DrawLogic();

		protected void DrawRemainLogic()
		{
			foreach (var process in _renderProcess.Values)
			{
				if (process.Dirty)
					process.DrawRemain();
			}
		}

		protected void Render(WorldStaticRenderStructData worldStaticRenderStructData, int layer = 0)
		{
			int assetid = worldStaticRenderStructData.assetId;
			if (!_renderProcess.TryGetValue(assetid, out RenderProcessBase process))
			{
				if (!_bigWorldRenderData.StaticRegionAssetDic.TryGetValue(assetid, out RegionAssetData renderAsset))
				{
					Debug.LogError("[BigWorldRender] staticRegionAssetDic not ContainsKey" + assetid);
					return;
				}

				if (renderAsset == null)
				{
					Debug.LogError("[BigWorldRender] renderAsset is null" + assetid);
					return;
				}

				process = RenderProcessHelper.GetRenderProcess(renderAsset.renderType);
				process.SetUpAsset(renderAsset);

				_renderProcess.Add(assetid, process);
			}

			process.Draw(worldStaticRenderStructData.GetMatrix(), layer);
		}

		protected abstract void ExitLogic();

		// debug bounds
		protected abstract void DrawGizmosLogic();

		protected abstract string getType();

		public virtual bool NeedRender()
		{
			return true;
		}
	}
}