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
    public class UnityEnginePlayablesPlayableBindingWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Playables.PlayableBinding);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 3, 2);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "streamName", _g_get_streamName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sourceObject", _g_get_sourceObject);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "outputTargetType", _g_get_outputTargetType);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "streamName", _s_set_streamName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sourceObject", _s_set_sourceObject);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Playables.PlayableBinding.None);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DefaultDuration", UnityEngine.Playables.PlayableBinding.DefaultDuration);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.Push(L, default(UnityEngine.Playables.PlayableBinding));
			        return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Playables.PlayableBinding constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Playables.PlayableBinding gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.streamName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sourceObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Playables.PlayableBinding gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.Push(L, gen_to_be_invoked.sourceObject);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_outputTargetType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Playables.PlayableBinding gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.Push(L, gen_to_be_invoked.outputTargetType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_streamName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Playables.PlayableBinding gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.streamName = LuaAPI.lua_tostring(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sourceObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Playables.PlayableBinding gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.sourceObject = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
