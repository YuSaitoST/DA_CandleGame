using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField,Tooltip("���e�ƂȂ�Q�[���I�u�W�F�N�g��������")]
    private GameObject bombObj_;

    //���ꂪ0�ɂȂ����甚�e���ݒu�����
    [SerializeField, Tooltip("���e�ݒu�ɂ����鎞��")]
    private float bombTime_ = 10.0f;

    [SerializeField,Tooltip("���ԕ\�L�p�X���C�_�[")]
    private Slider bombSlider_;

    [SerializeField, Tooltip("���ԕ\�L�p�X���C�_�[")]
    private Canvas bombCanvas_;

    //�{�^���������ꂽ�Ƃ���true�ɂȂ�
    private bool installationFlg_ = false;

    //�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�
    private bool rangeFlg_ = false;

    Vector3 setPos_ = new Vector3(0.0f, -300.0f, 0.0f);

    private float bombLimit_ = 0.0f;

    //�A���Ŏ��s����Ȃ��悤�ɂ���
    private bool flg_ = false;



    // Start is called before the first frame update
    void Start()
    {
        bombCanvas_.enabled = false; // UI�����ɂ���
        bombSlider_.maxValue = bombTime_;
       
    }

    // Update is called once per frame
    void Update()
    {
        //A�{�^��
        if (Input.GetButton("Fire1")&& rangeFlg_ == true)
        {
            installationFlg_ = true;
        }
        else
        {
            installationFlg_ = false;
        }

        //�p�x�𓯂��ɂ���
        bombCanvas_.transform.localRotation = Camera.main.transform.rotation;
    }

    private void FixedUpdate()
    {
        //true�̂Ƃ�1�b���������
        if(installationFlg_ == true)
        {
            bombLimit_ += Time.deltaTime;
        }

        //0�b�ɂȂ�Ǝ��s
        if (bombTime_ <= bombLimit_ && flg_ == false)
        {
            bombCanvas_.enabled = false; // UI�����ɂ���
            bombTime_ = 10.0f;
            //���e���\�������
            Debug.Log("���e���u���ꂽ");
            bombObj_.SetActive(true);
            //�G���A���\����(�܂��͍폜)
            //this.gameObject.SetActive(false);
            this.transform.localPosition = setPos_;
            flg_ = true;
        }
       bombSlider_.value = bombLimit_;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeFlg_ = true;
            bombCanvas_.enabled = true;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeFlg_ = false;
            bombCanvas_.enabled = false;
        }
       
    }
}
