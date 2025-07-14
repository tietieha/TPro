// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-06-26 14:27 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{

	
	/// <summary>
	/// UW 所有RendererFeature 都应该继承此类
	/// 
	/// </summary>
	public class UWRenderFeature<T> : ScriptableRendererFeature where T : UWRenderFeature<T>
	{
		static Dictionary<Camera, string> S_CameraNameCache = new Dictionary<Camera, string>();
		
		[System.Serializable]
		public class RenderFeatureCameraSettings
		{
			[Header("反向选中")] public bool IsReverse;
			[Header("目标相机名")] public String[] TargetCamerasName = new[] {"MainCamera"};
		}

		#region singleton
		protected static T m_Instance;
		public static T Instance
		{
			get
			{
				return m_Instance;
			}
		}
		#endregion

		[Header("相机筛选")]
		public RenderFeatureCameraSettings CameraSettings;
		
		public override void Create()
		{
			m_Instance = (T) this;
		}
		
		[Tooltip("此方法在UW弃用，请使用 UWAddRenderPasses ")]
		public sealed override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!CheckTargetCamera(renderingData))
				return;
			
			UWAddRenderPasses(renderer, ref renderingData);
		}
		
		public virtual void UWAddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			
		}

		protected virtual bool CheckTargetCamera(RenderingData renderingData)
		{
			if (CameraSettings.TargetCamerasName.Length == 0)
				return true;

			if (!S_CameraNameCache.TryGetValue(renderingData.cameraData.camera, out string cameraName))
			{
				cameraName = renderingData.cameraData.camera.name;
				S_CameraNameCache.Add(renderingData.cameraData.camera, cameraName);
			}
			bool succ = CameraSettings.TargetCamerasName.Contains(cameraName);

			if (CameraSettings.IsReverse)
			{
				return !succ;
			}
			
			return succ;
		}
		

	}
}