using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//������x�`�ɂ�����֐��������Ĉ�ɂ܂Ƃ߂�\��
public class Player : MonoBehaviour
{
    Transform tr_;
    Rigidbody rb_;

    [Header("�v���C���[�̋���")]
    [SerializeField,Tooltip("�v���C���[�̈ړ����x(�����l)")]
    private float    player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("�u�[�X�g���̈ړ����x(�ő�l)")]
    private float    player_move_boost_ = 1.0f;

    [SerializeField, Tooltip("���݂̂̈ړ����x(�ő�l)")]
    private float    player_move_ = 1.0f;

    [SerializeField, Tooltip("�v���C���[�̃A�j���[�^�[������")]
    private Animator player_animator_ =null;

    [SerializeField]
    private bool     player_move_flg_ = true;//�v���C���[�̈ړ���~�ptrue�ňړ���

    [SerializeField, Tooltip("�v���C���[�̗̑�(�}�e���A���̕s�����x�ŕ\��)")]
    private float player_life_ = 100.0f; 

    [SerializeField]
    private float player_life_inv_time_ = 3.0f;

    //���G����
    private float player_life_inv_tmp_ = 0.0f;

    [Header("�_�f�Q�[�W�֘A")]
    //[SerializeField,Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    //private float    oxy_           = 0.0f;

    [SerializeField, Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float    oxy_recovery_ = 0.5f;

    [SerializeField,Tooltip("������Ȃ�")]
    private int      oxy_count_ =0;

    [SerializeField, Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float    oxy_total_ = 0.0f;

    //[SerializeField, Tooltip("�_�f�Q�[�W1�{�̍ő�l(�����l)"), Range(0, 33.3f)]
    private float[]  oxy_max_       =  new float[3];

    [SerializeField, Tooltip("�u�[�X�g���̃Q�[�W����{��")]
    private float    oxy_cost_boost_ = 2.0f;

    [SerializeField, Tooltip("���펞�̏���_�f")]
    private float    oxy_cost_      = 1.0f;

    [SerializeField, Tooltip("�_�f�Q�[�W�̒l�\���p�e�L�X�g")]
    private TMP_Text oxy_text_      = null;

    [SerializeField, Tooltip("�{���x�̐�")]
    private Slider[] oxy_slider_ = new Slider[3];

  

    [Header("A�{�^���֘A")]
    [SerializeField]
    private bool fire1_range_flg_ = false;//�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�

    [Header("B�{�^���֘A")]
    //[SerializeField]
    //private bool fire2_flg_ = false;


    [Header("X�{�^���֘A")]
    [SerializeField]
    private bool fire3_flg_ = false;

  

    //�������b���̎擾
    [SerializeField]
    private float fire3_button_count_ = 0.0f;

    [SerializeField, Tooltip("�e��Prefab")]
    private GameObject fire3_tank_prefab_;
    //bulletPrefab;

   
    private DrawArc fire3_draw_;

    [SerializeField, Tooltip("�C�g�̃I�u�W�F�N�g")]
    private GameObject fire3_point_;
    //barrelObject_

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(1.0F, 20.0F), Tooltip("�e�̎ˏo���鑬��")]
    private float fire3_speed_ = 1.0F;

   
    // �e�̏����x
    private Vector3 shootVelocity_;
 
    // �e�̏����x(�ǂݎ���p)
    public Vector3 ShootVelocity
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
    [SerializeField]
    private float damage_ = 10.0f;

    [SerializeField]
    private float knockback_power_ =0.8f;

    [SerializeField]
    private float knockback_power_up_ = 0.7f;

    [SerializeField]
    private float knockback_stan_ = 1.0f;

    //state
    public enum State
    {
        Idle,     //�ʏ�
        Dash,     //�ړ�(����)
        Action00, //�A�C�e�����E��
        Action01, //�|���v���Ƃ�
        Action02, //�|���v����
        Action03, //�p�[�c���E��
        Damage,   //�_���[�W���炤
        Death     //��
    }
    //2�^�̍쐬
    public State type_ = State.Idle;

    [SerializeField,Tooltip("BloodDirection���A�^�b�`")]
    BloodDirection bloodDirection_ = null;

    void Start()
    {
        // �e�̏����x�␶�����W�����R���|�[�l���g
        fire3_draw_ = gameObject.GetComponent<DrawArc>();

        for (int i = 0; i < oxy_max_.Length; i++)
       {
            oxy_max_[i] = 33.3f;
       }        

        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        player_move_= player_move_speed_;
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
        
        ////���G���Ԃ���X�^�����ԕ�������
        //player_life_inv_time_ -= knockback_stan_;

        

    }

    // Update is called once per frame
    void Update()
    {
       

        //�v���C���[����
        PlayerInput();

        //����
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
        oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("��")*/);

        oxy_slider_[0].value = oxy_max_[0];
        oxy_slider_[1].value = oxy_max_[1];
        oxy_slider_[2].value = oxy_max_[2];
        // �e�̏����x���X�V
        shootVelocity_ = fire3_point_.transform.up * fire3_speed_;

        // �e�̐������W���X�V
        instantiatePosition_ = fire3_point_.transform.position;

        //0�ɂȂ�����Q�[���I�[�o�[
        if (oxy_total_ <= 0.01f)
        {
            type_ = State.Death;
        }

        //�{���x�̃Q�[�W��0�ɂȂ����玟�̃{���x�ɐ؂�ւ���
        if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 2)
        {
            oxy_max_[oxy_count_] = 0.0f;
            //�ʃN���X�Ăяo��
            fire3_draw_.Off();

            oxy_count_++;
            Debug.Log(oxy_count_);
        }

        //if (Input.GetButtonDown("Fire2"))
        //{
        //    Debug.Log("B�{�^���������ꂽ");
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
    void PlayerInput()
    {
        //�v���C���[�̓���
        //A�{�^��
        //if (Input.GetButton("Fire1"))
        //{
        //    Debug.Log("A�{�^����������Ă���");
        //    //���e�ݒu
        //    if (fire1_range_flg_ == true)
        //    {

        //        player_move_flg_ = false;
        //    }

        //}
        //else if (Input.GetButtonUp("Fire1"))
        //{
        //    player_move_flg_ = true;
        //}

        //B�{�^��
        //�ړ����x�㏸
        if (Input.GetButton("Fire2")&&type_!=State.Damage)
        {
            type_ = State.Dash;
            Debug.Log("B�{�^���������ꂽ");
           
        }
        else if(Input.GetButtonUp("Fire2") && type_ != State.Damage)
        {
            type_ = State.Idle;
            Debug.Log("B�{�^���������ꂽ");
            
        }

        //X�{�^��
        //�{���x�A�N�V����
        if (oxy_count_ != 2 && type_ != State.Damage)
        {

            if (Input.GetButton("Fire3"))
            {
                //���߂Ă��Ԃ��킩��₷������
                //canvasOn
                fire3_button_count_ += Time.deltaTime;
                type_ = State.Action01;
                if (fire3_button_count_ >= 1.0f)
                {
                    //�ʃN���X�Ăяo��
                    fire3_draw_.On();

                }
            }
            if (Input.GetButtonUp("Fire3"))
            {

                if (fire3_button_count_ <= 1.0f)
                {

                    //oxy_max_[oxy_count_] = 0;
                    //oxy_count_++;

                    type_ = State.Action02;//�̂Ă�X�e�[�g
                    Debug.Log("�A�N�V�������s02-1");
                }
                else if (fire3_button_count_ >= 1.0f)
                {
                    

                    type_ = State.Action03;//������X�e�[�g
                    Debug.Log("�A�N�V�������s03-1");
                }

                //canvasOff
                

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
                    //fire2_flg_ = false;                    
                    Move();
                    //����

                }
                break;
            
            case State.Dash://����
                {
                    //�ʃN���X�Ăяo��
                    fire3_draw_.Off();

                    //�u�[�X�g���͎_�f����ʂ��㏸����
                    oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;
                   // fire2_flg_ = true;
                    Move();       
                    
                }
                break;

            case State.Action00: //�E��
                {
                  
                   
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    //����
                    //�A�j���[�V����
                    Action00();
                }
                break;
            case State.Action01: //X�{�^���ҋ@
                {

                   
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                    Move();
                    //����

                }
                break;
            case State.Action02://���Ƃ�
                {
                    
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
                  
                    //����
                    type_ = State.Idle; //idle�Ɉڍs
                }
                break;
            case State.Action03://������
                {
                    
                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;

                    //����
                    Action03();
                    type_ =State.Idle; //idle�Ɉڍs
                    
                }
                break;
            case State.Damage://�_���[�W���炤
                {

                    oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;


                    //����
                    Damage();
                    
                }
                break;

            case State.Death:
                {
                    //
                    GameProgress.instance_.GameOver();
                    //
                    //oxy��0�ɂȂ�����Q�[���I�[�o�[
                    Debug.Log("�Q�[���I�[�o�[");
                    //����
                    //�A�j���[�^�[���Z�b�g
                    player_animator_.SetBool("isRunning", false);
                    player_animator_.SetBool("isWalking", false);

                    //oxy_text_.enabled = false;
                    oxy_max_[0] = 0;
                    oxy_max_[1] = 0;
                    oxy_max_[2] = 0;
                    oxy_total_ = 0;
                    
                }
                break;
        }

        //�G�ɓ����������̏���
       
        if (player_life_inv_tmp_ > 0)
        {
            player_life_inv_tmp_ -= 1.0f*Time.deltaTime;
        }

    }
    private void Action00()
    {
        StartCoroutine(OnAction00());

    }

    IEnumerator OnAction00()
    {
      
       //yield return null;
        yield return new WaitForSeconds(0.5f);

        //�����ɍĊJ��̏���������
        type_ = State.Idle;
       
    }

    private void Action03()
    {
        //�ʃN���X�Ăяo��
        fire3_draw_.Off();

        //�{���x�������
        oxy_max_[oxy_count_] = 0;
        oxy_count_++;

        // �e�𐶐����Ĕ�΂�
        GameObject _obj = Instantiate(fire3_tank_prefab_, instantiatePosition_, Quaternion.identity);
        Rigidbody _rid = _obj.GetComponent<Rigidbody>();
        _rid.AddForce(shootVelocity_ * _rid.mass, ForceMode.Impulse);

        // 5�b��ɏ�����
        Destroy(_obj, 5.0F);
        Debug.Log("�A�N�V�������s02-2");
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
        type_ = State.Idle; //idle�Ɉڍs
        
        
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
            var direction2 = new Vector3(_stick_left.x, 0, _stick_left.y);
            transform.localRotation = Quaternion.LookRotation(direction2);

        }
        else
        {
            player_animator_.SetBool("isWalking", false);

        }
    }
    //�_���[�W����
    private void OnCollisionStay(Collision collision)
    {
        //tag�͕ς���
        if (collision.gameObject.tag == "BombArea"&& player_life_inv_tmp_ <= 0)
        {
            
            type_ = State.Damage;
            //�_���[�W�H�炤
            oxy_max_[oxy_count_] -= damage_;

            //���G���ԊJ�n
            player_life_inv_tmp_ = player_life_inv_time_;

            rb_.velocity = Vector3.zero;
            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            Vector3 distination = (transform.position - collision.transform.position).normalized;

            rb_.AddForce(distination * knockback_power_, ForceMode.VelocityChange);
            rb_.AddForce(transform.up * knockback_power_up_, ForceMode.VelocityChange);

          
        }
    }
   

    private void OnTriggerStay(Collider other)
    {
        //�A�C�e���͈̔�
        if (other.gameObject.tag == "PC")
        {
            fire1_range_flg_ = true;
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("�����ꂽ");
                var _stageScene = other;
                _stageScene.GetComponent<Parts>().Pickup();

                _stageScene = null;

            }
            
        }
       

        //�Q�[�W�񕜃A�C�e��
        //if (other.gameObject.tag == "BombArea")
        //{
        //    oxy_max_[oxy_count_] += oxy_recovery_;

        //}

        //�{���x�񕜃A�C�e��
        //if (other.gameObject.tag == "BombArea")
        //{
        // if(oxy_count_<2)
        //{
        //    oxy_count_++;
        //}
        //   
        //}






    }
    private void OnTriggerExit(Collider other)
    {
        //�A�C�e���͈̔͂���o��
        if (other.gameObject.tag == "PC")
        {
            fire1_range_flg_ = false;
        }

    }
}
