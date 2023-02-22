#define _DEBUG_OFF

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    static readonly int[] fellow_number = { 0, 1, 1, 1, 1, 1, 2, 3 };

    static public GameProgress instance_;   // インスタンス

    [SerializeField] GameObject     player_             = null; // プレイヤー
    [SerializeField] CameraMover    mainCamera_         = null; // メインカメラ
    [SerializeField] Camera         minimapCamera_      = null; // マップカメラ
    [SerializeField] ObjectCreator  creator_            = null; // オブジェクト生成
    [SerializeField] BGMManager     bgmManager_         = null; // BGM切り替え担当
    [SerializeField] AudioSource    audiosource_        = null; // SE再生
    [SerializeField] AudioClip      se_death_           = null; // SE
    [SerializeField] Text           txt_remainingNum_   = null; // 残り人数text

    [SerializeField] Player             sc_player_      = null;
    [SerializeField] SubmarineManager   sc_submarine_   = null;

    [SerializeField] GameObject[] fellow_general_ = null;

    ParametersSet parameters_;     // パラメータ

    GAME_PROGRESS progress_;    // ゲームの進行状態

    System.Collections.Generic.List<bool> friendsWhoHelped_;

    int num_pursuers_;  // 追っている敵の数
    int num_people_saved_;  // 助けた人数
    int num_people_remaining_;  // 残りの人数


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
        
        Paramater _param = instance_.parameters_.GetParameter();
        PLAYER _param_p = _param.player;
        GENERAL_FELLOW[] _param_g = _param.general_fellow;
        
        player_.transform.position = new Vector3(_param_p.pos_x, _param_p.pos_y, _param_p.pos_z);
        player_.transform.rotation = Quaternion.AngleAxis(_param_p.rot_y, Vector3.up);

        //for(int i = 0; i < fellow_general_.Length; ++i)
        //{
        //    fellow_general_[i].transform.position = new Vector3(_param_g[i].pos_x, _param_g[i].pos_y, _param_g[i].pos_z);
        //    fellow_general_[i].transform.rotation = Quaternion.AngleAxis(_param_g[i].rot_y, Vector3.up);
        //}

        // 重力の強さを調整
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // ゲームの進行状態をセット
        instance_.progress_ = GAME_PROGRESS.START;

        // 数をリセット
        instance_.num_pursuers_ = 0;
        instance_.num_people_saved_ = 0;
        instance_.num_people_remaining_ = (instance_.num_people_remaining_ == 0) ? _param.result.s_high : instance_.num_people_remaining_;

        instance_.mainCamera_ = Camera.main.GetComponent<CameraMover>();
        instance_.mainCamera_.transform.rotation = Quaternion.AngleAxis(75.0f, Vector3.right);

        instance_.friendsWhoHelped_ = new System.Collections.Generic.List<bool>() { false, false, false, false, false };

        instance_.txt_remainingNum_.text = "残り" + instance_.num_people_remaining_ + "人";

        StartCoroutine(TankIconActive());
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
    /// 助けた仲間を登録する
    /// </summary>
    /// <param name="id">助けた仲間の番号</param>
    /// <param name="people_num">助けた仲間の人数</param>
    public void SetFriendWhoHelped(/*Fellow.fellows_ id*/ int people_num)
    {
        num_people_saved_ += people_num;//fellow_number[(int)id];
        num_people_remaining_ = Mathf.Max(num_people_remaining_ - num_people_saved_, 0);
        instance_.txt_remainingNum_.text = "残り" + num_people_remaining_ + "人";
    }

    /// <summary>
    /// 助けた人数をカウントする
    /// </summary>
    public void FriendWhoHelpedCount()
    {
        num_people_saved_ += 1;
        num_people_remaining_ = Mathf.Max(num_people_remaining_ - 1, 0);
    }

    /// <summary>
    /// 助けた人数を取得する
    /// </summary>
    /// <returns>助けた人数</returns>
    public int GetFriendWhoHelpedCount()
    {
        return num_people_saved_;
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
        return (_param != null) ? _param : null;
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