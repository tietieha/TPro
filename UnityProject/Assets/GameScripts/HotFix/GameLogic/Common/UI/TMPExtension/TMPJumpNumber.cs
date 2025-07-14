
using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using TMPro;
using UnityEngine;

public class TMPJumpNumber : MonoBehaviour
{
    long currentNum = 0;
    long targetNum = 0;
    long fixNum = 0;
    public TextMeshProUGUI text;
    private Coroutine _coroutine;
    private bool isS2Sec = false;
    // Start is called before the first frame update

    void Reset()
    {
        text = this.transform.GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Log.Error("???  没有 TextMeshProUGUI 组件 ！！ 请自行添加并 填充引用");
        }
    }

    public void InitNumber(long number)
    {
        if (text!= null)
        {
            text.text = number.ToString();
            currentNum = number;
        }
    }

    public void SetNumber(long number,bool isS2Sec = false)
    {
        if (currentNum == number)
        {
            return;
        }
        this.isS2Sec = isS2Sec;
        targetNum = number;
        fixNum = (long)((number - currentNum) / 10);
        if (!this.isActiveAndEnabled)
        {
            currentNum = number;
            SetText(text, number); 
            return;
        }
           
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine("StartJumpNum");
    }
    IEnumerator StartJumpNum()
    {
        while (targetNum != currentNum)
        {
            if (targetNum > currentNum)
            {
                currentNum += fixNum;
                if (currentNum >= targetNum)
                {
                    currentNum = targetNum;
                    SetText(text, currentNum);
                    yield break;
                }
                else
                {
                    SetText(text, currentNum);
                    yield return UnityConstans.wait0dot01s;
                }

            }
            else if (targetNum < currentNum)
            {
                currentNum += fixNum;
                if (currentNum <= targetNum)
                {
                    currentNum = targetNum;
                    SetText(text, currentNum);
                    yield break;
                }
                else
                {
                    SetText(text, currentNum);
                    yield return UnityConstans.wait0dot01s;
                }
            }
            else
            {
                yield break;
            }
        }
       
    }
    /// <summary>
    /// 可能产生大量gc 慎用
    /// </summary>
    /// <param name="text"></param>
    /// <param name="currentNum"></param>
    private void SetText(TextMeshProUGUI text, long currentNum)
    {
        if (this.isS2Sec)
        {
            text.text = StringUtils.S2Sec(currentNum.ToString());
        }
        else 
        {
            text.text = currentNum.ToString();
        }
    }
    public void StopJump()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = null;
    }
    private void OnDisable()
    {
        StopJump();
    }
}
