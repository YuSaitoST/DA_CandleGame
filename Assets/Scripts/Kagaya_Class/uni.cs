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
                Debug.Log("�O�ꂽ");

                time += Time.deltaTime;
                if (time >= 5.0f)
                {
                    Debug.Log("���������");
                    tracking = false;
                    time = 0.0f;
                    state.SmallMode();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //���E�͈͓̔��̓����蔻��
        {
            Vector3 posDelta = other.transform.position - this.transform.position;
            if (posDelta.magnitude > trackingRange)
                return;
            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle) //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
            {
                //if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
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
        //trackingRange�͈̔͂�Ԃ����C���[�t���[���Ŏ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
}