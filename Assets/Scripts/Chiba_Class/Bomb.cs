using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField,Tooltip("���e�ƂȂ�Q�[���I�u�W�F�N�g��������")]
    private GameObject bomb_obj_;

    //���ꂪ0�ɂȂ����甚�e���ݒu�����
    [SerializeField, Tooltip("���e�ݒu�ɂ����鎞��")]
    private float bomb_time_ = 10.0f;

    [SerializeField,Tooltip("���ԕ\�L�p�X���C�_�[")]
    private Slider bomb_slider_;

    [SerializeField, Tooltip("���ԕ\�L�p�X���C�_�[")]
    private Canvas bomb_canvas_;

    //�{�^���������ꂽ�Ƃ���true�ɂȂ�
    private bool bomb_installation_flg_ = false;

    //�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�
    private bool bomb_range_flg_ = false;

    Vector3 setPos_ = new Vector3(0.0f, -300.0f, 0.0f);

    private float bomb_limit_ = 0.0f;

    //�A���Ŏ��s����Ȃ��悤�ɂ���
    private bool flg_ = false;
    [SerializeField]
    bool isInstalled_ = false;



    // Start is called before the first frame update
    void Start()
    {
        bomb_canvas_.enabled = false; // UI�����ɂ���
        bomb_slider_.maxValue = bomb_time_;
       
    }

    // Update is called once per frame
    void Update()
    {
        //A�{�^��
        if (Input.GetButton("Fire1")&& bomb_range_flg_ == true)
        {
            bomb_installation_flg_ = true;
        }
        else
        {
            bomb_installation_flg_ = false;
        }

        //�p�x�𓯂��ɂ���
        bomb_canvas_.transform.localRotation = Camera.main.transform.rotation;



       
    }

    private void FixedUpdate()
    {
        //true�̂Ƃ�1�b���������
        if(bomb_installation_flg_ == true)
        {
            bomb_limit_ += Time.deltaTime;
        }

        //0�b�ɂȂ�Ǝ��s
        if (bomb_time_ <= bomb_limit_ && flg_ == false)
        {
            bomb_canvas_.enabled = false; // UI�����ɂ���
            bomb_time_ = 10.0f;
            //���e���\�������
            Debug.Log("���e���u���ꂽ");
            bomb_obj_.SetActive(true);
            //�G���A���\����(�܂��͍폜)
            //this.gameObject.SetActive(false);
            this.transform.localPosition = setPos_;
            flg_ = true;

            // �Q�[���I����͏��������Ȃ�
            if (BombManager.instance_.IsAllInstalled())
            {
                return;
            }

            isInstalled_ = true;
            BombManager.instance_.Check_AllInstalled();   // �S�Ă̐ݒu�G���A�𒲂ׂ�
        }
       bomb_slider_.value = bomb_limit_;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bomb_range_flg_ = true;
            bomb_canvas_.enabled = true;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bomb_range_flg_ = false;
            bomb_canvas_.enabled = false;
        }
       
    }
}
