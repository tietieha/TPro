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
    public class MPathFindingBattleMapDebuggerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.PathFinding.BattleMapDebugger);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Enable", _m_Enable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearAll", _m_ClearAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterBattleMapDebugger", _m_RegisterBattleMapDebugger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeDebugUnit", _m_ChangeDebugUnit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeType", _m_ChangeType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawHollowRectangle", _m_DrawHollowRectangle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateVirtualHero", _m_UpdateVirtualHero);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateNode", _m_UpdateNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GeneratePath", _m_GeneratePath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemovePath", _m_RemovePath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Awake", _m_Awake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "_groundCube", _g_get__groundCube);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_allNode", _g_get__allNode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_virtualHero", _g_get__virtualHero);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_currentDebugUnit", _g_get__currentDebugUnit);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_currentUnitType", _g_get__currentUnitType);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "_groundCube", _s_set__groundCube);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_allNode", _s_set__allNode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_virtualHero", _s_set__virtualHero);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_currentDebugUnit", _s_set__currentDebugUnit);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_currentUnitType", _s_set__currentUnitType);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInstance", _m_GetInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Release", _m_Release_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new M.PathFinding.BattleMapDebugger();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.BattleMapDebugger constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = M.PathFinding.BattleMapDebugger.GetInstance(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Enable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMap));
                    
                    gen_to_be_invoked.Enable( _map );
                    
                    
                    
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
                    
                    M.PathFinding.BattleMapDebugger.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterBattleMapDebugger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BattleMap _map = (M.PathFinding.BattleMap)translator.GetObject(L, 2, typeof(M.PathFinding.BattleMap));
                    
                    gen_to_be_invoked.RegisterBattleMapDebugger( _map );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeDebugUnit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeDebugUnit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ChangeType(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawHollowRectangle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _center;translator.Get(L, 2, out _center);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    int _side = LuaAPI.xlua_tointeger(L, 4);
                    float _showTime = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.DrawHollowRectangle( _center, _radius, _side, _showTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateVirtualHero(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateVirtualHero(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.HashSet<M.PathFinding.Node> _nodeSet = (System.Collections.Generic.HashSet<M.PathFinding.Node>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<M.PathFinding.Node>));
                    
                    gen_to_be_invoked.UpdateNode( _nodeSet );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GeneratePath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.Queue<FixPoint.FixInt2> _path = (System.Collections.Generic.Queue<FixPoint.FixInt2>)translator.GetObject(L, 3, typeof(System.Collections.Generic.Queue<FixPoint.FixInt2>));
                    
                    gen_to_be_invoked.GeneratePath( _unitId, _path );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemovePath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _unitId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.RemovePath( _unitId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Awake(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Awake(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__groundCube(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._groundCube);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__allNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._allNode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__virtualHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._virtualHero);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__currentDebugUnit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._currentDebugUnit);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__currentUnitType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                translator.PushMPathFindingEUnitType(L, gen_to_be_invoked._currentUnitType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__groundCube(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._groundCube = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__allNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._allNode = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__virtualHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._virtualHero = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__currentDebugUnit(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._currentDebugUnit = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__currentUnitType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.BattleMapDebugger gen_to_be_invoked = (M.PathFinding.BattleMapDebugger)translator.FastGetCSObj(L, 1);
                M.PathFinding.EUnitType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked._currentUnitType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
