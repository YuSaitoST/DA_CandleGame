using System.Collections;
using UnityEngine;

public class BreakingWall : MonoBehaviour
{
    [SerializeField] GameObject pref_breakEffect_ = null;


    public void OnTriggerEnter(Collider other)
    {
        string tags = other.tag.Substring(0, other.tag.Length - 2);
        if(tags == "OxyBomb")
        {
            //ÉvÉåÉnÉuê∂ê¨
            GameObject _obj = Instantiate(pref_breakEffect_, transform.position, Quaternion.identity);
            ParticleSystem _particle = _obj.GetComponent<ParticleSystem>();
            _particle.Play();

            Destroy(gameObject);
        }
    }
}
