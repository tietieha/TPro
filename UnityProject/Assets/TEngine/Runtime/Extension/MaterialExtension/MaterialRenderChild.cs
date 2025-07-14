using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TEngine.MaterialExtension
{
    /// <summary>
    /// 材质管理子结构
    /// </summary>
    [Serializable]
    public class MaterialRenderChild
    {
        public Renderer MainRenderer;
        
        public List<MaterialKeyValue> MaterialMap = new List<MaterialKeyValue>();


        private MaterialKeyValue GetMaterialItem(string key)
        {
            for (int i = 0; i < MaterialMap.Count; i++)
            {
                if (MaterialMap[i].key == key)
                {
                    return MaterialMap[i];
                }
            }
            return null;
        }

        public void ChangeMatColorProperty(string propertyKey, float targetAlpha, float during, Action changeFinish = null)
        {
            if (MainRenderer == null)
            {
                Debug.LogError(" 主渲染对象是 null ");
                return;
            }

            foreach (var mat in MainRenderer.sharedMaterials)
            {
                Color c = mat.GetColor(propertyKey);
                c.a = 1;
                mat.SetColor(propertyKey,c);
                mat.DOColor(new Color(c.r,c.g,c.b,targetAlpha), propertyKey, during).OnComplete(() =>
                {
                    if (changeFinish != null)
                    {
                        changeFinish();
                    }
                });
            }
        }
        
        /// <summary>
        /// 切换材质球
        /// </summary>
        /// <param name="key"></param>
        public void ChangeMat(string key)
        {
            if (MainRenderer == null)
            {
                Debug.LogError(" 主渲染对象是 null ");
                return;
            }
            MaterialKeyValue item = GetMaterialItem(key);
            if (item== null)
            {
                Debug.LogWarning("key = "+key+" 没有找到 ");
                item = GetMaterialItem("default");
            }
            if (item != null)
            {
                MainRenderer.sharedMaterials = item.mats.ToArray();
            }
        }
    }
}