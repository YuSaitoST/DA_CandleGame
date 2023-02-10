using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Fellow))]
#endif

public class GroupEntourage : Fellow
{
    [Header("取り巻き専用設定")]
    //追尾したいFellowを入れる(Groupのみ)
    [SerializeField]
    private Fellow parent_fellow_ = null;

    [SerializeField]
    private GameObject parent_fellow_gameobject_ = null;

    [SerializeField]
    private float delay_time_ = 2;

    protected override void Update()
    {
        //親が救助されたら自動的に子もついていく
        if (parent_fellow_.Rescue_flg_)
        {
            chase_target_ = parent_fellow_gameobject_;
            StartCoroutine(FellowGroupEntourage());

        }

        if (follow_flg_)
        {
            //救助されたときに一度だけ実行
            if (!rescue_flg_)
            {

                GameProgress.instance_.FriendWhoHelpedCount();

                //レーダーアイコンを消す処理
                radericon_.Detectioned();

                RescueProcess();

                rescue_flg_ = true;
            }

            Move();

            
        }

        //潜水艦に回収される処理
        if (player_script_.fellow_Count_ == 0 && follow_flg_)
        {
            row_ = 0;
            transform.position = new(0, 0, 0);
            this.gameObject.SetActive(false);
            follow_flg_ = false;
        }

    }

    protected override void RescueProcess()
    {

        

        //if (!debug_)
        //{
           

        //    GameProgress.instance_.SetFriendWhoHelped(fellows_.groupEntourage);
        //}

    }

    private IEnumerator FellowGroupEntourage()
    {
        yield return new WaitForSeconds(delay_time_);
        follow_flg_ = true;
        row_ = parent_fellow_.Row_ + 1;
        if (player_script_.fellow_Count_ > 1)
        {
            last_ = true;

            for (int i = 0; i < fellows_obj_.Length; i++)
            {

                if (fellow_[i].Row_ == row_ - 1)
                {
                    chase_target_ = fellows_obj_[i];
                    fellow_[i].AddFellow();
                    break;
                }
            }
        }

    }
}
