#define _DEBUG_ON

using UnityEngine;

public class GameProgress : MonoBehaviour
{
    static public GameProgress instance_;   // インスタンス

    [SerializeField] GameObject player_         = null; // プレイヤー
    [SerializeField] GoalCreate goalCreater_    = null; // ゴールクリエイター
    [SerializeField] DebugPanel debugPanel_     = null; // デバッグパネル


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
        // 重力がかかる方向を変更する(NavMeshを楽に機能させるために)
        Physics.gravity = new Vector3(0.0f, 0.0f, -1.0f);

        // ゴール設置
        goalCreater_.CreateGoalArea(new Vector3(7.0f, 0.0f, 3.0f));

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
        debugPanel_.SetMessageText("Not Clear", "GameClear");
#endif

        // 重力を解除
        Physics.gravity = Vector3.zero;
    }
}
