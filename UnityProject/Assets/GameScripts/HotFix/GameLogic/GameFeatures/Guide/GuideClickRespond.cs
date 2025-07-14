using UnityEngine;
using UnityEngine.EventSystems;

public class GuideClickRespond : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPress != null)
            GameApp.Event.Fire(this, new CommonEventArgs(EventId.GUIDE_REMOVE_GUIDE_WEAK, eventData.pointerPress.GetInstanceID()));
    }
}
