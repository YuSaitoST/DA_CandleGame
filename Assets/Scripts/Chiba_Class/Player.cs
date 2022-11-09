using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    Rigidbody rb_;

    [SerializeField,Tooltip("�v���C���[�̈ړ����x")]
    private float    player_move_speed =2.0f;

    //[SerializeField, Tooltip("�v���C���[�̃A�j���[�^�[������")]
    //private Animator palyer_animator_;

    [SerializeField]
    private bool     player_move_flg_ = true;//�v���C���[�̈ړ���~�ptrue�ňړ���

    

    [SerializeField]
    private float    oxy_           = 0.0f;

    [SerializeField, Tooltip("�_�f�Q�[�W�̍ő�l(�����l)")]
    private float    oxy_max_       = 100.0f;

    [SerializeField, Tooltip("�ړ����̒ǉ�����_�f(������)")]
    private float    oxy_cost_move_ = 0.3f;

    [SerializeField, Tooltip("���펞�̏���_�f")]
    private float    oxy_cost_      = 0.3f;

    [SerializeField, Tooltip("�_�f�Q�[�W�̒l�\���p�e�L�X�g")]
    private TMP_Text oxy_text_;

    [SerializeField, Tooltip("�_�f�Q�[�W")]
    private Slider   oxy_slider_;

    [SerializeField, Tooltip("�_�f�Q�[�W��0�ɂȂ��false")]
    private bool     oxy_flg_ = true;

    private bool fire1_range_flg_ = false;//�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�

    private bool fire2_flg_ = false;

    //[SerializeField, Tooltip("���j���[���J�����Ƃ��ɍŏ��ɑI�������{�^��")]
    //private GameObject ui_button_firstSelect_;





    void Start()
    {
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
            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("B�{�^���������ꂽ");
                if (fire2_flg_ == false)
                {
                    fire2_flg_ = true;
                }
                else if (fire2_flg_ == true)
                {
                    fire2_flg_ = false;
                    //EventSystem.current.SetSelectedGameObject(button_firstSelect_);
                }

            }
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
        Vector3 _position_ = new Vector3(0, 0, 0);
        Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (_stick_left.x != 0.0f || _stick_left.y != 0.0f)
        {
            _position_.x = _stick_left.x;
            _position_.z = _stick_left.y;
            rb_.velocity =
                new Vector3(_position_.normalized.x * player_move_speed,
                            0,
                            _position_.normalized.z * player_move_speed);
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
        oxy_ -= oxy_cost_ * Time.deltaTime;

        if(oxy_ <= 0.01f)
        {
           oxy_flg_ = false;
           oxy_ = 0;
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
