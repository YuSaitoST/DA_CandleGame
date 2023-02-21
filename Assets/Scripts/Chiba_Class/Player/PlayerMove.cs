using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("�v���C���[�̃A�j���[�^�[������")]
    private Animator player_animator_ = null;
    [Space(10)]
    [Header("Player�N���X")]
    //[SerializeField]
    //private PlayerOxy script_player_oxy_ = null;

    [Space(10)]
    private Rigidbody rb_;
    [Header("�v���C���[�̋���")]

    [SerializeField, Tooltip("�v���C���[�̈ړ����x(�����l)")]
    private float player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("�u�[�X�g���̈ړ����x(�ő�l)")]
    private float player_move_boost_ = 1.0f;

    [SerializeField, Tooltip("���݂̈ړ����x(�ő�l)")]
    private float player_move_ = 1.0f;

    public float Player_move
    {
        get { return player_move_; }
    }

    [Header("�t�B�[���h�ړ�����(�����A���)")]
    [SerializeField, Tooltip("X���̐���")]
    private Vector2 x_clip_ = new(-10, 10);

    [SerializeField, Tooltip("Y���̐���")]
    private Vector2 y_clip_ = new(-500, 500);

    [SerializeField, Tooltip("Z���̐���")]
    private Vector2 z_clip_ = new(-500, 500);

    [Header("�m�b�N�o�b�N�̐ݒ�")]
    [SerializeField, Tooltip("�m�b�N�o�b�N���̗�")]
    private float knockback_power_ = 0.4f;

    [SerializeField, Tooltip("�m�b�N�o�b�N���̏�ւ̗�")]
    private float knockback_power_up_ = 0.2f;
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        PLAYER _param = GameProgress.instance_.GetParameters().player;
        player_move_speed_ = _param.move_speed;
        player_move_boost_ = _param.move_boost;
       
        knockback_power_ = _param.knockback_power;
        knockback_power_up_ = _param.knockback_power_up;
      //  knockback_stan_ = _param.knockback_stan_;

    }

    public void Move(bool _Dash,Vector2 _stick_left)
    {
        Vector3 _position = Vector3.zero;
        //Vector2 _stick_left = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //����
        if (_Dash)
        {
            Dash();
        }
        else
        {
            Walk();
        }

        //�ړ�����
        transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, x_clip_.x, x_clip_.y),
                Mathf.Clamp(this.transform.position.y, y_clip_.x, y_clip_.y),
                Mathf.Clamp(this.transform.position.z, z_clip_.x, z_clip_.y)
                );

        if (_stick_left.x != 0.0f || _stick_left.y != 0.0f)
        {
            _position.x = _stick_left.x;
            _position.z = _stick_left.y;
            rb_.velocity =
                new Vector3(_position.normalized.x * player_move_,
                            0,
                            _position.normalized.z * player_move_);

            player_animator_.SetBool("isWalking", true);

            // �X�e�B�b�N���|��Ă���΁A�|��Ă������������
            var _direction2 = new Vector3(_stick_left.x, 0, _stick_left.y);
            transform.localRotation = Quaternion.LookRotation(_direction2);

        }
        else
        {
            player_animator_.SetBool("isWalking", false);

        }
    }

    void Dash()
    {
       

        if (player_move_ <= player_move_boost_)
        {
            player_animator_.SetBool("isRunning", true);
            //���X�ɑ�����Ă���
            player_move_ += 0.1f * Time.deltaTime;
        }
        else
        {
            player_move_ = player_move_boost_;
        }
    }

    void Walk()
    {
       


        if (player_move_ >= player_move_speed_)
        {
            player_animator_.SetBool("isRunning", false);
            //���X�ɂЂ���Ă���
            player_move_ -= 0.1f * Time.deltaTime;
        }
        else
        {
            player_move_ = player_move_speed_;
        }
    }

    //�G�ɐڐG�����Ƃ��Ƀm�b�N�o�b�N
    public void KnockBack(Vector3 _transform)
    {
        
        rb_.velocity = Vector3.zero;
        // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu���v�Z
        Vector3 _distination = (transform.position - _transform).normalized;

        rb_.AddForce(_distination * knockback_power_, ForceMode.VelocityChange);
        rb_.AddForce(transform.up * knockback_power_up_, ForceMode.VelocityChange);

        Debug.Log("�G�ɐڐG");


        //�A�j���[�^�[���Z�b�g
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);

    }

    //Animator���Ƃ߂�
    public void ResetAnimator()
    {
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);
    }

}
