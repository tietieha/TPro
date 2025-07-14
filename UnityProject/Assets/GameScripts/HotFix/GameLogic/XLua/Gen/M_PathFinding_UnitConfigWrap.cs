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
    public class MPathFindingUnitConfigWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(M.PathFinding.UnitConfig);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 16, 16);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "_worldPos", _g_get__worldPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_id", _g_get__id);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_teamId", _g_get__teamId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_side", _g_get__side);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_isHero", _g_get__isHero);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_isVirtual", _g_get__isVirtual);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_isInstrument", _g_get__isInstrument);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_isMelee", _g_get__isMelee);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_speed", _g_get__speed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_attackRange", _g_get__attackRange);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_radius", _g_get__radius);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_extend", _g_get__extend);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_greenCircleRadius", _g_get__greenCircleRadius);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_meleeStandNum", _g_get__meleeStandNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_rangeStandNum", _g_get__rangeStandNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_luaDatAccess", _g_get__luaDatAccess);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "_worldPos", _s_set__worldPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_id", _s_set__id);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_teamId", _s_set__teamId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_side", _s_set__side);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_isHero", _s_set__isHero);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_isVirtual", _s_set__isVirtual);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_isInstrument", _s_set__isInstrument);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_isMelee", _s_set__isMelee);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_speed", _s_set__speed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_attackRange", _s_set__attackRange);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_radius", _s_set__radius);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_extend", _s_set__extend);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_greenCircleRadius", _s_set__greenCircleRadius);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_meleeStandNum", _s_set__meleeStandNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_rangeStandNum", _s_set__rangeStandNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_luaDatAccess", _s_set__luaDatAccess);
            
			
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
				if(LuaAPI.lua_gettop(L) == 17 && translator.Assignable<FixPoint.FixInt2>(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 11) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 12) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 13) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 14) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 15) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 16) && translator.Assignable<LuaArrAccess>(L, 17))
				{
					FixPoint.FixInt2 _worldPos;translator.Get(L, 2, out _worldPos);
					int _id = LuaAPI.xlua_tointeger(L, 3);
					int _teamId = LuaAPI.xlua_tointeger(L, 4);
					int _side = LuaAPI.xlua_tointeger(L, 5);
					bool _isHero = LuaAPI.lua_toboolean(L, 6);
					bool _isVirtual = LuaAPI.lua_toboolean(L, 7);
					bool _isInstrument = LuaAPI.lua_toboolean(L, 8);
					bool _isMelee = LuaAPI.lua_toboolean(L, 9);
					int _speed = LuaAPI.xlua_tointeger(L, 10);
					int _attackRange = LuaAPI.xlua_tointeger(L, 11);
					int _radius = LuaAPI.xlua_tointeger(L, 12);
					int _extend = LuaAPI.xlua_tointeger(L, 13);
					int _greenCircleRadius = LuaAPI.xlua_tointeger(L, 14);
					int _meleeStandNum = LuaAPI.xlua_tointeger(L, 15);
					int _rangeStandNum = LuaAPI.xlua_tointeger(L, 16);
					LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 17, typeof(LuaArrAccess));
					
					var gen_ret = new M.PathFinding.UnitConfig(_worldPos, _id, _teamId, _side, _isHero, _isVirtual, _isInstrument, _isMelee, _speed, _attackRange, _radius, _extend, _greenCircleRadius, _meleeStandNum, _rangeStandNum, _luaArray);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to M.PathFinding.UnitConfig constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__worldPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._worldPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__id(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._id);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__teamId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._teamId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__side(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._side);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__isHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked._isHero);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__isVirtual(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked._isVirtual);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__isInstrument(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked._isInstrument);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__isMelee(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked._isMelee);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__speed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._speed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__attackRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._attackRange);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__radius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._radius);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__extend(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._extend);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__greenCircleRadius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._greenCircleRadius);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__meleeStandNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._meleeStandNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__rangeStandNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked._rangeStandNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__luaDatAccess(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._luaDatAccess);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__worldPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                FixPoint.FixInt2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked._worldPos = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__id(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._id = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__teamId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._teamId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__side(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._side = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__isHero(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._isHero = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__isVirtual(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._isVirtual = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__isInstrument(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._isInstrument = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__isMelee(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._isMelee = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__speed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._speed = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__attackRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._attackRange = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__radius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._radius = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__extend(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._extend = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__greenCircleRadius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._greenCircleRadius = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__meleeStandNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._meleeStandNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__rangeStandNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._rangeStandNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__luaDatAccess(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                M.PathFinding.UnitConfig gen_to_be_invoked = (M.PathFinding.UnitConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._luaDatAccess = (LuaArrAccess)translator.GetObject(L, 2, typeof(LuaArrAccess));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
