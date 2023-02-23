using System.Collections;
using UnityEngine;

public class BombParent : MonoBehaviour
{
    [SerializeField]
    private bool flg_ = false;

    [SerializeField]
    private GameObject exp_ = null;

    [SerializeField]
    private GameObject bomb_obj_ = null;

    [SerializeField]
    private float exp_count_ = 3.0f;

    IEnumerator Exp()
    {
        yield return new WaitForSeconds(exp_count_);
        bomb_obj_.SetActive(false);
        exp_.SetActive(true);

        GameProgress.instance_.CameraShake();

        yield return new WaitForSeconds(0.7f);
        exp_.SetActive(false);
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            flg_ = true;
            StartCoroutine(Exp());

        }
    }

}

    
