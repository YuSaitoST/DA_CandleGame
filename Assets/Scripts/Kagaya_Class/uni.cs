using UnityEngine;

public class uni : MonoBehaviour
{
    Rigidbody rigidbody;
    private GameObject playerC;
    float trackingRange = 1.0f;
    const float angle = 360f;
    bool tracking = false;
    float time = 0.0f;
    UniState state;

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
                time += Time.deltaTime;
                if (time >= 5.0f)
                {
                    tracking = false;
                    time = 0.0f;
                    state.SmallMode();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Vector3 posDelta = other.transform.position - this.transform.position;
            if (posDelta.magnitude > trackingRange)
                return;
            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle) 
            {
               tracking = true;
               Debug.Log("range of ƒEƒj");
               state.BigMode();
            }
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
#endif
}