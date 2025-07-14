using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RenderFeature
{
    public class CustomGrabRenderFeature : UWRenderFeature<CustomGrabRenderFeature>
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
        CustomGrabRenderPass m_CustomGrabRenderPass;
        public string GrabTexName = "_CustomGrabTex";
        public ResolutionDownSample DownSample = ResolutionDownSample.None;
        private int switch_GP = 0;
        //private int downSmapleCount = 0;
        // public static CustomGrabRenderFeature Instance;

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
        public bool useRenderObj= true;
        [SerializeField]
        public CustomUseGrabSettings m_CustomUseGrabSettings = new CustomUseGrabSettings();
        CustomUseGrabRenderPass m_CustomUseGrabRenderPass;
        public LayerMask layerMask = -1;
        public string DefaultShaderLightMode = "CustomGrabPass";

//------------------------------------------------------------------   
        public override void Create()
        {
	        base.Create();
	        m_CustomGrabRenderPass = new CustomGrabRenderPass(m_CustomGrabSettings.renderTiming, GrabTexName);
            
            ShaderTagId[] forwardOnlyShaderTagIds = new ShaderTagId[] {new ShaderTagId(DefaultShaderLightMode) };
            RenderQueueRange queueRange;
            if (m_CustomUseGrabSettings.queue == RenderQueue.opaque)
            {
                 queueRange = RenderQueueRange.opaque;
            }
            else if (m_CustomUseGrabSettings.queue == RenderQueue.transparent)
            {
                 queueRange = RenderQueueRange.transparent;
            }
            else
            {
                 queueRange = RenderQueueRange.all;
            }
            m_CustomUseGrabRenderPass = new CustomUseGrabRenderPass(m_CustomUseGrabSettings.renderTiming, forwardOnlyShaderTagIds, queueRange, layerMask);
        }
        public override void UWAddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (switch_GP <= 0) return;
            if (useGetGrabTex) renderer.EnqueuePass(m_CustomGrabRenderPass);
            if (useRenderObj) renderer.EnqueuePass(m_CustomUseGrabRenderPass);
        }
        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) 
        {
            if (renderingData.cameraData.cameraType == CameraType.Game) 
            {
                m_CustomGrabRenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
                m_CustomGrabRenderPass.SetUp(switch_GP, DownSample);
            }
            //Debug.Log(GrabTexName+"的switch_GP值：" + switch_GP );
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void InitializeGrabTexIndex()
        {
            switch_GP = 0;
        }
        public void AddGrabIndex()
        {
            switch_GP++;
        }
        public void RemoveGrabIndex()
        {
            switch_GP--;
            if (switch_GP <= 0)
            {
                switch_GP = 0;
                //downSmapleCount = 0;
            }
        }
        //public void AddDownSmapleGrabTexIndex()
        //{
        //    downSmapleCount++;
        //}
        //public void RemoveDownSmapleGrabTexIndex()
        //{
        //    downSmapleCount--;
        //    if (downSmapleCount <= 0)
        //    {
        //        downSmapleCount = 0;
        //    }
        //}
    }
}