#if UNITY_EDITOR && ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
using UnityEditor;
#endif
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

public enum BoldMode
{
    off = 0,
    adaptation = 1, //bolda
    level_1 = 2, //bold1
    level_2 = 3, //bold2
    level_3 = 4, //bold3
}

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPRenderParameter : MonoBehaviour
{
#if UNITY_EDITOR && ODIN_INSPECTOR_3
    [ValueDropdown("GetStyleNames")]
#endif
    [Header("字体样式")]public string StyleName;

    // 加粗
    [Header("加粗设置")] public BoldMode BoldType = BoldMode.adaptation;

    //描边
    [Header("开启描边")] public bool useOutLine = false;
    [Range(0, 10)] public float m_OutLine = 0.35f;
    public Color m_OutLineColor = new Color(0, 0, 0, 1);

    [HideInInspector][Header("描边关后有效")] [Range(-1, 1)] public float m_FaceDilate = 0;
    [HideInInspector][Range(0, 1)] public float m_OutLineThickness = 0;
    [HideInInspector]public Texture2D _OutlineTextures;

    //阴影
    [Header("是否开启阴影")] public bool useUnderlayColor = false;
    public Color m_UnderlayColor = new Color(0, 0, 0, 1);
    [Range(-10, 10)] public float m_UnderlayOffsetX = 0;
    [Range(-10, 10)] public float m_UnderlayOffsetY = 0;
    [Range(-1, 1)] public float m_UnderlayDilate = 0;
    [Range(0, 1)] public float m_UnderlaySoftness = 0;

    //贴图
    [HideInInspector][Header("贴图")] public Texture2D m_GroundTextures;
    [HideInInspector][Range(-5, 5)] public float m_SpeedX = 0;
    [HideInInspector][Range(-5, 5)] public float m_SpeedY = 0;
    [HideInInspector]public Vector2 m_Tiling = Vector2.one;
    [HideInInspector]public Vector2 m_Offset = Vector2.zero;

    [HideInInspector][Header("遮罩贴图")] public Texture2D maskTex;
    [HideInInspector][Header("是否切换UV")] public bool UseChangeUV = false;
    [HideInInspector]public float uv;
    [HideInInspector]public Vector2 m_Tiling02 = Vector2.one;
    [HideInInspector]public Vector2 m_Offset02 = Vector2.one;
    [HideInInspector][Range(-1, 1)] public float maskThreshold_R = 0;
    [HideInInspector][Range(0, 1)] public float maskSoft_R = 0.001f;

    //外发光
    [HideInInspector][Header("是否开启外发光")] public bool useGlowColor = false;
    [HideInInspector]public Color m_GlowColor = new Color(1, 1, 1, 1);
    [HideInInspector][Range(-1, 1)] public float m_GlowOffset = 0;
    [HideInInspector][Range(0, 1)] public float m_GlowInner = 0;
    [HideInInspector][Range(0, 1)] public float m_GlowOuter = 0;
    [HideInInspector][Range(1, 0)] public float m_GlowPower = 0;

    //光照
    [HideInInspector][Header("是否开启光照")] public bool useLight = false;
    [HideInInspector][Header("是否开启斜面")] public bool useBevel = false;
    [HideInInspector][Range(-1, 1)] public float m_Bevel = 0.5f;
    [HideInInspector][Range((float)-0.5, (float)0.5)] public float m_BevelOffset = 0;
    [HideInInspector][Range((float)-0.5, (float)0.5)] public float m_BevelWidth = 0;
    [HideInInspector][Range(0, 1)] public float m_BevelClamp = 0;
    [HideInInspector][Range(0, 1)] public float m_BevelRoundness = 0;

    [HideInInspector][Header("是否开启局部照明")] public bool UseLocalLight = false;
    [HideInInspector]public Color m_SpecularColor = new Color(1, 1, 1, 1);
    [HideInInspector][Range((float)0.0, (float)6.2831853)] public float m_LightAngle = 3.1416f;
    [HideInInspector][Range(0, 4)] public float m_SpecularPower = 2;
    [HideInInspector][Range(5, 15)] public float m_Reflectivity = 10;
    [HideInInspector][Range(0, 1)] public float m_Diffuse = 0.5f;
    [HideInInspector][Range(1, 0)] public float m_Ambient = 0.5f;

    [HideInInspector][Header("是否开启凹凸贴图")] public bool UseBumpMap = false;
    [HideInInspector]public Texture2D m_BumpMap;
    [HideInInspector][Range(0, 1)] public float m_BumpOutline = 0;
    [HideInInspector][Range(0, 1)] public float m_BumpFace = 0;

    private TextMeshProUGUI m_text;
    private TMP_FontAsset m_OriFont;
    private Material m_CurrentMate;
    private string m_StyleName;
    private Hash128 m_MaterialHash;

    private void OnEnable()
    {
        Init();
        ApplyStyle();
        Refresh();
    }

    private void Update()
    {
        if (m_StyleName != StyleName)
        {
            m_StyleName = StyleName;
            ApplyStyle();
            Refresh();
        }
    }

    private void OnDestroy()
    {
        // SafeDestroy(m_CurrentMate);
        ReleaseMaterial();
    }

    float bold1 = 0.1f;
    float bold2 = 0.13f;
    float bold3 = 0.16f;

    private void Init()
    {
        if (m_text == null)
        {
            m_text = GetComponent<TextMeshProUGUI>();
        }

        if (m_OriFont != m_text.font)
        {
            // if (m_CurrentMate != null)
            // {
            //     SafeDestroy(m_CurrentMate);
            // }
            ReleaseMaterial();
            m_OriFont = m_text.font;
            if (!Application.isPlaying)
                m_CurrentMate = Instantiate(m_text.fontSharedMaterial);
            else
                UpdateMaterial();
        }
    }
    // 在类顶部添加静态StringBuilder用于复用
    private static readonly System.Text.StringBuilder s_HashBuilder = new System.Text.StringBuilder(256);
    private Hash128 CalculateMaterialHash()
    {
        var hash = new Hash128();

        // 基于原始材质ID
        // if (m_text.fontSharedMaterial != null)
        // {
        //     hash = Hash128.Compute(m_text.fontSharedMaterial.GetInstanceID().ToString());
        // }

        // 添加可见的公共属性到哈希计算
        var fontGuid = m_OriFont.name;
        var boldType = (int)BoldType;
        var outline = (int)(m_OutLine * 100);
        var underlayOffsetX = (int)(m_UnderlayOffsetX * 100);
        var underlayOffsetY = (int)(m_UnderlayOffsetY * 100);
        var underlayDilate = (int)(m_UnderlayDilate * 100);
        var underlaySoftness = (int)(m_UnderlaySoftness * 100);
        var outlineColorR = (int)(m_OutLineColor.r * 255);
        var outlineColorG = (int)(m_OutLineColor.g * 255);
        var outlineColorB = (int)(m_OutLineColor.b * 255);
        var outlineColorA = (int)(m_OutLineColor.a * 255);
        var underlayColorR = (int)(m_UnderlayColor.r * 255);
        var underlayColorG = (int)(m_UnderlayColor.g * 255);
        var underlayColorB = (int)(m_UnderlayColor.b * 255);
        var underlayColorA = (int)(m_UnderlayColor.a * 255);

        // 使用StringBuilder构建哈希字符串
        s_HashBuilder.Clear();
        s_HashBuilder.Append(fontGuid)
            .Append('_').Append(boldType)
            .Append('_').Append(useOutLine)
            .Append('_').Append(outline)
            .Append('_').Append(outlineColorR).Append('_').Append(outlineColorG).Append('_').Append(outlineColorB).Append('_').Append(outlineColorA)
            .Append('_').Append(useUnderlayColor)
            .Append('_').Append(underlayColorR).Append('_').Append(underlayColorG).Append('_').Append(underlayColorB).Append('_').Append(underlayColorA)
            .Append('_').Append(underlayOffsetX)
            .Append('_').Append(underlayOffsetY)
            .Append('_').Append(underlayDilate)
            .Append('_').Append(underlaySoftness);
        var stringToHash = s_HashBuilder.ToString();
        hash = Hash128.Compute(stringToHash);
        // Debug.Log($"TMPRenderParameter_Log GameObject: {gameObject.name}, Hash: {hash}, stringToHash{stringToHash}, Material InstanceID: {(m_CurrentMate ? m_CurrentMate.GetInstanceID() : 0)}, Original Material: {(m_text.fontSharedMaterial ? m_text.fontSharedMaterial.GetInstanceID() : 0)}");
        return hash;
    }
    private void UpdateMaterial()
    {
        // 添加基础检查
        if (m_text == null || m_text.fontSharedMaterial == null)
        {
            return;
        }

#if UNITY_EDITOR
        // 编辑器模式下不使用缓存，直接创建材质实例
        if (!Application.isPlaying)
        {
            if (m_CurrentMate != null)
            {
                ApplyMaterialProperties();
            }
            return;
        }
#endif
        var newHash = CalculateMaterialHash();

        if (newHash == m_MaterialHash && m_CurrentMate != null)
            return;

        // 释放旧材质
        ReleaseMaterial();

        // 获取新材质
        m_MaterialHash = newHash;
        m_CurrentMate = TMPMaterialCache.Get(m_MaterialHash, m_text.fontSharedMaterial);

        if (m_CurrentMate != null)
        {
            // DebugMaterialCache();
            ApplyMaterialProperties();
        }
    }
    private void ReleaseMaterial()
    {
#if UNITY_EDITOR
        // 编辑器模式下直接销毁材质
        if (!Application.isPlaying)
        {
            if (m_CurrentMate != null)
            {
                Object.DestroyImmediate(m_CurrentMate);
                m_CurrentMate = null;
            }
            return;
        }
#endif
        if (m_MaterialHash.isValid)
        {
            TMPMaterialCache.Release(m_MaterialHash);
            m_MaterialHash = new Hash128();
            m_CurrentMate = null;
            SafeDestroy(m_CurrentMate);
        }
    }

    private void ApplyStyle()
    {
        if (string.IsNullOrEmpty(StyleName))
            return;

        var textAsset = GetTextStyleAsset();
        var style = textAsset.GetStyleParam(StyleName);

        if (style == null)
            return;

        m_text.fontSize = style.m_FontSize;
        m_text.color = style.m_FontColor;
        BoldType = style.BoldType;
        useOutLine = style.useOutLine;
        m_OutLine = style.m_OutLine;
        m_OutLineColor = style.m_OutLineColor;
        useUnderlayColor = style.useUnderlayColor;
        m_UnderlayColor = style.m_UnderlayColor;
        m_UnderlayOffsetX = style.m_UnderlayOffsetX;
        m_UnderlayOffsetY = style.m_UnderlayOffsetY;
        m_UnderlayDilate = style.m_UnderlayDilate;
        m_UnderlaySoftness = style.m_UnderlaySoftness;
    }

    public void Refresh()
    {
        if (m_CurrentMate == null)
        {
            Init();
            ApplyStyle();
        }

        UpdateMaterial();

        if (m_CurrentMate != null)
        {
            GetComponent<TextMeshProUGUI>().fontMaterial = m_CurrentMate;
        }
    }
    private void ApplyMaterialProperties()
    {
        if (m_CurrentMate == null)
            return;

        if (m_CurrentMate == null)
            return;

        float bolda = 1.2f / m_text.fontSize;
        float boldd = bolda > 0.032f ? bolda : 0.0f;
        switch (BoldType)
        {
            case BoldMode.off:
                m_CurrentMate.SetFloat("_FaceDilate", m_FaceDilate);
                break;
            case BoldMode.adaptation:
                m_CurrentMate.SetFloat("_FaceDilate", boldd);
                break;
            case BoldMode.level_1:
                m_CurrentMate.SetFloat("_FaceDilate", bold1);
                break;
            case BoldMode.level_2:
                m_CurrentMate.SetFloat("_FaceDilate", bold2);
                break;
            case BoldMode.level_3:
                m_CurrentMate.SetFloat("_FaceDilate", bold3);
                break;
            default:
                break;
        }

        if (useOutLine)
        {
            float m_otsize = m_OutLine / (m_text.fontSize * 0.21978f);
            float m_ot = m_otsize < 1f ? m_otsize : 0.99f;
            m_CurrentMate.SetColor("_OutlineColor", m_OutLineColor);
            m_CurrentMate.SetFloat("_OutlineWidth", m_ot);
            switch (BoldType)
            {
                case BoldMode.off:
                    m_CurrentMate.SetFloat("_FaceDilate", m_ot);
                    break;
                case BoldMode.level_1:
                    m_CurrentMate.SetFloat("_FaceDilate", m_ot + bold1);
                    break;
                case BoldMode.level_2:
                    m_CurrentMate.SetFloat("_FaceDilate", m_ot + bold2);
                    break;
                case BoldMode.level_3:
                    m_CurrentMate.SetFloat("_FaceDilate", m_ot + bold3);
                    break;
                default:
                    break;
            }
            //m_CurrentMate.SetFloat("_FaceDilate", m_ot);
        }
        else
        {
            m_CurrentMate.SetColor("_OutlineColor", m_OutLineColor);
            //m_CurrentMate.SetFloat("_FaceDilate", m_FaceDilate);
            m_CurrentMate.SetFloat("_OutlineWidth", m_OutLineThickness);
            m_CurrentMate.SetTexture("_OutlineTex", _OutlineTextures);
        }

        m_CurrentMate.SetTexture("_FaceTex", m_GroundTextures);
        m_CurrentMate.SetFloat("_FaceUVSpeedX", m_SpeedX);
        m_CurrentMate.SetFloat("_FaceUVSpeedY", m_SpeedY);
        m_CurrentMate.SetTextureScale("_FaceTex", m_Tiling);
        m_CurrentMate.SetTextureOffset("_FaceTex", m_Offset);

        m_CurrentMate.SetTexture("_DiffuseMaskTex", maskTex);
        m_CurrentMate.SetTextureScale("_DiffuseMaskTex", m_Tiling02);
        m_CurrentMate.SetTextureOffset("_DiffuseMaskTex", m_Offset02);
        m_CurrentMate.SetFloat("_RangeMaskR", maskThreshold_R);
        m_CurrentMate.SetFloat("_MaskSoftR", maskSoft_R);

        if (UseChangeUV)
        {
            m_CurrentMate.SetFloat("_ChangeUV", 1);
        }
        else
        {
            m_CurrentMate.SetFloat("_ChangeUV", 0);
        }

        if (useGlowColor)
        {
            m_CurrentMate.EnableKeyword("GLOW_ON");
            m_CurrentMate.SetColor("_GlowColor", m_GlowColor);
            m_CurrentMate.SetFloat("_GlowOffset", m_GlowOffset);
            m_CurrentMate.SetFloat("_GlowInner", m_GlowInner);
            m_CurrentMate.SetFloat("_GlowOuter", m_GlowOuter);
            m_CurrentMate.SetFloat("_GlowPower", m_GlowPower);
        }
        else
        {
            m_CurrentMate.DisableKeyword("GLOW_ON");
        }

        if (useUnderlayColor)
        {
            m_CurrentMate.EnableKeyword("UNDERLAY_ON");
            m_CurrentMate.SetColor("_UnderlayColor", m_UnderlayColor);
            m_CurrentMate.SetFloat("_UnderlayOffsetX", m_UnderlayOffsetX);
            m_CurrentMate.SetFloat("_UnderlayOffsetY", m_UnderlayOffsetY);
            m_CurrentMate.SetFloat("_UnderlayDilate", m_UnderlayDilate);
            m_CurrentMate.SetFloat("_UnderlaySoftness", m_UnderlaySoftness);
        }
        else
        {
            m_CurrentMate.DisableKeyword("UNDERLAY_ON");
        }

        if (useLight)
        {
            useBevel = true;
            UseLocalLight = true;
            UseBumpMap = true;
            m_CurrentMate.EnableKeyword("BEVEL_ON");
            //斜面
            m_CurrentMate.SetFloat("_Bevel", m_Bevel);
            m_CurrentMate.SetFloat("_BevelOffset", m_BevelOffset);
            m_CurrentMate.SetFloat("_BevelClamp", m_BevelClamp);
            m_CurrentMate.SetFloat("_BevelRoundness", m_BevelRoundness);
            m_CurrentMate.SetFloat("_BevelWidth", m_BevelWidth);
            //局部
            m_CurrentMate.SetColor("_SpecularColor", m_SpecularColor);
            m_CurrentMate.SetFloat("_LightAngle", m_LightAngle);
            m_CurrentMate.SetFloat("_SpecularPower", m_SpecularPower);
            m_CurrentMate.SetFloat("_Reflectivity", m_Reflectivity);
            m_CurrentMate.SetFloat("_Diffuse", m_Diffuse);
            m_CurrentMate.SetFloat("_Ambient", m_Ambient);
            //凹凸
            m_CurrentMate.SetTexture("_BumpMap", m_BumpMap);
            m_CurrentMate.SetFloat("_BumpOutline", m_BumpOutline);
            m_CurrentMate.SetFloat("_BumpFace", m_BumpFace);
        }
        else
        {
            m_CurrentMate.DisableKeyword("BEVEL_ON");
        }
    }

    private void OnValidate()
    {
        if (m_CurrentMate != null)
            Refresh();
    }

    private void SafeDestroy(Object obj)
    {
        if (!Application.isPlaying)
        {
            DestroyImmediate(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    // TextStyleAsset
    private static UITextStyleAsset m_TextStyleAsset;

    public static void SetTextStyleAsset(UITextStyleAsset textStyleAsset)
    {
        if (textStyleAsset == null)
        {
            Debug.LogError("UITextStyleAsset cannot be null");
            return;
        }

        m_TextStyleAsset = textStyleAsset;
    }

    private UITextStyleAsset GetTextStyleAsset()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            return m_TextStyleAsset;
        }
        else
        {
            var textAsset = AssetDatabase.LoadAssetAtPath<UITextStyleAsset>("Assets/GameAssets/SoAsset/TextAsset/UITextStyleAsset.asset");
            return textAsset;
        }
#endif
        return m_TextStyleAsset;
    }

    private string[] GetStyleNames()
    {
        var textAsset = GetTextStyleAsset();
        return textAsset.GetStyleNames();
    }

#if UNITY_EDITOR && ODIN_INSPECTOR_3
    [Button("保存样式")]
    public void SaveStyle(string styleName)
    {
        if (string.IsNullOrEmpty(styleName))
        {
            Debug.LogWarning("样式名称不能为空");
            return;
        }

        var textAsset = GetTextStyleAsset();
        var style = textAsset.GetStyleParam(styleName);
        var confirm = true;
        if (style == null)
        {
            style = new UITextStyleAsset.UITextStyleParam();
            style.StyleName = styleName;
            textAsset.AddStyleParam(style);
        }
        else
        {
            confirm = EditorUtility.DisplayDialog("保存样式", $"样式 {styleName} 已存在，是否变更新参数", "确认", "取消");
        }

        if (confirm)
        {
            style.m_FontColor = m_text.color;
            style.m_FontSize = m_text.fontSize;
            style.BoldType = BoldType;
            style.useOutLine = useOutLine;
            style.m_OutLine = m_OutLine;
            style.m_OutLineColor = m_OutLineColor;
            style.useUnderlayColor = useUnderlayColor;
            style.m_UnderlayColor = m_UnderlayColor;
            style.m_UnderlayOffsetX = m_UnderlayOffsetX;
            style.m_UnderlayOffsetY = m_UnderlayOffsetY;
            style.m_UnderlayDilate = m_UnderlayDilate;
            style.m_UnderlaySoftness = m_UnderlaySoftness;

            EditorUtility.SetDirty(textAsset);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("保存样式", $"样式 {styleName} 已保存！", "确认");
        }
    }

    [Button("查看样式文件")]
    public void PingAsset()
    {
        var textAsset = AssetDatabase.LoadAssetAtPath<UITextStyleAsset>("Assets/GameAssets/SoAsset/TextAsset/UITextStyleAsset.asset");
        EditorGUIUtility.PingObject(textAsset);
    }
#endif
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void DebugMaterialCache()
    {
        // var hash = CalculateMaterialHash();
        // Debug.Log($"GameObject: {gameObject.name}, Hash: {hash}, Material InstanceID: {(m_CurrentMate ? m_CurrentMate.GetInstanceID() : 0)}, Original Material: {(m_text.fontSharedMaterial ? m_text.fontSharedMaterial.GetInstanceID() : 0)}");

        // 输出影响哈希的关键属性
        // Debug.Log($"Key Properties - StyleName: {StyleName}, BoldType: {BoldType}, useOutLine: {useOutLine}, m_OutLine: {m_OutLine}, " +
        //           $"useUnderlayColor: {useUnderlayColor}, m_UnderlayColor: {m_UnderlayColor}");
    }
}