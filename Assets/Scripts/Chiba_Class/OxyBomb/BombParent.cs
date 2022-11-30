using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombParent : MonoBehaviour
{
    [SerializeField]
    private bool flg_ = false;

    [SerializeField]
    private GameObject exp_ = null;

    [SerializeField]
    private GameObject parent_ = null;

    [SerializeField]
    private float exp_count_ = 3.0f;



    private void Start()
    {
        exp_.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if(flg_)
        {
            StartCoroutine(Exp());
        }
    }

    IEnumerator Exp()
    {
        yield return new WaitForSeconds(exp_count_);
        exp_.SetActive(true);
        Debug.Log("”š”­");
        exp_.SetActive(false);
        Destroy(parent_.gameObject);

    }


    private void OnCollisionEnter(Collision collision)
    {
        //var _layer_name = LayerMask.LayerToName(collision.gameObject.layer);

        //if (_layer_name == "Default")
        //{

        //    // Destroy(collision.gameObject);
        //}


        if (collision.gameObject.tag == "Ground")
        {
            flg_ = true;

            Debug.Log("”š”­");
        }
    }

}

    
