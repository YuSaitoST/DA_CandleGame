using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake01 : MonoBehaviour
{
    [Header("ƒXƒ^ƒ“ŽžŠÔ")]
    [SerializeField]
    private float stan_time_ = 5.0f;

    [SerializeField]
    private Animator animator_ = null;

    [SerializeField]
    private bool stan_flg_ = false;

   // private float time_ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    //// Update is called once per frame
    //void FixedUpdate()
    //{  

    //}

    public void EndAnimation()
    {
        animator_.SetBool("isDamage", false);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!stan_flg_)
        {

            if (collision.gameObject.tag == "Player")
            {
                StartCoroutine(OnStan());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stan_flg_)
        {
            if (other.gameObject.tag == "OxyBomb00")
            {
                StartCoroutine(OnStan());
            }

            if (other.gameObject.tag == "OxyBomb01")
            {
                StartCoroutine(OnStan());
            }

            if (other.gameObject.tag == "OxyBomb02")
            {
                StartCoroutine(OnStan());
            }

            if (other.gameObject.tag == "OxyBomb03")
            {
                StartCoroutine(OnStan());
            }

            if (other.gameObject.tag == "OxyBomb04")
            {
                StartCoroutine(OnStan());
            }

            if (other.gameObject.tag == "OxyBomb05")
            {
                StartCoroutine(OnStan());
            }
        }
    }

    //private void Stan()
    //{
        
        
    //}

    IEnumerator OnStan()
    {
        Debug.Log("‚Ô‚Â‚©‚Á‚½");
        animator_.SetBool("isDamage", true);
        stan_flg_ = true;

        yield return new WaitForSeconds(stan_time_);
        animator_.SetBool("isDamage", false);
        stan_flg_ = false;
    }
}
