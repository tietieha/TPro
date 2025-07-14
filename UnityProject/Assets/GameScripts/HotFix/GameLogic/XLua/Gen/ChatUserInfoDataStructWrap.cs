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
    public class ChatUserInfoDataStructWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ChatUserInfoDataStruct);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 23, 23);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Key", _g_get_Key);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Uid", _g_get_Uid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UserName", _g_get_UserName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CareerId", _g_get_CareerId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AllianceId", _g_get_AllianceId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AllianceSimpleName", _g_get_AllianceSimpleName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ServerId", _g_get_ServerId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CrossFightSrcServerId", _g_get_CrossFightSrcServerId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HeadPic", _g_get_HeadPic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HeadPicVer", _g_get_HeadPicVer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GmFlag", _g_get_GmFlag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VipLevel", _g_get_VipLevel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SvipLevel", _g_get_SvipLevel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Vipframe", _g_get_Vipframe);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VipEndTime", _g_get_VipEndTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LastUpdateTime", _g_get_LastUpdateTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MonthCard", _g_get_MonthCard);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChatBantime", _g_get_ChatBantime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChatSkinId", _g_get_ChatSkinId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChatFrameId", _g_get_ChatFrameId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "JsonStr", _g_get_JsonStr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OpenVipState", _g_get_OpenVipState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BubbleId", _g_get_BubbleId);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Key", _s_set_Key);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Uid", _s_set_Uid);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UserName", _s_set_UserName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CareerId", _s_set_CareerId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AllianceId", _s_set_AllianceId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AllianceSimpleName", _s_set_AllianceSimpleName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ServerId", _s_set_ServerId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CrossFightSrcServerId", _s_set_CrossFightSrcServerId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "HeadPic", _s_set_HeadPic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "HeadPicVer", _s_set_HeadPicVer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GmFlag", _s_set_GmFlag);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "VipLevel", _s_set_VipLevel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SvipLevel", _s_set_SvipLevel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Vipframe", _s_set_Vipframe);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "VipEndTime", _s_set_VipEndTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LastUpdateTime", _s_set_LastUpdateTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MonthCard", _s_set_MonthCard);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ChatBantime", _s_set_ChatBantime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ChatSkinId", _s_set_ChatSkinId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ChatFrameId", _s_set_ChatFrameId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "JsonStr", _s_set_JsonStr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OpenVipState", _s_set_OpenVipState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BubbleId", _s_set_BubbleId);
            
			
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
					
					var gen_ret = new ChatUserInfoDataStruct();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ChatUserInfoDataStruct constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Key(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Key);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Uid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Uid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UserName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UserName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CareerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.CareerId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AllianceId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AllianceId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AllianceSimpleName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AllianceSimpleName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ServerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ServerId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CrossFightSrcServerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CrossFightSrcServerId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HeadPic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.HeadPic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HeadPicVer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.HeadPicVer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GmFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GmFlag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VipLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.VipLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SvipLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SvipLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Vipframe(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Vipframe);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VipEndTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.VipEndTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LastUpdateTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.LastUpdateTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MonthCard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MonthCard);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChatBantime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.ChatBantime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChatSkinId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ChatSkinId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChatFrameId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ChatFrameId);
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
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.JsonStr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OpenVipState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.OpenVipState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BubbleId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BubbleId);
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
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Key = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Uid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Uid = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UserName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UserName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CareerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CareerId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AllianceId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AllianceId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AllianceSimpleName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AllianceSimpleName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ServerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ServerId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CrossFightSrcServerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CrossFightSrcServerId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HeadPic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HeadPic = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HeadPicVer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HeadPicVer = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GmFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GmFlag = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VipLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.VipLevel = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SvipLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SvipLevel = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Vipframe(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Vipframe = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VipEndTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.VipEndTime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LastUpdateTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LastUpdateTime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MonthCard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MonthCard = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChatBantime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ChatBantime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChatSkinId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ChatSkinId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChatFrameId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ChatFrameId = LuaAPI.lua_tostring(L, 2);
            
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
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.JsonStr = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OpenVipState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OpenVipState = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BubbleId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatUserInfoDataStruct gen_to_be_invoked = (ChatUserInfoDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BubbleId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
