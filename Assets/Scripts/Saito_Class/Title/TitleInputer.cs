using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInputer : MonoBehaviour
{
    [SerializeField] string sceneName_ = null;
    bool flg_ = false;

    // Start is called before the first frame update
    void Start()
    {
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
            FadeManager.Instance.LoadScene(sceneName_, 2.0f);
        }
    }
}
