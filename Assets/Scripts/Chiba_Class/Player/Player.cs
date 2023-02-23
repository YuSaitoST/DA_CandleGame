using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�f�o�b�N�p")]
    [SerializeField]
    private PlayerDebugMode script_debug_ = null;

    [Space(10)]

    [Header("Player�N���X")]
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
    [SerializeField, Tooltip("BloodDirection���A�^�b�`")]
    BloodDirection bloodDirection_ = null;

    [Header("SE")]
    [SerializeField, Tooltip("SE�v���C���[")]
    PlayersSEPlayer sePlayer_ = null;

    [Space(10)]
    [SerializeField, Tooltip("���G����")]
    private float player_life_inv_time_ = 3.0f;

    //���G����
    [SerializeField]
    private float player_life_inv_tmp_ = 0.0f;

    public float Player_life_inv_tmp_
    {
        get { return player_life_inv_tmp_; }
    }
    public enum state
    {
        Idle,     //�ʏ�
        Action00, //�A�C�e�����E��
        Action_Bomb, //�_�f���e����
        Damage,   //�_���[�W���炤
        Blood,    //�_�f���Ȃ��Ƃ�
        Menu,     //�����̓��j���[���쒆
        Rescue,   //�����͂ɒ��Ԃ����e��
        Escape,   //�E�o���邩�̑I�����
        Death     //��
    }
    public state state_ = state.Idle;

    public enum State
    {
        Run,     //�ʏ�
        Dash,     //�_�b�V��
        None,     //�����Ȃ�
    }

    //2�^�̍쐬
    public State type_ = State.Run;

    //���߂ă{���𓊂����Ƃ���true�ɂȂ�
    [SerializeField]
    public bool first_bomb_ = false;

    //���߂ă^���N���E��ꂽ�Ƃ�true�ɂȂ�
    [SerializeField]
    public bool first_tank_ = false;

    //Fellow�ւ̎Q�Ɨp
    public float Player_Move_
    {
        get { return script_player_move_.Player_move; }
    }


    [SerializeField, Tooltip("�_���[�W����������Ƃ��ɓ����Ȃ��Ȃ鎞��")]
    private float knockback_stan_ = 3.0f;


    [SerializeField]
    private float rescue_gauge_tmp_ = 0;

    [SerializeField]
    private float rescue_gauge_ = 5.0f;

    //�E�o�t���O
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

        //���G���Ԃ̃J�E���g
        if (player_life_inv_tmp_ > 0)
        {
            player_life_inv_tmp_ -= 1.0f * Time.deltaTime;
        }

        //0�ɂȂ�����Blood�X�e�[�g�Ɉړ�
        if (!script_debug_.DebugMode)
        {
            if (script_player_oxy_.oxy_Total <= 0.01f && state_ != state.Death && state_ != state.Damage && state_ != state.Menu)
            {

                state_ = state.Blood;

                //����Ȃ�����
                type_ = State.Run;
                //// �V���̒ǉ��\�[�X
                bloodDirection_.OxyEmpty();
            }
        }
    }

    void TypeMove()
    {
        //�����Ă鎞�̏��
        switch (type_)
        {
            case State.Run://(idle�ɖ߂�Ȃ�)
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
                        Debug.Log("B�{�^���������ꂽ");
#endif
                    }
                    else if (!Input.GetButton("Fire2"))
                    {
                        type_ = State.Run;
#if UNITY_EDITOR
                        Debug.Log("B�{�^���������ꂽ");
#endif

                    }

                    //�ق��X�e�[�g�ւ̕���

                    //X�{�^�����������
                    if (Input.GetButtonDown("Fire3"))
                    {

                        state_ = state.Action_Bomb;
                    }

                    break;
                }
            case state.Action_Bomb:
                {
                    //���e�����߂Ă���Ƃ��͑���Ȃ�
                    type_ = State.Run;

                    if (Input.GetButton("Fire3"))
                    {
                        script_player_oxybomb_.PressCount();

                    }

                    if (Input.GetButtonUp("Fire3"))
                    {
                        //���߂Ď_�f���e���g����
                        if (!first_bomb_)
                        {
                            first_bomb_ = true;
                        }
                        script_player_oxybomb_.Bomb();
                        script_player_oxybomb_.CountReset();

                        state_ = state.Idle;
                    }
                    //�L�����Z��
                    if (Input.GetButtonDown("Fire1"))
                    {
                        script_player_oxybomb_.CountReset();
                        state_ = state.Idle;
                    }
                    break;
                }
            case state.Blood:
                {
                    //// �V���̒ǉ��\�[�X
                    //bloodDirection_.OxyEmpty();
                    //�_�f���Ȃ��Ƃ��͑���Ȃ�
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

    //�������G�ɓ����������̏���
    public void FellowHit()
    {
        //���G���ԊJ�n
        player_life_inv_tmp_ = player_life_inv_time_;
        script_player_count_.fellow_count_ -= 1;
        //bloodDirection_.DamageDone();
        sePlayer_.TakeDamage();
    }

    //�^���N�ǉ��t���O
    public void FellowOxyAdd()
    {
        script_player_oxy_.OxyAdd();
    }
    //�{�������t���O
    public void FellowOxyBomb()
    {
        script_player_oxybomb_.OxyBombAdd();
    }

    //�Q�[���I�[�o�[���ɌĂяo��
    public void GameOver()
    {
        state_ = state.Damage;
        //type = state.Death;
        script_player_move_.ResetAnimator();
    }

    public void EnemyHit(Vector3 _transform)
    {
        //���G���Ԃ��Ȃ��Ƃ�
        if (player_life_inv_tmp_ <= 0)
        {
            //�_�f�{���x�̎c�ʂ��Ȃ��Ƃ��̓_���[�W��^���Ȃ�
            if (state_ != state.Blood)
            {
                //�_�f�{���x�Ƀ_���[�W��^����
                script_player_oxy_.OxyDamage();
                bloodDirection_.DamageDone();
            }
            //�_���[�W�X�e�[�g
            state_ = state.Damage;

            StartCoroutine(OnDamage());

            player_life_inv_tmp_ = player_life_inv_time_;
            sePlayer_.TakeDamage();
            script_player_move_.KnockBack(_transform);
        }
    }

    //�X�^�������p
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

       
        //����state�̏�Ԃ�blood��������
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

    //���Ԃ�����͂ɉ����
    public void Rescue()
    {
        //������
        if (Input.GetButton("Fire1"))
        {
            //�X�e�[�g��Rescue���ɂ���
            state_ = state.Rescue;

            rescue_gauge_tmp_ += Time.deltaTime;

            UI_.recovery_UI_.SetActive(true);
            UI_.rescue_gauge_UI.SetActive(true);
            UI_.rescue_gauge_.value = rescue_gauge_tmp_;


            if (rescue_gauge_ <= rescue_gauge_tmp_)
            {
                rescue_gauge_tmp_ = 0;
                state_ = state.Idle;

            
                //���Ԃ����
                Debug.Log("���Ԃ��������");
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

    //�����̓��j���[���J���ꂽ�Ƃ��ɌĂяo��
    public void OpenMenu()
    {
        state_ = state.Menu;
        

    }
    //�����̓��j���[������ꂽ�Ƃ��ɌĂяo��
    public void CloseMenu()
    {
        state_ = state.Idle;

    }
}

