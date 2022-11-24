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

    // Start is called before the first frame update
    void Start()
    {
        count_ui_.sprite = count_ui_sprite_[count_];
        //count_ui_.SetText(count_.ToString("F0")/* + ("��")*/);
    }

    public static PartsManager GetInstance()
    {
        // �^�O�ɂ�錟��
        var partsManager = GameObject.FindWithTag("PartsManager");
        return partsManager.GetComponent<PartsManager>();
    }

    public void CountPlus()
    {
        count_++;
        count_ui_.sprite = count_ui_sprite_[count_];
        //count_ui_.SetText(count_.ToString("F0")/* + ("��")*/);
    }

    public void submarine()
    {
        count_--;
        count_ui_.sprite = count_ui_sprite_[count_];
        clear_count_++;

        if(clear_>=3)
        {
            //clear����������
        }

    }
}
