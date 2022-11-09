using UnityEngine;

public class GameProgress : MonoBehaviour
{
    static public GameProgress instance_;   // インスタンス

    [SerializeField] GameObject player_         = null; // プレイヤー
    [SerializeField] GoalCreate goalCreater_    = null; // ゴールクリエイター


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
}
