using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace GEgineRunTime
{
    /// <summary>
    /// 世界分块地形的数据
    /// </summary>
    [Serializable]
    public class WorldRegionResSerializeData
    {
        public int resID;
        public string meshName;
        public string materialName;
        public int renderType;//渲染类型 1 drawmesh 2 gpuInstance
        public int renderLod = 3;//1 地形 2 山 3 其余
        public int cullingGrade = 1; //1 不进行建筑遮挡剔除，进行相机culling 2 进行建筑剔除 3低配地形使用
        public int subMeshIndex = 0; //子网格索引
        public WorldRegionResSerializeData()
        {
        }

        public WorldRegionResSerializeData(int resId, string meshName, string materialName, int renderType, int renderLod, int cullingGrade, int subMeshIndex)
        {
	        this.resID = resId;
            this.meshName = meshName;
            this.materialName = materialName;
            this.renderType = renderType;
            this.renderLod = renderLod;
            this.cullingGrade = cullingGrade;
            this.subMeshIndex = subMeshIndex;
        }

        public override bool Equals(object obj)
        {
            WorldRegionResSerializeData res = obj as WorldRegionResSerializeData;
            return res != null && 
                   this.meshName.Equals(res.meshName) && 
                   this.materialName.Equals(res.materialName) &&
                   this.renderType == res.renderType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>-
    /// 世界整体数据
    /// </summary>
    public class WorldRegionTotalScriptableObject : ScriptableObject
    {
        public float regionTotalWidth;
        public float regionTotalHeight;
        public int regionRowNum;
        public int regionColNum;
        public int regionObjTotalNum;

        /// <summary>
        /// 不参与地形裁剪数据
        /// </summary>
        public List<WorldRenderVariable> worldGlobalRenderDataList = new List<WorldRenderVariable>();
        /// <summary>
        /// 世界使用的资源数据列表
        /// </summary>
        public List<WorldRegionResSerializeData> regionResResDataList = new List<WorldRegionResSerializeData>();

        public List<ParticleEffectAssetData> worldEffectRenderDataList = new List<ParticleEffectAssetData>();

#if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }
    /// <summary>
    /// 最高层地形数据
    /// </summary>

    [Serializable]
    public class WorldLowRegionScriptableObject
    {
        public Vector3 pos;
        public Vector3 euler;
        public Vector3 scale;
        public int assetId;
    }

    //特效数据
    [Serializable]
    public class ParticleEffectAssetData
    {
        public int effectId;
        public float radius;
        public int lod;
        public List<Vector3> posList;
    }


}
