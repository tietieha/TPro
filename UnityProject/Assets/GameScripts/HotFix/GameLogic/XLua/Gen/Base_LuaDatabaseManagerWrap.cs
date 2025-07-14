#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLuaBase.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class BaseLuaDatabaseManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Base.LuaDatabaseManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 39, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteAllFile", _m_DeleteAllFile_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitDataBase", _m_InitDataBase_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Release", _m_Release_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryByTime", _m_QueryByTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryBySeqId", _m_QueryBySeqId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Get", _m_Get_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryChatByServerTime", _m_QueryChatByServerTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetByTime", _m_GetByTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetByRoomId", _m_GetByRoomId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBySeqId", _m_GetBySeqId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetChatUserInfoAsync", _m_GetChatUserInfoAsync_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetChatUserInfoSync", _m_GetChatUserInfoSync_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Count", _m_Count_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Delete", _m_Delete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteWithWhere", _m_DeleteWithWhere_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InsertOrReplace", _m_InsertOrReplace_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InsertOrReplaceAllTask", _m_InsertOrReplaceAllTask_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Insert", _m_Insert_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Update", _m_Update_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteMail", _m_DeleteMail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteCollectMail", _m_DeleteCollectMail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Execute4Mail", _m_Execute4Mail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ExecuteScalar4Mail", _m_ExecuteScalar4Mail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAllTask", _m_UpdateAllTask_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMail", _m_GetMail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryMailCollect", _m_QueryMailCollect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryMail", _m_QueryMail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryMailFields", _m_QueryMailFields_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QueryMailUnReadCount", _m_QueryMailUnReadCount_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NewCustomData", _m_NewCustomData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InsertOrReplaceCustomData", _m_InsertOrReplaceCustomData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetCustomData", _m_GetCustomData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateCustomData", _m_UpdateCustomData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteCustomData", _m_DeleteCustomData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DropCustomData", _m_DropCustomData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteAllMailData", _m_DeleteAllMailData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteAllMailCollectData", _m_DeleteAllMailCollectData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteAllChatData", _m_DeleteAllChatData_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "PlayerUid", _g_get_PlayerUid);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "PlayerUid", _s_set_PlayerUid);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Base.LuaDatabaseManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteAllFile_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Base.LuaDatabaseManager.DeleteAllFile(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitDataBase_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _playerUid = LuaAPI.lua_tostring(L, 1);
                    
                    Base.LuaDatabaseManager.InitDataBase( _playerUid );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Base.LuaDatabaseManager.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryByTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    long _sendTimeFrom = LuaAPI.lua_toint64(L, 1);
                    long _sendTimeTo = LuaAPI.lua_toint64(L, 2);
                    string _roomId = LuaAPI.lua_tostring(L, 3);
                    System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>>>(L, 4);
                    
                    Base.LuaDatabaseManager.QueryByTime( _sendTimeFrom, _sendTimeTo, _roomId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryBySeqId_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _seqIdFrom = LuaAPI.xlua_tointeger(L, 1);
                    int _seqIdTo = LuaAPI.xlua_tointeger(L, 2);
                    string _roomId = LuaAPI.lua_tostring(L, 3);
                    System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>>>(L, 4);
                    
                    Base.LuaDatabaseManager.QueryBySeqId( _seqIdFrom, _seqIdTo, _roomId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<ChatDataStruct> _callback = translator.GetDelegate<System.Action<ChatDataStruct>>(L, 2);
                    
                    Base.LuaDatabaseManager.Get( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryChatByServerTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _roomId = LuaAPI.lua_tostring(L, 1);
                    long _serverTime = LuaAPI.lua_toint64(L, 2);
                    int _limit = LuaAPI.xlua_tointeger(L, 3);
                    System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>>>(L, 4);
                    
                    Base.LuaDatabaseManager.QueryChatByServerTime( _roomId, _serverTime, _limit, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetByTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    long _sendTime = LuaAPI.lua_toint64(L, 1);
                    string _roomId = LuaAPI.lua_tostring(L, 2);
                    System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>>>(L, 3);
                    
                    Base.LuaDatabaseManager.GetByTime( _sendTime, _roomId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetByRoomId_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _roomId = LuaAPI.lua_tostring(L, 1);
                    System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>>>(L, 2);
                    
                    Base.LuaDatabaseManager.GetByRoomId( _roomId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBySeqId_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _seqId = LuaAPI.xlua_tointeger(L, 1);
                    string _roomId = LuaAPI.lua_tostring(L, 2);
                    System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<ChatDataStruct>>>(L, 3);
                    
                    Base.LuaDatabaseManager.GetBySeqId( _seqId, _roomId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChatUserInfoAsync_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<ChatUserInfoDataStruct> _callback = translator.GetDelegate<System.Action<ChatUserInfoDataStruct>>(L, 2);
                    
                    Base.LuaDatabaseManager.GetChatUserInfoAsync( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChatUserInfoSync_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Base.LuaDatabaseManager.GetChatUserInfoSync( _primaryKey );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Count_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _fromSeqId = LuaAPI.xlua_tointeger(L, 1);
                    int _toSeqId = LuaAPI.xlua_tointeger(L, 2);
                    string _roomId = LuaAPI.lua_tostring(L, 3);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 4);
                    
                    Base.LuaDatabaseManager.Count( _fromSeqId, _toSeqId, _roomId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Delete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.Delete( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteWithWhere_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _cmdStr = LuaAPI.lua_tostring(L, 1);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.DeleteWithWhere( _cmdStr, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertOrReplace_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _dataStruct = translator.GetObject(L, 1, typeof(object));
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.InsertOrReplace( _dataStruct, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertOrReplaceAllTask_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _dataStructList = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.InsertOrReplaceAllTask( _dataStructList, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Insert_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<object>(L, 1)&& translator.Assignable<System.Action<int>>(L, 2)) 
                {
                    object _dataStruct = translator.GetObject(L, 1, typeof(object));
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.Insert( _dataStruct, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object _dataStruct = translator.GetObject(L, 1, typeof(object));
                    
                    Base.LuaDatabaseManager.Insert( _dataStruct );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.Insert!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<ChatDataStruct>(L, 1)) 
                {
                    ChatDataStruct _chatDataStruct = (ChatDataStruct)translator.GetObject(L, 1, typeof(ChatDataStruct));
                    
                    Base.LuaDatabaseManager.Update( _chatDataStruct );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& translator.Assignable<MailDataStruct>(L, 1)) 
                {
                    MailDataStruct _dataStruct = (MailDataStruct)translator.GetObject(L, 1, typeof(MailDataStruct));
                    
                    Base.LuaDatabaseManager.Update( _dataStruct );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.Update!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteMail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.DeleteMail( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteCollectMail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.DeleteCollectMail( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Execute4Mail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _cmdStr = LuaAPI.lua_tostring(L, 1);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.Execute4Mail( _cmdStr, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExecuteScalar4Mail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _cmdStr = LuaAPI.lua_tostring(L, 1);
                    System.Action<string> _callback = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    Base.LuaDatabaseManager.ExecuteScalar4Mail( _cmdStr, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAllTask_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.List<MailDataStruct> _mailDataStructs = (System.Collections.Generic.List<MailDataStruct>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<MailDataStruct>));
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.UpdateAllTask( _mailDataStructs, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<MailDataStruct> _callback = translator.GetDelegate<System.Action<MailDataStruct>>(L, 2);
                    
                    Base.LuaDatabaseManager.GetMail( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryMailCollect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    long _collectTime = LuaAPI.lua_toint64(L, 1);
                    int _limit = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<System.Collections.Generic.IEnumerator<MailCollectDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<MailCollectDataStruct>>>(L, 3);
                    
                    Base.LuaDatabaseManager.QueryMailCollect( _collectTime, _limit, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryMail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _tab = LuaAPI.xlua_tointeger(L, 1);
                    long _uuid = LuaAPI.lua_toint64(L, 2);
                    int _limit = LuaAPI.xlua_tointeger(L, 3);
                    System.Action<System.Collections.Generic.IEnumerator<MailDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.IEnumerator<MailDataStruct>>>(L, 4);
                    
                    Base.LuaDatabaseManager.QueryMail( _tab, _uuid, _limit, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryMailFields_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _queryStr = LuaAPI.lua_tostring(L, 1);
                    System.Action<System.Collections.Generic.List<MailDataStruct>> _callback = translator.GetDelegate<System.Action<System.Collections.Generic.List<MailDataStruct>>>(L, 2);
                    
                    Base.LuaDatabaseManager.QueryMailFields( _queryStr, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryMailUnReadCount_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _tab = LuaAPI.xlua_tointeger(L, 1);
                    long _nowTime = LuaAPI.lua_toint64(L, 2);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 3);
                    
                    Base.LuaDatabaseManager.QueryMailUnReadCount( _tab, _nowTime, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NewCustomData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Base.LuaDatabaseManager.NewCustomData( _primaryKey );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertOrReplaceCustomData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<object>(L, 1)&& translator.Assignable<System.Action<int>>(L, 2)) 
                {
                    object _dataStruct = translator.GetObject(L, 1, typeof(object));
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.InsertOrReplaceCustomData( _dataStruct, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object _dataStruct = translator.GetObject(L, 1, typeof(object));
                    
                    Base.LuaDatabaseManager.InsertOrReplaceCustomData( _dataStruct );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.InsertOrReplaceCustomData!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCustomData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<CustomDataStruct> _callback = translator.GetDelegate<System.Action<CustomDataStruct>>(L, 2);
                    
                    Base.LuaDatabaseManager.GetCustomData( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateCustomData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    CustomDataStruct _customDataStruct = (CustomDataStruct)translator.GetObject(L, 1, typeof(CustomDataStruct));
                    
                    Base.LuaDatabaseManager.UpdateCustomData( _customDataStruct );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteCustomData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _primaryKey = LuaAPI.lua_tostring(L, 1);
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    
                    Base.LuaDatabaseManager.DeleteCustomData( _primaryKey, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DropCustomData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Action<int>>(L, 1)) 
                {
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 1);
                    
                    Base.LuaDatabaseManager.DropCustomData( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    Base.LuaDatabaseManager.DropCustomData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.DropCustomData!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteAllMailData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Action<int>>(L, 1)) 
                {
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 1);
                    
                    Base.LuaDatabaseManager.DeleteAllMailData( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    Base.LuaDatabaseManager.DeleteAllMailData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.DeleteAllMailData!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteAllMailCollectData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Action<int>>(L, 1)) 
                {
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 1);
                    
                    Base.LuaDatabaseManager.DeleteAllMailCollectData( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    Base.LuaDatabaseManager.DeleteAllMailCollectData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.DeleteAllMailCollectData!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteAllChatData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Action<int>>(L, 1)) 
                {
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 1);
                    
                    Base.LuaDatabaseManager.DeleteAllChatData( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    Base.LuaDatabaseManager.DeleteAllChatData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Base.LuaDatabaseManager.DeleteAllChatData!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerUid(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Base.LuaDatabaseManager.PlayerUid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PlayerUid(RealStatePtr L)
        {
		    try {
                
			    Base.LuaDatabaseManager.PlayerUid = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
