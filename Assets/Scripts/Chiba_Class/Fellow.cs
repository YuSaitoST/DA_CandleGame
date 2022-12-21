using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fellow : MonoBehaviour
{
 
    [SerializeField]
    private NavMeshAgent agent_;
    // 追いかけるキャラクター

    //プレイヤー
    [SerializeField]
    private GameObject player_ = null;

    [SerializeField]
    private GameObject[] fellows_obj_ = null;

    [SerializeField]
    private Fellow[] fellow_ /*= new Fellow[100]*/;

    [SerializeField]
    private GameObject chase_target_;

    private Animator animator_;
    //　到着したとする距離
    [SerializeField]
    private float arrivedDistance_ = 1.5f;
    //　追いかけ始める距離
    [SerializeField]
    private float followDistance_ = 1f;

    private bool follow_flg_ = false;

   [SerializeField,Tooltip("無敵時間")]
    private float life_inv_time_ = 3.0f;

    //無敵時間
    private float life_inv_tmp_ = 0.0f;

    [SerializeField]
    private Player player_script_ = null;

    //自分が最後尾かどうか
    [SerializeField]
    private bool last_ = false;

    //自分が列で何番目か
    private int row_ = 0;

   //読み取り用
    public int Row_
    {
        get { return row_; }
    }


    // Start is called before the first frame update
    void Start()
    {
        animator_ = GetComponent<Animator>();
        agent_ = GetComponent<NavMeshAgent>();
        fellows_obj_ = GameObject.FindGameObjectsWithTag("Fellow");//ここまではうまくいってる

        // 配列でコンポーネントの一覧を取得

        //fellow_[0] = fellows_obj_[0].GetComponent<Fellow>();


        //fellow_[0]= fllows_obj_[0].GetComponent<Fellow>();
        //for (int h = 0; h <= fllows_obj_.Length ; h++)
        //{
        //    //fellow_[h] = fllows_obj_[h].GetComponent<Fellow>();
        //}
        

        var _fellow = new Fellow[fellows_obj_.Length];
        //fellow_[0] = fellows_obj_[0].GetComponent<Fellow>();
        for (int h = 0; h < fellows_obj_.Length; h++)
        {
            _fellow[h] = fellows_obj_[h].GetComponent<Fellow>();
        }

        fellow_ = _fellow;
    }
   

    // Update is called once per frame
    void Update()
    {
        if (follow_flg_)
        {
            agent_.SetDestination(chase_target_.transform.position);

            //　到着している時
            if (agent_.remainingDistance < arrivedDistance_)
            {

                agent_.isStopped = true;
                animator_.SetFloat("Speed", 0f);
                //　到着していない時で追いかけ出す距離になったら
            }
            else if (agent_.remainingDistance > followDistance_)
            {

                agent_.isStopped = false;
                animator_.SetFloat("Speed", agent_.desiredVelocity.magnitude);
            }
        }

        if (life_inv_tmp_ > 0)
        {
            life_inv_tmp_ -= 1.0f * Time.deltaTime;
        }

        if(player_script_.fellow_Count_ ==0&&follow_flg_)
        {
            row_ = 0;
            transform.position = new(0,0,0);
            this.gameObject.SetActive(false);
            follow_flg_ = false;
        }
    }
    private void OnAnimatorIK()
    {
        animator_.SetLookAtWeight(1f, 0.3f, 1f, 0f, 0.5f);
        animator_.SetLookAtPosition(chase_target_.transform.position + Vector3.up * 1.5f);
    }

    //プレイヤーにタッチされたときの判定
    public void Follow()
    {
        follow_flg_ = true;
        if (player_script_.fellow_Count_ > 1)
        {
            row_ = player_script_.fellow_Count_ ;
            //ほかに仲間がいる場合こいつが最後尾になる
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


            //chase_target_ = player_;
        }
        else
        {
            row_ = 1;
            //仲間を連れていないとき
            chase_target_ = player_;

        }
    }
    public void AddFellow()
    {
        last_ = false;

    }

    //最後尾が被弾した場合の引継ぎ処理
    public void Last()
    {
        last_ = true;

    }


    //潜水艦に収納されたときの処理
    //public void Submarine()
    //{
       
    //    Destroy(gameObject);
    //}

   


    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.tag == "Enemy" && life_inv_tmp_ <= 0 && follow_flg_ == true)
        {

            if (last_&& row_ !=1)
            {

                //死んだときの処理
                //自分が最後尾の時
                for (int i = 0; i < fellows_obj_.Length; i++)
                {
                    if (fellow_[i].Row_ == row_ - 1)
                    {
                        fellow_[i].Last();
                        break;
                    }
                }
               
                //無敵時間開始
                follow_flg_ = false;
                row_ = 0;
                player_script_.FellowHit();



            }
            else
            {
                //無敵時間開始
                life_inv_tmp_ = life_inv_time_;
                player_script_.FellowHit();
            }
        }
    }
}
