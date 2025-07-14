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
    public class ChatDataStructWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ChatDataStruct);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 18, 18);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Key", _g_get_Key);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RoomId", _g_get_RoomId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Group", _g_get_Group);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SeqId", _g_get_SeqId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SenderUid", _g_get_SenderUid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CreateTime", _g_get_CreateTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SendLocalTime", _g_get_SendLocalTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Post", _g_get_Post);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Msg", _g_get_Msg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TranslateMsg", _g_get_TranslateMsg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OriginalLang", _g_get_OriginalLang);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsTranslating", _g_get_IsTranslating);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SendState", _g_get_SendState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AttachmentId", _g_get_AttachmentId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Media", _g_get_Media);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MsgMask", _g_get_MsgMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "JsonStr", _g_get_JsonStr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldId", _g_get_WorldId);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Key", _s_set_Key);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RoomId", _s_set_RoomId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Group", _s_set_Group);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SeqId", _s_set_SeqId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SenderUid", _s_set_SenderUid);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CreateTime", _s_set_CreateTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SendLocalTime", _s_set_SendLocalTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Post", _s_set_Post);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Msg", _s_set_Msg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TranslateMsg", _s_set_TranslateMsg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OriginalLang", _s_set_OriginalLang);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsTranslating", _s_set_IsTranslating);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SendState", _s_set_SendState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AttachmentId", _s_set_AttachmentId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Media", _s_set_Media);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MsgMask", _s_set_MsgMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "JsonStr", _s_set_JsonStr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WorldId", _s_set_WorldId);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new ChatDataStruct();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ChatDataStruct constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Key(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Key);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RoomId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.RoomId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Group(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Group);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SeqId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SeqId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SenderUid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.SenderUid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CreateTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CreateTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SendLocalTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.SendLocalTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Post(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Post);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Msg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Msg);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TranslateMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.TranslateMsg);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OriginalLang(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.OriginalLang);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsTranslating(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsTranslating);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SendState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SendState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AttachmentId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AttachmentId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Media(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Media);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MsgMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.MsgMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_JsonStr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.JsonStr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.WorldId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Key(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Key = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RoomId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RoomId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Group(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Group = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SeqId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SeqId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SenderUid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SenderUid = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CreateTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CreateTime = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SendLocalTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SendLocalTime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Post(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Post = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Msg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Msg = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TranslateMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TranslateMsg = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OriginalLang(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OriginalLang = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsTranslating(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsTranslating = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SendState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SendState = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AttachmentId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AttachmentId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Media(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Media = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MsgMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MsgMask = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_JsonStr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.JsonStr = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WorldId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatDataStruct gen_to_be_invoked = (ChatDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WorldId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
