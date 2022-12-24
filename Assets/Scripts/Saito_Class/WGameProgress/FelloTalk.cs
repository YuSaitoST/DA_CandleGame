using System.Collections;
using UnityEngine;
using Fungus;

public class FelloTalk : MonoBehaviour
{
    [SerializeField, Tooltip("Flow Chat")] Flowchart flowchart_ = null;
    [SerializeField, Tooltip("ダイアログ")] GameObject dialog_ = null;
    [SerializeField, Tooltip("ダイアログ前の会話ブロック名")] string message_front_ = "";
    [SerializeField, Tooltip("ダイアログ後の会話ブロック名")] string message_back_ = "";


    void Start()
    {
        dialog_.SetActive(false);
    }

    /// <summary>
    /// 会話開始
    /// </summary>
    public void PlayTalk()
    {
        flowchart_.SendFungusMessage(message_front_);
    }

    /// <summary>
    /// ダイアログ表示
    /// </summary>
    public void OpenDialog()
    {
        dialog_.SetActive(true);
        StartCoroutine(StayNextInput());
    }

    IEnumerator StayNextInput()
    {

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        dialog_.SetActive(false);

        yield return null;
        flowchart_.SendFungusMessage(message_back_);

        yield return null;
    }
}

/*
 * [アセットサイト]
 * https://github.com/snozbot/fungus/releases/tag/v3.13.7
 */