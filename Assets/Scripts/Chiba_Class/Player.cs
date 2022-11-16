using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//ある程度形にしたら関数を消して一つにまとめる予定
public class Player : MonoBehaviour
{
    Transform tr_;
    Rigidbody rb_;

    [Header("プレイヤーの挙動")]
    [SerializeField,Tooltip("プレイヤーの移動速度(初期値)")]
    private float    player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("ブースト時の移動速度(最大値)")]
    private float    player_move_boost_ = 1.0f;

    [SerializeField, Tooltip("現在のの移動速度(最大値)")]
    private float player_move_ = 1.0f;

    //[SerializeField, Tooltip("プレイヤーのアニメーターを入れる")]
    //private Animator palyer_animator_;

    [SerializeField]
    private bool     player_move_flg_ = true;//プレイヤーの移動停止用trueで移動可


    [Header("酸素ゲージ関連")]
    //[SerializeField,Tooltip("酸素ゲージ量(現在の値)")]
    //private float    oxy_           = 0.0f;

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

    [SerializeField, Tooltip("聴力のパラメータ")]
    private Slider[] oxy_slider_ = new Slider[3];

    [SerializeField, Tooltip("酸素ゲージが0になるとfalse")]
    private bool     oxy_flg_       = true;

    [Header("Aボタン関連")]
    [SerializeField]
    private bool fire1_range_flg_ = false;//プレイヤーが爆弾設置範囲内に入ったらtrueになる

    [Header("Bボタン関連")]
    [SerializeField]
    private bool fire2_flg_ = false;


    [Header("Xボタン関連")]
    [SerializeField]
    private bool fire3_flg_ = false;

    [SerializeField]
    private bool fire3_flg2_ = false;

    //長押し秒数の取得
    private float fire3_button_count_ = 0.0f;

    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject fire3_tank_prefab_;
    //bulletPrefab;

   
    private DrawArc fire3_Draw;

    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject fire3_point_;
    //barrelObject_

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(1.0F, 20.0F), Tooltip("弾の射出する速さ")]
    private float speed = 1.0F;

   
    // 弾の初速度
    private Vector3 shootVelocity;
 
    // 弾の初速度(読み取り専用)
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
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

    



    void Start()
    {
        // 弾の初速度や生成座標を持つコンポーネント
        fire3_Draw = gameObject.GetComponent<DrawArc>();

        for (int i = 0; i < oxy_max_.Length; i++)
       {
            oxy_max_[i] = 33.3f;
       }
        

        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        

        oxy_flg_ = true;

        player_move_= player_move_speed_;

        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
    }

    // Update is called once per frame
    void Update()
    {
        // 弾の初速度を更新
        shootVelocity = fire3_point_.transform.up * speed;

        // 弾の生成座標を更新
        instantiatePosition_ = fire3_point_.transform.position;


        if (oxy_flg_ == true)
        {
            //プレイヤーの入力
            //Aボタン
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("Aボタンが押されている");
                //爆弾設置
                if (fire1_range_flg_ == true)
                {

                    player_move_flg_ = false;
                }

            }
            else if (Input.GetButtonUp("Fire1"))
            {
                player_move_flg_ = true;
            }

            //Bボタン
            //移動速度上昇
            if (Input.GetButton("Fire2"))
            {
                Debug.Log("Bボタンが押された");
                fire2_flg_ = true;
            }
            else 
            {
                fire2_flg_ = false;
            }

            //Xボタン
            //ボンベアクション
            if (oxy_count_ != 2)
            {
               
                if (Input.GetButton("Fire3"))
                {
                    fire3_flg_ = true;
                }
                if (Input.GetButtonUp("Fire3"))
                {
                    fire3_flg_ = false;
                    if (fire3_button_count_ <= 1.0f)
                    {
                        oxy_max_[oxy_count_] = 0;
                        oxy_count_++;
                        Debug.Log("アクション実行01-2");
                    }

                }
            }

            //if (Input.GetButtonDown("Fire2"))
            //{
            //    Debug.Log("Bボタンが押された");
            //    if (fire2_flg_ == false)
            //    {
            //        fire2_flg_ = true;

            //    }
            //    else if (fire2_flg_ == true)
            //    {
            //        fire2_flg_ = false;
            //        //EventSystem.current.SetSelectedGameObject(button_firstSelect_);
            //    }

            //}
        }
        else
        {
            player_move_flg_ = false;
        }
        Sync();
    }

    void FixedUpdate()
    {
        if (oxy_flg_ == true)
        {
            Oxy();
            OxyAction();
        }

        if (player_move_flg_ == true)
        {
            Move();
        }

        


    }
    private void OxyAction()
    {
        if (fire3_flg_ == true)
        {

            fire3_button_count_ += Time.deltaTime;
            if (fire3_button_count_ >= 1.0f)
            {
                fire3_flg2_ = true;
                Debug.Log("アクション実行02");
                fire3_Draw.On();
            }
            else
            {

                Debug.Log("アクション実行01-1");
            }
        }
        else if (fire3_flg_ == false)
        {
            fire3_button_count_ = 0;
            if (fire3_flg2_ == true)
            {
                fire3_Draw.Off();
                fire3_flg2_ = false;
                // 弾を生成して飛ばす
                GameObject _obj = Instantiate(fire3_tank_prefab_, instantiatePosition_, Quaternion.identity);
                Rigidbody _rid = _obj.GetComponent<Rigidbody>();
                _rid.AddForce(shootVelocity * _rid.mass, ForceMode.Impulse);

                // 5秒後に消える
                Destroy(_obj, 5.0F);
                Debug.Log("アクション実行02-2");
            }
        }
    }
        //移動処理
        private void Move()
    {
      
        

        Vector3 _position   = Vector3.zero;
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //移動ブースト
        if (fire2_flg_== true)
        {
            if (player_move_<= player_move_boost_)
            {
                
                //徐々に足されていく
                player_move_+= 0.1f* Time.deltaTime;
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
              
                //徐々にひかれていく
                player_move_-= 0.1f * Time.deltaTime;
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
           
            //animator_.SetBool("walking", true);

            // スティックが倒れていれば、倒れている方向を向く
            var direction2 = new Vector3(_stick_left.x, 0, _stick_left.y);
            transform.localRotation = Quaternion.LookRotation(direction2);

        }
        else
        {
            //animator_.SetBool("walking", false);

        }
    }

    //UI同期
    private void Sync()
    {
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
        oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("％")*/);

        oxy_slider_[0].value = oxy_max_[0];
        oxy_slider_[1].value = oxy_max_[1];
        oxy_slider_[2].value = oxy_max_[2];
    }

    private void Oxy()
    {
       
        if (oxy_max_[oxy_count_] <= 0.0f)
        {
            oxy_count_++;
            Debug.Log(oxy_count_);
        }

        if (fire2_flg_ == true)
        {      
            
            //ブースト中は酸素消費量も上昇する
            oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;

        }
        else
        {
            oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
        }
        
       

        if(oxy_total_ <= 0.01f)
        {
           oxy_flg_ = false;
           oxy_total_ = 0;
           Debug.Log("a");
        }
        //oxyが0になったらゲームオーバー
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            fire1_range_flg_ = true;
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            fire1_range_flg_ = false;
        }
        
    }
}
