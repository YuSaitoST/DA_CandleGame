using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRemovalEffect : MonoBehaviour
{
    ParticleSystem ps_;

    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioClip clip = null;


    // Start is called before the first frame update
    void Start()
    {
        ps_ = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (ps_.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
