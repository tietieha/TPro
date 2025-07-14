using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Toggle组合，添加监听事件用于功能逻辑处理
/// </summary>
[XLua.LuaCallCSharp]
public class UIToggleGroup : MonoBehaviour
{
    [SerializeField] private List<ToggleObjects> m_ToggleList = new List<ToggleObjects>();


    private int m_CurrentIndex = -1;
    private int m_LastIndex = -1;

    private Action<int, int> m_ToggleGroupEvent;

    public void AddToggle(GameObject toggleGo)
    {
        var toggle = toggleGo.GetOrAddComponent<ToggleObjects>();
        if (m_ToggleList.Contains(toggle))
        {
            return;
        }

        toggle.SetValue(false);
        toggle.AddListener();
        m_ToggleList.Add(toggle);
    }

    public void Initialize(Action<int, int> toggleEvent)
    {
        m_CurrentIndex = -1;
        m_LastIndex = -1;

        foreach (ToggleObjects obj in m_ToggleList)
        {
            obj.AddListener();
            obj.SetOnValueChange(HandleValueChange);
        }

        m_ToggleGroupEvent = toggleEvent;

        // 默认都不选
        foreach (ToggleObjects obj in m_ToggleList)
        {
            obj.SetValue(false);
        }
    }

    public void ForceHideAll()
    {
        if (m_CurrentIndex == -1)
        {
            return;
        }

        m_LastIndex = m_CurrentIndex;
        m_CurrentIndex = -1;

        foreach (ToggleObjects obj in m_ToggleList)
        {
            obj.SetValue(false);
        }
    }

    public void SetSelect(int index)
    {
        if (m_CurrentIndex == index)
        {
            m_ToggleGroupEvent?.Invoke(m_CurrentIndex, m_LastIndex);
            return;
        }

        m_LastIndex = m_CurrentIndex;
        m_CurrentIndex = index;


        if (index >= 0 && index < m_ToggleList.Count)
        {
            for (int i = 0; i < m_ToggleList.Count; i++)
            {
                m_ToggleList[i].SetValue(index == i);
            }
        }
    }

    public void Clear()
    {
        m_CurrentIndex = -1;
        m_LastIndex = -1;
        m_ToggleGroupEvent = null;
        foreach (var toggleObject in m_ToggleList)
        {
            toggleObject.RemoveListener();
        }
        m_ToggleList.Clear();
    }

    public void OnDisable()
    {
        Clear();
    }

    private void HandleValueChange(ToggleObjects toggle)
    {
        m_LastIndex = m_CurrentIndex;
        m_CurrentIndex = m_ToggleList.IndexOf(toggle);
        m_ToggleGroupEvent?.Invoke(m_CurrentIndex, m_LastIndex);
    }
}