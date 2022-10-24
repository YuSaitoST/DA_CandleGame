using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField,Tooltip("爆弾となるゲームオブジェクトをここに")]
    private GameObject bombobj_;

    //これが0になったら爆弾が設置される
    [SerializeField, Tooltip("爆弾設置にかかる時間")]
    private float bombtime_ = 10.0f;

    //ボタンが押されたときにtrueになる
    private bool installationflg_ = false;

    //プレイヤーが爆弾設置範囲内に入ったらtrueになる
    private bool rangeflg_ = false;

    Vector3 setPos_ = new Vector3(0.0f, -300.0f, 0.0f);

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //Aボタン
        if (Input.GetButton("Fire1")&& rangeflg_ == true)
        {
            installationflg_ = true;
        }
        else
        {
            installationflg_ = false;
        }

        //0秒になると実行
        if(bombtime_<=0)
        {
            bombtime_ = 0.0f;
            //爆弾が表示される
            bombobj_.SetActive(true);
            //エリアを非表示に(または削除)
            //this.gameObject.SetActive(false);
            this.transform.localPosition = setPos_; 
        }
    }

    private void FixedUpdate()
    {
        //trueのとき1秒ずつ減っていく
        if(installationflg_ == true)
        {
            bombtime_ -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeflg_ = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeflg_ = false;
        }
    }
}
