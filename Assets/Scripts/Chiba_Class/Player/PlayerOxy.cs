using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerOxy : MonoBehaviour
{
    [Header("デバック用")]
    [SerializeField]
    private PlayerDebugMode script_debug_ = null;

    [Header("酸素ゲージ関連")]
    [SerializeField, Tooltip("酸素ゲージ量(現在の値)")]
    private float oxy_recovery_ = 33.3f;

    [SerializeField, Tooltip("いじらない")]
    private int oxy_count_ = 0;

    [SerializeField, Tooltip("酸素ゲージ量(現在の値)")]
    private float oxy_total_ = 0.0f;

    public float oxy_Total
    {
        get { return oxy_total_; }

    }

    //[SerializeField, Tooltip("酸素ゲージ1本の最大値(初期値)"), Range(0, 33.3f)]
    private float[] oxy_max_ = new float[10];

    //[SerializeField, Tooltip("酸素ゲージ1本の最大値(初期値)"), Range(0, 33.3f)]
    private float[] oxy_max_red_ = new float[10];

    //最初のタンクの数
    [SerializeField]
    private int oxy_start_ = 3;

    //追加タンクフラグ
    public bool fellow_oxy_add_ = false;

    [SerializeField]
    private float enemy_hit_damage_ = 10.0f;

    [SerializeField, Tooltip("平常時の消費酸素")]
    private float oxy_cost_ = 1.0f;

    //[SerializeField, Tooltip("酸素ゲージの値表示用テキスト")]
    //private TMP_Text oxy_text_ = null;

    [SerializeField, Tooltip("ボンベの数")]
    private Slider[] oxy_slider_ = new Slider[3];

    [SerializeField, Tooltip("ボンベの数")]
    private Slider[] oxy_slider_red_ = null;

    [SerializeField]
    private GameObject oxy_add_slider_ = null;

    public bool dash_flg_ = false;
    [SerializeField, Tooltip("ブースト時のゲージ消費倍率")]
    private float oxy_cost_boost_ = 2.0f;
    void Start()
    {
        PLAYER _param = GameProgress.instance_.GetParameters().player;
        oxy_cost_ = _param.oxy_cost;
        oxy_cost_boost_ = _param.oxy_cost_boost;
        enemy_hit_damage_ = _param.damage;

        //追加酸素ボンベのUIを非表示
        oxy_add_slider_.SetActive(false);
        //酸素のセット
        float _h = 100 / oxy_start_;
        for (int i = 0; i < oxy_start_; i++)
        {

            oxy_max_[i] = _h;
            oxy_max_red_[i] = _h;

        }
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (dash_flg_)
        {
            //ダッシュ時は倍率分消費する
            oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;
        }
        else
        {
            oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;

        }




        //ボンベのゲージが0になったら次のボンベに切り替える
        if (fellow_oxy_add_)
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 3)
            {

                oxy_max_[oxy_count_] = 0.0f;
 

                oxy_count_++;
                Debug.Log(oxy_count_);


            }
        }
        else
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 2)
            {

                oxy_max_[oxy_count_] = 0.0f;
               

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

        //同期
        if (fellow_oxy_add_)
        {
            oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2] + oxy_max_[3];

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


        //赤ゲージ関連
        if (oxy_max_red_[oxy_count_] > oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] -= 9.0f * Time.deltaTime;

        }
        else if (oxy_max_red_[oxy_count_] < oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] = oxy_max_[oxy_count_];

        }

        //赤ゲージがなくなる前に次のボンベに行った場合赤ゲージを消す
        if (oxy_count_ >= 1)
        {
            oxy_max_red_[oxy_count_ - 1] = 0.0f;
        }

      //  oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("％")*/);
    }

    //酸素爆弾を使った時の消費量計算
    public void OxyBomb(float _cost)
    {
        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] - _cost;
        if (_tmp < 0.0f)
        {
            oxy_max_[oxy_count_] = 0;
            if (script_debug_ == false)
            {
                oxy_count_++;
            }
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= _cost;
        }
    }

    public void OxyAdd()
    {
        fellow_oxy_add_ = true;
    }

   public void OxyDamage()
    {
        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] - enemy_hit_damage_;
        if (_tmp < 0.0f)
        {
            oxy_max_[oxy_count_] = 0;
            if (script_debug_ == false)
            {
                oxy_count_++;
            }
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= enemy_hit_damage_;
        }
    }

    //酸素ボンベを空状態から回復
    public void OxyRecoveryBlood()
    {
        oxy_count_--;
        oxy_max_[2] = 33.3f;
        
    }

    public void OxyRecovery()
    {
        ////1本以上消費している場合
        if (oxy_count_ >= 1)
        {
            oxy_max_[oxy_count_] += oxy_recovery_;

            float _tmp = 0.0f;
            _tmp = oxy_max_[oxy_count_] - 33.3f;
            oxy_max_[oxy_count_] = 33.3f;
            oxy_count_--;
            oxy_max_[oxy_count_] = _tmp;
        }
        else
        {

            oxy_max_[oxy_count_] = 33.3f;
            
        }
    }
}
