using UnityEngine;

public class GameProgress : MonoBehaviour
{
    static public GameProgress instance_;   // �C���X�^���X

    [SerializeField] GameObject player_         = null; // �v���C���[
    [SerializeField] GoalCreate goalCreater_    = null; // �S�[���N���G�C�^�[


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
        // �d�͂������������ύX����(NavMesh���y�ɋ@�\�����邽�߂�)
        Physics.gravity = new Vector3(0.0f, 0.0f, -1.0f);

        // �S�[���ݒu
        goalCreater_.CreateGoalArea(new Vector3(7.0f, 0.0f, 3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �v���C���[�̍��W���擾����
    /// </summary>
    /// <returns>�v���C���[�̍��W</returns>
    public Vector3 GetPlayerPos() { 
        return player_.transform.position; 
    }
}
