using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFootPrint : MonoBehaviour
{
    [SerializeField] GameObject pref_footPrint_ = null;
    static float createTime_ = 1.5f;
    float time_ = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time_ += Time.deltaTime;
        if (createTime_ < time_)
        {
            time_ = 0.0f;
            Instantiate(pref_footPrint_, transform.position, transform.rotation);
        }
    }
}
