using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPlayer : MonoBehaviour
{
    ParticleSystem ps_ = null;

    // Start is called before the first frame update
    void Start()
    {
        ps_ = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 自身のパーティクルを再生する
    /// </summary>
    public void PlayOneShot()
    {
        if (ps_.isStopped)
        {
            ps_.Play();
            StartCoroutine(IsFine());
        }
    }

    public bool IsFined()
    {
        return ps_.isStopped;
    }

    IEnumerator IsFine()
    {
        while (!ps_.isStopped)
        {
            yield return null;
        }
    }
}
