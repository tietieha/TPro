using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

public static class UIExtension
{
    public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration,
        Action callback = null)
    {
        float time = 0f;
        float originalAlpha = canvasGroup.alpha;
        while (time < duration)
        {
            time += GameTime.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = alpha;

        callback?.Invoke();
    }

    public static IEnumerator SmoothValue(this Slider slider, float value, float duration, Action callback = null)
    {
        float time = 0f;
        float originalValue = slider.value;
        while (time < duration)
        {
            time += GameTime.deltaTime;
            slider.value = Mathf.Lerp(originalValue, value, time / duration);
            yield return new WaitForEndOfFrame();
        }

        slider.value = value;

        callback?.Invoke();
    }

    public static IEnumerator SmoothValue(this Scrollbar slider, float value, float duration, Action callback = null)
    {
        float time = 0f;
        float originalValue = slider.size;
        while (time < duration)
        {
            time += GameTime.deltaTime;
            slider.size = Mathf.Lerp(originalValue, value, time / duration);
            yield return new WaitForEndOfFrame();
        }

        slider.size = value;

        callback?.Invoke();
    }

    public static IEnumerator SmoothValue(this Image image, float value, float duration, Action callback = null)
    {
        float time = 0f;
        float originalValue = image.fillAmount;
        while (time < duration)
        {
            time += GameTime.deltaTime;
            image.fillAmount = Mathf.Lerp(originalValue, value, time / duration);
            yield return new WaitForEndOfFrame();
        }

        image.fillAmount = value;

        callback?.Invoke();
    }

    public static bool GetMouseDownUiPos(this UIModule uiModule, out Vector3 screenPos)
    {
        bool hadMouseDown = false;
        Vector3 mousePos = Vector3.zero;

#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        mousePos = Input.mousePosition;
        hadMouseDown = Input.GetMouseButton(0);
#else
        if (Input.touchCount > 0)
        {
            mousePos = Input.GetTouch(0).position;
            hadMouseDown = true;
        }
        else
        {
            hadMouseDown = false;
        }
#endif

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiModule.UIRoot as RectTransform,
            mousePos,
            uiModule.UICamera, out var pos);
        screenPos = uiModule.UIRoot.TransformPoint(pos);

        return hadMouseDown;
    }
    
    /// <summary>
    /// 对字符串加自定义颜色格式
    /// </summary>
    /// <param name="desc"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ToColor(this string desc, string color)
    {
        return $"<color=#{color}>{desc}</color>";
    }

    public static void SetAnchoredPositionX(this RectTransform rectTransform, float anchoredPositionX)
    {
        var value = rectTransform.anchoredPosition;
        value.x = anchoredPositionX;
        rectTransform.anchoredPosition = value;
    }
    public static void SetAnchoredPositionY(this RectTransform rectTransform, float anchoredPositionY)
    {
        var value = rectTransform.anchoredPosition;
        value.y = anchoredPositionY;
        rectTransform.anchoredPosition = value;
    }
    public static void SetAnchoredPosition3DZ(this RectTransform rectTransform, float anchoredPositionZ)
    {
        var value = rectTransform.anchoredPosition3D;
        value.z = anchoredPositionZ;
        rectTransform.anchoredPosition3D = value;
    }

    public static void SetColorAlpha(this UnityEngine.UI.Graphic graphic, float alpha)
    {
        var value = graphic.color;
        value.a = alpha;
        graphic.color = value;
    }
    public static void SetFlexibleSize(this LayoutElement layoutElement, Vector2 flexibleSize)
    {
        layoutElement.flexibleWidth = flexibleSize.x;
        layoutElement.flexibleHeight = flexibleSize.y;
    }
    public static Vector2 GetFlexibleSize(this LayoutElement layoutElement)
    {
        return new Vector2(layoutElement.flexibleWidth, layoutElement.flexibleHeight);
    }
    public static void SetMinSize(this LayoutElement layoutElement, Vector2 size)
    {
        layoutElement.minWidth = size.x;
        layoutElement.minHeight = size.y;
    }
    public static Vector2 GetMinSize(this LayoutElement layoutElement)
    {
        return new Vector2(layoutElement.minWidth, layoutElement.minHeight);
    }
    public static void SetPreferredSize(this LayoutElement layoutElement, Vector2 size)
    {
        layoutElement.preferredWidth = size.x;
        layoutElement.preferredHeight = size.y;
    }
    public static Vector2 GetPreferredSize(this LayoutElement layoutElement)
    {
        return new Vector2(layoutElement.preferredWidth, layoutElement.preferredHeight);
    }
}