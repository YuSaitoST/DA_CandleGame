using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Fungus;
using Unity.VisualScripting;

enum ENE_STATE { 
    STAY,
    TRACKING,
    TRACKING_NEXT,
    bomb0,
    bomb1,
    bomb2,
    bomb3
}
[RequireComponent(typeof(NavMeshAgent))]

public class yadokarock : MonoBehaviour
{

    Rigidbody rigidbody;
    private GameObject playerC;
    private NavMeshAgent Agent;
    //bool enabled = true;
    float time = 0;
    Mesh mesh;
    Vector3[] vertices;

#if UNITY_EDITOR
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
#endif

    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);
    Quaternion targetRot;
    Vector3 axis = Vector3.up;
    const float angle = 90f;
    Vector3 playerPos;
    const float trackingRange = 1.6f;
    ENE_STATE state = ENE_STATE.STAY;
    const float widthAngle = 90.0f;
    const float heightAngle = 0.0f;
    const float length = 1.5f;
    private Animator Animator;
    float speed_;
    float timebent = 0.0f;
    bool anime = true;

    public float WidthAngle { get { return widthAngle; } }
    public float HeightAngle { get { return heightAngle; } }
    public float Length { get { return length; } }

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        rigidbody = this.GetComponent<Rigidbody>();
        playerC = GameProgress.instance_.Get_PlayerC();
        targetRot = Quaternion.AngleAxis(angle, axis) * transform.rotation;
        enabled = true;
        state = ENE_STATE.STAY;
        Animator = gameObject.transform.GetComponent<Animator>();
        //SPEED=GameProgress.instance_
    }

    public void SetParameters(float speed)
    {
        speed_ = speed; // メンバー変数に代入する
    }

    private void OnTriggerEnter(Collider other)
    {
        timebent = Time.deltaTime;
        Debug.Log("止");
        string tags = other.transform.tag.Substring(0, other.transform.tag.Length - 2);

        if (tags == "OxyBomb")
        {
            
            if (tags == "OxyBomb00")
            {
                state = ENE_STATE.bomb0;
                Debug.Log("スタン");
            }
            if (tags == "OxyBomb01")
            {
                state = ENE_STATE.bomb1;
            }
            if (tags == "OxyBomb02")
            {
                state = ENE_STATE.bomb2;
            }
            if (tags == "OxyBomb03")
            {
                state = ENE_STATE.bomb3;
            }

        }

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
                        time += Time.deltaTime;
                        Agent.destination = playerC.transform.position;
                        GameProgress.instance_.Enemy_StartTracking();
                        if (time >= 1.0f)
                        {
                            state = ENE_STATE.TRACKING;
                            GameProgress.instance_.Enemy_StartTracking();
                        }
                        if(anime == true)
                        {
                            Animator.SetBool("bulubulu", true);
                            anime = false;
                        }
                        Debug.Log("range of view");
                    }
                }
            }
        }
    }
    
    void Update()
    {
        //var child = transform.Find("CrabMonster 1/Armature/Root/Thorax_1");
        //var child2 = transform.Find("CrabMonster 1/Armature/Eyes");
        //child.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //child2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //if (time >= 1.0f)
        //{
        //    child.transform.localScale = new Vector3(1f, 1f, 1f);
        //    child2.transform.localScale = new Vector3(1f, 1f, 1f);
        //    Debug.Log("デカい");

        //}

        rigidbody.velocity = Vector3.zero;
        if (state == ENE_STATE.TRACKING)
        {
            DoMove(Agent.destination);
            float dist = Vector3.Distance(playerC.transform.position, transform.position);
            if(dist <= 0.3f)
            {
                this.rigidbody.velocity = Vector3.zero;
                Debug.Log("アンジュそこ！");
            }

            if (dist > trackingRange)
            {
                state = ENE_STATE.TRACKING_NEXT;
                rigidbody.velocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
                GameProgress.instance_.Enemy_EndTracking();
                Agent.destination = playerC.transform.position;
                DoMove(Agent.destination);
                Debug.Log("外れた");
            }
        }
        else if (state == ENE_STATE.TRACKING_NEXT)
        {
            time += Time.deltaTime;
            Agent.destination = playerC.transform.position;
            DoMove(Agent.destination);
            if (time >= 10.0f)
            {
                Debug.Log("止まった");
                rigidbody.velocity = Vector3.zero;
                time = 0.0f;
                GameProgress.instance_.Enemy_EndTracking();
                state = ENE_STATE.STAY;
            }
        }

        if(state == ENE_STATE.bomb0)
        {
            //state = ENE_STATE.STAY;
            if (timebent >= 1.0f)
            {
                Debug.Log("再始動");
                state = ENE_STATE.TRACKING;
                GameProgress.instance_.Enemy_StartTracking();
                timebent = 0.0f;
            }
        }
        if (state == ENE_STATE.bomb1)
        {
            state = ENE_STATE.STAY;
            if (timebent >= 1.4f)
            {
                state = ENE_STATE.TRACKING;
                GameProgress.instance_.Enemy_StartTracking();
                timebent = 0.0f;
            }
        }
        if (state == ENE_STATE.bomb2)
        {
            state = ENE_STATE.STAY;
            if (timebent >= 1.8f)
            {
                state = ENE_STATE.TRACKING;
                GameProgress.instance_.Enemy_StartTracking();
                timebent = 0.0f;
            }
        }
        if (state == ENE_STATE.bomb3)
        {
            state = ENE_STATE.STAY;
            if (timebent >= 2.2f)
            {
                state = ENE_STATE.TRACKING;
                GameProgress.instance_.Enemy_StartTracking();
                timebent = 0.0f;
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

#if UNITY_EDITOR
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    private static void DrawPointGizmos(yadokarock i_object, GizmoType i_gizmoType)
    {
        if (i_object.Length <= 0.0f)
        {
            return;
        }
        Gizmos.color = MESH_COLOR;
        Transform transform = i_object.transform;
        Vector3 pos = transform.position + transform.forward * 0.1f + Vector3.up * 0.03f; 
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * i_object.Length;
        Mesh fanMesh = CreateFanMesh(i_object.WidthAngle, TRIANGLE_COUNT);
        Gizmos.DrawMesh(fanMesh, pos, rot, scale);
    }
#endif
}