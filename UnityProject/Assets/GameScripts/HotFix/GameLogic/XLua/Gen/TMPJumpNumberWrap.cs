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
    public class TMPJumpNumberWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(TMPJumpNumber);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitNumber", _m_InitNumber);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetNumber", _m_SetNumber);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopJump", _m_StopJump);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "text", _g_get_text);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "text", _s_set_text);
            
			
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
					
					var gen_ret = new TMPJumpNumber();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to TMPJumpNumber constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitNumber(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPJumpNumber gen_to_be_invoked = (TMPJumpNumber)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _number = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.InitNumber( _number );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetNumber(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPJumpNumber gen_to_be_invoked = (TMPJumpNumber)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) || LuaAPI.lua_isint64(L, 2))&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    long _number = LuaAPI.lua_toint64(L, 2);
                    bool _isS2Sec = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetNumber( _number, _isS2Sec );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) || LuaAPI.lua_isint64(L, 2))) 
                {
                    long _number = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.SetNumber( _number );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TMPJumpNumber.SetNumber!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopJump(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TMPJumpNumber gen_to_be_invoked = (TMPJumpNumber)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopJump(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_text(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPJumpNumber gen_to_be_invoked = (TMPJumpNumber)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.text);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_text(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TMPJumpNumber gen_to_be_invoked = (TMPJumpNumber)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.text = (TMPro.TextMeshProUGUI)translator.GetObject(L, 2, typeof(TMPro.TextMeshProUGUI));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
