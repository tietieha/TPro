using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

using SQLite4Unity3d;
using TEngine;
using UnityEngine;

using XLua;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    public static readonly object LockObj = new object();

    public static DatabaseManager Instance
    {
        get
        {
            // Double-Checked Locking
            if (_instance == null)
            {
                lock (LockObj)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<DatabaseManager>();

                        if (FindObjectsOfType<DatabaseManager>().Length > 1)
                        {
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject("(Singleton) " + typeof(DatabaseManager));
                            _instance = singleton.AddComponent<DatabaseManager>();

                            DontDestroyOnLoad(singleton);
                        }
                    }
                }
            }

            return _instance;
        }
    }

    private System.Action<bool> OnInitCallback;

    public void Initialize(string databaseFile, System.Action<bool> callback)
    {
        OnInitCallback = callback;
        string dstFilePath = Path.Combine(Application.persistentDataPath, databaseFile);
        int pos = dstFilePath.LastIndexOf("/");
        if(pos > 0 )
        {
            string dstPath = dstFilePath.Substring(0, pos);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
            }
        }

        InitDatabase(dstFilePath);
     
    }

    public void Release()
    {
        isInited = false;
        taskQueue.Clear();
        if(thread != null)
        {
            thread.Stop();
            thread = null;
        }

        if (dbConnection != null)
            dbConnection.Close();
    }

    private MultiThread thread;
    private SQLiteConnection dbConnection;
    private Queue<DatabaseActionTask> taskQueue = new Queue<DatabaseActionTask>();

    private bool isInited;

    void InitDatabase(string path)
    {
        dbConnection = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        if (dbConnection != null)
        {
            thread = new MultiThread("DatabaseThread");
            thread.Start();

            isInited = true;
        }

        OnInitCallback?.Invoke(isInited);
    }

    private void Update()
    {
        UpdateTask();
    }

    public void UpdateTask()
    {
        if (thread == null)
            return ;

        if (taskQueue.Count > 0)
        {
            if (taskQueue.Peek().Processed)
            {
                try
                {
                    taskQueue.Dequeue().CallBack();
                } catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }

    public void DropTable<T>(Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        DropTableTask<T> task = new DropTableTask<T>(dbConnection, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void CreateTable<T>(Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        CreateTableTask<T> task = new CreateTableTask<T>(dbConnection, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void Insert<T>(T obj, string extra = "", Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        InsertTask<T> task = new InsertTask<T>(dbConnection, obj, extra, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void InsertOrReplace<T>(T obj, Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        InsertOrReplaceTask<T> task = new InsertOrReplaceTask<T>(dbConnection, obj, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void InsertAll<T>(List<T> obj, string extra = "", Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        List<T> clone = new List<T>();
        clone.AddRange(obj);

        InsertAllTask<T> task = new InsertAllTask<T>(dbConnection, clone, extra, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void InsertOrReplaceAll<T>(List<T> obj, Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        List<T> clone = new List<T>();
        clone.AddRange(obj);

        InsertOrReplaceAllTask<T> task = new InsertOrReplaceAllTask<T>(dbConnection, clone, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void Delete<T>(object primaryKey, Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        DeleteTask<T> task = new DeleteTask<T>(dbConnection, primaryKey, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }
    
    
    public void DeleteAll<T>(Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        DeleteAllTask<T> task = new DeleteAllTask<T>(dbConnection, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }
    
    public void Execute<T>(string cmdStr, Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        ExecuteTask<T> task = new ExecuteTask<T>(dbConnection, cmdStr, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void ExecuteScalar<T>(string cmdStr, Action<string> callback) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(string.Empty);
            return;
        }

        ExecuteScalarTask<T> task = new ExecuteScalarTask<T>(dbConnection, cmdStr, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void Update<T>(T obj, Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        UpdateTask<T> task = new UpdateTask<T>(dbConnection, obj, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void UpdateAll<T>(List<T> obj, Action<int> callback = null) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(-1);
            return;
        }

        List<T> clone = new List<T>();
        clone.AddRange(obj);

        UpdateAllTask<T> task = new UpdateAllTask<T>(dbConnection, clone, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    /// <summary>
    /// 尝试从与指定类型关联的表中使用给定的主键检索对象。使用此方法要求给定类型具有指定的PrimaryKey(使用PrimaryKeyAttribute)。
    /// </summary>
    /// <param name="primaryKey">Primary key.</param>
    /// <param name="callback">The object with the given primary key or null if the object is not found.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public void Find<T>(object primaryKey, Action<bool> callback) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(false);
            return;
        }

        FindTask<T> task = new FindTask<T>(dbConnection, primaryKey, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    /// <summary>
    /// 尝试从与指定类型关联的表中使用给定的主键检索对象。使用此方法要求给定类型具有指定的PrimaryKey(使用PrimaryKeyAttribute)
    /// </summary>
    /// <param name="primaryKey">Primary key.</param>
    /// <param name="callback">The object with the given primary key. Throws a not found exception if the object is not found.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public void Get<T>(object primaryKey, Action<T> callback) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(default);
            return;
        }

        GetTask<T> task = new GetTask<T>(dbConnection, primaryKey, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public T GetSync<T>(object primaryKey) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            return default;
        }
        lock (DatabaseManager.LockObj)
        {
            T result;
            try
            {
                result = dbConnection.Get<T>(primaryKey);
            } catch (Exception e)
            {
                result = default;
            }
            return  result;
        }
    }

    public void Count<T>(Expression<Func<T, bool>> predExpr, Action<int> callback)
        where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(0);
            return;
        }

        CountTask<T> task = new CountTask<T>(dbConnection, predExpr, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    /// <summary>
    /// Returns a queryable interface to the table represented by the given type.
    /// </summary>
    /// <param name="predExpr">Pred expr.</param>
    /// <param name="callback">A queryable object that is able to translate Where, OrderBy, and Take queries into native SQL.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public void Query<T>(Expression<Func<T, bool>> predExpr, Action<IEnumerator<T>> callback) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(null);
            return;
        }

        QueryTask<T> task = new QueryTask<T>(dbConnection, predExpr, callback);

        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void Query<T>(Expression<Func<T, bool>> predExpr, Expression<Func<T, object>> orderExpr,bool asc,int limit, Action<IEnumerator<T>> callback) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(null);
            return;
        }

        QueryTask<T> task = new QueryTask<T>(dbConnection, predExpr, callback);
        if (orderExpr != null) task.OrderBy(orderExpr, asc);
        task.SetLimit(limit);
        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }

    public void QueryFields<T>(string queryStr, Action<List<T>> callback) where T : IDatabaseStruct, new()
    {
        if (!isInited)
        {
            callback?.Invoke(null);
            return;
        }

        QueryFieldsTask<T> task = new QueryFieldsTask<T>(dbConnection, queryStr, callback);
        thread.AddTask(task);
        taskQueue.Enqueue(task);
    }
}

#region Thread Tasks
abstract class DatabaseActionTask : ThreadTask
{
    protected SQLiteConnection dbConnection;

    public bool Processed { get; private set; }

    protected internal DatabaseActionTask(SQLiteConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public override void Process()
    {
        Processed = true;
    }

    protected internal abstract void CallBack();
}

class CreateTableTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;

    public CreateTableTask(SQLiteConnection dbConnection, Action<int> callback = null) : base(dbConnection)
    {
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.CreateTable<T>();
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class DropTableTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;

    public DropTableTask(SQLiteConnection dbConnection, Action<int> callback = null) : base(dbConnection)
    {
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.DropTable<T>();
            base.Process();
        }
 
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class InsertTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    T obj;
    string extra;

    public InsertTask(SQLiteConnection dbConnection, T obj, string extra = "", Action<int> callback = null) : base(dbConnection)
    {
        this.obj = obj;
        this.extra = extra;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            if (string.IsNullOrEmpty(dbConnection.Find<T>(obj.Key)?.Key))
                result = dbConnection.Insert(obj, extra);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class InsertOrReplaceTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    T obj;

    public InsertOrReplaceTask(SQLiteConnection dbConnection, T obj, Action<int> callback = null) : base(dbConnection)
    {
        this.obj = obj;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.InsertOrReplace(obj);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class InsertAllTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    List<T> obj;
    string extra;

    public InsertAllTask(SQLiteConnection dbConnection, List<T> obj, string extra = "", Action<int> callback = null) : base(dbConnection)
    {
        this.obj = obj;
        this.extra = extra;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.InsertAll(obj, extra);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class InsertOrReplaceAllTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    List<T> obj;

    public InsertOrReplaceAllTask(SQLiteConnection dbConnection, List<T> obj, Action<int> callback = null) : base(dbConnection)
    {
        this.obj = obj;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.InsertAll(obj, "OR REPLACE");
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class DeleteTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
 {
     int result;
     Action<int> callback;
     object primaryKey;
 
     public DeleteTask(SQLiteConnection dbConnection, object primaryKey, Action<int> callback = null) : base(dbConnection)
     {
         this.primaryKey = primaryKey;
         this.callback = callback;
     }
 
     public override void Process()
     {
         lock (DatabaseManager.LockObj)
         {
             result = dbConnection.Delete<T>(primaryKey);
             base.Process();
         }
     }
 
     protected internal override void CallBack()
     {
         callback?.Invoke(result);
     }
 }


class DeleteAllTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
 
    public DeleteAllTask(SQLiteConnection dbConnection, Action<int> callback = null) : base(dbConnection)
    {
        this.callback = callback;
    }
 
    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.DeleteAll<T>();
            base.Process();
        }
    }
 
    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class ExecuteTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    string cmdStr;

    public ExecuteTask(SQLiteConnection dbConnection, string cmdStr, Action<int> callback = null) : base(dbConnection)
    {
        this.cmdStr = cmdStr;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.Execute(cmdStr, null);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class ExecuteScalarTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    string result;
    Action<string> callback;
    string cmdStr;

    public ExecuteScalarTask(SQLiteConnection dbConnection, string cmdStr, Action<string> callback = null) : base(dbConnection)
    {
        this.cmdStr = cmdStr;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.ExecuteScalar<string>(cmdStr, null);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class UpdateTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    T obj;

    public UpdateTask(SQLiteConnection dbConnection, T obj, Action<int> callback = null) : base(dbConnection)
    {
        this.obj = obj;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.Update(obj);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class UpdateAllTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    int result;
    Action<int> callback;
    List<T> obj;

    public UpdateAllTask(SQLiteConnection dbConnection, List<T> obj, Action<int> callback = null) : base(dbConnection)
    {
        this.obj = obj;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.UpdateAll(obj);
            base.Process(); 
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class FindTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    bool result;
    Action<bool> callback;
    object primaryKey;

    public FindTask(SQLiteConnection dbConnection, object primaryKey, Action<bool> callback = null) : base(dbConnection)
    {
        this.primaryKey = primaryKey;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = !string.IsNullOrEmpty(dbConnection.Find<T>(primaryKey)?.Key);
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class GetTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    T result;
    Action<T> callback;
    object primaryKey;

    public GetTask(SQLiteConnection dbConnection, object primaryKey, Action<T> callback = null) : base(dbConnection)
    {
        this.primaryKey = primaryKey;
        this.callback = callback;
    }

    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            try
            {
                result = dbConnection.Get<T>(primaryKey);
            } catch (Exception e)
            {
                result = default;
                //Log.Error(e.Message);
            }
            base.Process();
        }
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}

class CountTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    private Action<int> callback;
    private Expression<Func<T, bool>> predExpr;
    private int result;
    public CountTask(SQLiteConnection dbConnection,Expression<Func<T, bool>> predExpr,Action<int> callback = null) : base(dbConnection)
    {
        this.callback = callback;
        this.predExpr = predExpr;
    }
    
    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            var tableQuery = dbConnection.Table<T>().Where(predExpr);
          
            result = tableQuery.Count();
            base.Process();
        }
        
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}


class QueryTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    IEnumerator<T> result;
    Action<IEnumerator<T>> callback;

    Expression<Func<T, bool>> predExpr;
    Expression<Func<T, object>> orderExpr;
    int limit;
    bool asc;

    public QueryTask(SQLiteConnection dbConnection, Expression<Func<T, bool>> predExpr, Action<IEnumerator<T>> callback = null) : base(dbConnection)
    {
        this.predExpr = predExpr;
        this.callback = callback;
        this.limit = 0;
        this.asc = true; //默认升序
    }

    public void SetLimit(int limit)
    {
        this.limit = limit;
    }

    public void OrderBy(Expression<Func<T,object>> orderExpr,bool asc)
    {
        this.orderExpr = orderExpr;
        this.asc = asc;
    }
    
    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            var tableQuery = dbConnection.Table<T>().Where(predExpr);
            if(this.orderExpr != null)
            {
                if(this.asc)
                {
                    tableQuery = tableQuery.OrderBy<object>(this.orderExpr);
                }
                else
                {
                    tableQuery = tableQuery.OrderByDescending<object>(this.orderExpr);
                }

            }
            if (this.limit > 0)
            {
                tableQuery = tableQuery.Take(this.limit);
            }

            result = tableQuery.GetEnumerator();
            base.Process();
        }
        
    }

    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}


class QueryFieldsTask<T> : DatabaseActionTask where T : IDatabaseStruct, new()
{
    List<T> result;
    Action<List<T>> callback;

    private string queryStr;
    
    public QueryFieldsTask(SQLiteConnection dbConnection, string queryStr, Action<List<T>> callback = null) : base(dbConnection)
    {
        this.queryStr = queryStr;
        this.callback = callback;
    }
    
    public override void Process()
    {
        lock (DatabaseManager.LockObj)
        {
            result = dbConnection.Query<T>(queryStr);
            base.Process();
        }
    }
    protected internal override void CallBack()
    {
        callback?.Invoke(result);
    }
}
#endregion

#region Database Structs
public interface IDatabaseStruct
{
    string Key { get; set; }
}
[TableAttribute("chat")]
[LuaCallCSharp()]
public class ChatDataStruct : IDatabaseStruct
{
    [PrimaryKey]
    public string Key { get; set; }
    public string RoomId { get; set; }
    public string Group { get; set; }
    public int SeqId { get; set; }
    public string SenderUid { get; set; }
    public int CreateTime { get; set; }
    public long SendLocalTime { get; set; }
    public int Post { get; set; }
    public string Msg { get; set; }
    public string TranslateMsg { get; set; }
    public string OriginalLang { get; set; }
    public bool IsTranslating { get; set; }
    public int SendState { get; set; }
    public string AttachmentId { get; set; }
    public string Media { get; set; }
    public string MsgMask { get; set; }
    public string JsonStr { get; set; }
    public int    WorldId { get; set; }
}

[TableAttribute("chatUserInfo")]
[LuaCallCSharp]
public class ChatUserInfoDataStruct : IDatabaseStruct
{
    [PrimaryKey]
    public string Key { get; set; }
    public string Uid { get; set; }
    public string UserName { get; set; }
    public string CareerId { get; set; }
    public string AllianceId { get; set; }
    public string AllianceSimpleName { get; set; }
    public int    ServerId { get; set; }
    public int    CrossFightSrcServerId { get; set; }
    public string HeadPic { get; set; }
    public int    HeadPicVer { get; set; }
    public int    GmFlag { get; set; }
    public int    VipLevel { get; set; }
    public int    SvipLevel { get; set; }
    public int    Vipframe { get; set; }
    public long   VipEndTime { get; set; }
    public long   LastUpdateTime { get; set; }
    public int    MonthCard { get; set; }
    public long   ChatBantime { get; set; }
    public int    ChatSkinId { get; set; }
    public string ChatFrameId { get; set; }
    public string JsonStr { get; set; }
    public int    OpenVipState { get; set; }
    public int    BubbleId { get; set; }

}

[TableAttribute("mail")]
[LuaCallCSharp]
public class MailDataStruct : IDatabaseStruct
{
    [PrimaryKey]
    public string Key { get; set; }
    public long Uuid { get; set; }
    [Indexed]
    public int Tab { get; set; }
    public byte[] PbByteStream { get; set; }
    public int    Status { get; set; }
    public long   CreateTime { get; set; }
    public long   ExpireTime { get; set; }
}

[TableAttribute("mailCollect")]
[LuaCallCSharp]
public class MailCollectDataStruct : IDatabaseStruct
{
    [PrimaryKey]
    public string Key { get; set; }
    public string ChannelId { get; set; }
    public string FromUser { get; set; }
    public string FromName { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Contents { get; set; }
    public string RewardId { get; set; }
    public int    Status { get; set; }
    public int    MailType { get; set; }
    public int    RewardStatus { get; set; }
    public long   RewardInvalidTime { get; set; }
    public int    SaveFlag { get; set; }
    public long   CreateTime { get; set; }
    public string Language { get; set; }
    public string Params { get; set; }
    public string jumpLink { get; set; }
    
    public int    IsCollect { get; set; }
    public long   CollectTime { get; set; }
    public int    WorldId { get; set; }

    public bool IsUnRead()
    {
        return false;
    }

    //是否过期
    public bool isExpired()
    {
        return false;
    }

    //是否有奖励为领取
    public bool IsHasRewardUnReceived()
    {
       return false;
    }
}

[TableAttribute("customData")]
public class CustomDataStruct : IDatabaseStruct
{
    [PrimaryKey]
    public string Key { get; set; }
    public string Content { get; set; }
}
#endregion