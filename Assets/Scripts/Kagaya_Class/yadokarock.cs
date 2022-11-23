using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //NavMeshAgent�g���Ƃ��ɕK�v
using UnityEditor;
using Unity.Services.Analytics.Internal;
using Unity.VisualScripting;

//�I�u�W�F�N�g��NavMeshAgent�R���|�[�l���g��ݒu
[RequireComponent(typeof(NavMeshAgent))]

public class yadokarock : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField]
    [Tooltip("�ǂ�������Ώ�")]
    private GameObject PlayerC;

    private NavMeshAgent Agent;

    Mesh mesh;
    Vector3[] vertices;

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]

    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);

    const float angle = 90f;
    Vector3 playerPos;
    GameObject enemy;
    
    const float trackingRange = 3f;
    
    bool tracking = false;

    //[SerializeField, Range(0.0f, 360.0f)]
    const float widthAngle = 90.0f;
    //[SerializeField, Range(0.0f, 360.0f)]
    const float heightAngle = 0.0f;
    //[SerializeField, Range(0.0f, 15.0f)]
    const float length = 2.5f;

    public float WidthAngle { get { return widthAngle; } }
    public float HeightAngle { get { return heightAngle; } }
    public float Length { get { return length; } }


    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        rigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //���E�͈͓̔��̓����蔻��
        {
            //���E�̊p�x���Ɏ��܂��Ă��邩
            Vector3 posDelta = other.transform.position - this.transform.position;
            if (posDelta.magnitude > trackingRange)
                return;

            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle) //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
            {
                if (Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
                {
                    if (hit.collider == other)
                    {
                        tracking = true;
                        Debug.Log("range of view");
                        Agent.destination = PlayerC.transform.position;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            DoMove(Agent.destination);
            //�ǐՂ̎��AtrackingRange��苗�������ꂽ�璆�~
            float dist = Vector3.Distance(GameProgress.instance_.GetPlayerPos(), transform.position);
            if (dist > trackingRange)
            {

                tracking = false;
                //rigidbody.velocity = Vector3.zero;
                rigidbody.velocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
                Debug.Log("�O�ꂽ");
            }
                
           
        }
        else
        {

        }

    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //trackingRange�͈̔͂�Ԃ����C���[�t���[���Ŏ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
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

        float forward_x = transform.forward.x * 1.0f;  //*������enemy�̑�������
        float forward_z = transform.forward.z * 1.0f;  //*������enemy�̑�������

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
        //    throw new System.ArgumentException(string.Format("�p�x�����������I i_angle={0}", i_angle));
        //}

        //if (i_triangleCount <= 0)
        //{
        //    throw new System.ArgumentException(string.Format("�������������I i_triangleCount={0}", i_triangleCount));
        //}

        i_angle = Mathf.Min(i_angle, 360.0f);

        var vertices = new List<Vector3>(i_triangleCount + 2);

        // �n�_
        vertices.Add(Vector3.zero);

        // Mathf.Sin()��Mathf.Cos()�Ŏg�p����̂͊p�x�ł͂Ȃ����W�A���Ȃ̂ŕϊ����Ă����B
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
        Vector3 pos = transform.position + transform.forward * 0.5f + Vector3.up * 0.03f; // 0.01f�͒n�ʂƍ������ƌ��Â炢�̂Œ����p�B
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * i_object.Length;

        Mesh fanMesh = CreateFanMesh(i_object.WidthAngle, TRIANGLE_COUNT);
        Gizmos.DrawMesh(fanMesh, pos, rot, scale);
    }
}

