using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class SimpleToggleGroup : MonoBehaviour
    {
        public int defaultSelectIndex = 0;
        private SimpleToggle[] _toggles;
        private Action<int> _selectToggleHandler;
        private int _curSelectIndex;

        public void Initialize(int forceSelectIndex,Action<int> selectToggleHandler)
        {
            if (forceSelectIndex >= 0)
            {
                _curSelectIndex = forceSelectIndex;
            }
            else
            {
                _curSelectIndex = defaultSelectIndex;
            }
            _toggles = GetComponentsInChildren<SimpleToggle>();
            _selectToggleHandler = selectToggleHandler;
            for (int i = 0; i < _toggles.Length; i++)
            {
                _toggles[i].Init(_curSelectIndex,i,SelectCurToggleHandler);
            }
        }

        //强制设置 Toggle
        public void ForceSetToggle(int index)
        {
            _toggles[_curSelectIndex].SetState(false);
            _curSelectIndex = index;
            _toggles[_curSelectIndex].SetState(true);
            _selectToggleHandler?.Invoke(_curSelectIndex);
        }

        private void SelectCurToggleHandler(int index)
        {
            if (_curSelectIndex == index)
                return;
            
            _toggles[_curSelectIndex].SetState(false);
            _curSelectIndex = index;
            _toggles[_curSelectIndex].SetState(true);
            _selectToggleHandler?.Invoke(_curSelectIndex);
        }
    }
}
