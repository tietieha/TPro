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
    public class UnityEngineUIExtensionsHorizontalScrollSnapWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.UI.Extensions.HorizontalScrollSnap);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DistributePages", _m_DistributePages);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddChild", _m_AddChild);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveChild", _m_RemoveChild);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllChildren", _m_RemoveAllChildren);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateLayout", _m_UpdateLayout);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDrag", _m_OnEndDrag);
			
			
			
			
			
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
					
					var gen_ret = new UnityEngine.UI.Extensions.HorizontalScrollSnap();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.UI.Extensions.HorizontalScrollSnap constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DistributePages(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.HorizontalScrollSnap gen_to_be_invoked = (UnityEngine.UI.Extensions.HorizontalScrollSnap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DistributePages(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddChild(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.HorizontalScrollSnap gen_to_be_invoked = (UnityEngine.UI.Extensions.HorizontalScrollSnap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _GO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.AddChild( _GO );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _GO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    bool _WorldPositionStays = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.AddChild( _GO, _WorldPositionStays );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.UI.Extensions.HorizontalScrollSnap.AddChild!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveChild(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.HorizontalScrollSnap gen_to_be_invoked = (UnityEngine.UI.Extensions.HorizontalScrollSnap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.GameObject _ChildRemoved;
                    
                    gen_to_be_invoked.RemoveChild( _index, out _ChildRemoved );
                    translator.Push(L, _ChildRemoved);
                        
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    bool _WorldPositionStays = LuaAPI.lua_toboolean(L, 3);
                    UnityEngine.GameObject _ChildRemoved;
                    
                    gen_to_be_invoked.RemoveChild( _index, _WorldPositionStays, out _ChildRemoved );
                    translator.Push(L, _ChildRemoved);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.UI.Extensions.HorizontalScrollSnap.RemoveChild!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllChildren(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.HorizontalScrollSnap gen_to_be_invoked = (UnityEngine.UI.Extensions.HorizontalScrollSnap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    UnityEngine.GameObject[] _ChildrenRemoved;
                    
                    gen_to_be_invoked.RemoveAllChildren( out _ChildrenRemoved );
                    translator.Push(L, _ChildrenRemoved);
                        
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _WorldPositionStays = LuaAPI.lua_toboolean(L, 2);
                    UnityEngine.GameObject[] _ChildrenRemoved;
                    
                    gen_to_be_invoked.RemoveAllChildren( _WorldPositionStays, out _ChildrenRemoved );
                    translator.Push(L, _ChildrenRemoved);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.UI.Extensions.HorizontalScrollSnap.RemoveAllChildren!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateLayout(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.UI.Extensions.HorizontalScrollSnap gen_to_be_invoked = (UnityEngine.UI.Extensions.HorizontalScrollSnap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateLayout(  );
                    
                    
                    
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
            
            
                UnityEngine.UI.Extensions.HorizontalScrollSnap gen_to_be_invoked = (UnityEngine.UI.Extensions.HorizontalScrollSnap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnEndDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
