using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class UIScrollRect : ScrollRect
    {
        
        public delegate void ScrollDragDelegate(PointerEventData eventData);
        public event ScrollDragDelegate OnDragStart;
        public event ScrollDragDelegate OnDragUpdate;
        public event ScrollDragDelegate OnDragStop;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            OnDragStart?.Invoke(eventData);
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            OnDragUpdate?.Invoke(eventData);
            base.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            OnDragStop?.Invoke(eventData);
        }
    }
}