#define _DEBUG_OFF

using System.Collections;
using UnityEngine;

/// <summary>
/// ゲームの進行状態
/// </summary>
public enum GAME_PROGRESS
{
    START,
    PLAY,
    PAUSE,
    CLEAR,
    OVER
}

public class GameProgress : MonoBehaviour
{
    static public GameProgress instance_;   // インスタンス

    [SerializeField] GameObject     player_         = null; // プレイヤー
    [SerializeField] CameraMover    mainCamera_     = null;
    [SerializeField] Camera         minimapCamera_  = null;
    [SerializeField] ObjectCreator  creator_        = null;
    [SerializeField] BGMManager     bgmManager_     = null; // BGM切り替え担当
    [SerializeField] AudioSource    audiosource_    = null;
    [SerializeField] AudioClip      se_death_       = null;

    [SerializeField] Player             sc_player_      = null;
    [SerializeField] SubmarineManager   sc_submarine_   = null;

    ParametersSet parameters_;     // パラメータ

    GAME_PROGRESS progress_;    // ゲームの進行状態

    bool[] friendsWhoHelped_;

    int num_pursuers_;  // 追っている敵の数


    private void Awake()
    {
        // インスタンス生成
        if(instance_ == null)
        {
            instance_ = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // パラメータをセットする
        instance_.parameters_ = new ParametersSet();
        instance_.parameters_.SetParameter();

        // 重力の強さを調整
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // ゲームの進行状態をセット
        instance_.progress_ = GAME_PROGRESS.START;

        // 追っている敵の数をリセット
        instance_.num_pursuers_ = 0;

        instance_.mainCamera_ = Camera.main.GetComponent<CameraMover>();
        instance_.mainCamera_.transform.rotation = Quaternion.AngleAxis(75.0f, Vector3.right);

        instance_.friendsWhoHelped_ = new bool[] { false, false, false, false, false };

        StartCoroutine(TankIconActive());
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.K))
        {
            bgmManager_.MainToEnemy();
        }else if (Input.GetKeyUp(KeyCode.L))
        {
            bgmManager_.EnemyToMain();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Radar_Contraction();
        }
#endif
    }

    public void SetPlayer(GameObject player) { instance_.player_ = player; }
    public void SetMiniCamera(Camera minicamera) { instance_.minimapCamera_ = minicamera; }
    public void SetCreator(ObjectCreator creater) { instance_.creator_ = creater; }
    public void SetBGMMamager(BGMManager manager) { instance_.bgmManager_ = manager; }
    public void SetAudioSource(AudioSource audioSource) { instance_.audiosource_ = audioSource; }

    /// <summary>
    /// 現在の進行状態を取得する
    /// </summary>
    /// <returns>進行状態</returns>
    public GAME_PROGRESS GetNowProgress()
    {
        return progress_;
    }

    /// <summary>
    /// 仲間の救出状態を取得する
    /// </summary>
    /// <returns>仲間の救出状態</returns>
    public bool[] GetFriendsWhoHelped()
    {
        return friendsWhoHelped_;
    }

    /// <summary>
    /// 助けた仲間を登録する
    /// </summary>
    /// <param name="id">助けた仲間の番号</param>
    public void SetFriendWhoHelped(Fellow.fellows_ id)
    {
        friendsWhoHelped_[(int)id] = true;
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void GameClear()
    {
        progress_ = GAME_PROGRESS.CLEAR;
#if _DEBUG_ON
        debugPanel_.SetMessageText("GameClear!", "no Clear");
#endif

        StartCoroutine(StayToGoResult());
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        progress_ = GAME_PROGRESS.OVER;
        sc_player_.GameOver();

        audiosource_.PlayOneShot(se_death_);
        StartCoroutine(StayToGoResult());
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    public void CameraShake()
    {
        mainCamera_.Shake(0.003f, 0.05f, 0.5f);
    }

    /// <summary>
    /// レーダーの表示範囲を拡大させる
    /// </summary>
    public void Radar_Contraction()
    {
        //minimapCamera_.orthographicSize = 7.0f;
        StartCoroutine(RadarScaleChange());
    }

    /// <summary>
    /// タンクをレーダーに表示させる
    /// </summary>
    public void Rader_TankIconActive()
    {
        creator_.TanksActive(true);
    }

    /// <summary>
    /// 敵の追跡が開始した時にBGMを切り替える
    /// </summary>
    public void Enemy_StartTracking()
    {
        num_pursuers_ += 1;
        if (num_pursuers_ == 1)
        {
            bgmManager_.MainToEnemy();
        }
    }

    /// <summary>
    /// 敵の追跡が終了したらBGMを切り替える
    /// </summary>
    public void Enemy_EndTracking()
    {
        num_pursuers_ = Mathf.Max(num_pursuers_ - 1, 0);
        if (num_pursuers_ == 0)
        {
            bgmManager_.EnemyToMain();
        }
    }

    /// <summary>
    /// プレイヤーの取得
    /// </summary>
    /// <returns>プレイヤー</returns>
    public GameObject Get_PlayerC()
    {
        return player_;
    }

    /// <summary>
    /// プレイヤーの座標を取得する
    /// </summary>
    /// <returns>プレイヤーの座標</returns>
    public Vector3 GetPlayerPos() { 
        return player_.transform.position; 
    }

    /// <summary>
    /// ゲーム終了時処理
    /// </summary>
    public void GameFine()
    {
#if _DEBUG_ON
        debugPanel_.SetMessageText("GameClear", "no Clear");
#else
        FadeManager.Instance.LoadScene("ResultScene", 2.0f);
#endif
    }

    /// <summary>
    /// パラメータを取得する
    /// </summary>
    /// <returns>パラメータセット</returns>
    public Paramater GetParameters()
    {
        Paramater _param = parameters_.GetParameter();
        if(_param != null)
        {
            return _param;
        }

        return null;
    }

    IEnumerator StayToGoResult()
    {
        yield return new WaitForSeconds(3.5f);

        GameFine();

        yield return null;
    }

    IEnumerator RadarScaleChange()
    {
        while (true)
        {
            minimapCamera_.orthographicSize = Mathf.Lerp(minimapCamera_.orthographicSize, 7.0f, 0.5f);

            if(7.0f <= minimapCamera_.orthographicSize)
            {
                break;
            }
        }

        yield return null;
    }

    IEnumerator TankIconActive()
    {
        yield return null;

        creator_.TanksActive(false);

        yield return null;
    }
}

/*
 * [参考]
 * https://indie-game-creation-with-unity.hatenablog.com/entry/area-music-cross-fade
 */