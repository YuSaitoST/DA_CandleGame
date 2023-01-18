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
        GameProgress _g_progress = GameProgress.instance_;
        GAME_PROGRESS _progress = _g_progress.GetNowProgress();

#if UNITY_EDITOR
        txt_result_.text = _progress.ToString();
#endif

        if (_progress == GAME_PROGRESS.CLEAR)
        {
            int _count = 0;
            bool[] _list = _g_progress.GetFriendsWhoHelped();
            foreach (bool help in _list)
            {
                _count += help ? 1 : 0;
            }
            img_result_.sprite = img_clears[_count - 3];    // ÉNÉäÉAç≈í·èåèêîï™à¯Ç≠
        }
        else if (_progress == GAME_PROGRESS.OVER)
        {
            img_result_.sprite = img_clears[3];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
