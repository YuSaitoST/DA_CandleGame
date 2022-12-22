using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class FelloTalk : MonoBehaviour
{
    [SerializeField] string message_ = "";
    Flowchart flowChart_ = null;
    bool isTalking_ = false;


    // Start is called before the first frame update
    void Start()
    {
        flowChart_ = GetComponent<Flowchart>();
    }

    public void Talk()
    {
        StartCoroutine(TalkStart());
    }

    IEnumerator TalkStart()
    {

        if (isTalking_)
        {
            yield return null;
        }

        isTalking_ = true;

        PauserObject.Pause();

        flowChart_.SendFungusMessage(message_);

        yield return new WaitUntil(() =>
            flowChart_.GetExecutingBlocks().Count == 0);

        isTalking_ = false;

        PauserObject.Resume();
    }
}

/*
 * [アセットサイト]
 * https://github.com/snozbot/fungus/releases/tag/v3.13.7
 */