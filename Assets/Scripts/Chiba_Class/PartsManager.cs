using UnityEngine;
using TMPro;

public class PartsManager : MonoBehaviour
{
    [SerializeField]
    int count_ = 0;

    [SerializeField, Tooltip("パーツの数をtext表示")]
    private TMP_Text count_ui_ = null;

    // Start is called before the first frame update
    void Start()
    {
        count_ui_.SetText(count_.ToString("F0")/* + ("％")*/);
    }

    public static PartsManager GetInstance()
    {
        // タグによる検索
        var partsManager = GameObject.FindWithTag("PartsManager");
        return partsManager.GetComponent<PartsManager>();
    }

    public void CountPlus()
    {
        count_++;
        count_ui_.SetText(count_.ToString("F0")/* + ("％")*/);
    }
}
