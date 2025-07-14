using UnityEngine;

namespace UW
{
    public interface IInputController
    {
        void Initialize();

        void Update(float deltaTime);

        Vector3 GetCurrentInputPosition();

        void ResetDrag();

        void Clear();
    }
}
