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
    public class RVOVector2Wrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(RVO.Vector2);
			Utils.BeginObjectRegister(type, L, translator, 6, 5, 2, 2);
			Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__mul", __MulMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__div", __DivMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__add", __AddMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__sub", __SubMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__unm", __UnmMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__eq", __EqMeta);
            
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "x", _m_x);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "y", _m_y);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DistanceSquared", _m_DistanceSquared);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Distance", _m_Distance);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "x_", _g_get_x_);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "y_", _g_get_y_);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "x_", _s_set_x_);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "y_", _s_set_y_);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Angle", _m_Angle_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Zero", _g_get_Zero);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Zero", _s_set_Zero);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					float _x = (float)LuaAPI.lua_tonumber(L, 2);
					float _y = (float)LuaAPI.lua_tonumber(L, 3);
					
					var gen_ret = new RVO.Vector2(_x, _y);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.Push(L, default(RVO.Vector2));
			        return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Vector2 constructor!");
            
        }
        
		
        
		
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __MulMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<RVO.Vector2>(L, 1) && translator.Assignable<RVO.Vector2>(L, 2))
				{
					RVO.Vector2 leftside;translator.Get(L, 1, out leftside);
					RVO.Vector2 rightside;translator.Get(L, 2, out rightside);
					
					LuaAPI.lua_pushnumber(L, leftside * rightside);
					
					return 1;
				}
            
			
				if (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) && translator.Assignable<RVO.Vector2>(L, 2))
				{
					float leftside = (float)LuaAPI.lua_tonumber(L, 1);
					RVO.Vector2 rightside;translator.Get(L, 2, out rightside);
					
					translator.Push(L, leftside * rightside);
					
					return 1;
				}
            
			
				if (translator.Assignable<RVO.Vector2>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					RVO.Vector2 leftside;translator.Get(L, 1, out leftside);
					float rightside = (float)LuaAPI.lua_tonumber(L, 2);
					
					translator.Push(L, leftside * rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of * operator, need RVO.Vector2!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __DivMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<RVO.Vector2>(L, 1) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					RVO.Vector2 leftside;translator.Get(L, 1, out leftside);
					float rightside = (float)LuaAPI.lua_tonumber(L, 2);
					
					translator.Push(L, leftside / rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of / operator, need RVO.Vector2!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __AddMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<RVO.Vector2>(L, 1) && translator.Assignable<RVO.Vector2>(L, 2))
				{
					RVO.Vector2 leftside;translator.Get(L, 1, out leftside);
					RVO.Vector2 rightside;translator.Get(L, 2, out rightside);
					
					translator.Push(L, leftside + rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of + operator, need RVO.Vector2!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __SubMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<RVO.Vector2>(L, 1) && translator.Assignable<RVO.Vector2>(L, 2))
				{
					RVO.Vector2 leftside;translator.Get(L, 1, out leftside);
					RVO.Vector2 rightside;translator.Get(L, 2, out rightside);
					
					translator.Push(L, leftside - rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of - operator, need RVO.Vector2!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __UnmMeta(RealStatePtr L)
        {
            
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
                RVO.Vector2 rightside;translator.Get(L, 1, out rightside);
                translator.Push(L, - rightside);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __EqMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<RVO.Vector2>(L, 1) && translator.Assignable<RVO.Vector2>(L, 2))
				{
					RVO.Vector2 leftside;translator.Get(L, 1, out leftside);
					RVO.Vector2 rightside;translator.Get(L, 2, out rightside);
					
					LuaAPI.lua_pushboolean(L, leftside == rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of == operator, need RVO.Vector2!");
            
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_x(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.x(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_y(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.y(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DistanceSquared(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    RVO.Vector2 _other;translator.Get(L, 2, out _other);
                    
                        var gen_ret = gen_to_be_invoked.DistanceSquared( _other );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Distance(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    RVO.Vector2 _other;translator.Get(L, 2, out _other);
                    
                        var gen_ret = gen_to_be_invoked.Distance( _other );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.Update(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Angle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RVO.Vector2 _from;translator.Get(L, 1, out _from);
                    RVO.Vector2 _to;translator.Get(L, 2, out _to);
                    
                        var gen_ret = RVO.Vector2.Angle( _from, _to );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_x_(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.x_);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_y_(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.y_);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Zero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, RVO.Vector2.Zero);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_x_(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.x_ = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_y_(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                RVO.Vector2 gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                gen_to_be_invoked.y_ = (float)LuaAPI.lua_tonumber(L, 2);
            
                translator.Update(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Zero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			RVO.Vector2 gen_value;translator.Get(L, 1, out gen_value);
				RVO.Vector2.Zero = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
