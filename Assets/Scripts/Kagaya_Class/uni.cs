using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class uni : MonoBehaviour
{
    new Rigidbody rigidbody;
    const float trackingRange = 3f;
    const float angle = 360f;
    bool tracking = false;
    float time = 5.0f;
    public Transform player;

    //public Vector3 scale;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        time = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            //追跡の時、trackingRangeより距離が離れたら中止
            if (dist > trackingRange)
            {
                tracking = false;
                time = 5.0f;
            }
                
        }
        else
        {
            time -= Time.deltaTime;

            if (time <= 0.0f)
            {
                this.GetComponent<Renderer>().material.color = Color.gray;
                this.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
        {
            //視界の角度内に収まっているか
            Vector3 posDelta = other.transform.position - this.transform.position;
            if (posDelta.magnitude > trackingRange)
                return;

            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle) //target_angleがangleに収まっているかどうか
            {
                if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
                {
                    if (hit.collider == other)
                    {
                        tracking = true;
                        Debug.Log("range of view");
                        this.GetComponent<Renderer>().material.color = Color.red;
                        this.transform.localScale = new Vector3(2, 2, 2);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        //trackingRangeの範囲を赤いワイヤーフレームで示す
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
}
