using System.Collections.Generic;
using UnityEngine;

namespace Ryan_T01
{
    //[ExecuteInEditMode]
    public class CurseBuildingMatParameter
    {
        public Material myMat;
        public bool ifBuildShipUnlockFogShader;
        public float original_DepthOffset= 0;
        public float original_DepthSoft = 0;
        public float original_SeriousDepthOffset = 0;
        public float original_SeriousDepthSoftOffset = 0;
        public float original_GostNoiseIntensity = 0;
        public Vector4 original_GostNoiseSpeedXYTillingZW = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        public float original_DistorTexDistortionIntensity = 0;
        public Vector4 original_DistorTexSpeedXYTillingZW = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        public float original_GostFogNoHeightRestrict = 0;
    }
    //[ExecuteInEditMode]
    public class SetCurseBuildingMatProp : MonoBehaviour
    {
        public Transform RoomRoot;
        [Header("需要单独调整部分材质")]
        public GameObject[] IndependentAdjustmentMaterials ;
        [Header("需要排除调整部分材质")]
        public Material[] ExcludeMaterials;

        [Space(10)]
        [Header("迷雾状态下贴图颜色")]
        public Color LightTexColor = new Color(0.5f, 0.8f, 1.0f, 1.0f);
        [Header("诅咒迷雾颜色")]
        public Color GostColor = new Color(0.13764706f, 0.21176471f, 0.13764706f, 1.0f);
        [Header("深度偏移  偏移")]
        public float DepthOffset = 0.0f;
        [Range(-10.0f, 10.0f)]
        [Header("深度渐变硬度 偏移")]
        public float DepthSoft = 0.0f;
        [Range(-1.0f, 1.0f)]
        [Header("烟雾细节强度 偏移")]
        public float GostNoiseIntensity = 0.0f;
        [Header("烟雾细节移动速度/重复度 偏移")]
        public Vector4 GostNoiseSpeedXYTillingZW = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        [Range(-5.0f, 5.0f)]
        [Header("烟雾细节扭曲 偏移")]
        public float DistorTexDistortionIntensity = 0.0f;
        [Header("烟雾细节扭曲移动速度/重复度 偏移")]
        public Vector4 DistorTexSpeedXYTillingZW = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        [Header("重度雾深度 偏移")]
        public float SeriousDepthOffset = 0.0f;
        [Header("重度雾硬度 偏移")]
        public float SeriousDepthSoftOffset = 0.0f;
        [Space(10)]
        [Header("船舱层级对灯光限制 开/关(停止运行会设置回去)")]
        public bool GostFogNoHeightRestrict = false;
        [Header("观察灰度渐变值(停止运行会设置为零)")]
        public bool DebugDepth = false;
        //[Header("移除假灯光对材质影响")]
        //public bool RemoveLight = false;
        [Header("雾效轻度重度变化(停止运行会设置为零)")]
        [Range(0.0f, 1.0f)]
        public float GostFogMildSeriousSW = 0.0f;
        [Header("解锁变化(停止运行会设置为零)")]
        [Range(0.0f, 1.0f)]
        public float GostFogMainSwitch = 0.0f;
        List<Material> SceneUsedMats = new List<Material>();
        List<CurseBuildingMatParameter> SceneMats = new List<CurseBuildingMatParameter>();
        CurseBuildingMatParameter myCurseBuildingMatParameter;
        public bool OverWriteMat = false;
        // Start is called before the first frame update
        
