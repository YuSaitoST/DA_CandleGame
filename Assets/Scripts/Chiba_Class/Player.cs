using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody rb_;
    //MeshRenderer mesh_;
    [SerializeField, Tooltip("�J����������")]
    private Camera cameraPos_;

    [SerializeField, Tooltip("�v���C���[�̃X�s�[�h")]
    private float moveSpeed_ = 1.0f;

    [SerializeField]
    private TMP_Text tempText_;

    [SerializeField]
    private Slider tempSlider_;

    [SerializeField]
    private float temp_;

    [SerializeField]
    private float moveTemp_ = 0.3f;

    //[SerializeField,Tooltip("(���b)�̉��̂Ԃ�")]
    //private Vector2 tempRandom_ = new Vector2(-0.45f, 0f);

    [SerializeField, Tooltip("�̉��̍Œ�l�ƍő�l")]

    private Vector2 tempRange_ = new Vector2(37.0f, 50.0f);


    //[SerializeField,Tooltip("�X�e���X�̎g�p��ɂǂꂭ�炢�̉����オ�邩")]
    //private float tempStealth_ = 20.0f;

    //�v���C���[���p�\�R���̋߂��ɂ��邩�ǂ���
    private bool pcFlg_ = false;
    private bool pccloseFlg_ = false;

    [SerializeField, Tooltip("PC���N�������Ƃ��\�����������L�����o�X�������ɓ����")]
    private GameObject pcObj_;

    [SerializeField, Tooltip("PC���N�������Ƃ��\�����������L�����o�X�������ɓ����")]
    private Canvas pcCan_;

    [SerializeField, Tooltip("�ړ����x�̃p�����[�^")]
    private float[] speedP_ = new float[4];
    //0.3,0.4,0.6,0.75;
  
        
    [SerializeField, Tooltip("����͈͂̃p�����[�^")]
    private float[] viewP_ = new float[4];
    //3,6,9,16;


    [SerializeField, Tooltip("���͂̃p�����[�^")]
    private float[] hearP_ = new float[4];
    //1,10,100,1000


    private bool moveFlg_ = true;
    //�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�
    private bool rangeFlg_ = false;
    //�v���C���[���X�L�����s���ɓ�������true�ɂȂ�
    private bool scanFlg_ = false;
    //�X�L�����ɐ���������true
    private bool scanSuccess_ = false;

    //�J�����̌������擾����p
    private Vector3 forward_ = new Vector3(1, 0, 1);

    ////���̃}�e���A���J���[�̈ꎞ�ۑ��p
    //private Color32 mat_ = new Color32(0, 0, 0, 0);

    ////�G�̃Q�[���I�u�W�F�N�g�̎擾�p
    //private GameObject enemy_;



    void Start()
    {
        pcObj_.SetActive(true);

        rb_ = GetComponent<Rigidbody>();
        //mesh_ = GetComponent<MeshRenderer>();

        //�̉��̏����l
        temp_ = Random.Range(37.0f, 39.0f);

        //�̉��̍ő�l
        tempSlider_.maxValue = tempRange_.y;

        //�}�e���A��
       // mat_ = mesh_.material.color;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {

            //���e�ݒu
            if (rangeFlg_ == true)
            {

                moveFlg_ = false;
            }
            //else
            //{
            //    moveFlg_ = true;
            //}
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            moveFlg_ = true;
        }

        //PC�N��������񉟂��ƕ���
        //�G�̃X�L����(Down�͉��Ȃ̂Ō�ŊO��)
        if (Input.GetButtonDown("Fire2"))
        {
            if (pcFlg_ == true && pccloseFlg_ == false)
            {
                moveFlg_ = false;
                pccloseFlg_ = true;
                pcObj_.SetActive(false);


                Debug.Log("B�{�^���������ꂽ");
            }
            //����
            else if (/*pcFlg_ == true && */pccloseFlg_ == true)
            {
                moveFlg_ = true;
                pccloseFlg_ = false;
                pcObj_.SetActive(true);
                Debug.Log("B�{�^���������ꂽ2");
            }
        }



    }

    void FixedUpdate()
    {
        //if (Gamepad.current == null && Keyboard.current == null)
        //{
        //    Debug.Log("�Q�[���p�b�h���L�[�{�[�h��ڑ����Ă�������");
        //    return;
        //}


        //�ړ��֘A�̊֐�(�̉������ȏゾ�Ɠ����Ȃ��Ȃ�)
        if (moveFlg_ == true )
        {
            Move();
        }

        //�X�L������������������s�����
        //if (scanSuccess_ == true )
        //{
        //    StartCoroutine("Scan");
        //    scanSuccess_ = false;
        //}

        //�̉�
        //Temp();
        //�J�������Z�b�g�����

        //UI�ɔ��f
        tempText_.SetText(temp_.ToString("F1") + ("��"));
        tempSlider_.value = temp_;

    }
    private void Move()
    {
        Vector2 _leftStick = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // �J�����̕�������A�x�N�g�����擾
        Vector3 _cameraForward = Vector3.Scale(cameraPos_.transform.forward, forward_).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 _moveForward = _cameraForward * _leftStick.y + Camera.main.transform.right * _leftStick.x;

        
        rb_.velocity = _moveForward * moveSpeed_ + new Vector3(0, rb_.velocity.y, 0).normalized;

        // �L�����N�^�[�̌�����i�s������
        if (_moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_moveForward);
        }
        //�X�e�B�b�N�����͂���Ă���Ƃ��ɉ��x���オ��
        if (_leftStick.x != 0|| _leftStick.y != 0)
        {
            temp_ += moveTemp_*Time.deltaTime;
        }
    }

    //private void Temp()
    //{
    //    temp_ += Random.Range(tempRandom_.x,tempRandom_.y) *Time.deltaTime;
        
    //    //�Œ�̉�
    //    //if(temp_ < tempRange_.x)
    //    //{
    //    //    temp_ = tempRange_.x+0.2f;
    //    //}

        
    //}

   
    //�R���[�`���킩���̂Ŗv
    //IEnumerator Scan()
    //{
    //    for (int i = 0; i < tempStealth_; i++)
    //    {
    //        mesh_.material.color -= new Color32(0, 0, 0, 10);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    //mesh_.material.color = new Color32(0, 0, 0, 30);
    //    //�X�e���X�̕b��
    //    yield return new WaitForSeconds(5.0f);

    //    mesh_.material.color = mat_;
    //    scanFlg_ = false;
    //    Debug.Log("�X�e���X���Z�b�g");
    //    for (int k = 0; k < tempStealth_; k++)
    //    {
    //        //mesh_.material.color -= new Color32(0, 0, 0, 10);
    //        temp_ += 1.0f;
    //        yield return new WaitForSeconds(0.2f);
    //    }
        
    //    //for (int i = 0; i < 255; i++)
    //    //{
    //    //    
    //    //}
    //}

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            rangeFlg_ = true;
        }
        if (other.gameObject.tag == "PC")
        {
            pcFlg_ = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            rangeFlg_ = false;
        }
        if (other.gameObject.tag == "PC")
        {
            pcFlg_ = false;
        }
    }
}
