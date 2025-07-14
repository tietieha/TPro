using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public static class GameObjectUtil
{

    public static void ChangeLayer(GameObject go, int layer, bool changeRenderOnly)
    {
        Type type = changeRenderOnly ? typeof(Renderer) : typeof(Transform);
        Component[] components = go.GetComponentsInChildren(type);
        for (int i = 0; i < components.Length; i++)
        {
            components[i].gameObject.layer = layer;
        }
    }

    public static void ChangeLayer(GameObject go, int layer, int ignoreLayer)
    {
        Transform[] components = go.GetComponentsInChildren<Transform>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].gameObject.layer != ignoreLayer)
            {
                components[i].gameObject.layer = layer;
            }
        }
    }

    public static Camera GetBaseCamera(GameObject go)
    {
        Camera[] components = go.GetComponentsInChildren<Camera>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay)
            {
                return components[i];
            }
        }

        return null;
    }
    
    #region Rendering

    public static void SetDepthOfFieldActive(GameObject root, bool active)
    {
        var volume = root.GetComponentInChildren<Volume>();
        volume.profile.TryGet<DepthOfField>(out var depthComponent);
        depthComponent.active = active;
    }
    
    public static void SetDepthOfFieldData(GameObject root, CityBuildAsset.DepthOfFieldData data)
    {
        var volume = root.GetComponentInChildren<Volume>();
        volume.profile.TryGet<DepthOfField>(out var depthComponent);
        depthComponent.focusDistance.value = data.FocusDistance;
        depthComponent.focalLength.value = data.FocalLength;
        depthComponent.aperture.value = data.Aperture;
    }

    #endregion

}
