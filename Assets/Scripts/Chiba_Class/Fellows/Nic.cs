using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif

public class Nic : Fellow
{
  
    protected override void RescueProcess()
    {
        //����C�x���g�Ȃ�(����̂��߂Ɉꉞ�m��)

        //�f�o�b�N���[�h�̂Ƃ��͉�b���X�L�b�v����
        if (!debug_)
        {
            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.nic);
        }
    }
}
