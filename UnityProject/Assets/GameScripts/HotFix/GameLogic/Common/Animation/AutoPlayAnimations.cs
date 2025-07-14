using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 //�������ź�ҵ���߼��޹أ��������б��˳�򲥷ţ��Լ���չ
public class AutoPlayAnimations : MonoBehaviour
{
    private Animation animation;
    public string animationClipName = "Idle";
    public void Start()
    {
        animation = GetComponent<Animation>();
    }


    //Animation Event����
    public void PlayFlagIdle()
    {
        animation.Play(animationClipName);
    }
}