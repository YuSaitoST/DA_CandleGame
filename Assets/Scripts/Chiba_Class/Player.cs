using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb_;
    [SerializeField,Tooltip("�J����������")]
    private Camera camerapos_;

    [SerializeField,Tooltip("�v���C���[�̃X�s�[�h")]
    float moveSpeed_ = 1.0f;

    private bool moveflg_ = true;

    //�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�
    private bool rangeflg_ = false;

    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
    }

    void Update()
    {

        //inputHorizontal = Input.GetAxisRaw("Horizontal");
        //inputVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (Gamepad.current == null && Keyboard.current == null)
        {
            Debug.Log("�Q�[���p�b�h���L�[�{�[�h��ڑ����Ă�������");
            return;
        }
        if (Input.GetButton("Fire1")&& rangeflg_ == true)
        {
           moveflg_ = false;
        }
        else
        {
            moveflg_ = true;
        }


        //�ړ��֘A�̊֐�
        if (moveflg_ == true)
        {
            move();
        }
        

        //�J�������Z�b�g�����
        
    }
    private void move()
    {
        Vector2 _leftStick = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 _cameraForward = Vector3.Scale(camerapos_.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 _moveForward = _cameraForward * _leftStick.y + Camera.main.transform.right * _leftStick.x;

        
        rb_.velocity = _moveForward * moveSpeed_ + new Vector3(0, rb_.velocity.y, 0);

        // �L�����N�^�[�̌�����i�s������
        if (_moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_moveForward);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            rangeflg_ = true;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            rangeflg_ = false;
        }
    }
}
