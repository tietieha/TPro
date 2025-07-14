using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Image = UnityEngine.UI.Image;
using Spine.Unity;
using TEngine;
using UnityEngine.Networking;

/// <summary>
/// Unity 扩展。
/// </summary>
public static class UnityExtension
{

    public static void TryAddElement<T>(this Dictionary<long, List<T>> dic, long key, T t)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(t);
        }
        else
        {
            dic[key] = new List<T>();
            dic[key].Add(t);
        }
    }



    public static GameObject Instantiate(this GameObject go)
    {
        if (go != null)
        {
            return GameObject.Instantiate(go);
        }
        return go;
    }

    public static void Destroy(this GameObject go)
    {
        if (go != null)
        {
            GameObject.Destroy(go);
        }
    }
    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件。</typeparam>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>获取或增加的组件。</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <param name="type">要获取或增加的组件类型。</param>
    /// <returns>获取或增加的组件。</returns>
    public static Component GetOrAddComponent(this GameObject gameObject, Type type)
    {
        Component component = gameObject.GetComponent(type);
        if (component == null)
        {
            component = gameObject.AddComponent(type);
        }

        return component;
    }

    /// <summary>
    /// 获取 GameObject 是否在场景中。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>GameObject 是否在场景中。</returns>
    /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
    public static bool InScene(this GameObject gameObject)
    {
        return gameObject.scene.name != null;
    }

    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
    /// <param name="layer">目标层次的编号。</param>
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector3" /> 的 (x, y, z) 转换为 <see cref="UnityEngine.Vector2" /> 的 (x, z)。
    /// </summary>
    /// <param name="vector3">要转换的 Vector3。</param>
    /// <returns>转换后的 Vector2。</returns>
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 0, y)。
    /// </summary>
    /// <param name="vector2">要转换的 Vector2。</param>
    /// <returns>转换后的 Vector3。</returns>
    public static Vector3 ToVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, 0f, vector2.y);
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 和给定参数 y 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 参数 y, y)。
    /// </summary>
    /// <param name="vector2">要转换的 Vector2。</param>
    /// <param name="y">Vector3 的 y 值。</param>
    /// <returns>转换后的 Vector3。</returns>
    public static Vector3 ToVector3(this Vector2 vector2, float y)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    public static Vector2Int Clone(this Vector2Int vector2Int)
    {
        return new Vector2Int(vector2Int.x, vector2Int.y);
    }

    #region Transform

    /// <summary>
    /// 设置绝对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 坐标值。</param>
    public static void SetPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.x = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 设置绝对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 坐标值。</param>
    public static void SetPositionY(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.y = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 设置绝对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 坐标值。</param>
    public static void SetPositionZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.z = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加绝对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">x 坐标值增量。</param>
    public static void AddPositionX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.x += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加绝对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">y 坐标值增量。</param>
    public static void AddPositionY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.y += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加绝对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">z 坐标值增量。</param>
    public static void AddPositionZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.z += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 设置相对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 坐标值。</param>
    public static void SetLocalPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.x = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 设置相对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 坐标值。</param>
    public static void SetLocalPositionY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.y = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 设置相对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 坐标值。</param>
    public static void SetLocalPositionZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.z = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 增加相对位置的 x 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">x 坐标值。</param>
    public static void AddLocalPositionX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.x += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 增加相对位置的 y 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">y 坐标值。</param>
    public static void AddLocalPositionY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.y += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 增加相对位置的 z 坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">z 坐标值。</param>
    public static void AddLocalPositionZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.z += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 设置相对尺寸的 x 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 分量值。</param>
    public static void SetLocalScaleX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.x = newValue;
        transform.localScale = v;
    }

    public static void SetLocalScaleEx(this Transform transform, float x,float y,float z)
    {
        Vector3 v = transform.localScale;
        v.x = x;
        v.y = y;
        v.z = z;
        transform.localScale = v;
    }
    public static void SetLocalScaleEx(this GameObject go, float x,float y,float z)
    {
        Vector3 v = go.transform.localScale;
        v.x = x;
        v.y = y;
        v.z = z;
        go.transform.localScale = v;
    }
    /// <summary>
    /// 设置相对尺寸的 y 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 分量值。</param>
    public static void SetLocalScaleY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.y = newValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 设置相对尺寸的 z 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 分量值。</param>
    public static void SetLocalScaleZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.z = newValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 增加相对尺寸的 x 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">x 分量增量。</param>
    public static void AddLocalScaleX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localScale;
        v.x += deltaValue;
        transform.localScale = v;
    }

    /// <summary>
    /// 增加相对尺寸的 y 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">y 分量增量。</param>
    public static void AddLocalScaleY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localScale;
        v.y += deltaValue;
        transform.localScale = v;
    }
    public static void SetLocalPositionEx(this GameObject go, float x,float y,float z)
    {
        var v = go.transform.localPosition;
        v.x = x;
        v.y = y;
        v.z = z;
        go.transform.localPosition = v;
    }
    public static void SetLocalPositionEx(this Transform go, float x,float y,float z)
    {
        var v = go.transform.localPosition;
        v.x = x;
        v.y = y;
        v.z = z;
        go.localPosition = v;
    }
    public static void SetPositionEx(this GameObject go, float x,float y,float z)
    {
        var v = go.transform.position;
        v.x = x;
        v.y = y;
        v.z = z;
        go.transform.position = v;
    }
    public static void SetPositionEx(this Transform go, float x,float y,float z)
    {
        var v = go.position;
        v.x = x;
        v.y = y;
        v.z = z;
        go.position = v;
    }

    public static void TransformDirectionLua(this Transform go, float inputX, float inputY, out float dirX, out float dirY, out float dirZ)
    {
        Vector3 input3D = new Vector3(inputX, 0, inputY);
        Vector3 worldSpaceDir = go.TransformDirection(input3D);
        Vector3 realMoveDir = new Vector3(worldSpaceDir.x, 0, worldSpaceDir.z);
        realMoveDir.Normalize();
        dirX = realMoveDir.x;
        dirY = realMoveDir.y;
        dirZ = realMoveDir.z;
    }
    
    public static void SetAnchoredPositionEx(this RectTransform go, float x,float y)
    {
        var v = go.anchoredPosition;
        v.x = x;
        v.y = y;
        go.anchoredPosition = v;
    }
    public static void SetSizeDeltaEx(this RectTransform go, float x,float y)
    {
        var v = go.sizeDelta;
        v.x = x;
        v.y = y;
        go.sizeDelta = v;
    }
    public static void SetRotationEx(this GameObject go, float x,float y,float z)
    {
        var v = go.transform.eulerAngles;
        v.x = x;
        v.y = y;
        v.z = z;
        go.transform.eulerAngles = v;
    }
    
   
    public static void SetRotationEx(this Transform go, float x,float y,float z)
    {
        var v = go.eulerAngles;
        v.x = x;
        v.y = y;
        v.z = z;
        go.eulerAngles = v;
    }
    
    public static void SetRotationQEx(this GameObject go, float x,float y,float z,float w)
    {
        var v = go.transform.rotation;
        v.Set(x,y,z,w);
        go.transform.rotation = v;
    }
    
    public static void SetRotationQEx(this Transform go, float x,float y,float z,float w)
    {
        var v = go.transform.rotation;
        v.Set(x,y,z,w);
        go.transform.rotation = v;
    }
    public static void SetLocalRotationEx(this GameObject go, float x,float y,float z)
    {
        var v = go.transform.localEulerAngles;
        v.x = x;
        v.y = y;
        v.z = z;
        go.transform.localEulerAngles = v;
    }

    public static void SetLocalRotationQEx(this GameObject go, float x,float y,float z,float w)
    {
        var v = go.transform.localRotation;
        v.Set(x,y,z,w);
        go.transform.localRotation = v;
    }

    public static void SetLocalRotationQEx(this Transform go, float x,float y,float z,float w)
    {
        var v = go.transform.localRotation;
        v.Set(x,y,z,w);
        go.transform.localRotation = v;
    }

    public static void SetLocalRotationYEx(this GameObject go, float y)
    {
        var v = go.transform.localEulerAngles;
        v.y = y;
        go.transform.localEulerAngles = v;
    }
    public static void SetLocalRotationXEx(this GameObject go, float x)
    {
        var v = go.transform.localEulerAngles;
        v.x = x;
        go.transform.localEulerAngles = v;
    }
    public static void SetLocalRotationZEx(this GameObject go, float z)
    {
        var v = go.transform.localEulerAngles;
        v.z = z;
        go.transform.localEulerAngles = v;
    }
    public static void SetLocalRotationEx(this Transform go, float x,float y,float z)
    {
        var v = go.localEulerAngles;
        v.x = x;
        v.y = y;
        v.z = z;
        go.localEulerAngles = v;
    }
    public static void SetLocalRotationYEx(this Transform go, float y)
    {
        var v = go.localEulerAngles;
        v.y = y;
        go.localEulerAngles = v;
    }
    public static void SetLocalRotationXEx(this Transform go, float x)
    {
        var v = go.localEulerAngles;
        v.x = x;
        go.localEulerAngles = v;
    }
    public static void SetLocalRotationZEx(this Transform go, float z)
    {
        var v = go.localEulerAngles;
        v.z = z;
        go.localEulerAngles = v;
    }
    /// <summary>
    /// 增加相对尺寸的 z 分量。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="deltaValue">z 分量增量。</param>
    public static void AddLocalScaleZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localScale;
        v.z += deltaValue;
        transform.localScale = v;
        TMP_InputField test;
    }

    /// <summary>
    /// 二维空间下使 <see cref="UnityEngine.Transform" /> 指向指向目标点的算法，使用世界坐标。
    /// </summary>
    /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="lookAtPoint2D">要朝向的二维坐标点。</param>
    /// <remarks>假定其 forward 向量为 <see cref="UnityEngine.Vector3.up" />。</remarks>
    public static void LookAt2D(this Transform transform, Vector2 lookAtPoint2D)
    {
        Vector3 vector = lookAtPoint2D.ToVector3() - transform.position;
        vector.y = 0f;

        if (vector.magnitude > 0f)
        {
            transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
        }
    }

    #endregion Transform
    
    //供lua检查gameObject是否valid
    public static bool IsNull(this UnityEngine.Object o)
    {
        return o == null;
    }

    public static bool IsNotNull(this UnityEngine.Object o)
    {
        return o;
    }
    /// <summary>
    /// 无需 Null check 的情况下不要调用该接口 GameObject == null 有额外的性能消耗
    /// 扩展SetActive, 提升性能
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="active"></param>
    public static void TrySetActive(this GameObject obj, bool active)
    {
        if (active)
        {
            if (obj != null && !obj.activeSelf)
                obj.SetActive(true);
        }
        else
        {
            if (obj != null && obj.activeSelf)
                obj.SetActive(false);
        }
    }
    public static void SetActiveEx(this GameObject obj, bool active)
    {
        if (obj && obj.activeSelf != active)
        {
            obj.SetActive(active);
        }
    }
    public delegate void LoadSpriteCallback(object spriteTarget, int result);
    /// <summary>
    /// Image组件异步加载图片接口;
    /// </summary>
    /// <param name="spriteName">图片名称</param>
    /// <param name="defaultSpriteName">默认图片名称</param>
    /// <param name="loadCallback">图片加载回调</param>
    /// <param name="isNeedNativeSize">是否设置原大小尺寸</param>
    /// <param name="loadingEnable">loading过程中是否可用</param>
    public static void LoadAsync(this Image image, string spriteName)
    {
        var sprite= Resources.Load<Sprite>($"Sprites/UI/{spriteName}");
        if (image!=null && sprite!=null)
        {
            image.sprite = sprite;
        }
    }


    public static void SetFov(this CinemachineVirtualCamera cv,float fov)
    {
        if (cv != null)
        {
            cv.m_Lens.FieldOfView = fov;
        }
    }

    public static void PlayReset(this Animation animation,string clipName,bool isReset)
    {
        if (animation[clipName] != null)
        {
            if (isReset)
            {
                animation.Rewind(clipName);
                animation[clipName].speed = 1f;
            }
            animation.Play(clipName);
        }
    }
    public static void Reset(this Animation animation, string clipName)
    {
        AnimationState state = animation[clipName];
        animation.Play(clipName);
        state.time = 0;
        animation.Sample();  
        state.enabled = false;
    }
    public static void SimpleLastFrame(this Animation animation, string clipName)
    {
        AnimationState state = animation[clipName];
        AnimationClip clip = state.clip;
        clip.SampleAnimation(animation.gameObject, state.length);
    }

    public static void SimpleFirstFrame(this Animation animation, string clipName)
    {
        AnimationState state = animation[clipName];
        AnimationClip clip = state.clip;
        clip.SampleAnimation(animation.gameObject, 0);
    }

    public static void RewindPlay(this Animation animation, string clipName)
    {
        if (animation[clipName] != null)
        {
            animation[clipName].time = animation[clipName].clip.length;
            animation[clipName].speed = -1f;
            animation.Play(clipName);
        }
    }
    public static void Play(this Animation animation, string clipName,float progress)
    {
        if (animation[clipName] != null)
        {
            float length = animation[clipName].length;
            float timeTemp = length * progress;
            if (length <= 0 || Mathf.Abs(animation[clipName].time - timeTemp) <= 0.01f)
            {
                return;
            }
            animation[clipName].time = timeTemp;
            animation[clipName].speed = 1f;
            animation.Play(clipName);
        }
    }
    public static float GetAnimationLengthByName(this Animation animation, string clipName)
    {
        if (animation[clipName] != null)
        {
            return animation[clipName].length;
        }
        return 0;
    }

    public static float GetSkeletonAnimationLengthByName(this SkeletonAnimation skeletonAnimation, string animationName)
    {
        Spine.Animation anim = skeletonAnimation.AnimationState.Data.SkeletonData.FindAnimation(animationName);
        if (anim != null)
        {
            return anim.Duration;
        }
        return 0;
    }

    public static void SetParticleColor(this ParticleSystem particleSystem, Color color)
    {
        if(particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.startColor = color;
        }
    }
    public static void SetParentEx(this GameObject go, GameObject parent)
    {
        if (parent!=null)
        {
            go.transform.SetParent(parent.transform,false);
        }
    }
    public static void SetParentEx(this GameObject go, Transform parent)
    {
        if (parent != null)
        {
            go.transform.SetParent(parent,false);
        }
    }
    public static void SetParentEx(this GameObject go, RectTransform parent)
    {
        if (parent != null)
        {
            go.transform.SetParent(parent,false);
        }
    }
    public static void SetParentEx(this Transform go, GameObject parent)
    {
        if (parent!=null)
        {
            go.SetParent(parent.transform,false);
        }
    }
    public static void SetParentEx(this Transform go, Transform parent)
    {
        if (parent != null)
        {
            go.SetParent(parent,false);
        }
    }
    public static void SetParentEx(this Transform go, RectTransform parent)
    {
        if (parent != null)
        {
            go.SetParent(parent,false);
        }
    }
    
    public static GameObject AddChild (this GameObject parent, GameObject prefab, int layer = -1)
    {
	    var go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
	    if (!Application.isPlaying)
		    UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
	    if (go != null)
	    {
		    go.name = prefab.name;

		    if (parent != null)
		    {
			    Transform t = go.transform;
			    t.parent = parent.transform;
			    t.localPosition = Vector3.zero;
			    t.localRotation = Quaternion.identity;
			    t.localScale = Vector3.one;
			    if (layer == -1) go.layer = parent.layer;
			    else if (layer > -1 && layer < 32) go.layer = layer;
		    }
		    go.SetActive(true);
	    }
	    return go;
    }

    public static string GetFullHierarchyPath(this Transform transform)
    {
	    if (transform.parent != null)
	    {
		    return transform.parent.GetFullHierarchyPath() + "/" + transform.name;
	    }
	    else
	    {
		    return transform.name;
	    }
    }

    public static bool TrySetGenericBinding(this PlayableDirector playableDirector, string timelineName, string trackName, GameObject bindingObj)
    {
        if (playableDirector.playableAsset.name != timelineName)
        {
            return false;
        }
        var bindings = playableDirector.playableAsset.outputs;
        foreach (var bind in bindings)
        {
            if (bind.streamName == trackName)
            {
                playableDirector.SetGenericBinding(bind.sourceObject, bindingObj);
                return true;
            }
        }
        return false;
    }

    public static int GetDownloadHandlerDataByteArrayLength(this DownloadHandler handler)
    {
        return handler.data.Length;
    }
    
    public static void ResetLua(this List<ChatDataStruct>.Enumerator iet)
    {
        ((IEnumerator) iet).Reset();
    }
    public static void ResetLua2(this List<Type>.Enumerator iet)
    {
        ((IEnumerator) iet).Reset();
    }
    
    public static Vector3 TransformPointEx(this Transform transform, float x, float y, float z)
    {
        return transform.TransformPoint(new Vector3(x, y, z));
    }
    
    public static Vector3 ObjectRelativeToParentPos(this Transform transform, float x, float y, float z)
    {
        Vector3 gPos =  transform.TransformPoint(new Vector3(x, y, z));
        return gPos - transform.position;
    }
    
    #region BoxCollider
    public static void BoxColliderSizeEx(this BoxCollider boxCollider, float x, float y, float z)
    {
        boxCollider.size = new Vector3(x, y, z);
    }
    
    #endregion
    
    #region SpriteRender

    public static void SetSizeEx(this SpriteRenderer render, float width, float height)
    {
        render.size = new Vector2(width, height);
    }

    #endregion
}
