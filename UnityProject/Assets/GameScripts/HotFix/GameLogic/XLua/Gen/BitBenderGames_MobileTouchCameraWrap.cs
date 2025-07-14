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
    public class BitBenderGamesMobileTouchCameraWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BitBenderGames.MobileTouchCamera);
			Utils.BeginObjectRegister(type, L, translator, 0, 41, 48, 44);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "reSetPosAndZoom", _m_reSetPosAndZoom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoCurveMove", _m_AutoCurveMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoLookAt", _m_AutoLookAt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoLookAtByRoomManager", _m_AutoLookAtByRoomManager);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoLookAt_CityBuilding", _m_AutoLookAt_CityBuilding);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoLookAt_Origin", _m_AutoLookAt_Origin);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAutoMove", _m_StopAutoMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AutoMoveTo", _m_AutoMoveTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LookAt", _m_LookAt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInitIntersectionPoint", _m_GetInitIntersectionPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWorldPoint", _m_GetWorldPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Start", _m_Start);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetCameraBoundaries", _m_ResetCameraBoundaries);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetZoomTilt", _m_ResetZoomTilt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFinger0PosWorld", _m_GetFinger0PosWorld);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RaycastGround", _m_RaycastGround);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIntersectionPoint", _m_GetIntersectionPoint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIntersectionPointUnsafe", _m_GetIntersectionPointUnsafe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdatePinch", _m_UpdatePinch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdatePosition", _m_UpdatePosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIsBoundaryPosition", _m_GetIsBoundaryPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClampToBoundaries", _m_GetClampToBoundaries);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ComputeCamBoundaries", _m_ComputeCamBoundaries);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetZoomInDuration", _m_SetZoomInDuration);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetZoomInDuration_Curve", _m_SetZoomInDuration_Curve);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshCameraToPos", _m_RefreshCameraToPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LateUpdate", _m_LateUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceCorrectCameraZoom", _m_ForceCorrectCameraZoom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FixedUpdate", _m_FixedUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsUIPicked", _m_IsUIPicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InputControllerOnDragStart", _m_InputControllerOnDragStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDragSceneObject", _m_OnDragSceneObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckCameraAxesErrors", _m_CheckCameraAxesErrors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawGizmosSelected", _m_OnDrawGizmosSelected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnprojectVector2", _m_UnprojectVector2);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ProjectVector3", _m_ProjectVector3);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopCameraScroll", _m_StopCameraScroll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ViewportPointToWorldPos", _m_ViewportPointToWorldPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdatePosXRange", _m_UpdatePosXRange);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "CameraAxes", _g_get_CameraAxes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsAutoScrolling", _g_get_IsAutoScrolling);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsPinching", _g_get_IsPinching);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsDragging", _g_get_IsDragging);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AfterSetCameraPosition", _g_get_AfterSetCameraPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ZoomingCallback", _g_get_ZoomingCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchinggCallback", _g_get_PinchinggCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Cam", _g_get_Cam);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CanZooming", _g_get_CanZooming);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CanDrag", _g_get_CanDrag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CanWheel", _g_get_CanWheel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomZoomSensitivity", _g_get_CustomZoomSensitivity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FactorSpeed", _g_get_FactorSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamZoom", _g_get_CamZoom);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AotoMove", _g_get_AotoMove);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamOverdragMargin", _g_get_CamOverdragMargin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AutoScrollVelocityMax", _g_get_AutoScrollVelocityMax);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamZoomMin", _g_get_CamZoomMin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamZoomMax", _g_get_CamZoomMax);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TargetZoomMin", _g_get_TargetZoomMin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TargetZoomMax", _g_get_TargetZoomMax);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamOverzoomMargin", _g_get_CamOverzoomMargin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamFollowFactor", _g_get_CamFollowFactor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AutoScrollDamp", _g_get_AutoScrollDamp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AutoScrollDampCurve", _g_get_AutoScrollDampCurve);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DampFactorTimeMultiplier", _g_get_DampFactorTimeMultiplier);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GroundLevelOffset", _g_get_GroundLevelOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BoundaryMin", _g_get_BoundaryMin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BoundaryMax", _g_get_BoundaryMax);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PerspectiveZoomMode", _g_get_PerspectiveZoomMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableRotation", _g_get_EnableRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableTilt", _g_get_EnableTilt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TiltAngleMin", _g_get_TiltAngleMin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TiltAngleMax", _g_get_TiltAngleMax);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableZoomTilt", _g_get_EnableZoomTilt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ZoomTiltAngleMin", _g_get_ZoomTiltAngleMin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ZoomTiltAngleMax", _g_get_ZoomTiltAngleMax);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RefPlane", _g_get_RefPlane);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsSmoothingEnabled", _g_get_IsSmoothingEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TerrainCollider", _g_get_TerrainCollider);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InitCamPos", _g_get_InitCamPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CanMoveing", _g_get_CanMoveing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "groundRotX", _g_get_groundRotX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maxGoundHeight", _g_get_maxGoundHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pinchEnabled", _g_get_pinchEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "endPos", _g_get_endPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamPosMin", _g_get_CamPosMin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CamPosMax", _g_get_CamPosMax);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "CameraAxes", _s_set_CameraAxes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsForceGuide", _s_set_IsForceGuide);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BeforeUpdate", _s_set_BeforeUpdate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AfterSetCameraPosition", _s_set_AfterSetCameraPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ZoomingCallback", _s_set_ZoomingCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PinchinggCallback", _s_set_PinchinggCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CanZooming", _s_set_CanZooming);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CanDrag", _s_set_CanDrag);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CanWheel", _s_set_CanWheel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FactorSpeed", _s_set_FactorSpeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamZoom", _s_set_CamZoom);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AotoMove", _s_set_AotoMove);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamOverdragMargin", _s_set_CamOverdragMargin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AutoScrollVelocityMax", _s_set_AutoScrollVelocityMax);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamZoomMin", _s_set_CamZoomMin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamZoomMax", _s_set_CamZoomMax);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TargetZoomMin", _s_set_TargetZoomMin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TargetZoomMax", _s_set_TargetZoomMax);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamOverzoomMargin", _s_set_CamOverzoomMargin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamFollowFactor", _s_set_CamFollowFactor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AutoScrollDamp", _s_set_AutoScrollDamp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AutoScrollDampCurve", _s_set_AutoScrollDampCurve);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GroundLevelOffset", _s_set_GroundLevelOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BoundaryMin", _s_set_BoundaryMin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BoundaryMax", _s_set_BoundaryMax);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PerspectiveZoomMode", _s_set_PerspectiveZoomMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableRotation", _s_set_EnableRotation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableTilt", _s_set_EnableTilt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TiltAngleMin", _s_set_TiltAngleMin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TiltAngleMax", _s_set_TiltAngleMax);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableZoomTilt", _s_set_EnableZoomTilt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ZoomTiltAngleMin", _s_set_ZoomTiltAngleMin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ZoomTiltAngleMax", _s_set_ZoomTiltAngleMax);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsSmoothingEnabled", _s_set_IsSmoothingEnabled);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TerrainCollider", _s_set_TerrainCollider);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InitCamPos", _s_set_InitCamPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsOpenUpGroundDrag", _s_set_IsOpenUpGroundDrag);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CanMoveing", _s_set_CanMoveing);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "groundRotX", _s_set_groundRotX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maxGoundHeight", _s_set_maxGoundHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pinchEnabled", _s_set_pinchEnabled);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "endPos", _s_set_endPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamPosMin", _s_set_CamPosMin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CamPosMax", _s_set_CamPosMax);
            
			
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
					
					var gen_ret = new BitBenderGames.MobileTouchCamera();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_reSetPosAndZoom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.reSetPosAndZoom(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoCurveMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<System.Action>(L, 7)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _pos1Dis;translator.Get(L, 5, out _pos1Dis);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 7);
                    
                    gen_to_be_invoked.AutoCurveMove( _target, _zoom, __time, _pos1Dis, _rotY, _handle );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _pos1Dis;translator.Get(L, 5, out _pos1Dis);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.AutoCurveMove( _target, _zoom, __time, _pos1Dis, _rotY );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _pos1Dis;translator.Get(L, 5, out _pos1Dis);
                    
                    gen_to_be_invoked.AutoCurveMove( _target, _zoom, __time, _pos1Dis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.AutoCurveMove!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoLookAt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _hasLimit = LuaAPI.lua_toboolean(L, 7);
                    bool _isLockHor = LuaAPI.lua_toboolean(L, 8);
                    
                    gen_to_be_invoked.AutoLookAt( _target, _zoom, __time, _handle, _rotY, _hasLimit, _isLockHor );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _hasLimit = LuaAPI.lua_toboolean(L, 7);
                    
                    gen_to_be_invoked.AutoLookAt( _target, _zoom, __time, _handle, _rotY, _hasLimit );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.AutoLookAt( _target, _zoom, __time, _handle, _rotY );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.AutoLookAt( _target, _zoom, __time, _handle );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.AutoLookAt( _target, _zoom, __time );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.AutoLookAt( _target, _zoom );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    
                    gen_to_be_invoked.AutoLookAt( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.AutoLookAt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoLookAtByRoomManager(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _hasLimit = LuaAPI.lua_toboolean(L, 7);
                    
                    gen_to_be_invoked.AutoLookAtByRoomManager( _target, _zoom, __time, _handle, _rotY, _hasLimit );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.AutoLookAtByRoomManager( _target, _zoom, __time, _handle, _rotY );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.AutoLookAtByRoomManager( _target, _zoom, __time, _handle );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.AutoLookAtByRoomManager( _target, _zoom, __time );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.AutoLookAtByRoomManager( _target, _zoom );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    
                    gen_to_be_invoked.AutoLookAtByRoomManager( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.AutoLookAtByRoomManager!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoLookAt_CityBuilding(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float __time = (float)LuaAPI.lua_tonumber(L, 3);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _hasLimit = LuaAPI.lua_toboolean(L, 7);
                    
                    gen_to_be_invoked.AutoLookAt_CityBuilding( _target, __time, _zoom, _handle, _rotY, _hasLimit );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float __time = (float)LuaAPI.lua_tonumber(L, 3);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    float _rotY = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.AutoLookAt_CityBuilding( _target, __time, _zoom, _handle, _rotY );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float __time = (float)LuaAPI.lua_tonumber(L, 3);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.AutoLookAt_CityBuilding( _target, __time, _zoom, _handle );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float __time = (float)LuaAPI.lua_tonumber(L, 3);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.AutoLookAt_CityBuilding( _target, __time, _zoom );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float __time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.AutoLookAt_CityBuilding( _target, __time );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    
                    gen_to_be_invoked.AutoLookAt_CityBuilding( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.AutoLookAt_CityBuilding!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoLookAt_Origin(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.AutoLookAt_Origin( _target, _zoom, __time, _handle );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.AutoLookAt_Origin( _target, _zoom, __time );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.AutoLookAt_Origin( _target, _zoom );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    
                        var gen_ret = gen_to_be_invoked.AutoLookAt_Origin( _target );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.AutoLookAt_Origin!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAutoMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopAutoMove(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AutoMoveTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _handle = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.AutoMoveTo( _target, _zoom, __time, _handle );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    float __time = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.AutoMoveTo( _target, _zoom, __time );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.AutoMoveTo( _target, _zoom );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    
                    gen_to_be_invoked.AutoMoveTo( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.AutoMoveTo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookAt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    
                        var gen_ret = gen_to_be_invoked.LookAt( _target );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInitIntersectionPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetInitIntersectionPoint(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWorldPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetWorldPoint(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Start(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Start(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDestroy(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetCameraBoundaries(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetCameraBoundaries(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetZoomTilt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetZoomTilt(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFinger0PosWorld(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetFinger0PosWorld(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RaycastGround(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Ray _ray;translator.Get(L, 2, out _ray);
                    UnityEngine.Vector3 _hitPoint;
                    
                        var gen_ret = gen_to_be_invoked.RaycastGround( _ray, out _hitPoint );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.PushUnityEngineVector3(L, _hitPoint);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIntersectionPoint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Ray _ray;translator.Get(L, 2, out _ray);
                    
                        var gen_ret = gen_to_be_invoked.GetIntersectionPoint( _ray );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIntersectionPointUnsafe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Ray _ray;translator.Get(L, 2, out _ray);
                    
                        var gen_ret = gen_to_be_invoked.GetIntersectionPointUnsafe( _ray );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdatePinch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.UpdatePinch( _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdatePosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.UpdatePosition( _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIsBoundaryPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _testPosition;translator.Get(L, 2, out _testPosition);
                    
                        var gen_ret = gen_to_be_invoked.GetIsBoundaryPosition( _testPosition );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClampToBoundaries(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _newPosition;translator.Get(L, 2, out _newPosition);
                    bool _includeSpringBackMargin = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetClampToBoundaries( _newPosition, _includeSpringBackMargin );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _newPosition;translator.Get(L, 2, out _newPosition);
                    
                        var gen_ret = gen_to_be_invoked.GetClampToBoundaries( _newPosition );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.GetClampToBoundaries!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComputeCamBoundaries(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    UnityEngine.Vector2 __CamPosMin;translator.Get(L, 3, out __CamPosMin);
                    UnityEngine.Vector2 __CamPosMax;translator.Get(L, 4, out __CamPosMax);
                    
                    gen_to_be_invoked.ComputeCamBoundaries( _pos, ref __CamPosMin, ref __CamPosMax );
                    translator.PushUnityEngineVector2(L, __CamPosMin);
                        translator.UpdateUnityEngineVector2(L, 3, __CamPosMin);
                        
                    translator.PushUnityEngineVector2(L, __CamPosMax);
                        translator.UpdateUnityEngineVector2(L, 4, __CamPosMax);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetZoomInDuration(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 2);
                    float _dt = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.SetZoomInDuration( _zoom, _dt, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 2);
                    float _dt = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetZoomInDuration( _zoom, _dt );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.SetZoomInDuration!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetZoomInDuration_Curve(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<DG.Tweening.Ease>(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 2);
                    float _dt = (float)LuaAPI.lua_tonumber(L, 3);
                    DG.Tweening.Ease _easeType;translator.Get(L, 4, out _easeType);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.SetZoomInDuration_Curve( _zoom, _dt, _easeType, _callback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<DG.Tweening.Ease>(L, 4)) 
                {
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 2);
                    float _dt = (float)LuaAPI.lua_tonumber(L, 3);
                    DG.Tweening.Ease _easeType;translator.Get(L, 4, out _easeType);
                    
                        var gen_ret = gen_to_be_invoked.SetZoomInDuration_Curve( _zoom, _dt, _easeType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.SetZoomInDuration_Curve!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshCameraToPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _target;translator.Get(L, 2, out _target);
                    float _zoom = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.RefreshCameraToPos( _target, _zoom );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LateUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LateUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceCorrectCameraZoom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ForceCorrectCameraZoom(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FixedUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.FixedUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUIPicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsUIPicked(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InputControllerOnDragStart(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _dragPosStart;translator.Get(L, 2, out _dragPosStart);
                    bool _isLongTap = LuaAPI.lua_toboolean(L, 3);
                    UnityEngine.Vector3 _offset;translator.Get(L, 4, out _offset);
                    
                    gen_to_be_invoked.InputControllerOnDragStart( _dragPosStart, _isLongTap, _offset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDragSceneObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDragSceneObject(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckCameraAxesErrors(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.CheckCameraAxesErrors(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawGizmosSelected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDrawGizmosSelected(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnprojectVector2(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector2 _v2;translator.Get(L, 2, out _v2);
                    float _offset = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.UnprojectVector2( _v2, _offset );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector2>(L, 2)) 
                {
                    UnityEngine.Vector2 _v2;translator.Get(L, 2, out _v2);
                    
                        var gen_ret = gen_to_be_invoked.UnprojectVector2( _v2 );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.UnprojectVector2!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ProjectVector3(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _v3;translator.Get(L, 2, out _v3);
                    
                        var gen_ret = gen_to_be_invoked.ProjectVector3( _v3 );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopCameraScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopCameraScroll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ViewportPointToWorldPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _vec;translator.Get(L, 2, out _vec);
                    
                        var gen_ret = gen_to_be_invoked.ViewportPointToWorldPos( _vec );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdatePosXRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _min = (float)LuaAPI.lua_tonumber(L, 2);
                    float _max = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.UpdatePosXRange( _min, _max );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraAxes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CameraAxes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsAutoScrolling(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsAutoScrolling);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsPinching(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsPinching);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsDragging(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsDragging);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AfterSetCameraPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AfterSetCameraPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ZoomingCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ZoomingCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchinggCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PinchinggCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Cam(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Cam);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CanZooming(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CanZooming);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CanDrag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CanDrag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CanWheel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CanWheel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomZoomSensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CustomZoomSensitivity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FactorSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FactorSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamZoom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CamZoom);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AotoMove(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AotoMove);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamOverdragMargin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CamOverdragMargin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AutoScrollVelocityMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AutoScrollVelocityMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamZoomMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CamZoomMin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamZoomMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CamZoomMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TargetZoomMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.TargetZoomMin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TargetZoomMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.TargetZoomMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamOverzoomMargin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CamOverzoomMargin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamFollowFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CamFollowFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AutoScrollDamp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AutoScrollDamp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AutoScrollDampCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AutoScrollDampCurve);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DampFactorTimeMultiplier(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DampFactorTimeMultiplier);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GroundLevelOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.GroundLevelOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BoundaryMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.BoundaryMin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BoundaryMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.BoundaryMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PerspectiveZoomMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PerspectiveZoomMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableTilt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableTilt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TiltAngleMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.TiltAngleMin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TiltAngleMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.TiltAngleMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableZoomTilt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableZoomTilt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ZoomTiltAngleMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ZoomTiltAngleMin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ZoomTiltAngleMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ZoomTiltAngleMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RefPlane(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RefPlane);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsSmoothingEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsSmoothingEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TerrainCollider(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TerrainCollider);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InitCamPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.InitCamPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CanMoveing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CanMoveing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_groundRotX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.groundRotX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxGoundHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.maxGoundHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pinchEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.pinchEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_endPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.endPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamPosMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.CamPosMin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CamPosMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.CamPosMax);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraAxes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                BitBenderGames.CameraPlaneAxes gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CameraAxes = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsForceGuide(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsForceGuide = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BeforeUpdate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BeforeUpdate = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AfterSetCameraPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AfterSetCameraPosition = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ZoomingCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ZoomingCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PinchinggCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PinchinggCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CanZooming(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CanZooming = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CanDrag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CanDrag = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CanWheel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CanWheel = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FactorSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FactorSpeed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamZoom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CamZoom = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AotoMove(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AotoMove = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamOverdragMargin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CamOverdragMargin = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AutoScrollVelocityMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AutoScrollVelocityMax = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamZoomMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CamZoomMin = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamZoomMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CamZoomMax = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TargetZoomMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TargetZoomMin = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TargetZoomMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TargetZoomMax = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamOverzoomMargin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CamOverzoomMargin = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamFollowFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CamFollowFactor = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AutoScrollDamp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AutoScrollDamp = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AutoScrollDampCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AutoScrollDampCurve = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GroundLevelOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GroundLevelOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BoundaryMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.BoundaryMin = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BoundaryMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.BoundaryMax = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PerspectiveZoomMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                BitBenderGames.PerspectiveZoomMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.PerspectiveZoomMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableRotation = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableTilt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableTilt = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TiltAngleMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TiltAngleMin = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TiltAngleMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TiltAngleMax = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableZoomTilt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableZoomTilt = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ZoomTiltAngleMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ZoomTiltAngleMin = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ZoomTiltAngleMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ZoomTiltAngleMax = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsSmoothingEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsSmoothingEnabled = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TerrainCollider(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TerrainCollider = (UnityEngine.TerrainCollider)translator.GetObject(L, 2, typeof(UnityEngine.TerrainCollider));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InitCamPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.InitCamPos = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsOpenUpGroundDrag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsOpenUpGroundDrag = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CanMoveing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CanMoveing = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_groundRotX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.groundRotX = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxGoundHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maxGoundHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pinchEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pinchEnabled = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_endPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.endPos = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamPosMin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CamPosMin = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CamPosMax(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BitBenderGames.MobileTouchCamera gen_to_be_invoked = (BitBenderGames.MobileTouchCamera)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CamPosMax = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
