using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

enum ENE_STATE { 
    STAY,
    TRACKING,
    TRACKING_NEXT
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

    public Vector3 lastCoordinate;
    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);
    Quaternion targetRot;
    Vector3 axis = Vector3.up;
    const float angle = 90f;
    Vector3 playerPos;
    const float trackingRange = 3.0f;
    ENE_STATE state = ENE_STATE.STAY;
    const float widthAngle = 90.0f;
    const float heightAngle = 0.0f;
    const float length = 2.9f;
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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //Ž‹ŠE‚Ì”ÍˆÍ“à‚Ì“–‚½‚è”»’è
        {
            Vector3 posDelta = other.transform.position - this.transform.position;
            if (posDelta.magnitude > trackingRange)
                return;
            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle) //target_angle‚ªangle‚ÉŽû‚Ü‚Á‚Ä‚¢‚é‚©‚Ç‚¤‚©
            {
                if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) 
                {
                    if (hit.collider == other)
                    {
                        //tracking = true;
                        state = ENE_STATE.TRACKING;
                        Debug.Log("range of view");
                        Agent.destination = playerC.transform.position;
                        time = 0.0f;
                    }
                }
            }
        }
    }
    
    void Update()
    {
        rigidbody.velocity = Vector3.zero;
        if (state == ENE_STATE.TRACKING)
        {
            DoMove(Agent.destination);
            float dist = Vector3.Distance(playerC.transform.position, transform.position);
            if (dist <= 0.3f)
            {
                this.rigidbody.velocity = Vector3.zero;
                Debug.Log("ƒAƒ“ƒWƒ…‚»‚±I");
            }

            if (dist > trackingRange)
            {
                state = ENE_STATE.TRACKING_NEXT;
                rigidbody.velocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
                Debug.Log("ŠO‚ê‚½");
                Agent.destination = playerC.transform.position;
                DoMove(Agent.destination);
            }
        }
        else if (state == ENE_STATE.TRACKING_NEXT)
        {
            time += Time.deltaTime;
            Agent.destination = playerC.transform.position;
            DoMove(Agent.destination);
            if (time >= 5.0f)
            {
                Debug.Log("Ž~‚Ü‚Á‚½");
                rigidbody.velocity = Vector3.zero;
                time = 0.0f;
                state = ENE_STATE.STAY;
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
        float forward_x = transform.forward.x * 0.6f;  //*‚±‚±‚Åenemy‚Ì‘¬‚³’²ß
        float forward_z = transform.forward.z * 0.6f;  //*‚±‚±‚Åenemy‚Ì‘¬‚³’²ß
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