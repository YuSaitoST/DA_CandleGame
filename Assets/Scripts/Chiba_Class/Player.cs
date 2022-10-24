using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb_;
    [SerializeField,Tooltip("カメラを入れる")]
    private Camera camerapos_;

    [SerializeField,Tooltip("プレイヤーのスピード")]
    float moveSpeed_ = 1.0f;

    private bool moveflg_ = true;

    //プレイヤーが爆弾設置範囲内に入ったらtrueになる
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
            Debug.Log("ゲームパッドかキーボードを接続してください");
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


        //移動関連の関数
        if (moveflg_ == true)
        {
            move();
        }
        

        //カメラリセットを作る
        
    }
    private void move()
    {
        Vector2 _leftStick = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 _cameraForward = Vector3.Scale(camerapos_.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 _moveForward = _cameraForward * _leftStick.y + Camera.main.transform.right * _leftStick.x;

        
        rb_.velocity = _moveForward * moveSpeed_ + new Vector3(0, rb_.velocity.y, 0);

        // キャラクターの向きを進行方向に
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
