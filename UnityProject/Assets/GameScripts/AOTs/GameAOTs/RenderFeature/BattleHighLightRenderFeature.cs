using System.Collections.Generic;

using RenderFeature;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{
	public class BattleHighLightRenderFeature : UWRenderFeature<BattleHighLightRenderFeature>
	{
		[System.Serializable]
		public class CustomRenderPassFeatureSettings
		{
			public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;


			public Material mat = null;
		}

		public CustomRenderPassFeatureSettings settings = new CustomRenderPassFeatureSettings();
		private List<Renderer> renderers = new List<Renderer>();


		public void AddRenderData(Renderer renderer)
		{
			renderers.Add(renderer);
		}

		public void RemoveRenderData(Renderer renderer)
		{
			if (renderers.Contains(renderer))
				renderers.Remove(renderer);
		}


		CustomRenderPass _scriptablePass;

		public override void Create()
		{
			base.Create();
			//if (settings.renderers.Count == 0)
			//    SetActive(false);
			//else


			//    SetActive(true);

			_scriptablePass = new CustomRenderPass(settings.mat);
			//_scriptablePass.rendererList = settings.renderers;
			_scriptablePass.renderPassEvent = settings.renderPassEvent;
		}

		public override void UWAddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			_scriptablePass.rendererList = renderers;
			_scriptablePass.Setup(renderer.cameraColorTarget);

			renderer.EnqueuePass(_scriptablePass);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		class CustomRenderPass : ScriptableRenderPass
		{
			public List<Renderer> rendererList;
			private Material _material;
			readonly RenderTargetHandle _testTargetHandle;
			private RenderTargetIdentifier _source;
			private string cmdName = "BattleMask";

			//   AddRenderPasses >    Setup >     Configure      >Execute
			public CustomRenderPass(Material material)
			{
				_material = material;

				_testTargetHandle.Init("_TestTexture");
			}

			public void Setup(RenderTargetIdentifier source)
			{
				_source = source;
				rendererList = BattleHighLightRenderFeature.Instance.renderers;
			}

			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				ConfigureTarget(_source);
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				var cmd = CommandBufferPool.Get(cmdName);

				for (int index = 0; index < rendererList.Count; index++)
				{
					cmd.DrawRenderer(rendererList[index], rendererList[index].material);
				}


				context.ExecuteCommandBuffer(cmd);
				cmd.Clear();
				CommandBufferPool.Release(cmd);
			}

			public override void FrameCleanup(CommandBuffer cmd)
			{
				// cmd.ReleaseTemporaryRT(_testTargetHandle.id);
			}
		}
	}
}