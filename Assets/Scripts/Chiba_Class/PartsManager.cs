using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartsManager : MonoBehaviour
{
    [SerializeField]
    public int count_ = 0;

    [SerializeField, Tooltip("�N���A�ɕK�v�ȃp�[�c��")]
    private int clear_ = 3;
   
    private int clear_count_ = 0;

    [SerializeField, Tooltip("�p�[�c�̐���text�\��")]
    private Image count_ui_ = null;
    [SerializeField]
    private Sprite[] count_ui_sprite_ = new Sprite[4];

    public static PartsManager Instance
    {
        get{ return instance; }
        private set { instance = value; }
        
    }
    static PartsManager instance = null;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {   
        count_ui_.sprite = count_ui_sprite_[count_];
        //count_ui_.SetText(count_.ToString("F0")/* + ("��")*/);
    }

   

    public void CountPlus()
    {
        count_++;
        count_ui_.sprite = count_ui_sprite_[count_];
    }

    public void submarine()
    {
        count_--;
        count_ui_.sprite = count_ui_sprite_[count_];
        clear_count_++;

        if (clear_count_>= clear_)
        {
            //clear����������
            GameProgress.instance_.GameClear();
        }

    }
}
