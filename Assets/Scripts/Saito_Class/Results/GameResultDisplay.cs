using System.
    Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameResultDisplay : MonoBehaviour
{
    [SerializeField] Image img_result_ = null;
    [SerializeField] Sprite[] img_clears = null;

    [SerializeField] BG_Scroll animation_ = null;
    [SerializeField] GameObject panel_lose_ = null;
    [SerializeField] Image dialog_ = null;

    [SerializeField] float speed_dialog_fade_ = 0.016f;
    [SerializeField] float max_dialog_fade_ = 0.73f;
    [SerializeField] float stay_loseDialog_time_ = 1.5f;

    int inputCount_ = 0;


    // Start is called before the first frame update
    void Start()
    {
        GameProgress _g_progress = GameProgress.instance_;
        GAME_PROGRESS _progress;

#if UNITY_EDITOR
        _progress = GAME_PROGRESS.CLEAR;
#else
        _progress = _g_progress.GetNowProgress();
#endif

        inputCount_ = 0;

        dialog_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        if (_progress == GAME_PROGRESS.CLEAR)
        {
            panel_lose_.SetActive(false);

#if UNITY_EDITOR
            img_result_.sprite = img_clears[1];    // クリア最低条件数分引く
#else
            img_result_.sprite = img_clears[_g_progress.GetFriendsWhoHelped().Count(n => n) - 3];    // クリア最低条件数分引く
#endif
            StartCoroutine(animation_.PlayOneShot(OpenDialog()));
        }
        else if (_progress == GAME_PROGRESS.OVER)
        {
            panel_lose_.SetActive(true);
            img_result_.sprite = img_clears[3];
            StartCoroutine(Lose());
        }
    }

    private void Update()
    {
        if (inputCount_ == 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                inputCount_ += 1;
                FadeManager.Instance.LoadScene("TitleScene", 2.0f);
            }
        }
    }

    IEnumerator Lose()
    {
        yield return new WaitForSeconds(stay_loseDialog_time_);

        StartCoroutine(OpenDialog());

        yield return null;
    }

    IEnumerator OpenDialog()
    {
        while (dialog_.color.a < max_dialog_fade_)
        {
            dialog_.color += new Color(0.0f, 0.0f, 0.0f, speed_dialog_fade_);
            yield return null;
        }

        yield return null;
    }
}
