using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEOneShotPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource_ = null;
    [SerializeField] AudioClip se_ = null;
    bool flag_ = false;

    /// <summary>
    /// SE���Z�b�g����
    /// </summary>
    /// <param name="se">SE�f�[�^</param>
    /// <returns>�C���X�^���X</returns>
    public SEOneShotPlayer SetSEData(AudioClip se)
    {
        se_ = se;
        return this;
    }

    /// <summary>
    /// SE��P���Đ�
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
