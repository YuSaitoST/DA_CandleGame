using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using static UnityEditor.Experimental.GraphView.GraphView;
using Fungus;
using PathCreation;

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
    float speed_;
    private NavMeshAgent Agent;
    ENE_STATE2 state = ENE_STATE2.STAY;
    private GameObject playerC;
    Quaternion targetRot;
    Vector3 axis = Vector3.up;

    [SerializeField]
    public PathCreator pathCreator;
    float speed = 2f;
    //Vector3 endPos;
    float dstTravelled;
    public EndOfPathInstruction end;
    bool flag = false;
    int counter = 1;

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
        speed_ = speed; // ÉÅÉìÉoÅ[ïœêîÇ…ë„ì¸Ç∑ÇÈ
    }

    void Update()
    {
        dstTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        var qt = pathCreator.path.GetRotationAtDistance(dstTravelled, end);
        var er = qt.eulerAngles;
        er.z = 0.0f;
        qt = Quaternion.Euler(er);
        transform.rotation = qt;// pathCreator.path.GetRotationAtDistance(moveDistance, EndOfPathInstruction.Stop);
        Debug.Log("è≠èóãFìòíÜ");
    }

    //private void DoMove(Vector3 targetPosition)
    //{
    //    if (Agent && Agent.enabled)
    //    {
    //        Agent.SetDestination(targetPosition);

    //        foreach (var pos in Agent.path.corners)
    //        {
    //            var diff2d = new Vector2(
    //                Mathf.Abs(pos.x - transform.position.x),
    //                Mathf.Abs(pos.z - transform.position.z));
    //            if (0.1f <= diff2d.magnitude)
    //            {
    //                targetPosition = pos;
    //                break;
    //            }
    //        }
    //        Debug.DrawLine(transform.position, targetPosition, Color.red);
    //    }
    //    Quaternion moveRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
    //    moveRotation.z = 0;
    //    moveRotation.x = 0;
    //    transform.rotation = moveRotation; Quaternion.Lerp(transform.rotation, targetRot, 0.2f);
    //    float forward_x = transform.forward.x * speed_;  //*Ç±Ç±Ç≈enemyÇÃë¨Ç≥í≤êﬂ
    //    float forward_z = transform.forward.z * speed_;  //*Ç±Ç±Ç≈enemyÇÃë¨Ç≥í≤êﬂ
    //    rigidbody.velocity = new Vector3(forward_x, rigidbody.velocity.y, forward_z);
    //}

    

    //private static Mesh CreateFanMesh(float i_angle, int i_triangleCount)
    //{
    //    var mesh = new Mesh();
    //    var vertices = CreateFanVertices(i_angle, i_triangleCount);
    //    var triangleIndexes = new List<int>(i_triangleCount * 3);
    //    for (int i = 0; i < i_triangleCount; ++i)
    //    {
    //        triangleIndexes.Add(0);
    //        triangleIndexes.Add(i + 1);
    //        triangleIndexes.Add(i + 2);
    //    }
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangleIndexes.ToArray();
    //    mesh.RecalculateNormals();
    //    return mesh;
    //}

    //private static Vector3[] CreateFanVertices(float i_angle, int i_triangleCount)
    //{
    //    i_angle = Mathf.Min(i_angle, 360.0f);
    //    var vertices = new List<Vector3>(i_triangleCount + 2);
    //    vertices.Add(Vector3.zero);
    //    float radian = i_angle * Mathf.Deg2Rad;
    //    float startRad = -radian / 2;
    //    float incRad = radian / i_triangleCount;
    //    for (int i = 0; i < i_triangleCount + 1; ++i)
    //    {
    //        float currentRad = startRad + (incRad * i);

    //        Vector3 vertex = new Vector3(Mathf.Sin(currentRad), 0.0f, Mathf.Cos(currentRad));
    //        vertices.Add(vertex);
    //    }
    //    return vertices.ToArray();
    //}

    //[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    //private static void DrawPointGizmos(straight i_object, GizmoType i_gizmoType)
    //{
    //    if (i_object.Length <= 0.0f)
    //    {
    //        return;
    //    }
    //    Gizmos.color = MESH_COLOR;
    //    Transform transform = i_object.transform;
    //    Vector3 pos = transform.position + transform.forward * 0.8f + Vector3.up * 0.03f;
    //    Quaternion rot = transform.rotation;
    //    Vector3 scale = Vector3.one * i_object.Length;
    //    Mesh fanMesh = CreateFanMesh(i_object.WidthAngle, TRIANGLE_COUNT);
    //    Gizmos.DrawMesh(fanMesh, pos, rot, scale);
    //}
}
