using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField,Tooltip("爆弾となるゲームオブジェクトをここに")]
    private GameObject bombObj_;

    //これが0になったら爆弾が設置される
    [SerializeField, Tooltip("爆弾設置にかかる時間")]
    private float bombTime_ = 10.0f;

    [SerializeField,Tooltip("時間表記用スライダー")]
    private Slider bombSlider_;

    [SerializeField, Tooltip("時間表記用スライダー")]
    private Canvas bombCanvas_;

    //ボタンが押されたときにtrueになる
    private bool installationFlg_ = false;

    //プレイヤーが爆弾設置範囲内に入ったらtrueになる
    private bool rangeFlg_ = false;

    Vector3 setPos_ = new Vector3(0.0f, -300.0f, 0.0f);

    private float bombLimit_ = 0.0f;

    //連続で実行されないようにする
    private bool flg_ = false;



    // Start is called before the first frame update
    void Start()
    {
        bombCanvas_.enabled = false; // UI無効にする
        bombSlider_.maxValue = bombTime_;
       
    }

    // Update is called once per frame
    void Update()
    {
        //Aボタン
        if (Input.GetButton("Fire1")&& rangeFlg_ == true)
        {
            installationFlg_ = true;
        }
        else
        {
            installationFlg_ = false;
        }

        //角度を同じにする
        bombCanvas_.transform.localRotation = Camera.main.transform.rotation;
    }

    private void FixedUpdate()
    {
        //trueのとき1秒ずつ足される
        if(installationFlg_ == true)
        {
            bombLimit_ += Time.deltaTime;
        }

        //0秒になると実行
        if (bombTime_ <= bombLimit_ && flg_ == false)
        {
            bombCanvas_.enabled = false; // UI無効にする
            bombTime_ = 10.0f;
            //爆弾が表示される
            Debug.Log("爆弾が置かれた");
            bombObj_.SetActive(true);
            //エリアを非表示に(または削除)
            //this.gameObject.SetActive(false);
            this.transform.localPosition = setPos_;
            flg_ = true;
        }
       bombSlider_.value = bombLimit_;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeFlg_ = true;
            bombCanvas_.enabled = true;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeFlg_ = false;
            bombCanvas_.enabled = false;
        }
       
    }
}
