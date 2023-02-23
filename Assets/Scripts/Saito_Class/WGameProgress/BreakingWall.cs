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
            Vector3 _pos = transform.position;
            _pos.y = 0.5f;

            // エフェクト生成
            Instantiate(pref_breakEffect_, _pos, Quaternion.identity)
                .GetComponent<ParticleSystem>()
                .Play();

            // SE生成
            Instantiate(pref_sePlayer_, transform.position, Quaternion.identity)
                .GetComponent<SEOneShotPlayer>()
                .SetSEData(se_break_)
                .PlayOneShot();

            Destroy(gameObject);
        }
    }
}
