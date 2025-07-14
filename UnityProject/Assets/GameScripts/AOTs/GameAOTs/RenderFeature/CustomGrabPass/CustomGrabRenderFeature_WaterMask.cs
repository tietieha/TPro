using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{
    public class CustomGrabRenderFeature_WaterMask : UWRenderFeature<CustomGrabRenderFeature_WaterMask>
    {
        //-------------------------1------------------------------------
        //获取不同渲染时机的相机画面
        [Space()]
        [Header("获取相机画面pass")]
        public bool useGetGrabTex = true;
        [Serializable]
        public class CustomGrabSettings
        {
            public RenderPassEvent renderTiming = RenderPassEvent.AfterRenderingTransparents;
        }
        [SerializeField]
        public CustomGrabSettings m_CustomGrabSettings = new CustomGrabSettings();
        CustomGrabRenderPass m_CustomGrabRenderPassWaterMask;
        public string GrabTexName = "_CustomMaskGrabTex";
        public ResolutionDownSample DownSample = ResolutionDownSample.None;
        private int switch_GP = 0;
        public bool m_UseCopy = true;
        //--------------------------2-----------------------------------
        //为了能设置单个物体的渲染，不让吧全屏截图混到靠前的序列里 ，显示会混乱
        public enum RenderQueue
        {
            all,
            opaque,
            transparent,
        };
        [Serializable]
        public class CustomUseGrabSettings
        {
            public RenderPassEvent renderTiming = RenderPassEvent.BeforeRenderingPostProcessing;
            public RenderQueue queue = RenderQueue.transparent;
        }
        [Header("渲染对象pass")]
        public bool useRenderObj = true;
        [SerializeField]
        public CustomUseGrabSettings m_CustomUseGrabSettingsPassWaterMask = new CustomUseGrabSettings();

        CustomUseGrabRenderPass m_CustomUseGrabRenderPassWaterMask;
        public LayerMask layerMask = -1;
        public string DefaultShaderLightMode = "UnderWater";

//------------------------------------------------------------------   
        public override void Create()
        {
            base.Create();
	        m_CustomGrabRenderPassWaterMask = new CustomGrabRenderPass(m_CustomGrabSettings.renderTiming, GrabTexName);
            
            ShaderTagId[] forwardOnlyShaderTagIds = new ShaderTagId[] {new ShaderTagId(DefaultShaderLightMode) };
            RenderQueueRange queueRange;
            if (m_CustomUseGrabSettingsPassWaterMask.queue == RenderQueue.opaque)
            {
                 queueRange = RenderQueueRange.opaque;
            }
            else if (m_CustomUseGrabSettingsPassWaterMask.queue == RenderQueue.transparent)
            {
                 queueRange = RenderQueueRange.transparent;
            }
            else
            {
                 queueRange = RenderQueueRange.all;
            }
          //  m_CustomUseGrabRenderPassWaterMask = new CustomUseGrabRenderPass(m_CustomUseGrabSettingsPassWaterMask.renderTiming, forwardOnlyShaderTagIds, queueRange, layerMask);
        }
        public override void UWAddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!useGetGrabTex) return;
            if (useGetGrabTex) renderer.EnqueuePass(m_CustomGrabRenderPassWaterMask);
           // if (useRenderObj) renderer.EnqueuePass(m_CustomUseGrabRenderPassWaterMask);
        }
        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {

            InitializeSwitch_GP();//直接本体判断 条件2是否成立
            if (renderingData.cameraData.cameraType == CameraType.Game) 
            {
                if (m_UseCopy)
                {
                    m_CustomGrabRenderPassWaterMask.ConfigureInput(ScriptableRenderPassInput.Color);
                }
                m_CustomGrabRenderPassWaterMask.SetUp(switch_GP, DownSample);
            }
            //Debug.Log(GrabTexName+"的switch_GP值：" + switch_GP );
        }
        protected override void Dispose(bool disposing)
        {
     
            base.Dispose(disposing);
        }

        public void InitializeSwitch_GP()
        {
            if (useGetGrabTex)
            {
                switch_GP = 1;
            }
            else switch_GP = 0;
        }

    }
}