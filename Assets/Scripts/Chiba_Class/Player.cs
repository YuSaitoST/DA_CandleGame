using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//ある程度形にしたら関数を消して一つにまとめる予定
public class Player : MonoBehaviour
{
    [Header("デバック用")]
    [SerializeField,Tooltip("trueでゲームオーバーにならなくなります")]
    private bool debug_death_ = false;

    Transform tr_;
    Rigidbody rb_;

    [Header("プレイヤーの挙動")]

    [SerializeField,Tooltip("プレイヤーの移動速度(初期値)")]
    private float    player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("ブースト時の移動速度(最大値)")]
    private float    player_move_boost_ = 1.0f;

    [SerializeField, Tooltip("現在のの移動速度(最大値)")]
    private float    player_move_ = 1.0f;

    public float Player_Move_
    {
        get { return player_move_; }
    }

    [SerializeField, Tooltip("プレイヤーのアニメーターを入れる")]
    private Animator player_animator_ =null;

    [SerializeField]
    private bool     player_move_flg_ = true;//プレイヤーの移動停止用trueで移動可

    [SerializeField, Tooltip("プレイヤーの体力(マテリアルの不透明度で表現)未使用")]
    private float player_life_ = 100.0f; 

    [SerializeField,Tooltip("無敵時間")]
    private float player_life_inv_time_ = 3.0f;

    //無敵時間
    private float player_life_inv_tmp_ = 0.0f;

    public float Player_life_inv_tmp_
    {
        get { return player_life_inv_tmp_; }
    }

    //state
    public enum State
    {
        Idle,     //通常
        Dash,     //移動(加速)
        Action00, //アイテムを拾う
        Action01, //押し始め
        Action02, //ポンプ投げ待機
        Action03, //ポンプ投げ
        Action04, //ポンプ落とす
        Damage,   //ダメージくらう
        Blood,    //酸素がないとき
        Death     //死
    }
    //2型の作成
    public State type_ = State.Idle;

    [Header("酸素ゲージ関連")]
    //[SerializeField,Tooltip("酸素ゲージ量(現在の値)")]
    //private float    oxy_           = 0.0f;

    [SerializeField, Tooltip("酸素ゲージ量(現在の値)")]
    private float    oxy_recovery_ = 33.3f;

    [SerializeField,Tooltip("いじらない")]
    private int      oxy_count_ =0;

    [SerializeField, Tooltip("酸素ゲージ量(現在の値)")]
    private float    oxy_total_ = 0.0f;

    //[SerializeField, Tooltip("酸素ゲージ1本の最大値(初期値)"), Range(0, 33.3f)]
    private float[]  oxy_max_       =  new float[10];

    //[SerializeField, Tooltip("酸素ゲージ1本の最大値(初期値)"), Range(0, 33.3f)]
    private float[] oxy_max_red_ = new float[10];

    //最初のタンクの数
    [SerializeField]
    private int oxy_start_ = 3;

    //追加タンクフラグ
    public bool fellow_oxy_add_ = false;

    //爆弾強化フラグ
    public bool fellow_oxy_bomb_ = false;

    [SerializeField, Tooltip("ブースト時のゲージ消費倍率")]
    private float    oxy_cost_boost_ = 2.0f;

    [SerializeField, Tooltip("平常時の消費酸素")]
    private float    oxy_cost_      = 1.0f;

    [SerializeField, Tooltip("酸素ゲージの値表示用テキスト")]
    private TMP_Text oxy_text_      = null;

    [SerializeField, Tooltip("ボンベの数")]
    private Slider[] oxy_slider_ = new Slider[3];

    [SerializeField, Tooltip("ボンベの数")]
    private Slider[] oxy_slider_red_ = null;

    [SerializeField]
    private GameObject oxy_add_slider_ = null;

    [Header("Aボタン関連")]
    [SerializeField,Tooltip("debug用")]
    private bool fire1_range_flg_ = false;//プレイヤーが範囲内に入ったらtrueになる

    [SerializeField]
    private bool fire1_cancel_ = false;

    [Header("Bボタン関連")]

    [SerializeField]
    private bool fire2_flg_ = false;

    public bool Fire2_Flg__
    {
        get { return fire2_flg_; }
    }

    [Header("Xボタン関連")]
    [SerializeField]
    private bool fire3_flg_ = false;

    private int count_ = 0;

    //長押し秒数の取得
    [SerializeField]
    private float fire3_button_count_ = 0;

    //酸素消費量
    [SerializeField]
    private float[] fire3_button_cost_ = new float[] { 5, 8, 11, 14, 17, 20};

    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject[] fire3_tank_prefab_ = new GameObject[] {null};
   
    private DrawArc fire3_draw_;

    [SerializeField, Tooltip("爆弾投げの開始点")]
    private GameObject fire3_point_;

    [SerializeField, Tooltip("爆弾を落とす位置")]
    private GameObject fire3_drop_;

    //プレハブの射出位置
    private Vector3 fire3_drop_pos_;

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(0.0f, 20.0f), Tooltip("弾の射出する速さ")]
    private float fire3_speed_ = 1.0f;


    [SerializeField, Range(0.0f, 1.0f), Tooltip("弾の射出する高さ")]
    private float fire3_speed2_ = 0.42f;

    // 弾の初速度
    private Vector3 shootVelocity_;
 
    // 弾の初速度(読み取り専用)
    public Vector3 ShootVelocity_
    {
        get { return shootVelocity_; }
    }

    [Header("フィールド移動制限(下限、上限)")]
    [SerializeField,Tooltip("X軸の制限")]
    private Vector2 x_clip_ = new(-9, 9);

    [SerializeField, Tooltip("Y軸の制限")]
    private Vector2 y_clip_ = new(-200, 200);

    [SerializeField, Tooltip("Z軸の制限")]
    private Vector2 z_clip_ = new(-5, 5);

    //[SerializeField, Tooltip("メニューを開いたときに最初に選択されるボタン")]
    //private GameObject ui_button_firstSelect_;

    [Header("ダメージ設定など")]
    [SerializeField, Tooltip("酸素ゲージへのダメージ量")]
    private float damage_ = 10.0f;

    [SerializeField, Tooltip("ノックバック時の力")]
    private float knockback_power_ =0.8f;

    [SerializeField, Tooltip("ノックバック時の上への力")]
    private float knockback_power_up_ = 0.7f;

    [SerializeField, Tooltip("動けない時間")]
    private float knockback_stan_ = 1.0f;

    //潜水艦リミット
    [SerializeField,Tooltip("潜水艦仲間回収のゲージ")]
    private Slider submarine_slider_ = null;
    [SerializeField, Tooltip("仲間回収のゲージのキャンバス")]
    private Canvas submarine_slider_canvas_ = null;
    [SerializeField]
    private float submarine_limit_ = 5.0f;

    private float submarine_limit_tmp_ = 0.0f;
  
    //ButtonUI
    //潜水艦
    [SerializeField]
    private Canvas submarine_ui_ = null;
    //ボタン
    [SerializeField]
    private Canvas item_ui_ = null;

    //アクションキャンセルボタン
    [SerializeField]
    private Canvas cancel_ui_ = null;

    [SerializeField]
    private Canvas fellow_ui_ = null;

    [SerializeField]
    private Canvas recovery_ui_ = null;

    [SerializeField]
    private int fellow_count_ = 0;

    [SerializeField]
    private int fellow_rescue_ = 0;

   //読み取り用
    public int fellow_Count_
    {
        get { return fellow_count_; }
    }

    public int fellow_die_row_ = 0;

    [SerializeField,Tooltip("BloodDirectionをアタッチ")]
    BloodDirection bloodDirection_ = null;


    [Header("SE")]
    [SerializeField, Tooltip("SEプレイヤー")]
    PlayersSEPlayer sePlayer_ = null;

    void Start()
    {


        oxy_add_slider_.SetActive(false);

        // 弾の初速度や生成座標を持つコンポーネント
        fire3_draw_ = GetComponent<DrawArc>();

        float _h = 100 / oxy_start_;
        for (int i = 0; i < oxy_start_; i++)
       {
            
            oxy_max_[i] = _h;
            oxy_max_red_[i] = _h;

        }

        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        player_move_= player_move_speed_;
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];

        ////無敵時間からスタン時間分を引く
        //player_life_inv_time_ -= knockback_stan_;

        submarine_slider_canvas_.enabled = false; 

        //ButtonUI
        //潜水艦
        submarine_ui_.enabled = false;
        //ボタン
        item_ui_.enabled = false;
        //アクションキャンセルボタン
        cancel_ui_.enabled = false;
        
        PLAYER _param = GameProgress.instance_.GetParameters().player;

        player_move_speed_  = _param.move_speed;
        player_move_boost_  = _param.move_boost;
        oxy_cost_boost_     = _param.oxy_cost_boost;
        oxy_cost_           = _param.oxy_cost;
        damage_             = _param.damage;
        knockback_power_    = _param.knockback_power;
        knockback_power_up_ = _param.knockback_power_up;
        knockback_stan_     = _param.knockback_stan_;

        float _pos_x = _param.pos_x;
        float _pos_y = _param.pos_y; 
        float _pos_z = _param.pos_z;

        this.transform.position = new Vector3(_pos_x, _pos_y, _pos_z);
    }

    // Update is called once per frame
    void Update()
    {
       

        //プレイヤー入力
        PlayerInput();

        //同期
        if (fellow_oxy_add_)
        {
            oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2]+ oxy_max_[3];

            oxy_slider_[0].value = oxy_max_[3];
            oxy_slider_[1].value = oxy_max_[2];
            oxy_slider_[2].value = oxy_max_[1];
            oxy_slider_[3].value = oxy_max_[0];

            oxy_slider_red_[0].value = oxy_max_red_[3];
            oxy_slider_red_[1].value = oxy_max_red_[2];
            oxy_slider_red_[2].value = oxy_max_red_[1];
            oxy_slider_red_[3].value = oxy_max_red_[0];
        }
        else
        {
            oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];

            oxy_slider_[0].value = oxy_max_[2];
            oxy_slider_[1].value = oxy_max_[1];
            oxy_slider_[2].value = oxy_max_[0];

            oxy_slider_red_[0].value = oxy_max_red_[2];
            oxy_slider_red_[1].value = oxy_max_red_[1];
            oxy_slider_red_[2].value = oxy_max_red_[0];

            
            
        }



        oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("％")*/);

     
        submarine_slider_.value = submarine_limit_tmp_;
        // 弾の初速度を更新
        shootVelocity_ = fire3_point_.transform.up * fire3_speed_;

        shootVelocity_.y = fire3_speed2_;

        // 弾の生成座標を更新
        instantiatePosition_ = fire3_point_.transform.position;
        fire3_drop_pos_ = fire3_drop_.transform.position;

        //0になったらBloodステートに移動
        if (debug_death_==false) 
        {
            if (oxy_total_ <= 0.01f)
            {

                type_ = State.Blood;

                // 齋藤の追加ソース
                bloodDirection_.OxyEmpty();
            }
        }

        //ボンベのゲージが0になったら次のボンベに切り替える
        if(fellow_oxy_add_)
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 3)
            {
                oxy_max_red_[oxy_count_] = 0.0f;
                oxy_max_[oxy_count_] = 0.0f;
                //別クラス呼び出し
                fire3_draw_.Off();

                oxy_count_++;
                Debug.Log(oxy_count_);
            }
        }
        else
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 2)
            {
                oxy_max_red_[oxy_count_] = 0.0f;
                oxy_max_[oxy_count_] = 0.0f;
                //別クラス呼び出し
                fire3_draw_.Off();

                oxy_count_++;
                Debug.Log(oxy_count_);
            }
        }
        //4本目のボンベを追加
        if (fellow_oxy_add_)
        {
            if (!oxy_add_slider_.activeSelf)
            {
                oxy_max_[3] = 33;
            }
            oxy_add_slider_.SetActive(true);

            //4本目のボンベUIを表示
            //float _tmp = oxy_max_[oxy_count_];
            //oxy_max_[oxy_count_] = 33;
           
            // oxy_max_[4] = 33;



        }
        //EventSystem.current.SetSelectedGameObject(button_firstSelect_);
    }
    
    void PlayerInput()
    {
        //プレイヤーの入力

        

        //Bボタン
        //移動速度上昇
        if (Input.GetButton("Fire2")&&type_!=State.Damage && type_ != State.Action00 && type_ != State.Blood)
        {
            fire2_flg_ = true;
            type_ = State.Dash;
            Debug.Log("Bボタンが押された");
           
        }
        else if(Input.GetButtonUp("Fire2") && type_ != State.Damage && type_ != State.Action00 && type_ != State.Blood)
        {
            fire2_flg_ = false;
            type_ = State.Idle;
            Debug.Log("Bボタンが離された");
            
        }

        //Xボタン
        //ボンベアクション
        if (/*oxy_count_ != 2 &&*/ oxy_max_[2] >= fire3_button_cost_[0] && type_ != State.Damage && type_ != State.Action00 && type_ != State.Blood
            || fellow_oxy_add_ && oxy_max_[3] >= fire3_button_cost_[0] && type_ != State.Damage && type_ != State.Action00 && type_ != State.Blood)
        {

            if (Input.GetButton("Fire3")&&fire1_cancel_ == false )
            {
                
                //溜めてる状態をわかりやすくする
                
                fire3_button_count_ += Time.deltaTime;
                type_ = State.Action01;
                if (fire3_button_count_ >= 1.0f)
                {
                  
                    type_ = State.Action02;
                }

                //アクションをキャンセル(Aボタン)
                if(Input.GetButtonDown("Fire1"))               
                {
                    fire1_cancel_ = true;
                    cancel_ui_.enabled = false;
                    fire3_button_count_ = 0;
                    //別クラス呼び出し
                    fire3_draw_.Off();
                    //処理
                    type_ = State.Idle; //idleに移行
                }
            }
            if (Input.GetButtonUp("Fire3"))
            {
                if (fire1_cancel_ == false)
                {
                    count_ = 0;
                    type_ = State.Idle; //idleに移行

                    //押した時間が1秒未満の場合
                    if (fire3_button_count_ < 1.0f)
                    {
                        type_ = State.Action04;//落とすステート
                        Debug.Log("アクション実行04-1");
                    }
                    else
                    {
                        if (fire3_button_count_ >= 5.0f&&fellow_oxy_bomb_)
                        {
                            count_ = 5;
                            type_ = State.Action03;//投げるステート
                            Debug.Log("アクション実行03-5");
                        }
                        else if (fire3_button_count_ >= 4.0f&& fellow_oxy_bomb_)
                        {
                            count_ = 4;
                            type_ = State.Action03;//投げるステート
                            Debug.Log("アクション実行03-4");
                        }
                        else if (fire3_button_count_ >= 3.0f)
                        {
                            count_ = 3;
                            type_ = State.Action03;//投げるステート
                            Debug.Log("アクション実行03-3");
                        }
                        else if (fire3_button_count_ >= 2.0f)
                        {
                            count_ = 2;
                            type_ = State.Action03;//投げるステート
                            Debug.Log("アクション実行03-2");
                        }
                        else if (fire3_button_count_ >= 1.0f)
                        {
                            count_ = 1;
                            type_ = State.Action03;//投げるステート
                            Debug.Log("アクション実行03-1");


                        }
                    }
                }
                fire1_cancel_ = false;
                cancel_ui_.enabled = false;
                fire3_button_count_ = 0;

            }
        }

        
    }

    void FixedUpdate()
    {
        //state
        switch (type_)
        {
            case State.Idle:
                {
                   
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                                     
                    Move();
                    //処理

                }
                break;
            
            case State.Dash://走る(勝手にidleに戻らない)
                {
                    //別クラス呼び出し
                    fire3_draw_.Off();
                    

                    //ブースト中は酸素消費量も上昇する
                    oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;
                   
                    Move();       
                    
                }
                break;

            case State.Action00: //パーツを設置(勝手にidleに戻らない)
                {        
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //処理
                    //アニメーション
                    Action00();
                }
                break;
            case State.Action01: //Xボタン開始(勝手にidleに戻らない)
                {      
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    Move();
                    //処理
                   

                }
                break;
            case State.Action02://Xボタン離し待機(勝手にidleに戻らない)
                {                   
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //処理
                    Move();
                    Action02();
                }
                break;
            case State.Action03://投げる
                {
                    
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //処理
                    Action03();
                    type_ =State.Idle;
                    
                }
                break;
            case State.Action04://落とす
                {

                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //処理
                    Action04();
                    type_ = State.Idle;

                }
                break;
            case State.Damage://ダメージくらう
                {

                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //処理
                    Damage();                
                }
                break;
            case State.Blood://ダメージくらう
                {
                    fire3_draw_.Off();
                    //処理
                    Move();
                }
                break;

            case State.Death://(idleに戻らない)
                {
                    
                   
                   
                    //oxyが0になったらゲームオーバー
                    Debug.Log("ゲームオーバー");
                    //処理
                    //アニメーターリセット
                    player_animator_.SetBool("isRunning", false);
                    player_animator_.SetBool("isWalking", false);

                   
                    oxy_max_[0] = 0;
                    oxy_max_[1] = 0;
                    oxy_max_[2] = 0;
                    oxy_total_ = 0;

                    GameProgress.instance_.GameOver();
                }
                break;
        }

        //敵に当たった時の処理
       
        if (player_life_inv_tmp_ > 0)
        {
            player_life_inv_tmp_ -= 1.0f*Time.deltaTime;
        }

        //赤ゲージ関連
        if (oxy_max_red_[oxy_count_] > oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] -= 9.0f * Time.deltaTime;

        }
        else if (oxy_max_red_[oxy_count_] < oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] = oxy_max_[oxy_count_];

        }
    }
    private void Action00()
    {
        //アニメーターリセット
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);

        if (submarine_limit_ >= submarine_limit_tmp_)
        {
            submarine_limit_tmp_ += Time.deltaTime;
        }
        else if (submarine_limit_ <= submarine_limit_tmp_)
        {
            recovery_ui_.enabled = false;
            //submarine_ui_.enabled = false;
            submarine_limit_tmp_ = 0;
            type_ = State.Idle;
           // PartsManager.Instance.submarine();
            submarine_slider_canvas_.enabled = false;

            //仲間を回収
            Debug.Log("仲間を回収した");
            fellow_rescue_ += fellow_count_;
            fellow_count_ = 0;
        }
       
        

    }
    private void Action02()
    {
        //別クラス呼び出し
        fire3_draw_.On();
        cancel_ui_.enabled = true;
        submarine_ui_.enabled = false;

    }

    private void Action03()
    {
       

        //別クラス呼び出し
        fire3_draw_.Off();

        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] -fire3_button_cost_[count_];
        if (_tmp < 0.0f)
        {
            oxy_max_red_[oxy_count_] = 0;
            oxy_max_[oxy_count_] = 0;
            if (debug_death_ == false)
            {
                oxy_count_++;
            }
            
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= fire3_button_cost_[count_];
        }
        // 弾を生成して飛ばす
        GameObject _obj = Instantiate(fire3_tank_prefab_[count_], instantiatePosition_, Quaternion.identity);
        Rigidbody _ri = _obj.GetComponent<Rigidbody>();
        _ri.AddForce(shootVelocity_ * _ri.mass, ForceMode.Impulse);

        Debug.Log("アクション実行03-2");

        //UIを非表示にする
        cancel_ui_.enabled = false;
    }
    private void Action04()
    { 
        //別クラス呼び出し
        fire3_draw_.Off();

        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] - fire3_button_cost_[count_];
        if (_tmp < 0.0f)
        {
            oxy_max_[oxy_count_] = 0;
            if (debug_death_ == false)
            {
                oxy_count_++;
            }
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= fire3_button_cost_[count_];
        }
       
        // 弾を生成して飛ばす
        GameObject _obj = Instantiate(fire3_tank_prefab_[0], fire3_drop_pos_, Quaternion.identity);
       
        Debug.Log("アクション実行04");

        //UIを非表示にする
        cancel_ui_.enabled = false;
    }

    private void Damage()
    {
        StartCoroutine(OnDamage());
    }

    IEnumerator OnDamage()
    {
        bloodDirection_.DamageDone();
        //別クラス呼び出し
        fire3_draw_.Off();

        //アニメーターリセット
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);

        yield return new WaitForSeconds(knockback_stan_);
        type_ = State.Idle; 
        
        
    }



        //移動処理
        private void Move()
    {
        Vector3 _position   = Vector3.zero;
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //移動ブースト
        if (type_==State.Dash)
        {
            if (player_move_<= player_move_boost_)
            {
                player_animator_.SetBool("isRunning", true);
                //徐々に足されていく
                player_move_ += 0.1f* Time.deltaTime;
            }
            else 
            {
                player_move_= player_move_boost_;
            }
        }
        else
        {
            if (player_move_>= player_move_speed_)
            {
                player_animator_.SetBool("isRunning", false);
                //徐々にひかれていく
                player_move_ -= 0.1f * Time.deltaTime;
            }
            else 
            {
                player_move_= player_move_speed_;
            }
           
        }

        //移動制限
        tr_.transform.position =  new Vector3(
                Mathf.Clamp(this.transform.position.x, x_clip_.x, x_clip_.y), 
                Mathf.Clamp(this.transform.position.y, y_clip_.x, y_clip_.y), 
                Mathf.Clamp(this.transform.position.z, z_clip_.x, z_clip_.y)
                );
      
        if (_stick_left.x != 0.0f || _stick_left.y != 0.0f)
        {
            _position.x  = _stick_left.x;
            _position.z  = _stick_left.y;
            rb_.velocity =
                new Vector3(_position.normalized.x * player_move_,
                            0,
                            _position.normalized.z * player_move_);

            player_animator_.SetBool("isWalking", true);

            // スティックが倒れていれば、倒れている方向を向く
            var _direction2 = new Vector3(_stick_left.x, 0, _stick_left.y);
            transform.localRotation = Quaternion.LookRotation(_direction2);

        }
        else
        {
            player_animator_.SetBool("isWalking", false);

        }
    }
    //ダメージ判定
    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.tag == "Enemy"&& player_life_inv_tmp_ <= 0)
        {
            rb_.velocity = Vector3.zero;
            // 自分の位置と接触してきたオブジェクトの位置を計算
            Vector3 _distination = (transform.position - collision.transform.position).normalized;

            rb_.AddForce(_distination * knockback_power_, ForceMode.VelocityChange);
            rb_.AddForce(transform.up * knockback_power_up_, ForceMode.VelocityChange);

            if (type_ != State.Blood)
            {
                type_ = State.Damage;
                //ダメージ食らう
                oxy_max_[oxy_count_] -= damage_;
            }
            else 
            {

            }
            //無敵時間開始
            player_life_inv_tmp_ = player_life_inv_time_;

          

            sePlayer_.TakeDamage();
        }
    }

    //味方が敵に当たった時の処理
    public void FellowHit()
    {
        //無敵時間開始
        player_life_inv_tmp_ = player_life_inv_time_;
        fellow_count_ -= 1;
        //bloodDirection_.DamageDone();
        sePlayer_.TakeDamage();
    }


    private void OnTriggerStay(Collider other)
    {
      
        if (type_ == State.Idle || type_ == State.Dash|| type_ == State.Action00 || type_ == State.Blood)
        {
            //潜水艦
            //仲間回収
            if (other.gameObject.CompareTag("Submarine") && fellow_count_ > 0)
            {

                recovery_ui_.enabled = true;


                if (Input.GetButton("Fire1"))
                {
                    submarine_slider_canvas_.enabled = true;
                    //ステート移行
                    type_ = State.Action00;
                }
                else
                {

                    submarine_limit_tmp_ = 0;
                    submarine_slider_canvas_.enabled = false;
                    type_ = State.Idle;
                }

            }

            //パーツ
            //if (other.gameObject.CompareTag("Submarine") && PartsManager.Instance.count_ > 0)
            //{

            //    submarine_ui_.enabled = true;


            //    if (Input.GetButton("Fire1"))
            //    {
            //        submarine_slider_canvas_.enabled = true;
            //        //ステート移行
            //        type_ = State.Action00;
            //    }
            //    else
            //    {

            //        submarine_limit_tmp_ = 0;
            //        submarine_slider_canvas_.enabled = false;
            //        type_ = State.Idle;
            //    }

            //}

            //仲間の処理
            if (other.gameObject.CompareTag("RescueArea"))
            {
                fellow_ui_.enabled = true;

                fire1_range_flg_ = true;
                if (Input.GetButton("Fire1"))
                {
                    fellow_ui_.enabled = false;
                    fellow_count_ += 1;
                    var _fellow = other;
                    _fellow.GetComponent<RescueArea>().Follow();
                    _fellow = null;
                    sePlayer_.PartGet();
                }


            }

                //パーツの範囲
            //    if (other.gameObject.CompareTag("PC"))
            //{

            //    item_ui_.enabled = true;

            //    fire1_range_flg_ = true;
            //    if (Input.GetButton("Fire1"))
            //    {
                  
            //        var _parts = other;
            //        _parts.GetComponent<Parts>().Pickup();
            //        _parts = null;
            //        sePlayer_.PartGet();
            //    }

            //}

            //ボンベ回復
            if (other.gameObject.CompareTag("Tank"))
            {
               
                item_ui_.enabled = true;
                
                fire1_range_flg_ = true;
                if (Input.GetButton("Fire1"))
                {
                   
                    //もしstateの状態がbloodだったら
                   
                    //1本以上消費している場合
                    if (oxy_count_ >= 1)
                    {
                        if (type_ == State.Blood)
                        {
                            oxy_count_--;
                            oxy_max_[2] = 33.3f;
                            type_ = State.Idle;

                        }

                        else
                        {
                            oxy_max_[oxy_count_] += oxy_recovery_;

                            //1案
                            float _tmp = 0.0f;
                            _tmp = oxy_max_[oxy_count_] - 33.3f;
                            oxy_max_[oxy_count_] = 33.3f;
                            oxy_count_--;
                            oxy_max_[oxy_count_] = _tmp;

                        }

                        //2案
                        //oxy_count_--;
                        //oxy_max_[oxy_count_] += oxy_recovery_;

                        var _tank = other;
                        _tank.GetComponent<Tank>().Pickup();
                        _tank = null;
                    }
                    //1本も消費していない場合
                    else
                    {
                        oxy_max_[oxy_count_] = 33.3f;
                        var _tank = other;
                        _tank.GetComponent<Tank>().Pickup();
                        _tank = null;
                    }

                    bloodDirection_.DamageRecovery();
                    sePlayer_.TankGet();
                }

            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        //アイテムの範囲から出た
        if (other.gameObject.CompareTag("PC"))
        {
            item_ui_.enabled = false;
            fire1_range_flg_ = false;
        }

        if (other.gameObject.CompareTag("Tank"))
        {
            item_ui_.enabled = false;
            fire1_range_flg_ = false;
        }

        //潜水艦
        if (other.gameObject.CompareTag("Submarine"))
        {
            submarine_limit_tmp_ = 0;
            fire1_range_flg_ = false;
            submarine_ui_.enabled = false;
            recovery_ui_.enabled = false;
            submarine_slider_canvas_.enabled = false;
            if (type_ == State.Action00)
            {
                type_= State.Idle;
            }
        }

        if (other.gameObject.CompareTag("RescueArea"))
        {
            fellow_ui_.enabled = false;
            fire1_range_flg_ = false;

        }

    }
}
