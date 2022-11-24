using UnityEngine;
using TMPro;

public class PartsManager : MonoBehaviour
{
    [SerializeField]
    public int count_ = 0;

    [SerializeField, Tooltip("�p�[�c�̐���text�\��")]
    private TMP_Text count_ui_ = null;

    [SerializeField, Tooltip("�N���A�ɕK�v�ȃp�[�c��")]
    private int clear_ = 3;
   
    private int clear_count_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        count_ui_.SetText(count_.ToString("F0")/* + ("��")*/);
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
        count_ui_.SetText(count_.ToString("F0")/* + ("��")*/);
    }

    public void submarine()
    {
        count_--;
        count_ui_.SetText(count_.ToString("F0")/* + ("��")*/);
        clear_count_++;

        if(clear_>=3)
        {
            //clear����������
        }

    }
}
