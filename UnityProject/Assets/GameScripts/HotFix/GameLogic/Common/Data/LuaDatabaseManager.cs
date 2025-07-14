using System;
using System.Collections.Generic;
using System.IO;
using TEngine;
using UnityEngine;
using XLua;

namespace Base
{
    [LuaCallCSharp]
    public class LuaDatabaseManager
    {
        public static string PlayerUid = string.Empty;
        public static void DeleteAllFile()
        {
            UnityEngine.Debug.Log("删除所有数据库文件");
            string dbPath = Path.Combine(Application.persistentDataPath, "DB");
            if (Directory.Exists(dbPath))
            {
                Directory.Delete(dbPath,true);
            }
        }
        
        public static void InitDataBase(string playerUid)
        {
            DatabaseManager.Instance.Release();
            if (string.IsNullOrEmpty(playerUid) == false)
            {
                string dbPath = string.Format("DB/{0}/gameData.db", playerUid);
                DatabaseManager.Instance.Initialize(dbPath, OnDatabaseInitialized);
            }
            else
            {
                Log.Error("初始化数据库时 玩家id不能为空 ~");
            }
        }
        
        /// <summary>
        /// 数据库初始化完成
        /// </summary>
        /// <param name="success">If set to <c>true</c> success.</param>
        private static void OnDatabaseInitialized(bool success)
        {
            if (success)
            {
                // 开始初始化一些表
                DatabaseManager.Instance.CreateTable<MailDataStruct>(result =>
                {
                    if (result == -1)
                    {
                        Log.Error("CreateTable (MailDataStruct) failed");
                    }
                });
                DatabaseManager.Instance.CreateTable<MailCollectDataStruct>(result =>
                {
                    if (result == -1)
                    {
                        Log.Error("CreateTable (MailCollectDataStruct) failed");
                    }
                });

                DatabaseManager.Instance.CreateTable<ChatUserInfoDataStruct>(result =>
                {
                    if (result == -1)
                    {
                        Log.Error("CreateTable (ChatUserInfoDataStruct) failed");
                    }
                });

                DatabaseManager.Instance.CreateTable<ChatDataStruct>(result =>
                {
                    if (result == -1)
                    {
                        Log.Error("CreateTable (ChatDataStruct) failed");
                    }
                });

                DatabaseManager.Instance.CreateTable<CustomDataStruct>(result =>
                {
                    if (result == -1)
                    {
                        Log.Error("CreateTable (CustomDataStruct) failed");
                    }
                });
            }

        }

        
        public static void Release()
        {
            DatabaseManager.Instance.Release();
        }

        
        public static void QueryByTime(long sendTimeFrom, long sendTimeTo, string roomId, Action<IEnumerator<ChatDataStruct>> callback)
        {

            DatabaseManager.Instance.Query<ChatDataStruct>(x => x.CreateTime >= sendTimeFrom && x.CreateTime < sendTimeTo && x.RoomId == roomId, callback);
        }
        
        public static void QueryBySeqId(int seqIdFrom, int seqIdTo, string roomId, Action<IEnumerator<ChatDataStruct>> callback)
        {

            DatabaseManager.Instance.Query<ChatDataStruct>(x => x.SeqId >= seqIdFrom && x.SeqId < seqIdTo && x.RoomId == roomId, callback);
        }
        
        public static void Get(string primaryKey, Action<ChatDataStruct> callback)
        {
            DatabaseManager.Instance.Get<ChatDataStruct>(primaryKey, callback);
        }
        
        public static void QueryChatByServerTime(string roomId,long serverTime,int limit, Action<IEnumerator<ChatDataStruct>> callback)
        {
            if (serverTime == 0)
            {
                DatabaseManager.Instance.Query<ChatDataStruct>(x => x.RoomId == roomId, (x => x.CreateTime), false, limit, callback);
            }
            else
            {
                DatabaseManager.Instance.Query<ChatDataStruct>(x => x.CreateTime < serverTime && x.RoomId == roomId, (x => x.CreateTime), false, limit, callback);
            }

            
        }
        
        public static void GetByTime(long sendTime, string roomId, Action<IEnumerator<ChatDataStruct>> callback)
        {

            DatabaseManager.Instance.Query<ChatDataStruct>(x => x.SendLocalTime == sendTime && x.RoomId == roomId, callback);
        }
        
        public static void GetByRoomId(string roomId, Action<IEnumerator<ChatDataStruct>> callback)
        {

            DatabaseManager.Instance.Query<ChatDataStruct>(x => x.RoomId == roomId, callback);
        }
        
