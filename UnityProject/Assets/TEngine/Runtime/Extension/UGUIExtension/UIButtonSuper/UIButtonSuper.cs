using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Sirenix.OdinInspector;
using TEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIButtonSuper), true)]
[CanEditMultipleObjects]
public class UIButtonSuperEditor : ButtonEditor
{
    private SerializedProperty m_ButtonUISounds;
    private SerializedProperty m_CanClick;
    private SerializedProperty m_CanDoubleClick;
    private SerializedProperty m_DoubleClickIntervalTime;
    private SerializedProperty onDoubleClick;

    private SerializedProperty m_CanLongPress;
    private SerializedProperty m_ResponseOnceByPress;
    private SerializedProperty m_LongPressDurationTime;
    private SerializedProperty onLongPress;
    
    private SerializedProperty m_CanPlayAni;
    private SerializedProperty m_AnimationNode;
    private SerializedProperty m_DownScale;
    private SerializedProperty m_UpScale;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_ButtonUISounds = serializedObject.FindProperty("m_ButtonUISounds");
        m_CanClick = serializedObject.FindProperty("m_CanClick");
        m_CanDoubleClick = serializedObject.FindProperty("m_CanDoubleClick");
        m_DoubleClickIntervalTime = serializedObject.FindProperty("m_DoubleClickIntervalTime");
        onDoubleClick = serializedObject.FindProperty("onDoubleClick");

        m_CanLongPress = serializedObject.FindProperty("m_CanLongPress");
        m_ResponseOnceByPress = serializedObject.FindProperty("m_ResponseOnceByPress");
        m_LongPressDurationTime = serializedObject.FindProperty("m_LongPressDurationTime");
        onLongPress = serializedObject.FindProperty("onLongPress");
        
        m_CanPlayAni = serializedObject.FindProperty("m_CanPlayAni");
        m_AnimationNode = serializedObject.FindProperty("m_AnimationNode");
        m_DownScale = serializedObject.FindProperty("m_DownScale");
        m_UpScale = serializedObject.FindProperty("m_UpScale");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_ButtonUISounds); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_CanClick); //显示我们创建的属性
        EditorGUILayout.Space(); //空行
        EditorGUILayout.PropertyField(m_CanDoubleClick); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_DoubleClickIntervalTime); //显示我们创建的属性
        EditorGUILayout.PropertyField(onDoubleClick); //显示我们创建的属性
        EditorGUILayout.Space(); //空行
        EditorGUILayout.PropertyField(m_CanLongPress); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_ResponseOnceByPress); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_LongPressDurationTime); //显示我们创建的属性
        EditorGUILayout.PropertyField(onLongPress); //显示我们创建的属性
        EditorGUILayout.Space(); //空行
        EditorGUILayout.PropertyField(m_CanPlayAni); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_AnimationNode); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_DownScale); //显示我们创建的属性
        EditorGUILayout.PropertyField(m_UpScale); //显示我们创建的属性
        serializedObject.ApplyModifiedProperties();
    }
}

#endif

public enum ButtonInteractionType
{
    Down,
    Up,
    Click,
    Enter,
    Exit,
}

[Serializable]
public class ButtonSoundCell
{
    public ButtonInteractionType buttonInteractionType = ButtonInteractionType.Click;
    // 全路径
    public string ButtonUISoundName = "Ugui_button_select_small";
}

public class UIButtonSuper : Button
{
    public List<ButtonSoundCell> m_ButtonUISounds = new List<ButtonSoundCell>() { new ButtonSoundCell() };
    [Tooltip("是否可以点击")] public bool m_CanClick = true;
    [Tooltip("是否可以双击")] public bool m_CanDoubleClick = false;
    [Tooltip("双击间隔时长")] public float m_DoubleClickIntervalTime = 0.1f;
    [Tooltip("双击事件")] public ButtonClickedEvent onDoubleClick;
    [Tooltip("是否可以长按")] public bool m_CanLongPress = false;
    [Tooltip("长按是否只响应一次")] public bool m_ResponseOnceByPress = false;
    [Tooltip("长按满足间隔")] public float m_LongPressDurationTime = 1;

    [Tooltip("长按事件")] public ButtonClickedEvent onLongPress;

    [Tooltip("是否播放按钮动画")] public bool m_CanPlayAni = false;
    [Tooltip("动画节点")] public Transform m_AnimationNode;
    [Tooltip("按下缩小值")] public float m_DownScale = 0.85f;
    [Tooltip("抬起放大值")] public float m_UpScale = 1.05f;
    
    private bool isDown = false;
    private bool isPress = false;
    private bool isDownExit = false;
    private float downTime = 0;

    private float defaultScale = 1;
    
    private int fingerId = int.MinValue;

    public bool IsDraging
    {
        get { return fingerId != int.MinValue; }
    } //摇杆拖拽状态

