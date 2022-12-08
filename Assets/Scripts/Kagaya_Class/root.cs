using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

//オブジェクトにNavMeshAgentコンポーネントを設置
[RequireComponent(typeof(NavMeshAgent))]

public class root : MonoBehaviour
{
    //private Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
    //const float circle_angle = 50;

    Mesh mesh;
    Vector3[] vertices;

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]

    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);

    Rigidbody rigidbody;
    private NavMeshAgent agent;

    //[SerializeField]
    //[Tooltip("追いかける対象")]
    private GameObject playerC;
    const float angle = 90f;
    Vector3 playerPos;

    const float trackingRange = 5.5f;
    //const float quitRange = 5f;
    bool tracking = false;
    //[SerializeField] private CapsuleCollider searchArea;
    //const float searchAngle = 90f;

    //[SerializeField, Range(0.0f, 360.0f)]
    const float widthAngle = 90.0f;
    //[SerializeField, Range(0.0f, 360.0f)]
    const float heightAngle = 0.0f;
    //[SerializeField, Range(0.0f, 15.0f)]
    const float length = 5.0f;

    public float WidthAngle { get { return widthAngle; } }
    public float HeightAngle { get { return heightAngle; } }
    public float Length { get { return length; } }

    void Start()

    {
        rigidbody = this.GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;

        //GotoNextPoint();

        //追跡したいオブジェクトの名前を入れる
        //enemy = GameObject.Find("Player");

    }

    //void GotoNextPoint()
    //{
    //    // 地点がなにも設定されていないときに返します
    //    if (points.Length == 0)
    //        return;

    //    // エージェントが現在設定された目標地点に行くように設定します
    //    agent.destination = points[destPoint].position;

    //    // 配列内の次の位置を目標地点に設定し、
    //    // 必要ならば出発地点にもどります
    //    destPoint = (destPoint + 1) % points.Length;
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
        {
            //視界の角度内に収まっているか
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
                    }
                }
            }
        }
    }


    void Update()
    {
        //Playerとこのオブジェクトの距離を測る
        //playerPos = enemy.transform.position;

        //distance = Vector3.Distance(this.transform.position, playerPos);

        if (tracking)
        {
            DoMove(agent.destination);
            //追跡の時、trackingRangeより距離が離れたら中止
            float dist = Vector3.Distance(playerPos, transform.position);
            if (dist > trackingRange)
                tracking = false;


            //Playerを目標とする
            //agent.destination = playerPos;
        }
        else
        {
            //PlayerがtrackingRangeより近づいたら追跡開始
            //if (distance < trackingRange)
            //    tracking = true;

            // エージェントが現目標地点に近づいてきたら、
            // 次の目標地点を選択します
            //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //GotoNextPoint();
        }

        //RotateAround(中心の場所,軸,回転角度)
        //transform.RotateAround(target,Vector3.up,circle_angle/* * Time.deltaTime*/);



    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //trackingRangeの範囲を赤いワイヤーフレームで示す
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        //quitRangeの範囲を青いワイヤーフレームで示す
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, quitRange);
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

        float forward_x = transform.forward.x * 0.5f;  //*ここでenemyの速さ調節
        float forward_z = transform.forward.z * 0.5f;  //*ここでenemyの速さ調節

        rigidbody.velocity = new Vector3(forward_x, rigidbody.velocity.y, forward_z);
    }
#endif

    private static Mesh CreateFanMesh(float i_angle, int i_triangleCount)
    {
        var mesh = new Mesh();

        var vertices = CreateFanVertices(i_angle, i_triangleCount);

        var triangleIndexes = new List<int>(i_triangleCount * 3);

        for (int i = 0; i < i_triangleCount; ++i)
        {
            triangleIndexes.Add(0);
            triangleIndexes.Add(i + 1);
            triangleIndexes.Add(i + 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangleIndexes.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    private static Vector3[] CreateFanVertices(float i_angle, int i_triangleCount)
    {
        //if (i_angle <= 0.0f)
        //{
        //    throw new System.ArgumentException(string.Format("角度がおかしい！ i_angle={0}", i_angle));
        //}

        //if (i_triangleCount <= 0)
        //{
        //    throw new System.ArgumentException(string.Format("数がおかしい！ i_triangleCount={0}", i_triangleCount));
        //}

        i_angle = Mathf.Min(i_angle, 360.0f);

        var vertices = new List<Vector3>(i_triangleCount + 2);

        // 始点
        vertices.Add(Vector3.zero);

        // Mathf.Sin()とMathf.Cos()で使用するのは角度ではなくラジアンなので変換しておく。
        float radian = i_angle * Mathf.Deg2Rad;
        float startRad = -radian / 2;
        float incRad = radian / i_triangleCount;

        for (int i = 0; i < i_triangleCount + 1; ++i)
        {
            float currentRad = startRad + (incRad * i);

            Vector3 vertex = new Vector3(Mathf.Sin(currentRad), 0.0f, Mathf.Cos(currentRad));
            vertices.Add(vertex);
        }

        return vertices.ToArray();
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    private static void DrawPointGizmos(root i_object, GizmoType i_gizmoType)
    {
        if (i_object.Length <= 0.0f)
        {
            return;
        }

        Gizmos.color = MESH_COLOR;

        Transform transform = i_object.transform;
        Vector3 pos = transform.position + transform.forward * 0.5f + Vector3.up * 0.03f; // 0.01fは地面と高さだと見づらいので調整用。
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * i_object.Length;

        Mesh fanMesh = CreateFanMesh(i_object.WidthAngle, TRIANGLE_COUNT);
        Gizmos.DrawMesh(fanMesh, pos, rot, scale);
    }
}