        public static void GetBySeqId(int seqId, string roomId, Action<IEnumerator<ChatDataStruct>> callback)
        {

            DatabaseManager.Instance.Query<ChatDataStruct>(x => x.SeqId == seqId && x.RoomId == roomId, callback);
        }
        
        public static void GetChatUserInfoAsync(string primaryKey, Action<ChatUserInfoDataStruct> callback)
        {
            DatabaseManager.Instance.Get<ChatUserInfoDataStruct>(primaryKey, callback);
        }

        
        public static ChatUserInfoDataStruct GetChatUserInfoSync(string primaryKey)
        {
            return DatabaseManager.Instance.GetSync<ChatUserInfoDataStruct>(primaryKey);
        }

        public static void Count(int fromSeqId ,int toSeqId ,string roomId,Action<int> callback)
        {
            DatabaseManager.Instance.Count<ChatDataStruct>(x => x.SeqId >= fromSeqId && x.SeqId < toSeqId && x.RoomId == roomId, callback);
        }


        public static void Delete(string primaryKey, Action<int> callback)
        {
            DatabaseManager.Instance.Delete<ChatDataStruct>(primaryKey, callback);
        }

        
        public static void DeleteWithWhere(string cmdStr, Action<int> callback)
        {
            if (string.IsNullOrEmpty(cmdStr))
                return;
            DatabaseManager.Instance.Execute<ChatDataStruct>(cmdStr, callback);
        }

        
        public static void InsertOrReplace(object dataStruct, Action<int> callback)
        {
            if (dataStruct is ChatDataStruct)
            {
                DatabaseManager.Instance.InsertOrReplace<ChatDataStruct>((ChatDataStruct)dataStruct, callback);
            }
            else if (dataStruct is ChatUserInfoDataStruct)
            {
                DatabaseManager.Instance.InsertOrReplace<ChatUserInfoDataStruct>((ChatUserInfoDataStruct)dataStruct, callback);
            }
            else if (dataStruct is MailDataStruct)
            {
                DatabaseManager.Instance.InsertOrReplace<MailDataStruct>((MailDataStruct)dataStruct, callback);
            }
            else if (dataStruct is MailCollectDataStruct)
            {
                DatabaseManager.Instance.InsertOrReplace<MailCollectDataStruct>((MailCollectDataStruct)dataStruct, callback);
            }
        }


        
        public static void InsertOrReplaceAllTask(LuaTable dataStructList, Action<int> callback)
        {
            if (dataStructList.Length == 0)
                return;
            var dataStruct = dataStructList.Get<int, object>(1);
            if (dataStruct is ChatDataStruct)
            {
                var datas = new List<ChatDataStruct>();
                dataStructList.ForEach<int, ChatDataStruct>((key, value) =>
                 {
                     datas.Add(value);
                 });
                DatabaseManager.Instance.InsertOrReplaceAll<ChatDataStruct>(datas, callback);
            }
            else if (dataStruct is ChatUserInfoDataStruct)
            {
                var datas = new List<ChatUserInfoDataStruct>();
                dataStructList.ForEach<int, ChatUserInfoDataStruct>((key, value) =>
                 {
                     datas.Add(value);
                 });
                DatabaseManager.Instance.InsertOrReplaceAll<ChatUserInfoDataStruct>(datas, callback);
            }
            else if (dataStruct is MailDataStruct)
            {
                var datas = new List<MailDataStruct>();
                dataStructList.ForEach<int, MailDataStruct>((key, value) =>
                 {
                     datas.Add(value);
                 });
                DatabaseManager.Instance.InsertOrReplaceAll<MailDataStruct>(datas, callback);
            }
            else if (dataStruct is MailCollectDataStruct)
            {
                var datas = new List<MailCollectDataStruct>();
                dataStructList.ForEach<int, MailCollectDataStruct>((key, value) =>
                {
                    datas.Add(value);
                });
                DatabaseManager.Instance.InsertOrReplaceAll<MailCollectDataStruct>(datas, callback);
            }
        }

        
        public static void Insert(object dataStruct, Action<int> callback = null)
        {
            string extra = "";
            if (dataStruct is ChatDataStruct)
            {
                DatabaseManager.Instance.Insert<ChatDataStruct>((ChatDataStruct)dataStruct, extra, callback);
            }
            else if (dataStruct is ChatUserInfoDataStruct)
            {
                DatabaseManager.Instance.Insert<ChatUserInfoDataStruct>((ChatUserInfoDataStruct)dataStruct, extra, callback);
            }
            else if (dataStruct is MailDataStruct)
            {
                DatabaseManager.Instance.Insert<MailDataStruct>((MailDataStruct)dataStruct, extra, callback);
            }
            else if (dataStruct is MailCollectDataStruct)
            {
                DatabaseManager.Instance.Insert<MailCollectDataStruct>((MailCollectDataStruct)dataStruct, extra, callback);
            }
        }

        
        public static void Update(ChatDataStruct chatDataStruct)
        {
            DatabaseManager.Instance.Update(chatDataStruct);
        }

        
        public static void DeleteMail(string primaryKey, Action<int> callback)
        {
            DatabaseManager.Instance.Delete<MailDataStruct>(primaryKey, callback);
        }
        
