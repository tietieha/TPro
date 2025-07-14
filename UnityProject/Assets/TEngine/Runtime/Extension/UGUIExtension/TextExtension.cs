/********************************************************************************
* @说    明: 文本类组件的扩展接口
* @作    者: zhoumingfeng
* @版 本 号: V1.00
********************************************************************************/

using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 文本组件扩展。
/// </summary>

public static class TextExtension
{
    #region TextMeshPro Extension

    public static void SetTextStyle(this TMP_Text tmp, string stylename)
    {
	    var style = TMP_Settings.defaultStyleSheet.GetStyle(stylename);
	    tmp.textStyle = style;
    }

    public static void SetText(this Text text,string content)
    {
        if(string.IsNullOrEmpty(content))
        {
            text.text = "";
            return;
        }

        text.text = content;
    }

    public static void SetTextEx(this TMP_Text tmp, string content)
    {
        if(string.IsNullOrEmpty(content))
        {
            tmp.SetText("");
            return;
        }

        string text = content;
        // if (LocalizationProxy.Instance.Language == Language.Arabic)
        // {// 阿拉伯语从右往左读, 需要将文本反转一下
        //     // 查找是否存在自适应文本宽度的组件,否则需要自行控制换行
        //     // var fitter = tmp.GetComponent<ContentSizeFitter>();
        //     // if (null == fitter || fitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
        //     // {
        //     //     var arabic = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        //     //     tmp.SetArabicTextSupportWrap(arabic);
        //     // }
        //     // else
        //     // {
        //     //     text = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        //     //     tmp.SetText(text);
        //     // }
        //
        //     // 设置文本右对齐
        //     //tmp.SetAlignmentForArabic();
        // }
        // else
        {
            tmp.SetText(text);
        }
    }

    // public static void SetTextEx(this TMP_Text tmp, params object[] args)
    // {
    //     string result = "";
    //     string refDialogId = "";
    //     for (int i = 0; i < args.Length; i++)
    //     {
    //         if (args[i] is string)
    //         {
    //             result += args[i];
    //         }
    //         else if (args[i] is int || args[i] is long || args[i] is double)
    //         {
    //             result += args[i].ToString();
    //         }
    //         // else if (args[i] is LuaTable)
    //         // {
    //         //     result += UnityExtension.ParseLuaTable(args[i], ref refDialogId);
    //         // }
    //         // else
    //         // {
    //         //     result += UnityExtension.ParseTuple(args[i], ref refDialogId);
    //         // }
    //     }
    //
    //     tmp.SetTextEx(result);
    // }

    public static void SetTextEx(this TMP_InputField inputField, string content)
    {
        // if (GameEntry.Localization.Language == Language.Arabic)
        //     inputField.text = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        // else
            inputField.text = content;
    }

    /// <summary>
    /// 按照阿拉伯语规则设置文本
    /// </summary>
    /// <param name="tmp"></param>
    /// <param name="content"></param>
    public static void SetArabicText(this TMP_Text tmp, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            tmp.SetText("");
            return;
        }

