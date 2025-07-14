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
    public class CommonHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(CommonHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 39, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "luaGetParamsList", _m_luaGetParamsList_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "isTouchingUI", _m_isTouchingUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRaycastHitInfo", _m_GetRaycastHitInfo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldPosToScreenLocalPos", _m_WorldPosToScreenLocalPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CameraScreenPointToRay", _m_CameraScreenPointToRay_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToUIWorldPoint", _m_ScreenPointToUIWorldPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToWorldPointInRectangle", _m_ScreenPointToWorldPointInRectangle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToUILocalPoint", _m_ScreenPointToUILocalPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RectangleContainsScreenPoint", _m_RectangleContainsScreenPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "isAnimatorStatus", _m_isAnimatorStatus_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsHitGameObjectBySceenPoint", _m_IsHitGameObjectBySceenPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TestPlanesAABB", _m_TestPlanesAABB_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetNavMeshSamplePosition", _m_GetNavMeshSamplePosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetNavMeshLength", _m_GetNavMeshLength_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetNavMeshCalculatePathLength", _m_GetNavMeshCalculatePathLength_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavMeshCalculateSamplePath", _m_NavMeshCalculateSamplePath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NavMeshCalculatePath", _m_NavMeshCalculatePath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoMoveInt", _m_DoMoveInt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoMoveFloat", _m_DoMoveFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoMoveVector", _m_DoMoveVector_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoCanvasGroup", _m_DoCanvasGroup_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DotweenAllFadeIn", _m_DotweenAllFadeIn_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DotweenAllFadeOut", _m_DotweenAllFadeOut_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DotweenScale", _m_DotweenScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOtweenAnchorPos", _m_DOtweenAnchorPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOtweenAnchorPosLoop", _m_DOtweenAnchorPosLoop_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOTweenAnchorPosX", _m_DOTweenAnchorPosX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoRotate", _m_DoRotate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoLocalRotate", _m_DoLocalRotate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoLocalRotateLoop", _m_DoLocalRotateLoop_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "To", _m_To_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoCamField", _m_DoCamField_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTransLoacalPos", _m_DoTransLoacalPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTransPos", _m_DoTransPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DotweenRestart", _m_DotweenRestart_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Kill", _m_Kill_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMd5Hash", _m_GetMd5Hash_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldToUI", _m_WorldToUI_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new CommonHelper();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_luaGetParamsList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object[] _paras = translator.GetParams<object>(L, 1);
                    
                        var gen_ret = CommonHelper.luaGetParamsList( _paras );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isTouchingUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = CommonHelper.isTouchingUI(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRaycastHitInfo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Ray>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    string _layerMaskName = LuaAPI.lua_tostring(L, 2);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = CommonHelper.GetRaycastHitInfo( _ray, _layerMaskName, _maxDistance );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Ray>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    string _layerMaskName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = CommonHelper.GetRaycastHitInfo( _ray, _layerMaskName );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _down;translator.Get(L, 2, out _down);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = CommonHelper.GetRaycastHitInfo( _origin, _down, _layerMask, _maxDistance );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _down;translator.Get(L, 2, out _down);
                    int _layerMask = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = CommonHelper.GetRaycastHitInfo( _origin, _down, _layerMask );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.GetRaycastHitInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldPosToScreenLocalPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    UnityEngine.RectTransform _rectangle = (UnityEngine.RectTransform)translator.GetObject(L, 3, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector3 _target;translator.Get(L, 4, out _target);
                    
                        var gen_ret = CommonHelper.WorldPosToScreenLocalPos( _camera, _uiCamera, _rectangle, _target );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CameraScreenPointToRay_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Camera>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<raycastHitCall>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Vector3 _scPos;translator.Get(L, 2, out _scPos);
                    float _distance = (float)LuaAPI.lua_tonumber(L, 3);
                    raycastHitCall _call_back = translator.GetDelegate<raycastHitCall>(L, 4);
                    int _layer = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = CommonHelper.CameraScreenPointToRay( _camera, _scPos, _distance, _call_back, _layer );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Camera>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<raycastHitCall>(L, 4)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Vector3 _scPos;translator.Get(L, 2, out _scPos);
                    float _distance = (float)LuaAPI.lua_tonumber(L, 3);
                    raycastHitCall _call_back = translator.GetDelegate<raycastHitCall>(L, 4);
                    
                        var gen_ret = CommonHelper.CameraScreenPointToRay( _camera, _scPos, _distance, _call_back );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.CameraScreenPointToRay!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToUIWorldPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rt = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _screenPoint;translator.Get(L, 2, out _screenPoint);
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    UnityEngine.Vector3 _globalMousePos;
                    
                        var gen_ret = CommonHelper.ScreenPointToUIWorldPoint( _rt, _screenPoint, _uiCamera, out _globalMousePos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.PushUnityEngineVector3(L, _globalMousePos);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToWorldPointInRectangle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                        var gen_ret = CommonHelper.ScreenPointToWorldPointInRectangle( _target, _eventData );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToUILocalPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rt = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _screenPoint;translator.Get(L, 2, out _screenPoint);
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    UnityEngine.Vector2 _globalMousePos;
                    
                        var gen_ret = CommonHelper.ScreenPointToUILocalPoint( _rt, _screenPoint, _uiCamera, out _globalMousePos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.PushUnityEngineVector2(L, _globalMousePos);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RectangleContainsScreenPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Camera>(L, 3)) 
                {
                    UnityEngine.RectTransform __rt = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector3 _sc;translator.Get(L, 2, out _sc);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    
                        var gen_ret = CommonHelper.RectangleContainsScreenPoint( __rt, _sc, _camera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.RectTransform __rt = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector3 _sc;translator.Get(L, 2, out _sc);
                    
                        var gen_ret = CommonHelper.RectangleContainsScreenPoint( __rt, _sc );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.RectangleContainsScreenPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isAnimatorStatus_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Animator __animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string __status = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = CommonHelper.isAnimatorStatus( __animator, __status );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsHitGameObjectBySceenPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.GameObject _gameObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                        var gen_ret = CommonHelper.IsHitGameObjectBySceenPoint( _camera, _gameObject );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TestPlanesAABB_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Bounds _bounds;translator.Get(L, 2, out _bounds);
                    
                        var gen_ret = CommonHelper.TestPlanesAABB( _camera, _bounds );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNavMeshSamplePosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _selfPos;translator.Get(L, 1, out _selfPos);
                    UnityEngine.Vector3 _targetPos;translator.Get(L, 2, out _targetPos);
                    
                        var gen_ret = CommonHelper.GetNavMeshSamplePosition( _selfPos, _targetPos );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNavMeshLength_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.AI.NavMeshAgent _agent = (UnityEngine.AI.NavMeshAgent)translator.GetObject(L, 1, typeof(UnityEngine.AI.NavMeshAgent));
                    
                        var gen_ret = CommonHelper.GetNavMeshLength( _agent );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNavMeshCalculatePathLength_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 1, out _startPos);
                    UnityEngine.Vector3 _endPos;translator.Get(L, 2, out _endPos);
                    
                        var gen_ret = CommonHelper.GetNavMeshCalculatePathLength( _startPos, _endPos );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavMeshCalculateSamplePath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 1, out _startPos);
                    UnityEngine.Vector3 _targetPos;translator.Get(L, 2, out _targetPos);
                    
                        var gen_ret = CommonHelper.NavMeshCalculateSamplePath( _startPos, _targetPos );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NavMeshCalculatePath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 1, out _startPos);
                    UnityEngine.Vector3 _endPos;translator.Get(L, 2, out _endPos);
                    
                        var gen_ret = CommonHelper.NavMeshCalculatePath( _startPos, _endPos );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoMoveInt_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _isLoop = LuaAPI.xlua_tointeger(L, 1);
                    DG.Tweening.LoopType _loopType;translator.Get(L, 2, out _loopType);
                    int _curValue = LuaAPI.xlua_tointeger(L, 3);
                    int _targetValue = LuaAPI.xlua_tointeger(L, 4);
                    float _time = (float)LuaAPI.lua_tonumber(L, 5);
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 6);
                    DG.Tweening.Ease _ease;translator.Get(L, 7, out _ease);
                    System.Action<int> _onProgress = translator.GetDelegate<System.Action<int>>(L, 8);
                    System.Action _onComplete = translator.GetDelegate<System.Action>(L, 9);
                    
                        var gen_ret = CommonHelper.DoMoveInt( _isLoop, _loopType, _curValue, _targetValue, _time, _delayTime, _ease, _onProgress, _onComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoMoveFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _isLoop = LuaAPI.xlua_tointeger(L, 1);
                    DG.Tweening.LoopType _loopType;translator.Get(L, 2, out _loopType);
                    float _curValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _targetValue = (float)LuaAPI.lua_tonumber(L, 4);
                    float _time = (float)LuaAPI.lua_tonumber(L, 5);
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 6);
                    DG.Tweening.Ease _ease;translator.Get(L, 7, out _ease);
                    System.Action<float> _onProgress = translator.GetDelegate<System.Action<float>>(L, 8);
                    System.Action _onComplete = translator.GetDelegate<System.Action>(L, 9);
                    System.Action _onStepComplete = translator.GetDelegate<System.Action>(L, 10);
                    
                        var gen_ret = CommonHelper.DoMoveFloat( _isLoop, _loopType, _curValue, _targetValue, _time, _delayTime, _ease, _onProgress, _onComplete, _onStepComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoMoveVector_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _isLoop = LuaAPI.xlua_tointeger(L, 1);
                    DG.Tweening.LoopType _loopType;translator.Get(L, 2, out _loopType);
                    UnityEngine.Vector3 _curValue;translator.Get(L, 3, out _curValue);
                    UnityEngine.Vector3 _targetValue;translator.Get(L, 4, out _targetValue);
                    float _time = (float)LuaAPI.lua_tonumber(L, 5);
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 6);
                    DG.Tweening.Ease _ease;translator.Get(L, 7, out _ease);
                    System.Action<UnityEngine.Vector3> _onProgress = translator.GetDelegate<System.Action<UnityEngine.Vector3>>(L, 8);
                    System.Action _onComplete = translator.GetDelegate<System.Action>(L, 9);
                    
                        var gen_ret = CommonHelper.DoMoveVector( _isLoop, _loopType, _curValue, _targetValue, _time, _delayTime, _ease, _onProgress, _onComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoCanvasGroup_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DoCanvasGroup( _rec, _endValue, _endTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DotweenAllFadeIn_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    
                    CommonHelper.DotweenAllFadeIn( _rec );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DotweenAllFadeOut_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    
                    CommonHelper.DotweenAllFadeOut( _rec );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DotweenScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    float _endScale = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callBack = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DotweenScale( _rec, _endScale, _time, _callBack );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    float _endScale = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DotweenScale( _rec, _endScale, _time );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector3 _endScale;translator.Get(L, 2, out _endScale);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callBack = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DotweenScale( _rec, _endScale, _time, _callBack );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector3 _endScale;translator.Get(L, 2, out _endScale);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DotweenScale( _rec, _endScale, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DotweenScale!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOtweenAnchorPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endPos;translator.Get(L, 2, out _endPos);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callBack = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DOtweenAnchorPos( _rec, _endPos, _time, _callBack );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endPos;translator.Get(L, 2, out _endPos);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DOtweenAnchorPos( _rec, _endPos, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DOtweenAnchorPos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOtweenAnchorPosLoop_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endPos;translator.Get(L, 2, out _endPos);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    int _loopCount = LuaAPI.xlua_tointeger(L, 4);
                    System.Action _callBack = translator.GetDelegate<System.Action>(L, 5);
                    
                    CommonHelper.DOtweenAnchorPosLoop( _rec, _endPos, _time, _loopCount, _callBack );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endPos;translator.Get(L, 2, out _endPos);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    int _loopCount = LuaAPI.xlua_tointeger(L, 4);
                    
                    CommonHelper.DOtweenAnchorPosLoop( _rec, _endPos, _time, _loopCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endPos;translator.Get(L, 2, out _endPos);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DOtweenAnchorPosLoop( _rec, _endPos, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DOtweenAnchorPosLoop!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOTweenAnchorPosX_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<DG.Tweening.Ease>(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    DG.Tweening.Ease _ease;translator.Get(L, 4, out _ease);
                    System.Action _complete = translator.GetDelegate<System.Action>(L, 5);
                    
                    CommonHelper.DOTweenAnchorPosX( _rec, _x, _time, _ease, _complete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<DG.Tweening.Ease>(L, 4)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    DG.Tweening.Ease _ease;translator.Get(L, 4, out _ease);
                    
                    CommonHelper.DOTweenAnchorPosX( _rec, _x, _time, _ease );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DOTweenAnchorPosX( _rec, _x, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DOTweenAnchorPosX!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoRotate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.Transform _t = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _euler;translator.Get(L, 2, out _euler);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _onFinished = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DoRotate( _t, _euler, _duration, _onFinished );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Transform _t = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _euler;translator.Get(L, 2, out _euler);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DoRotate( _t, _euler, _duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DoRotate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoLocalRotate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.Transform _t = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _euler;translator.Get(L, 2, out _euler);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _onFinished = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DoLocalRotate( _t, _euler, _duration, _onFinished );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Transform _t = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _euler;translator.Get(L, 2, out _euler);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DoLocalRotate( _t, _euler, _duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DoLocalRotate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoLocalRotateLoop_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.RectTransform _rt = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    int _isLoop = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _euler;translator.Get(L, 3, out _euler);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _onFinished = translator.GetDelegate<System.Action>(L, 5);
                    
                    CommonHelper.DoLocalRotateLoop( _rt, _isLoop, _euler, _duration, _onFinished );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.RectTransform _rt = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    int _isLoop = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _euler;translator.Get(L, 3, out _euler);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    CommonHelper.DoLocalRotateLoop( _rt, _isLoop, _euler, _duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DoLocalRotateLoop!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<System.Action<float>>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    System.Action<float> _setter = translator.GetDelegate<System.Action<float>>(L, 1);
                    float _startValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = CommonHelper.To( _setter, _startValue, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Transform>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    float _startValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 4);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        var gen_ret = CommonHelper.To( _target, _setter, _startValue, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<System.Action<float>>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<DG.Tweening.Ease>(L, 6)) 
                {
                    System.Action<float> _action = translator.GetDelegate<System.Action<float>>(L, 1);
                    float _startValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    int _loopTime = LuaAPI.xlua_tointeger(L, 5);
                    DG.Tweening.Ease _easeType;translator.Get(L, 6, out _easeType);
                    
                        var gen_ret = CommonHelper.To( _action, _startValue, _endValue, _duration, _loopTime, _easeType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<System.Action<float>>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    System.Action<float> _action = translator.GetDelegate<System.Action<float>>(L, 1);
                    float _startValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    int _loopTime = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = CommonHelper.To( _action, _startValue, _endValue, _duration, _loopTime );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<System.Action<float>>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    System.Action<float> _action = translator.GetDelegate<System.Action<float>>(L, 1);
                    float _startValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = CommonHelper.To( _action, _startValue, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.To!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoCamField_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Camera>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    float _end = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DoCamField( _camera, _end, _time, _action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Camera>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    float _end = (float)LuaAPI.lua_tonumber(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DoCamField( _camera, _end, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DoCamField!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTransLoacalPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DoTransLoacalPos( _transform, _end, _time, _action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DoTransLoacalPos( _transform, _end, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DoTransLoacalPos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTransPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)&& translator.Assignable<DG.Tweening.Ease>(L, 5)) 
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    DG.Tweening.Ease _ease;translator.Get(L, 5, out _ease);
                    
                    CommonHelper.DoTransPos( _transform, _end, _time, _action, _ease );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    
                    CommonHelper.DoTransPos( _transform, _end, _time, _action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    CommonHelper.DoTransPos( _transform, _end, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CommonHelper.DoTransPos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DotweenRestart_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rec = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    
                    CommonHelper.DotweenRestart( _rec );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Kill_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _tweener = translator.GetObject(L, 1, typeof(object));
                    
                        var gen_ret = CommonHelper.Kill( _tweener );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMd5Hash_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _input = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = CommonHelper.GetMd5Hash( _input );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldToUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 1);
                    float _y = (float)LuaAPI.lua_tonumber(L, 2);
                    float _z = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Camera _sceneCamera = (UnityEngine.Camera)translator.GetObject(L, 4, typeof(UnityEngine.Camera));
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 5, typeof(UnityEngine.Camera));
                    UnityEngine.RectTransform _rectTransform = (UnityEngine.RectTransform)translator.GetObject(L, 6, typeof(UnityEngine.RectTransform));
                    
                        var gen_ret = CommonHelper.WorldToUI( _x, _y, _z, _sceneCamera, _uiCamera, _rectTransform );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
