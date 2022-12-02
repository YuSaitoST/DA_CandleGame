using UnityEngine;

public class uni : MonoBehaviour
{
    Rigidbody rigidbody;
    const float trackingRange = 2.5f;
    const float angle = 360f;
    bool tracking = false;
    float time = 5.0f;
    private GameObject playerC;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        time = 5.0f;
        playerC = GameProgress.instance_.Get_PlayerC();
    }

    void Update()
    {
        if (tracking)
        {
            float dist = Vector3.Distance(playerC.transform.position, transform.position);
            if (dist > trackingRange)
            {
                Debug.Log("外れた");
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