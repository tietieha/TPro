using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HeroRenderer
{
    public class HDistortRendererFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class HDistortSetting
        {
            public RenderPassEvent passEvent = RenderPassEvent.AfterRenderingTransparents;
            public LayerMask layerMask = ~0;
            public string shaderTag = "HDistortPass";
            public int downSample = 2;
            public RenderTextureFormat rtFormat = RenderTextureFormat.ARGB2101010;
            public FilterMode filterMode = FilterMode.Point;
        }

        public HDistortSetting m_setting = new HDistortSetting();
        private HDistortRenderPass m_pass;

        public override void Create()
        {
            m_pass = new HDistortRenderPass(m_setting);
            m_pass.renderPassEvent = m_setting.passEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {

             if (renderingData.cameraData.cameraViewType != UniversalAdditionalCameraData.CameraViewType.SCENE &&
                 renderingData.cameraData.cameraViewType != UniversalAdditionalCameraData.CameraViewType.GUICAMERA)
             {
                 Shader.DisableKeyword("H_DEBUG_DISTORT_ON");
                 return;
             }

             if (renderingData.cameraData.isSceneViewCamera)
             {
                 return;
             }

             Shader.EnableKeyword("H_DEBUG_DISTORT_ON");
             renderer.EnqueuePass(m_pass);
        }
    }

    public class HDistortRenderPass : ScriptableRenderPass
    {
        private FilteringSettings m_filter;
        private ShaderTagId m_idTag;
        private HDistortRendererFeature.HDistortSetting m_setting;
        private RenderTargetHandle m_destination;
        
        public HDistortRenderPass(HDistortRendererFeature.HDistortSetting setting)
        {
            var queueRange =  RenderQueueRange.transparent;
            m_filter = new FilteringSettings(queueRange, setting.layerMask);
            m_idTag = new ShaderTagId(setting.shaderTag);
            m_setting = setting;
            m_destination.Init("_HDistortMask");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 0;
            descriptor.colorFormat = m_setting.rtFormat;
            descriptor.width /= m_setting.downSample;
            descriptor.height /= m_setting.downSample;
            
            cmd.GetTemporaryRT(m_destination.id, descriptor, m_setting.filterMode);

            
            ConfigureTarget(new RenderTargetIdentifier(m_destination.Identifier(), 0, CubemapFace.Unknown, -1));
            ConfigureClear(ClearFlag.All, new Color(0,0,0,0));
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get();
            
            var distortMaskDrawSetting = CreateDrawingSettings(m_idTag, ref renderingData, SortingCriteria.BackToFront);
            distortMaskDrawSetting.perObjectData = PerObjectData.None;
                
            context.DrawRenderers(renderingData.cullResults, ref distortMaskDrawSetting, ref m_filter);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(m_destination.id);
        }
    }
}
