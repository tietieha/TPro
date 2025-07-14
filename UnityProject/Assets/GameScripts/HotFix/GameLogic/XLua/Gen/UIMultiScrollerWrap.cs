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
    public class UIMultiScrollerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UIMultiScroller);
			Utils.BeginObjectRegister(type, L, translator, 0, 25, 30, 28);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetChildOffset", _m_SetChildOffset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetScroller", _m_ResetScroller);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToIndex", _m_MoveToIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToLine", _m_MoveToLine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToTop", _m_MoveToTop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToBottom", _m_MoveToBottom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceUpdateItems", _m_ForceUpdateItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateItem", _m_UpdateItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateAllItem", _m_UpdateAllItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemList", _m_GetItemList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosition_Normal", _m_GetPosition_Normal);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosition_Custom", _m_GetPosition_Custom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosition", _m_GetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemByIndex", _m_GetItemByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddItem", _m_AddItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckedItem", _m_CheckedItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NotifyRefreshCheckState", _m_NotifyRefreshCheckState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnValueChange", _m_OnValueChange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsItemVisible100P", _m_IsItemVisible100P);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DelItem", _m_DelItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetItems", _m_ResetItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBeginDrag", _m_OnBeginDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDrag", _m_OnEndDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterDragEvent", _m_RegisterDragEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnRegisterDragEvent", _m_UnRegisterDragEvent);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Count", _g_get_Count);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DataCount", _g_get_DataCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectIndex", _g_get_SelectIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_movement", _g_get__movement);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maxPerLine", _g_get_maxPerLine);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "spacingX", _g_get_spacingX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "spacingY", _g_get_spacingY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "intervalFrame", _g_get_intervalFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isParallelCreate", _g_get_isParallelCreate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useTween", _g_get_useTween);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "tweenDuringTime", _g_get_tweenDuringTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "itemGapTime", _g_get_itemGapTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "easeType", _g_get_easeType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "startValue", _g_get_startValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "endValue", _g_get_endValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "function", _g_get_function);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "tweenChildName", _g_get_tweenChildName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnFuncCreateOver", _g_get_OnFuncCreateOver);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellWidth", _g_get_cellWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellHeight", _g_get_cellHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useSecondSize", _g_get_useSecondSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "secondCellWidth", _g_get_secondCellWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "secondCellHeight", _g_get_secondCellHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "viewCount", _g_get_viewCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "itemPrefab", _g_get_itemPrefab);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_content", _g_get__content);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnItemCreate", _g_get_OnItemCreate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnCheckItemUseSecondSize", _g_get_OnCheckItemUseSecondSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnTouchBottom", _g_get_OnTouchBottom);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnDragMoveCell", _g_get_OnDragMoveCell);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "DataCount", _s_set_DataCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_movement", _s_set__movement);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maxPerLine", _s_set_maxPerLine);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "spacingX", _s_set_spacingX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "spacingY", _s_set_spacingY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "intervalFrame", _s_set_intervalFrame);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isParallelCreate", _s_set_isParallelCreate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useTween", _s_set_useTween);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "tweenDuringTime", _s_set_tweenDuringTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "itemGapTime", _s_set_itemGapTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "easeType", _s_set_easeType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "startValue", _s_set_startValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "endValue", _s_set_endValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "function", _s_set_function);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "tweenChildName", _s_set_tweenChildName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnFuncCreateOver", _s_set_OnFuncCreateOver);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellWidth", _s_set_cellWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellHeight", _s_set_cellHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useSecondSize", _s_set_useSecondSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "secondCellWidth", _s_set_secondCellWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "secondCellHeight", _s_set_secondCellHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "viewCount", _s_set_viewCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "itemPrefab", _s_set_itemPrefab);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_content", _s_set__content);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnItemCreate", _s_set_OnItemCreate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnCheckItemUseSecondSize", _s_set_OnCheckItemUseSecondSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnTouchBottom", _s_set_OnTouchBottom);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnDragMoveCell", _s_set_OnDragMoveCell);
            
			
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
					
					var gen_ret = new UIMultiScroller();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UIMultiScroller constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetChildOffset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<int> _childOffset = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
                    
                    gen_to_be_invoked.SetChildOffset( _childOffset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetScroller(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _moveToIndex = LuaAPI.xlua_tointeger(L, 2);
                    bool _check = LuaAPI.lua_toboolean(L, 3);
                    int _childIndex = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.ResetScroller( _moveToIndex, _check, _childIndex );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _moveToIndex = LuaAPI.xlua_tointeger(L, 2);
                    bool _check = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.ResetScroller( _moveToIndex, _check );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _moveToIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ResetScroller( _moveToIndex );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ResetScroller(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIMultiScroller.ResetScroller!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UIMultiScroller.MoveCompleteHandler>(L, 6)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    bool _check = LuaAPI.lua_toboolean(L, 3);
                    bool _smooth = LuaAPI.lua_toboolean(L, 4);
                    int _childIndex = LuaAPI.xlua_tointeger(L, 5);
                    UIMultiScroller.MoveCompleteHandler _moveComplete = translator.GetDelegate<UIMultiScroller.MoveCompleteHandler>(L, 6);
                    
                    gen_to_be_invoked.MoveToIndex( _index, _check, _smooth, _childIndex, _moveComplete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    bool _check = LuaAPI.lua_toboolean(L, 3);
                    bool _smooth = LuaAPI.lua_toboolean(L, 4);
                    int _childIndex = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.MoveToIndex( _index, _check, _smooth, _childIndex );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    bool _check = LuaAPI.lua_toboolean(L, 3);
                    bool _smooth = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.MoveToIndex( _index, _check, _smooth );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    bool _check = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.MoveToIndex( _index, _check );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.MoveToIndex( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIMultiScroller.MoveToIndex!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToLine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<UIMultiScroller.MoveCompleteHandler>(L, 4)) 
                {
                    int _lineIndex = LuaAPI.xlua_tointeger(L, 2);
                    bool _smooth = LuaAPI.lua_toboolean(L, 3);
                    UIMultiScroller.MoveCompleteHandler _moveComplete = translator.GetDelegate<UIMultiScroller.MoveCompleteHandler>(L, 4);
                    
                    gen_to_be_invoked.MoveToLine( _lineIndex, _smooth, _moveComplete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _lineIndex = LuaAPI.xlua_tointeger(L, 2);
                    bool _smooth = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.MoveToLine( _lineIndex, _smooth );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _lineIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.MoveToLine( _lineIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIMultiScroller.MoveToLine!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToTop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.MoveToTop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToBottom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.MoveToBottom(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceUpdateItems(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ForceUpdateItems(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.UpdateItem( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAllItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateAllItem(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetItemList(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosition_Normal(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _i = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPosition_Normal( _i );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosition_Custom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _i = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPosition_Custom( _i );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _i = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPosition( _i );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetItemByIndex( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AddItem( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckedItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.CheckedItem( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NotifyRefreshCheckState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UIMultiScrollIndex _item = (UIMultiScrollIndex)translator.GetObject(L, 2, typeof(UIMultiScrollIndex));
                    
                    gen_to_be_invoked.NotifyRefreshCheckState( _item );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnValueChange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _pos;translator.Get(L, 2, out _pos);
                    
                    gen_to_be_invoked.OnValueChange( _pos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsItemVisible100P(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.IsItemVisible100P( _index );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DelItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.DelItem( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetItems(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetItems(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBeginDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnBeginDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEndDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnEndDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterDragEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.UI.UIScrollRect.ScrollDragDelegate _dragStartEvent = translator.GetDelegate<UnityEngine.UI.UIScrollRect.ScrollDragDelegate>(L, 2);
                    UnityEngine.UI.UIScrollRect.ScrollDragDelegate _dragUpdateEvent = translator.GetDelegate<UnityEngine.UI.UIScrollRect.ScrollDragDelegate>(L, 3);
                    UnityEngine.UI.UIScrollRect.ScrollDragDelegate _dragStopEvent = translator.GetDelegate<UnityEngine.UI.UIScrollRect.ScrollDragDelegate>(L, 4);
                    
                    gen_to_be_invoked.RegisterDragEvent( _dragStartEvent, _dragUpdateEvent, _dragStopEvent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegisterDragEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.UI.UIScrollRect.ScrollDragDelegate _dragStartEvent = translator.GetDelegate<UnityEngine.UI.UIScrollRect.ScrollDragDelegate>(L, 2);
                    UnityEngine.UI.UIScrollRect.ScrollDragDelegate _dragUpdateEvent = translator.GetDelegate<UnityEngine.UI.UIScrollRect.ScrollDragDelegate>(L, 3);
                    UnityEngine.UI.UIScrollRect.ScrollDragDelegate _dragStopEvent = translator.GetDelegate<UnityEngine.UI.UIScrollRect.ScrollDragDelegate>(L, 4);
                    
                    gen_to_be_invoked.UnRegisterDragEvent( _dragStartEvent, _dragUpdateEvent, _dragStopEvent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Count(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Count);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DataCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SelectIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__movement(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._movement);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxPerLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.maxPerLine);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_spacingX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.spacingX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_spacingY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.spacingY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_intervalFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.intervalFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isParallelCreate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isParallelCreate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useTween(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useTween);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tweenDuringTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.tweenDuringTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_itemGapTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.itemGapTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_easeType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.easeType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.startValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_endValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.endValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_function(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.function);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tweenChildName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.tweenChildName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnFuncCreateOver(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnFuncCreateOver);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.cellWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.cellHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useSecondSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useSecondSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_secondCellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.secondCellWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_secondCellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.secondCellHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_viewCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.viewCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_itemPrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.itemPrefab);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._content);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnItemCreate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnItemCreate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnCheckItemUseSecondSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnCheckItemUseSecondSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnTouchBottom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnTouchBottom);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnDragMoveCell(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnDragMoveCell);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DataCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DataCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__movement(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                UIMultiScroller.Arrangement gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked._movement = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxPerLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maxPerLine = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_spacingX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.spacingX = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_spacingY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.spacingY = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_intervalFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.intervalFrame = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isParallelCreate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isParallelCreate = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useTween(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useTween = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tweenDuringTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.tweenDuringTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_itemGapTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.itemGapTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_easeType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                DG.Tweening.Ease gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.easeType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.startValue = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_endValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.endValue = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_function(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                TweenFunction gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.function = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tweenChildName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.tweenChildName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnFuncCreateOver(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnFuncCreateOver = translator.GetDelegate<UIMultiScroller.OnFuncCreateOverHandler>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cellWidth = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cellHeight = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useSecondSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useSecondSize = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_secondCellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.secondCellWidth = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_secondCellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.secondCellHeight = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_viewCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.viewCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_itemPrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.itemPrefab = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._content = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnItemCreate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnItemCreate = translator.GetDelegate<UIMultiScroller.OnItemCreateHandler>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnCheckItemUseSecondSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnCheckItemUseSecondSize = translator.GetDelegate<UIMultiScroller.OnCheckItemUseSecondSizeHandler>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnTouchBottom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnTouchBottom = translator.GetDelegate<UIMultiScroller.OnTouchBottomHandler>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnDragMoveCell(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnDragMoveCell = translator.GetDelegate<UIMultiScroller.OnDragMoveCellHandler>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
