using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class straight : MonoBehaviour
{
    float time = 0.0f;
    Rigidbody rigidbody;

    private float move;
    Transform target;

    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);

    const float widthAngle = 90.0f;
    const float heightAngle = 0.0f;
    const float length = 1.5f;
    public float WidthAngle { get { return widthAngle; } }
    public float HeightAngle { get { return heightAngle; } }
    public float Length { get { return length; } }

    Vector3 axis = Vector3.up;
    //float step = 1.5f;
    float speed = 120f;
    Quaternion targetRot;

    private void Start()
    {
        target = GameObject.Find("box").transform;
        move = 0.1f;
        Quaternion fromRotation = transform.rotation;
    }

    void Update()
    {
        time += Time.deltaTime;

        float step = speed * Time.deltaTime;

        int number = 1;
        if (time >= 3.0f)
        {
            number += 1;//case1
            if (time >= 5.0f)
            {
                number += 1;//case2
                if (time >= 8.0f)
                {
                    number += 1;//case3
                    if (time >= 10.0f)
                    {
                        number += 1;//case4
                        if (time >= 13.0f)
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
                Debug.Log("リゼひょこ！！！");
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

#if UNITY_EDITOR
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    private static void DrawPointGizmos(straight i_object, GizmoType i_gizmoType)
    {
        if (i_object.Length <= 0.0f)
        {
            return;
        }

        Gizmos.color = MESH_COLOR;

        Transform transform = i_object.transform;
        Vector3 pos = transform.position + transform.forward * 0.45f + Vector3.up * 0.03f; // 0.01fは地面と高さだと見づらいので調整用。
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * i_object.Length;

        Mesh fanMesh = CreateFanMesh(i_object.WidthAngle, TRIANGLE_COUNT);
        Gizmos.DrawMesh(fanMesh, pos, rot, scale);
    }
#endif
}
