using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class SimpleToggle : MonoBehaviour
    {
        public List<GameObject> Active;
        public List<GameObject> Inactive;
        private UIButtonSuper _button;
        private Action<int> _selectToggleEvent;
        private int _index;
        private void Awake()
        {
            _button = GetComponent<UIButtonSuper>();
            _button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            if (_selectToggleEvent != null)
            {
                _selectToggleEvent(_index);
            }
        }

        public void Init(int select, int index,Action<int> selectToggleEvent )
        {
            _index = index;
            _selectToggleEvent = selectToggleEvent;
            SetState(select == index);
        }

        public void SetState(bool state)
        {
            foreach (var go in Active)
            {
                go.SetActive(state);
            }
            foreach (var go in Inactive)
            {
                go.SetActive(!state);
            }
        }
    }
}
