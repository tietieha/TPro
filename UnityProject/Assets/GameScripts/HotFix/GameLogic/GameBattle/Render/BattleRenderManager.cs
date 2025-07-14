using System;
using XLua;
using UnityEngine;
using UnityEngine.UIElements;

namespace M.Battle
{
    [LuaCallCSharp]
    public class BattleRenderManager
    {
        public static float MagInterval = 0.01f;
        public static float PosInterval = 0.0001f;

        public static float TimeScale
        {
            get { return 0; }
            set
            {
                MagInterval = 0.01f * value;
                PosInterval = 0.0001f * value;
            }
        }

        public static void SetRotateAndoPos(Transform transform, float x, float y, float z)
        {
            Vector3 newPos = new Vector3(x, y, z);
            Vector3 diff = newPos - transform.localPosition;
            if (diff.sqrMagnitude > MagInterval)
            {
                transform.localRotation =
                    Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(diff), 0.25f);
                transform.localPosition = newPos;
            }
        }

        public static void SetRotationImmediately(Transform transform, float x, float y)
        {
            Vector3 diff = new Vector3(x - transform.localPosition.x, 0, y - transform.localPosition.z);
            if (diff == Vector3.zero)
                return;

            transform.localRotation = Quaternion.LookRotation(diff);
        }

        public static void SetRotation(Transform transform, float x, float y)
        {
            if (Math.Abs(x - transform.localPosition.x) < PosInterval &&
                Math.Abs(y - transform.localPosition.z) < PosInterval)
                return;

            Vector3 diff = new Vector3(x - transform.localPosition.x, 0, y - transform.localPosition.z);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(diff), 0.25f);

            //transform.LookAt(transform.TransformPoint(new Vector3(x, 0, y)));
        }


        public static void SetRotateAndoPos3(Transform transform, float x, float y, float z)
        {
            if (Double.IsNaN(x) || Double.IsNaN(y) || Double.IsNaN(z))
            {
            }
            else
            {
                Vector3 newPos = new Vector3(x, y, z);
                transform.LookAt(transform.parent.TransformPoint(newPos));
                transform.localPosition = newPos;
            }
        }

        public static void SetPos3(Transform transform, float x, float y, float z)
        {
            Vector3 newPos = new Vector3(x, y, z);
            transform.localPosition = newPos;
        }

        // get body's world position.
        // public static void GetBodyPos(Transform transform, Actor.Actor actor, ref float x, ref float y, ref float z)
        // {
        //
        //     Vector3 bodyPos = actor.GetMount().GetBindNode(MountNodeType.BodySocket).transform
        //         .InverseTransformPoint(transform.localPosition);
        //     x = bodyPos.x;
        //     y = bodyPos.y;
        //     z = bodyPos.z;
        // }

        public static void SetActive(GameObject obj, bool active)
        {
            obj.SetActive(active);
        }

        public static void Animator_Replay(Animator actor, string aniName)
        {
            actor.Play(aniName, -1, 0f);
        }

        public static void Animator_CrossFadeRandom(Animator actor, string aniName, float crossFadeTime,
            float normalizedTime)
        {
            // actor.CrossFadeRandom(aniName, crossFadeTime); TODO LZL
            actor.CrossFade(aniName, crossFadeTime, 0, normalizedTime);
        }

        public static void Animator_CrossFade(Animator actor, string aniName, float crossFadeTime)
        {
            actor.CrossFade(aniName, crossFadeTime);
        }

        public static void Animator_Speed(Animator actor, float speed)
        {
            actor.speed = speed;
        }

        public static void Animator_Play(Animator actor, string aniName)
        {
            actor.Play(aniName);
        }

        /// <summary>
        /// 获取动画播放了多久了，返回值是几轮 比如 1.4代表 完整播放了一次，且当前进度是40%
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static double Animator_GetCurrentStatePlayTime(Animator actor)
        {
            return actor.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public static double Animator_GetCurrentStateTime(Animator actor)
        {
            return actor.GetCurrentAnimatorStateInfo(0).length;
        }

        /// <summary>
        /// 动画从什么时间开始播放
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="aniName"></param>
        /// <param name="startTime"></param> 归一化的值 0-1之间
        public static void Animator_PlayStartTime(Animator actor, string aniName, float startTime)
        {
            actor.Play(aniName, 0, startTime);
        }

        public static bool Get_Transf_PosXYZ(Transform xf, out float x, out float y, out float z)
        {
            if (xf)
            {
                var pos = xf.position;
                x = pos.x;
                y = pos.y;
                z = pos.z;
                return true;
            }

            x = y = z = 0f;
            return false;
        }

        public static void GetReversePos(Transform transform, float speed, float t, out float x, out float z)
        {
            var len = speed * t;
            Vector3 forward = transform.forward;
            Vector3 back = new Vector3(-forward.x, 0, -forward.z);
            back.Normalize();
            x = len * back.x;
            z = len * back.z;
        }

        public static void SetNextPos3AndAlpha(Transform transform, float x, float y, float z, float alpha, float t)
        {
            float pre = t / 1000;
            Vector3 curPos = transform.localPosition;
            Vector3 newPos = new Vector3(curPos.x + x * pre, curPos.y + y * pre, curPos.z + z * pre);

            Vector3 diff = newPos - curPos;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(diff), 0.25f);
            transform.localPosition = newPos;
        }
    }
}