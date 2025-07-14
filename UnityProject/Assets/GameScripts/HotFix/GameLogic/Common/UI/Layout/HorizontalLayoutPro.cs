using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GameLogic
{
    public enum ProLayoutMode
    {
        OnlyX,
        OnlyY,
    }
    
    /// <summary>
    /// 多个不同物体之间可以进行水平排序
    /// </summary>
    public class HorizontalLayoutPro : MonoBehaviour
    {
        public List<RectTransform> children = new List<RectTransform>();
        public ProLayoutMode mode = ProLayoutMode.OnlyX;
        public float space = 0;
        public float updateTime = 0;
        
        private List<Vector2> _childrenSizes = new List<Vector2>();
        private float _currentUpdateTime = 0;
        private void Awake()
        {
            _currentUpdateTime = updateTime;
            //开始的时候存一个子对象的尺寸大小
            foreach (var child in children)
            {
                _childrenSizes.Add(child.sizeDelta);
            }
        }

        public void ResetUpdateTime()
        {
            _currentUpdateTime = updateTime;
        }

        public void SetChildLayoutHorizontal()
        {
            for (int i = 0; i < children.Count-1; i++)
            {
                RectTransform currentRect = children[i];
                RectTransform nextRect = children[i+1];
                switch (mode)
                {
                    case ProLayoutMode.OnlyX:
                        UpdateOnlyX(currentRect,nextRect);
                        break;
                    case ProLayoutMode.OnlyY:
                        UpdateOnlyY(currentRect,nextRect);
                        break;
                }
            }
        }

        private void Update()
        {
            if (_currentUpdateTime > 0)
            {
                _currentUpdateTime -= Time.deltaTime;
                if (_currentUpdateTime <= 0)
                {
                    if (IsChanged())
                    {
                        SetChildLayoutHorizontal();
                    }
                    _currentUpdateTime = updateTime;
                }
            }
        }

        private bool IsChanged()
        {
            for (int i = 0; i < _childrenSizes.Count; i++)
            {
                RectTransform childRect = children[i];
                Vector2 childSize = childRect.sizeDelta;
                Vector2 targetSize = _childrenSizes[i];
                if (!Mathf.Approximately(targetSize.x, childSize.x) || !Mathf.Approximately(targetSize.y, childSize.y))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateOnlyX(RectTransform currentRect, RectTransform nextRect)
        {
            if (nextRect == null)
                return;
            
            Vector2 pos = nextRect.anchoredPosition;
            pos.x = currentRect.anchoredPosition.x + currentRect.sizeDelta.x + space;
            nextRect.anchoredPosition = pos;
        }
        
        private void UpdateOnlyY(RectTransform currentRect, RectTransform nextRect)
        {
            if (nextRect == null)
                return;
            
            Vector2 pos = nextRect.anchoredPosition;
            pos.y = currentRect.anchoredPosition.y + currentRect.sizeDelta.y + space;
            nextRect.anchoredPosition = pos;
        }

        [ContextMenu("测试布局")]
        public void TestLayout()
        {
            SetChildLayoutHorizontal();
        }
    }
}
