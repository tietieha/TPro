using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TEngine
{
    [RequireComponent(typeof(RectTransform))]
    public class UIAdapter : MonoBehaviour
    {
        #if UNITY_EDITOR && ODIN_INSPECTOR
        [LabelText("是否两边都适配")]
        #endif
        public bool IsBothSides = false;
        
#if UNITY_EDITOR && ODIN_INSPECTOR
        [LabelText("启用测试模式")]
        public bool EnableTestMode = false;
        [LabelText("测试刘海平宽度")]
        public float TestWidth = 0;
        
        [LabelText("测试刘海平方向")]
        public TestOriEnum TestOrientation;
        public enum TestOriEnum
        {
            [LabelText("左侧")]
            Left = 0,
            [LabelText("右侧")]
            Right = 1,
        }
#endif
        private RectTransform _rectTransform;
        private ScreenOrientation _orientation;
        
        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            GameModule.UI.OnScreenOrientation += _onOrientation;
            
            ApplySafeArea();
        }
        
        private void ApplySafeArea()
        {
            _rectTransform.offsetMin = new Vector2(0, 0);
            _rectTransform.offsetMax = new Vector2(0, 0);
            
            var width = GetAreaWidth();
            if (IsBothSides)
            {
                _rectTransform.offsetMin = new Vector2(width, 0);
                _rectTransform.offsetMax = new Vector2(-width, 0);
                return;
            }
            
            var orientation = GetOrientation();
            if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.PortraitUpsideDown)
            {
                _rectTransform.offsetMin = new Vector2(width, 0);
                _rectTransform.offsetMax = new Vector2(0, 0);
            }
            else if(orientation == ScreenOrientation.LandscapeRight || orientation == ScreenOrientation.Portrait)
            {
                _rectTransform.offsetMin = new Vector2(0, 0);
                _rectTransform.offsetMax = new Vector2(-width, 0);
            }
        }

        private void _onOrientation()
        {
            ApplySafeArea();
        }
        
        private float GetAreaWidth()
        {
#if UNITY_EDITOR
            if (EnableTestMode)
            {
                return TestWidth;
            }
#endif

            var area = GameModule.UI.GetSafeArea();
            return area.x;
        }

        private ScreenOrientation GetOrientation()
        {
#if UNITY_EDITOR
            if (EnableTestMode)
            {
                if (TestOrientation == TestOriEnum.Left)
                {
                    return ScreenOrientation.LandscapeLeft;
                }
                else if (TestOrientation == TestOriEnum.Right)
                {
                    return ScreenOrientation.LandscapeRight;
                }
            }
#endif
            return GameModule.UI.Orientation;
        }
        
        private void OnDestroy()
        {
            // GameModule.UI.OnScreenOrientation -= _onOrientation;
        }
        
#if ODIN_INSPECTOR
        [Button("测试")]
        public void Test()
        {
            ApplySafeArea();
        }
#endif
    }
}
