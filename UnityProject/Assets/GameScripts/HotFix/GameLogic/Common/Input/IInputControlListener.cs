
using UnityEngine;

namespace UW
{
    /// <summary>
    /// 输入控制的手势识别事件
    /// </summary>
    public interface IInputControlListener
    {
        void InputDragStart(Vector3 position, bool isLongTap, Vector3 offset);
        void TouchDown(Vector3 position);
        void TouchUp(Vector3 position);
        void DragUpdate(Vector3 dragPositionStart, Vector3 dragPositionCurrent, Vector3 correctionOffset);
        void DragStop(Vector3 dragStopPosition, Vector3 dragFinalMomentum);
        void PinchStart(Vector3 pinchCenter, float pinchDistance);
        void PinchUpdate(Vector3 pinchCenter, float pinchDistance, float pinchStartDistance);
        void PinchStop();
        void ProgressLongTap(float progress);
        void InputClick(Vector3 clickPosition, bool isDoubleClick, bool isLongTap);
        Vector3 GetCurrentInputPosition();
    }
}
