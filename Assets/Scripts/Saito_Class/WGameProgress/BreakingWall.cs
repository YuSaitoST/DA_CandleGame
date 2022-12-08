using System.Collections;
using UnityEngine;

public class BreakingWall : MonoBehaviour
{
    [SerializeField] GameObject pref_breakEffect_ = null;
    [SerializeField] GameObject pref_sePlayer_ = null;
    [SerializeField] AudioClip se_break_ = null;


    public void OnTriggerEnter(Collider other)
    {
        string tags = other.tag.Substring(0, other.tag.Length - 2);
        if(tags == "OxyBomb")
        {
            // �G�t�F�N�g����
            Instantiate(pref_breakEffect_, transform.position, Quaternion.identity)
                .GetComponent<ParticleSystem>()
                .Play();

            // SE����
            Instantiate(pref_sePlayer_, transform.position, Quaternion.identity)
                .GetComponent<SEOneShotPlayer>()
                .SetSEData(se_break_)
                .PlayOneShot();

            Destroy(gameObject);
        }
    }
}
