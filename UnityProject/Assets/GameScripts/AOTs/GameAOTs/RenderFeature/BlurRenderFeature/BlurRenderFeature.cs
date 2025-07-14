// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-09-11 20:30 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System.Collections.Generic;
using BlurEffect;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{
	public class BlurRenderFeature : UWRenderFeature<BlurRenderFeature>
	{
		[Header("时机")] public RenderPassEvent renderTiming = RenderPassEvent.AfterRenderingTransparents;

		[SerializeField] public BlurEffectParam _blurEffectParam = new BlurEffectParam();

		const string BLUR_SHADER_NAME = "Unlit/BlurEffect";
		public Shader _blurShader;
		private static Material _blurMat;

		private BlurRenderPass _blurRenderPass;

		private List<Camera> _effectCameras;

		public override void Create()
		{
			base.Create();
			_blurRenderPass = new BlurRenderPass(name);

			if (_blurShader == null)
				return;

			if (_blurMat == null)
				_blurMat = new Material(_blurShader);

			_blurRenderPass.renderPassEvent = renderTiming;
			_blurRenderPass.blurEffectParam = _blurEffectParam;
			_blurRenderPass.blurMaterial = _blurMat;

			if (_effectCameras == null)
				_effectCameras = new List<Camera>();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		protected override bool CheckTargetCamera(RenderingData renderingData)
		{
			return true;
		}

		public override void UWAddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (_effectCameras != null && !_effectCameras.Contains(renderingData.cameraData.camera)) return;
			if (_blurMat == null) return;
			renderer.EnqueuePass(_blurRenderPass);
		}

		public void AddRef(Camera camera)
		{
			if (!_effectCameras.Contains(camera))
				_effectCameras.Add(camera);
		}

		public void RemoveRef(Camera camera)
		{
			if (_effectCameras.Contains(camera))
				_effectCameras.Remove(camera);
		}

		class BlurRenderPass : ScriptableRenderPass
		{
			string S_ProfilerTag = "BlurRenderPass";
			public Material blurMaterial;
			public BlurEffectParam blurEffectParam;

			private RenderTargetHandle m_Buffer0; //缓冲区1
			private RenderTargetHandle m_Buffer1; //缓冲区2

			private RenderTargetIdentifier m_Source; //屏幕图

			public BlurRenderPass(string name)
			{
				S_ProfilerTag = name;

				m_Buffer0.Init("_Buffer0");
				m_Buffer1.Init("_Buffer1");
			}

			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				RenderTextureDescriptor descriptor = cameraTextureDescriptor;
				descriptor.depthBufferBits = 0;
				descriptor.width = (int) (descriptor.width / blurEffectParam.blur_down_sample);
				descriptor.height = (int) (descriptor.height / blurEffectParam.blur_down_sample);

				cmd.GetTemporaryRT(m_Buffer0.id, descriptor, FilterMode.Bilinear);
				cmd.GetTemporaryRT(m_Buffer1.id, descriptor, FilterMode.Bilinear);
			}

			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_Buffer0.id);
				cmd.ReleaseTemporaryRT(m_Buffer1.id);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				var cameraColorTexture = renderingData.cameraData.renderer.cameraColorTarget;
				CommandBuffer cmd = CommandBufferPool.Get(S_ProfilerTag);

				cmd.Blit(cameraColorTexture, m_Buffer0.Identifier());

				blurMaterial.SetFloat("_Dark", blurEffectParam.color_to_dark);
				blurMaterial.SetFloat("_Saturate", blurEffectParam.color_saturate);
				for (int i = 0; i < blurEffectParam.blur_iteration; i++)
				{
					blurMaterial.SetFloat("_BlurSize",
						(1.0f + i * blurEffectParam.blur_spread) * blurEffectParam.blur_size); // 设置模糊扩散uv偏移

					cmd.Blit(m_Buffer0.Identifier(), m_Buffer1.Identifier(), blurMaterial, 0);
					cmd.Blit(m_Buffer1.Identifier(), m_Buffer0.Identifier(), blurMaterial, 1);
				}

				cmd.Blit(m_Buffer0.Identifier(), cameraColorTexture); //将最终结果渲染到摄像机上
				context.ExecuteCommandBuffer(cmd);
				cmd.Clear();

				CommandBufferPool.Release(cmd);
			}
		}
	}
}