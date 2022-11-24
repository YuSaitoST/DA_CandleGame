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
            //�ǐՂ̎��AtrackingRange��苗�������ꂽ�璆�~
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
        if (other.gameObject.tag == "Player") //���E�͈͓̔��̓����蔻��
        {
            //���E�̊p�x���Ɏ��܂��Ă��邩
            Vector3 posDelta = other.transform.position - this.transform.position;
            if (posDelta.magnitude > trackingRange)
                return;

            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle) //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
            {
                if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
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
        //trackingRange�͈̔͂�Ԃ����C���[�t���[���Ŏ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
}
