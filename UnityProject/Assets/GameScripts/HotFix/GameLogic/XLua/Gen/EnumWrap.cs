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
    
    public class UnityEngineTextAnchorWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.TextAnchor), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.TextAnchor), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.TextAnchor), L, null, 10, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpperLeft", UnityEngine.TextAnchor.UpperLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpperCenter", UnityEngine.TextAnchor.UpperCenter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpperRight", UnityEngine.TextAnchor.UpperRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiddleLeft", UnityEngine.TextAnchor.MiddleLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiddleCenter", UnityEngine.TextAnchor.MiddleCenter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiddleRight", UnityEngine.TextAnchor.MiddleRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LowerLeft", UnityEngine.TextAnchor.LowerLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LowerCenter", UnityEngine.TextAnchor.LowerCenter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LowerRight", UnityEngine.TextAnchor.LowerRight);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.TextAnchor), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTextAnchor(L, (UnityEngine.TextAnchor)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "UpperLeft"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.UpperLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpperCenter"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.UpperCenter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpperRight"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.UpperRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MiddleLeft"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.MiddleLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MiddleCenter"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.MiddleCenter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MiddleRight"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.MiddleRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LowerLeft"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.LowerLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LowerCenter"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.LowerCenter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LowerRight"))
                {
                    translator.PushUnityEngineTextAnchor(L, UnityEngine.TextAnchor.LowerRight);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.TextAnchor!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.TextAnchor! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class EventIdWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(EventId), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(EventId), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(EventId), L, null, 18, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", EventId.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TestEvent", EventId.TestEvent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NETWORK_SOCKET_CONNECT_SUCCESS", EventId.NETWORK_SOCKET_CONNECT_SUCCESS);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NETWORK_SOCKET_CONNECT_FAILURE", EventId.NETWORK_SOCKET_CONNECT_FAILURE);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NETWORK_FORCE_HEART_BEAT", EventId.NETWORK_FORCE_HEART_BEAT);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RETURN_LOGIN", EventId.RETURN_LOGIN);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DEBUG_CONNECT_GAMESERVER", EventId.DEBUG_CONNECT_GAMESERVER);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LOCALIZATION_LOAD_SUCCESS", EventId.LOCALIZATION_LOAD_SUCCESS);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TZ_Camera_Moved", EventId.TZ_Camera_Moved);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TZ_Camera_Move_End", EventId.TZ_Camera_Move_End);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TZ_Camera_Change_View", EventId.TZ_Camera_Change_View);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TZ_Camera_Change_Refresh_Ship", EventId.TZ_Camera_Change_Refresh_Ship);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OPEN_UI_MAIN_VIEW", EventId.OPEN_UI_MAIN_VIEW);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GUIDE_CLOSE_GUIDE_MAIN_VIEW", EventId.GUIDE_CLOSE_GUIDE_MAIN_VIEW);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GUIDE_REMOVE_GUIDE_WEAK", EventId.GUIDE_REMOVE_GUIDE_WEAK);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GUIDE_OPEN_UIWIDGET_STARTGUIDE_CS", EventId.GUIDE_OPEN_UIWIDGET_STARTGUIDE_CS);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "THRONE_APPEAR_FINISH", EventId.THRONE_APPEAR_FINISH);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(EventId), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushEventId(L, (EventId)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushEventId(L, EventId.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TestEvent"))
                {
                    translator.PushEventId(L, EventId.TestEvent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NETWORK_SOCKET_CONNECT_SUCCESS"))
                {
                    translator.PushEventId(L, EventId.NETWORK_SOCKET_CONNECT_SUCCESS);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NETWORK_SOCKET_CONNECT_FAILURE"))
                {
                    translator.PushEventId(L, EventId.NETWORK_SOCKET_CONNECT_FAILURE);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NETWORK_FORCE_HEART_BEAT"))
                {
                    translator.PushEventId(L, EventId.NETWORK_FORCE_HEART_BEAT);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RETURN_LOGIN"))
                {
                    translator.PushEventId(L, EventId.RETURN_LOGIN);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DEBUG_CONNECT_GAMESERVER"))
                {
                    translator.PushEventId(L, EventId.DEBUG_CONNECT_GAMESERVER);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LOCALIZATION_LOAD_SUCCESS"))
                {
                    translator.PushEventId(L, EventId.LOCALIZATION_LOAD_SUCCESS);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TZ_Camera_Moved"))
                {
                    translator.PushEventId(L, EventId.TZ_Camera_Moved);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TZ_Camera_Move_End"))
                {
                    translator.PushEventId(L, EventId.TZ_Camera_Move_End);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TZ_Camera_Change_View"))
                {
                    translator.PushEventId(L, EventId.TZ_Camera_Change_View);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TZ_Camera_Change_Refresh_Ship"))
                {
                    translator.PushEventId(L, EventId.TZ_Camera_Change_Refresh_Ship);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OPEN_UI_MAIN_VIEW"))
                {
                    translator.PushEventId(L, EventId.OPEN_UI_MAIN_VIEW);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GUIDE_CLOSE_GUIDE_MAIN_VIEW"))
                {
                    translator.PushEventId(L, EventId.GUIDE_CLOSE_GUIDE_MAIN_VIEW);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GUIDE_REMOVE_GUIDE_WEAK"))
                {
                    translator.PushEventId(L, EventId.GUIDE_REMOVE_GUIDE_WEAK);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GUIDE_OPEN_UIWIDGET_STARTGUIDE_CS"))
                {
                    translator.PushEventId(L, EventId.GUIDE_OPEN_UIWIDGET_STARTGUIDE_CS);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "THRONE_APPEAR_FINISH"))
                {
                    translator.PushEventId(L, EventId.THRONE_APPEAR_FINISH);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for EventId!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for EventId! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class EmbattleCellColorTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(EmbattleCellColorType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(EmbattleCellColorType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(EmbattleCellColorType), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", EmbattleCellColorType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Blue", EmbattleCellColorType.Blue);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LightBlue", EmbattleCellColorType.LightBlue);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Pink", EmbattleCellColorType.Pink);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LightPink", EmbattleCellColorType.LightPink);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Transparent", EmbattleCellColorType.Transparent);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(EmbattleCellColorType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushEmbattleCellColorType(L, (EmbattleCellColorType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushEmbattleCellColorType(L, EmbattleCellColorType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Blue"))
                {
                    translator.PushEmbattleCellColorType(L, EmbattleCellColorType.Blue);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LightBlue"))
                {
                    translator.PushEmbattleCellColorType(L, EmbattleCellColorType.LightBlue);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Pink"))
                {
                    translator.PushEmbattleCellColorType(L, EmbattleCellColorType.Pink);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LightPink"))
                {
                    translator.PushEmbattleCellColorType(L, EmbattleCellColorType.LightPink);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Transparent"))
                {
                    translator.PushEmbattleCellColorType(L, EmbattleCellColorType.Transparent);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for EmbattleCellColorType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for EmbattleCellColorType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MPathFindingEUnitTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(M.PathFinding.EUnitType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(M.PathFinding.EUnitType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(M.PathFinding.EUnitType), L, null, 11, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Soldier_Left", M.PathFinding.EUnitType.Soldier_Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hero_Left", M.PathFinding.EUnitType.Hero_Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Virtual_Hero_Left", M.PathFinding.EUnitType.Virtual_Hero_Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Instrument_Hero_Left", M.PathFinding.EUnitType.Instrument_Hero_Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Soldier_Right", M.PathFinding.EUnitType.Soldier_Right);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hero_Right", M.PathFinding.EUnitType.Hero_Right);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Virtual_Hero_Right", M.PathFinding.EUnitType.Virtual_Hero_Right);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Instrument_Hero_Riht", M.PathFinding.EUnitType.Instrument_Hero_Riht);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Count", M.PathFinding.EUnitType.Count);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "All", M.PathFinding.EUnitType.All);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(M.PathFinding.EUnitType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMPathFindingEUnitType(L, (M.PathFinding.EUnitType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Soldier_Left"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Soldier_Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Hero_Left"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Hero_Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Virtual_Hero_Left"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Virtual_Hero_Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Instrument_Hero_Left"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Instrument_Hero_Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Soldier_Right"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Soldier_Right);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Hero_Right"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Hero_Right);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Virtual_Hero_Right"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Virtual_Hero_Right);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Instrument_Hero_Riht"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Instrument_Hero_Riht);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Count"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.Count);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "All"))
                {
                    translator.PushMPathFindingEUnitType(L, M.PathFinding.EUnitType.All);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for M.PathFinding.EUnitType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for M.PathFinding.EUnitType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MPathFindingEUnitStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(M.PathFinding.EUnitState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(M.PathFinding.EUnitState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(M.PathFinding.EUnitState), L, null, 22, 0, 0);

            Utils.RegisterEnumType(L, typeof(M.PathFinding.EUnitState));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(M.PathFinding.EUnitState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMPathFindingEUnitState(L, (M.PathFinding.EUnitState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(M.PathFinding.EUnitState), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(M.PathFinding.EUnitState) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for M.PathFinding.EUnitState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}