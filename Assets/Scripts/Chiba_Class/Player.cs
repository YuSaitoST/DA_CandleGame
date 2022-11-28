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

    //state
    public enum State
    {
        Idle,     //通常
        Dash,     //移動(加速)
        Action00, //アイテムを拾う
        Action01, //ポンプ落とす
        Action02, //ポンプ投げ
        Action03, //パーツを拾う
        Damage,   //ダメージくらう
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
    private float[]  oxy_max_       =  new float[3];

    [SerializeField, Tooltip("ブースト時のゲージ消費倍率")]
    private float    oxy_cost_boost_ = 2.0f;

    [SerializeField, Tooltip("平常時の消費酸素")]
    private float    oxy_cost_      = 1.0f;

    [SerializeField, Tooltip("酸素ゲージの値表示用テキスト")]
    private TMP_Text oxy_text_      = null;

    [SerializeField, Tooltip("ボンベの数")]
    private Slider[] oxy_slider_ = new Slider[3];

  

    [Header("Aボタン関連")]
    [SerializeField,Tooltip("debug用")]
    private bool fire1_range_flg_ = false;//プレイヤーが範囲内に入ったらtrueになる

    [Header("Bボタン関連")]
    //[SerializeField]
    //private bool fire2_flg_ = false;


    [Header("Xボタン関連")]
    [SerializeField]
    private bool fire3_flg_ = false;

  

    //長押し秒数の取得
    [SerializeField]
    private float fire3_button_count_ = 0.0f;

    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject fire3_tank_prefab_;
   
    private DrawArc fire3_draw_;

    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject fire3_point_;

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(1.0F, 20.0F), Tooltip("弾の射出する速さ")]
    private float fire3_speed_ = 1.0F;
   
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
    [SerializeField,Tooltip("潜水艦パーツのゲージ")]
    private Slider submarine_slider_ = null;
    [SerializeField, Tooltip("潜水艦パーツのゲージのキャンバス")]
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

    [SerializeField,Tooltip("BloodDirectionをアタッチ")]
    BloodDirection bloodDirection_ = null;

    void Start()
    {
        
        // 弾の初速度や生成座標を持つコンポーネント
        fire3_draw_ = gameObject.GetComponent<DrawArc>();

        for (int i = 0; i < oxy_max_.Length; i++)
       {
            oxy_max_[i] = 33.3f;
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

        tr_.transform.position = new Vector3(
        GameProgress.instance_.GetParameters().player_pos_x,
        GameProgress.instance_.GetParameters().player_pos_y,
        GameProgress.instance_.GetParameters().player_pos_x);


    }

    // Update is called once per frame
    void Update()
    {
       

        //プレイヤー入力
        PlayerInput();

        //同期
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
        oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("％")*/);

        oxy_slider_[0].value    = oxy_max_[0];
        oxy_slider_[1].value    = oxy_max_[1];
        oxy_slider_[2].value    = oxy_max_[2];
        submarine_slider_.value = submarine_limit_tmp_;
        // 弾の初速度を更新
        shootVelocity_ = fire3_point_.transform.up * fire3_speed_;

        // 弾の生成座標を更新
        instantiatePosition_ = fire3_point_.transform.position;

        //0になったらゲームオーバー
        if (debug_death_==false) 
        {
            if (oxy_total_ <= 0.01f)
            {
                type_ = State.Death;
            }
        }

        //ボンベのゲージが0になったら次のボンベに切り替える
        if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 2)
        {
            oxy_max_[oxy_count_] = 0.0f;
            //別クラス呼び出し
            fire3_draw_.Off();

            oxy_count_++;
            Debug.Log(oxy_count_);
        }
        //EventSystem.current.SetSelectedGameObject(button_firstSelect_);
    }
    void PlayerInput()
    {
        //プレイヤーの入力
       

        //Bボタン
        //移動速度上昇
        if (Input.GetButton("Fire2")&&type_!=State.Damage && type_ != State.Action00)
        {
            type_ = State.Dash;
            Debug.Log("Bボタンが押された");
           
        }
        else if(Input.GetButtonUp("Fire2") && type_ != State.Damage && type_ != State.Action00)
        {
            type_ = State.Idle;
            Debug.Log("Bボタンが離された");
            
        }

        //Xボタン
        //ボンベアクション
        if (oxy_count_ != 2 && type_ != State.Damage && type_ != State.Action00)
        {

            if (Input.GetButton("Fire3"))
            {
                //溜めてる状態をわかりやすくする
                //canvasOn
                fire3_button_count_ += Time.deltaTime;
                type_ = State.Action01;
                if (fire3_button_count_ >= 1.0f)
                {
                    //別クラス呼び出し
                    fire3_draw_.On();
                    cancel_ui_.enabled = true;
                }

                //アクションをキャンセル(Aボタン)
                if(Input.GetButtonDown("Fire1"))               
                {
                    cancel_ui_.enabled = false;
                    fire3_button_count_ = 0;
                    //別クラス呼び出し
                    fire3_draw_.Off();
                    //処理
                    type_ = State.Dash; //idleに移行
                }
            }
            if (Input.GetButtonUp("Fire3"))
            {

                if (fire3_button_count_ <= 1.0f)
                {

                    //oxy_max_[oxy_count_] = 0;
                    //oxy_count_++;

                    //type_ = State.Action02;//捨てるステート
                    //Debug.Log("アクション実行02-1");
                }
                else if (fire3_button_count_ >= 1.0f)
                {
                    

                    type_ = State.Action03;//投げるステート
                    Debug.Log("アクション実行03-1");
                }

                
                

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
                    //fire2_flg_ = false;                    
                    Move();
                    //処理

                }
                break;
            
            case State.Dash://走る(勝手にidleに戻らない)
                {
                    //別クラス呼び出し
                    fire3_draw_.Off();
                    //不具合修正仮
                    cancel_ui_.enabled = false;

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
            case State.Action01: //Xボタン待機(勝手にidleに戻らない)
                {      
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    Move();
                    //処理

                }
                break;
            case State.Action02://落とす未使用
                {                   
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;

                    //処理
                    Action02();
                    type_ = State.Idle; 
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
            case State.Damage://ダメージくらう
                {

                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;


                    //処理
                    Damage();                
                }
                break;

            case State.Death://(idleに戻らない)
                {
                    
                   
                    //
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
            submarine_ui_.enabled = false;
            submarine_limit_tmp_ = 0;
            type_ = State.Idle;
            PartsManager.Instance.submarine();
            submarine_slider_canvas_.enabled = false;
        }
       
        

    }
    private void Action02()
    {
        //別クラス呼び出し
        fire3_draw_.Off();

    }

    private void Action03()
    {
        //別クラス呼び出し
        fire3_draw_.Off();

       
        oxy_max_[oxy_count_] = 0;
        oxy_count_++;

        // 弾を生成して飛ばす
        GameObject _obj = Instantiate(fire3_tank_prefab_, instantiatePosition_, Quaternion.identity);
        Rigidbody _rid = _obj.GetComponent<Rigidbody>();
        _rid.AddForce(shootVelocity_ * _rid.mass, ForceMode.Impulse);

        // 5秒後に消える
        Destroy(_obj, 5.0F);
        Debug.Log("アクション実行02-2");

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
        //tagは変える
        if (collision.gameObject.tag == "BombArea"&& player_life_inv_tmp_ <= 0)
        {
            
            type_ = State.Damage;
            //ダメージ食らう
            oxy_max_[oxy_count_] -= damage_;

            //無敵時間開始
            player_life_inv_tmp_ = player_life_inv_time_;

            rb_.velocity = Vector3.zero;
            // 自分の位置と接触してきたオブジェクトの位置を計算
            Vector3 _distination = (transform.position - collision.transform.position).normalized;

            rb_.AddForce(_distination * knockback_power_, ForceMode.VelocityChange);
            rb_.AddForce(transform.up * knockback_power_up_, ForceMode.VelocityChange);

          
        }
    }
   

    private void OnTriggerStay(Collider other)
    {
        //潜水艦
        if(other.gameObject.CompareTag("Submarine") && PartsManager.Instance.count_>0)
        {
            if (cancel_ui_.enabled == false)
            {
                submarine_ui_.enabled = true;
            }
            if(Input.GetButton("Fire1"))
                {
                submarine_slider_canvas_.enabled = true;
                //ステート移行
                type_ = State.Action00;
                }
            else //(Input.GetButtonUp("Fire1"))
            {
                
                submarine_limit_tmp_ = 0;
                submarine_slider_canvas_.enabled = false;
                type_ = State.Idle;
            }

        }

        //パーツの範囲
        if (other.gameObject.CompareTag("PC"))
        {
            if (cancel_ui_.enabled == false)
            {
                item_ui_.enabled = true;
            }
            fire1_range_flg_ = true;
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("押された");
                var _parts = other;
                _parts.GetComponent<Parts>().Pickup();
                _parts = null;

            }
            
        }

        //ボンベ回復
        if (other.gameObject.CompareTag("Tank"))
        {
            if (cancel_ui_.enabled == false)
            {
                item_ui_.enabled = true;
            }
            fire1_range_flg_ = true;
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("押された");
                //1本以上消費している場合
                if (oxy_count_ >= 1)
                {
                    oxy_max_[oxy_count_] += oxy_recovery_;

                    //1案
                    float _tmp = 0.0f;
                    _tmp = oxy_max_[oxy_count_] - 33.3f;
                    oxy_max_[oxy_count_] = 33.3f;
                    oxy_count_--;
                    oxy_max_[oxy_count_] = _tmp;

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
            submarine_slider_canvas_.enabled = false;
            if (type_ == State.Action00)
            {
                type_= State.Idle;
            }
        }

    }
}
