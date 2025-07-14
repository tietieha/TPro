using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace TMPro
{
    [CreateAssetMenu(fileName = "UITextStyleAsset", menuName = "UIText/TextStyleAsset", order = 0)]
    public class UITextStyleAsset : ScriptableObject
    {
        [System.Serializable]
        public class UITextStyleParam
        {
            public string StyleName;
            // 字色
            public Color m_FontColor = Color.white;
            
            // 字体
            public float m_FontSize = 14;
            
            // 加粗
            [Header("加粗设置")] public BoldMode BoldType = BoldMode.adaptation;

            // 描边
            [Header("开启描边")] public bool useOutLine = false;
            [Range(0, 10)] public float m_OutLine = 0.35f;
            public Color m_OutLineColor = new Color(0, 0, 0, 1);
            
            //阴影
            [Header("是否开启阴影")] public bool useUnderlayColor = false;
            public Color m_UnderlayColor = new Color(0, 0, 0, 1);
            [Range(-10, 10)] public float m_UnderlayOffsetX = 0;
            [Range(-10, 10)] public float m_UnderlayOffsetY = 0;
            [Range(-1, 1)] public float m_UnderlayDilate = 0;
            [Range(0, 1)] public float m_UnderlaySoftness = 0;
        }
        
#if UNITY_EDITOR && ODIN_INSPECTOR
        [DictionaryDrawerSettings(KeyLabel = "字体样式", ValueLabel = "样式参数")]
        [ShowInInspector]
#endif
        public List<UITextStyleParam> StyleParams = new List<UITextStyleParam>();
        
        public UITextStyleParam GetStyleParam(string styleName)
        {
            foreach (var value in StyleParams)
            {
                if (value.StyleName == styleName)
                {
                    return value;
                }
            }
            return null;
        }
        
        public void AddStyleParam(UITextStyleParam param)
        {
            for (int i = 0; i < StyleParams.Count; i++)
            {
                if (StyleParams[i].StyleName == param.StyleName)
                {
                    return;
                }
            }

            StyleParams.Add(param);
        }

        public string[] GetStyleNames()
        {
            string[] styleNames = new string[StyleParams.Count];
            for (int i = 0; i < StyleParams.Count; i++)
            {
                styleNames[i] = StyleParams[i].StyleName;
            }

            return styleNames;
        }

#if UNITY_EDITOR && ODIN_INSPECTOR_3
        [Button("保存")]
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}