using System.Collections;
using System.Collections.Generic;
// using Topjoy.Engine.GPUSkinning;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace M.Battle
{
    public class InstanceAsset : ScriptableObject
    {
#if UNITY_EDITOR

        [MenuItem("Assets/Create/AnimData/InstanceData", false, 0)]
        public static void CreateInstanceData()
        {
            var instance = ScriptableObject.CreateInstance<InstanceAsset>();
            AssetDatabase.CreateAsset(instance, "Assets/InstanceData.asset");
        }
#endif

        // public List<AnimationClipConfig> AnimationClipConfigs; // TODO LZL 不知道是啥
        public Mesh InstanceMesh;
        public Material IntanceMaterial;
    }
}

