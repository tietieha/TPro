﻿using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// 调试器窗口接口。
    /// </summary>
    public interface IDebuggerWindow
    {
        /// <summary>
        /// 初始化调试器窗口。
        /// </summary>
        /// <param name="args">初始化调试器窗口参数。</param>
        void Initialize(params object[] args);

        /// <summary>
        /// 关闭调试器窗口。
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 进入调试器窗口。
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 离开调试器窗口。
        /// </summary>
        void OnLeave();

        /// <summary>
        /// 调试器窗口轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 调试器窗口绘制。
        /// </summary>
        /// <param name="contentRect"></param>
        /// <param name="touch"></param>
        /// <param name="windowScale"></param>
        void OnDraw(Rect contentRect, Vector4 touch, float windowScale);
    }
}
