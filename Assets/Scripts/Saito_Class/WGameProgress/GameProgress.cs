#define _DEBUG_ON

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

    [SerializeField] GameObject player_         = null; // �v���C���[
    [SerializeField] GoalCreate goalCreater_    = null; // �S�[���N���G�C�^�[
    [SerializeField] DebugPanel debugPanel_     = null; // �f�o�b�O�p�l��

    GAME_PROGRESS progress_;    // �Q�[���̐i�s���


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
        // �d�͂̋����𒲐�
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // �S�[���ݒu
        goalCreater_.CreateGoalArea(new Vector3(7.0f, -1.0f, 0.0f));

        // �Q�[���̐i�s��Ԃ��Z�b�g
        progress_ = GAME_PROGRESS.START;

        // �f�o�b�O�p�l���̕\����Ԃ̕ύX
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
    /// ���݂̐i�s��Ԃ��擾����
    /// </summary>
    /// <returns>�i�s���</returns>
    public GAME_PROGRESS GetNowProgress()
    {
        return progress_;
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver()
    {
        progress_ = GAME_PROGRESS.OVER;
        debugPanel_.SetMessageText("GameOver...");
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
        debugPanel_.SetMessageText("GameClear");
#endif
    }
}
