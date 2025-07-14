// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-04-07       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using TEngine;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using XLua;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
[LuaCallCSharp]
public class UIProgressBar : MonoBehaviour
{
    private static int S_ProgressProperty = Shader.PropertyToID("_Progress");
    private static int S_SpriteUVProperty = Shader.PropertyToID("_SpriteUV");
    private static Shader S_BarShader;

    private Material m_BarMat;
    private Image m_BarImage;
    private void OnEnable()
    {
        m_BarImage = GetComponent<Image>();
        if (m_BarImage == null)
        {
            Debug.LogError("UIProgressBar: Image component not found.");
            return;
        }

        if (S_BarShader == null)
        {
            if (Application.isPlaying)
                S_BarShader = GameModule.Resource.LoadAsset<Shader>("UI-ProgressBar");
            else
                S_BarShader = Shader.Find("UI/ProgressBar");
        }

        if (m_BarMat == null)
            m_BarMat = new Material(S_BarShader);
        m_BarImage.material = m_BarMat;

        Vector4 outUV = DataUtility.GetOuterUV(m_BarImage.sprite);
        m_BarMat.SetVector(S_SpriteUVProperty, outUV);
    }

    private void OnDestroy()
    {
        if (m_BarMat != null)
            if (Application.isEditor)
                DestroyImmediate(m_BarMat);
            else
                Destroy(m_BarMat);
    }

    public void SetProgress(float progress)
    {
        if (m_BarMat != null)
            m_BarMat.SetFloat(S_ProgressProperty, progress);
    }
}