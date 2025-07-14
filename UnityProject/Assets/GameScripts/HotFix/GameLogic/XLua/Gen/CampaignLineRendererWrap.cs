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
    public class CampaignLineRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(CampaignLineRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearPos", _m_ClearPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLineList", _m_SetLineList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateLine", _m_UpdateLine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Run", _m_Run);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetMaterial", _m_SetMaterial);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "materials", _g_get_materials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lr", _g_get_lr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerMask", _g_get_layerMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lineRendererVertical", _g_get_lineRendererVertical);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "materials", _s_set_materials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lr", _s_set_lr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layerMask", _s_set_layerMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lineRendererVertical", _s_set_lineRendererVertical);
            
			
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
					
					var gen_ret = new CampaignLineRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CampaignLineRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearPos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLineList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3[]>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3[] _lineList = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    int _type = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetLineList( _lineList, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3[]>(L, 2)) 
                {
                    UnityEngine.Vector3[] _lineList = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    
                    gen_to_be_invoked.SetLineList( _lineList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CampaignLineRenderer.SetLineList!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateLine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.UpdateLine( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Run(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Run(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMaterial(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetMaterial( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.materials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.lr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layerMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layerMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lineRendererVertical(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.lineRendererVertical);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.materials = (System.Collections.Generic.List<UnityEngine.Material>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Material>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.lr = (UnityEngine.LineRenderer)translator.GetObject(L, 2, typeof(UnityEngine.LineRenderer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layerMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.layerMask = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lineRendererVertical(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CampaignLineRenderer gen_to_be_invoked = (CampaignLineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.lineRendererVertical = (GameLogic.LineRendererVertical)translator.GetObject(L, 2, typeof(GameLogic.LineRendererVertical));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
