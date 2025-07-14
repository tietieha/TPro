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
    public class SystemCollectionsICollectionWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(System.Collections.ICollection);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 3, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CopyTo", _m_CopyTo);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Count", _g_get_Count);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SyncRoot", _g_get_SyncRoot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsSynchronized", _g_get_IsSynchronized);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "System.Collections.ICollection does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CopyTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Collections.ICollection gen_to_be_invoked = (System.Collections.ICollection)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Array _array = (System.Array)translator.GetObject(L, 2, typeof(System.Array));
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.CopyTo( _array, _index );
                    
                    
                    
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
			
                System.Collections.ICollection gen_to_be_invoked = (System.Collections.ICollection)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Count);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SyncRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Collections.ICollection gen_to_be_invoked = (System.Collections.ICollection)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.SyncRoot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsSynchronized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Collections.ICollection gen_to_be_invoked = (System.Collections.ICollection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsSynchronized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
