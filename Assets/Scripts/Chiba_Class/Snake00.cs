using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake00 : MonoBehaviour
{
    [Header("往復スピード")]
    [SerializeField]
    private float speed_ = 3.0f;

    [SerializeField]
    private Animator animator_ = null;

    private float time_ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time_ += Time.deltaTime;

        if (time_ >= speed_)
        {
            animator_.SetBool("isAttack", true);
            
        }
       

    }

    public void EndAnimation()
    {
        animator_.SetBool("isAttack", false);
        time_ = 0.0f;
    }
}
