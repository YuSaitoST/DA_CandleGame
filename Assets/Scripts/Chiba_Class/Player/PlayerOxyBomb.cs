using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxyBomb : MonoBehaviour
{
    [Header("Playerクラス")]
    [SerializeField]
    private PlayerOxy script_player_oxy_ = null;
    [SerializeField]
    private DrawArc fire3_draw_;
    [Space(10)]
    //酸素消費量
    [SerializeField]
    private float[] bomb_cost_ = new float[] { 5, 8, 11, 14, 17, 20 };

    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject[] bomb_prefab_ = new GameObject[] { null };  

    [SerializeField, Tooltip("爆弾投げの開始点")]
    private GameObject point_;

    [SerializeField, Tooltip("爆弾を落とす位置")]
    private GameObject drop_;

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
    [Space(10)]
    [SerializeField]
    private float press_count_ = 0;

    //爆弾強化フラグ
    private bool fellow_oxy_bomb_ = false;

    // Update is called once per frame
    void Update()
    {
        // 弾の初速度を更新
        shootVelocity_ = point_.transform.up * fire3_speed_;

        shootVelocity_.y = fire3_speed2_;

        // 弾の生成座標を更新
        instantiatePosition_ = point_.transform.position;
        fire3_drop_pos_ = drop_.transform.position;
    }

    //爆弾強化フラグ
    public void OxyBombAdd()
    {
        fellow_oxy_bomb_ = true;
    }


    public void PressCount()
    { //別クラス呼び出し
        //fire3_draw_.On();

        press_count_ += Time.deltaTime;
    }

    public void CountReset()
    {
        //別クラス呼び出し
        //fire3_draw_.Off();
        press_count_ = 0;
    }

    public void Bomb()
    {
        //別クラス呼び出し
        //fire3_draw_.Off();
        //1秒未満
        if (press_count_ <=1)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[0]);
            // 弾を生成して飛ばす
            GameObject _obj = Instantiate(bomb_prefab_[0], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if(press_count_ >=1)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[1]);
            // 弾を生成して飛ばす
            GameObject _obj = Instantiate(bomb_prefab_[1], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 2)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[2]);
            // 弾を生成して飛ばす
            GameObject _obj = Instantiate(bomb_prefab_[2], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 3)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[3]);
            // 弾を生成して飛ばす
            GameObject _obj = Instantiate(bomb_prefab_[3], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 4&& fellow_oxy_bomb_)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[4]);
            // 弾を生成して飛ばす
            GameObject _obj = Instantiate(bomb_prefab_[4], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 5&& fellow_oxy_bomb_)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[5]);
            // 弾を生成して飛ばす
            GameObject _obj = Instantiate(bomb_prefab_[5], fire3_drop_pos_, Quaternion.identity);
            return;
        }
       

    }
        

}
