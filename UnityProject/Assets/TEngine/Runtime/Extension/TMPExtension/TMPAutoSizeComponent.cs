using System;
using TMPro;
using UnityEngine;

namespace UW
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform), typeof(TextMeshProUGUI))]
    public class TMPAutoSizeComponent : MonoBehaviour
    {
        // 实现根据字体宽度，自适应rectTransform 宽度
        public float minWidth = 0; // 最小宽度
        public float maxWidth = 0; // 最大宽度
        public bool useTextWidth = false;
        
        private TextMeshProUGUI _textMeshPro; // TMP 文本组件
        private RectTransform _rectTransform; // RectTransform 组件
        private string _textValue = String.Empty;
        
        
        void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            
            AutoSetWidth();
        }

        void Update()
        {
            if (_textValue == _textMeshPro.text)
            {
                return;
            }

            AutoSetWidth();
        }

        private void AutoSetWidth()
        {
            _textValue = _textMeshPro.text;
            // 获取内容的实际宽度
            float preferredWidth = _textMeshPro.GetPreferredValues().x;
            if (useTextWidth)
            {
                // 设置 RectTransform 的宽度
                _rectTransform.sizeDelta = new Vector2(preferredWidth, _rectTransform.sizeDelta.y);
                return;
            }
            
            // 限制宽度范围
            float clampedWidth = Mathf.Clamp(preferredWidth, minWidth, maxWidth);
            // 设置 RectTransform 的宽度
            _rectTransform.sizeDelta = new Vector2(clampedWidth, _rectTransform.sizeDelta.y);
        }
    }
}