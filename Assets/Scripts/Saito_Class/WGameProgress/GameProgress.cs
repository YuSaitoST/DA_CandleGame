#define _DEBUG_OFF

using System.Collections;
using UnityEngine;

/// <summary>
/// �Q�[���̐i�s���
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
    static public GameProgress instance_;   // �C���X�^���X

    [SerializeField] GameObject     player_         = null; // �v���C���[
    [SerializeField] CameraMover    mainCamera_     = null;
    [SerializeField] DebugPanel     debugPanel_     = null; // �f�o�b�O�p�l��
    [SerializeField] BGMManager     bgmManager_     = null; // BGM�؂�ւ��S��
    [SerializeField] AudioSource    audiosource_    = null;
    [SerializeField] AudioClip      se_death_       = null;

    [SerializeField] Player             sc_player_      = null;
    [SerializeField] SubmarineManager   sc_submarine_   = null;

    ParametersSet parameters_;     // �p�����[�^

    GAME_PROGRESS progress_;    // �Q�[���̐i�s���

    int num_pursuers_;  // �ǂ��Ă���G�̐�


    private void Awake()
    {
        // �C���X�^���X����
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
        // �p�����[�^���Z�b�g����
        instance_.parameters_ = new ParametersSet();
        instance_.parameters_.SetParameter();

        // �d�͂̋����𒲐�
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // �Q�[���̐i�s��Ԃ��Z�b�g
        instance_.progress_ = GAME_PROGRESS.START;

        // �ǂ��Ă���G�̐������Z�b�g
        instance_.num_pursuers_ = 0;


        // �f�o�b�O�p�l���̕\����Ԃ̕ύX
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
    /// ���݂̐i�s��Ԃ��擾����
    /// </summary>
    /// <returns>�i�s���</returns>
    public GAME_PROGRESS GetNowProgress()
    {
        return progress_;
    }

    /// <summary>
    /// �Q�[���N���A����
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
    /// �Q�[���I�[�o�[����
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
    /// �J������h�炷
    /// </summary>
    public void CameraShake()
    {
        mainCamera_.Shake(0.003f, 0.05f, 0.5f);
    }

    /// <summary>
    /// �G�̒ǐՂ��J�n��������BGM��؂�ւ���
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
    /// �G�̒ǐՂ��I��������BGM��؂�ւ���
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
    /// �v���C���[�̎擾
    /// </summary>
    /// <returns>�v���C���[</returns>
    public GameObject Get_PlayerC()
    {
        return player_;
    }

    /// <summary>
    /// �v���C���[�̍��W���擾����
    /// </summary>
    /// <returns>�v���C���[�̍��W</returns>
    public Vector3 GetPlayerPos() { 
        return player_.transform.position; 
    }

    /// <summary>
    /// �Q�[���I��������
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
    /// �p�����[�^���擾����
    /// </summary>
    /// <returns>�p�����[�^�Z�b�g</returns>
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
 * [�Q�l]
 * https://indie-game-creation-with-unity.hatenablog.com/entry/area-music-cross-fade
 */