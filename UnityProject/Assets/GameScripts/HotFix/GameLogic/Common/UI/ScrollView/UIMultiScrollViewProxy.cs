using System;
//using Shelter.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/**
 * add by sunliwen
 * 目的：同时有两个scrollView ，滑动子scrollView的时候也可以触发滑动父scrollView,滑动父scrollView的时候也可以滑动子scrollView
 * 实现：
 * 分为两种操作：向上滑动和向下滑动
 * 向上滑动的时候看看父scrollView滑到底没有，没滑到底继续滚动父scrollView，如果滑到底了就滚动子scrollview
 * 向下滑动的时候看看子scrollview滑到顶没有，没滑到顶继续滑动子scrollview,如果滑到顶了就滚动父scrollview
 *
 * 注意：在滑动时候没抬起手指的时候也即OnDrag()的时候要注意达到临界点时父scrollview和子scrollView的平滑过度
 *
 * 挂载的时候3个地方需要挂
 * 1、父scrollview viewport节点
 * 2、子scrollview viewport节点
 * 3、子scrollview content节点
 *   
 * 例子：可以参考联盟排行榜
 */
public class UIMultiScrollViewProxy : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IScrollHandler
{
    [SerializeField] private ScrollRect parentScrollView;
    [SerializeField] private ScrollRect childScrollView;

    private bool isDraggingParent = false;
    private bool isDraggingChild = false;
    private Vector2 pointerStartLocalCursor = Vector2.zero;
    private bool isDragging = false;

    private void OnDisable()
    {
        isDragging = false;
        isDraggingParent = false;
        isDraggingChild = false;
    }

    private bool IsReachTop(ScrollRect scrollView)
    {
        return scrollView.content.localPosition.y <= 4;
    }

    private bool IsReachBottom(ScrollRect scrollView)
    {
        var content = scrollView.content;
        return content.sizeDelta.y - content.anchoredPosition.y < scrollView.viewport.rect.height + 5;
    }

        
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parentScrollView.viewport, eventData.position, eventData.pressEventCamera, out localCursor))
            return;

        var deltaPos = localCursor - pointerStartLocalCursor;
        
        if (!isDraggingParent && !isDraggingChild)
        {
            if (deltaPos.y > 0)//向上滑动
            {
                if (IsReachBottom(parentScrollView))
                {
                    childScrollView.OnBeginDrag(eventData);
                    isDraggingChild = true;
                }
                else
                {
                    parentScrollView.OnBeginDrag(eventData);
                    isDraggingParent = true;
                }
            }
            else if (deltaPos.y < 0)//向下滑动
            {
                if (IsReachTop(childScrollView))
                {
                    parentScrollView.OnBeginDrag(eventData);
                    isDraggingParent = true;
                }
                else
                {
                    childScrollView.OnBeginDrag(eventData);
                    isDraggingChild = true;
                }
            }
        }

        if (isDraggingParent)
        {
            parentScrollView.OnDrag(eventData);
            if (deltaPos.y > 0)
            {
                if(IsReachBottom(parentScrollView))
                {
                    isDraggingParent = false;
                    isDraggingChild = true;
                    parentScrollView.OnEndDrag(eventData);
                    childScrollView.OnBeginDrag(eventData);
                }
            }
            
        }

        if (isDraggingChild)
        {
            childScrollView.OnDrag(eventData);
            if (deltaPos.y < 0)
            {
                if (IsReachTop(childScrollView))
                {
                    isDraggingChild = false;
                    isDraggingParent = true;
                    childScrollView.OnEndDrag(eventData);
                    parentScrollView.OnBeginDrag(eventData);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        pointerStartLocalCursor = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentScrollView.viewport, eventData.position, eventData.pressEventCamera, out pointerStartLocalCursor);
        isDragging = true;
        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        isDragging = false;
        isDraggingParent = false;
        isDraggingChild = false;
        parentScrollView.OnEndDrag(eventData);
        childScrollView.OnEndDrag(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!this.enabled)
            return;
        Vector2 delta = eventData.scrollDelta;
        
        if (delta.y > 0)//向下滚动
        {
            if (IsReachTop(childScrollView))
            {
                parentScrollView.OnScroll(eventData);
            }
            else
            {
                childScrollView.OnScroll(eventData);
            }
        }
        else if (delta.y < 0)//向上滚动
        {
            if (IsReachBottom(parentScrollView))
            {
                childScrollView.OnScroll(eventData);
            }
            else
            {
                parentScrollView.OnScroll(eventData);
            }
        }
    }
}