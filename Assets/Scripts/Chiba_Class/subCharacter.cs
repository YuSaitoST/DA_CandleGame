using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class subCharacter : MonoBehaviour
{
 
    private NavMeshAgent agent_;
    // 追いかけるキャラクター
    [SerializeField]
    private GameObject player_;
    private Animator animator_;
    //　到着したとする距離
    [SerializeField]
    private float arrivedDistance_ = 1.5f;
    //　追いかけ始める距離
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

        //　到着している時
        if (agent_.remainingDistance < arrivedDistance_)
        {
            //agent.Stop();
            //　Unity5.6バージョン以降の停止
            agent_.isStopped = true;
            animator_.SetFloat("Speed", 0f);
            //　到着していない時で追いかけ出す距離になったら
        }
        else if (agent_.remainingDistance > followDistance_)
        {
            //agent.Resume();
            //　Unity5.6バージョン以降の再開
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
