using UnityEngine;

namespace UW
{
    public class InputController_Empty : IInputController
    {
        public void Initialize()
        {
        }

        public void Update(float deltaTime)
        {
        }

        public void Clear()
        {
        }

        public void ResetDrag()
        {
        }

        public Vector3 GetCurrentInputPosition()
        {
            return Vector3.zero;
        }
    }
}
