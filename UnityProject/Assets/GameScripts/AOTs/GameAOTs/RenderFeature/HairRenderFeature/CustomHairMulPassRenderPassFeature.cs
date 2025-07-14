using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomHairMulPassRenderPassFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSetting
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;
        public LayerMask LayerMask = ~0;
        public string HairAlphaTestPass = "HairAlphaTesPass";
        public string HairBlendPass = "HairTransparentPass";
        public string HairBlendPass1 = "HairTransparentPass1";
    }

    public CustomPassSetting setting = new CustomPassSetting();
    CustomRenderPass m_ScriptablePass_Depth;
    CustomRenderPass m_ScriptablePass_Transparent;

    public override void Create()
    {
        m_ScriptablePass_Depth = new CustomRenderPass("HairAlphaTestPass",1,RenderQueue.AlphaTest,RenderQueue.Transparent);
        m_ScriptablePass_Transparent = new CustomRenderPass("HairTransparentPass",2,RenderQueue.AlphaTest,RenderQueue.Transparent);
        m_ScriptablePass_Depth.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        m_ScriptablePass_Transparent.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass_Depth);
        renderer.EnqueuePass(m_ScriptablePass_Transparent);
        // renderer.EnqueuePass(m_ScriptablePass_Transparent);
    }


    class CustomRenderPass : ScriptableRenderPass
    {
        private FilteringSettings m_filter;
        private List<ShaderTagId> m_idTagList = new List<ShaderTagId>();
        private LayerMask m_layerMask = ~0;
        private string m_cmdTag0 = "HairCMD";
        private int m_drawCount;

        // public CustomRenderPass(CustomPassSetting setting)
        // {
        //     RenderQueueRange queueRange =
        //         new RenderQueueRange((int)RenderQueue.AlphaTest, (int)RenderQueue.Transparent);
        //     m_layerMask = setting.LayerMask;
        //     m_filter = new FilteringSettings(queueRange, m_layerMask);
        //
        //     m_idTagList.Add(new ShaderTagId(setting.HairAlphaTestPass));
        //     m_idTagList.Add(new ShaderTagId(setting.HairBlendPass));
        //     m_idTagList.Add(new ShaderTagId(setting.HairBlendPass1));
        // }
        public CustomRenderPass(string passName,int passCount, RenderQueue lower,RenderQueue upper)
        {
            RenderQueueRange queueRange =
                new RenderQueueRange((int)lower, (int)upper);
            // m_layerMask = setting.LayerMask;
            m_filter = new FilteringSettings(queueRange, m_layerMask);

            m_idTagList.Add(new ShaderTagId(passName));

            m_drawCount = passCount;
            // m_idTagList.Add(new ShaderTagId(setting.HairBlendPass));
            // m_idTagList.Add(new ShaderTagId(setting.HairBlendPass1));
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_cmdTag0);
            DrawingSettings hairDrawSetting1 = CreateDrawingSettings(m_idTagList[0], ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            // DrawingSettings hairDrawSetting2 = CreateDrawingSettings(m_idTagList[1], ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            // DrawingSettings hairDrawSetting3 = CreateDrawingSettings(m_idTagList[2], ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            cmd.Clear();
            context.ExecuteCommandBuffer(cmd);
            
            for(int i = 0;i < m_drawCount; i++)
            {
                context.DrawRenderers(renderingData.cullResults, ref hairDrawSetting1, ref m_filter);
            }
            // context.DrawRenderers(renderingData.cullResults, ref hairDrawSetting2, ref m_filter);
            // context.DrawRenderers(renderingData.cullResults, ref hairDrawSetting3, ref m_filter);
            CommandBufferPool.Release(cmd);
        }


        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }
}
