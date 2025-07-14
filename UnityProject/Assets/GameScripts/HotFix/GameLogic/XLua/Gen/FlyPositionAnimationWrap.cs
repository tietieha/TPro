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
    public class FlyPositionAnimationWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FlyPositionAnimation);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialized", _m_Initialized);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFlyMode", _m_SetFlyMode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dequeue", _m_Dequeue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_posX", _g_get_m_posX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_posY", _g_get_m_posY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_posZ", _g_get_m_posZ);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Time", _g_get_m_Time);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_delTime", _g_get_m_delTime);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_posX", _s_set_m_posX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_posY", _s_set_m_posY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_posZ", _s_set_m_posZ);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Time", _s_set_m_Time);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_delTime", _s_set_m_delTime);
            
			
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
					
					var gen_ret = new FlyPositionAnimation();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FlyPositionAnimation constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialized(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    int _defalutNum = LuaAPI.xlua_tointeger(L, 3);
                    int _maxSize = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.Initialized( __object, _defalutNum, _maxSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFlyMode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<System.Action>(L, 6)) 
                {
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector3 __startpostion;translator.Get(L, 3, out __startpostion);
                    UnityEngine.Vector3 __ednposition;translator.Get(L, 4, out __ednposition);
                    int __index = LuaAPI.xlua_tointeger(L, 5);
                    System.Action __action = translator.GetDelegate<System.Action>(L, 6);
                    
                    gen_to_be_invoked.SetFlyMode( __object, __startpostion, __ednposition, __index, __action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector3 __startpostion;translator.Get(L, 3, out __startpostion);
                    UnityEngine.Vector3 __ednposition;translator.Get(L, 4, out __ednposition);
                    int __index = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetFlyMode( __object, __startpostion, __ednposition, __index );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)) 
                {
                    UnityEngine.GameObject __object = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector3 __startpostion;translator.Get(L, 3, out __startpostion);
                    UnityEngine.Vector3 __ednposition;translator.Get(L, 4, out __ednposition);
                    
                    gen_to_be_invoked.SetFlyMode( __object, __startpostion, __ednposition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FlyPositionAnimation.SetFlyMode!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dequeue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject __gameObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.Dequeue( __gameObject );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_posX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_posX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_posY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_posY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_posZ(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_posZ);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Time(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_Time);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_delTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_delTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_posX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_posX = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_posY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_posY = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_posZ(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_posZ = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Time(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_Time = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_delTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FlyPositionAnimation gen_to_be_invoked = (FlyPositionAnimation)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_delTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
