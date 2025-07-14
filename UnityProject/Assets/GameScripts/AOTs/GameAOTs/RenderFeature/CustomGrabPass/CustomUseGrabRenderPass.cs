using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{
    class CustomUseGrabRenderPass : ScriptableRenderPass
    {
        FilteringSettings m_FilteringSettings;//层级筛选数据
        RenderStateBlock m_RenderStateBlock;//模板测试数据
        List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();
        RenderTargetIdentifier m_Source;
        string m_ProfilerTag = "CustomGrabRenderObject";//FrameDebug中显示
        ProfilingSampler m_ProfilingSampler;

        public CustomUseGrabRenderPass(RenderPassEvent renderTiming, ShaderTagId[] shaderTagIds, 
        RenderQueueRange renderQueueRange, LayerMask layerMask)
        {
            renderPassEvent = renderTiming;
            base.profilingSampler = new ProfilingSampler(nameof(CustomUseGrabRenderPass));
            m_ProfilingSampler = new ProfilingSampler(m_ProfilerTag);

            foreach (ShaderTagId sid in shaderTagIds)
                m_ShaderTagIdList.Add(sid);
            
            renderPassEvent = renderTiming;

            m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
        
            m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);

        }
        public void SetUp(RenderTargetIdentifier source)
        {
            m_Source = source;
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
                ExecuteCommand(context, cmd);
                SortingCriteria sortingCriteria = SortingCriteria.CommonTransparent;
                DrawingSettings drawSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
                
                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings, ref m_RenderStateBlock);
            }

            ExecuteCommand(context, cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
        }
//-------------------------------------------------------------------------------------
        void ExecuteCommand(ScriptableRenderContext context, CommandBuffer cmd)
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
    }
}