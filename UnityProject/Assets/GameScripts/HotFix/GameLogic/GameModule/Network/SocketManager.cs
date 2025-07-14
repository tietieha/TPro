using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TEngine;
using UnityEngine;
using XLua;

//socket状态
public enum ESocket
{
    Closed = 0,//关闭
    Connecting,//连接中
    Connected,//已连接
}

/// <summary>
/// Socket网络套接字管理器
/// </summary>
public class SocketManager : DynamicModule
{
    private const string Event_Socket_Connect_Success = "NETWORK_SOCKET_CONNECT_SUCCESS";
    private const string Event_Socket_Connect_Failure = "NETWORK_SOCKET_CONNECT_FAILURE";
    private const string Event_Force_Heart_Beat = "NETWORK_FORCE_HEART_BEAT";
    private const string Event_Return_Login = "RETURN_LOGIN";

    private const int CONNECT_TIME_OUT = 30;//连接超时 秒

    private const int MAX_RECEIVE_BUFFER = 1024 * 1024 * 2;//单次接收字节最大长度
    private const int MSG_MAGIC_LENGTH = 2;//解码字节数
    private const int MSG_ID_LENGTH = 4;//协议号字节数
    private const int MSG_SEQUENCE_LENGTH = 4;//消息序号字节数
    private const int MSG_BODY_LENGTH = 4;//包体长度字节数
    int msgHeadLength = MSG_MAGIC_LENGTH + MSG_ID_LENGTH + MSG_SEQUENCE_LENGTH + MSG_BODY_LENGTH;//包头长度

    public bool IsConnect()
    {
        return mStatus == ESocket.Connected;
    }
    private ESocket mStatus = ESocket.Closed;//连接状态

    private Socket clientSocket = null;
    private Thread receiveThread = null;//接收线程
    private Thread sendThread = null;//发送线程
    private bool receiveWork = false;//接收线程工作中
    private bool sendWork = false;//发送线程工作中

    private IMessageQueue receiveMsgQueue = null;//接收消息队列（内部已加锁）
    private IMessageQueue sendMsgQueue = null;//发送消息队列（内部已加锁）
    private MessageSemaphore messageSemaphore = null;//发送消息的信号量
    private List<byte[]> tempMsgList = null;//发送给Lua层的消息列表

    private bool _sendConnectSuccessed = false;

    public List<string> listSendError = null;//发送线程异常列表
    object sendErrorLock = null;
    private float connectingTime = 0;

    private object closeLock = new object(); // 新增状态锁

    #region 异常处理

    // 添加异常信息结构
    private struct NetworkExceptionInfo
    {
        public Exception exception;
        public bool isFatal;
        public string errorMsg;
    }

    // 在主线程处理的异常队列
    private Queue<NetworkExceptionInfo> mainThreadExceptionQueue = new Queue<NetworkExceptionInfo>();
    private object exceptionQueueLock = new object(); // 异常队列锁

    #endregion


    //构造函数
    public SocketManager()
    {
        receiveMsgQueue = new MessageQueue();
        sendMsgQueue = new MessageQueue();
        tempMsgList = new List<byte[]>();
        messageSemaphore = new MessageSemaphore();

        listSendError = new List<string>();
        sendErrorLock = new object();
    }

    #region 生命周期

    private void Update()
    {
        UpdatePacket();
        ProcessExceptionQueue();
        UpdateConnectingTimeOut(GameTime.deltaTime, GameTime.unscaledDeltaTime);
        if (_sendConnectSuccessed)
        {
            LuaNetworkMgrBridge.Invoke(Event_Socket_Connect_Success, String.Empty);
            _sendConnectSuccessed = false;
        }
    }

