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
    public class MPathFindingBattleEntityPoolManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.PathFinding.BattleEntityPoolManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 26, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GeneratePathRequestUID", _m_GeneratePathRequestUID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RestPathRequestUID", _m_RestPathRequestUID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getUnitObj", _m_getUnitObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backUnitObj", _m_backUnitObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getNodeObj", _m_getNodeObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backNodeObj", _m_backNodeObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getTeamObj", _m_getTeamObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backTeamObj", _m_backTeamObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getBSPTreeNodeObj", _m_getBSPTreeNodeObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backBSPTreeNodeObj", _m_backBSPTreeNodeObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getPathRequestObj", _m_getPathRequestObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backPathRequestObj", _m_backPathRequestObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getFastPriorityQueue", _m_getFastPriorityQueue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backFastPriorityQueue", _m_backFastPriorityQueue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getProcessedNodeNearest", _m_getProcessedNodeNearest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backProcessedNodeNearest", _m_backProcessedNodeNearest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getTargetWorldPos", _m_getTargetWorldPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backTargetWorldPos", _m_backTargetWorldPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getTargetGridPos", _m_getTargetGridPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backTargetGridPos", _m_backTargetGridPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getResultPath", _m_getResultPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backResultPath", _m_backResultPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getResultPathSimplified", _m_getResultPathSimplified);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backResultPathSimplified", _m_backResultPathSimplified);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getRestoreGridOccupyUnitList", _m_getRestoreGridOccupyUnitList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "backRestoreGridOccupyUnitList", _m_backRestoreGridOccupyUnitList);
			
			
			
			
			
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
					
					var gen_ret = new M.PathFinding.BattleEntityPoolManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.BattleEntityPoolManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GeneratePathRequestUID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GeneratePathRequestUID(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RestPathRequestUID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RestPathRequestUID(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getUnitObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getUnitObj(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backUnitObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Unit _u = (M.PathFinding.Unit)translator.GetObject(L, 2, typeof(M.PathFinding.Unit));
                    
                    gen_to_be_invoked.backUnitObj( _u );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getNodeObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getNodeObj(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backNodeObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Node _n = (M.PathFinding.Node)translator.GetObject(L, 2, typeof(M.PathFinding.Node));
                    
                    gen_to_be_invoked.backNodeObj( _n );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getTeamObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getTeamObj(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backTeamObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.Team _t = (M.PathFinding.Team)translator.GetObject(L, 2, typeof(M.PathFinding.Team));
                    
                    gen_to_be_invoked.backTeamObj( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getBSPTreeNodeObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getBSPTreeNodeObj(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backBSPTreeNodeObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.BSPTreeNode _n = (M.PathFinding.BSPTreeNode)translator.GetObject(L, 2, typeof(M.PathFinding.BSPTreeNode));
                    
                    gen_to_be_invoked.backBSPTreeNodeObj( _n );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getPathRequestObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getPathRequestObj(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backPathRequestObj(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    M.PathFinding.PathRequest _t = (M.PathFinding.PathRequest)translator.GetObject(L, 2, typeof(M.PathFinding.PathRequest));
                    
                    gen_to_be_invoked.backPathRequestObj( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getFastPriorityQueue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getFastPriorityQueue(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backFastPriorityQueue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Priority_Queue.FastPriorityQueue<M.PathFinding.Node> _t = (Priority_Queue.FastPriorityQueue<M.PathFinding.Node>)translator.GetObject(L, 2, typeof(Priority_Queue.FastPriorityQueue<M.PathFinding.Node>));
                    
                    gen_to_be_invoked.backFastPriorityQueue( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getProcessedNodeNearest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getProcessedNodeNearest(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backProcessedNodeNearest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<M.PathFinding.Node> _t = (System.Collections.Generic.List<M.PathFinding.Node>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<M.PathFinding.Node>));
                    
                    gen_to_be_invoked.backProcessedNodeNearest( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getTargetWorldPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getTargetWorldPos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backTargetWorldPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<FixPoint.FixInt2> _t = (System.Collections.Generic.List<FixPoint.FixInt2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<FixPoint.FixInt2>));
                    
                    gen_to_be_invoked.backTargetWorldPos( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getTargetGridPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getTargetGridPos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backTargetGridPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<M.PathFinding.Integer2> _t = (System.Collections.Generic.List<M.PathFinding.Integer2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<M.PathFinding.Integer2>));
                    
                    gen_to_be_invoked.backTargetGridPos( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getResultPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getResultPath(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backResultPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<FixPoint.FixInt2> _t = (System.Collections.Generic.List<FixPoint.FixInt2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<FixPoint.FixInt2>));
                    
                    gen_to_be_invoked.backResultPath( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getResultPathSimplified(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getResultPathSimplified(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backResultPathSimplified(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<FixPoint.FixInt2> _t = (System.Collections.Generic.List<FixPoint.FixInt2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<FixPoint.FixInt2>));
                    
                    gen_to_be_invoked.backResultPathSimplified( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getRestoreGridOccupyUnitList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.getRestoreGridOccupyUnitList(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_backRestoreGridOccupyUnitList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                M.PathFinding.BattleEntityPoolManager gen_to_be_invoked = (M.PathFinding.BattleEntityPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<M.PathFinding.Unit> _t = (System.Collections.Generic.List<M.PathFinding.Unit>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<M.PathFinding.Unit>));
                    
                    gen_to_be_invoked.backRestoreGridOccupyUnitList( _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
