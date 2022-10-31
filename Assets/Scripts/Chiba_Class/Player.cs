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
    [SerializeField,Tooltip("カメラを入れる")]
    private Camera cameraPos_;

    [SerializeField,Tooltip("プレイヤーのスピード")]
    private float moveSpeed_ = 1.0f;

    [SerializeField]
    private TMP_Text tempText_;

    [SerializeField]
    private Slider tempSlider_;

    [SerializeField]
    private float temp_;

    [SerializeField]
    private float moveTemp_ = 0.3f;

    //[SerializeField,Tooltip("(毎秒)体温のぶれ")]
    //private Vector2 tempRandom_ = new Vector2(-0.45f, 0f);

    [SerializeField,Tooltip("体温の最低値と最大値")]
   
    private Vector2 tempRange_ = new Vector2(37.0f, 50.0f);

    
    //[SerializeField,Tooltip("ステルスの使用後にどれくらい体温が上がるか")]
    //private float tempStealth_ = 20.0f;

    //プレイヤーがパソコンの近くにいるかどうか
    private bool pcFlg_ = false;

    private bool moveFlg_ = true;
    //プレイヤーが爆弾設置範囲内に入ったらtrueになる
    private bool rangeFlg_ = false;
    //プレイヤーがスキャン行動に入ったらtrueになる
    private bool scanFlg_ = false;
    //スキャンに成功したらtrue
    private bool scanSuccess_ = false;

    //カメラの向きを取得する用
    private Vector3 forward_ = new Vector3(1, 0, 1);

    ////元のマテリアルカラーの一時保存用
    //private Color32 mat_ = new Color32(0, 0, 0, 0);

    ////敵のゲームオブジェクトの取得用
    //private GameObject enemy_;

    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        //mesh_ = GetComponent<MeshRenderer>();

        //体温の初期値
        temp_ = Random.Range(37.0f, 39.0f);

        //体温の最大値
        tempSlider_.maxValue = tempRange_.y;

        //マテリアル
       // mat_ = mesh_.material.color;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if(pcFlg_ == true)
            {
                Debug.Log("a");
            }

            //爆弾設置
            if (rangeFlg_ == true)
            {

                moveFlg_ = false;
            }
            //else
            //{
            //    moveFlg_ = true;
            //}
        }
        else
        {
            moveFlg_ = true;
        }

        //敵のスキャン(Downは仮なので後で外す)
        if (Input.GetButtonDown("Fire2")&& moveFlg_ == true&& scanFlg_ == false/*&&temp_<tempRange_.y*/)
        {
            //スキャンのシステムができるまで成功フラグはここに入れとく
            //scanSuccess_ = true;

            //scanFlg_ = true;
            Debug.Log("Bボタンが押された");
        }
        //else
        //{
        //    ScanFlg_ = false;
        //}



    }

    void FixedUpdate()
    {
        //if (Gamepad.current == null && Keyboard.current == null)
        //{
        //    Debug.Log("ゲームパッドかキーボードを接続してください");
        //    return;
        //}


        //移動関連の関数(体温が一定以上だと動けなくなる)
        if (moveFlg_ == true )
        {
            Move();
        }

        //スキャンが成功したら実行される
        //if (scanSuccess_ == true )
        //{
        //    StartCoroutine("Scan");
        //    scanSuccess_ = false;
        //}

        //体温
        //Temp();
        //カメラリセットを作る

        //UIに反映
        tempText_.SetText(temp_.ToString("F1") + ("℃"));
        tempSlider_.value = temp_;

    }
    private void Move()
    {
        Vector2 _leftStick = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // カメラの方向から、ベクトルを取得
        Vector3 _cameraForward = Vector3.Scale(cameraPos_.transform.forward, forward_).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 _moveForward = _cameraForward * _leftStick.y + Camera.main.transform.right * _leftStick.x;

        
        rb_.velocity = _moveForward * moveSpeed_ + new Vector3(0, rb_.velocity.y, 0).normalized;

        // キャラクターの向きを進行方向に
        if (_moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_moveForward);
        }
        //スティックが入力されているときに温度が上がる
        if (_leftStick.x != 0|| _leftStick.y != 0)
        {
            temp_ += moveTemp_*Time.deltaTime;
        }
    }

    //private void Temp()
    //{
    //    temp_ += Random.Range(tempRandom_.x,tempRandom_.y) *Time.deltaTime;
        
    //    //最低体温
    //    //if(temp_ < tempRange_.x)
    //    //{
    //    //    temp_ = tempRange_.x+0.2f;
    //    //}

        
    //}

   
    //コルーチンわからんので没
    //IEnumerator Scan()
    //{
    //    for (int i = 0; i < tempStealth_; i++)
    //    {
    //        mesh_.material.color -= new Color32(0, 0, 0, 10);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    //mesh_.material.color = new Color32(0, 0, 0, 30);
    //    //ステルスの秒数
    //    yield return new WaitForSeconds(5.0f);

    //    mesh_.material.color = mat_;
    //    scanFlg_ = false;
    //    Debug.Log("ステルスリセット");
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
        //if (other.gameObject.tag == "PC")
        //{ 
        //    pcFlg_ = true;
        //}

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BombArea")
        {
            rangeFlg_ = false;
        }
        //if (other.gameObject.tag == "PC")
        //{
        //    pcFlg_ = false;
        //}
    }
}
