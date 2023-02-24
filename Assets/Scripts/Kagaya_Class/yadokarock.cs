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
    bool enabled = true;
    float time = 0.0f;
    Mesh mesh;
    Vector3[] vertices;
    [SerializeField, Header("Thorax_1")] GameObject atomic;
    [SerializeField] GameObject eyes_ = null;
    //[SerializeField] GameObject bulu;

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
    private Animator animator;
    bool Anima = true;
    float speed_;
    float timebent = 0.0f;
    bool fill = true;
    

    private Vector3 speed = new Vector3(0.3f, 0.3f, 0.3f);

    public float WidthAngle { get { return widthAngle; } }
    public float HeightAngle { get { return heightAngle; } }
    public float Length { get { return length; } }

    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        playerC = GameProgress.instance_.Get_PlayerC();
        targetRot = Quaternion.AngleAxis(angle, axis) * transform.rotation;
        state = ENE_STATE.STAY;
        animator = gameObject.GetComponent<Animator>();
        atomic.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        eyes_.SetActive(false);
        
        //SPEED=GameProgress.instance_
    }

    public void SetParameters(float speed)
    {
        speed_ = speed; // メンバー変数に代入する
    }

    private void OnTriggerEnter(Collider other)
    {
        timebent = Time.deltaTime;
        
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
                    Debug.Log("range of view1");
                    //if (hit.collider == other)
                    //{
                    Agent.destination = playerC.transform.position;
                    GameProgress.instance_.Enemy_StartTracking();
                    time += Time.deltaTime;
                    if (time >= 1.0f)
                    {
                        eyes_.SetActive(true);
                        state = ENE_STATE.TRACKING;
                        Debug.Log("range of view2");
                    }

                    if (Anima == true)
                    {
                        //StartCoroutine(shiver());
                        animator.SetBool("bulubulu", true);
                        Anima = false;
                        
                        Debug.Log("range of view3");
                    }
                }
            }
        }
    }
    
    void Update()
    {
        if (fill == true)
        {
            //this.transform.localPosition = new Vector3(0.0f, -100.0f, 0.0f);
            //Vector3 pos = this.transform.position;
            //this.transform.position = new Vector3(pos.x, pos.y - 10.0f, pos.z);
            fill = false;
        }

        rigidbody.velocity = Vector3.zero;

        if (state == ENE_STATE.STAY)
        {
            if (atomic.transform.localScale.x <= 0.1f)
            {
                atomic.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            Debug.Log("起動");
        }
        else if (state == ENE_STATE.TRACKING)
        {
            DoMove(Agent.destination);
            float dist = Vector3.Distance(playerC.transform.position, transform.position);
            Debug.Log("ココッ！");
            if (time >= 1.0f)
            {
                atomic.transform.localScale += speed * Time.deltaTime;
                Debug.Log("デカい");
                if (atomic.transform.localScale.x > 1.0f)
                {
                    atomic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.SetBool("walking", true);
                }
                
            }

            if (dist <= 0.3f)
            {
                this.rigidbody.velocity = Vector3.zero;
                Debug.Log("アンジュそこ！");
            }

            if (dist > trackingRange)
            {
                
                rigidbody.velocity = new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
                GameProgress.instance_.Enemy_EndTracking();
                Agent.destination = playerC.transform.position;
                DoMove(Agent.destination);
                state = ENE_STATE.TRACKING_NEXT;
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
                rigidbody.velocity = Vector3.zero;
                time = 0.0f;
                GameProgress.instance_.Enemy_EndTracking();
                eyes_.SetActive(false);
                atomic.transform.localScale -= speed * Time.deltaTime;
                state = ENE_STATE.STAY;
                //sub.Rocking();
                Debug.Log("止まった");
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

    //IEnumerator shiver()
    //{
    //    int count = 0;
    //    while (true)
    //    {
    //        if (count <= 5)
    //        {
    //            this.transform.position =
    //                new Vector3(
    //                Mathf.Sin(time),
    //                transform.position.y,
    //                transform.position.z
    //                );
    //            count++;
    //            Debug.Log("反復横跳び");
    //            yield return null;
    //        }
    //        else
    //        {
    //            Debug.Log("ぶっ壊して");
    //            yield break;
    //        }
    //    }


        //for (int i = 0; i < 255; i++)
        //{
        //    scelton.material.color = scelton.material.color - new Color32(0, 0, 0, 1);
        //    yield return new WaitForSeconds(0.01f);
        //}
        //this.gameObject.transform.Translate(0, 0.1f, 0);
        //if (transform.position.y > 1.0f)
        //{
        //    Debug.Log("発進");
        //    yield break;
        //}

    //}

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
                    Debug.Log("こいつ、動くぞ！");
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
        Debug.Log("俺が、ガンダムだ");
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