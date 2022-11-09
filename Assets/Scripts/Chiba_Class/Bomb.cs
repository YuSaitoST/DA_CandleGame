using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField,Tooltip("爆弾となるゲームオブジェクトをここに")]
    private GameObject bomb_obj_;

    //これが0になったら爆弾が設置される
    [SerializeField, Tooltip("爆弾設置にかかる時間")]
    private float bomb_time_ = 10.0f;

    [SerializeField,Tooltip("時間表記用スライダー")]
    private Slider bomb_slider_;

    [SerializeField, Tooltip("時間表記用スライダー")]
    private Canvas bomb_canvas_;

    //ボタンが押されたときにtrueになる
    private bool bomb_installation_flg_ = false;

    //プレイヤーが爆弾設置範囲内に入ったらtrueになる
    private bool bomb_range_flg_ = false;

    Vector3 setPos_ = new Vector3(0.0f, -300.0f, 0.0f);

    private float bomb_limit_ = 0.0f;

    //連続で実行されないようにする
    private bool flg_ = false;
    [SerializeField]
    bool isInstalled_ = false;



    // Start is called before the first frame update
    void Start()
    {
        bomb_canvas_.enabled = false; // UI無効にする
        bomb_slider_.maxValue = bomb_time_;
       
    }

    // Update is called once per frame
    void Update()
    {
        //Aボタン
        if (Input.GetButton("Fire1")&& bomb_range_flg_ == true)
        {
            bomb_installation_flg_ = true;
        }
        else
        {
            bomb_installation_flg_ = false;
        }

        //角度を同じにする
        bomb_canvas_.transform.localRotation = Camera.main.transform.rotation;



       
    }

    private void FixedUpdate()
    {
        //trueのとき1秒ずつ足される
        if(bomb_installation_flg_ == true)
        {
            bomb_limit_ += Time.deltaTime;
        }

        //0秒になると実行
        if (bomb_time_ <= bomb_limit_ && flg_ == false)
        {
            bomb_canvas_.enabled = false; // UI無効にする
            bomb_time_ = 10.0f;
            //爆弾が表示される
            Debug.Log("爆弾が置かれた");
            bomb_obj_.SetActive(true);
            //エリアを非表示に(または削除)
            //this.gameObject.SetActive(false);
            this.transform.localPosition = setPos_;
            flg_ = true;

            // ゲーム終了後は処理をしない
            if (BombManager.instance_.IsAllInstalled())
            {
                return;
            }

            isInstalled_ = true;
            BombManager.instance_.Check_AllInstalled();   // 全ての設置エリアを調べる
        }
       bomb_slider_.value = bomb_limit_;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bomb_range_flg_ = true;
            bomb_canvas_.enabled = true;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bomb_range_flg_ = false;
            bomb_canvas_.enabled = false;
        }
       
    }
}
