using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    Transform tr_;
    Rigidbody rb_;

    [Header("プレイヤーの挙動")]
    [SerializeField,Tooltip("プレイヤーの移動速度")]
    private float    player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("ブースト時の移動速度倍率")]
    private float    player_move_boost_ = 2.0f;

    //[SerializeField, Tooltip("プレイヤーのアニメーターを入れる")]
    //private Animator palyer_animator_;

    [SerializeField]
    private bool     player_move_flg_ = true;//プレイヤーの移動停止用trueで移動可


    [Header("酸素ゲージ関連")]
    [SerializeField,Tooltip("酸素ゲージ量(現在の値)")]
    private float    oxy_           = 0.0f;

    [SerializeField, Tooltip("酸素ゲージの最大値(初期値)"), Range(0, 100)]
    private float    oxy_max_       = 100.0f;

    //[SerializeField, Tooltip("移動時の追加消費酸素(未実装)")]
    //private float    oxy_cost_move_ = 0.3f;

    [SerializeField, Tooltip("ブースト時のゲージ消費倍率")]
    private float    oxy_cost_boost_ = 2.0f;

    [SerializeField, Tooltip("平常時の消費酸素")]
    private float    oxy_cost_      = 0.3f;

    [SerializeField, Tooltip("酸素ゲージの値表示用テキスト")]
    private TMP_Text oxy_text_      = null;

    [SerializeField, Tooltip("酸素ゲージ")]
    private Slider   oxy_slider_    = null;

    [SerializeField, Tooltip("酸素ゲージが0になるとfalse")]
    private bool     oxy_flg_       = true;

    [Header("Aボタン関連")]
    [SerializeField]
    private bool fire1_range_flg_ = false;//プレイヤーが爆弾設置範囲内に入ったらtrueになる

    [Header("Bボタン関連")]
    [SerializeField]
    private bool fire2_flg_ = false;

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
        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        oxy_ = oxy_max_ -0.01f;
        oxy_flg_ = true;
    }

    // Update is called once per frame
    void Update()
    {
      


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
        }

        if (player_move_flg_ == true)
        {
            Move();
        }

    }
    //移動処理
    private void Move()
    {
      
        float _speed = 1.0f;

        Vector3 _position   = Vector3.zero;
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //移動ブースト
        if (fire2_flg_== true)
        {
            _speed = player_move_speed_ * player_move_boost_;
        }
        else
        {
            _speed = player_move_speed_;
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
                new Vector3(_position.normalized.x * _speed,
                            0,
                            _position.normalized.z * _speed);
            //oxy_ -= oxy_cost_move_ * Time.deltaTime;
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
       
        oxy_text_.SetText(oxy_.ToString("F1")/* + ("％")*/);
        oxy_slider_.value = oxy_;
    }

    private void Oxy()
    {
        if (fire2_flg_ == true)
        {      
            //ブースト中は酸素消費量も上昇する
            oxy_ -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;

        }
        else
        {
            oxy_ -= oxy_cost_ * Time.deltaTime;
        }
        

        if(oxy_ <= 0.01f)
        {
           oxy_flg_ = false;
           oxy_ = 0;
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
