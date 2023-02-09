using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif

public class Catherine : Fellow
{
   
    protected override void RescueProcess()
    {
        //�v���C���[�̎_�f�{���x�̐���3����4�ɂȂ�
        player_script_.fellow_oxy_add_ = true;

        if (!debug_)
        {

            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.catherine);
        }
    }
}
