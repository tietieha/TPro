using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineAni : MonoBehaviour
{
    [SerializeField]
    public AnimationCurve _camerFOV;  // ????????
    public float _aniTime = 0.75f;
    private float _default = 1f;
    private CinemachineVirtualCamera _virtualCamera;
    private bool _isPlay = false;
    private float _time = 0;
    private float _initFov = 30f;
    private float _targetFov = 30f;
    private float _dis = 30f;
    // Use this for initialization
    void OnEnable()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _isPlay = true;
        _time = 0;
    }

    public void SetAniParam(float init_fov, float target_fov )
    {
        _initFov = init_fov;
        _targetFov = target_fov;
        _dis = _targetFov - _initFov;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        GetAnimationCurveValue();
    }
    private float GetBaseFOV(float frameValue)
    {
        return _initFov + _dis * frameValue;
    }
    private void GetAnimationCurveValue()
    {
        if (!_isPlay)
        {
            return;
        }
        if (_time < _aniTime)
        {
            _time += Time.deltaTime;
            float frameValue = _time / _aniTime * _default;
            float value = _camerFOV.Evaluate(frameValue);
            _virtualCamera.m_Lens.FieldOfView = GetBaseFOV(frameValue) * value;
        }
        else
        {
            _isPlay = false;
        }
    }

}
