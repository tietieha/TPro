
using UnityEngine;
using TEngine;

public class WorldCityColor
{
    public int id;
    public Color outlineColor;
    public Color innerColor;
    public float innerIntensity;
    public Color baseColor;
    public float baseAlpha;
    public int splashIndex;
    public int splashX;
    public int splashY;
    public int splashRot;
    public float splashAlpha;
    public Color groundColor;

    public static Color ConvertToColor(string str)
    {
        if (!str.IsNullOrEmpty())
        {
            if (str[0] != '#')
                str = "#" + str;

            if (ColorUtility.TryParseHtmlString(str, out Color color))
                return color;
        }

        return Color.black;
    }

    public WorldCityColor()
    {
        id = 1;
        innerIntensity = 1;
        baseAlpha = 0.5f;
        splashX = 15;
        splashY = 1;
        splashRot = 30;
        splashAlpha = 0;
        
        splashIndex = 1;
        if (splashIndex > 0) splashIndex -= 1;
        
        // outlineColor = ConvertToColor("e0a743");
        // innerColor = ConvertToColor("945525");
        // baseColor = ConvertToColor("d9983a");
        // groundColor = ConvertToColor("ffb961");
        
        outlineColor = Color.white;
        innerColor = Color.white;
        baseColor =Color.white;
        groundColor = Color.white;
    }
}