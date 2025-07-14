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
    public class MBattleBattlePoolManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.Battle.BattlePoolManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddPoolItem", _m_AddPoolItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasPool", _m_HasPool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInstanceAsset", _m_GetInstanceAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemFromPool", _m_GetItemFromPool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BackItemToPool", _m_BackItemToPool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReleasePoolByPath", _m_ReleasePoolByPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReleaseAllPoos", _m_ReleaseAllPoos);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ReleaseBattlePoolManager", _m_ReleaseBattlePoolManager_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new M.Battle.BattlePoolManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattlePoolManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseBattlePoolManager_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    M.Battle.BattlePoolManager.ReleaseBattlePoolManager(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddPoolItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _poolName = LuaAPI.lua_tostring(L, 2);
                    string _assetPath = LuaAPI.lua_tostring(L, 3);
                    UnityEngine.Object _loadObj = (UnityEngine.Object)translator.GetObject(L, 4, typeof(UnityEngine.Object));
                    int _prepareNum = LuaAPI.xlua_tointeger(L, 5);
                    System.Action _cbk = translator.GetDelegate<System.Action>(L, 6);
                    
                    gen_to_be_invoked.AddPoolItem( _poolName, _assetPath, _loadObj, _prepareNum, _cbk );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _poolName = LuaAPI.lua_tostring(L, 2);
                    string _assetPath = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.HasPool( _poolName, _assetPath );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstanceAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetInstanceAsset( _assetPath );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemFromPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _poolName = LuaAPI.lua_tostring(L, 2);
                    string _assetPath = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetItemFromPool( _poolName, _assetPath );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BackItemToPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _poolName = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                        var gen_ret = gen_to_be_invoked.BackItemToPool( _poolName, _go );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleasePoolByPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _poolName = LuaAPI.lua_tostring(L, 2);
                    string _assetPath = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.ReleasePoolByPath( _poolName, _assetPath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseAllPoos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.Battle.BattlePoolManager gen_to_be_invoked = (M.Battle.BattlePoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _poolName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ReleaseAllPoos( _poolName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, M.Battle.BattlePoolManager.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
