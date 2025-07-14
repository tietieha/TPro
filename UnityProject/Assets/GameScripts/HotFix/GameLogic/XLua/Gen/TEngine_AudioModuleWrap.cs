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
    public class TEngineAudioModuleWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(TEngine.AudioModule);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 13, 12);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Restart", _m_Restart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Stop", _m_Stop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAll", _m_StopAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PutInAudioPool", _m_PutInAudioPool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveClipFromPool", _m_RemoveClipFromPool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CleanSoundPool", _m_CleanSoundPool);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "MAudioMixer", _g_get_MAudioMixer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InstanceRoot", _g_get_InstanceRoot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Volume", _g_get_Volume);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Enable", _g_get_Enable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MusicVolume", _g_get_MusicVolume);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SoundVolume", _g_get_SoundVolume);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UISoundVolume", _g_get_UISoundVolume);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VoiceVolume", _g_get_VoiceVolume);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MusicEnable", _g_get_MusicEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SoundEnable", _g_get_SoundEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UISoundEnable", _g_get_UISoundEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VoiceEnable", _g_get_VoiceEnable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AudioModuleImp", _g_get_AudioModuleImp);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "InstanceRoot", _s_set_InstanceRoot);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Volume", _s_set_Volume);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Enable", _s_set_Enable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MusicVolume", _s_set_MusicVolume);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SoundVolume", _s_set_SoundVolume);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UISoundVolume", _s_set_UISoundVolume);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "VoiceVolume", _s_set_VoiceVolume);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MusicEnable", _s_set_MusicEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SoundEnable", _s_set_SoundEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UISoundEnable", _s_set_UISoundEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "VoiceEnable", _s_set_VoiceEnable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AudioModuleImp", _s_set_AudioModuleImp);
            
			
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
					
					var gen_ret = new TEngine.AudioModule();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.AudioModule constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Restart(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Restart(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    int _type = LuaAPI.xlua_tointeger(L, 2);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    float _volume = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _bAsync = LuaAPI.lua_toboolean(L, 6);
                    bool _bInPool = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop, _volume, _bAsync, _bInPool );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    int _type = LuaAPI.xlua_tointeger(L, 2);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    float _volume = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _bAsync = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop, _volume, _bAsync );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _type = LuaAPI.xlua_tointeger(L, 2);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    float _volume = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop, _volume );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _type = LuaAPI.xlua_tointeger(L, 2);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    int _type = LuaAPI.xlua_tointeger(L, 2);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<TEngine.AudioType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    TEngine.AudioType _type;translator.Get(L, 2, out _type);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    float _volume = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _bAsync = LuaAPI.lua_toboolean(L, 6);
                    bool _bInPool = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop, _volume, _bAsync, _bInPool );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<TEngine.AudioType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    TEngine.AudioType _type;translator.Get(L, 2, out _type);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    float _volume = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _bAsync = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop, _volume, _bAsync );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<TEngine.AudioType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    TEngine.AudioType _type;translator.Get(L, 2, out _type);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    float _volume = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop, _volume );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<TEngine.AudioType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    TEngine.AudioType _type;translator.Get(L, 2, out _type);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    bool _bLoop = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path, _bLoop );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<TEngine.AudioType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    TEngine.AudioType _type;translator.Get(L, 2, out _type);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Play( _type, _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.AudioModule.Play!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    TEngine.AudioType _type;translator.Get(L, 2, out _type);
                    bool _fadeout = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.Stop( _type, _fadeout );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _fadeout = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.StopAll( _fadeout );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PutInAudioPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<string> _list = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
                    
                    gen_to_be_invoked.PutInAudioPool( _list );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveClipFromPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<string> _list = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
                    
                    gen_to_be_invoked.RemoveClipFromPool( _list );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CleanSoundPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CleanSoundPool(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MAudioMixer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MAudioMixer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InstanceRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.InstanceRoot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Volume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Volume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Enable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Enable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MusicVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MusicVolume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SoundVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SoundVolume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UISoundVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.UISoundVolume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VoiceVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.VoiceVolume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MusicEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.MusicEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SoundEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SoundEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UISoundEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UISoundEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VoiceEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.VoiceEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AudioModuleImp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.AudioModuleImp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InstanceRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InstanceRoot = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Volume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Volume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Enable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Enable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MusicVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MusicVolume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SoundVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SoundVolume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UISoundVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UISoundVolume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VoiceVolume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.VoiceVolume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MusicEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MusicEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SoundEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SoundEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UISoundEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UISoundEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VoiceEnable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.VoiceEnable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AudioModuleImp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.AudioModule gen_to_be_invoked = (TEngine.AudioModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AudioModuleImp = (TEngine.IAudioModule)translator.GetObject(L, 2, typeof(TEngine.IAudioModule));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
