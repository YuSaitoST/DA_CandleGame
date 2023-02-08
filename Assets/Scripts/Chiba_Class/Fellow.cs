using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fellow : MonoBehaviour
{
    private CapsuleCollider collider_ = null;
 
    [SerializeField]
    private GameObject death_effect_ = null;

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

    [SerializeField]
    private Animator animator_;
    //　到着したとする距離
    [SerializeField]
    private float arrivedDistance_ = 1.5f;
    //　追いかけ始める距離
    [SerializeField]
    private float followDistance_ = 1f;

    

    [SerializeField]
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

    [SerializeField]
    private Vector3 getLastFellow_;

    public Vector3 GetLastFellow
    {
        get { return getLastFellow_; }
    }



    [SerializeField]
    private RaderIcon radericon_ = null;

    [SerializeField]
    private FelloTalk fellotalk_ = null;
    
    public enum fellows_
        {
         normal,
         bob,
         nic,
         spencer,
         alan,
         catherine,
         groupParent,
         groupEntourage

    };
    [Header("仲間の種類")]
    [SerializeField]
    public fellows_ type_= fellows_.normal;

    [Header("デバック用")]
    [SerializeField]
    private bool debug_ = false;



    //救助されたときに一度だけ実行
    [SerializeField]
    private bool rescue_flg_ = false;

    public bool Rescue_flg_
    {
        get { return rescue_flg_; }
    }

    //親となるFellowを入れる(Groupのみ)
    [SerializeField]
    private Fellow parent_fellow_ = null;

    [SerializeField]
    private GameObject parent_fellow_gameobject_ = null;

    [SerializeField]
    private float delay_time_ = 2;

    // Start is called before the first frame update
    void Start()
    {

        death_effect_.SetActive(false);



        radericon_ = GetComponent<RaderIcon>();
        // animator_ = GetComponent<Animator>();
        agent_ = GetComponent<NavMeshAgent>();
        fellows_obj_ = GameObject.FindGameObjectsWithTag("Fellow");

        player_ = GameObject.FindGameObjectWithTag("Player");

        player_script_ = player_.GetComponent<Player>();

        collider_ = GetComponent<CapsuleCollider>();

        //最初からメンバ変数に入れようとするとしっぱいするからローカル変数にいったん入れてからメンバ変数に入れた
        var _fellow = new Fellow[fellows_obj_.Length];

        for (int h = 0; h < fellows_obj_.Length; h++)
        {
            _fellow[h] = fellows_obj_[h].GetComponent<Fellow>();
        }

        fellow_ = _fellow;

        //エラー対策(後で改善)
        if (type_ != fellows_.groupEntourage)
        {
            parent_fellow_ = gameObject.GetComponent<Fellow>();
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        if (type_ == fellows_.groupEntourage)
        {
            //親が救助されたら自動的に子もついていく
            if (parent_fellow_.Rescue_flg_)
            {

                follow_flg_ = true;
            }
        }

        if (follow_flg_)
        {
            //救助されたときに一度だけ実行
            if (!rescue_flg_)
            {
                if (type_ == fellows_.bob)
                {
                    player_script_.fellow_oxy_bomb_ = true;

                }
                else if (type_ == fellows_.spencer)
                {
                    GameProgress.instance_.Radar_Contraction();
                }
                else if (type_ == fellows_.alan)
                {
                    GameProgress.instance_.Rader_TankIconActive();
                }
                else if (type_ == fellows_.catherine)
                {

                    player_script_.fellow_oxy_add_ = true;
                }

                if (!debug_&& type_ != fellows_.groupParent&& type_ != fellows_.groupEntourage)
                {
                    fellotalk_.PlayTalk();
                    GameProgress.instance_.SetFriendWhoHelped(type_);
                }
                else if(type_ == fellows_.groupEntourage)
                {
                    
                    chase_target_ = parent_fellow_gameobject_;
                    StartCoroutine(FellowGroupEntourage());
                }

                player_script_.FellowCount();
                GameProgress.instance_.FriendWhoHelpedCount();
                    

                
                //レーダーアイコンを消す処理
                radericon_.Detectioned();

                

                rescue_flg_ = true;
            }

            agent_.speed = player_script_.Player_Move_;

            agent_.SetDestination(chase_target_.transform.position);
            //　到着している時
            if (agent_.remainingDistance < arrivedDistance_)
            {

                agent_.isStopped = true;
                animator_.SetBool("isWalking", false);
               
                animator_.SetBool("isRunning", player_script_.Fire2_Flg__);
                //animator_.SetFloat("Speed", 0f);

                //　到着していない時で追いかけ出す距離になったら
            }
            else if (agent_.remainingDistance > followDistance_)
            {

                animator_.SetBool("isWalking", true);
                agent_.isStopped = false;
                if (player_script_.Fire2_Flg__)
                {
                    animator_.SetBool("isRunning", true);
                   
                }
                else if (!player_script_.Fire2_Flg__)
                {
                   
                    animator_.SetBool("isRunning", false);
                }
                
                //animator_.SetFloat("Speed", agent_.desiredVelocity.magnitude);
            }
        }

        //最後尾の時に座標を返す
        if(last_)
        {
            getLastFellow_ = transform.position;
        }

       

        //潜水艦に回収される処理
        if(player_script_.fellow_Count_ ==0&&follow_flg_)
        {
            row_ = 0;
            transform.position = new(0,0,0);
            this.gameObject.SetActive(false);
            follow_flg_ = false;
        }

        life_inv_tmp_ = player_script_.Player_life_inv_tmp_;

        
    }
    private void FixedUpdate()
    {
        if (life_inv_tmp_ > 0)
        {
            life_inv_tmp_ -= 1.0f * Time.deltaTime;
        }
    }
    private void OnAnimatorIK()
    {
        animator_.SetLookAtWeight(1f, 0.3f, 1f, 0f, 0.5f);
        animator_.SetLookAtPosition(chase_target_.transform.position + Vector3.up * 1.5f);
    }

    IEnumerator FellowGroupEntourage()
    {
        yield return new WaitForSeconds(delay_time_);
        follow_flg_ = true;
        row_ = parent_fellow_.Row_+1;
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

    //プレイヤーにタッチされたときの判定
    public void Follow()
    {
       
        follow_flg_ = true;
        if (player_script_.fellow_Count_ > 1)
        {
            Debug.Log("A");
            row_ = player_script_.fellow_Count_ +1;
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


           
        }
        else
        {
            last_ = true;
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


    private void DeathProcess()
    {
        player_script_.fellow_die_row_ = row_;

        row_ = 0;
        animator_.SetBool("isRunning", false);
        animator_.SetBool("isWalking", false);
        agent_.enabled = false;
        collider_.enabled = false;

        death_effect_.SetActive(true);

        follow_flg_ = false;
       
        player_script_.FellowHit();

        Vector3 position = transform.position;
        position.y = 0.07f;
        transform.position = position;

        transform.eulerAngles = new Vector3(90, transform.rotation.y, 0);
    }



    public void Death()
    {

        if(follow_flg_)
        {
            int _fellow_die = player_script_.fellow_die_row_;
            if(_fellow_die <row_)
            {
               
                row_ -= 1;
                if (row_ == 1)
                {
                    row_ = 1;
                    chase_target_ = player_;

                }
                else
                {
                 
                    for (int j = 0; j < fellows_obj_.Length; j++)
                    {
                        if (fellow_[j].Row_ == row_ - 1)
                        {
                            chase_target_ = fellows_obj_[j];
                            break;

                        }
                    }

                }
            }      

        }

    }



    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.tag == "Enemy" && player_script_.Player_life_inv_tmp_ <= 0 && follow_flg_ == true)
        {
            Debug.Log("a");
            if (last_) //自分が最後尾の時
            {
                Debug.Log("最後尾死亡");
                //死んだときの処理
               
                if (row_ != 1)
                {
                    for (int i = 0; i < fellows_obj_.Length; i++)
                    {
                        if (fellow_[i].Row_ == row_ - 1)
                        {
                            fellow_[i].Last();
                            break;
                        }
                    }
                }
                DeathProcess();
                //gameObject.SetActive(false);


            }
            else//自分が最後尾ではないとき
            {
                Debug.Log("最後尾じゃない仲間が当たった");
                DeathProcess();
                for (int i = 0; i < fellows_obj_.Length; i++)
                {
                   
                    fellow_[i].Death();
                    //if (fellow_[i].Row_ == row_ + 1)
                    //{
                    //    fellow_[i].Death();
                    //    break;
                    //}
                }
              

                //無敵時間開始
                //life_inv_tmp_ = life_inv_time_;
                //player_script_.FellowHit();
            }
        }
    }
}