        public static void DeleteCollectMail(string primaryKey, Action<int> callback)
        {
            DatabaseManager.Instance.Delete<MailCollectDataStruct>(primaryKey, callback);
        }

        public static void Execute4Mail(string cmdStr, Action<int> callback)
        {
            if (string.IsNullOrEmpty(cmdStr))
                return;
            DatabaseManager.Instance.Execute<MailDataStruct>(cmdStr, callback);
        }

        public static void ExecuteScalar4Mail(string cmdStr, Action<string> callback)
        {
            if (string.IsNullOrEmpty(cmdStr))
                return;
            DatabaseManager.Instance.ExecuteScalar<MailDataStruct>(cmdStr, callback);
        }
        
        public static void Update(MailDataStruct dataStruct)
        {
            DatabaseManager.Instance.Update(dataStruct);
        }
        
        public static void UpdateAllTask(List<MailDataStruct> mailDataStructs, Action<int> callback)
        {
            DatabaseManager.Instance.UpdateAll(mailDataStructs,callback);
        }
        
        public static void GetMail(string primaryKey, Action<MailDataStruct> callback)
        {
            DatabaseManager.Instance.Get<MailDataStruct>(primaryKey, callback);
        }
        
        public static void QueryMailCollect(long collectTime, int limit, Action<IEnumerator<MailCollectDataStruct>> callback)
        {
            if (collectTime == 0)
            {
                DatabaseManager.Instance.Query<MailCollectDataStruct>((x => x.CollectTime >= 0 ), (x => x.CollectTime), false, limit, callback);
            }
            else
            {
                DatabaseManager.Instance.Query<MailCollectDataStruct>((x => x.CollectTime < collectTime), (x => x.CollectTime), false, limit, callback);
            }
        }
        
        public static void QueryMail(int tab, long uuid, int limit, Action<IEnumerator<MailDataStruct>> callback)
        {
            DatabaseManager.Instance.Query<MailDataStruct>(x => x.Uuid < uuid && x.Tab == tab, (x => x.Uuid), false, limit, callback);
        }

        public static void QueryMailFields(string queryStr, Action<List<MailDataStruct>> callback)
        {
            DatabaseManager.Instance.QueryFields<MailDataStruct>(queryStr, callback);
        }
        
        public static void QueryMailUnReadCount(int tab, long nowTime, Action<int> callback)
        {
            DatabaseManager.Instance.Count<MailDataStruct>((x =>  x.Tab == tab && x.Status == 0 &&  x.ExpireTime > nowTime), callback);
        }
        
        public static CustomDataStruct NewCustomData(string primaryKey)
        {
            return new CustomDataStruct()
            {
                Key = primaryKey
            };
        }

        
        public static void InsertOrReplaceCustomData(object dataStruct, Action<int> callback = null)
        {
            if (dataStruct is CustomDataStruct)
            {
                DatabaseManager.Instance.InsertOrReplace<CustomDataStruct>((CustomDataStruct)dataStruct, callback);
            }
        }

        
        public static void GetCustomData(string primaryKey, Action<CustomDataStruct> callback)
        {
            DatabaseManager.Instance.Get<CustomDataStruct>(primaryKey, callback);
        }

        
        public static void UpdateCustomData(CustomDataStruct customDataStruct)
        {
            DatabaseManager.Instance.Update(customDataStruct);
        }

        
        public static void DeleteCustomData(string primaryKey, Action<int> callback)
        {
            DatabaseManager.Instance.Delete<CustomDataStruct>(primaryKey, callback);
        }

        
        public static void DropCustomData(Action<int>  callback = null)
        {
            DatabaseManager.Instance.DropTable<CustomDataStruct>(callback);
        }
        
        public static void DeleteAllMailData(Action<int> callback = null)
        {
            DatabaseManager.Instance.DeleteAll<MailDataStruct>(callback);
        }
        
        public static void DeleteAllMailCollectData(Action<int> callback = null)
        {
            DatabaseManager.Instance.DeleteAll<MailCollectDataStruct>(callback);
        }
        
        public static void DeleteAllChatData(Action<int> callback = null)
        {
            DatabaseManager.Instance.DeleteAll<ChatDataStruct>(callback);
        }

    }
}