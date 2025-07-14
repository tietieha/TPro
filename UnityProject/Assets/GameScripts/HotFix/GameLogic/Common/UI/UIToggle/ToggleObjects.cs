using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
[RequireComponent(typeof(Toggle))]
public class ToggleObjects : MonoBehaviour
{
    [SerializeField, Tooltip("点击页签显示的内容")] private List<GameObject> containerObjects;
    [SerializeField, Tooltip("页签显示列表")] private List<GameObject> activeObjects;
    [SerializeField, Tooltip("页签隐藏列表")] private List<GameObject> deactiveObjects;
    [SerializeField, Tooltip("页签点击音效路径")] public string audioClipPath;

    private Action<ToggleObjects> m_OnValueChangeEvent;

    public Toggle toggle;
    private bool isInit;
    private ButtonHandler _buttonHandler;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        _buttonHandler = GetComponent<ButtonHandler>();
    }

    private void Start()
    {
        OnToggleValueChanged(toggle.isOn);
        isInit = true;
    }

    public void AddListener()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void RemoveListener()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    public void SetOnValueChange(Action<ToggleObjects> onValueChange)
    {
        m_OnValueChangeEvent = onValueChange;
    }

    public void SetValue(bool isOn)
    {
        toggle.isOn = isOn;
        if (_buttonHandler != null)
        {
            _buttonHandler.SetLock(isOn);
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (_buttonHandler != null)
        {
            _buttonHandler.SetLock(isOn);
        }
        if (activeObjects != null)
        {
            int count = activeObjects.Count;
            for (int i = 0; i < count; ++i)
            {
                activeObjects[i].SetActive(isOn);
            }
        }

        if (deactiveObjects != null)
        {
            int count = deactiveObjects.Count;
            for (int i = 0; i < count; ++i)
            {
                deactiveObjects[i].SetActive(!isOn);
            }
        }

        if (containerObjects != null)
        {
            int count = containerObjects.Count;
            for (int i = 0; i < count; ++i)
            {
                containerObjects[i].SetActive(isOn);
            }
        }

        if (isOn)
        {
            m_OnValueChangeEvent?.Invoke(this);
        }
    }
}
