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
    public class EmbattleHexagonGridWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(EmbattleHexagonGrid);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 7, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitializeHexagon", _m_InitializeHexagon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowGridRender", _m_ShowGridRender);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeGridColor", _m_ChangeGridColor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeGridColorOnly", _m_ChangeGridColorOnly);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HexagonMeshTriangulate", _m_HexagonMeshTriangulate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Fade", _m_Fade);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "OffsetX", _g_get_OffsetX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OffsetY", _g_get_OffsetY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rowNum", _g_get_rowNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "colNum", _g_get_colNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gridSize", _g_get_gridSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pointyTop", _g_get_pointyTop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "showTest", _g_get_showTest);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "rowNum", _s_set_rowNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "colNum", _s_set_colNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gridSize", _s_set_gridSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pointyTop", _s_set_pointyTop);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "showTest", _s_set_showTest);
            
			
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
					
					var gen_ret = new EmbattleHexagonGrid();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to EmbattleHexagonGrid constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitializeHexagon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<EmbattleCellColorType>(L, 7)&& translator.Assignable<System.Collections.Generic.Dictionary<int, int>>(L, 8)) 
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 2);
                    int _width = LuaAPI.xlua_tointeger(L, 3);
                    int _height = LuaAPI.xlua_tointeger(L, 4);
                    float _cellSize = (float)LuaAPI.lua_tonumber(L, 5);
                    int _hexagonTexGridNum = LuaAPI.xlua_tointeger(L, 6);
                    EmbattleCellColorType _defaultColorType;translator.Get(L, 7, out _defaultColorType);
                    System.Collections.Generic.Dictionary<int, int> _hidDic = (System.Collections.Generic.Dictionary<int, int>)translator.GetObject(L, 8, typeof(System.Collections.Generic.Dictionary<int, int>));
                    
                    gen_to_be_invoked.InitializeHexagon( _pointyTop, _width, _height, _cellSize, _hexagonTexGridNum, _defaultColorType, _hidDic );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<EmbattleCellColorType>(L, 7)) 
                {
                    bool _pointyTop = LuaAPI.lua_toboolean(L, 2);
                    int _width = LuaAPI.xlua_tointeger(L, 3);
                    int _height = LuaAPI.xlua_tointeger(L, 4);
                    float _cellSize = (float)LuaAPI.lua_tonumber(L, 5);
                    int _hexagonTexGridNum = LuaAPI.xlua_tointeger(L, 6);
                    EmbattleCellColorType _defaultColorType;translator.Get(L, 7, out _defaultColorType);
                    
                    gen_to_be_invoked.InitializeHexagon( _pointyTop, _width, _height, _cellSize, _hexagonTexGridNum, _defaultColorType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EmbattleHexagonGrid.InitializeHexagon!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowGridRender(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _show = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.ShowGridRender( _show );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeGridColor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    EmbattleCellColorType _cellColorType;translator.Get(L, 4, out _cellColorType);
                    
                    gen_to_be_invoked.ChangeGridColor( _x, _y, _cellColorType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeGridColorOnly(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    EmbattleCellColorType _cellColorType;translator.Get(L, 4, out _cellColorType);
                    
                    gen_to_be_invoked.ChangeGridColorOnly( _x, _y, _cellColorType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HexagonMeshTriangulate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.HexagonMeshTriangulate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Fade(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.Fade( _time, _endValue );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Fade( _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EmbattleHexagonGrid.Fade!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OffsetX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.OffsetX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OffsetY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.OffsetY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rowNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.rowNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_colNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.colNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.gridSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pointyTop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.pointyTop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_showTest(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.showTest);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rowNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.rowNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_colNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.colNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.gridSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pointyTop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pointyTop = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_showTest(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EmbattleHexagonGrid gen_to_be_invoked = (EmbattleHexagonGrid)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.showTest = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
