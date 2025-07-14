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
    public class GameLogicUIVideoPlayerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLogic.UIVideoPlayer);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayVideo", _m_PlayVideo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopVideo", _m_StopVideo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SkipVideo", _m_SkipVideo);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "rawImageRect", _g_get_rawImageRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnVideoPlayEnd", _g_get_OnVideoPlayEnd);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "rawImageRect", _s_set_rawImageRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnVideoPlayEnd", _s_set_OnVideoPlayEnd);
            
			
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
					
					var gen_ret = new GameLogic.UIVideoPlayer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLogic.UIVideoPlayer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayVideo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _videoPath = LuaAPI.lua_tostring(L, 2);
                    System.Action _onEnd = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.PlayVideo( _videoPath, _onEnd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopVideo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopVideo(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SkipVideo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SkipVideo(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rawImageRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.rawImageRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnVideoPlayEnd(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnVideoPlayEnd);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rawImageRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.rawImageRect = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnVideoPlayEnd(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLogic.UIVideoPlayer gen_to_be_invoked = (GameLogic.UIVideoPlayer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnVideoPlayEnd = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
