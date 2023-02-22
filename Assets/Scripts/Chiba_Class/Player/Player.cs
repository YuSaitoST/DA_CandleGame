using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("デバック用")]
    [SerializeField]
    private PlayerDebugMode script_debug_ = null;

    [Space(10)]

    [Header("Playerクラス")]
    [SerializeField]
    private PlayerOxy script_player_oxy_ = null;
    [SerializeField]
    private PlayerMove script_player_move_ = null;
    [SerializeField]
    private PlayerOxyBomb script_player_oxybomb_ = null;
    [SerializeField]
    private PlayerFellowCount script_player_count_ = null;

    [SerializeField]
    private UIManager UI_ = null;

    [Space(10)]
    [SerializeField, Tooltip("BloodDirectionをアタッチ")]
    BloodDirection bloodDirection_ = null;

    [Header("SE")]
    [SerializeField, Tooltip("SEプレイヤー")]
    PlayersSEPlayer sePlayer_ = null;

    [Space(10)]
    [SerializeField, Tooltip("無敵時間")]
    private float player_life_inv_time_ = 3.0f;

    //無敵時間
    [SerializeField]
    private float player_life_inv_tmp_ = 0.0f;

    public float Player_life_inv_tmp_
    {
        get { return player_life_inv_tmp_; }
    }
    public enum state
    {
        Idle,     //通常
        Action00, //アイテムを拾う
        Action_Bomb, //酸素爆弾投げ
        Damage,   //ダメージくらう
        Blood,    //酸素がないとき
        Menu,     //潜水艦メニュー操作中
        Rescue,   //潜水艦に仲間を収容中
        Escape,   //脱出するかの選択画面
        Death     //死
    }
    public state state_ = state.Idle;

    public enum State
    {
        Run,     //通常
        Dash,     //ダッシュ
        None,     //動けない
    }

    //2型の作成
    public State type_ = State.Run;

    //初めてボムを投げたときにtrueになる
    [SerializeField]
    public bool first_bomb_ = false;

    //初めてタンクが拾われたときtrueになる
    [SerializeField]
    public bool first_tank_ = false;

    //Fellowへの参照用
    public float Player_Move_
    {
        get { return script_player_move_.Player_move; }
    }


    [SerializeField, Tooltip("ダメージをくらったときに動けなくなる時間")]
    private float knockback_stan_ = 3.0f;


    [SerializeField]
    private float rescue_gauge_tmp_ = 0;

    [SerializeField]
    private float rescue_gauge_ = 5.0f;

    //脱出フラグ
    [SerializeField]
    private bool escape_ui_flg_;

    public bool escape_ui_Glg_
    {
        get { return escape_ui_flg_; }
    }


    void Start()
    {
        first_bomb_ = false;
        first_tank_ = false;

        GameProgress.instance_.SetPlayer(gameObject);

        PLAYER _param = GameProgress.instance_.GetParameters().player;

        float _pos_x = _param.pos_x;
        float _pos_y = _param.pos_y;
        float _pos_z = _param.pos_z;

        this.transform.position = new Vector3(_pos_x, _pos_y, _pos_z);
    }

    // Update is called once per frame
    void Update()
    {


        TypeMove();
        Type();

        //無敵時間のカウント
        if (player_life_inv_tmp_ > 0)
        {
            player_life_inv_tmp_ -= 1.0f * Time.deltaTime;
        }

        //0になったらBloodステートに移動
        if (!script_debug_.DebugMode)
        {
            if (script_player_oxy_.oxy_Total <= 0.01f && state_ != state.Death && state_ != state.Damage && state_ != state.Menu)
            {

                state_ = state.Blood;

                //走れなくする
                type_ = State.Run;
                //// 齋藤の追加ソース
                bloodDirection_.OxyEmpty();
            }
        }
    }

    void TypeMove()
    {
        //動いてる時の状態
        switch (type_)
        {
            case State.Run://(idleに戻らない)
                {
                    script_player_oxy_.dash_flg_ = false;
                    Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    script_player_move_.Move(false, _stick_left);
                    script_player_oxy_.Oxy();
                    break;
                }

            case State.Dash:
                {
                    script_player_oxy_.dash_flg_ = true;
                    Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    script_player_move_.Move(true, _stick_left);
                    script_player_oxy_.Oxy();


                    break;
                }
            case State.None:
                {
                    script_player_oxy_.dash_flg_ = false;
                    break;
                }
        }
    }

    void Type()
    {
        switch (state_)
        {
            case state.Idle:
                {
                    if (Input.GetButton("Fire2"))
                    {

                        type_ = State.Dash;
#if UNITY_EDITOR
                        Debug.Log("Bボタンが押された");
#endif
                    }
                    else if (!Input.GetButton("Fire2"))
                    {
                        type_ = State.Run;
#if UNITY_EDITOR
                        Debug.Log("Bボタンが離された");
#endif

                    }

                    //ほかステートへの分岐

                    //Xボタンが押されら
                    if (Input.GetButtonDown("Fire3"))
                    {

                        state_ = state.Action_Bomb;
                    }

                    break;
                }
            case state.Action_Bomb:
                {
                    //爆弾をためているときは走れない
                    type_ = State.Run;

                    if (Input.GetButton("Fire3"))
                    {
                        script_player_oxybomb_.PressCount();

                    }

                    if (Input.GetButtonUp("Fire3"))
                    {
                        //初めて酸素爆弾を使った
                        if (!first_bomb_)
                        {
                            first_bomb_ = true;
                        }
                        script_player_oxybomb_.Bomb();
                        script_player_oxybomb_.CountReset();

                        state_ = state.Idle;
                    }
                    //キャンセル
                    if (Input.GetButtonDown("Fire1"))
                    {
                        script_player_oxybomb_.CountReset();
                        state_ = state.Idle;
                    }
                    break;
                }
            case state.Blood:
                {
                    //// 齋藤の追加ソース
                    //bloodDirection_.OxyEmpty();
                    //酸素がないときは走れない
                    type_ = State.Run;
                    break;
                }
            case state.Damage:
                {
                    type_ = State.None;
                    break;

                }
            case state.Rescue:
                {
                    type_ = State.None;
                    break;
                }
            case state.Menu:
                {
                    script_player_move_.ResetAnimator();
                    type_ = State.None;
                    break;
                }
        }
    }

    //味方が敵に当たった時の処理
    public void FellowHit()
    {
        //無敵時間開始
        player_life_inv_tmp_ = player_life_inv_time_;
        script_player_count_.fellow_count_ -= 1;
        //bloodDirection_.DamageDone();
        sePlayer_.TakeDamage();
    }

    //タンク追加フラグ
    public void FellowOxyAdd()
    {
        script_player_oxy_.OxyAdd();
    }
    //ボム強化フラグ
    public void FellowOxyBomb()
    {
        script_player_oxybomb_.OxyBombAdd();
    }

    //ゲームオーバー時に呼び出し
    public void GameOver()
    {
        state_ = state.Damage;
        //type = state.Death;
        script_player_move_.ResetAnimator();
    }

    public void EnemyHit(Vector3 _transform)
    {
        //無敵時間がないとき
        if (player_life_inv_tmp_ <= 0)
        {
            //酸素ボンベの残量がないときはダメージを与えない
            if (state_ != state.Blood)
            {
                //酸素ボンベにダメージを与える
                script_player_oxy_.OxyDamage();
                bloodDirection_.DamageDone();
            }
            //ダメージステート
            state_ = state.Damage;

            StartCoroutine(OnDamage());

            player_life_inv_tmp_ = player_life_inv_time_;
            sePlayer_.TakeDamage();
            script_player_move_.KnockBack(_transform);
        }
    }

    //スタン処理用
    IEnumerator OnDamage()
    {
        script_player_move_.ResetAnimator();

        yield return new WaitForSeconds(knockback_stan_);

        if (script_player_oxy_.oxy_Total >= 0.0f)
        {

            state_ = state.Idle;


        }
        else
        {

            state_ = state.Blood;
        }

    }

    public void GetTank()
    {
        sePlayer_.TankGet();

        if (!first_tank_)
        {
            first_tank_ = true;
        }

       
        //もしstateの状態がbloodだったら
        if (state_ == state.Blood)
        {
           
            script_player_oxy_.OxyRecoveryBlood();
            state_ = state.Idle;

        }

        else
        {
            script_player_oxy_.OxyRecovery();
        }
    }   

    //仲間を潜水艦に回収中
    public void Rescue()
    {
        //長押し
        if (Input.GetButton("Fire1"))
        {
            //ステートをRescue中にする
            state_ = state.Rescue;

            rescue_gauge_tmp_ += Time.deltaTime;

            UI_.recovery_UI_.SetActive(true);
            UI_.rescue_gauge_UI.SetActive(true);
            UI_.rescue_gauge_.value = rescue_gauge_tmp_;


            if (rescue_gauge_ <= rescue_gauge_tmp_)
            {
                rescue_gauge_tmp_ = 0;
                state_ = state.Idle;

            
                //仲間を回収
                Debug.Log("仲間を回収した");
                script_player_count_.fellow_rescue_ += script_player_count_.fellow_count_;
                script_player_count_.fellow_count_ = 0;

                if (script_player_count_.fellow_rescue_ <= 10)
                {
                    escape_ui_flg_ = true;
                }

                UI_.recovery_UI_.SetActive(false);
                UI_.rescue_gauge_UI.SetActive(false);
            }

        }
        else
        {
            state_ = state.Idle;
            UI_.CloseUI();
            rescue_gauge_tmp_ = 0;
        }         
    }

    //潜水艦メニューが開かれたときに呼び出し
    public void OpenMenu()
    {
        state_ = state.Menu;
        

    }
    //潜水艦メニューが閉じられたときに呼び出し
    public void CloseMenu()
    {
        state_ = state.Idle;

    }
}

