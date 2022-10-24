using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField,Tooltip("���e�ƂȂ�Q�[���I�u�W�F�N�g��������")]
    private GameObject bombobj_;

    //���ꂪ0�ɂȂ����甚�e���ݒu�����
    [SerializeField, Tooltip("���e�ݒu�ɂ����鎞��")]
    private float bombtime_ = 10.0f;

    //�{�^���������ꂽ�Ƃ���true�ɂȂ�
    private bool installationflg_ = false;

    //�v���C���[�����e�ݒu�͈͓��ɓ�������true�ɂȂ�
    private bool rangeflg_ = false;

    Vector3 setPos_ = new Vector3(0.0f, -300.0f, 0.0f);

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //A�{�^��
        if (Input.GetButton("Fire1")&& rangeflg_ == true)
        {
            installationflg_ = true;
        }
        else
        {
            installationflg_ = false;
        }

        //0�b�ɂȂ�Ǝ��s
        if(bombtime_<=0)
        {
            bombtime_ = 0.0f;
            //���e���\�������
            bombobj_.SetActive(true);
            //�G���A���\����(�܂��͍폜)
            //this.gameObject.SetActive(false);
            this.transform.localPosition = setPos_; 
        }
    }

    private void FixedUpdate()
    {
        //true�̂Ƃ�1�b�������Ă���
        if(installationflg_ == true)
        {
            bombtime_ -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeflg_ = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rangeflg_ = false;
        }
    }
}
