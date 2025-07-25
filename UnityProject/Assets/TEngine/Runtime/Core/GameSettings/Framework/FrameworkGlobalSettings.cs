using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
// using Sirenix.OdinInspector;
using UnityEditor;
#endif

/// <summary>
/// 资源存放地址。
/// </summary>
[Serializable]
public class ResourcesArea
{
    [Tooltip("资源管理类型")] [SerializeField] private string m_ResAdminType = "Default";

    /// <summary>
    /// 资源管理类型。
    /// </summary>
    public string ResAdminType => m_ResAdminType;

    [Tooltip("资源管理编号")] [SerializeField] private string m_ResAdminCode = "0";

    /// <summary>
    /// 资源管理编号。
    /// </summary>
    public string ResAdminCode => m_ResAdminCode;

    [Tooltip("服务器类型")] [SerializeField] private ServerTypeEnum m_ServerType = ServerTypeEnum.Intranet;

    /// <summary>
    /// 服务器类型。
    /// </summary>
    public ServerTypeEnum ServerType => m_ServerType;
    
    [Tooltip("是否在构建资源的时候清理上传到服务端目录的老资源")] [SerializeField]
    private bool m_CleanCommitPathRes = true;

    public bool CleanCommitPathRes => m_CleanCommitPathRes;

    [Tooltip("内网地址")] [SerializeField] private string m_InnerResourceSourceUrl = "http://127.0.0.1:8088";

    public string InnerResourceSourceUrl => m_InnerResourceSourceUrl;

    [Tooltip("外网地址")] [SerializeField] private string m_ExtraResourceSourceUrl = "http://127.0.0.1:8088";

    public string ExtraResourceSourceUrl => m_ExtraResourceSourceUrl;

    [Tooltip("正式地址")] [SerializeField] private string m_FormalResourceSourceUrl = "http://127.0.0.1:8088";

    public string FormalResourceSourceUrl => m_FormalResourceSourceUrl;
}

[Serializable]
public class ServerIpAndPort
{
    public string ip;
    public int port;

    public string URL => $"{ip}:{port}";
}


[Serializable]
public class UpdateInfo
{
    [Header("服务器类型")]
    public GameNetType gameNetType;
    [Header("配置表更新地址")]
    public string configURL;
    [Header("资源版本请求地址")]
    public string resVersionURL;
    [Header("资源更新地址")]
    public string resURL;
    [Header("包名")]
    public string packageName;
}

[Serializable]
public class FrameworkGlobalSettings
{
    [SerializeField] [Tooltip("脚本作者名")] private string m_ScriptAuthor = "Default";

    public string ScriptAuthor => m_ScriptAuthor;

    [SerializeField] [Tooltip("版本")] private string m_ScriptVersion = "0.1";

    public string ScriptVersion => m_ScriptVersion;

    [Header("Font")] [SerializeField] private string m_DefaultFont = "Arial";
    public string DefaultFont => m_DefaultFont;

    [Header("Logic")]
    [Tooltip("业务服务器类型")] [SerializeField] private GameNetType m_GameNetType = GameNetType.Internal;

    public GameNetType GameNetType
    {
        get => m_GameNetType;
        set => m_GameNetType = value;
    }

    [Header("更新配置")]
    public bool EnableUpdateConfig = false;
    public bool EnableUpdateRes = false;

    [SerializeField] private List<UpdateInfo> m_UpdateInfos;
    public List<UpdateInfo> UpdateInfos => m_UpdateInfos;

    [SerializeField] private string @namespace = "GameLogic";

    public string NameSpace => @namespace;

    [SerializeField] private List<ScriptGenerateRuler> scriptGenerateRule = new List<ScriptGenerateRuler>()
    {
        new ScriptGenerateRuler("m_go", "GameObject"),
        new ScriptGenerateRuler("m_item", "GameObject"),
        new ScriptGenerateRuler("m_tf", "Transform"),
        new ScriptGenerateRuler("m_rect", "RectTransform"),
        new ScriptGenerateRuler("m_text", "Text"),
        new ScriptGenerateRuler("m_richText", "RichTextItem"),
        new ScriptGenerateRuler("m_btn", "Button"),
        new ScriptGenerateRuler("m_img", "Image"),
        new ScriptGenerateRuler("m_rimg", "RawImage"),
        new ScriptGenerateRuler("m_scrollBar", "Scrollbar"),
        new ScriptGenerateRuler("m_scroll", "ScrollRect"),
        new ScriptGenerateRuler("m_input", "InputField"),
        new ScriptGenerateRuler("m_grid", "GridLayoutGroup"),
        new ScriptGenerateRuler("m_hlay", "HorizontalLayoutGroup"),
        new ScriptGenerateRuler("m_vlay", "VerticalLayoutGroup"),
        new ScriptGenerateRuler("m_red", "RedNoteBehaviour"),
        new ScriptGenerateRuler("m_slider", "Slider"),
        new ScriptGenerateRuler("m_group", "ToggleGroup"),
        new ScriptGenerateRuler("m_curve", "AnimationCurve"),
        new ScriptGenerateRuler("m_canvasGroup", "CanvasGroup"),
#if ENABLE_TEXTMESHPRO
        new ScriptGenerateRuler("m_tmp","TextMeshProUGUI"),
#endif
    };

    public List<ScriptGenerateRuler> ScriptGenerateRule => scriptGenerateRule;
}

[Serializable]
public class ScriptGenerateRuler
{
    public string uiElementRegex;
    public string componentName;

    public ScriptGenerateRuler(string uiElementRegex, string componentName)
    {
        this.uiElementRegex = uiElementRegex;
        this.componentName = componentName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptGenerateRuler))]
public class ScriptGenerateRulerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var uiElementRegexRect = new Rect(position.x, position.y, 120, position.height);
        var componentNameRect = new Rect(position.x + 125, position.y, 150, position.height);
        EditorGUI.PropertyField(uiElementRegexRect, property.FindPropertyRelative("uiElementRegex"), GUIContent.none);
        EditorGUI.PropertyField(componentNameRect, property.FindPropertyRelative("componentName"), GUIContent.none);
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
#endif