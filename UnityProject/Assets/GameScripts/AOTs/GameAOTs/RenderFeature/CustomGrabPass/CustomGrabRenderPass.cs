using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{
    class CustomGrabRenderPass : ScriptableRenderPass
    {
        private readonly ProfilingSampler m_ProfilingSampler = new("CustomGrabSetScreenColor");
        
        private RenderTargetHandle m_SourceTex;
        private readonly int m_GrabTexPropertyId;
        
        private int m_switch_GP;
        private ResolutionDownSample m_downSmapleCount;
        //private int downSample_width;
        //private int downSample_height;
        
        public CustomGrabRenderPass(RenderPassEvent renderTiming, string rtName)
        {
	        renderPassEvent = renderTiming;
	        m_GrabTexPropertyId = Shader.PropertyToID(rtName);
	        m_SourceTex.Init(rtName);
        }
        public void SetUp(int switch_GP, ResolutionDownSample DownSample)
        {
            // this.m_SourceTex = src;
            this.m_switch_GP = switch_GP;
            this.m_downSmapleCount = DownSample;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get();
            if (m_switch_GP <= 0)
            {
                m_downSmapleCount = ResolutionDownSample.None;
                ExecuteCommand(context, cmd);
                return;
            }

            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
	            RenderTextureDescriptor m_OpaqueDesc = renderingData.cameraData.cameraTargetDescriptor;//获取相机参数
	            m_OpaqueDesc.depthBufferBits = 0;// 设置深度缓冲区
	            m_OpaqueDesc.width = (int)(m_OpaqueDesc.width * UWRenderFeatureHelper.GetResolutionScaleValue(m_downSmapleCount) );
	            m_OpaqueDesc.height = (int)(m_OpaqueDesc.height * UWRenderFeatureHelper.GetResolutionScaleValue(m_downSmapleCount));
	        
	            cmd.GetTemporaryRT(m_SourceTex.id, m_OpaqueDesc, FilterMode.Bilinear);
	            cmd.SetGlobalTexture(m_GrabTexPropertyId, m_SourceTex.Identifier());
	            
                var cameraColorTexture = renderingData.cameraData.renderer.cameraColorTargetHandle;
                cmd.Blit(cameraColorTexture, m_SourceTex.Identifier());
            }
            ExecuteCommand(context, cmd);// 执行命令缓冲区
            CommandBufferPool.Release(cmd);// 释放命令缓存
        }
        public override void FrameCleanup(CommandBuffer cmd)
        {
	        cmd.ReleaseTemporaryRT(m_SourceTex.id);
        }
//-------------------------------------------------------------------------------------
        void ExecuteCommand(ScriptableRenderContext context, CommandBuffer cmd)
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
    }
}       
