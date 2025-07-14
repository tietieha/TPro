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
    public class WorldMarchLineRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(World.MarchLineRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLineList", _m_SetLineList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTarget", _m_SetTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetMaterial", _m_SetMaterial);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Materials", _g_get_Materials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lr", _g_get_lr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerMask", _g_get_layerMask);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Materials", _s_set_Materials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "lr", _s_set_lr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layerMask", _s_set_layerMask);
            
			
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
					
					var gen_ret = new World.MarchLineRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to World.MarchLineRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLineList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.GameObject>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 2, out _startPos);
                    UnityEngine.Vector3 _endPos;translator.Get(L, 3, out _endPos);
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
                    int _type = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetLineList( _startPos, _endPos, _target, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.GameObject>(L, 4)) 
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 2, out _startPos);
                    UnityEngine.Vector3 _endPos;translator.Get(L, 3, out _endPos);
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.SetLineList( _startPos, _endPos, _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to World.MarchLineRenderer.SetLineList!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.SetTarget( _target );
                    
                    
                    
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
            
            
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
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
            
            
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_Materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Materials);
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
			
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
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
			
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layerMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Materials = (System.Collections.Generic.List<UnityEngine.Material>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Material>));
            
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
			
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
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
			
                World.MarchLineRenderer gen_to_be_invoked = (World.MarchLineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.layerMask = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
