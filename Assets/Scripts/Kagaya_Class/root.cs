using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

//�I�u�W�F�N�g��NavMeshAgent�R���|�[�l���g��ݒu
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
    //[Tooltip("�ǂ�������Ώ�")]
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

        // autoBraking �𖳌��ɂ���ƁA�ڕW�n�_�̊Ԃ��p���I�Ɉړ����܂�
        //(�܂�A�G�[�W�F���g�͖ڕW�n�_�ɋ߂Â��Ă�
        // ���x�����Ƃ��܂���)
        agent.autoBraking = false;

        //GotoNextPoint();

        //�ǐՂ������I�u�W�F�N�g�̖��O������
        //enemy = GameObject.Find("Player");

    }

    //void GotoNextPoint()
    //{
    //    // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ��܂�
    //    if (points.Length == 0)
    //        return;

    //    // �G�[�W�F���g�����ݐݒ肳�ꂽ�ڕW�n�_�ɍs���悤�ɐݒ肵�܂�
    //    agent.destination = points[destPoint].position;

    //    // �z����̎��̈ʒu��ڕW�n�_�ɐݒ肵�A
    //    // �K�v�Ȃ�Ώo���n�_�ɂ��ǂ�܂�
    //    destPoint = (destPoint + 1) % points.Length;
    //}

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
                    }
                }
            }
        }
    }


    void Update()
    {
        //Player�Ƃ��̃I�u�W�F�N�g�̋����𑪂�
        //playerPos = enemy.transform.position;

        //distance = Vector3.Distance(this.transform.position, playerPos);

        if (tracking)
        {
            DoMove(agent.destination);
            //�ǐՂ̎��AtrackingRange��苗�������ꂽ�璆�~
            float dist = Vector3.Distance(playerPos, transform.position);
            if (dist > trackingRange)
                tracking = false;


            //Player��ڕW�Ƃ���
            //agent.destination = playerPos;
        }
        else
        {
            //Player��trackingRange���߂Â�����ǐՊJ�n
            //if (distance < trackingRange)
            //    tracking = true;

            // �G�[�W�F���g�����ڕW�n�_�ɋ߂Â��Ă�����A
            // ���̖ڕW�n�_��I�����܂�
            //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //GotoNextPoint();
        }

        //RotateAround(���S�̏ꏊ,��,��]�p�x)
        //transform.RotateAround(target,Vector3.up,circle_angle/* * Time.deltaTime*/);



    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //trackingRange�͈̔͂�Ԃ����C���[�t���[���Ŏ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        //quitRange�͈̔͂�����C���[�t���[���Ŏ���
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

        float forward_x = transform.forward.x * 0.5f;  //*������enemy�̑�������
        float forward_z = transform.forward.z * 0.5f;  //*������enemy�̑�������

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