        // string text = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        tmp.SetText(content);
    }

    public static void SetArabicTextSupportWrap(this TMP_Text tmp, string arabic)
    {
        if (string.IsNullOrEmpty(arabic))
        {
            tmp.SetText("");
            return;
        }

        if (tmp.enableWordWrapping)
        {// 换行处理
            var rect = tmp.GetComponent<RectTransform>();
            float width = rect.sizeDelta.x;
            float length = tmp.GetTextLeng(arabic);
            if (width > 0 && width < length)
            {
                string content = arabic;

                // RichLabelRanges richLabelRanges = new RichLabelRanges();
                // richLabelRanges.InitRangeInfos(content);
                // if (tmp.richText && richLabelRanges.Count > 0)
                //     content = RichLabelUtils.RemoveLabels(content);

                if (content.Contains("\n"))
                    content = content.Replace("\n", Environment.NewLine);

                if (content.Contains("\r\r\n"))
                    content = content.Replace("\r\r\n", Environment.NewLine);

                if (content.Contains(Environment.NewLine))
                {
                    string[] stringSeparators = new string[] { Environment.NewLine };
                    string[] strLine = content.Split(stringSeparators, StringSplitOptions.None);

                    string arabicLine = tmp.GetArabicLineTextFor(width, strLine[0]);
                    for (int i = 1; i < strLine.Length; i++)
                        arabicLine += Environment.NewLine + tmp.GetArabicLineTextFor(width, strLine[i]);

                    tmp.SetText(arabicLine);
                }
                else
                {
                    var array = content.Split(new char[] { ' ' });
                    if (array.Length <= 1)
                        tmp.text = arabic;
                    else
                        tmp.SetText(tmp.GetArabicLineTextFor(width, content));
                }
            }
            else
            {
                tmp.SetText(arabic);
            }
        }
        else
        {
            tmp.SetText(arabic);
        }
    }

    public static string GetArabicLineTextFor(this TMP_Text text, float width, string content = null)
    {
        // split之后是反序的
        var array = content.Split(new char[] { ' ' });
        if (array.Length <= 1)
            return content;

        StringBuilder stringBuilder = new StringBuilder();

        int start = array.Length - 1;
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(array[i]))
            {
                start = i;
                stringBuilder.Append(array[i]);

                break;
            }
        }

        float length = 0;
        string part = stringBuilder.ToString();
        for (int i = start - 1; i >= 0; i--)
        {
            if (string.IsNullOrEmpty(array[i]))
                continue;

            part += " ";
            stringBuilder.Append(" ");

            if (i > 0)
                length = text.GetTextLeng(part + " " + array[i] + " ");
            else
                length = text.GetTextLeng(part + " " + array[i]);

            // 如果长度超过了文本组件宽度,那么添加换行符
            if (length > width)
            {
                part = string.Empty;

                stringBuilder.Append(Environment.NewLine);
            }

            part += array[i];
            stringBuilder.Append(array[i]);
        }

        string temp = stringBuilder.ToString();
        string[] stringSeparators = new string[] { Environment.NewLine };
        string[] strSplit = temp.Split(stringSeparators, StringSplitOptions.None);

        stringBuilder.Clear();
        for (int i = 0; i < strSplit.Length; i++)
        {
            var tmpArray = strSplit[i].Split(new char[] { ' ' });
            if (tmpArray.Length <= 1)
            {
                stringBuilder.Append(tmpArray[0]);
            }
            else
            {
                for (int j = tmpArray.Length - 1; j >= 0; j--)
                {
                    if (string.IsNullOrEmpty(tmpArray[j]))
                        continue;

                    stringBuilder.Append(tmpArray[j]);

                    if (j > 0)
                        stringBuilder.Append(" ");
                }
            }

            if (i < strSplit.Length - 1)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(" ");
            }
        }

        return stringBuilder.ToString();
    }

    public static float GetTextLeng(this TMP_Text text, string str = null)
    {
        string content = string.IsNullOrEmpty(str) ? text.text : str;
        if (string.IsNullOrEmpty(content))
            return 0;

        // RichLabelRanges richLabelRanges = new RichLabelRanges();
        // richLabelRanges.InitRangeInfos(content);
        // if (text.richText && richLabelRanges.Count > 0)
        //     content = RichLabelUtils.RemoveLabels(content);

        TMP_TextInfo info;
        
        try
        {
            info = text.GetTextInfo(content);
        }
        catch (Exception e)
        {
            // Log.Error("TMP_Text.GetTextInfo {0} throw exception, error:{1}", string.IsNullOrEmpty(str) ? "" : str, e == null ? "" : e.Message);
            return 0;
        }

        float totalTextLeng = 0;
        for(int i = 0; i < info.characterCount; i++)
            totalTextLeng += Mathf.Abs(info.characterInfo[i].topLeft.x - info.characterInfo[i].topRight.x);

        return totalTextLeng;
    }

    /// <summary>
    /// 为阿拉伯语设置文字对齐方式
    /// 由于阿拉伯语是从右往左阅读, 左对齐需要改成右对齐
    /// </summary>
    public static void SetAlignmentForArabic(this TMP_Text text)
    {
        // if (GameEntry.Localization.Language != Language.Arabic)
        //     return;

        if (text.alignment == TextAlignmentOptions.MidlineLeft)
            text.alignment = TextAlignmentOptions.MidlineRight;
        else if (text.alignment == TextAlignmentOptions.BottomLeft)
            text.alignment = TextAlignmentOptions.BottomRight;
        else if (text.alignment == TextAlignmentOptions.TopLeft)
            text.alignment = TextAlignmentOptions.TopRight;
        else if (text.alignment == TextAlignmentOptions.Left)
            text.alignment = TextAlignmentOptions.Right;
    }

    public static void SetAlignmentForArabic(this TMP_InputField inputField)
    {
        if (null == inputField)
            return;

        // if (GameEntry.Localization.Language != Language.Arabic)
        //     return;

        if (inputField.textComponent.alignment == TextAlignmentOptions.MidlineLeft)
            inputField.textComponent.alignment = TextAlignmentOptions.MidlineRight;
        else if (inputField.textComponent.alignment == TextAlignmentOptions.BottomLeft)
            inputField.textComponent.alignment = TextAlignmentOptions.BottomRight;
        else if (inputField.textComponent.alignment == TextAlignmentOptions.TopLeft)
            inputField.textComponent.alignment = TextAlignmentOptions.TopRight;
        else if (inputField.textComponent.alignment == TextAlignmentOptions.Left)
            inputField.textComponent.alignment = TextAlignmentOptions.Right;
    }

    #endregion TextMeshPro Extension

    #region InputField Extension

    public static void SetTextEx(this InputField tmp, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            tmp.text = "";
            return;
        }

        string text = content;
        // if (GameEntry.Localization.Language == Language.Arabic)
        // {// 阿拉伯语从右往左读, 需要将文本反转一下
        //     text = ArabicSupport.ArabicFixer.FixEx(content, true, false);
        //
        //     // 设置文本右对齐
        //     //tmp.SetAlignmentForArabic();
        // }

        tmp.text = text;
    }

    public static void SetAlignmentForArabic(this InputField input)
    {
        if (null == input)
            return;

        // if (GameEntry.Localization.Language != Language.Arabic)
        //     return;

        if (input.textComponent.alignment == TextAnchor.MiddleLeft)
            input.textComponent.alignment = TextAnchor.MiddleRight;
        else if (input.textComponent.alignment == TextAnchor.LowerLeft)
            input.textComponent.alignment = TextAnchor.LowerRight;
        else if (input.textComponent.alignment == TextAnchor.UpperLeft)
            input.textComponent.alignment = TextAnchor.UpperRight;
    }

    #endregion InputField Extension
}
