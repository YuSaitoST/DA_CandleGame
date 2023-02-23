using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FellowPatrol : MonoBehaviour
{
    //[SerializeField]
    private Fellow fellow_script_ = null;

    // NavMeshAgentコンポーネント
    private NavMeshAgent agent_ = null;

    //アタッチされているオブジェクトのアニメーター(ない場合は階層の下にある可能性あり)
    [SerializeField]
    private Animator animator_ = null;

    [SerializeField]
    [Tooltip("巡回する地点の配列")]
   // private Transform[] waypoint_ = null;
    private List<Transform> waypoint_ = new List<Transform> {};

    [SerializeField]
    private float wait_time_ = 3.0f;

    // 現在の目的地
    private int waypoint_index = 0;

   
    // Start is called before the first frame update
    void Start()
    {
        //目的地が設定されていない場合はこのスクリプトはオフにする
        if(waypoint_.Count == 0)
        {
            this.enabled = false;
        }

        fellow_script_ = GetComponent<Fellow>();
        agent_ = GetComponent<NavMeshAgent>();
       
        // 最初の目的地を入れる
        agent_.SetDestination(waypoint_[0].position);

        animator_.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!fellow_script_.Fellow_flg_)
        {
           
            // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
            if (agent_.remainingDistance <= agent_.stoppingDistance)
            {
                StartCoroutine(NextWayPoint());
            }
        }
        else
        {
            StopCoroutine(NextWayPoint());
            //アニメーションストップ
            animator_.SetBool("isWalking", false);
           
            //このコンポーネントを非アクティブにする
            this.enabled = false;
        }
    }

    private IEnumerator NextWayPoint()
    {
        //アニメーションストップ
        animator_.SetBool("isWalking", false);

        //移動停止する
        agent_.isStopped = true;

        // 目的地の番号を１更新（右辺を剰余演算子にすることで目的地をループさせれる）
        waypoint_index = (waypoint_index + 1) % waypoint_.Count;

        // 目的地を次の場所に
        agent_.SetDestination(waypoint_[waypoint_index].position);

        //指定された時間だけ待機
        yield return new WaitForSeconds(wait_time_);  

        //移動再開
        agent_.isStopped = false;

        //アニメーション再開
        animator_.SetBool("isWalking", true);
    }

    //配列の受け取り用
    public void SetWayPoint(Transform[] _a)
    {
        //リストの数だけまわす
        for(int i = 0; i < _a.Length; i++)
        {
            //リストの初期化
            waypoint_ = new List<Transform>();

            //リストに追加
            waypoint_.Add(_a[i]);
        }
    }
}
