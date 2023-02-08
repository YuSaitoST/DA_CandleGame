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
        //レーダーの強化(tankの位置がレーダーで見えるように)
        GameProgress.instance_.Rader_TankIconActive();

        if (!debug_)
        {

            fellotalk_.PlayTalk();
            GameProgress.instance_.SetFriendWhoHelped(fellows_.alan);
        }
    }
}
