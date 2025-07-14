using DG.Tweening;
using UnityEngine;
using XLua;

namespace UW
{
    [LuaCallCSharp]
    public class DoTweenUtil
    {
        public static Tweener To_Float(float start, float to, float duration, LuaFunction setter, LuaFunction onCompleteLua)
        {
            return DOTween.To(()=> start, (value) => setter?.Call(value), to, duration).OnComplete(() => onCompleteLua?.Call());
        }

        public static Tweener To_Int(int start, int to, float duration, LuaFunction setter, LuaFunction onCompleteLua)
        {
            return DOTween.To(() => start, (value) => setter?.Call(value), to, duration).OnComplete(() => onCompleteLua?.Call());
        }
        public static Tweener To_Vector3(Vector3 start, Vector3 to, float duration, LuaFunction setter, LuaFunction onCompleteLua)
        {
            return DOTween.To(() => start, (value) => setter?.Call(value), to, duration).OnComplete(() => onCompleteLua?.Call());
        }
        
        public static Tweener To_Vector2(Vector2 start, Vector2 to, float duration, LuaFunction setter, LuaFunction onCompleteLua)
        {
            return DOTween.To(() => start, (value) => setter?.Call(value), to, duration).OnComplete(() => onCompleteLua?.Call());
        }
        
        public static Tweener To_String(string start, string to, float duration, LuaFunction setter, LuaFunction onCompleteLua)
        {
            return DOTween.To(() => start, (value) => setter?.Call(value), to, duration).OnComplete(() => onCompleteLua?.Call());
        }
        
        public static Tweener DoPath(Transform tf,Vector3 start, Vector3 to, Vector3 to2, float duration, LuaFunction setter, LuaFunction onCompleteLua)
        {
            Vector3[] path = new Vector3[3];
            path[0] = start;
            path[1] = to2;
            path[2] = to;
            var tweenPath = tf.DOPath(path, duration, PathType.CatmullRom);
            tweenPath.OnComplete(() => onCompleteLua?.Call());
            tweenPath.SetEase(Ease.InCubic);
            return tweenPath;
        }
    }
}
