using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif


public class Alan : Fellow
{
    
    
    protected override void RescueProcess()
    {
        //�p�~
        ////���[�_�[�̋���(tank�̈ʒu�����[�_�[�Ō�����悤��)
        //GameProgress.instance_.Rader_TankIconActive();

        if (!debug_)
        {

            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(1);
        }
    }
}
