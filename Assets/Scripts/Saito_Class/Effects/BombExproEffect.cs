using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExproEffect : MonoBehaviour
{
    AudioSource audioSource_ = null;

    private void OnEnable()
    {
        audioSource_ = GetComponent<AudioSource>();
        GameProgress.instance_.CameraShake();
    }

    private void Update()
    {
        if (!audioSource_.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
