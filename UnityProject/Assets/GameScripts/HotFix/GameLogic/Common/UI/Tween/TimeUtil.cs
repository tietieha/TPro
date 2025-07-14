using System;
using UnityEngine;

namespace GameLogic
{
    public class TimeUtil : MonoBehaviour
    {
        public static TimeUtil Get(GameObject go)
        {
            TimeUtil instance = go.GetComponent<TimeUtil>();
            if (instance == null)
            {
                instance = go.AddComponent<TimeUtil>();
            }
            return instance;
        }

        private float _duration;
        private Action _callback;
        private bool _running;
        public void Delay( float duration, Action callback )
        {
            _duration = duration;
            _callback = callback;
            _running = true;
        }

        private void Update()
        {
            if (_running)
            {
                _duration -= Time.deltaTime;
                if (_duration <= 0)
                {
                    if (_callback != null)
                    {
                        _callback();
                    }
                    _running = false;
                }
            }
        }

        public void Kill()
        {
            _running = false;
            _duration = 0;
            _callback = null;
        }
    }
}