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
    private float player_move_ = 1.0f;

    //[SerializeField, Tooltip("�v���C���[�̃A�j���[�^�[������")]
    //private Animator palyer_animator_;

    [SerializeField]
    private bool     player_move_flg_ = true;//�v���C���[�̈ړ���~�ptrue�ňړ���


    [Header("�_�f�Q�[�W�֘A")]
    //[SerializeField,Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    //private float    oxy_           = 0.0f;

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

    [SerializeField, Tooltip("���͂̃p�����[�^")]
    private Slider[] oxy_slider_ = new Slider[3];

    [SerializeField, Tooltip("�_�f�Q�[�W��0�ɂȂ��false")]
    private bool     oxy_flg_       = true;

    [Header("A�{�^���֘A")]
    [SerializeField]
    private bool fire1_range_flg_ = false;//�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�

    [Header("B�{�^���֘A")]
    [SerializeField]
    private bool fire2_flg_ = false;


    [Header("X�{�^���֘A")]
    [SerializeField]
    private bool fire3_flg_ = false;

    [SerializeField]
    private bool fire3_flg2_ = false;

    //�������b���̎擾
    private float fire3_button_count_ = 0.0f;

    [SerializeField, Tooltip("�e��Prefab")]
    private GameObject fire3_tank_prefab_;
    //bulletPrefab;

   
    private DrawArc fire3_Draw;

    [SerializeField, Tooltip("�C�g�̃I�u�W�F�N�g")]
    private GameObject fire3_point_;
    //barrelObject_

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(1.0F, 20.0F), Tooltip("�e�̎ˏo���鑬��")]
    private float speed = 1.0F;

   
    // �e�̏����x
    private Vector3 shootVelocity;
 
    // �e�̏����x(�ǂݎ���p)
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
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

    



    void Start()
    {
        // �e�̏����x�␶�����W�����R���|�[�l���g
        fire3_Draw = gameObject.GetComponent<DrawArc>();

        for (int i = 0; i < oxy_max_.Length; i++)
       {
            oxy_max_[i] = 33.3f;
       }
        

        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        

        oxy_flg_ = true;

        player_move_= player_move_speed_;

        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
    }

    // Update is called once per frame
    void Update()
    {
        // �e�̏����x���X�V
        shootVelocity = fire3_point_.transform.up * speed;

        // �e�̐������W���X�V
        instantiatePosition_ = fire3_point_.transform.position;


        if (oxy_flg_ == true)
        {
            //�v���C���[�̓���
            //A�{�^��
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("A�{�^����������Ă���");
                //���e�ݒu
                if (fire1_range_flg_ == true)
                {

                    player_move_flg_ = false;
                }

            }
            else if (Input.GetButtonUp("Fire1"))
            {
                player_move_flg_ = true;
            }

            //B�{�^��
            //�ړ����x�㏸
            if (Input.GetButton("Fire2"))
            {
                Debug.Log("B�{�^���������ꂽ");
                fire2_flg_ = true;
            }
            else 
            {
                fire2_flg_ = false;
            }

            //X�{�^��
            //�{���x�A�N�V����
            if (oxy_count_ != 2)
            {
               
                if (Input.GetButton("Fire3"))
                {
                    fire3_flg_ = true;
                }
                if (Input.GetButtonUp("Fire3"))
                {
                    fire3_flg_ = false;
                    if (fire3_button_count_ <= 1.0f)
                    {
                        oxy_max_[oxy_count_] = 0;
                        oxy_count_++;
                        Debug.Log("�A�N�V�������s01-2");
                    }

                }
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
            OxyAction();
        }

        if (player_move_flg_ == true)
        {
            Move();
        }

        


    }
    private void OxyAction()
    {
        if (fire3_flg_ == true)
        {

            fire3_button_count_ += Time.deltaTime;
            if (fire3_button_count_ >= 1.0f)
            {
                fire3_flg2_ = true;
                Debug.Log("�A�N�V�������s02");
                fire3_Draw.On();
            }
            else
            {

                Debug.Log("�A�N�V�������s01-1");
            }
        }
        else if (fire3_flg_ == false)
        {
            fire3_button_count_ = 0;
            if (fire3_flg2_ == true)
            {
                fire3_Draw.Off();
                fire3_flg2_ = false;
                // �e�𐶐����Ĕ�΂�
                GameObject _obj = Instantiate(fire3_tank_prefab_, instantiatePosition_, Quaternion.identity);
                Rigidbody _rid = _obj.GetComponent<Rigidbody>();
                _rid.AddForce(shootVelocity * _rid.mass, ForceMode.Impulse);

                // 5�b��ɏ�����
                Destroy(_obj, 5.0F);
                Debug.Log("�A�N�V�������s02-2");
            }
        }
    }
        //�ړ�����
        private void Move()
    {
      
        

        Vector3 _position   = Vector3.zero;
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //�ړ��u�[�X�g
        if (fire2_flg_== true)
        {
            if (player_move_<= player_move_boost_)
            {
                
                //���X�ɑ�����Ă���
                player_move_+= 0.1f* Time.deltaTime;
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
              
                //���X�ɂЂ���Ă���
                player_move_-= 0.1f * Time.deltaTime;
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
           
            //animator_.SetBool("walking", true);

            // �X�e�B�b�N���|��Ă���΁A�|��Ă������������
            var direction2 = new Vector3(_stick_left.x, 0, _stick_left.y);
            transform.localRotation = Quaternion.LookRotation(direction2);

        }
        else
        {
            //animator_.SetBool("walking", false);

        }
    }

    //UI����
    private void Sync()
    {
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
        oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("��")*/);

        oxy_slider_[0].value = oxy_max_[0];
        oxy_slider_[1].value = oxy_max_[1];
        oxy_slider_[2].value = oxy_max_[2];
    }

    private void Oxy()
    {
       
        if (oxy_max_[oxy_count_] <= 0.0f)
        {
            oxy_count_++;
            Debug.Log(oxy_count_);
        }

        if (fire2_flg_ == true)
        {      
            
            //�u�[�X�g���͎_�f����ʂ��㏸����
            oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;

        }
        else
        {
            oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;
        }
        
       

        if(oxy_total_ <= 0.01f)
        {
           oxy_flg_ = false;
           oxy_total_ = 0;
           Debug.Log("a");
        }
        //oxy��0�ɂȂ�����Q�[���I�[�o�[
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
