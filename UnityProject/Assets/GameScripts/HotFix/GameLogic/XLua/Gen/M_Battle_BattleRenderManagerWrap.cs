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
    public class MBattleBattleRenderManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.Battle.BattleRenderManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 18, 3, 3);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRotateAndoPos", _m_SetRotateAndoPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRotationImmediately", _m_SetRotationImmediately_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRotation", _m_SetRotation_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRotateAndoPos3", _m_SetRotateAndoPos3_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetPos3", _m_SetPos3_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetActive", _m_SetActive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_Replay", _m_Animator_Replay_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_CrossFadeRandom", _m_Animator_CrossFadeRandom_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_CrossFade", _m_Animator_CrossFade_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_Speed", _m_Animator_Speed_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_Play", _m_Animator_Play_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_GetCurrentStatePlayTime", _m_Animator_GetCurrentStatePlayTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_GetCurrentStateTime", _m_Animator_GetCurrentStateTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Animator_PlayStartTime", _m_Animator_PlayStartTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Get_Transf_PosXYZ", _m_Get_Transf_PosXYZ_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetReversePos", _m_GetReversePos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetNextPos3AndAlpha", _m_SetNextPos3AndAlpha_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "TimeScale", _g_get_TimeScale);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MagInterval", _g_get_MagInterval);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "PosInterval", _g_get_PosInterval);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "TimeScale", _s_set_TimeScale);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "MagInterval", _s_set_MagInterval);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "PosInterval", _s_set_PosInterval);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new M.Battle.BattleRenderManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.Battle.BattleRenderManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRotateAndoPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    M.Battle.BattleRenderManager.SetRotateAndoPos( _transform, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRotationImmediately_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleRenderManager.SetRotationImmediately( _transform, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRotation_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleRenderManager.SetRotation( _transform, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRotateAndoPos3_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    M.Battle.BattleRenderManager.SetRotateAndoPos3( _transform, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPos3_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    M.Battle.BattleRenderManager.SetPos3( _transform, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetActive_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    bool _active = LuaAPI.lua_toboolean(L, 2);
                    
                    M.Battle.BattleRenderManager.SetActive( _obj, _active );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_Replay_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _aniName = LuaAPI.lua_tostring(L, 2);
                    
                    M.Battle.BattleRenderManager.Animator_Replay( _actor, _aniName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_CrossFadeRandom_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _aniName = LuaAPI.lua_tostring(L, 2);
                    float _crossFadeTime = (float)LuaAPI.lua_tonumber(L, 3);
                    float _normalizedTime = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    M.Battle.BattleRenderManager.Animator_CrossFadeRandom( _actor, _aniName, _crossFadeTime, _normalizedTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_CrossFade_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _aniName = LuaAPI.lua_tostring(L, 2);
                    float _crossFadeTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleRenderManager.Animator_CrossFade( _actor, _aniName, _crossFadeTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_Speed_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    float _speed = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    M.Battle.BattleRenderManager.Animator_Speed( _actor, _speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_Play_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _aniName = LuaAPI.lua_tostring(L, 2);
                    
                    M.Battle.BattleRenderManager.Animator_Play( _actor, _aniName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_GetCurrentStatePlayTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    
                        var gen_ret = M.Battle.BattleRenderManager.Animator_GetCurrentStatePlayTime( _actor );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_GetCurrentStateTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    
                        var gen_ret = M.Battle.BattleRenderManager.Animator_GetCurrentStateTime( _actor );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Animator_PlayStartTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator _actor = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _aniName = LuaAPI.lua_tostring(L, 2);
                    float _startTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    M.Battle.BattleRenderManager.Animator_PlayStartTime( _actor, _aniName, _startTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get_Transf_PosXYZ_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _xf = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x;
                    float _y;
                    float _z;
                    
                        var gen_ret = M.Battle.BattleRenderManager.Get_Transf_PosXYZ( _xf, out _x, out _y, out _z );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushnumber(L, _x);
                        
                    LuaAPI.lua_pushnumber(L, _y);
                        
                    LuaAPI.lua_pushnumber(L, _z);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetReversePos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _speed = (float)LuaAPI.lua_tonumber(L, 2);
                    float _t = (float)LuaAPI.lua_tonumber(L, 3);
                    float _x;
                    float _z;
                    
                    M.Battle.BattleRenderManager.GetReversePos( _transform, _speed, _t, out _x, out _z );
                    LuaAPI.lua_pushnumber(L, _x);
                        
                    LuaAPI.lua_pushnumber(L, _z);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetNextPos3AndAlpha_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    float _alpha = (float)LuaAPI.lua_tonumber(L, 5);
                    float _t = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    M.Battle.BattleRenderManager.SetNextPos3AndAlpha( _transform, _x, _y, _z, _alpha, _t );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TimeScale(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, M.Battle.BattleRenderManager.TimeScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MagInterval(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, M.Battle.BattleRenderManager.MagInterval);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PosInterval(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, M.Battle.BattleRenderManager.PosInterval);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TimeScale(RealStatePtr L)
        {
		    try {
                
			    M.Battle.BattleRenderManager.TimeScale = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MagInterval(RealStatePtr L)
        {
		    try {
                
			    M.Battle.BattleRenderManager.MagInterval = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PosInterval(RealStatePtr L)
        {
		    try {
                
			    M.Battle.BattleRenderManager.PosInterval = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
