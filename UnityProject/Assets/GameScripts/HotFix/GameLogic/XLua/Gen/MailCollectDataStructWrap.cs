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
    public class MailCollectDataStructWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MailCollectDataStruct);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 20, 20);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsUnRead", _m_IsUnRead);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "isExpired", _m_isExpired);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsHasRewardUnReceived", _m_IsHasRewardUnReceived);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Key", _g_get_Key);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChannelId", _g_get_ChannelId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FromUser", _g_get_FromUser);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FromName", _g_get_FromName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Title", _g_get_Title);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SubTitle", _g_get_SubTitle);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Contents", _g_get_Contents);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RewardId", _g_get_RewardId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Status", _g_get_Status);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MailType", _g_get_MailType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RewardStatus", _g_get_RewardStatus);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RewardInvalidTime", _g_get_RewardInvalidTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SaveFlag", _g_get_SaveFlag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CreateTime", _g_get_CreateTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Language", _g_get_Language);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Params", _g_get_Params);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "jumpLink", _g_get_jumpLink);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsCollect", _g_get_IsCollect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CollectTime", _g_get_CollectTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldId", _g_get_WorldId);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Key", _s_set_Key);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ChannelId", _s_set_ChannelId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FromUser", _s_set_FromUser);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FromName", _s_set_FromName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Title", _s_set_Title);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SubTitle", _s_set_SubTitle);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Contents", _s_set_Contents);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RewardId", _s_set_RewardId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Status", _s_set_Status);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MailType", _s_set_MailType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RewardStatus", _s_set_RewardStatus);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RewardInvalidTime", _s_set_RewardInvalidTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SaveFlag", _s_set_SaveFlag);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CreateTime", _s_set_CreateTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Language", _s_set_Language);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Params", _s_set_Params);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "jumpLink", _s_set_jumpLink);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsCollect", _s_set_IsCollect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CollectTime", _s_set_CollectTime);
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
					
					var gen_ret = new MailCollectDataStruct();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MailCollectDataStruct constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUnRead(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsUnRead(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isExpired(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.isExpired(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsHasRewardUnReceived(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsHasRewardUnReceived(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Key(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Key);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChannelId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ChannelId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FromUser(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.FromUser);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FromName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.FromName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Title(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Title);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SubTitle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.SubTitle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Contents(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Contents);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RewardId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.RewardId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Status(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Status);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MailType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MailType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RewardStatus(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RewardStatus);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RewardInvalidTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.RewardInvalidTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SaveFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SaveFlag);
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
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.CreateTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Language(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Language);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Params(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Params);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_jumpLink(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.jumpLink);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsCollect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.IsCollect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CollectTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.CollectTime);
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
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
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
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Key = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ChannelId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ChannelId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FromUser(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FromUser = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FromName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FromName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Title(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Title = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SubTitle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SubTitle = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Contents(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Contents = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RewardId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RewardId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Status(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Status = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MailType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MailType = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RewardStatus(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RewardStatus = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RewardInvalidTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RewardInvalidTime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SaveFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SaveFlag = LuaAPI.xlua_tointeger(L, 2);
            
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
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CreateTime = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Language(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Language = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Params(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Params = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_jumpLink(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.jumpLink = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsCollect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsCollect = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CollectTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CollectTime = LuaAPI.lua_toint64(L, 2);
            
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
			
                MailCollectDataStruct gen_to_be_invoked = (MailCollectDataStruct)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WorldId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
