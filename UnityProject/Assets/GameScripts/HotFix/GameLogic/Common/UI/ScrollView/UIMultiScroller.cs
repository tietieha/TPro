// -------------------------------------------------------------------------------------------
// @说       明: ScrollerView组件,内部使用缓存池,简单易用,性能比原生的好很多;
// @作       者: zhoumingfeng
// @版  本  号: V1.00
// @创建时间: 2020.11.18
// @修改记录:
//  2022.02.18 增加功能
//    1.自动计算一行或者一列显示的数量,可解决屏幕不同分辨率的适配问题;
//    2.自动计算可创建的行数或者列数;
//    3.变量添加注释说明;
//  2022.11.17 增加功能
//    1.内部自动绑定onValueChanged事件,防止有人忘了绑定onValueChanged事件;
//    2.增加自动跳转功能(支持跳转到某项、某行或者某列);
//    3.增加选中功能;
//    4.支持跳转并选中;
//  2022.12.30
//    1.支持最外框根据Content内容自适应高度或者宽度;
//    2.左侧和上方增加边距的设置;
// @使用方法:
//    1.定义变量
//       public UIMultiScroller scroller;
//    2.赋值
//       scroller.DataCount = 10;                   // 创建Item的数量
//       scroller.OnItemCreate = OnItemCreate;      // 创建每个Item项处理回调
//       scroller.ResetScroller();                  // 创建ScrollerView
//    3.实现创建Item的回调
//       private void OnItemCreate(int index, GameObject obj)
//       {
//       }
//      回调函数参数说明:
//       index: 创建的第几项(第一项为0)
//       obj:  item对象, 可通过其获取到每一个item对象上挂的脚本
//  2023.11.15
//    1.增加触底回调
//  @使用方法
//      function OnTouchBottom()
//
//      end
//      scroller.OnTouchBottom = OnTouchBottom;
// --------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

using DG.Tweening;
using System.Drawing.Text;
using GameLogic;
using TEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public enum TweenFunction
{
    DOScale,
    DOMove,
    DORotate,
}


