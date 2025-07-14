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
    public class PVEFogOfWarWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(PVEFogOfWar);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetData", _m_SetData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFogMesh", _m_GetFogMesh);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ISPVEFogOfWar", _g_get_ISPVEFogOfWar);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DebugPveFog", _g_get_DebugPveFog);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PVEFogOfWarMesh", _g_get_PVEFogOfWarMesh);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PVEFogOfWarMaterial", _g_get_PVEFogOfWarMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PVEFogOfWarFadeMaterial", _g_get_PVEFogOfWarFadeMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PVEFogOfWarMaskEdge", _g_get_PVEFogOfWarMaskEdge);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ISPVEFogOfWar", _s_set_ISPVEFogOfWar);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DebugPveFog", _s_set_DebugPveFog);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PVEFogOfWarMesh", _s_set_PVEFogOfWarMesh);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PVEFogOfWarMaterial", _s_set_PVEFogOfWarMaterial);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PVEFogOfWarFadeMaterial", _s_set_PVEFogOfWarFadeMaterial);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PVEFogOfWarMaskEdge", _s_set_PVEFogOfWarMaskEdge);
            
			
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
					
					var gen_ret = new PVEFogOfWar();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to PVEFogOfWar constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SetData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFogMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float[] _vertexArr = (float[])translator.GetObject(L, 2, typeof(float[]));
                    
                        var gen_ret = gen_to_be_invoked.GetFogMesh( _vertexArr );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ISPVEFogOfWar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ISPVEFogOfWar);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DebugPveFog(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DebugPveFog);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PVEFogOfWarMesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PVEFogOfWarMesh);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PVEFogOfWarMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PVEFogOfWarMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PVEFogOfWarFadeMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PVEFogOfWarFadeMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PVEFogOfWarMaskEdge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector4(L, gen_to_be_invoked.PVEFogOfWarMaskEdge);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ISPVEFogOfWar(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ISPVEFogOfWar = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DebugPveFog(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DebugPveFog = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PVEFogOfWarMesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PVEFogOfWarMesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PVEFogOfWarMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PVEFogOfWarMaterial = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PVEFogOfWarFadeMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PVEFogOfWarFadeMaterial = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PVEFogOfWarMaskEdge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogOfWar gen_to_be_invoked = (PVEFogOfWar)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector4 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.PVEFogOfWarMaskEdge = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