    private float _pausedTime = 0;
    private void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            if (_pausedTime!=0 && Time.realtimeSinceStartup - _pausedTime>260f)
            {
                _pausedTime = 0;
                // GameApp.Event.Fire(this, new CommonEventArgs(EventId.RETURN_LOGIN));
                LuaNetworkMgrBridge.Invoke(Event_Return_Login, String.Empty);
            }
            else if (mStatus == ESocket.Connected)
            {
                // 先检查TCP实际连接状态
                if (!IsSocketAlive())
                {
                    Disconnect("Background connection loss");
                    return;
                }
                //切回前台 强制发送心跳判断是否还连接着
                // GameApp.Event.Fire(this, new CommonEventArgs(EventId.NETWORK_FORCE_HEART_BEAT));
                LuaNetworkMgrBridge.Invoke(Event_Force_Heart_Beat, String.Empty);
                Log.Info("切回前台，发起强制心跳检测");
            }
        }
        else
        {
            _pausedTime = Time.realtimeSinceStartup;
        }

    }

    // 检测socket真实连接状态
    private bool IsSocketAlive()
    {
        try
        {
            if (clientSocket == null) return false;

            // 双检查法确认连接状态
            return !(clientSocket.Poll(1000, SelectMode.SelectRead) &&
                     clientSocket.Available == 0);
        }
        catch
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        doClose();
    }

    private void OnApplicationQuit()
    {
        doClose();

        System.Threading.Thread.Sleep(200);
    }


    #endregion

    #region 公共方法

    //连接网络
    public void Connect(string host, int port)
    {
        doClose();
        if (mStatus != ESocket.Closed) return;
        try
        {
            Log.Debug("连接地址：{0} 连接端口：{1}", host, port);
            //判断IP6或IP4
            IPAddress[] address = Dns.GetHostAddresses(host);
            if (address.Length == 0)
            {
                Log.Info("SocketManager host invalid");
                return;
            }
            //创建套接字
            if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
            {
                clientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            connectingTime = 0;
            mStatus = ESocket.Connecting;

            //异步连接
            IAsyncResult result = clientSocket.BeginConnect(host, port, new AsyncCallback(onConnect_Sucess), clientSocket);
        }
        catch (SocketException ex)
        {
            bool isFatal = IsFatalSocketError(ex.SocketErrorCode);
            HandleNetworkException(ex, isFatal);
        }
        catch (Exception ex)
        {
            HandleNetworkException(ex, true);
        }
    }

    public void Disconnect(string err = "")
    {
        lock (closeLock)
        {
            if (mStatus != ESocket.Closed)
            {
                doClose();
                LuaNetworkMgrBridge.Invoke(Event_Socket_Connect_Failure, err);
            }
        }
    }

    //强制下线
    public void ForceDisconnect()
    {
        lock (closeLock)
        {
            if (mStatus != ESocket.Closed)
                doClose();
        }
    }

    //发送消息
    public void SendMsg(byte[] msgObj)
    {
        sendMsgQueue.Add(msgObj);
        messageSemaphore.ProduceResrouce();
    }

    public void ClearThread()
    {
        doClose();
    }

    #endregion

    //连接成功
    void onConnect_Sucess(IAsyncResult iar)
    {
        Log.Info("Enter onConnect");
        try
        {
            Socket client = (Socket)iar.AsyncState;
            client.EndConnect(iar);
            if (client.Connected)
            {
                mStatus = ESocket.Connected;
                Log.Debug("SocketManager 连接成功 线程开启");
                //开启消息接收线程
                if (receiveThread == null || !receiveThread.IsAlive)
                    receiveThread = new Thread(onReceiveThread);
                receiveThread.IsBackground = true;
                receiveWork = true;
                receiveThread.Start(null);

                //开启发送线程
                if (sendThread == null || !sendThread.IsAlive)
                    sendThread = new Thread(onSendThread);
                sendThread.IsBackground = true;
                sendWork = true;
                sendThread.Start(null);

                _sendConnectSuccessed = true;
            }
            else
            {
                Log.Info("onConnect_Sucess ESocket.Closed");
                mStatus = ESocket.Closed;
            }
        }
        catch (SocketException ex)
        {
            bool isFatal = IsFatalSocketError(ex.SocketErrorCode);
            HandleNetworkException(ex, isFatal);
        }
        catch (Exception ex)
        {
            HandleNetworkException(ex, true);
        }
    }

    //监听服务器消息
    void onReceiveThread(object o)
    {
        StreamBuffer receiveStreamBuffer = StreamBufferPool.GetStream(MAX_RECEIVE_BUFFER, false, true);
        int bufferCurLen = 0;//当前缓存buff的字节长度
        while (receiveWork)
        {
            try
            {
                if (clientSocket == null || !clientSocket.Connected)
                    continue;

                if (clientSocket.Poll(100000, SelectMode.SelectRead))
                {
                    int available = clientSocket.Available;
                    if (available > 0)
                    {
                        int bufferLeftLen = receiveStreamBuffer.size - bufferCurLen; //计算缓存可存大小
                        byte[] result = receiveStreamBuffer.GetBuffer();
                        int readLen = clientSocket.Receive(result, bufferCurLen,
                            Math.Min(bufferLeftLen, available), SocketFlags.None);
                        if (readLen == 0)
                            throw new ObjectDisposedException("DisposeEX", "receive from server 0 bytes,closed it");
                        if (readLen < 0) throw new Exception("Unknow exception, readLen < 0" + readLen);

                        bufferCurLen += readLen; //当前缓存总大小
                        DoReceive(receiveStreamBuffer, ref bufferCurLen);
                    }
                }
                else if (!clientSocket.Connected) // 连接断开检测
                {
                    throw new SocketException((int)SocketError.NotConnected);
                }
            }
            catch (SocketException ex)
            {
                // 连接断开等错误视为可恢复错误
                HandleNetworkException(ex);
            }
            catch (Exception ex)
            {
                if (mStatus == ESocket.Closed || clientSocket == null)
                    break;
                // 其他异常视为致命错误
                HandleNetworkException(ex, true);
                break;
            }
        }

        StreamBufferPool.RecycleStream(receiveStreamBuffer);
    }

    //处理包体内容
    void DoReceive(StreamBuffer streamBuffer, ref int bufferCurLen)
    {
        // 长度信息法解决拆包粘包问题
        //服务器约定消息规则  包头+包体 =  包头（4字节消息长度 + 2字节Mgaic + 4字节协议 + 4字节消息序列） + 包体
        try
        {
            byte[] data = streamBuffer.GetBuffer();//内部已加锁
            int start = 0;
            streamBuffer.ResetStream();//重置内存position

            // 取消息长度
            while (true)
            {
                //判断长度是否不小于4个字节（包头14个字节）
                if (bufferCurLen - start < sizeof(short) + sizeof(int) * 3)
                {
                    break;
                }

                // 现在用消息长度，且放在最前面
                int msgLen = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, start));

                //消息不满足长度，消息被拆包了，等待下次粘包
                if (msgLen > bufferCurLen - start - MSG_BODY_LENGTH)
                {
                    Log.Debug("SocketManager 消息被拆包了");
                    break;
                }

                byte[] bytes = streamBuffer.ToArray(start, MSG_BODY_LENGTH + msgLen);
                receiveMsgQueue.Add(bytes);

                // 下一次组包
                start += MSG_BODY_LENGTH + msgLen;
            }

            //清除已入队列的数据
            if (start > 0)
            {
                bufferCurLen -= start;
                streamBuffer.CopyFrom(data, start, 0, bufferCurLen);
            }
        }
        catch (Exception ex)
        {
            Log.Error("SocketManager receive package err : {0}\n {1}", ex.Message, ex.StackTrace);
            onSocketClosed(ex.Message);
        }
    }

    void onSendThread(object o)
    {
        List<byte[]> workList = new List<byte[]>(10);

        while (sendWork)
        {
            if (!sendWork)
            {
                break;
            }

            if (clientSocket == null || !clientSocket.Connected)
            {
                continue;
            }

            messageSemaphore.WaitResource();
            if (sendMsgQueue.Empty())
            {
                continue;
            }

            sendMsgQueue.MoveTo(workList);
            try
            {
                //每帧清空消息列表（暂未做数量限制）
                for (int k = 0; k < workList.Count; ++k)
                {
                    var msgObj = workList[k];
                    if (sendWork)
                    {
                        clientSocket.Send(msgObj, msgObj.Length, SocketFlags.None);
                    }
                }
            }
            catch (SocketException ex)
            {
                HandleNetworkException(ex);
            }
            catch (Exception ex)
            {
                HandleNetworkException(ex, true);
            }
            finally
            {
                for (int k = 0; k < workList.Count; ++k)
                {
                    var msgObj = workList[k];
                    StreamBufferPool.RecycleBuffer(msgObj);
                }
                workList.Clear();
            }
        }
    }

    private void HandleNetworkException(SocketException ex)
    {
        HandleNetworkException(ex, IsFatalSocketError(ex.SocketErrorCode));
    }

    /// <summary>
    /// 处理网络异常
    /// </summary>
    private void HandleNetworkException(Exception ex, bool isFatal)
    {
        string errorMsg = ex.Message;

        // 在主线程记录错误
        lock (exceptionQueueLock)
        {
            mainThreadExceptionQueue.Enqueue(new NetworkExceptionInfo {
                exception = ex,
                isFatal = isFatal,
                errorMsg = errorMsg
            });
        }

        Log.Info($"[Socket] Network IsFatal: {isFatal.ToString()}, exception: {errorMsg}");
    }

    /// <summary>
    /// 判断是否为致命错误
    /// </summary>
    private bool IsFatalSocketError(SocketError error)
    {
        // 这些错误通常是配置错误或协议错误，无法通过重连解决
        return error == SocketError.AccessDenied ||
               error == SocketError.AddressFamilyNotSupported ||
               error == SocketError.AddressNotAvailable ||
               error == SocketError.ProtocolFamilyNotSupported ||
               error == SocketError.ProtocolNotSupported ||
               error == SocketError.ProtocolOption ||
               error == SocketError.ProtocolType ||
               error == SocketError.SocketNotSupported ||
               error == SocketError.OperationNotSupported ||
               error == SocketError.InvalidArgument ||
               error == SocketError.Fault ||
               error == SocketError.NoRecovery;
    }

    /// <summary>
    /// 在主线程处理所有异常
    /// </summary>
    private void ProcessExceptionQueue()
    {
        lock (exceptionQueueLock)
        {
            while (mainThreadExceptionQueue.Count > 0)
            {
                var exInfo = mainThreadExceptionQueue.Dequeue();
                // 带上堆栈
                string errorMsg = $"[Socket] Exception: {exInfo.exception.Message}\n{exInfo.exception.StackTrace}";
                bool isFatal = exInfo.isFatal;

                Log.Debug($"[Socket] Main thread exception: {errorMsg}, IsFatal: {isFatal}");

                if (isFatal)
                {
                    _sendConnectSuccessed = false;
                    onSocketClosed(errorMsg);
                    break;
                }
            }
        }
    }


    //更新包 每帧传递给Lua层消息
    void UpdatePacket()
    {
        if (!receiveMsgQueue.Empty())
        {
            receiveMsgQueue.MoveTo(tempMsgList);//内部已加锁
            try
            {
                for (int i = 0; i < tempMsgList.Count; ++i)
                {
                    var objMsg = tempMsgList[i];
                    luaReceiveMsg(objMsg);//把数据转给lua解析 tip：不用事件是因为数据会为空
                }
            }
            catch (Exception e)
            {
                Log.Error("SocketManager UpdatePacket Error :{0} \n {1}", e.Message,e.StackTrace);
            }
            finally
            {
                for (int i = 0; i < tempMsgList.Count; ++i)
                {
                    StreamBufferPool.RecycleBuffer(tempMsgList[i]);
                }
                tempMsgList.Clear();
            }
        }
    }

    void UpdateConnectingTimeOut(float elapsedTime, float realElapsedTime)
    {
        if (mStatus != ESocket.Connecting)
            return;

        connectingTime += elapsedTime;
        if (connectingTime >= CONNECT_TIME_OUT)
        {
            onSocketClosed("连接超时");
        }
    }

    //异常断开
    void onSocketClosed(string msg = "")
    {
        lock (closeLock)
        {
            // 状态检查避免重复断开
            if (mStatus == ESocket.Closed || clientSocket == null)
                return;

            Log.Error("异常断开:{0}", msg);
            doClose();
            LuaNetworkMgrBridge.Invoke(Event_Socket_Connect_Failure, msg);
        }
    }

    //断开 清理socket
    public void doClose()
    {
        lock (closeLock)
        {
            Log.Info($"断开连接，清理socket {mStatus}");
            if (mStatus == ESocket.Closed) return;
            // 标记为已关闭状态
            mStatus = ESocket.Closed;

            // 1. 设置停止标志
            receiveWork = false;
            sendWork = false;

            // 2. 先关闭Socket唤醒阻塞线程
            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket.Dispose();
                }
            }
            catch { /* 忽略关闭异常 */ }
            finally
            {
                clientSocket = null;
            }

            // 3. 唤醒发送线程
            messageSemaphore.ProduceResrouce();

            // 4. 安全等待线程退出（增加等待时间）
            try
            {
                if (receiveThread != null && receiveThread.IsAlive)
                {
                    receiveThread.Interrupt(); // 安全中断
                    if (!receiveThread.Join(500)) // 增加至500ms
                    {
                        Log.Warning("Receive thread exit timeout");
                    }
                }

                if (sendThread != null && sendThread.IsAlive)
                {
                    if (!sendThread.Join(500))
                    {
                        sendThread.Interrupt();
                    }
                }
            }
            catch { /* 忽略线程操作异常 */ }

            // 清空队列
            receiveMsgQueue.Dispose();
            sendMsgQueue.Dispose();
        }
    }

    #region C#调用Lua
    [CSharpCallLua] public delegate void LuaReceiveMsg(byte[] bytes);
    private LuaReceiveMsg m_LuaReceiveMsg;
    private LuaReceiveMsg luaReceiveMsg => m_LuaReceiveMsg ??= GameApp.Lua.GetGlobal<LuaReceiveMsg>("OnReceiveMsg");

    private Action<string, string> _luaNetworkMgrBridge;
    public Action<string, string> LuaNetworkMgrBridge
    {
        get
        {
            if (_luaNetworkMgrBridge == null)
            {
                _luaNetworkMgrBridge = GameApp.Lua.GetGlobal<Action<string, string>>("NetworkMgrBridge");
            }

            return _luaNetworkMgrBridge;
        }
    }


    #endregion

}
