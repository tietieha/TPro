using System;
using System.Collections.Generic;
using UnityEngine;
using BigWorldRender;

public class WorldTestRoot : MonoBehaviour
{
    //data
    public Vector3 mapOffset;
    public bool loopX;

    public ResRenderData resRenderData;
    public Camera mainCamera;

    public int testLod;

    private BigWorldRenderManager _bigWorldRenderManager;
    private bool isInit = false;

    void Start()
    {
        if (resRenderData != null)
        {
            Init(resRenderData, mapOffset, LoadWorldRenderDataFinishCallBack);
        }
    }
    
    void Update()
    {
        if (isInit)
        {
            // JobBigWorldRenderLogicManager.Instance.UpdateLogic();
            // JobBigWorldRenderLogicManager.Instance.SetRenderLod(testLod);
            
            _bigWorldRenderManager.Update(Time.deltaTime * 1000f);
            _bigWorldRenderManager.SetRenderLod(testLod);
        }
    }

    private void OnDisable()
    {
        UnInit();
    }

    public void Init(ResRenderData data, Vector3 offset, Action callback = null)
    {
        if (_bigWorldRenderManager == null)
        {
            _bigWorldRenderManager = new BigWorldRenderManager();
            _bigWorldRenderManager.Init();
        }
        else
        {
            _bigWorldRenderManager.Release();
        }

        // JobBigWorldRenderLogicManager.Instance.InitData(data, mapOffset, callback);
        // JobBigWorldRenderLogicManager.Instance.SetWorldCamera(mainCamera);
        // JobBigWorldRenderLogicManager.Instance.SetWorldLoop(loopX);
        // mainCamera.depthTextureMode |= DepthTextureMode.Depth;
        
        _bigWorldRenderManager.InitData(data, mapOffset, callback);
        _bigWorldRenderManager.SetWorldCamera(mainCamera);
        _bigWorldRenderManager.SetWorldLoop(loopX);
        isInit = true;
    }

    public void UnInit()
    {
        // JobBigWorldRenderLogicManager.Instance.ExitLogic();
        // JobBigWorldRenderLogicManager.Instance.DestructWorldCameraLogic();
        if (_bigWorldRenderManager != null)
            _bigWorldRenderManager.Release();
        isInit = false;
    }

    public float GetAnalyzeProgress()
    {
        // return JobBigWorldRenderLogicManager.Instance.GetStaticRenderDataLoadProgress();
        return 0;
    }
    
    private void LoadWorldRenderDataFinishCallBack()
    {
        Debug.Log("加载完成");
    }

    // private void OnDrawGizmos()
    // {
       //  if (Application.isPlaying)
          //   JobBigWorldRenderLogicManager.Instance.DrawRegionData();
    //
    // }

    //DrawRegionData
}