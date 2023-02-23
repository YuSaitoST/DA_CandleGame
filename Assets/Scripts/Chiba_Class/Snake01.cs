using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake01 : MonoBehaviour
{
    [Header("ƒXƒ^ƒ“ŽžŠÔ")]
    [SerializeField]
    private float[] stan_time_ =
        {
          5.0f,
          6.0f,
          7.0f,
          8.0f,
          9.0f,
          10.0f
           

        };

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
                StartCoroutine(OnStan(0));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stan_flg_)
        {
            if (other.gameObject.tag == "OxyBomb00")
            {
                StartCoroutine(OnStan(0));
            }

            if (other.gameObject.tag == "OxyBomb01")
            {
                StartCoroutine(OnStan(1));
            }

            if (other.gameObject.tag == "OxyBomb02")
            {
                StartCoroutine(OnStan(2));
            }

            if (other.gameObject.tag == "OxyBomb03")
            {
                StartCoroutine(OnStan(3));
            }

            if (other.gameObject.tag == "OxyBomb04")
            {
                StartCoroutine(OnStan(4));
            }

            if (other.gameObject.tag == "OxyBomb05")
            {
                StartCoroutine(OnStan(5));
            }
        }
    }

    //private void Stan()
    //{
        
        
    //}

    IEnumerator OnStan(int _bomb)
    {
        Debug.Log("‚Ô‚Â‚©‚Á‚½");
        animator_.SetBool("isDamage", true);
        stan_flg_ = true;

        yield return new WaitForSeconds(_bomb);
        animator_.SetBool("isDamage", false);
        stan_flg_ = false;
    }
}
