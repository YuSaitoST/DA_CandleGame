using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NavMeshAgent使うときに必要
using UnityEngine.AI;
using UnityEditor;

//オブジェクトにNavMeshAgentコンポーネントを設置
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

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;

        GotoNextPoint();

        //追跡したいオブジェクトの名前を入れる
        enemy = GameObject.Find("Player");
        //ghost2 = GameObject.Find("Cube1");
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        agent.destination = points[destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % points.Length;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
    //    {
    //        //視界の角度内に収まっているか
    //        Vector3 posDelta = other.transform.position - this.transform.position;
    //        float target_angle = Vector3.Angle(this.transform.forward, posDelta);

    //        if (target_angle < angle) //target_angleがangleに収まっているかどうか
    //        {
    //            if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
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
        //Playerとこのオブジェクトの距離を測る
        playerPos = enemy.transform.position;
        
        distance = Vector3.Distance(this.transform.position, playerPos);

        


        if (tracking)
        {
            //追跡の時、quitRangeより距離が離れたら中止
            if (distance > quitRange)
                tracking = false;

            //Playerを目標とする
            agent.destination = playerPos;
        }
        else
        {
            //PlayerがtrackingRangeより近づいたら追跡開始
            if (distance < trackingRange)
                tracking = true;

            // エージェントが現目標地点に近づいてきたら、
            // 次の目標地点を選択します
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }

        DoMove(agent.destination);

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //trackingRangeの範囲を赤いワイヤーフレームで示す
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        //quitRangeの範囲を青いワイヤーフレームで示す
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
        //*ここでenemyの速さ調節
        float forward_x = transform.forward.x * 4;
        float forward_z = transform.forward.z * 4;

        rigidbody.velocity = new Vector3(forward_x, rigidbody.velocity.y, forward_z);
    }
#endif
}
