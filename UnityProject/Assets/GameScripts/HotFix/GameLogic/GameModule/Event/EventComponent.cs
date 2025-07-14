using System;
using TEngine;
using XLua;

public class EventComponent
{
    /// <summary>
    /// 检查订阅事件处理回调函数。
    /// </summary>
    /// <param name="id">事件类型编号。</param>
    /// <param name="handler">要检查的事件处理回调函数。</param>
    /// <returns>是否存在事件处理回调函数。</returns>
    public bool Check(int id, EventHandler<GameEventArgs> handler)
    {
        return GameApp.EventModule.Check(id, handler);
    }

    /// <summary>
    /// 订阅事件处理回调函数。
    /// </summary>
    /// <param name="id">事件类型编号。</param>
    /// <param name="handler">要订阅的事件处理回调函数。</param>
    public void Subscribe(int id, EventHandler<GameEventArgs> handler)
    {
        GameApp.EventModule.Subscribe(id, handler);
    }

    public void Subscribe(EventId id, EventHandler<GameEventArgs> handler)
    {
        GameApp.EventModule.Subscribe((int)id, handler);
    }

    /// <summary>
    /// 取消订阅事件处理回调函数。
    /// </summary>
    /// <param name="id">事件类型编号。</param>
    /// <param name="handler">要取消订阅的事件处理回调函数。</param>
    public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
    {
        GameApp.EventModule.Unsubscribe(id, handler);
    }

    public void Unsubscribe(EventId id, EventHandler<GameEventArgs> handler)
    {
        GameApp.EventModule.Unsubscribe((int)id, handler);
    }
    
    /// <summary>
    /// 设置默认事件处理函数。
    /// </summary>
    /// <param name="handler">要设置的默认事件处理函数。</param>
    public void SetDefaultHandler(EventHandler<GameEventArgs> handler)
    {
        GameApp.EventModule.SetDefaultHandler(handler);
    }

    /// <summary>
    /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
    /// </summary>
    /// <param name="sender">事件发送者。</param>
    /// <param name="e">事件内容。</param>
    public void Fire(object sender, GameEventArgs e)
    {
        GameApp.EventModule.Fire(sender, e);
        
        var func = GameApp.Lua.Env.CustomGlobal.GetInPath<LuaFunction>("BroadcastEvent.csDispatch");
        if (func != null)
        {
            func.Call(e.Id, sender, e);
        }
    }

    public void FireFromLua(object sender, GameEventArgs e)
    {
        GameApp.EventModule.Fire(sender, e);
    }

    /// <summary>
    /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
    /// </summary>
    /// <param name="sender">事件发送者。</param>
    /// <param name="e">事件内容。</param>
    public void FireNow(object sender, GameEventArgs e)
    {
        GameApp.EventModule.FireNow(sender, e);
    }
}
