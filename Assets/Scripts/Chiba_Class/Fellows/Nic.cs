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
        //現状イベントなし(今後のために一応確保)

        //デバックモードのときは会話をスキップする
        if (!debug_)
        {
            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.nic);
        }
    }
}
