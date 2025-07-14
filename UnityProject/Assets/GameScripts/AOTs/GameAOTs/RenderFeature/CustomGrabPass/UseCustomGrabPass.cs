
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RenderFeature
{
    [ExecuteInEditMode]
    public class UseCustomGrabPass : MonoBehaviour
    {
        [Header("场景内只会单次获取相机画面时可勾选(强制从0开始计算)")]
        public bool InitializeGrabTexIndex = false;
        private void OnEnable()
        {
            if (CustomGrabRenderFeature.Instance == null)
            {
                Debug.LogError(" CustomGrabRenderFeature.Instance == null ");
                gameObject.SetActive(false);
                return;
            }
            if (InitializeGrabTexIndex)
            {
                CustomGrabRenderFeature.Instance.InitializeGrabTexIndex();
            }
            CustomGrabRenderFeature.Instance.AddGrabIndex();
        }

        void OnDisable()
        {
            if (CustomGrabRenderFeature.Instance == null)
            {
                return;
            }
            CustomGrabRenderFeature.Instance.RemoveGrabIndex();
        }
    }
}