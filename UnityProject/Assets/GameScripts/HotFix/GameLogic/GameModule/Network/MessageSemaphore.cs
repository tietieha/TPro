using System.Threading;

/// <summary>
/// 消息信号量
/// 处理多线程共享资源，类似互斥锁
/// </summary>
class MessageSemaphore
{
    int mResource;
    object lockObj = null;

    public MessageSemaphore()
    {
        mResource = 0;
        lockObj = new object();
    }

    public void WaitResource()
    {
        WaitResource(1);
    }

    public void WaitResource(int count)
    {
        while (true)
        {
            lock (lockObj)
            {
                if (mResource >= count)
                {
                    mResource -= count;
                    return;
                }
            }
            lock (this)
            {
                //释放锁、进入等待队列
                Monitor.Wait(this);
            }
        }
    }

    public void ProduceResrouce()
    {
        ProduceResrouce(1);
    }

    public void ProduceResrouce(int count)
    {
        lock (lockObj)
        {
            mResource += count;
        }

        lock (this)
        {
            //释放锁，并唤醒等待队列中的线程使其进入就绪队列
            Monitor.Pulse(this);
        }
    }
}

