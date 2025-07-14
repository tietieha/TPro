using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Transform))]
public class PeriodicShake : MonoBehaviour
{
    [Header("摇动设置")]
    [Tooltip("每次摇动时长（秒）")]
    public float shakeDuration = 0.3f;
    [Tooltip("最小间隔（秒）")]
    public float minInterval = 3f;
    [Tooltip("最大间隔（秒）")]
    public float maxInterval = 8f;
    [Tooltip("摇动强度（度数）")]
    public float strength = 15f;
    [Tooltip("振动段数，越大越平滑")]
    public int vibrato = 10;
    [Tooltip("随机程度（0 = 无随机，每次偏移一致；越大偏移越不一致）")]
    public float randomness = 10f;
    [Tooltip("是否忽略 Time.timeScale")]
    public bool ignoreTimeScale = false;
    private Vector3 shakeVector = Vector3.zero;
    private Tween _timer;

    void Start()
    {
        shakeVector = new Vector3(0, 0, strength);
        ScheduleNext();
    }

    private void ScheduleNext()
    {
        float delay = Random.Range(minInterval, maxInterval);
        _timer = DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOShakeRotation(
                    shakeDuration,
                    shakeVector,
                    vibrato,
                    randomness,
                    fadeOut: true
                )
                .SetUpdate(ignoreTimeScale);
            ScheduleNext();
        }, ignoreTimeScale);
    }

    void OnDisable()
    {
        _timer?.Kill();
    }
}