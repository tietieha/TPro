using System;
using System.Collections.Generic;
using System.Threading;
using TEngine;

public abstract class ThreadTask
{
    public abstract void Process();
}

public class MultiThread
{
    public static int CurrentThreadCount = 0;

    private Thread thread;
    private AutoResetEvent wakeupEvent = new AutoResetEvent(false);
    private volatile bool stop;
    private Queue<ThreadTask> tasks = new Queue<ThreadTask>();

    public string Name { get; private set; }

    public MultiThread(string name)
    {
        Name = name;
        CurrentThreadCount++;
    }

    public void Start()
    {
        stop = false;
        thread = new Thread(ThreadProc)
        {
            Name = Name
        };
        thread.Start();
    }

    public void Stop()
    {
        stop = true;
        Wakeup();
        if (thread != null)
        {
            thread.Join();
            thread = null;
        }
        tasks.Clear();
    }

    public void AddTask<T>(T task) where T : ThreadTask
    {
        lock (tasks)
        {
            tasks.Enqueue(task);
        }

        Wakeup();
    }

    
    private void Sleep()
    {
        wakeupEvent.WaitOne();
    }

    private void Wakeup()
    {
        wakeupEvent.Set();
    }

    private void ThreadProc()
    {
        while (!stop)
        {
            ThreadTask task = null;
            lock (tasks)
            {
                if (tasks.Count > 0)
                {
                    task = tasks.Dequeue();
                }
            }

            if (task != null)
            {
                try
                {
                    task.Process();
                }
                catch (Exception e)
                {
                    Log.Error("[{0} Exception]{1}", Name, e);
                }
            }
            else
            {
                Sleep();
            }
        }
    }
}
