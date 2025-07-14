using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Transform))]
public class DialogPopupScale : MonoBehaviour
{
    [Header("动画参数")]
    [Tooltip("从这个缩放开始")]
    public Vector3 fromScale = Vector3.zero;
    [Tooltip("最终目标缩放（通常为1,1,1）")]
    public Vector3 toScale = Vector3.one;
    [Tooltip("总时长（秒）")]
    [Range(0.1f, 1f)]
    public float duration = 0.35f;
    [Tooltip("回弹张力，1.2 左右对应峰值 ~1.05")]
    [Min(0f)]
    public float overshoot = 1.2f;
    [Tooltip("是否忽略 Time.timeScale")]
    public bool ignoreTimeScale = false;

    void OnEnable()
    {
        // 先重置到 fromScale
        transform.localScale = fromScale;
        // 一条 Tween，使用 OutBack 曲线带 overshoot
        transform
            .DOScale(toScale, duration)
            .SetEase(Ease.OutBack, overshoot)
            .SetUpdate(ignoreTimeScale);
    }
}