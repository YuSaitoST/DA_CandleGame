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
        //レーダーの強化(検知範囲の拡張)
        GameProgress.instance_.Radar_Contraction();

        if (!debug_)
        {
            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.spencer);
        }
    }
}
