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

    [SerializeField] Player             sc_player_      = null;
    [SerializeField] SubmarineManager   sc_submarine_   = null;

    ParametersSet parameters_;     // �p�����[�^

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
        // �p�����[�^���Z�b�g����
        parameters_ = new ParametersSet();
        parameters_.SetParameter();

        // �d�͂̋����𒲐�
        Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);

        // �S�[���ݒu
        goalCreater_.CreateGoalArea(new Vector3(7.0f, -1.0f, 0.0f));

        // �Q�[���̐i�s��Ԃ��Z�b�g
        progress_ = GAME_PROGRESS.START;

        // �e��I�u�W�F�N�g�̏�����
        //if (sc_player_ != null)
        //{
        //    sc_player_.Initialize();
        //}
        //if(sc_submarine_ != null)
        //{
        //    sc_submarine_.Initialize();
        //}

        // �f�o�b�O�p�l���̕\����Ԃ̕ύX
#if _DEBUG_ON
        debugPanel_.SetActive(true);
#else
        debugPanel_.SetActive(false);
#endif
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
        debugPanel_.SetMessageText("GameOver...", "no Clear");
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
#endif
    }

    /// <summary>
    /// �p�����[�^���擾����
    /// </summary>
    /// <returns>�p�����[�^�Z�b�g</returns>
    public Paramater GetParameters()
    {
        return parameters_.GetParameter();
    }
}
