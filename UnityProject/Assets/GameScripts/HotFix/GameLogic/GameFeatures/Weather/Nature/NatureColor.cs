using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[ExecuteInEditMode]
public class NatureColor : MonoBehaviour
{
    [LabelText("美术工程目录")] public Vector3 HueZoomMinMax = new Vector3(1f, 1f, 0f);
    public Vector3 SaturationZoomMinMax = new Vector3(1, 0, 1);

    public Vector3 ValueZoomMinMax = new Vector3(1, 0, 1);

    //初始后大小固定，后续无法更改，建议第一次初始化足够的颜色数量
    public static int colorNum = 30;
    public Vector4[] SetColors;
    public Color[] colors;

    void SetData()
    {
        Shader.SetGlobalVectorArray("g_NatureColos", SetColors);
        Shader.SetGlobalFloat("g_ColorNum", colorNum);
    }

    void OnEnable()
    {
        SetData();
    }

#if UNITY_EDITOR
    void Update()
    {
        SetData();
    }

    // 默认值，用于编辑器预览
    [InitializeOnLoadMethod]
    static void OnLoad()
    {
        var colors = new Vector4[colorNum];
        for (int index = 0; index < colorNum; index++)
        {
            colors[index] = new Vector4(1, 1, 1, 1.0f);
        }

        Shader.SetGlobalVectorArray("g_NatureColos", colors);
    }
#endif
}