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
    [SerializeField] DebugPanel     debugPanel_     = null; // デバッグパネル
    [SerializeField] AudioSource    audiosource_    = null;
    [SerializeField] AudioClip      se_death_       = null;

    [SerializeField] Player             sc_player_      = null;
    [SerializeField] SubmarineManager   sc_submarine_   = null;

    ParametersSet parameters_;     // パラメータ

    GAME_PROGRESS progress_;    // ゲームの進行状態


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
        parameters_ = new ParametersSet();
        parameters_.SetParameter();

        // 重力の強さを調整
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // ゲームの進行状態をセット
        progress_ = GAME_PROGRESS.START;


        // デバッグパネルの表示状態の変更
#if _DEBUG_ON
        debugPanel_.SetActive(true);
#else
        debugPanel_.SetActive(false);
#endif
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
