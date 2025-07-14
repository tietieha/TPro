using System;
using System.Collections.Generic;
using UnityEngine;

namespace UW
{
    public class AnimationController : MonoBehaviour
    {
        private Animation _anim;
        public List<string> _clipNames = new List<string>();
        private void Awake()
        {
            _anim = GetComponent<Animation>();
        }

        public void Play(int index)
        {
            if (_clipNames.Count <= index)
            {
                return;
            }
            string clipName = _clipNames[index];
            _anim.Play(clipName);
        }
    }
}