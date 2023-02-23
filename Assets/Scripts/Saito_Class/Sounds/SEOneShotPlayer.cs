using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEOneShotPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource_ = null;
    [SerializeField] AudioClip se_ = null;
    bool flag_ = false;

    /// <summary>
    /// SEをセットする
    /// </summary>
    /// <param name="se">SEデータ</param>
    /// <returns>インスタンス</returns>
    public SEOneShotPlayer SetSEData(AudioClip se)
    {
        se_ = se;
        return this;
    }

    /// <summary>
    /// SEを単発再生
    /// </summary>
    public void PlayOneShot()
    {
        if (!flag_)
        {
            flag_ = true;
            audioSource_.PlayOneShot(se_);
        }
    }
}
