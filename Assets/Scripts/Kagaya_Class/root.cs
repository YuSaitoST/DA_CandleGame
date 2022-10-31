using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NavMeshAgent�g���Ƃ��ɕK�v
using UnityEngine.AI;
using UnityEditor;

//�I�u�W�F�N�g��NavMeshAgent�R���|�[�l���g��ݒu
[RequireComponent(typeof(NavMeshAgent))]

public class root : MonoBehaviour
{
    Rigidbody rigidbody;
    public Transform[] points;
    [SerializeField] int destPoint = 0;
    private NavMeshAgent agent;

    public float angle = 120f;
    Vector3 playerPos;
    GameObject enemy;
    float distance;
    [SerializeField] float trackingRange = 5f;
    [SerializeField] float quitRange = 7f;
    [SerializeField] bool tracking = false;
    [SerializeField] private SphereCollider searchArea;
    [SerializeField] private float searchAngle = 90f;

    void Start()

    {
        rigidbody = this.GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        // autoBraking �𖳌��ɂ���ƁA�ڕW�n�_�̊Ԃ��p���I�Ɉړ����܂�
        //(�܂�A�G�[�W�F���g�͖ڕW�n�_�ɋ߂Â��Ă�
        // ���x�����Ƃ��܂���)
        agent.autoBraking = false;

        GotoNextPoint();

        //�ǐՂ������I�u�W�F�N�g�̖��O������
        enemy = GameObject.Find("Player");
        //ghost2 = GameObject.Find("Cube1");
    }

    void GotoNextPoint()
    {
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ��܂�
        if (points.Length == 0)
            return;

        // �G�[�W�F���g�����ݐݒ肳�ꂽ�ڕW�n�_�ɍs���悤�ɐݒ肵�܂�
        agent.destination = points[destPoint].position;

        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ肵�A
        // �K�v�Ȃ�Ώo���n�_�ɂ��ǂ�܂�
        destPoint = (destPoint + 1) % points.Length;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player") //���E�͈͓̔��̓����蔻��
    //    {
    //        //���E�̊p�x���Ɏ��܂��Ă��邩
    //        Vector3 posDelta = other.transform.position - this.transform.position;
    //        float target_angle = Vector3.Angle(this.transform.forward, posDelta);

    //        if (target_angle < angle) //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
    //        {
    //            if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
    //            {
    //                if (hit.collider == other)
    //                {
    //                    Debug.Log("range of view");
    //                }
    //            }
    //        }
    //    }
    //}

    
    void Update()
    {
        //Player�Ƃ��̃I�u�W�F�N�g�̋����𑪂�
        playerPos = enemy.transform.position;
        
        distance = Vector3.Distance(this.transform.position, playerPos);

        


        if (tracking)
        {
            //�ǐՂ̎��AquitRange��苗�������ꂽ�璆�~
            if (distance > quitRange)
                tracking = false;

            //Player��ڕW�Ƃ���
            agent.destination = playerPos;
        }
        else
        {
            //Player��trackingRange���߂Â�����ǐՊJ�n
            if (distance < trackingRange)
                tracking = true;

            // �G�[�W�F���g�����ڕW�n�_�ɋ߂Â��Ă�����A
            // ���̖ڕW�n�_��I�����܂�
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }

        DoMove(agent.destination);

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //trackingRange�͈̔͂�Ԃ����C���[�t���[���Ŏ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        //quitRange�͈̔͂�����C���[�t���[���Ŏ���
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, quitRange);
    }

    private void DoMove(Vector3 targetPosition)
    {
        if (agent && agent.enabled)
        {
            agent.SetDestination(targetPosition);

            foreach (var pos in agent.path.corners)
            {
                var diff2d = new Vector2(
                    Mathf.Abs(pos.x - transform.position.x),
                    Mathf.Abs(pos.z - transform.position.z)
                );

                if (0.1f <= diff2d.magnitude)
                {
                    targetPosition = pos;
                    break;
                }
            }

            Debug.DrawLine(transform.position, targetPosition, Color.red);
        }

        Quaternion moveRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
        moveRotation.z = 0;
        moveRotation.x = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, 0.1f);
        //*������enemy�̑�������
        float forward_x = transform.forward.x * 4;
        float forward_z = transform.forward.z * 4;

        rigidbody.velocity = new Vector3(forward_x, rigidbody.velocity.y, forward_z);
    }
#endif
}
