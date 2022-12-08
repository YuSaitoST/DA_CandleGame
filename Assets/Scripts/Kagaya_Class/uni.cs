using UnityEngine;

public class uni : MonoBehaviour
{
    Rigidbody rigidbody;
    private GameObject playerC;
    float trackingRange = 1.3f;
    const float angle = 360f;
    bool tracking = false;
    float time = 0.0f;
    UniState state;

    //[SerializeField]
    //UniState unistate;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        time = 0.0f;
        playerC = GameProgress.instance_.Get_PlayerC();
        state = GetComponentInChildren<UniState>();
    }

    void Update()
    {
        
        if (tracking == true)
        {
            float dist = Vector3.Distance(playerC.transform.position, transform.position);
            if (dist > trackingRange)
            {
                Debug.Log("外れた");

                time += Time.deltaTime;
                if (time >= 5.0f)
                {
                    Debug.Log("すいちゃん");
                    tracking = false;
                    time = 0.0f;
                    state.SmallMode();
                }
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
                //if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
                //{
                //    if (hit.collider == other)
                //    {
                        tracking = true;
                        Debug.Log("range of view");
                        state.BigMode();
                    
                
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