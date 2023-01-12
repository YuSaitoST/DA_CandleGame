using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//������x�`�ɂ�����֐��������Ĉ�ɂ܂Ƃ߂�\��
public class Player : MonoBehaviour
{
    [Header("�f�o�b�N�p")]
    [SerializeField,Tooltip("true�ŃQ�[���I�[�o�[�ɂȂ�Ȃ��Ȃ�܂�")]
    private bool debug_death_ = false;

    Transform tr_;
    Rigidbody rb_;

    [Header("�v���C���[�̋���")]

    [SerializeField,Tooltip("�v���C���[�̈ړ����x(�����l)")]
    private float    player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("�u�[�X�g���̈ړ����x(�ő�l)")]
    private float    player_move_boost_ = 1.0f;

    [SerializeField, Tooltip("���݂̂̈ړ����x(�ő�l)")]
    private float    player_move_ = 1.0f;

    public float Player_Move_
    {
        get { return player_move_; }
    }

    [SerializeField, Tooltip("�v���C���[�̃A�j���[�^�[������")]
    private Animator player_animator_ =null;

    [SerializeField]
    private bool     player_move_flg_ = true;//�v���C���[�̈ړ���~�ptrue�ňړ���

    [SerializeField, Tooltip("�v���C���[�̗̑�(�}�e���A���̕s�����x�ŕ\��)���g�p")]
    private float player_life_ = 100.0f; 

    [SerializeField,Tooltip("���G����")]
    private float player_life_inv_time_ = 3.0f;

    //���G����
    private float player_life_inv_tmp_ = 0.0f;

    public float Player_life_inv_tmp_
    {
        get { return player_life_inv_tmp_; }
    }

    //state
    public enum State
    {
        Idle,     //�ʏ�
        Dash,     //�ړ�(����)
        Action00, //�A�C�e�����E��
        Action01, //�����n��
        Action02, //�|���v�����ҋ@
        Action03, //�|���v����
        Action04, //�|���v���Ƃ�
        Damage,   //�_���[�W���炤
        Blood,    //�_�f���Ȃ��Ƃ�
        Death     //��
    }
    //2�^�̍쐬
    public State type_ = State.Idle;

    [Header("�_�f�Q�[�W�֘A")]
    //[SerializeField,Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    //private float    oxy_           = 0.0f;

    [SerializeField, Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float    oxy_recovery_ = 33.3f;

    [SerializeField,Tooltip("������Ȃ�")]
    private int      oxy_count_ =0;

    [SerializeField, Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float    oxy_total_ = 0.0f;

    //[SerializeField, Tooltip("�_�f�Q�[�W1�{�̍ő�l(�����l)"), Range(0, 33.3f)]
    private float[]  oxy_max_       =  new float[10];

    //[SerializeField, Tooltip("�_�f�Q�[�W1�{�̍ő�l(�����l)"), Range(0, 33.3f)]
    private float[] oxy_max_red_ = new float[10];

    //�ŏ��̃^���N�̐�
    [SerializeField]
    private int oxy_start_ = 3;

    //�ǉ��^���N�t���O
    public bool fellow_oxy_add_ = false;

    //���e�����t���O
    public bool fellow_oxy_bomb_ = false;

    [SerializeField, Tooltip("�u�[�X�g���̃Q�[�W����{��")]
    private float    oxy_cost_boost_ = 2.0f;

    [SerializeField, Tooltip("���펞�̏���_�f")]
    private float    oxy_cost_      = 1.0f;

    [SerializeField, Tooltip("�_�f�Q�[�W�̒l�\���p�e�L�X�g")]
    private TMP_Text oxy_text_      = null;

    [SerializeField, Tooltip("�{���x�̐�")]
    private Slider[] oxy_slider_ = new Slider[3];

    [SerializeField, Tooltip("�{���x�̐�")]
    private Slider[] oxy_slider_red_ = null;

    [SerializeField]
    private GameObject oxy_add_slider_ = null;

    [Header("A�{�^���֘A")]
    [SerializeField,Tooltip("debug�p")]
    private bool fire1_range_flg_ = false;//�v���C���[���͈͓��ɓ�������true�ɂȂ�

    [SerializeField]
    private bool fire1_cancel_ = false;

    [Header("B�{�^���֘A")]

    [SerializeField]
    private bool fire2_flg_ = false;

    public bool Fire2_Flg__
    {
        get { return fire2_flg_; }
    }

    [Header("X�{�^���֘A")]
    [SerializeField]
    private bool fire3_flg_ = false;

    private int count_ = 0;

    //�������b���̎擾
    [SerializeField]
    private float fire3_button_count_ = 0;

    //�_�f�����
    [SerializeField]
    private float[] fire3_button_cost_ = new float[] { 5, 8, 11, 14, 17, 20};

    [SerializeField, Tooltip("�e��Prefab")]
    private GameObject[] fire3_tank_prefab_ = new GameObject[] {null};
   
    private DrawArc fire3_draw_;

    [SerializeField, Tooltip("���e�����̊J�n�_")]
    private GameObject fire3_point_;

    [SerializeField, Tooltip("���e�𗎂Ƃ��ʒu")]
    private GameObject fire3_drop_;

    //�v���n�u�̎ˏo�ʒu
    private Vector3 fire3_drop_pos_;

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(0.0f, 20.0f), Tooltip("�e�̎ˏo���鑬��")]
    private float fire3_speed_ = 1.0f;


    [SerializeField, Range(0.0f, 1.0f), Tooltip("�e�̎ˏo���鍂��")]
    private float fire3_speed2_ = 0.42f;

    // �e�̏����x
    private Vector3 shootVelocity_;
 
    // �e�̏����x(�ǂݎ���p)
    public Vector3 ShootVelocity_
    {
        get { return shootVelocity_; }
    }

    [Header("�t�B�[���h�ړ�����(�����A���)")]
    [SerializeField,Tooltip("X���̐���")]
    private Vector2 x_clip_ = new(-9, 9);

    [SerializeField, Tooltip("Y���̐���")]
    private Vector2 y_clip_ = new(-200, 200);

    [SerializeField, Tooltip("Z���̐���")]
    private Vector2 z_clip_ = new(-5, 5);

    //[SerializeField, Tooltip("���j���[���J�����Ƃ��ɍŏ��ɑI�������{�^��")]
    //private GameObject ui_button_firstSelect_;

    [Header("�_���[�W�ݒ�Ȃ�")]
    [SerializeField, Tooltip("�_�f�Q�[�W�ւ̃_���[�W��")]
    private float damage_ = 10.0f;

    [SerializeField, Tooltip("�m�b�N�o�b�N���̗�")]
    private float knockback_power_ =0.8f;

    [SerializeField, Tooltip("�m�b�N�o�b�N���̏�ւ̗�")]
    private float knockback_power_up_ = 0.7f;

    [SerializeField, Tooltip("�����Ȃ�����")]
    private float knockback_stan_ = 1.0f;

    //�����̓��~�b�g
    [SerializeField,Tooltip("�����͒��ԉ���̃Q�[�W")]
    private Slider submarine_slider_ = null;
    [SerializeField, Tooltip("���ԉ���̃Q�[�W�̃L�����o�X")]
    private Canvas submarine_slider_canvas_ = null;
    [SerializeField]
    private float submarine_limit_ = 5.0f;

    private float submarine_limit_tmp_ = 0.0f;
  
    //ButtonUI
    //������
    [SerializeField]
    private Canvas submarine_ui_ = null;
    //�{�^��
    [SerializeField]
    private Canvas item_ui_ = null;

    //�A�N�V�����L�����Z���{�^��
    [SerializeField]
    private Canvas cancel_ui_ = null;

    [SerializeField]
    private Canvas fellow_ui_ = null;

    [SerializeField]
    private Canvas recovery_ui_ = null;

    [SerializeField]
    private int fellow_count_ = 0;

    [SerializeField]
    private int fellow_rescue_ = 0;

   //�ǂݎ��p
    public int fellow_Count_
    {
        get { return fellow_count_; }
    }

    public int fellow_die_row_ = 0;

    [SerializeField,Tooltip("BloodDirection���A�^�b�`")]
    BloodDirection bloodDirection_ = null;


    [Header("SE")]
    [SerializeField, Tooltip("SE�v���C���[")]
    PlayersSEPlayer sePlayer_ = null;

    void Start()
    {


        oxy_add_slider_.SetActive(false);

        // �e�̏����x�␶�����W�����R���|�[�l���g
        fire3_draw_ = GetComponent<DrawArc>();

        float _h = 100 / oxy_start_;
        for (int i = 0; i < oxy_start_; i++)
       {
            
            oxy_max_[i] = _h;
            oxy_max_red_[i] = _h;

        }

        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        player_move_= player_move_speed_;
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];

        ////���G���Ԃ���X�^�����ԕ�������
        //player_life_inv_time_ -= knockback_stan_;

        submarine_slider_canvas_.enabled = false; 

        //ButtonUI
        //������
        submarine_ui_.enabled = false;
        //�{�^��
        item_ui_.enabled = false;
        //�A�N�V�����L�����Z���{�^��
        cancel_ui_.enabled = false;
        
        PLAYER _param = GameProgress.instance_.GetParameters().player;

        player_move_speed_  = _param.move_speed;
        player_move_boost_  = _param.move_boost;
        oxy_cost_boost_     = _param.oxy_cost_boost;
        oxy_cost_           = _param.oxy_cost;
        damage_             = _param.damage;
        knockback_power_    = _param.knockback_power;
        knockback_power_up_ = _param.knockback_power_up;
        knockback_stan_     = _param.knockback_stan_;

        float _pos_x = _param.pos_x;
        float _pos_y = _param.pos_y; 
        float _pos_z = _param.pos_z;

        this.transform.position = new Vector3(_pos_x, _pos_y, _pos_z);
    }

    // Update is called once per frame
    void Update()
    {
       

        //�v���C���[����
        PlayerInput();

        //����
        if (fellow_oxy_add_)
        {
            oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2]+ oxy_max_[3];

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



        oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("��")*/);

     
        submarine_slider_.value = submarine_limit_tmp_;
        // �e�̏����x���X�V
        shootVelocity_ = fire3_point_.transform.up * fire3_speed_;

        shootVelocity_.y = fire3_speed2_;

        // �e�̐������W���X�V
        instantiatePosition_ = fire3_point_.transform.position;
        fire3_drop_pos_ = fire3_drop_.transform.position;

        //0�ɂȂ�����Blood�X�e�[�g�Ɉړ�
        if (debug_death_==false) 
        {
            if (oxy_total_ <= 0.01f)
            {

                type_ = State.Blood;

                // �V���̒ǉ��\�[�X
                bloodDirection_.OxyEmpty();
            }
        }

        //�{���x�̃Q�[�W��0�ɂȂ����玟�̃{���x�ɐ؂�ւ���
        if(fellow_oxy_add_)
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 3)
            {
                oxy_max_red_[oxy_count_] = 0.0f;
                oxy_max_[oxy_count_] = 0.0f;
                //�ʃN���X�Ăяo��
                fire3_draw_.Off();

                oxy_count_++;
                Debug.Log(oxy_count_);
            }
        }
        else
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 2)
            {
                oxy_max_red_[oxy_count_] = 0.0f;
                oxy_max_[oxy_count_] = 0.0f;
                //�ʃN���X�Ăяo��
                fire3_draw_.Off();

                oxy_count_++;
                Debug.Log(oxy_count_);
            }
        }
        //4�{�ڂ̃{���x��ǉ�
        if (fellow_oxy_add_)
        {
            if (!oxy_add_slider_.activeSelf)
            {
                oxy_max_[3] = 33;
            }
            oxy_add_slider_.SetActive(true);

            //4�{�ڂ̃{���xUI��\��
            //float _tmp = oxy_max_[oxy_count_];
            //oxy_max_[oxy_count_] = 33;
           
            // oxy_max_[4] = 33;



        }
        //EventSystem.current.SetSelectedGameObject(button_firstSelect_);
    }
    
    void PlayerInput()
    {
        //�v���C���[�̓���

        

        //B�{�^��
        //�ړ����x�㏸
        if (Input.GetButton("Fire2")&&type_!=State.Damage && type_ != State.Action00 && type_ != State.Blood)
        {
            fire2_flg_ = true;
            type_ = State.Dash;
            Debug.Log("B�{�^���������ꂽ");
           
        }
        else if(Input.GetButtonUp("Fire2") && type_ != State.Damage && type_ != State.Action00 && type_ != State.Blood)
        {
            fire2_flg_ = false;
            type_ = State.Idle;
            Debug.Log("B�{�^���������ꂽ");
            
        }

        //X�{�^��
        //�{���x�A�N�V����
        if (/*oxy_count_ != 2 &&*/ oxy_max_[2] >= fire3_button_cost_[0] && type_ != State.Damage && type_ != State.Action00 && type_ != State.Blood
            || fellow_oxy_add_ && oxy_max_[3] >= fire3_button_cost_[0] && type_ != State.Damage && type_ != State.Action00 && type_ != State.Blood)
        {

            if (Input.GetButton("Fire3")&&fire1_cancel_ == false )
            {
                
                //���߂Ă��Ԃ��킩��₷������
                
                fire3_button_count_ += Time.deltaTime;
                type_ = State.Action01;
                if (fire3_button_count_ >= 1.0f)
                {
                  
                    type_ = State.Action02;
                }

                //�A�N�V�������L�����Z��(A�{�^��)
                if(Input.GetButtonDown("Fire1"))               
                {
                    fire1_cancel_ = true;
                    cancel_ui_.enabled = false;
                    fire3_button_count_ = 0;
                    //�ʃN���X�Ăяo��
                    fire3_draw_.Off();
                    //����
                    type_ = State.Idle; //idle�Ɉڍs
                }
            }
            if (Input.GetButtonUp("Fire3"))
            {
                if (fire1_cancel_ == false)
                {
                    count_ = 0;
                    type_ = State.Idle; //idle�Ɉڍs

                    //���������Ԃ�1�b�����̏ꍇ
                    if (fire3_button_count_ < 1.0f)
                    {
                        type_ = State.Action04;//���Ƃ��X�e�[�g
                        Debug.Log("�A�N�V�������s04-1");
                    }
                    else
                    {
                        if (fire3_button_count_ >= 5.0f&&fellow_oxy_bomb_)
                        {
                            count_ = 5;
                            type_ = State.Action03;//������X�e�[�g
                            Debug.Log("�A�N�V�������s03-5");
                        }
                        else if (fire3_button_count_ >= 4.0f&& fellow_oxy_bomb_)
                        {
                            count_ = 4;
                            type_ = State.Action03;//������X�e�[�g
                            Debug.Log("�A�N�V�������s03-4");
                        }
                        else if (fire3_button_count_ >= 3.0f)
                        {
                            count_ = 3;
                            type_ = State.Action03;//������X�e�[�g
                            Debug.Log("�A�N�V�������s03-3");
                        }
                        else if (fire3_button_count_ >= 2.0f)
                        {
                            count_ = 2;
                            type_ = State.Action03;//������X�e�[�g
                            Debug.Log("�A�N�V�������s03-2");
                        }
                        else if (fire3_button_count_ >= 1.0f)
                        {
                            count_ = 1;
                            type_ = State.Action03;//������X�e�[�g
                            Debug.Log("�A�N�V�������s03-1");


                        }
                    }
                }
                fire1_cancel_ = false;
                cancel_ui_.enabled = false;
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
                                     
                    Move();
                    //����

                }
                break;
            
            case State.Dash://����(�����idle�ɖ߂�Ȃ�)
                {
                    //�ʃN���X�Ăяo��
                    fire3_draw_.Off();
                    

                    //�u�[�X�g���͎_�f����ʂ��㏸����
                    oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;
                   
                    Move();       
                    
                }
                break;

            case State.Action00: //�p�[�c��ݒu(�����idle�ɖ߂�Ȃ�)
                {        
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //����
                    //�A�j���[�V����
                    Action00();
                }
                break;
            case State.Action01: //X�{�^���J�n(�����idle�ɖ߂�Ȃ�)
                {      
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    Move();
                    //����
                   

                }
                break;
            case State.Action02://X�{�^�������ҋ@(�����idle�ɖ߂�Ȃ�)
                {                   
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //����
                    Move();
                    Action02();
                }
                break;
            case State.Action03://������
                {
                    
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //����
                    Action03();
                    type_ =State.Idle;
                    
                }
                break;
            case State.Action04://���Ƃ�
                {

                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //����
                    Action04();
                    type_ = State.Idle;

                }
                break;
            case State.Damage://�_���[�W���炤
                {

                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //����
                    Damage();                
                }
                break;
            case State.Blood://�_���[�W���炤
                {
                    fire3_draw_.Off();
                    //����
                    Move();
                }
                break;

            case State.Death://(idle�ɖ߂�Ȃ�)
                {
                    
                   
                   
                    //oxy��0�ɂȂ�����Q�[���I�[�o�[
                    Debug.Log("�Q�[���I�[�o�[");
                    //����
                    //�A�j���[�^�[���Z�b�g
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

        //�G�ɓ����������̏���
       
        if (player_life_inv_tmp_ > 0)
        {
            player_life_inv_tmp_ -= 1.0f*Time.deltaTime;
        }

        //�ԃQ�[�W�֘A
        if (oxy_max_red_[oxy_count_] > oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] -= 9.0f * Time.deltaTime;

        }
        else if (oxy_max_red_[oxy_count_] < oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] = oxy_max_[oxy_count_];

        }
    }
    private void Action00()
    {
        //�A�j���[�^�[���Z�b�g
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);

        if (submarine_limit_ >= submarine_limit_tmp_)
        {
            submarine_limit_tmp_ += Time.deltaTime;
        }
        else if (submarine_limit_ <= submarine_limit_tmp_)
        {
            recovery_ui_.enabled = false;
            //submarine_ui_.enabled = false;
            submarine_limit_tmp_ = 0;
            type_ = State.Idle;
           // PartsManager.Instance.submarine();
            submarine_slider_canvas_.enabled = false;

            //���Ԃ����
            Debug.Log("���Ԃ��������");
            fellow_rescue_ += fellow_count_;
            fellow_count_ = 0;
        }
       
        

    }
    private void Action02()
    {
        //�ʃN���X�Ăяo��
        fire3_draw_.On();
        cancel_ui_.enabled = true;
        submarine_ui_.enabled = false;

    }

    private void Action03()
    {
       

        //�ʃN���X�Ăяo��
        fire3_draw_.Off();

        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] -fire3_button_cost_[count_];
        if (_tmp < 0.0f)
        {
            oxy_max_red_[oxy_count_] = 0;
            oxy_max_[oxy_count_] = 0;
            if (debug_death_ == false)
            {
                oxy_count_++;
            }
            
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= fire3_button_cost_[count_];
        }
        // �e�𐶐����Ĕ�΂�
        GameObject _obj = Instantiate(fire3_tank_prefab_[count_], instantiatePosition_, Quaternion.identity);
        Rigidbody _ri = _obj.GetComponent<Rigidbody>();
        _ri.AddForce(shootVelocity_ * _ri.mass, ForceMode.Impulse);

        Debug.Log("�A�N�V�������s03-2");

        //UI���\���ɂ���
        cancel_ui_.enabled = false;
    }
    private void Action04()
    { 
        //�ʃN���X�Ăяo��
        fire3_draw_.Off();

        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] - fire3_button_cost_[count_];
        if (_tmp < 0.0f)
        {
            oxy_max_[oxy_count_] = 0;
            if (debug_death_ == false)
            {
                oxy_count_++;
            }
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= fire3_button_cost_[count_];
        }
       
        // �e�𐶐����Ĕ�΂�
        GameObject _obj = Instantiate(fire3_tank_prefab_[0], fire3_drop_pos_, Quaternion.identity);
       
        Debug.Log("�A�N�V�������s04");

        //UI���\���ɂ���
        cancel_ui_.enabled = false;
    }

    private void Damage()
    {
        StartCoroutine(OnDamage());
    }

    IEnumerator OnDamage()
    {
        bloodDirection_.DamageDone();
        //�ʃN���X�Ăяo��
        fire3_draw_.Off();

        //�A�j���[�^�[���Z�b�g
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);

        yield return new WaitForSeconds(knockback_stan_);
        type_ = State.Idle; 
        
        
    }



        //�ړ�����
        private void Move()
    {
        Vector3 _position   = Vector3.zero;
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //�ړ��u�[�X�g
        if (type_==State.Dash)
        {
            if (player_move_<= player_move_boost_)
            {
                player_animator_.SetBool("isRunning", true);
                //���X�ɑ�����Ă���
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
                //���X�ɂЂ���Ă���
                player_move_ -= 0.1f * Time.deltaTime;
            }
            else 
            {
                player_move_= player_move_speed_;
            }
           
        }

        //�ړ�����
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

            // �X�e�B�b�N���|��Ă���΁A�|��Ă������������
            var _direction2 = new Vector3(_stick_left.x, 0, _stick_left.y);
            transform.localRotation = Quaternion.LookRotation(_direction2);

        }
        else
        {
            player_animator_.SetBool("isWalking", false);

        }
    }
    //�_���[�W����
    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.tag == "Enemy"&& player_life_inv_tmp_ <= 0)
        {
            rb_.velocity = Vector3.zero;
            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu���v�Z
            Vector3 _distination = (transform.position - collision.transform.position).normalized;

            rb_.AddForce(_distination * knockback_power_, ForceMode.VelocityChange);
            rb_.AddForce(transform.up * knockback_power_up_, ForceMode.VelocityChange);

            if (type_ != State.Blood)
            {
                type_ = State.Damage;
                //�_���[�W�H�炤
                oxy_max_[oxy_count_] -= damage_;
            }
            else 
            {

            }
            //���G���ԊJ�n
            player_life_inv_tmp_ = player_life_inv_time_;

          

            sePlayer_.TakeDamage();
        }
    }

    //�������G�ɓ����������̏���
    public void FellowHit()
    {
        //���G���ԊJ�n
        player_life_inv_tmp_ = player_life_inv_time_;
        fellow_count_ -= 1;
        //bloodDirection_.DamageDone();
        sePlayer_.TakeDamage();
    }


    private void OnTriggerStay(Collider other)
    {
      
        if (type_ == State.Idle || type_ == State.Dash|| type_ == State.Action00 || type_ == State.Blood)
        {
            //������
            //���ԉ��
            if (other.gameObject.CompareTag("Submarine") && fellow_count_ > 0)
            {

                recovery_ui_.enabled = true;


                if (Input.GetButton("Fire1"))
                {
                    submarine_slider_canvas_.enabled = true;
                    //�X�e�[�g�ڍs
                    type_ = State.Action00;
                }
                else
                {

                    submarine_limit_tmp_ = 0;
                    submarine_slider_canvas_.enabled = false;
                    type_ = State.Idle;
                }

            }

            //�p�[�c
            //if (other.gameObject.CompareTag("Submarine") && PartsManager.Instance.count_ > 0)
            //{

            //    submarine_ui_.enabled = true;


            //    if (Input.GetButton("Fire1"))
            //    {
            //        submarine_slider_canvas_.enabled = true;
            //        //�X�e�[�g�ڍs
            //        type_ = State.Action00;
            //    }
            //    else
            //    {

            //        submarine_limit_tmp_ = 0;
            //        submarine_slider_canvas_.enabled = false;
            //        type_ = State.Idle;
            //    }

            //}

            //���Ԃ̏���
            if (other.gameObject.CompareTag("RescueArea"))
            {
                fellow_ui_.enabled = true;

                fire1_range_flg_ = true;
                if (Input.GetButton("Fire1"))
                {
                    fellow_ui_.enabled = false;
                    fellow_count_ += 1;
                    var _fellow = other;
                    _fellow.GetComponent<RescueArea>().Follow();
                    _fellow = null;
                    sePlayer_.PartGet();
                }


            }

                //�p�[�c�͈̔�
            //    if (other.gameObject.CompareTag("PC"))
            //{

            //    item_ui_.enabled = true;

            //    fire1_range_flg_ = true;
            //    if (Input.GetButton("Fire1"))
            //    {
                  
            //        var _parts = other;
            //        _parts.GetComponent<Parts>().Pickup();
            //        _parts = null;
            //        sePlayer_.PartGet();
            //    }

            //}

            //�{���x��
            if (other.gameObject.CompareTag("Tank"))
            {
               
                item_ui_.enabled = true;
                
                fire1_range_flg_ = true;
                if (Input.GetButton("Fire1"))
                {
                   
                    //����state�̏�Ԃ�blood��������
                   
                    //1�{�ȏ����Ă���ꍇ
                    if (oxy_count_ >= 1)
                    {
                        if (type_ == State.Blood)
                        {
                            oxy_count_--;
                            oxy_max_[2] = 33.3f;
                            type_ = State.Idle;

                        }

                        else
                        {
                            oxy_max_[oxy_count_] += oxy_recovery_;

                            //1��
                            float _tmp = 0.0f;
                            _tmp = oxy_max_[oxy_count_] - 33.3f;
                            oxy_max_[oxy_count_] = 33.3f;
                            oxy_count_--;
                            oxy_max_[oxy_count_] = _tmp;

                        }

                        //2��
                        //oxy_count_--;
                        //oxy_max_[oxy_count_] += oxy_recovery_;

                        var _tank = other;
                        _tank.GetComponent<Tank>().Pickup();
                        _tank = null;
                    }
                    //1�{������Ă��Ȃ��ꍇ
                    else
                    {
                        oxy_max_[oxy_count_] = 33.3f;
                        var _tank = other;
                        _tank.GetComponent<Tank>().Pickup();
                        _tank = null;
                    }

                    bloodDirection_.DamageRecovery();
                    sePlayer_.TankGet();
                }

            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        //�A�C�e���͈̔͂���o��
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

        //������
        if (other.gameObject.CompareTag("Submarine"))
        {
            submarine_limit_tmp_ = 0;
            fire1_range_flg_ = false;
            submarine_ui_.enabled = false;
            recovery_ui_.enabled = false;
            submarine_slider_canvas_.enabled = false;
            if (type_ == State.Action00)
            {
                type_= State.Idle;
            }
        }

        if (other.gameObject.CompareTag("RescueArea"))
        {
            fellow_ui_.enabled = false;
            fire1_range_flg_ = false;

        }

    }
}
