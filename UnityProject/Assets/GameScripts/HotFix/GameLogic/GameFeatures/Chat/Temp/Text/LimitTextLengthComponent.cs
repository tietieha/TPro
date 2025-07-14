/***************************************************************************************************************************
 * @说    明: 对文本字符串的长度进行限制,超出部分替换为"...";
 * @作    者: zhoumingfeng
 * @版 本 号: V1.01
 * @创建时间: 2019-11-27
 * @修改记录: 2019-12-31 可在监视器上设置限制文本的像素宽度,如果设置值为小于等于0的值,那么将以RectTransform的宽度为准
 *
 * 注意：富文本处理不能简单判断长度大小，需要先将标签去掉计算长度，然后截取子串再将原有标签加上
 * 1、作为底层组件首先要检查富文本格式是否正确，比如 <color=red>XXX <color> 标签错误就不能无脑处理，此时就得原样输出
 * 2、如果富文本格式正确，那么就先将标签去掉并计算位置及长度，需要截取子串后再计算将原有标签加上
****************************************************************************************************************************/

using System;
using TEngine;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

[RequireComponent(typeof(Text))]
public class LimitTextLengthComponent : MonoBehaviour
{
    protected Text text;

    private RichLabelRanges richLabelRanges = new RichLabelRanges();

    [SerializeField]
    private int width = -1;

    // Use this for initialization
    private void Awake()
    {
        this.Init();
    }

    private void Start()
    {
        
    }

    public void SetWidth(int _width)
    {
        this.width = _width;
    }

    private void Init()
    {
        text = this.GetComponent<Text>();

        if (width <= 0)
        {
            RectTransform rectTransform = this.GetComponent<RectTransform>();
            if (null != rectTransform)
            {
                width = Mathf.CeilToInt(rectTransform.sizeDelta.x);

                if(width <= 0)
                    width = Mathf.CeilToInt(rectTransform.rect.width);
            }
        }

        if (null != text)
            text.RegisterDirtyLayoutCallback(OnTextLayoutChange);
    }

    private void OnTextLayoutChange()
    {
        return;
        text.text = this.StringEllipsis(text, width);
    }
    
    /// <summary>
    /// 根据给定的宽度截断字符串。并将给定的后缀拼接到截断后的字符串。
    /// </summary>
    private string StringEllipsis(Text text, int maxWidth, string suffix = "...")
    {
        if (null == text)
            return string.Empty;

        if (string.IsNullOrEmpty(text.text))
            return string.Empty;
        
        if (text.supportRichText)
        {
            richLabelRanges.InitRangeInfos(text.text);
        }

        int textLeng = GetTextLeng(text);

        if (textLeng > maxWidth)
        {
            int suffixLeng = GetTextLeng(text, suffix);
            return StripLength(text, maxWidth - suffixLeng) + suffix;
        }
        else
        {
            return text.text;
        }
    }


    /// 按照指定宽度截断字符串
    private string StripLength(Text text, int width)
    {
        int totalLength = 0;
        Font myFont = text.font;
        string originStr = text.text;
        myFont.RequestCharactersInTexture(originStr, text.fontSize, text.fontStyle);

        char[] charArr = originStr.ToCharArray();

        int i = 0;
        for (; i < charArr.Length; i++)
        {
            if(richLabelRanges.IsInTagRange(i))
                continue;
            myFont.GetCharacterInfo(charArr[i], out var characterInfo, text.fontSize);

            int newLength = totalLength + characterInfo.advance;
            if (newLength > width)
                  break;
            totalLength += characterInfo.advance;
        }
        
        //如果把富文本给裁剪了 那么要把富文本的后缀标签加回来
        var backLabel = richLabelRanges.GetBackLabel(i);
        if (!backLabel.IsNullOrEmpty())
        {
            return originStr.Substring(0, i) + backLabel;
        }

        return originStr.Substring(0, i);
    }

    /// <summary>
    /// 获取字符串在text中的长度
    /// </summary>
    /// <param name="text"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    private int GetTextLeng(Text text, string str = null)
    {
        Font mFont = text.font;
        string tempStr;
        if (text.supportRichText && richLabelRanges.Count > 0)
        {
            //匹配关于颜色的富文本
            tempStr = RichLabelUtils.RemoveLabels(text.text);
        }
        else
        {
            tempStr = text.text;
        }

        string mStr = string.IsNullOrEmpty(str) ? tempStr: str;

        if (string.IsNullOrEmpty(mStr))
            return 0;

        try
        {
            mFont.RequestCharactersInTexture(mStr, text.fontSize, text.fontStyle);
        }
        catch(Exception e)
        {
            Log.Error("Font.RequestCharactersInTexture string {0} throw exception, error:{1}", string.IsNullOrEmpty(str) ? "" : str, e == null ? "" : e.Message);
        }

        char[] charArr = mStr.ToCharArray();
        int totalTextLeng = 0;
        CharacterInfo character = new CharacterInfo();
        for (int i = 0; i < charArr.Length; i++)
        {
            mFont.GetCharacterInfo(charArr[i], out character, text.fontSize);
            totalTextLeng += character.advance;
        }
        return totalTextLeng;
    }

#if UNITY_EDITOR

    #region UNITY_EDITOR 模式下的代码

#if ODIN_INSPECTOR
    [Button("测试", ButtonSizes.Large)]
#endif
    private void Rebuild()
    {
        this.DoRebuild();
    }

    /// <summary>
    /// 加载特效文件
    /// </summary>
    private void DoRebuild()
    {
        this.Init();

        if(null != this.text)
            this.text.OnRebuildRequested();
    }

    #endregion

#endif
}
