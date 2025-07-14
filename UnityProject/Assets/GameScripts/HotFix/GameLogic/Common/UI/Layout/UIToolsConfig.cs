using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// UI 工具 [配置]
    /// </summary>
    public static class UIToolsConfig
    {
        /// <summary>
        /// [方向]
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// 横
            /// </summary>
            Horizontal,

            /// <summary>
            /// 竖
            /// </summary>
            Vertical,
        }

        #region 锚点

        /// <summary>
        /// [锚点 上左]
        /// </summary>
        public static Vector2 Anchor_UpperLeft = new Vector2(0.0f, 1.0f);

        /// <summary>
        /// [锚点 上中]
        /// </summary>
        public static Vector2 Anchor_UpperCenter = new Vector2(0.5f, 1.0f);

        /// <summary>
        /// [锚点 上右]
        /// </summary>
        public static Vector2 Anchor_UpperRight = new Vector2(1.0f, 1.0f);

        /// <summary>
        /// [锚点 中左]
        /// </summary>
        public static Vector2 Anchor_MiddleLeft = new Vector2(0.0f, 0.5f);

        /// <summary>
        /// [锚点 中中]
        /// </summary>
        public static Vector2 Anchor_MiddleCenter = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// [锚点 中右]
        /// </summary>
        public static Vector2 Anchor_MiddleRight = new Vector2(1.0f, 0.5f);

        /// <summary>
        /// [锚点 下左]
        /// </summary>
        public static Vector2 Anchor_LowerLeft = new Vector2(0.0f, 0.0f);

        /// <summary>
        /// [锚点 下中]
        /// </summary>
        public static Vector2 Anchor_LowerCenter = new Vector2(0.5f, 0.0f);

        /// <summary>
        /// [锚点 下右]
        /// </summary>
        public static Vector2 Anchor_LowerRight = new Vector2(1.0f, 0.0f);

        #endregion
    }
}