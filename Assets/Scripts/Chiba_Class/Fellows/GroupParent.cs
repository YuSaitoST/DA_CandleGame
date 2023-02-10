using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif


public class GroupParent : Fellow
{
   
    protected override void RescueProcess()
    {
     

        if (!debug_)
        {
            //未実装のためこのまま呼び出すとエラー
            fellotalk_.PlayTalk();

            GameProgress.instance_.SetFriendWhoHelped(fellows_.groupParent);
        }
    }

}
