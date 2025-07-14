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
    public class MPathFindingTeamWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.PathFinding.Team);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 7, 7);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddWillAttackUnit", _m_AddWillAttackUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveWillAttackUnit", _m_RemoveWillAttackUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddWillAttackSourceUnit", _m_AddWillAttackSourceUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveWillAttackSourceUnit", _m_RemoveWillAttackSourceUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsLive", _m_IsLive);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Id", _g_get_Id);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Hero", _g_get_Hero);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AllUnitList", _g_get_AllUnitList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WillAttackTargetUnitSet", _g_get_WillAttackTargetUnitSet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WillAttackSourceUnitSet", _g_get_WillAttackSourceUnitSet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WillAttackTargetTeamSet", _g_get_WillAttackTargetTeamSet);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WillAttackSourceTeamSet", _g_get_WillAttackSourceTeamSet);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Id", _s_set_Id);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Hero", _s_set_Hero);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AllUnitList", _s_set_AllUnitList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WillAttackTargetUnitSet", _s_set_WillAttackTargetUnitSet);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WillAttackSourceUnitSet", _s_set_WillAttackSourceUnitSet);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WillAttackTargetTeamSet", _s_set_WillAttackTargetTeamSet);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WillAttackSourceTeamSet", _s_set_WillAttackSourceTeamSet);
            
			
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
					
					var gen_ret = new M.PathFinding.Team();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.Team constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddWillAttackUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.AddWillAttackUnit( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveWillAttackUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.RemoveWillAttackUnit( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddWillAttackSourceUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.AddWillAttackSourceUnit( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveWillAttackSourceUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.RemoveWillAttackSourceUnit( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsLive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsLive(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Id(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Id);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Hero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Hero);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AllUnitList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AllUnitList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WillAttackTargetUnitSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WillAttackTargetUnitSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WillAttackSourceUnitSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WillAttackSourceUnitSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WillAttackTargetTeamSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WillAttackTargetTeamSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WillAttackSourceTeamSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WillAttackSourceTeamSet);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Id(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Id = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Hero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Hero = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AllUnitList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AllUnitList = (System.Collections.Generic.List<M.PathFinding.Unit>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<M.PathFinding.Unit>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WillAttackTargetUnitSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WillAttackTargetUnitSet = (System.Collections.Generic.HashSet<M.PathFinding.Unit>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<M.PathFinding.Unit>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WillAttackSourceUnitSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WillAttackSourceUnitSet = (System.Collections.Generic.HashSet<M.PathFinding.Unit>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<M.PathFinding.Unit>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WillAttackTargetTeamSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WillAttackTargetTeamSet = (System.Collections.Generic.Dictionary<M.PathFinding.Team, int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<M.PathFinding.Team, int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WillAttackSourceTeamSet(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.Team gen_to_be_invoked = (M.PathFinding.Team)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WillAttackSourceTeamSet = (System.Collections.Generic.Dictionary<M.PathFinding.Team, int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<M.PathFinding.Team, int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
