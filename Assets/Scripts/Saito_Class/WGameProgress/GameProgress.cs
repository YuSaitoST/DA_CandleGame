#define _DEBUG_ON

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

    [SerializeField] GameObject player_         = null; // プレイヤー
    [SerializeField] GoalCreate goalCreater_    = null; // ゴールクリエイター
    [SerializeField] DebugPanel debugPanel_     = null; // デバッグパネル

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

        // ゴール設置
        goalCreater_.CreateGoalArea(new Vector3(7.0f, -1.0f, 0.0f));

        // ゲームの進行状態をセット
        progress_ = GAME_PROGRESS.START;

        // 各種オブジェクトの初期化
        //if (sc_player_ != null)
        //{
        //    sc_player_.Initialize();
        //}
        //if(sc_submarine_ != null)
        //{
        //    sc_submarine_.Initialize();
        //}

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
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        progress_ = GAME_PROGRESS.OVER;
        debugPanel_.SetMessageText("GameOver...", "no Clear");
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
#endif
    }

    /// <summary>
    /// パラメータを取得する
    /// </summary>
    /// <returns>パラメータセット</returns>
    public Paramater GetParameters()
    {
        return parameters_.GetParameter();
    }
}
