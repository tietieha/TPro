using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// UI 工具 [布局组]
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIToolsLayoutGroup : MonoBehaviour
    {
        [Serializable]
        public struct Padding
        {
            public float Left;
            public float Right;
            public float Top;
            public float Bottom;
        }
        /// <summary>
        /// [边距设置]
        /// </summary>
        [Tooltip("边距设置")] [SerializeField]
        private Padding _padding = new Padding();
        /// <summary>
        /// [间隔]
        /// </summary>
        [Tooltip("间隔")] [SerializeField] private float _spacing = 0.0f;

        /// <summary>
        /// [间隔]
        /// </summary>
        public float Spacing
        {
            get => _spacing;
            set => _spacing = value;
        }

        [SerializeField]
        private UIToolsConfig.Direction _direction = UIToolsConfig.Direction.Horizontal;

        /// <summary>
        /// [对齐方式]
        /// </summary>
        [Tooltip("对齐方式")] [SerializeField]
        private TextAnchor _alignment = TextAnchor.UpperLeft;

        /// <summary>
        /// [矩形对象 列表]
        /// </summary>
        [Tooltip("矩形对象 列表")] [SerializeField]
        private List<RectTransform> _rctList = new List<RectTransform>();

        [Tooltip("是否不处理子对象的 Anchor")] [SerializeField]
        private bool _doNotHandleChildAnchors = false;

        /// <summary>
        /// [自身 矩形对象]
        /// </summary>
        private RectTransform _selfRct = null;

        /// <summary>
        /// [自身 矩形对象 大小]
        /// </summary>
        private Vector2 _selfRctSize = Vector2.zero;

        /// <summary>
        /// [是否是 特殊的 对齐方式]
        /// </summary>
        private bool _isSpecialAlignment = false;

        /// <summary>
        /// [矩形对象 数量]
        /// </summary>
        private int _rctCount = 0;

        /// <summary>
        /// [矩形对象 总大小]
        /// </summary>
        private float _rctTotalSize = 0.0f;

        /// <summary>
        /// [矩形对象 总大小 半]
        /// </summary>
        private float _rctTotalSizeHalf = 0.0f;

        /// <summary>
        /// [临时用 矩形对象]
        /// </summary>
        private RectTransform _tempRct = null;

        /// <summary>
        /// [临时用 矩形对象 数量]
        /// </summary>
        private int _tempRctCount = 0;

        /// <summary>
        /// [临时用 对齐方式 值]
        /// </summary>
        private Vector2 _tempAlignmentValue = Vector2.zero;

        /// <summary>
        /// [临时用 矩形对象 大小 半]
        /// </summary>
        private float _tempRctSizeHalf = 0.0f;

        /// <summary>
        /// [临时用 矩形对象 锚点位置]
        /// </summary>
        private Vector2 _tempRctAnchoredPos = Vector2.zero;


        private void Start()
        {
            Handle_Init();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Inspector面板 属性修改时会调用
        /// </summary>
        private void OnValidate()
        {
            Handle_Init();
        }

        private void Reset()
        {
            Handle_Init();
        }
#endif

        #region 获取

        /// <summary>
        /// 获取 [是否是 特殊的 对齐方式]
        /// </summary>
        /// <returns>是否是 特殊的 对齐方式</returns>
        private bool Get_IsSpecialAlignment()
        {
            switch (_direction)
            {
                case UIToolsConfig.Direction.Horizontal:
                    if (_alignment == TextAnchor.UpperCenter ||
                        _alignment == TextAnchor.MiddleCenter ||
                        _alignment == TextAnchor.LowerCenter)
                    {
                        return true;
                    }

                    break;
                case UIToolsConfig.Direction.Vertical:
                    if (_alignment == TextAnchor.MiddleLeft ||
                        _alignment == TextAnchor.MiddleCenter ||
                        _alignment == TextAnchor.MiddleRight)
                    {
                        return true;
                    }

                    break;
                default:
                    break;
            }

            return false;
        }

        #endregion

        #region 处理

        /// <summary>
        /// 处理 [添加]
        /// </summary>
        /// <param name="rct">矩形对象</param>
        public void Handle_Add(RectTransform rct)
        {
            if (rct == null || _rctList.Contains(rct))
            {
                return;
            }
            Handle_RefreshAlignment(rct);

            _rctList.Add(rct);
        }

        /// <summary>
        /// 处理 [移除 全部]
        /// </summary>
        public void Handle_RemoveAll()
        {
            _rctList.Clear();
        }

        // public int GetListCount()
        // {
        //     return _rctList.Count;
        // }

        /// <summary>
        /// 处理 [刷新]
        /// </summary>
        ///

        //private int frameCount = 0;
        //private const int REFRESH_INTERVAL = 2;
        public async void Handle_Refresh(Action callback = null)
        {
            await UniTask.NextFrame();
            //frameCount++;
            //if (frameCount <= REFRESH_INTERVAL)
            //{
            //    return;
            //}
            //frameCount = 0;
            if (!this)
            {
                return;
            }

            _rctCount = _rctList.Count;

            Handle_RefreshRctTotalSize();
            Handle_RefreshAnchoredPos(callback);
        }

        public void Handle_RefreshImmedialy(Action callback = null)
        {
            if (!this)
            {
                return;
            }

            _rctCount = _rctList.Count;

            Handle_RefreshRctTotalSize();
            Handle_RefreshAnchoredPos(callback);
        }


        /// <summary>
        /// 处理 [初始化]
        /// </summary>
        private void Handle_Init()
        {
            Handle_RefreshAlignmentAll();
            Handle_Refresh(null);
        }

        /// <summary>
        /// 处理 [刷新 对齐方式 全部]
        /// </summary>
        private void Handle_RefreshAlignmentAll()
        {
            _isSpecialAlignment = Get_IsSpecialAlignment();
            _rctCount = _rctList.Count;
            for (var index = 0; index < _rctCount; index++)
            {
                Handle_RefreshAlignment(_rctList[index]);
            }
        }

        /// <summary>
        /// 处理 [刷新 对齐方式]
        /// </summary>
        /// <param name="rct">矩形对象</param>
        private void Handle_RefreshAlignment(RectTransform rct)
        {
            if (rct == null) return;

            switch (_alignment)
            {
                case TextAnchor.UpperLeft:
                    _tempAlignmentValue = UIToolsConfig.Anchor_UpperLeft;
                    break;
                case TextAnchor.UpperCenter:
                    _tempAlignmentValue = UIToolsConfig.Anchor_UpperCenter;
                    break;
                case TextAnchor.UpperRight:
                    _tempAlignmentValue = UIToolsConfig.Anchor_UpperRight;
                    break;

                case TextAnchor.MiddleLeft:
                    _tempAlignmentValue = UIToolsConfig.Anchor_MiddleLeft;
                    break;
                case TextAnchor.MiddleCenter:
                    _tempAlignmentValue = UIToolsConfig.Anchor_MiddleCenter;
                    break;
                case TextAnchor.MiddleRight:
                    _tempAlignmentValue = UIToolsConfig.Anchor_MiddleRight;
                    break;

                case TextAnchor.LowerLeft:
                    _tempAlignmentValue = UIToolsConfig.Anchor_LowerLeft;
                    break;
                case TextAnchor.LowerCenter:
                    _tempAlignmentValue = UIToolsConfig.Anchor_LowerCenter;
                    break;
                case TextAnchor.LowerRight:
                    _tempAlignmentValue = UIToolsConfig.Anchor_LowerRight;
                    break;

                default:
                    _tempAlignmentValue = Vector2.zero;
                    break;
            }

            if (!_doNotHandleChildAnchors)
            {
                rct.anchorMin = _tempAlignmentValue;
                rct.anchorMax = _tempAlignmentValue;
            }

            rct.pivot = _tempAlignmentValue;
        }

        /// <summary>
        /// 处理 [刷新 矩形对象 总大小]
        /// </summary>
        private void Handle_RefreshRctTotalSize()
        {
            _rctTotalSize = 0.0f;
            _tempRct = null;
            _tempRctCount = 0;
            for (var index = 0; index < _rctCount; index++)
            {
                _tempRct = _rctList[index];
                if (_tempRct == null || _tempRct.gameObject.activeSelf == false) continue;

                _rctTotalSize += _direction == UIToolsConfig.Direction.Horizontal
                    ? _tempRct.sizeDelta.x
                    : _tempRct.sizeDelta.y;
                _tempRctCount++;
            }

            _rctTotalSizeHalf = _rctTotalSize * 0.5f;

            if (_selfRct == null)
            {
                _selfRct = GetComponent<RectTransform>();
                _selfRctSize = _selfRct.sizeDelta;
            }

            switch (_direction)
            {
                case UIToolsConfig.Direction.Horizontal:
                    _selfRctSize.x = (_tempRctCount > 0
                        ? _rctTotalSize + (_tempRctCount - 1) * _spacing
                        : 0) + _padding.Left + _padding.Right;
                    _selfRct.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                        _selfRctSize.x);
                    break;
                case UIToolsConfig.Direction.Vertical:
                    _selfRctSize.y = (_tempRctCount > 0
                        ? _rctTotalSize + (_tempRctCount - 1) * _spacing
                        : 0) + _padding.Top + _padding.Bottom;
                    _selfRct.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                        _selfRctSize.y);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 处理 [刷新 锚点位置]
        /// </summary>
        private void Handle_RefreshAnchoredPos(Action callback = null)
        {
            _tempRct = null;
            _tempRctSizeHalf = 0.0f;
            _tempRctAnchoredPos = Vector2.zero;
            // 初始化位置时加入Padding偏移
            switch (_direction)
            {
                case UIToolsConfig.Direction.Horizontal:
                    _tempRctAnchoredPos.x += _padding.Left;
                    _tempRctAnchoredPos.y = -_padding.Top;
                    break;
                case UIToolsConfig.Direction.Vertical:
                    _tempRctAnchoredPos.y -= _padding.Top;
                    _tempRctAnchoredPos.x = _padding.Left;
                    break;
            }

            if (_isSpecialAlignment == true && _tempRctCount > 1)
            {
                switch (_direction)
                {
                    case UIToolsConfig.Direction.Horizontal:
                        _tempRctAnchoredPos.x = -_rctTotalSizeHalf -
                                                (_tempRctCount - 1) * (_spacing * 0.5f) + _padding.Left;
                        break;
                    case UIToolsConfig.Direction.Vertical:
                        _tempRctAnchoredPos.y = _rctTotalSizeHalf +
                                                (_tempRctCount - 1) * (_spacing * 0.5f) - _padding.Top;
                        break;
                    default:
                        break;
                }
            }

            for (var index = 0; index < _rctCount; index++)
            {
                _tempRct = _rctList[index];
                if (_tempRct == null || _tempRct.gameObject.activeSelf == false) continue;

                switch (_direction)
                {
                    case UIToolsConfig.Direction.Horizontal:
                        if (_alignment == TextAnchor.UpperLeft ||
                            _alignment == TextAnchor.MiddleLeft ||
                            _alignment == TextAnchor.LowerLeft)
                        {
                            _tempRct.anchoredPosition = _tempRctAnchoredPos;
                            _tempRctAnchoredPos.x += _tempRct.sizeDelta.x + _spacing;
                        }
                        else if (_alignment == TextAnchor.UpperRight ||
                                 _alignment == TextAnchor.MiddleRight ||
                                 _alignment == TextAnchor.LowerRight)
                        {
                            _tempRct.anchoredPosition = _tempRctAnchoredPos;
                            _tempRctAnchoredPos.x -= _tempRct.sizeDelta.x + _spacing;
                        }
                        else
                        {
                            _tempRctSizeHalf =
                                _tempRctCount > 1 ? _tempRct.sizeDelta.x * 0.5f : 0;
                            _tempRctAnchoredPos.x += _tempRctSizeHalf;
                            _tempRct.anchoredPosition = _tempRctAnchoredPos;
                            _tempRctAnchoredPos.x += _tempRctSizeHalf + _spacing;
                        }

                        break;
                    case UIToolsConfig.Direction.Vertical:
                        if (_alignment == TextAnchor.UpperLeft ||
                            _alignment == TextAnchor.UpperCenter ||
                            _alignment == TextAnchor.UpperRight)
                        {
                            _tempRct.anchoredPosition = _tempRctAnchoredPos;
                            _tempRctAnchoredPos.y -= _tempRct.sizeDelta.y + _spacing;
                        }
                        else if (_alignment == TextAnchor.LowerLeft ||
                                 _alignment == TextAnchor.LowerCenter ||
                                 _alignment == TextAnchor.LowerRight)
                        {
                            _tempRct.anchoredPosition = _tempRctAnchoredPos;
                            _tempRctAnchoredPos.y += _tempRct.sizeDelta.y + _spacing;
                        }
                        else
                        {
                            _tempRctSizeHalf =
                                _tempRctCount > 1 ? _tempRct.sizeDelta.y * 0.5f : 0;
                            _tempRctAnchoredPos.y -= _tempRctSizeHalf;
                            _tempRct.anchoredPosition = _tempRctAnchoredPos;
                            _tempRctAnchoredPos.y -= _tempRctSizeHalf + _spacing;
                        }

                        break;
                    default:
                        break;
                }
            }

            if (callback != null)
            {
                callback();
            }
        }

        #endregion
    }
}