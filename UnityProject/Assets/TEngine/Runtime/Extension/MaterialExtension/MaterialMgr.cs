using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TEngine.MaterialExtension
{
    public class MaterialMgr : MonoBehaviour
    {
        public List<MaterialRenderChild> ChildrenList = new List<MaterialRenderChild>();
        public string TestKey = "";
        [Button("切换材质")]
        void TestChangeMat()
        {
            ChangeMat(TestKey);
        }

        public void ChangeMat( string key )
        {
            for (int i = 0; i < ChildrenList.Count; i++)
            {
                ChildrenList[i].ChangeMat(key);
            }
        }

        public void ChangeMatColorProperty(string propertyKey, float targetAlpha,float during,Action changeFinish = null)
        {
            for (int i = 0; i < ChildrenList.Count; i++)
            {
                ChildrenList[i].ChangeMatColorProperty(propertyKey,targetAlpha,during,changeFinish);
            }
        }
    }
}