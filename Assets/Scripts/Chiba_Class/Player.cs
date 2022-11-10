using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    Transform tr_;
    Rigidbody rb_;

    [Header("�v���C���[�̋���")]
    [SerializeField,Tooltip("�v���C���[�̈ړ����x")]
    private float    player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("�u�[�X�g���̈ړ����x�{��")]
    private float    player_move_boost_ = 2.0f;

    //[SerializeField, Tooltip("�v���C���[�̃A�j���[�^�[������")]
    //private Animator palyer_animator_;

    [SerializeField]
    private bool     player_move_flg_ = true;//�v���C���[�̈ړ���~�ptrue�ňړ���


    [Header("�_�f�Q�[�W�֘A")]
    [SerializeField,Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float    oxy_           = 0.0f;

    [SerializeField, Tooltip("�_�f�Q�[�W�̍ő�l(�����l)"), Range(0, 100)]
    private float    oxy_max_       = 100.0f;

    //[SerializeField, Tooltip("�ړ����̒ǉ�����_�f(������)")]
    //private float    oxy_cost_move_ = 0.3f;

    [SerializeField, Tooltip("�u�[�X�g���̃Q�[�W����{��")]
    private float    oxy_cost_boost_ = 2.0f;

    [SerializeField, Tooltip("���펞�̏���_�f")]
    private float    oxy_cost_      = 0.3f;

    [SerializeField, Tooltip("�_�f�Q�[�W�̒l�\���p�e�L�X�g")]
    private TMP_Text oxy_text_      = null;

    [SerializeField, Tooltip("�_�f�Q�[�W")]
    private Slider   oxy_slider_    = null;

    [SerializeField, Tooltip("�_�f�Q�[�W��0�ɂȂ��false")]
    private bool     oxy_flg_       = true;

    [Header("A�{�^���֘A")]
    [SerializeField]
    private bool fire1_range_flg_ = false;//�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�

    [Header("B�{�^���֘A")]
    [SerializeField]
    private bool fire2_flg_ = false;

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
        tr_ = GetComponent<Transform>();
        rb_ = GetComponent<Rigidbody>();
        oxy_ = oxy_max_ -0.01f;
        oxy_flg_ = true;
    }

    // Update is called once per frame
    void Update()
    {
      


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
        }

        if (player_move_flg_ == true)
        {
            Move();
        }

    }
    //�ړ�����
    private void Move()
    {
      
        float _speed = 1.0f;

        Vector3 _position   = Vector3.zero;
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //�ړ��u�[�X�g
        if (fire2_flg_== true)
        {
            _speed = player_move_speed_ * player_move_boost_;
        }
        else
        {
            _speed = player_move_speed_;
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
                new Vector3(_position.normalized.x * _speed,
                            0,
                            _position.normalized.z * _speed);
            //oxy_ -= oxy_cost_move_ * Time.deltaTime;
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
       
        oxy_text_.SetText(oxy_.ToString("F1")/* + ("��")*/);
        oxy_slider_.value = oxy_;
    }

    private void Oxy()
    {
        if (fire2_flg_ == true)
        {      
            //�u�[�X�g���͎_�f����ʂ��㏸����
            oxy_ -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;

        }
        else
        {
            oxy_ -= oxy_cost_ * Time.deltaTime;
        }
        

        if(oxy_ <= 0.01f)
        {
           oxy_flg_ = false;
           oxy_ = 0;
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
