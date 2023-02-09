using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif

public class Bob : Fellow
{

    protected override void RescueProcess()
    {
        //�v���C���[�̎_�f���e�����������
        player_script_.fellow_oxy_bomb_ = true;

        if (!debug_)
        {
            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.bob);
        }
    }
}