        public void InitRoom()
        {
            if (RoomRoot != null)
            {
                SceneMats.Clear();
                Renderer[] mySceneRenders = RoomRoot.GetComponentsInChildren<Renderer>();
                if (mySceneRenders.Length > 0)
                {
                    foreach (var item in mySceneRenders)
                    {
                        if (OverWriteMat) SceneUsedMats.Add(item.sharedMaterial);
                        else SceneUsedMats.Add(item.material);
                    }

                    if (IndependentAdjustmentMaterials.Length>0&& IndependentAdjustmentMaterials[0]!=null)
                    {
                        SceneUsedMats.Clear();
                        foreach (var usedmat in IndependentAdjustmentMaterials)
                        {
                            Renderer[] myMaterial = usedmat.GetComponentsInChildren<Renderer>();
                            foreach (var itemRenderer02 in myMaterial)
                            {
                                SceneUsedMats.Add(itemRenderer02.sharedMaterial);
                            }
                        }
                    }
                    if (ExcludeMaterials.Length > 0 && ExcludeMaterials[0] != null)
                    {
                        foreach (var ExcludeMaterials03 in ExcludeMaterials)
                        {
                            if (SceneUsedMats.Contains(ExcludeMaterials03))
                            {
                                SceneUsedMats.Remove(ExcludeMaterials03);
                            }
                        }
                    }
                    foreach (var useMat00 in SceneUsedMats)
                    {
                        myCurseBuildingMatParameter = new CurseBuildingMatParameter();
                        myCurseBuildingMatParameter.myMat = useMat00;
                        bool CurseShipShader = myCurseBuildingMatParameter.myMat.shader.name == "UW/BuildShipUnlockFog";
                        myCurseBuildingMatParameter.ifBuildShipUnlockFogShader = CurseShipShader;
                        if (CurseShipShader)
                        {
                            myCurseBuildingMatParameter.original_DepthOffset = myCurseBuildingMatParameter.myMat.GetFloat("_DepthOffset");
                            myCurseBuildingMatParameter.original_DepthSoft = myCurseBuildingMatParameter.myMat.GetFloat("_DepthSoft");
                            myCurseBuildingMatParameter.original_SeriousDepthOffset = myCurseBuildingMatParameter.myMat.GetFloat("_SeriousDepthOffset");
                            myCurseBuildingMatParameter.original_SeriousDepthSoftOffset = myCurseBuildingMatParameter.myMat.GetFloat("_SeriousDepthSoftOffset");
                            myCurseBuildingMatParameter.original_GostNoiseIntensity = myCurseBuildingMatParameter.myMat.GetFloat("_GostNoiseIntensity");
                            myCurseBuildingMatParameter.original_GostNoiseSpeedXYTillingZW = myCurseBuildingMatParameter.myMat.GetVector("_GostNoiseSpeedXYTillingZW");
                            myCurseBuildingMatParameter.original_DistorTexDistortionIntensity = myCurseBuildingMatParameter.myMat.GetFloat("_DistorTexDistortionIntensity");
                            myCurseBuildingMatParameter.original_DistorTexSpeedXYTillingZW = myCurseBuildingMatParameter.myMat.GetVector("_DistorTexSpeedXYTillingZW");
                            myCurseBuildingMatParameter.original_GostFogNoHeightRestrict = myCurseBuildingMatParameter.myMat.GetFloat("_GostFogNoHeightRestrict");
                        }
                        SceneMats.Add(myCurseBuildingMatParameter);
                    }
                }
            }
        }

        public void UnInitRoom()
        {
            if (SceneMats.Count > 0)
            {
                foreach (var oneMat in SceneMats)
                {
                    if (oneMat.myMat.HasProperty("_GostFogMainSwitch"))
                    {
                        oneMat.myMat.SetFloat("_GostFogMainSwitch", 0);
                    }
                    if (oneMat.myMat.HasProperty("_GostFogMildSeriousSW"))
                    {
                        oneMat.myMat.SetFloat("_GostFogMildSeriousSW", 0);
                    }
                    //调主色调，调偏移软硬，
                    if (oneMat.ifBuildShipUnlockFogShader)
                    {
                        oneMat.myMat.SetColor("_LightTexColor", LightTexColor);
                        oneMat.myMat.SetColor("_GostColor", GostColor);
                        //做偏移操作
                        //做偏移操作
                        if (oneMat.original_DepthOffset < 100)
                        {
                            oneMat.myMat.SetFloat("_DepthOffset", oneMat.original_DepthOffset + DepthOffset);
                            oneMat.myMat.SetFloat("_DepthSoft", Mathf.Clamp((oneMat.original_DepthSoft + DepthSoft), 0.00001f, 10.0f));
                        }
                        else
                        {
                            oneMat.myMat.SetFloat("_DepthOffset", oneMat.original_DepthOffset + DepthOffset * 100.0f);
                            oneMat.myMat.SetFloat("_DepthSoft", Mathf.Clamp((oneMat.original_DepthSoft + DepthSoft * 0.01f), 0.00001f, 10.0f));
                        }
                        oneMat.myMat.SetFloat("_SeriousDepthOffset", oneMat.original_SeriousDepthOffset + SeriousDepthOffset);
                        oneMat.myMat.SetFloat("_SeriousDepthSoftOffset", oneMat.original_SeriousDepthSoftOffset + SeriousDepthSoftOffset);
                        oneMat.myMat.SetFloat("_GostNoiseIntensity", Mathf.Clamp((oneMat.original_GostNoiseIntensity + GostNoiseIntensity), 0.000001f, 1.0f));
                        oneMat.myMat.SetVector("_GostNoiseSpeedXYTillingZW", oneMat.original_GostNoiseSpeedXYTillingZW + GostNoiseSpeedXYTillingZW);
                        oneMat.myMat.SetFloat("_DistorTexDistortionIntensity", oneMat.original_DistorTexDistortionIntensity + DistorTexDistortionIntensity);
                        oneMat.myMat.SetVector("_DistorTexSpeedXYTillingZW", oneMat.original_DistorTexSpeedXYTillingZW + DistorTexSpeedXYTillingZW);

                        oneMat.myMat.SetFloat("_GostFogNoHeightRestrict", oneMat.original_GostFogNoHeightRestrict);
                        oneMat.myMat.SetFloat("_DebugDepth", 0);
                    }
                }
            }
        }
        
