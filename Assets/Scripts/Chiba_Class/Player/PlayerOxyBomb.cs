using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxyBomb : MonoBehaviour
{
    [Header("Player�N���X")]
    [SerializeField]
    private PlayerOxy script_player_oxy_ = null;
    [SerializeField]
    private DrawArc fire3_draw_;
    [Space(10)]
    //�_�f�����
    [SerializeField]
    private float[] bomb_cost_ = new float[] { 5, 8, 11, 14, 17, 20 };

    [SerializeField, Tooltip("�e��Prefab")]
    private GameObject[] bomb_prefab_ = new GameObject[] { null };  

    [SerializeField, Tooltip("���e�����̊J�n�_")]
    private GameObject point_;

    [SerializeField, Tooltip("���e�𗎂Ƃ��ʒu")]
    private GameObject drop_;

    //�v���n�u�̎ˏo�ʒu
    private Vector3 fire3_drop_pos_;

    private Vector3 instantiatePosition_;
    public Vector3 InstantiatePosition_
    {
        get { return instantiatePosition_; }
    }

    [SerializeField, Range(0.0f, 20.0f), Tooltip("�e�̎ˏo���鑬��")]
    private float fire3_speed_ = 1.0f;


    [SerializeField, Range(0.0f, 1.0f), Tooltip("�e�̎ˏo���鍂��")]
    private float fire3_speed2_ = 0.42f;

    // �e�̏����x
    private Vector3 shootVelocity_;

    // �e�̏����x(�ǂݎ���p)
    public Vector3 ShootVelocity_
    {
        get { return shootVelocity_; }
    }
    [Space(10)]
    [SerializeField]
    private float press_count_ = 0;

    //���e�����t���O
    private bool fellow_oxy_bomb_ = false;

    // Update is called once per frame
    void Update()
    {
        // �e�̏����x���X�V
        shootVelocity_ = point_.transform.up * fire3_speed_;

        shootVelocity_.y = fire3_speed2_;

        // �e�̐������W���X�V
        instantiatePosition_ = point_.transform.position;
        fire3_drop_pos_ = drop_.transform.position;
    }

    //���e�����t���O
    public void OxyBombAdd()
    {
        fellow_oxy_bomb_ = true;
    }


    public void PressCount()
    { //�ʃN���X�Ăяo��
        //fire3_draw_.On();

        press_count_ += Time.deltaTime;
    }

    public void CountReset()
    {
        //�ʃN���X�Ăяo��
        //fire3_draw_.Off();
        press_count_ = 0;
    }

    public void Bomb()
    {
        //�ʃN���X�Ăяo��
        //fire3_draw_.Off();
        //1�b����
        if (press_count_ <=1)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[0]);
            // �e�𐶐����Ĕ�΂�
            GameObject _obj = Instantiate(bomb_prefab_[0], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if(press_count_ >=1)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[1]);
            // �e�𐶐����Ĕ�΂�
            GameObject _obj = Instantiate(bomb_prefab_[1], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 2)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[2]);
            // �e�𐶐����Ĕ�΂�
            GameObject _obj = Instantiate(bomb_prefab_[2], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 3)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[3]);
            // �e�𐶐����Ĕ�΂�
            GameObject _obj = Instantiate(bomb_prefab_[3], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 4&& fellow_oxy_bomb_)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[4]);
            // �e�𐶐����Ĕ�΂�
            GameObject _obj = Instantiate(bomb_prefab_[4], fire3_drop_pos_, Quaternion.identity);
            return;
        }
        else if (press_count_ >= 5&& fellow_oxy_bomb_)
        {
            script_player_oxy_.OxyBomb(bomb_cost_[5]);
            // �e�𐶐����Ĕ�΂�
            GameObject _obj = Instantiate(bomb_prefab_[5], fire3_drop_pos_, Quaternion.identity);
            return;
        }
       

    }
        

}
