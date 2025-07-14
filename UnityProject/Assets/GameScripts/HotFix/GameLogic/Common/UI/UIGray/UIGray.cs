using System.Collections.Generic;
using TEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI置灰
/// </summary>
public class UIGray
{
    //灰色材质
    private static Material grayMat;
    //置灰前缓存的颜色，用于恢复后还原
    private static Dictionary<GameObject, Color> colors = new Dictionary<GameObject, Color>();

    /// <summary>
    /// 获得灰色材质
    /// </summary>
    private static Material GetGrayMat()
    {
        if (grayMat == null)
        {
            Shader shader = GameModule.Resource.LoadAsset<Shader>("UI-ImageGray");
            if (shader == null)
            {
                Debug.Log("can not found ImageGray shader...");
                return null;
            }
            grayMat = new Material(shader);
        }
        return grayMat;
    }

    /// <summary>
    /// 置灰
    /// </summary>
    public static void SetGray(GameObject go, bool isGray)
    {
        if (go == null) return;

        var images = go.transform.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            if (isGray)
            {
                image.material = GetGrayMat();
                image.SetMaterialDirty();
            }
            else
            {
                image.material = null;
            }
        }

        var texts = go.transform.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            if (isGray)
            {
                if (!colors.ContainsKey(text.gameObject))
                {
                    colors.Add(text.gameObject, text.color);
                    text.color = new Color(0.364f, 0.364f, 0.364f, 1);
                }
            }
            else
            {
                if (colors.ContainsKey(text.gameObject))
                {
                    text.color = colors[text.gameObject];
                    colors.Remove(text.gameObject);
                }
            }
        }

        var proTexts = go.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var proText in proTexts)
        {
            if (isGray)
            {
                if (!colors.ContainsKey(proText.gameObject))
                {
                    colors.Add(proText.gameObject, proText.color);
                    proText.color = new Color(0.364f, 0.364f, 0.364f, 1);
                }
            }
            else
            {
                if (colors.ContainsKey(proText.gameObject))
                {
                    proText.color = colors[proText.gameObject];
                    colors.Remove(proText.gameObject);
                }
            }
        }
    }

    public static void SetGray(GameObject go, bool isGray, Color color)
    {
        if (go == null) return;
        
        var images = go.transform.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            if (isGray)
            {
                image.material = GetGrayMat();
                image.SetMaterialDirty();
            }
            else
            {
                image.material = null;
            }
        }

        var texts = go.transform.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            if (isGray)
            {
                if (!colors.ContainsKey(text.gameObject))
                {
                    colors.Add(text.gameObject, text.color);
                    text.color = color; //new Color(0.25f, 0.25f, 0.25f, 1);
                }
            }
            else
            {
                if (colors.ContainsKey(text.gameObject))
                {
                    text.color = colors[text.gameObject];
                    colors.Remove(text.gameObject);
                }
            }
        }

        var proTexts = go.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var proText in proTexts)
        {
            if (isGray)
            {
                if (!colors.ContainsKey(proText.gameObject))
                {
                    colors.Add(proText.gameObject, proText.color);
                    proText.color = color;//new Color(0.25f, 0.25f, 0.25f, 1);
                }
            }
            else
            {
                if (colors.ContainsKey(proText.gameObject))
                {
                    proText.color = colors[proText.gameObject];
                    colors.Remove(proText.gameObject);
                }
            }
        }
    }
}
