using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif
public class Spencer : Fellow
{
   
    
    protected override void RescueProcess()
    {
        //���[�_�[�̋���(���m�͈͂̊g��)
        GameProgress.instance_.Radar_Contraction();

        if (!debug_)
        {
            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.spencer);
        }
    }
}
