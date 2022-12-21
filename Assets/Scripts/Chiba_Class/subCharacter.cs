using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class subCharacter : MonoBehaviour
{
 
    private NavMeshAgent agent_;
    // �ǂ�������L�����N�^�[
    [SerializeField]
    private GameObject player_;
    private Animator animator_;
    //�@���������Ƃ��鋗��
    [SerializeField]
    private float arrivedDistance_ = 1.5f;
    //�@�ǂ������n�߂鋗��
    [SerializeField]
    private float followDistance_ = 1f;
    // Start is called before the first frame update
    void Start()
    {
       animator_ = GetComponent<Animator>();
        agent_ = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent_.SetDestination(player_.transform.position);

        //�@�������Ă��鎞
        if (agent_.remainingDistance < arrivedDistance_)
        {
            //agent.Stop();
            //�@Unity5.6�o�[�W�����ȍ~�̒�~
            agent_.isStopped = true;
            animator_.SetFloat("Speed", 0f);
            //�@�������Ă��Ȃ����Œǂ������o�������ɂȂ�����
        }
        else if (agent_.remainingDistance > followDistance_)
        {
            //agent.Resume();
            //�@Unity5.6�o�[�W�����ȍ~�̍ĊJ
            agent_.isStopped = false;
            animator_.SetFloat("Speed", agent_.desiredVelocity.magnitude);
        }
    }
    void OnAnimatorIK()
    {
        animator_.SetLookAtWeight(1f, 0.3f, 1f, 0f, 0.5f);
        animator_.SetLookAtPosition(player_.transform.position + Vector3.up * 1.5f);
    }

}
