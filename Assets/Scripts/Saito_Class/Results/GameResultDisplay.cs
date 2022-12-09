using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultDisplay : MonoBehaviour
{
    [SerializeField] Text txt_result_ = null;
    [SerializeField] Image img_result_ = null;
    [SerializeField] Sprite[] img_clears = null;


    // Start is called before the first frame update
    void Start()
    {
        GAME_PROGRESS _progress = GameProgress.instance_.GetNowProgress();

#if UNITY_EDITOR
        txt_result_.text = _progress.ToString();
#endif

        if (_progress == GAME_PROGRESS.CLEAR)
        {
            img_result_.sprite = img_clears[0];
        }
        else if (_progress == GAME_PROGRESS.OVER)
        {
            img_result_.sprite = img_clears[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
