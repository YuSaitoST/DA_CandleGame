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
        //プレイヤーの酸素ボンベの数が3つから4つになる
        player_script_.FellowOxyAdd();

        if (!debug_)
        {

            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(1);
        }
    }
}
