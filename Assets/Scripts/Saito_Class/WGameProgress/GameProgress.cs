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
        // 重力の強さを調整
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // ゴール設置
        goalCreater_.CreateGoalArea(new Vector3(7.0f, -1.0f, 0.0f));

        // ゲームの進行状態をセット
        progress_ = GAME_PROGRESS.START;

        // デバッグパネルの表示状態の変更
#if _DEBUG_ON
        debugPanel_.SetActive(true);
#else
        debugPanel_.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
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
        debugPanel_.SetMessageText("GameOver...");
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
        debugPanel_.SetMessageText("GameClear");
#endif
    }
}
