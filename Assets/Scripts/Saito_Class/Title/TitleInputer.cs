using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInputer : MonoBehaviour
{
    [SerializeField] string sceneName_ = null;
    [SerializeField] AudioSource bgm_audioSource_ = null;
    [SerializeField] AudioSource se_audioSource_ = null;
    [SerializeField] AudioClip se_detection_ = null;
    bool flg_ = false;

    // Start is called before the first frame update
    void Start()
    {
        bgm_audioSource_.volume = 1;
        flg_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flg_)
        {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            flg_ = true;
            StartCoroutine(GoNextScene());
        }
    }

    IEnumerator GoNextScene()
    {
        se_audioSource_.PlayOneShot(se_detection_);

        while (se_audioSource_.isPlaying)
        {
            bgm_audioSource_.volume = Mathf.Max(bgm_audioSource_.volume - 0.016f, 0.0f);

            yield return null;
        }

        yield return null;

        FadeManager.Instance.LoadScene(sceneName_, 2.0f);
    }
}
