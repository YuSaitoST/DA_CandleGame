using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerOxy : MonoBehaviour
{
    [Header("�f�o�b�N�p")]
    [SerializeField]
    private PlayerDebugMode script_debug_ = null;

    [Header("�_�f�Q�[�W�֘A")]
    [SerializeField, Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float oxy_recovery_ = 33.3f;

    [SerializeField, Tooltip("������Ȃ�")]
    private int oxy_count_ = 0;

    [SerializeField, Tooltip("�_�f�Q�[�W��(���݂̒l)")]
    private float oxy_total_ = 0.0f;

    public float oxy_Total
    {
        get { return oxy_total_; }

    }

    //[SerializeField, Tooltip("�_�f�Q�[�W1�{�̍ő�l(�����l)"), Range(0, 33.3f)]
    private float[] oxy_max_ = new float[10];

    //[SerializeField, Tooltip("�_�f�Q�[�W1�{�̍ő�l(�����l)"), Range(0, 33.3f)]
    private float[] oxy_max_red_ = new float[10];

    //�ŏ��̃^���N�̐�
    [SerializeField]
    private int oxy_start_ = 3;

    //�ǉ��^���N�t���O
    public bool fellow_oxy_add_ = false;

    [SerializeField]
    private float enemy_hit_damage_ = 10.0f;

    [SerializeField, Tooltip("���펞�̏���_�f")]
    private float oxy_cost_ = 1.0f;

    //[SerializeField, Tooltip("�_�f�Q�[�W�̒l�\���p�e�L�X�g")]
    //private TMP_Text oxy_text_ = null;

    [SerializeField, Tooltip("�{���x�̐�")]
    private Slider[] oxy_slider_ = new Slider[3];

    [SerializeField, Tooltip("�{���x�̐�")]
    private Slider[] oxy_slider_red_ = null;

    [SerializeField]
    private GameObject oxy_add_slider_ = null;

    public bool dash_flg_ = false;
    [SerializeField, Tooltip("�u�[�X�g���̃Q�[�W����{��")]
    private float oxy_cost_boost_ = 2.0f;
    void Start()
    {
        PLAYER _param = GameProgress.instance_.GetParameters().player;
        oxy_cost_ = _param.oxy_cost;
        oxy_cost_boost_ = _param.oxy_cost_boost;
        enemy_hit_damage_ = _param.damage;

        //�ǉ��_�f�{���x��UI���\��
        oxy_add_slider_.SetActive(false);
        //�_�f�̃Z�b�g
        float _h = 100 / oxy_start_;
        for (int i = 0; i < oxy_start_; i++)
        {

            oxy_max_[i] = _h;
            oxy_max_red_[i] = _h;

        }
        oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (dash_flg_)
        {
            //�_�b�V�����͔{���������
            oxy_max_[oxy_count_] -= oxy_cost_ * oxy_cost_boost_ * Time.deltaTime;
        }
        else
        {
            oxy_max_[oxy_count_] -= oxy_cost_ * Time.deltaTime;

        }




        //�{���x�̃Q�[�W��0�ɂȂ����玟�̃{���x�ɐ؂�ւ���
        if (fellow_oxy_add_)
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 3)
            {

                oxy_max_[oxy_count_] = 0.0f;
 

                oxy_count_++;
                Debug.Log(oxy_count_);


            }
        }
        else
        {
            if (oxy_max_[oxy_count_] <= 0.0f && oxy_count_ < 2)
            {

                oxy_max_[oxy_count_] = 0.0f;
               

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

        //����
        if (fellow_oxy_add_)
        {
            oxy_total_ = oxy_max_[0] + oxy_max_[1] + oxy_max_[2] + oxy_max_[3];

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


        //�ԃQ�[�W�֘A
        if (oxy_max_red_[oxy_count_] > oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] -= 9.0f * Time.deltaTime;

        }
        else if (oxy_max_red_[oxy_count_] < oxy_max_[oxy_count_])
        {

            oxy_max_red_[oxy_count_] = oxy_max_[oxy_count_];

        }

        //�ԃQ�[�W���Ȃ��Ȃ�O�Ɏ��̃{���x�ɍs�����ꍇ�ԃQ�[�W������
        if (oxy_count_ >= 1)
        {
            oxy_max_red_[oxy_count_ - 1] = 0.0f;
        }

      //  oxy_text_.SetText(oxy_total_.ToString("F1")/* + ("��")*/);
    }

    //�_�f���e���g�������̏���ʌv�Z
    public void OxyBomb(float _cost)
    {
        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] - _cost;
        if (_tmp < 0.0f)
        {
            oxy_max_[oxy_count_] = 0;
            if (script_debug_ == false)
            {
                oxy_count_++;
            }
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= _cost;
        }
    }

    public void OxyAdd()
    {
        fellow_oxy_add_ = true;
    }

   public void OxyDamage()
    {
        float _tmp = 0.0f;
        _tmp = oxy_max_[oxy_count_] - enemy_hit_damage_;
        if (_tmp < 0.0f)
        {
            oxy_max_[oxy_count_] = 0;
            if (script_debug_ == false)
            {
                oxy_count_++;
            }
            oxy_max_[oxy_count_] += _tmp;
        }
        else
        {
            oxy_max_[oxy_count_] -= enemy_hit_damage_;
        }
    }

    //�_�f�{���x�����Ԃ����
    public void OxyRecoveryBlood()
    {
        oxy_count_--;
        oxy_max_[2] = 33.3f;
        
    }

    public void OxyRecovery()
    {
        ////1�{�ȏ����Ă���ꍇ
        if (oxy_count_ >= 1)
        {
            oxy_max_[oxy_count_] += oxy_recovery_;

            float _tmp = 0.0f;
            _tmp = oxy_max_[oxy_count_] - 33.3f;
            oxy_max_[oxy_count_] = 33.3f;
            oxy_count_--;
            oxy_max_[oxy_count_] = _tmp;
        }
        else
        {

            oxy_max_[oxy_count_] = 33.3f;
            
        }
    }
}