public class UIMultiScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    #region Inspector

    public enum Arrangement { Horizontal, Vertical, }
    public Arrangement _movement = Arrangement.Horizontal;

    /// <summary>
    /// 单行或单列的Item数量
    /// </summary>
    [Range(0, 20)]
    [Header("单行或单列显示的Item数量,值为0时自动计算数量")]
    public int maxPerLine = 0;

    [Range(0, 900)]
    [Header("X坐标间距")]
    public int spacingX = 5;

    [Range(0, 100)]
    [Header("Y坐标间距")]
    public int spacingY = 5;

    //[Tooltip("初始化间隔(以帧计算)")]
    [Header("创建Item的间隔帧数")]
    public int intervalFrame = 0;
    private int tempIntervalFrame = 0;
    [Header("是否以行或列同时创建")]
    public bool isParallelCreate = false;

    [Header("是否开启缓动")]
    public bool useTween = false;
    public float tweenDuringTime = 0.2f;
    public float itemGapTime = 0.1f;
    public Ease easeType = Ease.Linear;
    public Vector3 startValue = Vector3.zero;
    public Vector3 endValue = Vector3.zero;
    public TweenFunction function = TweenFunction.DOScale;
    [Header("因为效率问题，只支持1层子查找")]
    public string tweenChildName;

    private List<int> m_CreateList = new List<int>();

    [XLua.CSharpCallLua]
    public delegate void OnFuncCreateOverHandler();

    // 分帧创建Item完毕时的回调函数
    public OnFuncCreateOverHandler OnFuncCreateOver;

    [Header("Item的宽度")]
    public int cellWidth = 100;
    [Header("Item的高度")]
    public int cellHeight = 100;

    [Header("开启根据Index使用的第二套宽高")]
    public bool useSecondSize = false;
    public int secondCellWidth = 100;
    public int secondCellHeight = 100;

    [Range(0, 200)]
    [Header("默认创建的行或列数,一般比可见的数量大2~3个,为0时自动计算")]
    public int viewCount = 0;

    [SerializeField]
    [Header("是否自适应居中item")]
    private bool SetCenter = false;

    [SerializeField]
    [Header("是否自适应高度或宽度")]
    private bool IsAdaptiveSize = false;

    [SerializeField]
    [Header("是否自动对齐cell")]
    private bool IsMoveCell;

    [SerializeField]
    [Header("滑动时长（秒）")]
    private float SmoothDuration = 0.2f;

    [Header("Item模板")]
    public GameObject itemPrefab;

    public RectTransform _content;

    [SerializeField]
    private ScrollRect _ScrollRect;

    [SerializeField]
    private RectOffset _Padding;

    #endregion

    #region 公有属性

    /// <summary>
    /// _itemList数量
    /// </summary>
    public int Count
    {
        get
        {
            if (_itemList == null) return 0;
            else return _itemList.Count;
        }
    }

    /// <summary>
    /// Item创建回调
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="obj">创建的物体</param>
    [XLua.CSharpCallLua]
    public delegate void OnItemCreateHandler(int index, GameObject obj);

    public OnItemCreateHandler OnItemCreate = null;

    [XLua.CSharpCallLua]
    public delegate bool OnCheckItemUseSecondSizeHandler(int index);

    public OnCheckItemUseSecondSizeHandler OnCheckItemUseSecondSize = null;

    [XLua.CSharpCallLua]
    public delegate void OnTouchBottomHandler();
    //触底回调
    public OnTouchBottomHandler OnTouchBottom;

    //滑动cell回调
    [XLua.CSharpCallLua]
    public delegate void OnDragMoveCellHandler(int cellIndex);
    public OnDragMoveCellHandler OnDragMoveCell;

    public delegate void MoveCompleteHandler();

    /// <summary>
    /// 总数量
    /// </summary>
    public int DataCount
    {
        get { return _dataCount; }
        set
        {
            _dataCount = value;

            if (this.maxPerLine <= 0)
                GetBestPerLine();

            if (this.viewCount <= 0)
                GetBestViewCount();

            UpdateTotalSize();
        }
    }

    public int SelectIndex
    {
        get
        {
            return _selectIndex;
        }
    }

    #endregion

    #region 私有变量

    private int _index = -1;
    private int _dataCount;
    private int _checked_index = -1;
    private float _default_size = 0;
    private int _selectIndex = 0;
    private int _dragStartIndex;
    private RectTransform _rectTransform;
    private Vector2 _lastPosition;
    private Dictionary<int,Tween> _tweens = new Dictionary<int,Tween>();

    private List<UIMultiScrollIndex> _itemList = new List<UIMultiScrollIndex>();

    //将未显示出来的Item存入未使用队列里面，等待需要使用的时候直接取出
    private Queue<UIMultiScrollIndex> _unUsedQueue = new Queue<UIMultiScrollIndex>();

    private Dictionary<int, Queue<GameObject>> m_Pool = new Dictionary<int, Queue<GameObject>>();
    private Dictionary<GameObject, int> m_GoTag = new Dictionary<GameObject, int>();

    private List<int> childOffsetList = new List<int>();//用于子物体偏移（具体参考推图主界面）
    private int _childOffsetIndex = 0;
    #endregion

    #region 框架接口

    void Start()
    {
        itemPrefab.SetActive(false);

        tempIntervalFrame = intervalFrame;

        // if (this.maxPerLine <= 0)
        //     GetBestPerLine();
        //
        // if (this.viewCount <= 0)
        //     GetBestViewCount();

        RegisterEvent();

        OnValueChange(Vector2.zero);
        this.ItemSetCenter();
    }

    void LateUpdate()
    {
        if (m_CreateList.Count == 0) return;

        tempIntervalFrame--;

        if (tempIntervalFrame > 0) return;
        if (isParallelCreate)
        {
            int index = 0;
            for (int i = 0; i < m_CreateList.Count; i++)
            {
                index++;
                CreateItem(m_CreateList[i]);
                tempIntervalFrame = intervalFrame;
                if (m_CreateList.Count < maxPerLine)
                {
                    if (index == m_CreateList.Count)
                    {
                        m_CreateList.Clear();
                    }
                }
                else
                {
                    if (index == maxPerLine)
                    {
                        for (int j = index -1; j >= 0; j--)
                        {
                            m_CreateList.RemoveAt(j);
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < m_CreateList.Count; i++)
            {
                CreateItem(m_CreateList[i]);
                m_CreateList.RemoveAt(i);
                tempIntervalFrame = intervalFrame;
                break;
            }
        }

        if (m_CreateList.Count == 0)
        {
            if (OnFuncCreateOver != null)
                OnFuncCreateOver.Invoke();
        }
    }

    private void OnDestroy()
    {
        itemPrefab = null;
        _content = null;
        _itemList = null;
        _unUsedQueue = null;
        OnItemCreate = null;
        OnTouchBottom = null;
        m_CreateList = null;
        DestoryAll();
        m_Pool = null;
        m_GoTag = null;
    }

    #endregion

    #region 公共接口
    [XLua.BlackList]
    public void SetScrollRect(ScrollRect scrollRect)
    {
        _ScrollRect = scrollRect;
    }

    public void SetChildOffset(List<int> childOffset)
    {
        childOffsetList = childOffset;
    }

    /// <summary>
    /// 重置列表
    /// </summary>
    /// <param name="moveToIndex">跳转到第N项(小于0时表示以当前状态刷新, 否则为跳转到指定项, 0表示第一项, 依次类推)</param>
    /// <param name="check">跳转后是否选中(true:选中 false:不选中)</param>
    /// <param name="childIndex">子节点第几项,从0开始</param>
    public void ResetScroller(int moveToIndex = 0, bool check = false,int childIndex = 0)
    {
        this.ResetChilds();

        _index = -1;
        _checked_index = -1;
        _selectIndex = moveToIndex;
        _childOffsetIndex = childIndex;

        UIMultiScrollIndex[] arr = _content.GetComponentsInChildren<UIMultiScrollIndex>();
        for (int i = 0; i < arr.Length; i++)
            DestroyItem(arr[i].gameObject);
        arr = null;

        _itemList?.Clear();
        _unUsedQueue?.Clear();
        m_CreateList?.Clear();

        if (moveToIndex < 0)
            this.OnValueChange(Vector2.zero);
        else
            this.MoveToIndex(moveToIndex, check,false,childIndex);

        this.CheckAdaptiveSize();
    }

    /// <summary>
    /// 移动到指定项
    /// </summary>
    /// <param name="index">第index项,从0开始</param>
    /// <param name="check">移动到指定项后是否选中(true:选中 false:不选中)</param>
    /// <param name="smooth">true:平滑移动 false:瞬间移动</param>
    /// <param name="childIndex">子节点第几项,从0开始</param>
    /// <param name="moveComplete">lua function 移动结束回调</param>
    public void MoveToIndex(int index, bool check = false, bool smooth = false,int childIndex = 0, MoveCompleteHandler moveComplete = null)
    {
        _selectIndex = index;
        _childOffsetIndex = childIndex;
        if (maxPerLine == 0)
        {
            GetBestPerLine();
        }
        this.MoveToLine(index / this.maxPerLine, smooth, moveComplete);
        if(check)
            this.CheckedItem(index);
    }

    /// <summary>
    /// 移动到指定行
    /// </summary>
    /// <param name="lineIndex">第lineIndex行,从0开始</param>
    /// <param name="smooth">true:平滑移动 false:瞬间移动</param>
    /// <param name="moveComplete">lua function 移动结束回调</param>
    public void MoveToLine(int lineIndex, bool smooth = false, MoveCompleteHandler moveComplete = null)
    {
        if (lineIndex <= 0 && !IsMoveCell && moveComplete == null)
        {
            this.MoveToTop();
        }
        else if (lineIndex > 0 && lineIndex >= (this._dataCount - 1) / this.maxPerLine && !IsMoveCell && moveComplete == null)
        {
            this.MoveToBottom();
        }
        else
        {
            if (null == _ScrollRect.viewport)
            {
                Log.Error("列表的ScrollRect的viewport没有挂载节点...");
                return;
            }

            // 列表显示区域大小;
            var rect = _ScrollRect.viewport.rect;

            //子物体偏移
            float tempChildOffset = 0;
            if (childOffsetList.Count > 0 && childOffsetList.Count > _childOffsetIndex)
            {
                tempChildOffset = childOffsetList[_childOffsetIndex];
            }

            switch (_movement)
            {
                case Arrangement.Horizontal:
                    {
                        if (_content.sizeDelta.x < rect.width)
                            break;

                        float targetX = this._Padding.left - tempChildOffset;
                        for (int _idx = 1; _idx <= lineIndex; _idx++)
                        {
                            var useSecond = false;
                            if (OnCheckItemUseSecondSize != null)
                            {
                                useSecond = OnCheckItemUseSecondSize(_idx-1);
                            }
                            // var height = useSecond ? secondCellHeight : cellHeight;
                            var width = useSecond ? secondCellWidth : cellWidth;
                            targetX += width + spacingX;
                        }

                        if (targetX > _content.sizeDelta.x - rect.width)
                            targetX = _content.sizeDelta.x - rect.width;
                        if (_dataCount <= viewCount)
                        {
                            targetX = 0;
                        }

                        if (smooth)
                            _content.DOAnchorPosX(-targetX, SmoothDuration, true).OnComplete(() =>
                            {
                                moveComplete?.Invoke();
                            });
                        else
                            _content.anchoredPosition = new Vector2(-targetX, _content.anchoredPosition.y);
                        break;
                    }
                case Arrangement.Vertical:
                    {
                        if (_content.sizeDelta.y < rect.height)
                            break;

                        float targetY = this._Padding.top - tempChildOffset;

                        for (int _idx = 1; _idx <= lineIndex; _idx++)
                        {
                            var useSecond = false;
                            if (OnCheckItemUseSecondSize != null)
                            {
                                useSecond = OnCheckItemUseSecondSize(_idx-1);
                            }

                            var height = useSecond ? secondCellHeight : cellHeight;
                            // var width = useSecond ? secondCellWidth : cellWidth;
                            targetY += height + spacingY;
                        }
                        if (targetY > _content.sizeDelta.y - rect.height)
                            targetY = _content.sizeDelta.y - rect.height;
                        
                        if (_dataCount <= viewCount)
                        {
                            targetY = 0;
                        }

                        if (smooth)
                            _content.DOAnchorPosY(targetY, SmoothDuration, true).OnComplete(() =>
                            {
                                moveComplete?.Invoke();
                            });
                        else
                            _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, targetY);

                        break;
                    }
                default:
                    break;
            }

            ForceUpdateItems();
        }
    }

    /// <summary>
    /// 移动到顶部
    /// </summary>
    public void MoveToTop()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                {
                    _content.anchoredPosition = new Vector2(0, _content.anchoredPosition.y);

                    break;
                }
            case Arrangement.Vertical:
                {
                    _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, 0);

                    break;
                }
            default:
                break;
        }

        ForceUpdateItems();
    }

    /// <summary>
    /// 移动到底部
    /// </summary>
    public void MoveToBottom()
    {
        if (null == _ScrollRect.viewport)
        {
            Log.Error("列表的ScrollRect的viewport没有挂载节点...");
            return;
        }

        float tempChildOffset = 0;
        if (childOffsetList.Count > 0 && childOffsetList.Count > _childOffsetIndex)
        {
            tempChildOffset = childOffsetList[_childOffsetIndex];
        }

        // 列表显示区域大小;
        var rect = _ScrollRect.viewport.rect;

        switch (_movement)
        {
            case Arrangement.Horizontal:
                if (_content.sizeDelta.x > rect.width)
                    _content.anchoredPosition = new Vector2(rect.width - _content.sizeDelta.x + tempChildOffset, _content.anchoredPosition.y);
                break;
            case Arrangement.Vertical:
                if (_content.sizeDelta.y > rect.height)
                    _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, _content.sizeDelta.y - rect.height - tempChildOffset);
                break;
        }

        ForceUpdateItems();
    }

    /// <summary>
    /// 强制刷新
    /// </summary>
    public void ForceUpdateItems()
    {
        m_CreateList.Clear();
        int index = GetPosIndex();

        if(_index == index && index > -1)
        {
            for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
            {
                if (i < 0)
                    continue;
                if (i > _dataCount - 1)
                    continue;
                bool isOk = false;
                foreach (UIMultiScrollIndex item in _itemList)
                {
                    if (item.Index == i)
                    {
                        item.Check(item.Index == this._checked_index, false);
                        ItemCreateFinish(item.Index, item.gameObject);
                        isOk = true;
                    }
                }
                if (isOk)
                    continue;
                if (intervalFrame > 0 && _itemList.Count <= 0)
                {
                    m_CreateList.Add(i);
                }
                else
                    CreateItem(i);
            }
        }
        else if (_index != index && index > -1)
        {
            _index = index;
            for (int i = _itemList.Count; i > 0; i--)
            {
                UIMultiScrollIndex item = _itemList[i - 1];
                if (item.Index < index * maxPerLine || (item.Index >= (index + viewCount) * maxPerLine))
                {
                    _itemList.Remove(item);
                    _unUsedQueue.Enqueue(item);
                }
            }
            for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
            {
                if (i < 0) continue;
                if (i > _dataCount - 1) continue;
                bool isOk = false;
                foreach (UIMultiScrollIndex item in _itemList)
                {
                    if (item.Index == i)
                    {
                        isOk = true;
                    }
                }
                if (isOk) continue;
                if (intervalFrame > 0 && _itemList.Count <= 0)
                {
                    m_CreateList.Add(i);
                }
                else
                    CreateItem(i);
            }
        }
    }


    private void ItemCreateFinish( int index, GameObject go )
    {
        if (useTween)
        {
            GameObject tweenGo = GetTweenChildObject(go);
            Tween tween = GetTween(tweenGo.GetInstanceID());
            if (tween != null)
            {
                tween.Kill();
                _tweens.Remove(tweenGo.GetInstanceID());
            }
            if (function == TweenFunction.DOScale)
            {
                tweenGo.transform.localScale = Vector3.zero;
                TimeUtil.Get(tweenGo).Delay(itemGapTime * index, () =>
                {
                    tweenGo.transform.localScale = startValue;
                    Tween newTween = tweenGo.transform.DOScale(endValue, tweenDuringTime).SetEase(easeType);
                    _tweens.Add(tweenGo.GetInstanceID(), newTween);
                });
            }
            else if(function == TweenFunction.DOMove)
            {
                tweenGo.transform.localScale = startValue;
                Tween newTween = tweenGo.transform.DOMove(endValue, tweenDuringTime).SetDelay(itemGapTime * index).SetEase(easeType);
                _tweens.Add(tweenGo.GetInstanceID(), newTween);
            }
            else if(function == TweenFunction.DORotate)
            {
                tweenGo.transform.localScale = startValue;
                Tween newTween = tweenGo.transform.DORotate(endValue, tweenDuringTime).SetDelay(itemGapTime * index).SetEase(easeType);
                _tweens.Add(tweenGo.GetInstanceID(), newTween);
            }
        }
        OnItemCreate?.Invoke(index, go);
    }


    private Tween GetTween( int instanceId )
    {
        if (_tweens.TryGetValue(instanceId,out Tween tween))
        {
            return tween;
        }
        return null;
    }


    private GameObject GetTweenChildObject( GameObject go)
    {
        if (string.IsNullOrEmpty(tweenChildName))
        {
            return go;
        }
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform child = go.transform.GetChild(i);
            if (child.name == tweenChildName)
            {
                return child.gameObject;
            }
        }
        return go;
    }


    /// <summary>
    /// 更新指定项
    /// </summary>
    /// <param name="index"></param>
    public void UpdateItem(int index)
    {
        if (_itemList == null) return;

        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].Index == index)
            {
                _itemList[i].Check(index == this._checked_index, false);
                ItemCreateFinish(index, _itemList[i].gameObject);
            }
        }
    }

    /// <summary>
    /// 更新当前的所有项
    /// </summary>
    public void UpdateAllItem()
    {
        if (_itemList == null) return;

        for (int i = 0; i < _itemList.Count; i++)
        {
            _itemList[i].Check(_itemList[i].Index == this._checked_index, false);

            ItemCreateFinish(_itemList[i].Index, _itemList[i].gameObject);
        }
    }

    /// <summary>
    /// 获得当前显示的所有Item
    /// </summary>
    /// <returns></returns>
    public List<UIMultiScrollIndex> GetItemList()
    {
        return this._itemList;
    }

    /// <summary>
    /// 根据索引号 获取当前item的位置
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Vector3 GetPosition_Normal(int i)
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                {
                    return new Vector3(this._Padding.left + (cellWidth + spacingX) * (i / maxPerLine), 0 - this._Padding.top - (cellHeight + spacingY) * (i % maxPerLine), 0f);
                }
            case Arrangement.Vertical:
                {
                    return new Vector3(this._Padding.left + (cellWidth + spacingX) * (i % maxPerLine), 0 - this._Padding.top - (cellHeight + spacingY) * (i / maxPerLine), 0f);
                }
        }

        return Vector3.zero;
    }

    public Vector3 GetPosition_Custom(int i)
    {
        float offsetX = 0f;
        float offsetY = 0f;
        for (int _idx = 1; _idx <= i; _idx++)
        {
            var useSecond = false;
            if(OnCheckItemUseSecondSize != null)
            {
                useSecond = OnCheckItemUseSecondSize(_idx-1);
            }

            var height = useSecond ? secondCellHeight : cellHeight;
            var width = useSecond ? secondCellWidth : cellWidth;
            if (_movement == Arrangement.Vertical)
            {
                offsetY += height + spacingY;
            }
            else
            {
                offsetX += width + spacingX;
            }
        }
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return new Vector3(offsetX, -_Padding.top, 0f);
            case Arrangement.Vertical:
                return new Vector3(this._Padding.left + (cellWidth + spacingX) * (i % maxPerLine), 0 - this._Padding.top -offsetY, 0f);
        }
        return Vector3.zero;
    }

    public Vector3 GetPosition(int i)
    {
        if (useSecondSize && OnCheckItemUseSecondSize != null)
        {
            return GetPosition_Custom(i);
        }
        else
        {
            return GetPosition_Normal(i);
        }
    }

    /// <summary>
    /// 获得指定项
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetItemByIndex(int index)
    {
        if (null == _itemList || _itemList.Count <= 0)
            return null;

        foreach (UIMultiScrollIndex item in _itemList)
        {
            if (item.Index == index)
                return item.gameObject;
        }

        return null;
    }

    /// <summary>
    /// 提供给外部的方法，添加指定位置的Item
    /// </summary>
    public void AddItem(int index)
    {
        DataCount += 1;
        AddItemIntoPanel(index);
    }

    /// <summary>
    /// 选中Item
    /// </summary>
    /// <param name="index">Item的索引,从0开始</param>
    public void CheckedItem(int index)
    {
        this._checked_index = index;

        if (null == _itemList || _itemList.Count <= 0)
            return;

        foreach (UIMultiScrollIndex item in _itemList)
            item.Check(item.Index == index, false);
    }

    /// <summary>
    /// 通知刷新选中的状态
    /// </summary>
    /// <param name="item">item对象</param>
    public void NotifyRefreshCheckState(UIMultiScrollIndex item)
    {
        this._checked_index = item.Index;

        if (null == _itemList || _itemList.Count <= 0)
            return;

        foreach (UIMultiScrollIndex per in _itemList)
        {
            if (per.Index != this._checked_index)
                per.Unchecked();
        }
    }
    
    
    public bool IsItemCompletelyVisible(int index)
    {
        var sItem = GetScrollIndex(index);
        if (sItem == null) return false;

        var itemRect = sItem.transform as RectTransform;
        if (itemRect == null) return false;

        // 获取item在viewport下的局部坐标
        Vector3[] itemCorners = new Vector3[4];
        itemRect.GetWorldCorners(itemCorners);
        RectTransform viewport = _ScrollRect.viewport;

        for (int i = 0; i < 4; i++)
        {
            Vector3 localPoint = viewport.InverseTransformPoint(itemCorners[i]);
            if (!viewport.rect.Contains(localPoint))
                return false;
        }
        return true;
    }

    public void OnValueChange(Vector2 pos)
    {
        m_CreateList.Clear();
        if (_itemList == null) return;

        if (pos.y < 0)
        {
            OnTouchBottom?.Invoke();
            return;
        }

        int index = GetPosIndex();

        bool isTopItemFullVisible = IsItemCompletelyVisible(index * maxPerLine);
        var showViewCount = isTopItemFullVisible ? viewCount : viewCount + 1;

        if ((!isTopItemFullVisible || _index != index) && index > -1)
        {
            _index = index;
            for (int i = _itemList.Count; i > 0; i--)
            {
                UIMultiScrollIndex item = _itemList[i - 1];
                if (item.Index < index * maxPerLine || (item.Index >= (index + showViewCount) * maxPerLine) || item.Index >= _dataCount)
                {
                    _itemList.Remove(item);
                    _unUsedQueue.Enqueue(item);
                }
            }
            for (int i = _index * maxPerLine; i < (_index + showViewCount) * maxPerLine; i++)
            {
                if (i < 0) continue;
                if (i > _dataCount - 1) continue;
                bool isOk = false;
                foreach (UIMultiScrollIndex item in _itemList)
                {
                    if (item.Index == i)
                    {
                        isOk = true;
                    }
                }
                if (isOk) continue;
                if (intervalFrame > 0 && _itemList.Count <= 0)
                {
                    m_CreateList.Add(i);
                }
                else
                    CreateItem(i);
            }
        }
    }

    // <summary>
    /// Item是否完全可见
    /// </summary>
    /// <returns></returns>
    public bool IsItemVisible100P(int index)
    {
        UIMultiScrollIndex sItem = null;
        foreach (UIMultiScrollIndex item in _itemList)
        {
            if (item.Index == index)
            {
                sItem = item;
                break;
            }
        }

        if (sItem == null) return false;

        var itemRect = sItem.transform as RectTransform;
        if (itemRect == null) return false;

        var corners = new Vector3[4];
        itemRect.GetWorldCorners(corners);

        var visible100P = true;
        for (var i = 0; i < 4; i++)
        {
            var point = _ScrollRect.viewport.transform.InverseTransformPoint(corners[i]);
            if (!_ScrollRect.viewport.rect.Contains(point))
            {
                visible100P = false;
                break;
            }
        }
        return visible100P;
    }

    #endregion

    #region 私有接口

    /// <summary>
    /// 重置子列表,处理列表套列表的情况
    /// </summary>
    private void ResetChilds()
    {
        UIMultiScrollIndex[] arr = _content.GetComponentsInChildren<UIMultiScrollIndex>();
        if (null == arr || arr.Length <= 0)
            return;

        for (int i = 0; i < arr.Length; i++)
        {
            UIMultiScroller scroller = arr[i].GetComponentInChildren<UIMultiScroller>();
            if (null != scroller)
            {
                scroller._index = -1;
                UIMultiScrollIndex[] childArr = scroller._content.GetComponentsInChildren<UIMultiScrollIndex>();
                for (int j = 0; j < childArr.Length; j++)
                {
                    scroller.DestroyItem(childArr[j].gameObject);
                }
                childArr = null;

                if (scroller._itemList != null)
                {
                    scroller._itemList.Clear();
                }

                if (scroller._unUsedQueue != null)
                {
                    scroller._unUsedQueue.Clear();
                }
            }
        }
    }

    /// <summary>
    /// 创建Item
    /// </summary>
    /// <param name="index"></param>
    private void CreateItem(int index)
    {
        UIMultiScrollIndex itemBase;

        if (_unUsedQueue.Count > 0)
        {
            itemBase = _unUsedQueue.Dequeue();
            itemBase.Scroller = this;
        }
        else
        {
            if (itemPrefab == null)
            {
                Log.Error("itemPrefab为空！！ 请检查资源");
                return;
            }
            //GameObject obj = Instantiate(itemPrefab);
            GameObject obj = RequestGameObejct(itemPrefab);
            obj.SetActiveEx(true);
            //obj.transform.SetParent(_content);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = itemPrefab.transform.localScale;
            itemBase = obj.GetOrAddComponent<UIMultiScrollIndex>();
            itemBase.Scroller = this;
        }

        _itemList.Add(itemBase);

        itemBase.Check(index == this._checked_index, false);
        ItemCreateFinish(index, itemBase.gameObject);
        itemBase.Index = index;
    }

    /// <summary>
    /// 获取最上位置的索引
    /// </summary>
    /// <returns></returns>
    private int GetPosIndex()
    {
        if (useSecondSize && OnCheckItemUseSecondSize != null)
        {
            return GetPosIndex_Custom();
        }
        else
        {
            return GetPosIndex_Normal();
        }
    }

    private int GetPosIndex_Normal()
    {
        int retValue = 0;

        switch (_movement)
        {
            case Arrangement.Horizontal:
                retValue = Mathf.FloorToInt((_content.anchoredPosition.x + this._Padding.left) / -(cellWidth + spacingX));
                break;
            case Arrangement.Vertical:
                retValue = Mathf.FloorToInt((_content.anchoredPosition.y - this._Padding.top) / (cellHeight + spacingY));
                break;
        }

        if (retValue < 0) retValue = 0;

        return retValue;
    }
    private int GetPosIndex_Custom()
    {
        int retValue = 0;
        float offset = 0f;

        for (int i = 0; i < _dataCount; i++)
        {
            var useSecond = OnCheckItemUseSecondSize(i);
            var height = useSecond ? secondCellHeight : cellHeight;
            var width = useSecond ? secondCellWidth : cellWidth;
            if (_movement == Arrangement.Vertical)
            {
                offset += height + spacingY;
                if (offset > _content.anchoredPosition.y + this._Padding.top)
                {
                    retValue = i / maxPerLine;
                    break;
                }
            }
            else if (_movement == Arrangement.Horizontal)
            {
                offset += width + spacingX;
                if (offset > -_content.anchoredPosition.x + this._Padding.left)
                {
                    retValue = i / maxPerLine;
                    break;
                }
            }
        }

        if (retValue < 0) retValue = 0;

        return retValue;
    }

    /// <summary>
    /// 获得指定项
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private UIMultiScrollIndex GetScrollIndex(int index)
    {
        if (null == _itemList || _itemList.Count <= 0)
            return null;

        foreach (UIMultiScrollIndex item in _itemList)
        {
            if (item.Index == index)
                return item;
        }

        return null;
    }

    /// <summary>
    /// 这个方法的目的 就是根据总数量 行列 来计算content的真正宽度或者高度
    /// </summary>
    private void UpdateTotalSize()
    {
        if (useSecondSize && OnCheckItemUseSecondSize != null)
        {
            UpdateTotalSize_Custom();
        }
        else
        {
            UpdateTotalSize_Normal();
        }
    }

    private void UpdateTotalSize_Normal()
    {
        if (null == _content)
            return;

        if (null == _Padding)
            _Padding = new RectOffset();

        int lineCount = Mathf.CeilToInt((float)_dataCount / maxPerLine);
        switch (_movement)
        {
            case Arrangement.Horizontal:
                _content.sizeDelta = new Vector2(this._Padding.left + cellWidth * lineCount + spacingX * (Mathf.Max(lineCount, 1) - 1), _content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, this._Padding.top + cellHeight * lineCount + spacingY * (Mathf.Max(lineCount, 1) - 1));
                break;
        }
    }

    private void UpdateTotalSize_Custom()
    {
        if (null == _content)
            return;

        float totalWidth = _Padding.left;
        float totalHeight = _Padding.top;

        for (int i = 0; i < _dataCount; i++)
        {
            var useSecond = OnCheckItemUseSecondSize(i);
            var width = useSecond ? secondCellWidth : cellWidth;
            var height = useSecond ? secondCellHeight : cellHeight;
            if (_movement == Arrangement.Horizontal)
            {
                totalWidth += width + spacingX;
            }
            else if (_movement == Arrangement.Vertical)
            {
                totalHeight += height + spacingY;
            }
        }

        if (_movement == Arrangement.Horizontal)
        {
            _content.sizeDelta = new Vector2(totalWidth - spacingX, _content.sizeDelta.y);
        }
        else if (_movement == Arrangement.Vertical)
        {
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, totalHeight + spacingY);
        }
    }

    /// <summary>
    /// 提供给外部的方法，删除指定位置的Item
    /// </summary>
    public void DelItem(int index)
    {
        if (index < 0 || index > _dataCount - 1)
        {
            Log.Error("删除错误:" + index);
            return;
        }
        DelItemFromPanel(index);
        DataCount -= 1;
    }

    private void AddItemIntoPanel(int index)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            UIMultiScrollIndex item = _itemList[i];
            if (item.Index >= index) item.Index += 1;
        }
        CreateItem(index);
    }

    private void DelItemFromPanel(int index)
    {
        int maxIndex = -1;
        int minIndex = int.MaxValue;
        for (int i = _itemList.Count; i > 0; i--)
        {
            UIMultiScrollIndex item = _itemList[i - 1];
            if (item.Index == index)
            {
                DestroyItem(item.gameObject);
                _itemList.Remove(item);
            }
            if (item.Index > maxIndex)
            {
                maxIndex = item.Index;
            }
            if (item.Index < minIndex)
            {
                minIndex = item.Index;
            }
            if (item.Index > index)
            {
                item.Index -= 1;
            }
        }
        if (maxIndex < DataCount - 1)
        {
            CreateItem(maxIndex);
        }
    }

    private void DestroyItem(GameObject obj)
    {
        //GameObject.Destroy(obj);
        ReturnGameObejct(obj);
    }

    private void ReturnGameObejct(GameObject go)
    {
        go.TrySetActive(false);

        if (!m_GoTag.ContainsKey(go))
            return;

        var tag = m_GoTag[go];
        RemoveOutMark(go);
        if (m_Pool.ContainsKey(tag))
            m_Pool[tag].Enqueue(go);
        else
        {
            var array = new Queue<GameObject>();
            array.Enqueue(go);
            m_Pool.Add(tag, array);
        }
    }

    private GameObject RequestGameObejct(GameObject prefab)
    {
        var tag = prefab.GetInstanceID();
        GameObject go = GetFromPool(tag);
        if (null == go)
            go = GameObject.Instantiate<GameObject>(prefab, _content);

        MarkAsOut(go, tag);

        return go;
    }

    private GameObject GetFromPool(int tag)
    {
        if (m_Pool.ContainsKey(tag) && m_Pool[tag].Count > 0)
        {
            GameObject obj = m_Pool[tag].Dequeue();
            obj.TrySetActive(true);

            return obj;
        }
        else
            return null;
    }

    private void MarkAsOut(GameObject go, int tag)
    {
        m_GoTag.Add(go, tag);
    }

    private void RemoveOutMark(GameObject go)
    {
        if (m_GoTag.ContainsKey(go))
        {
            m_GoTag.Remove(go);
        }
        else
        {
            Log.Error("remove out mark error, gameObject has not been marked");
        }
    }

    private void ForceRebuildLayoutImmediate(RectTransform rect)
    {
        RectTransform childrect = null;
        for (int i = 0; i < rect.childCount; i++)
        {
            childrect = rect.GetChild(i).GetComponent<RectTransform>();
            if (childrect != null)
            {
                ForceRebuildLayoutImmediate(childrect);
            }
            childrect = null;
        }

        if (rect.GetComponent<ContentSizeFitter>() != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }

    private void DestoryAll()
    {
        if (m_Pool != null)
        {
            foreach (var queues in m_Pool)
            {
                foreach (var go in queues.Value)
                {
                    if (go != null)
                        GameObject.Destroy(go);
                }
            }
            m_Pool.Clear();
        }

        if (m_GoTag != null)
        {
            foreach (var it in m_GoTag)
            {
                var go = it.Key;
                if (go != null)
                    GameObject.Destroy(go);
            }

            m_GoTag.Clear();
        }
    }

    private void GetBestPerLine()
    {
        if (null == this._content)
            return;

        switch (_movement)
        {
            case Arrangement.Horizontal:
                {
                    this.maxPerLine = (int)((this._content.rect.height + this.spacingY - this._Padding.top) / (this.cellHeight + this.spacingY));

                    break;
                }
            case Arrangement.Vertical:
                {
                    this.maxPerLine = (int)((this._content.rect.width + this.spacingX - this._Padding.left) / (this.cellWidth + this.spacingX));

                    break;
                }
            default:
                break;
        }
    }

    private void GetBestViewCount()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                {
                    this.viewCount = Mathf.RoundToInt((this._ScrollRect.viewport.rect.width + this.spacingX - this._Padding.left) / (this.cellWidth + this.spacingX)) + 2;

                    break;
                }
            case Arrangement.Vertical:
                {
                    this.viewCount = Mathf.RoundToInt((this._ScrollRect.viewport.rect.height + this.spacingY - this._Padding.top) / (this.cellHeight + this.spacingY)) + 2;

                    break;
                }
            default:
                break;
        }
    }

    /// <summary>
    /// 检查并处理自适应高度或者宽度
    /// </summary>
    private void CheckAdaptiveSize()
    {
        if (!this.IsAdaptiveSize)
            return;

        this.GetDefaultSize();

        if (null == _rectTransform)
            return;

        if (this._default_size <= 0.0001f)
            return;

        switch (this._movement)
        {
            case Arrangement.Horizontal:
                {
                    float width = Mathf.Min(this._default_size, this._content.rect.width);
                    this._rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                    break;
                }
            case Arrangement.Vertical:
                {
                    float height = Mathf.Min(this._default_size, this._content.rect.height);
                    this._rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

                    break;
                }
            default:
                break;
        }
    }

    private void GetDefaultSize()
    {
        if (null == _rectTransform)
            _rectTransform = this.GetComponent<RectTransform>();

        if (null == _rectTransform)
            return;

        if (this.IsAdaptiveSize && this._default_size <= 0)
        {
            switch (this._movement)
            {
                case Arrangement.Horizontal:
                    {
                        this._default_size = _rectTransform.rect.width;
                        break;
                    }
                case Arrangement.Vertical:
                    {
                        this._default_size = _rectTransform.rect.height;
                        break;
                    }
                default:
                    break;
            }
        }
    }

    private void RegisterEvent()
    {
        if (null != this._ScrollRect)
        {
            this._ScrollRect.onValueChanged.RemoveAllListeners();
            this._ScrollRect.onValueChanged.AddListener(this.OnValueChange);
        }
    }

    private void ItemSetCenter()
    {
        if (!this.SetCenter)
            return;

        float Pixel = (_content.rect.width - maxPerLine * (cellWidth + spacingX)) / 2;//算偏移值
        _content.offsetMin = new Vector2(_content.offsetMin.x + Pixel, _content.offsetMin.y);
        _content.offsetMax = new Vector2(_content.offsetMax.x + Pixel, _content.offsetMax.y);

    }

    #endregion

    [ContextMenu("ResetItems")]
    public void ResetItems()
    {
#if UNITY_EDITOR

        ResetScroller();
#endif
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsMoveCell) return;

        _lastPosition = eventData.position;
        _dragStartIndex = _selectIndex;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsMoveCell) return;
        if (Vector2.Distance(eventData.position, _lastPosition) < 100)
        {
            MoveToIndex(_selectIndex, false, true);
            return;
        }

        int index;
        var distanceX = eventData.position.x - _lastPosition.x;
        var distanceY = eventData.position.y - _lastPosition.y;

        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
        {
            //水平滑动
            index = distanceX < 0 ? _dragStartIndex + 1 : _dragStartIndex - 1;
        }
        else
        {
            //竖直滑动
            index = distanceY > 0 ? _dragStartIndex + 1 : _dragStartIndex - 1;
        }

        if (index < 0 || index > DataCount - 1)
        {
            MoveToIndex(_selectIndex, false, true);
            return;
        }

        _selectIndex = index;

        //UpdateAllItem();
        MoveToIndex(_selectIndex, false, true);
        OnDragMoveCell?.Invoke(_selectIndex);
    }

    private void WithValidScrollRect(System.Action<UIScrollRect> action)
    {
        if (_ScrollRect is UIScrollRect scrollRect)
        {
            action?.Invoke(scrollRect);
        }
        else
        {
            Debug.LogError("未配置正确的UIScrollRect");
        }
    }

    public void RegisterDragEvent(
        UIScrollRect.ScrollDragDelegate dragStartEvent,
        UIScrollRect.ScrollDragDelegate dragUpdateEvent,
        UIScrollRect.ScrollDragDelegate dragStopEvent)
    {
        WithValidScrollRect(scrollRect =>
        {
            if (dragStartEvent != null) scrollRect.OnDragStart += dragStartEvent;
            if (dragUpdateEvent != null) scrollRect.OnDragUpdate += dragUpdateEvent;
            if (dragStopEvent != null) scrollRect.OnDragStop += dragStopEvent;
        });
    }

    public void UnRegisterDragEvent(
        UIScrollRect.ScrollDragDelegate dragStartEvent,
        UIScrollRect.ScrollDragDelegate dragUpdateEvent,
        UIScrollRect.ScrollDragDelegate dragStopEvent)
    {
        WithValidScrollRect(scrollRect =>
        {
            if (dragStartEvent != null) scrollRect.OnDragStart -= dragStartEvent;
            if (dragUpdateEvent != null) scrollRect.OnDragUpdate -= dragUpdateEvent;
            if (dragStopEvent != null) scrollRect.OnDragStop -= dragStopEvent;
        });
    }
}
