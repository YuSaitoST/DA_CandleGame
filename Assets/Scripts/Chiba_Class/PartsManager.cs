using UnityEngine;
using TMPro;

public class PartsManager : MonoBehaviour
{
    [SerializeField]
    int count_ = 0;

    [SerializeField, Tooltip("�p�[�c�̐���text�\��")]
    private TMP_Text count_ui_ = null;

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
}
