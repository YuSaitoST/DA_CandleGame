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
        PauserObject.Pause();
        flowchart_.SendFungusMessage(message_front_);
    }

    /// <summary>
    /// ダイアログ表示(Flowchat側から呼ぶ)
    /// </summary>
    public void OpenDialog()
    {
        dialog_.SetActive(true);
        StartCoroutine(StayNextInput());
    }

    public void AnPauserObject()
    {
        PauserObject.Resume();
    }

    IEnumerator StayNextInput()
    {

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        dialog_.SetActive(false);

        yield return null;

        if (message_back_ != "")
        {
            flowchart_.SendFungusMessage(message_back_);
        }
        else
        {
            PauserObject.Resume();
        }

        yield return null;
    }
}

/*
 * [アセットサイト]
 * https://github.com/snozbot/fungus/releases/tag/v3.13.7
 */