    public int FingerId
    {
        get { return fingerId; }
    }

    private float clickIntervalTime = 0;
    private int clickTimes = 0;

    void Update()
    {
        if (!interactable)
        {
            return;
        }
        
        if (isDown)
        {
            if (!m_CanLongPress)
            {
                return;
            }

            if (m_ResponseOnceByPress && isPress)
            {
                return;
            }

            downTime += GameTime.deltaTime;
            if (downTime > m_LongPressDurationTime)
            {
                isPress = true;
                onLongPress.Invoke();
            }
        }

        if (clickTimes >= 1)
        {
            if (!m_CanLongPress && !m_CanDoubleClick && m_CanClick)
            {
                onClick.Invoke();
                clickTimes = 0;
            }
            else
            {
                clickIntervalTime += GameTime.deltaTime;
                if (clickIntervalTime >= m_DoubleClickIntervalTime)
                {
                    if (clickTimes >= 2)
                    {
                        if (m_CanDoubleClick)
                        {
                            onDoubleClick.Invoke();
                        }
                    }
                    else
                    {
                        if (m_CanClick)
                        {
                            onClick.Invoke();
                        }
                    }

                    clickTimes = 0;
                    clickIntervalTime = 0;
                }
            }
        }
    }

    /// <summary>
    /// 是否按钮按下
    /// </summary>
    public bool IsDown
    {
        get { return isDown; }
    }

    /// <summary>
    /// 是否按钮长按
    /// </summary>
    public bool IsPress
    {
        get { return isPress; }
    }

    /// <summary>
    /// 是否按钮按下后离开按钮位置
    /// </summary>
    public bool IsDownExit
    {
        get { return isDownExit; }
    }

    public ButtonSoundCell GetButtonSound(ButtonInteractionType buttonInteractionType)
    {
        foreach (var buttonSound in m_ButtonUISounds)
        {
            if (buttonSound.buttonInteractionType == buttonInteractionType)
            {
                return buttonSound;
            }
        }

        return null;
    }

    /// <summary>
    /// 设置音效
    /// </summary>
    /// <param name="buttonInteractionType"></param>
    /// <param name="soundPath">全路径</param>
    public void SetButtonSound(ButtonInteractionType buttonInteractionType, string soundPath)
    {
        ButtonSoundCell buttonSound = GetButtonSound(buttonInteractionType);
        if (buttonSound == null)
        {
            buttonSound = new ButtonSoundCell { buttonInteractionType = buttonInteractionType };
            m_ButtonUISounds.Add(buttonSound);
        }
        
        buttonSound.ButtonUISoundName = soundPath;
    }
    
    private void PlayButtonSound(ButtonInteractionType buttonInteractionType)
    {
        return;
        ButtonSoundCell buttonSound = GetButtonSound(buttonInteractionType);
        if (buttonSound == null)
        {
            return;
        }
        
        GameModule.Audio.Play(TEngine.AudioType.UISound, buttonSound.ButtonUISoundName);
    }

    private void PlayButtonAni(ButtonInteractionType interactionType)
    {
        if (!interactable)
        {
            return;
        }
        
        if (m_CanPlayAni && m_AnimationNode != null)
        {
            if (interactionType == ButtonInteractionType.Down)
            {
                m_AnimationNode.localScale = new Vector3(m_DownScale, m_DownScale, m_DownScale);
            }
            else if (interactionType == ButtonInteractionType.Up)
            {
                m_AnimationNode.localScale = new Vector3(m_UpScale, m_UpScale, m_UpScale);

                GameModule.Timer.AddTimer((args) =>
                {
                    if (m_AnimationNode != null)
                    {
                        m_AnimationNode.localScale = Vector3.one;
                    }
                }, 0.1f);
            }
        }
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        PlayButtonSound(ButtonInteractionType.Enter);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (eventData.pointerId < -1 || IsDraging) return; //适配 Touch：只响应一个Touch；适配鼠标：只响应左键
        fingerId = eventData.pointerId;
        isDown = true;
        isDownExit = false;
        downTime = 0;
        PlayButtonSound(ButtonInteractionType.Down);
        PlayButtonAni(ButtonInteractionType.Down);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (fingerId != eventData.pointerId) return; //正确的手指抬起时才会；
        fingerId = int.MinValue;
        isDown = false;
        isDownExit = true;
        PlayButtonSound(ButtonInteractionType.Up);
        PlayButtonAni(ButtonInteractionType.Up);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (fingerId != eventData.pointerId) return; //正确的手指抬起时才会；
        isPress = false;
        isDownExit = true;
        PlayButtonSound(ButtonInteractionType.Exit);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!isPress)
        {
            clickTimes += 1;
        }
        else
            isPress = false;

        PlayButtonSound(ButtonInteractionType.Click);
    }
}