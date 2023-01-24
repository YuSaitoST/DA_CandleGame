using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector3 transform_origin_;

    private Vector3 a;
    private Vector3 b;

    [Header("trueだと下がる")]
    [SerializeField]
    private bool back_  = false;

    [Header("trueの間動かなくなる")]
    [SerializeField]
    private bool bomb_hit_ = false;

    [SerializeField]
    private Animator animator_ = null;

    [Header("往復スピード")]
    [SerializeField]
    private float speed_ = 0.005f;

    [Header("ひるむ時間")]
    [SerializeField]
    private float[] shrink_ =
    {
        1.0f,
        1.4f,
        1.8f,
        2.2f,
        4.0f,
        5.0f
    };

    [SerializeField]
    private float shrink_time_ = 0.0f;
    [SerializeField]
    private float time_ = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
       
        transform_origin_ = transform.position;
        //rb_ = GetComponent<Rigidbody>();

        a = transform_origin_+(transform.TransformDirection(Vector3.forward) *0.3f);

        b = transform_origin_ + (transform.TransformDirection(Vector3.back) * 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       

        if (!bomb_hit_)
        {
            animator_.SetBool("isDamage", false);

            time_ += Time.deltaTime;

            //if (time_ >= 3.0f && !back_)
            //{
            //    back_ = true;
            //    time_ = 0.0f;
            //}
            //else if (time_ >= 3.0f&&back_)
            //{
            //    back_ = false;
            //    time_ = 0.0f;
            //}

            if (a==transform.position)
            {
                back_ = true;
                time_ = 0.0f;
            }
            else if (b==transform.position)
            {
                back_ = false;
                time_ = 0.0f;
            }

            if (!back_)
            {

                transform.position += transform.TransformDirection(Vector3.forward) * speed_;
                animator_.SetBool("isAttack", true);

            }
            else if(back_)
            {
                transform.position += transform.TransformDirection(Vector3.back) * speed_;
                animator_.SetBool("isAttack", false);
            }
        }
        else
        {
            transform.position = b;
            back_ = false;
        }

        if (shrink_time_ >= 0.0f)
        {
            shrink_time_ -= Time.deltaTime;
        }
        else
        {
            shrink_time_ = 0.0f;
            bomb_hit_ = false;
        }

     

    }

   
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "OxyBomb00"&& !bomb_hit_)
        {
            Process(0);
            
        }

        if (collision.gameObject.tag == "OxyBomb01" && !bomb_hit_)
        {
            Process(1);

        }

        if (collision.gameObject.tag == "OxyBomb02" && !bomb_hit_)
        {
            Process(2);

        }

        if (collision.gameObject.tag == "OxyBomb03" && !bomb_hit_)
        {
            Process(3);

        }

        if (collision.gameObject.tag == "OxyBomb04" && !bomb_hit_)
        {
            Process(4);

        }

        if (collision.gameObject.tag == "OxyBomb05" && !bomb_hit_)
        {
            Process(5);

        }
    }

    private void Process(int a)
    {

       
        back_ = true;
        bomb_hit_ = true;
        shrink_time_ = shrink_[a];

        animator_.SetBool("isDamage", true);
    }
}
