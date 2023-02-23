using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����C��
public class Snake00 : MonoBehaviour
{
    [Header("�����X�s�[�h")]
    [SerializeField]
    private float speed_ = 3.0f;

    [SerializeField]
    private Animator animator_ = null;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void EndAnimation()
    {
        StartCoroutine(OnAnimation());
       
    }

    IEnumerator OnAnimation()
    {
       
        yield return new WaitForSeconds(speed_);
        animator_.SetTrigger("Attack");
    }
}
