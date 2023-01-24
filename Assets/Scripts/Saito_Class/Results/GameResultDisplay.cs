using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameResultDisplay : MonoBehaviour
{
    [SerializeField] Text txt_result_ = null;
    [SerializeField] Image img_result_ = null;
    [SerializeField] Sprite[] img_clears = null;

    [SerializeField] BG_Scroll animation_ = null;
    [SerializeField] Image dialog_ = null;

    [SerializeField] float speed_dialog_fade_ = 0.016f;
    [SerializeField] float max_dialog_fade_ = 0.73f;


    // Start is called before the first frame update
    void Start()
    {
        GameProgress _g_progress = GameProgress.instance_;
        GAME_PROGRESS _progress = GAME_PROGRESS.OVER;//_g_progress.GetNowProgress();

#if UNITY_EDITOR
        txt_result_.text = _progress.ToString();
#endif

        if (_progress == GAME_PROGRESS.CLEAR)
        {
            img_result_.sprite = img_clears[_g_progress.GetFriendsWhoHelped().Count(n => n) - 3];    // ƒNƒŠƒAÅ’áğŒ”•ªˆø‚­
        }
        else if (_progress == GAME_PROGRESS.OVER)
        {
            img_result_.sprite = img_clears[3];
        }

        dialog_.color = new Color(1, 1, 1, 0);

        StartCoroutine(animation_.Scroll(OpenDialog()));
    }

    IEnumerator OpenDialog()
    {
        while (dialog_.color.a < max_dialog_fade_)
        {
            dialog_.color += new Color(0, 0, 0, speed_dialog_fade_);
            yield return null;
        }

        yield return null;
    }
}
