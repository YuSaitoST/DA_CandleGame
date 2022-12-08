using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersSEPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource_ = null;
    [SerializeField] AudioClip se_partGet_ = null;
    [SerializeField] AudioClip se_tankGet_ = null;
    [SerializeField] AudioClip se_damage_ = null;


    public void PartGet()
    {
        audioSource_.PlayOneShot(se_partGet_);
    }

    public void TankGet()
    {
        audioSource_.PlayOneShot(se_tankGet_);
    }

    public void TakeDamage()
    {
        audioSource_.PlayOneShot(se_damage_);
    }
}
