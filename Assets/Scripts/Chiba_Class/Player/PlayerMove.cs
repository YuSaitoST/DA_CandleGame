using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーのアニメーターを入れる")]
    private Animator player_animator_ = null;
    [Space(10)]
    [Header("Playerクラス")]
    //[SerializeField]
    //private PlayerOxy script_player_oxy_ = null;

    [Space(10)]
    private Rigidbody rb_;
    [Header("プレイヤーの挙動")]

    [SerializeField, Tooltip("プレイヤーの移動速度(初期値)")]
    private float player_move_speed_ = 0.5f;

    [SerializeField, Tooltip("ブースト時の移動速度(最大値)")]
    private float player_move_boost_ = 1.0f;

    [SerializeField, Tooltip("現在の移動速度(最大値)")]
    private float player_move_ = 1.0f;

    public float Player_move
    {
        get { return player_move_; }
    }

    [Header("フィールド移動制限(下限、上限)")]
    [SerializeField, Tooltip("X軸の制限")]
    private Vector2 x_clip_ = new(-10, 10);

    [SerializeField, Tooltip("Y軸の制限")]
    private Vector2 y_clip_ = new(-500, 500);

    [SerializeField, Tooltip("Z軸の制限")]
    private Vector2 z_clip_ = new(-500, 500);

    [Header("ノックバックの設定")]
    [SerializeField, Tooltip("ノックバック時の力")]
    private float knockback_power_ = 0.4f;

    [SerializeField, Tooltip("ノックバック時の上への力")]
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


        //走り
        if (_Dash)
        {
            Dash();
        }
        else
        {
            Walk();
        }

        //移動制限
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

            // スティックが倒れていれば、倒れている方向を向く
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
            //徐々に足されていく
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
            //徐々にひかれていく
            player_move_ -= 0.1f * Time.deltaTime;
        }
        else
        {
            player_move_ = player_move_speed_;
        }
    }

    //敵に接触したときにノックバック
    public void KnockBack(Vector3 _transform)
    {
        
        rb_.velocity = Vector3.zero;
        // 自分の位置と接触してきたオブジェクトの位置を計算
        Vector3 _distination = (transform.position - _transform).normalized;

        rb_.AddForce(_distination * knockback_power_, ForceMode.VelocityChange);
        rb_.AddForce(transform.up * knockback_power_up_, ForceMode.VelocityChange);

        Debug.Log("敵に接触");


        //アニメーターリセット
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);

    }

    //Animatorをとめる
    public void ResetAnimator()
    {
        player_animator_.SetBool("isRunning", false);
        player_animator_.SetBool("isWalking", false);
    }

}
