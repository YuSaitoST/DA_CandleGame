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
        player_script_.FellowOxyBomb();

        if (!debug_)
        {
            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(1);
        }
    }
}
