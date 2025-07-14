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
    public class RenderToolsAnimInstanceAnimInstanceWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(RenderTools.AnimInstance.AnimInstance);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 7, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentTime", _m_GetCurrentTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimInstanceFrameInfos", _m_GetAnimInstanceFrameInfos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayDefaultAnim", _m_PlayDefaultAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimFrameCount", _m_GetAnimFrameCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimTime", _m_GetAnimTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimIndex", _m_GetAnimIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayStartTime", _m_PlayStartTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayPose", _m_PlayPose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Stop", _m_Stop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUp", _m_SetUp);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AnimSpeed", _g_get_AnimSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurAnimName", _g_get_CurAnimName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurAnimIndex", _g_get_CurAnimIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurFrame", _g_get_CurFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsPlaying", _g_get_IsPlaying);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RenderMatBlockDic", _g_get_RenderMatBlockDic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isDebug", _g_get_isDebug);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "AnimSpeed", _s_set_AnimSpeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isDebug", _s_set_isDebug);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "S_FPS", RenderTools.AnimInstance.AnimInstance.S_FPS);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new RenderTools.AnimInstance.AnimInstance();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to RenderTools.AnimInstance.AnimInstance constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetCurrentTime(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimInstanceFrameInfos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetAnimInstanceFrameInfos(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayDefaultAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.PlayDefaultAnim(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimFrameCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _playName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAnimFrameCount( _playName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _playName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAnimTime( _playName );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAnimIndex( _animName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
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
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _inActiveIfNone = LuaAPI.lua_toboolean(L, 4);
                    float _offsetTime = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.Play( _index, _speed, _inActiveIfNone, _offsetTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _inActiveIfNone = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.Play( _index, _speed, _inActiveIfNone );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.Play( _index, _speed );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Play( _index );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _inActiveIfNone = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.Play( _animName, _speed, _inActiveIfNone );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.Play( _animName, _speed );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Play( _animName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RenderTools.AnimInstance.AnimInstance.Play!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayStartTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    float _offsetTime = (float)LuaAPI.lua_tonumber(L, 4);
                    bool _inActiveIfNone = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.PlayStartTime( _animName, _speed, _offsetTime, _inActiveIfNone );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    float _offsetTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.PlayStartTime( _animName, _speed, _offsetTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.PlayStartTime( _animName, _speed );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.PlayStartTime( _animName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RenderTools.AnimInstance.AnimInstance.PlayStartTime!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayPose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    int _frame = LuaAPI.xlua_tointeger(L, 3);
                    bool _inActiveIfNone = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.PlayPose( _index, _frame, _inActiveIfNone );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    int _frame = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.PlayPose( _index, _frame );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    int _frame = LuaAPI.xlua_tointeger(L, 3);
                    bool _inActiveIfNone = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.PlayPose( _animName, _frame, _inActiveIfNone );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _animName = LuaAPI.lua_tostring(L, 2);
                    int _frame = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.PlayPose( _animName, _frame );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RenderTools.AnimInstance.AnimInstance.PlayPose!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Stop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<System.Collections.Generic.List<string>>(L, 2)&& translator.Assignable<System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo>>(L, 3)&& translator.Assignable<UnityEngine.MeshRenderer[]>(L, 4)&& translator.Assignable<UnityEngine.Bounds>(L, 5)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Transform>>(L, 6)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)) 
                {
                    System.Collections.Generic.List<string> _selectClipNames = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
                    System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo> _animMeshInstanceFrameInfos = (System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo>));
                    UnityEngine.MeshRenderer[] _meshRenderers = (UnityEngine.MeshRenderer[])translator.GetObject(L, 4, typeof(UnityEngine.MeshRenderer[]));
                    UnityEngine.Bounds _bounds;translator.Get(L, 5, out _bounds);
                    System.Collections.Generic.List<UnityEngine.Transform> _effectTransList = (System.Collections.Generic.List<UnityEngine.Transform>)translator.GetObject(L, 6, typeof(System.Collections.Generic.List<UnityEngine.Transform>));
                    string _defaultState = LuaAPI.lua_tostring(L, 7);
                    
                    gen_to_be_invoked.SetUp( _selectClipNames, _animMeshInstanceFrameInfos, _meshRenderers, _bounds, _effectTransList, _defaultState );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& translator.Assignable<System.Collections.Generic.List<string>>(L, 2)&& translator.Assignable<System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo>>(L, 3)&& translator.Assignable<UnityEngine.MeshRenderer[]>(L, 4)&& translator.Assignable<UnityEngine.Bounds>(L, 5)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Transform>>(L, 6)) 
                {
                    System.Collections.Generic.List<string> _selectClipNames = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
                    System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo> _animMeshInstanceFrameInfos = (System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<RenderTools.AnimInstance.AnimInstanceFrameInfo>));
                    UnityEngine.MeshRenderer[] _meshRenderers = (UnityEngine.MeshRenderer[])translator.GetObject(L, 4, typeof(UnityEngine.MeshRenderer[]));
                    UnityEngine.Bounds _bounds;translator.Get(L, 5, out _bounds);
                    System.Collections.Generic.List<UnityEngine.Transform> _effectTransList = (System.Collections.Generic.List<UnityEngine.Transform>)translator.GetObject(L, 6, typeof(System.Collections.Generic.List<UnityEngine.Transform>));
                    
                    gen_to_be_invoked.SetUp( _selectClipNames, _animMeshInstanceFrameInfos, _meshRenderers, _bounds, _effectTransList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RenderTools.AnimInstance.AnimInstance.SetUp!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AnimSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AnimSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurAnimName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.CurAnimName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurAnimIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CurAnimIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CurFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsPlaying(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsPlaying);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RenderMatBlockDic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RenderMatBlockDic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isDebug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isDebug);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AnimSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AnimSpeed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isDebug(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RenderTools.AnimInstance.AnimInstance gen_to_be_invoked = (RenderTools.AnimInstance.AnimInstance)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isDebug = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
