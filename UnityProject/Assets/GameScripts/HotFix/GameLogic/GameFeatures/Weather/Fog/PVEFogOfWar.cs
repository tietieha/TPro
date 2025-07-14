using UnityEngine;
using XLua;

[LuaCallCSharp]
[ExecuteInEditMode]
public class PVEFogOfWar : MonoBehaviour
{
    public bool ISPVEFogOfWar = true;
    public bool DebugPveFog = false;

    public Mesh PVEFogOfWarMesh = null;
    public Material PVEFogOfWarMaterial = null;
    public Material PVEFogOfWarFadeMaterial = null;
    // [ColorUsageAttribute(true, true)]
    // public Color MistColor = Color.white;
    // public Texture2D Bottom_Texture = null;
    // public Vector4 Bottom_UV = new Vector4(-0.01f, -0.01f, 0.0f, 0.0f);
    // [Range(0, 1)]
    // public float Bottom_Time = 1;
    //
    //
    // public Texture2D MistNoise = null;
    // public Vector4 MistNoise_UV = new Vector4(-0.01f, -0.01f, 0.0f, 0.0f);
    // [Range(0, 1)]
    // public float MistNoise_Time = 1;
    // [Range(0, 100)]
    // public float MistH = 29;
    // [Range(-1, 10)]
    // public float MistNoise_Alpha = 1;
    //
    // public Texture2D BNoiseTex = null;
    // public Vector4 BNoiseTex_ST = new Vector4(0.05f, 0.05f, 0.0f, 0.0f);
    // //迷雾蒙版
    // public Texture2D PVEFogOfWarMask = null;
    // public Vector4 PVEFogOfWarMask_ST = new Vector4(1.00f, 1.00f, 0.0f, 0.0f);
    //迷雾消散边界（X：POW Y：强度）
    public Vector4 PVEFogOfWarMaskEdge = new Vector4(1.00f, 100f, 0.0f, 0.0f);
    //
    // [Range(0, 1)]
    // public float Intensity = 0;
    //
    // [ColorUsageAttribute(true, true)]
    // public Color SelectedColor = new Color(0.1882f, 0.396f, 0.5764f, 1.0f);
    
    void UpdateMaterial()
    {
        Shader.SetGlobalFloat("G_PVEFogOfWar_On", 0f);
        if (ISPVEFogOfWar)
        {
            Shader.SetGlobalFloat("G_PVEFogOfWar_On", 2f);
            
            // Shader.EnableKeyword("G_PVEFogOfWar_ON");
            // Shader.SetGlobalColor("MistColor", MistColor);
            // Shader.SetGlobalTexture("Bottom_Texture", Bottom_Texture);
            // Shader.SetGlobalVector("Bottom_UV", Bottom_UV);
            // Shader.SetGlobalFloat("Bottom_Time", Bottom_Time);
            //
            // Shader.SetGlobalTexture("MistNoise", MistNoise);
            // Shader.SetGlobalVector("MistNoise_UV", MistNoise_UV);
            // Shader.SetGlobalFloat("MistNoise_Time", MistNoise_Time);
            // Shader.SetGlobalFloat("MistH", MistH);
            // Shader.SetGlobalFloat("MistNoise_Alpha", MistNoise_Alpha);
            //
            // Shader.SetGlobalTexture("BNoiseTex", BNoiseTex);
            // Shader.SetGlobalVector("BNoiseTex_ST", BNoiseTex_ST);
            // Shader.SetGlobalFloat("Intensity", Intensity);
            // Shader.SetGlobalColor("SelectedColor", SelectedColor);

            Shader.SetGlobalVector("PVEFogOfWarMaskEdge", PVEFogOfWarMaskEdge);
        }
        else
        {
            Shader.SetGlobalFloat("G_PVEFogOfWar_On", 0f);
        }

    }
    public void SetData()
    {
        UpdateMaterial();
    }
    
    void OnEnable()
    {
        SetData();
    }

    public Mesh GetFogMesh(float[] vertexArr)
    {
        if (vertexArr.Length % 2 != 0)
        {
            Debug.LogError("Vertex array length must be a multiple of 2. xz");
            return null;
        }
        Vector3[] vertices = new Vector3[vertexArr.Length / 2];
        for (int i = 0, vIndex = 0; i < vertexArr.Length; i += 2, vIndex++)
        {
            vertices[vIndex] = new Vector3(vertexArr[i], 0, vertexArr[i+1]);
        }
        var mesh = Object.Instantiate(PVEFogOfWarMesh);
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        return mesh;
    }

#if UNITY_EDITOR
        void Update()
        {
            // if (ModelRoot.Current.SceneSwitching)
            // {
            //     return;
            // }
            //
            SetData();

            /*
             * 蓝色：全局迷雾开启，且局部迷雾开启
             *  红色：全局迷雾开启，且局部迷雾关闭
             *  绿色：全局迷雾关闭
             */
            if (DebugPveFog)
            {
                Shader.EnableKeyword("_ENBALE_DEBUG_PVE_FOG");
            }
            else
            {
                Shader.DisableKeyword("_ENBALE_DEBUG_PVE_FOG");
            }
        }
    #endif


}
