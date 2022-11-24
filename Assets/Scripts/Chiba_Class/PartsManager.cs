using UnityEngine;
using TMPro;

public class PartsManager : MonoBehaviour
{
    [SerializeField]
    public int count_ = 0;

    [SerializeField, Tooltip("パーツの数をtext表示")]
    private TMP_Text count_ui_ = null;

    [SerializeField, Tooltip("クリアに必要なパーツ数")]
    private int clear_ = 3;
   
    private int clear_count_ = 0;

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

    public void submarine()
    {
        count_--;
        count_ui_.SetText(count_.ToString("F0")/* + ("％")*/);
        clear_count_++;

        if(clear_>=3)
        {
            //clear処理を書く
        }

    }
}
