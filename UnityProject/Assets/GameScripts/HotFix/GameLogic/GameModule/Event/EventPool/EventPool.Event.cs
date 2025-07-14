/// <summary>
/// 事件池模式。
/// </summary>

public enum EventPoolMode
{
    /// <summary>
    /// 默认事件池模式，即必须存在有且只有一个事件处理函数。
    /// </summary>
    Default = 0,

    /// <summary>
    /// 允许不存在事件处理函数。
    /// </summary>
    AllowNoHandler = 1,

    /// <summary>
    /// 允许存在多个事件处理函数。
    /// </summary>
    AllowMultiHandler = 2,

    /// <summary>
    /// 允许存在重复的事件处理函数。
    /// </summary>
    AllowDuplicateHandler = 4,
}

public partial class EventPool<T>
{
    /// <summary>
    /// 事件结点。
    /// </summary>
    public class Event
    {
        private readonly object m_Sender;
        private readonly T m_EventArgs;

        public Event(object sender, T e)
        {
            m_Sender = sender;
            m_EventArgs = e;
        }

        public object Sender
        {
            get
            {
                return m_Sender;
            }
        }

        public T EventArgs
        {
            get
            {
                return m_EventArgs;
            }
        }
    }
}

