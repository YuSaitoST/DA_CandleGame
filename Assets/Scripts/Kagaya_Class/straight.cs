using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using static UnityEditor.Experimental.GraphView.GraphView;

enum ENE_STATE2
{
    STAY,
    TRACKING,
    TRACKING_NEXT,
}

public class straight : MonoBehaviour
{
    float time = 0.0f;
    float timebent = 0.0f;
    bool timeflag = false;
    Rigidbody rigidbody;
    private float move;
    const float angle = 90f;
    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);
    const float trackingRange = 14.0f;
    const float widthAngle = 90.0f;
    const float heightAngle = 0.0f;
    const float length = 13.0f;
    float speed_;
    public float WidthAngle { get { return widthAngle; } }
    public float HeightAngle { get { return heightAngle; } }
    public float Length { get { return length; } }
    private NavMeshAgent Agent;
    ENE_STATE2 state = ENE_STATE2.STAY;
    private GameObject playerC;
    Quaternion targetRot;
    Vector3 axis = Vector3.up;
    int number = 1;
    const float zikan = 3.0f;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        rigidbody = this.GetComponent<Rigidbody>();
        playerC = GameProgress.instance_.Get_PlayerC();
        move = 0.1f;
        Quaternion fromRotation = transform.rotation;
        targetRot = Quaternion.AngleAxis(angle, axis) * transform.rotation;
        state = ENE_STATE2.STAY;
    }
    public void SetParameters(float speed)
    {
        speed_ = speed; // メンバー変数に代入する
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
                if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit))
                {
                    if (hit.collider == other)
                    {
                        timebent += Time.deltaTime;
                        time = 0;
                        Agent.destination = playerC.transform.position;
                        state = ENE_STATE2.TRACKING;
                        timeflag = true;
                        number = 0;

                        Debug.Log("range of view");


                    }
                }
            }
        }
    }


    void Update()
    {
        transform.Translate(new Vector3(0, 0, move));
        if (timeflag == false)
        {
            time += Time.deltaTime;
        }

        float step = speed_ * Time.deltaTime;

        number = 1;
        if (time >= zikan)  //zikan = 3.0f 
        {
            number += 1;//case1
            if (time >= zikan + 2.0f)
            {
                number += 1;//case2
                if (time >= zikan + 5.0f)
                {
                    number += 1;//case3
                    if (time >= zikan + 7.0f)
                    {
                        number += 1;//case4
                        if (time >= zikan + 10.0f)
                        {
                            number += 1;//case5
                        }
                    }
                }
            }
        }
        //switch文
        switch (number)
        {
            case 1:
                transform.Translate(new Vector3(0, 0, move));
                Debug.Log("アンジュそこ！");
                break;
            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -180f, 0), step);
                Debug.Log("戌亥ここ！！");
                break;
            case 3:
                transform.Translate(new Vector3(0, 0, move));
                Debug.Log("リゼひよこ！！！");
                break;
            case 4:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -360f, 0), step);
                Debug.Log("ぽっぴっぽー");
                break;
            case 5:
                time = 0.0f;
                Debug.Log("八卦六十四掌");
                break;
        }

        rigidbody.velocity = Vector3.zero;
        if (state == ENE_STATE2.TRACKING)
        {
            time = 0.0f;
            DoMove(Agent.destination);
            float dist = Vector3.Distance(playerC.transform.position, transform.position);
            if (dist <= 0.3f)
            {
                this.rigidbody.velocity = Vector3.zero;
                Debug.Log("アンジュそこ！");
            }

            if (dist > trackingRange)
            {
                state = ENE_STATE2.TRACKING_NEXT;
                rigidbody.velocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
                Debug.Log("外れた");
                Agent.destination = playerC.transform.position;
                DoMove(Agent.destination);
            }
        }
        else if (state == ENE_STATE2.TRACKING_NEXT)
        {
            time += Time.deltaTime;
            Agent.destination = playerC.transform.position;
            DoMove(Agent.destination);
            if (time >= 5.0f)
            {
                Debug.Log("止まった");
                rigidbody.velocity = Vector3.zero;
                time = 0.0f;
                state = ENE_STATE2.STAY;
            }
        }

        if (timebent >= 1.0f)
        {
            state = ENE_STATE2.STAY;
            Debug.Log("マッスグマ");
            if (timebent >= 2.0f)
            {
                transform.position = Vector3.MoveTowards(
                   transform.position,
                   playerC.transform.position,
                   speed_ * Time.deltaTime);

            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }

    void OnCollisionStay(Collision collision)
    {
        timebent += Time.deltaTime;
        Debug.Log("止まれ");
        if (this.gameObject.CompareTag("enemy"))
        {
            move = 0;
            number = 0;
            timeflag = true;
            Debug.Log("当たり");
        }

        if (timebent >= 1.0f)
        {
            timeflag = false;
            Debug.Log("動く");
            move = 0.1f;
            number = 1;
            if (timebent >= 2.0f)
            {
                timebent = 0.0f;
            }
        }
    }

    private void DoMove(Vector3 targetPosition)
    {
        if (Agent && Agent.enabled)
        {
            Agent.SetDestination(targetPosition);

            foreach (var pos in Agent.path.corners)
            {
                var diff2d = new Vector2(
                    Mathf.Abs(pos.x - transform.position.x),
                    Mathf.Abs(pos.z - transform.position.z));
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
        transform.rotation = moveRotation; Quaternion.Lerp(transform.rotation, targetRot, 0.2f);
        float forward_x = transform.forward.x * speed_;  //*ここでenemyの速さ調節
        float forward_z = transform.forward.z * speed_;  //*ここでenemyの速さ調節
        rigidbody.velocity = new Vector3(forward_x, rigidbody.velocity.y, forward_z);
    }

    void OnCollisionExit(Collision other)
    {

        timebent = 0.0f;

    }

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
        i_angle = Mathf.Min(i_angle, 360.0f);
        var vertices = new List<Vector3>(i_triangleCount + 2);
        vertices.Add(Vector3.zero);
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
    private static void DrawPointGizmos(straight i_object, GizmoType i_gizmoType)
    {
        if (i_object.Length <= 0.0f)
        {
            return;
        }
        Gizmos.color = MESH_COLOR;
        Transform transform = i_object.transform;
        Vector3 pos = transform.position + transform.forward * 0.8f + Vector3.up * 0.03f;
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * i_object.Length;
        Mesh fanMesh = CreateFanMesh(i_object.WidthAngle, TRIANGLE_COUNT);
        Gizmos.DrawMesh(fanMesh, pos, rot, scale);
    }
}
