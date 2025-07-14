using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;
    
    public GameObject _effect;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPointerDown(Input.mousePosition);
        }
    }

    public void OnPointerDown(Vector3 position)
    {
        PlayTouchEffect(position);
    }
    
    private void PlayTouchEffect(Vector3 position)
    {
        if (_effect != null && _uiCamera != null)
        {
            _effect.SetActive(false);
            _effect.transform.position = _uiCamera.ScreenToWorldPoint(position);
            _effect.SetActive(true);
        }
    }
}
