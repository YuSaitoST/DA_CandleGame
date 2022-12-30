using System.Collections;
using UnityEngine;
using Fungus;

public class FelloTalk : MonoBehaviour
{
    [SerializeField, Tooltip("Flow Chat")] Flowchart flowchart_ = null;
    [SerializeField, Tooltip("ダイアログ")] GameObject dialog_ = null;
    [SerializeField, Tooltip("キャラ画像")] GameObject character_ = null;
    [SerializeField, Tooltip("ダイアログ前の会話ブロック名")] string message_front_ = "";
    [SerializeField, Tooltip("ダイアログ後の会話ブロック名")] string message_back_ = "";
    int count_ = 0;

    void Start()
    {
        dialog_.SetActive(false);
        count_ = 0;
    }

    /// <summary>
    /// 会話開始
    /// </summary>
    public void PlayTalk()
    {
        if (count_ == 0)
        {
            count_ += 1;
            PauserObject.Pause();
            flowchart_.SendFungusMessage(message_front_);
        }
    }

    /// <summary>
    /// キャラクターを表示させる
    /// </summary>
    public void OpenCharacter()
    {
        character_.SetActive(true);
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
        character_.SetActive(false);
        PauserObject.Resume();
    }

    IEnumerator StayNextInput()
    {

        while (!(Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetButton("Fire3")))
        {
            yield return null;
        }

        dialog_.SetActive(false);
        character_.SetActive(false);

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