using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// 游戏框架模块抽象类。
    /// todo: 生命周期管理
    /// </summary>
    public abstract class Module : MonoBehaviour
    {
        public bool IsInit => m_IsInit;
        protected bool m_IsInit = false;
        /// <summary>
        /// 游戏框架模块初始化。
        /// </summary>
        protected virtual void Awake()
        {
            ModuleSystem.RegisterModule(this);
        }

        public virtual void Init()
        {
            m_IsInit = true;
        }
    }
}