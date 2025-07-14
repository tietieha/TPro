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
    public class HUDUIHUDBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HUDUI.HUDBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 3, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ExecuteMesh", _m_ExecuteMesh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Release", _m_Release);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHpTitles", _m_GetHpTitles);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateHpTitle", _m_CreateHpTitle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasHpTitle", _m_HasHpTitle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeFollowTarget", _m_ChangeFollowTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowHpTitle", _m_ShowHpTitle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearHpFollowTarget", _m_ClearHpFollowTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetHpRate", _m_SetHpRate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowJumpWorld", _m_ShowJumpWorld);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "HPContext", _g_get_HPContext);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isDebug", _g_get_isDebug);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "target", _g_get_target);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "isDebug", _s_set_isDebug);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "target", _s_set_target);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 2, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetHUDMainCamera", _m_GetHUDMainCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetHUDUICamera", _m_GetHUDUICamera_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "s_HUDCamera", _g_get_s_HUDCamera);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "s_HUDUICamera", _g_get_s_HUDUICamera);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "s_HUDCamera", _s_set_s_HUDCamera);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "s_HUDUICamera", _s_set_s_HUDUICamera);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new HUDUI.HUDBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Camera>(L, 2)) 
                {
                    UnityEngine.Camera _sceneCam = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.Init( _sceneCam );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour.Init!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExecuteMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ExecuteMesh(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHUDMainCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = HUDUI.HUDBehaviour.GetHUDMainCamera(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHUDUICamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = HUDUI.HUDBehaviour.GetHUDUICamera(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHpTitles(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetHpTitles(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateHpTitle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<UnityEngine.Transform>(L, 7)) 
                {
                    int _hpType = LuaAPI.xlua_tointeger(L, 2);
                    int _entityId = LuaAPI.xlua_tointeger(L, 3);
                    int _width = LuaAPI.xlua_tointeger(L, 4);
                    int _height = LuaAPI.xlua_tointeger(L, 5);
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 6);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 7, typeof(UnityEngine.Transform));
                    
                        var gen_ret = gen_to_be_invoked.CreateHpTitle( _hpType, _entityId, _width, _height, _offsetY, _target );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _hpType = LuaAPI.xlua_tointeger(L, 2);
                    int _entityId = LuaAPI.xlua_tointeger(L, 3);
                    int _width = LuaAPI.xlua_tointeger(L, 4);
                    int _height = LuaAPI.xlua_tointeger(L, 5);
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.CreateHpTitle( _hpType, _entityId, _width, _height, _offsetY );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _hpType = LuaAPI.xlua_tointeger(L, 2);
                    int _entityId = LuaAPI.xlua_tointeger(L, 3);
                    int _width = LuaAPI.xlua_tointeger(L, 4);
                    int _height = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.CreateHpTitle( _hpType, _entityId, _width, _height );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _hpType = LuaAPI.xlua_tointeger(L, 2);
                    int _entityId = LuaAPI.xlua_tointeger(L, 3);
                    int _width = LuaAPI.xlua_tointeger(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.CreateHpTitle( _hpType, _entityId, _width );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _hpType = LuaAPI.xlua_tointeger(L, 2);
                    int _entityId = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.CreateHpTitle( _hpType, _entityId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour.CreateHpTitle!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasHpTitle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _entityId = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.HasHpTitle( _entityId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeFollowTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _entityId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.ChangeFollowTarget( _entityId, _target, _offsetY );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    int _entityId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.ChangeFollowTarget( _entityId, _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour.ChangeFollowTarget!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowHpTitle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _titleId = LuaAPI.xlua_tointeger(L, 2);
                    bool _bShow = LuaAPI.lua_toboolean(L, 3);
                    float _showTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.ShowHpTitle( _titleId, _bShow, _showTime );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int _titleId = LuaAPI.xlua_tointeger(L, 2);
                    bool _bShow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.ShowHpTitle( _titleId, _bShow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour.ShowHpTitle!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearHpFollowTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _entityId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ClearHpFollowTarget( _entityId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetHpRate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _titleId = LuaAPI.xlua_tointeger(L, 2);
                    float _hpRate = (float)LuaAPI.lua_tonumber(L, 3);
                    float _preHpRate = (float)LuaAPI.lua_tonumber(L, 4);
                    float _time = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetHpRate( _titleId, _hpRate, _preHpRate, _time );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _titleId = LuaAPI.xlua_tointeger(L, 2);
                    float _hpRate = (float)LuaAPI.lua_tonumber(L, 3);
                    float _preHpRate = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetHpRate( _titleId, _hpRate, _preHpRate );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _titleId = LuaAPI.xlua_tointeger(L, 2);
                    float _hpRate = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetHpRate( _titleId, _hpRate );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour.SetHpRate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowJumpWorld(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    int _eneityId = LuaAPI.xlua_tointeger(L, 2);
                    int _nType = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    int _number = LuaAPI.xlua_tointeger(L, 5);
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 6);
                    float _scale = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.ShowJumpWorld( _eneityId, _nType, _target, _number, _offsetY, _scale );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _eneityId = LuaAPI.xlua_tointeger(L, 2);
                    int _nType = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    int _number = LuaAPI.xlua_tointeger(L, 5);
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.ShowJumpWorld( _eneityId, _nType, _target, _number, _offsetY );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _eneityId = LuaAPI.xlua_tointeger(L, 2);
                    int _nType = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    int _number = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.ShowJumpWorld( _eneityId, _nType, _target, _number );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Transform>(L, 4)) 
                {
                    int _eneityId = LuaAPI.xlua_tointeger(L, 2);
                    int _nType = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.ShowJumpWorld( _eneityId, _nType, _target );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _eneityId = LuaAPI.xlua_tointeger(L, 2);
                    int _nType = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.ShowJumpWorld( _eneityId, _nType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HUDUI.HUDBehaviour.ShowJumpWorld!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HPContext(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.HPContext);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_s_HUDCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, HUDUI.HUDBehaviour.s_HUDCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_s_HUDUICamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, HUDUI.HUDBehaviour.s_HUDUICamera);
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
			
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isDebug);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_target(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.target);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_s_HUDCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    HUDUI.HUDBehaviour.s_HUDCamera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_s_HUDUICamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    HUDUI.HUDBehaviour.s_HUDUICamera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
            
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
			
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isDebug = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_target(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HUDUI.HUDBehaviour gen_to_be_invoked = (HUDUI.HUDBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.target = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