        void OnEnable()
        {
            InitRoom();
        }

        // Update is called once per frame
        void Update()
        {
            if (SceneMats.Count > 0)
            {
                foreach (var oneMat in SceneMats)
                {
                    if (oneMat.myMat.HasProperty("_GostFogMainSwitch"))
                    {
                        oneMat.myMat.SetFloat("_GostFogMainSwitch", GostFogMainSwitch);
                    }
                    if (oneMat.myMat.HasProperty("_GostFogMildSeriousSW"))
                    {
                        oneMat.myMat.SetFloat("_GostFogMildSeriousSW", GostFogMildSeriousSW);
                    }
                    //调主色调，调偏移软硬，
                    if (oneMat.ifBuildShipUnlockFogShader)
                    {
                        oneMat.myMat.SetColor("_LightTexColor", LightTexColor);
                        oneMat.myMat.SetColor("_GostColor", GostColor);
                        oneMat.myMat.SetFloat("_GostFogNoHeightRestrict", GostFogNoHeightRestrict ? 1 : 0);
                        oneMat.myMat.SetFloat("_DebugDepth", DebugDepth ? 1 : 0);
                        //做偏移操作
                        if (oneMat.original_DepthOffset<100)
                        {
                            oneMat.myMat.SetFloat("_DepthOffset", oneMat.original_DepthOffset + DepthOffset);
                            oneMat.myMat.SetFloat("_DepthSoft", Mathf.Clamp((oneMat.original_DepthSoft + DepthSoft), 0.00001f, 10.0f));
                        }
                        else
                        {
                            oneMat.myMat.SetFloat("_DepthOffset", oneMat.original_DepthOffset + DepthOffset*100.0f);
                            oneMat.myMat.SetFloat("_DepthSoft", Mathf.Clamp((oneMat.original_DepthSoft + DepthSoft*0.01f), 0.00001f, 10.0f));
                        }
                        oneMat.myMat.SetFloat("_SeriousDepthOffset", oneMat.original_SeriousDepthOffset + SeriousDepthOffset);
                        oneMat.myMat.SetFloat("_SeriousDepthSoftOffset", oneMat.original_SeriousDepthSoftOffset + SeriousDepthSoftOffset);
                        oneMat.myMat.SetFloat("_GostNoiseIntensity", Mathf.Clamp((oneMat.original_GostNoiseIntensity + GostNoiseIntensity), 0.000001f, 1.0f));
                        oneMat.myMat.SetVector("_GostNoiseSpeedXYTillingZW", oneMat.original_GostNoiseSpeedXYTillingZW + GostNoiseSpeedXYTillingZW);
                        oneMat.myMat.SetFloat("_DistorTexDistortionIntensity", Mathf.Clamp((oneMat.original_DistorTexDistortionIntensity + DistorTexDistortionIntensity), 0.000001f, 5.0f));
                        oneMat.myMat.SetVector("_DistorTexSpeedXYTillingZW", oneMat.original_DistorTexSpeedXYTillingZW + DistorTexSpeedXYTillingZW);

                    }
                }
            }
        }

        private void OnDisable()
        {
            UnInitRoom();
        }
    }
}
