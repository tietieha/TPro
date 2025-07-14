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
    public class HexagonGridUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HexagonGridUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 9, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetHexagonOuterRadius", _m_GetHexagonOuterRadius_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetHexagonInnerRadius", _m_GetHexagonInnerRadius_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HexagonXDistance", _m_HexagonXDistance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HexagonYDistance", _m_HexagonYDistance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetHexagonMeshMiddlePositionX", _m_GetHexagonMeshMiddlePositionX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetHexagonMeshMiddlePositionY", _m_GetHexagonMeshMiddlePositionY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGridMiddlePositionX", _m_GetGridMiddlePositionX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetGridMiddlePositionY", _m_GetGridMiddlePositionY_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "HexagonGridUtil does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHexagonOuterRadius_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _gridSize = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        var gen_ret = HexagonGridUtil.GetHexagonOuterRadius( _gridSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHexagonInnerRadius_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _gridSize = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        var gen_ret = HexagonGridUtil.GetHexagonInnerRadius( _gridSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HexagonXDistance_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 1);
                    float _gridSize = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        var gen_ret = HexagonGridUtil.HexagonXDistance( _pointyTop, _gridSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HexagonYDistance_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 1);
                    float _gridSize = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        var gen_ret = HexagonGridUtil.HexagonYDistance( _pointyTop, _gridSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHexagonMeshMiddlePositionX_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 1);
                    int _rowNum = LuaAPI.xlua_tointeger(L, 2);
                    int _colNum = LuaAPI.xlua_tointeger(L, 3);
                    float _gridSize = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = HexagonGridUtil.GetHexagonMeshMiddlePositionX( _pointyTop, _rowNum, _colNum, _gridSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHexagonMeshMiddlePositionY_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 1);
                    int _rowNum = LuaAPI.xlua_tointeger(L, 2);
                    int _colNum = LuaAPI.xlua_tointeger(L, 3);
                    float _gridSize = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = HexagonGridUtil.GetHexagonMeshMiddlePositionY( _pointyTop, _rowNum, _colNum, _gridSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridMiddlePositionX_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 1);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _z = LuaAPI.xlua_tointeger(L, 3);
                    float _hexagonXDis = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = HexagonGridUtil.GetGridMiddlePositionX( _pointyTop, _x, _z, _hexagonXDis );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGridMiddlePositionY_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 1);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _z = LuaAPI.xlua_tointeger(L, 3);
                    float _hexagonYDis = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = HexagonGridUtil.GetGridMiddlePositionY( _pointyTop, _x, _z, _hexagonYDis );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
