
using System.Collections.Generic;
using GameLogic.GameFeatures.City;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

[CreateAssetMenu(fileName = "CityBuildAsset", menuName = "City/CityBuildAsset")]
public class CityBuildAsset : ScriptableObject
{
    public Vector3 CameraPosition;
    public Vector3 CameraEuler;
    public float CameraNearClip;
    public float CameraFarClip;
    public float CameraFieldOfView;
    public float CameraBoundaryMinX;
    public float CameraBoundaryMaxX;
    
    [System.Serializable]
    public class BuildInfo
    {
        public string BuildName;
        
        public float ContainerPosX;
        public float ContainerPosY;
        public float ContainerPosZ;
        
        public float ModelEulerX;
        public float ModelEulerY;
        public float ModelEulerZ;
        
        public float ModelScaleX;
        public float ModelScaleY;
        public float ModelScaleZ;

        public float HudPosX;
        public float HudPosY;
        public float HudPosZ;
        
        public float HudScaleX;
        public float HudScaleY;
        public float HudScaleZ;
        
        public float HudRotateX;
        public float HudRotateY;
        public float HudRotateZ;
        
        public float MenuTagPosX;
        public float MenuTagPosY;
        public float MenuTagPosZ;

        public float FocusPosX;
        public float FocusPosY;
        public float FocusPosZ;

        public float FocusRotateX;
        public float FocusRotateY;
        public float FocusRotateZ;
        
        public float MenuFocusPosX;
        public float MenuFocusPosY;
        public float MenuFocusPosZ;

        public float MenuFocusRotateX;
        public float MenuFocusRotateY;
        public float MenuFocusRotateZ;

        public DepthOfFieldData depthOfFieldData;

        public void SetDepthData(DepthOfFieldDataMono dataMono)
        {
            if (depthOfFieldData == null)
            {
                depthOfFieldData = new DepthOfFieldData();
            }
            depthOfFieldData.FocusDistance = dataMono.FocusDistance;
            depthOfFieldData.FocalLength = dataMono.FocalLength;
            depthOfFieldData.Aperture = dataMono.Aperture;
        }
    }
    
    [System.Serializable]
    public class DepthOfFieldData
    {
        public float FocusDistance = 0;
        public float FocalLength = 0;
        public float Aperture = 0;
    }
    
#if UNITY_EDITOR && ODIN_INSPECTOR
    [ListDrawerSettings] [ReadOnly]
#endif
    public List<BuildInfo> BuildInfos;

    public BuildInfo GetAssetByName(string name)
    {
        BuildInfo result = null;
        BuildInfos.ForEach(asset =>
        {
            if (asset.BuildName.ToLower() == name.ToLower())
            {
                result = asset;
            }
        });
        return result;
    }
}