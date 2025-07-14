using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMultiScrollIndex : MonoBehaviour
{
    [SerializeField]
    public GameObject nodeCheckedFrame;
    [SerializeField]
    public List<TextMeshProUGUI> nodeCheckedText;

    [SerializeField] public Color selectColor = Color.white;
    [SerializeField] public Color unSelectColor = Color.white;
    
    private UIMultiScroller _scroller;
    protected int _index;
    
    private void Start()
    {
        
    }

    public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
            transform.localPosition = _scroller.GetPosition(_index);
            gameObject.name = "Scroll" + (_index < 10 ? "0" + _index : _index.ToString());
        }
    }

    public UIMultiScroller Scroller
    {
        set { _scroller = value; }
    }

    /// <summary>
    /// 是否选中Item,选中时通知其他Item刷新状态
    /// </summary>
    /// <param name="check">true:显示选中框 false:隐藏选中框</param>
    /// <param name="notifyRefresh">true:通知其他Item刷新状态 false:不通知</param>
    public void Check(bool check, bool notifyRefresh = true)
    {
        if (check)
        {
            if (notifyRefresh)
                this.CheckedAndNotify();
            else
                this.Checked();
        }
        else
        {
            this.Unchecked();
        }
    }

    /// <summary>
    /// 选中(不会通知其他项刷新状态)
    /// </summary>
    public void Checked()
    {
        if (null != this.nodeCheckedFrame)
        {
            this.nodeCheckedFrame.SetActiveEx(true);
        }

        if (null != this.nodeCheckedText && nodeCheckedText.Count>0)
        {
            for (int i = 0; i < nodeCheckedText.Count; i++)
            {
                nodeCheckedText[i].color = selectColor;
            }
        }
        
    }

    /// <summary>
    /// 选中并通知列表刷新其他项
    /// </summary>
    public void CheckedAndNotify()
    {
        if (null != this.nodeCheckedFrame)
        {
            this.nodeCheckedFrame.SetActiveEx(true);
        }

        if (null != this.nodeCheckedText && nodeCheckedText.Count>0)
        {
            for (int i = 0; i < nodeCheckedText.Count; i++)
            {
                nodeCheckedText[i].color = selectColor;
            }
        }
        if(null != this._scroller)
            this._scroller.NotifyRefreshCheckState(this);
    }

    /// <summary>
    /// 取消选中
    /// </summary>
    public void Unchecked()
    {
        
        if (null != this.nodeCheckedFrame)
        {
            this.nodeCheckedFrame.SetActiveEx(false);
        }

        if (null != this.nodeCheckedText && nodeCheckedText.Count>0)
        {
            for (int i = 0; i < nodeCheckedText.Count; i++)
            {
                nodeCheckedText[i].color = unSelectColor;
            }
        }
        
    }

    /// <summary>
    /// 是否为选中状态
    /// </summary>
    /// <returns></returns>
    public bool IsChecked()
    {
        if (null != this.nodeCheckedFrame)
            return this.nodeCheckedFrame.activeSelf;

        return false;
    }

    /// <summary>
    /// 选中状态是否生效
    /// </summary>
    /// <returns></returns>
    public bool IsCheckEnable()
    {
        return null != this.nodeCheckedFrame;
    }
}