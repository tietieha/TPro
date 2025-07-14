namespace UW
{
    /// <summary>
    /// 输入手势识别参数
    /// </summary>
    [System.Serializable]
    public struct InputControlParameters
    {
        /// <summary>
        /// 当手指在一个物品上停留至少这个时间而不动时，这个手势就被认为是一个长敲击
        /// </summary>
        public float ClickDurationThreshold;

        /// <summary>
        /// 当连续两次点击之间的时间短于此时间时，可以识别双击手势
        /// </summary>
        public float DoubleclickDurationThreshold;

        /// <summary>
        /// 当用户的手指移动的距离超过这个值时，就会开始拖动。
        /// 该值被定义为规范化值。拖动屏幕的整个宽度等于1。拖动整个屏幕的高度也等于1
        /// </summary>
        public float DragStartDistanceThresholdRelative;

        /// <summary>
        /// 拖动的时间阈值
        /// </summary>
        public float DragDurationThreshold;
    }
}
