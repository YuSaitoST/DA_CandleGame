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
    [SerializeField] DebugPanel     debugPanel_     = null; // デバッグパネル
    [SerializeField] BGMManager     bgmManager_     = null; // BGM切り替え担当
    [SerializeField] AudioSource    audiosource_    = null;
    [SerializeField] AudioClip      se_death_       = null;

    [SerializeField] Player             sc_player_      = null;
    [SerializeField] SubmarineManager   sc_submarine_   = null;

    ParametersSet parameters_;     // パラメータ

    GAME_PROGRESS progress_;    // ゲームの進行状態

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
    }


    // Start is called before the first frame update
    void Start()
    {
        // パラメータをセットする
        instance_.parameters_ = new ParametersSet();
        instance_.parameters_.SetParameter();

        // 重力の強さを調整
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // ゲームの進行状態をセット
        instance_.progress_ = GAME_PROGRESS.START;

        // 追っている敵の数をリセット
        instance_.num_pursuers_ = 0;


        // デバッグパネルの表示状態の変更
#if _DEBUG_ON
        instance_.debugPanel_.SetActive(true);
#else
        instance_.debugPanel_.SetActive(false);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            bgmManager_.MainToEnemy();
        }else if (Input.GetKeyUp(KeyCode.L))
        {
            bgmManager_.EnemyToMain();
        }
    }

    /// <summary>
    /// 現在の進行状態を取得する
    /// </summary>
    /// <returns>進行状態</returns>
    public GAME_PROGRESS GetNowProgress()
    {
        return progress_;
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
#if _DEBUG_ON
        debugPanel_.SetMessageText("GameOver...", "no Clear");
#endif

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
}

/*
 * [参考]
 * https://indie-game-creation-with-unity.hatenablog.com/entry/area-music-cross-fade
 */