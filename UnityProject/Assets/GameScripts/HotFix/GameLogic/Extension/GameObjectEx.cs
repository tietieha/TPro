using System;
using UnityEngine;
using XLua;

namespace Components
{
    [LuaCallCSharp]
    public class GameObjectEx:MonoBehaviour
    {
        private GameObject _go;
        private Transform _tf;

        private void Awake()
        {
            _go = this.gameObject;
            _tf = _go.transform;
        }
        private Vector3 _relPos = Vector3.zero;
        private Vector3 _longPos = new Vector3(9999,9999,9999);
        public void SetVisible(bool b)
        {
            if (b)
            {
                _tf.localPosition = _relPos;
            }
            else
            {
                if (!_relPos.Equals(_tf.localPosition))
                {
                    _relPos = _tf.localPosition;
                }
                _tf.localPosition = _longPos;
            }
        }
        public void SetPosition(float x,float y,float z)
        {
            var v3 = _tf.position;
            v3.x = x;
            v3.y = y;
            v3.z = z;
            _tf.position = v3;
        }
        
        public Vector3 GetPosition()
        {
            return _tf.position;
        }
        
        public void SetLocalScale(float x,float y,float z)
        {
            var v3 = _tf.localScale;
            v3.x = x;
            v3.y = y;
            v3.z = z;
            _tf.localScale = v3;
        }
        
        public Vector3 GetLocalScale()
        {
            return _tf.localScale;
        }
        
        public void SetLocalPosition(float x,float y,float z)
        {
            var v3 = _tf.localPosition;
            v3.x = x;
            v3.y = y;
            v3.z = z;
            _tf.localPosition = v3;
        }
        
        public Vector3 GetLocalPosition()
        {
            return _tf.localPosition;
        }

        public void SetRotation(float x,float y,float z,float w)
        {
            var q = _tf.rotation;
            q.x = x;
            q.y = y;
            q.z = z;
            q.w = w;
            _tf.rotation = q;
        }
        
        public Quaternion GetRotation()
        {
            return _tf.rotation;
        }
        
        public void SetLocalRotation(float x,float y,float z,float w)
        {
            var q = _tf.localRotation;
            q.x = x;
            q.y = y;
            q.z = z;
            q.w = w;
            _tf.localRotation = q;
        }
        
        public Quaternion GetLocalRotation()
        {
            return _tf.localRotation;
        }
        
      
        public void ToReset()
        {
            _tf.localPosition = Vector3.zero;
            _tf.localScale = Vector3.one;
            _tf.localRotation = Quaternion.identity;
        }
    }
}