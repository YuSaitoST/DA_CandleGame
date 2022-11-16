using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArc : MonoBehaviour
{
    /// <summary>
    /// �������̕`��ON/OFF
    /// </summary>
    //[SerializeField]
    private bool drawArc = false;

    /// <summary>
    /// ���������\����������̐�
    /// </summary>
    //[SerializeField]
    private int segmentCount = 60;

    /// <summary>
    /// �����������b���v�Z���邩
    /// </summary>
    private float predictionTime = 6.0F;

    /// <summary>
    /// ��������Material
    /// </summary>
    [SerializeField, Tooltip("�������̃}�e���A��")]
    private Material arcMaterial;

    /// <summary>
    /// �������̕�
    /// </summary>
    [SerializeField, Tooltip("�������̕�")]
    private float arcWidth = 0.02F;

    /// <summary>
    /// ���������\������LineRenderer
    /// </summary>
    private LineRenderer[] lineRenderers;

    /// <summary>
    /// �e�̏����x�␶�����W�����R���|�[�l���g
    /// </summary>
    private Player shootBullet;

    /// <summary>
    /// �e�̏����x
    /// </summary>
    private Vector3 initialVelocity;

    /// <summary>
    /// �������̊J�n���W
    /// </summary>
    private Vector3 arcStartPosition;

    /// <summary>
    /// ���e�}�[�J�[�I�u�W�F�N�g��Prefab
    /// </summary>
    [SerializeField, Tooltip("���e�n�_�ɕ\������}�[�J�[��Prefab")]
    private GameObject pointerPrefab;

    /// <summary>
    /// ���e�_�̃}�[�J�[�̃I�u�W�F�N�g
    /// </summary>
    private GameObject pointerObject;


    void Start()
    {
        // ��������LineRenderer�I�u�W�F�N�g��p��
        CreateLineRendererObjects();

        // �}�[�J�[�̃I�u�W�F�N�g��p��
        pointerObject = Instantiate(pointerPrefab, Vector3.zero, Quaternion.identity);
        pointerObject.SetActive(false);

        // �e�̏����x�␶�����W�����R���|�[�l���g
        shootBullet = gameObject.GetComponent<Player>();
    }

    void Update()
    {
        // �����x�ƕ������̊J�n���W���X�V
        initialVelocity = shootBullet.ShootVelocity;
        arcStartPosition = shootBullet.InstantiatePosition_;

        if (drawArc)
        {
            // ��������\��
            float timeStep = predictionTime / segmentCount;
            bool draw = false;
            float hitTime = float.MaxValue;
            for (int i = 0; i < segmentCount; i++)
            {
                // ���̍��W���X�V
                float startTime = timeStep * i;
                float endTime = startTime + timeStep;
                SetLineRendererPosition(i, startTime, endTime, !draw);

                // �Փ˔���
                if (!draw)
                {
                    hitTime = GetArcHitTime(startTime, endTime);
                    if (hitTime != float.MaxValue)
                    {
                        draw = true; // �Փ˂����炻�̐�̕������͕\�����Ȃ�
                    }
                }
            }

            // �}�[�J�[�̕\��
            if (hitTime != float.MaxValue)
            {
                Vector3 hitPosition = GetArcPositionAtTime(hitTime);
                ShowPointer(hitPosition);
            }
        }
        else
        {
            // �������ƃ}�[�J�[��\�����Ȃ�
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].enabled = false;
            }
            pointerObject.SetActive(false);
        }
    }

    /// <summary>
    /// �w�莞�Ԃɑ΂���A�[�`�̕�������̍��W��Ԃ�
    /// </summary>
    /// <param name="time">�o�ߎ���</param>
    /// <returns>���W</returns>
    private Vector3 GetArcPositionAtTime(float time)
    {
        return (arcStartPosition + ((initialVelocity * time) + (0.5f * time * time) * Physics.gravity));
    }

    /// <summary>
    /// LineRenderer�̍��W���X�V
    /// </summary>
    /// <param name="index"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    private void SetLineRendererPosition(int index, float startTime, float endTime, bool draw = true)
    {
        lineRenderers[index].SetPosition(0, GetArcPositionAtTime(startTime));
        lineRenderers[index].SetPosition(1, GetArcPositionAtTime(endTime));
        lineRenderers[index].enabled = draw;
    }

    /// <summary>
    /// LineRenderer�I�u�W�F�N�g���쐬
    /// </summary>
    private void CreateLineRendererObjects()
    {
        // �e�I�u�W�F�N�g�����ALineRenderer�����q�I�u�W�F�N�g�����
        GameObject arcObjectsParent = new GameObject("ArcObject");

        lineRenderers = new LineRenderer[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newObject = new GameObject("LineRenderer_" + i);
            newObject.transform.SetParent(arcObjectsParent.transform);
            lineRenderers[i] = newObject.AddComponent<LineRenderer>();

            // �����֘A���g�p���Ȃ�
            lineRenderers[i].receiveShadows = false;
            lineRenderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            lineRenderers[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lineRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // ���̕��ƃ}�e���A��
            lineRenderers[i].material = arcMaterial;
            lineRenderers[i].startWidth = arcWidth;
            lineRenderers[i].endWidth = arcWidth;
            lineRenderers[i].numCapVertices = 5;
            lineRenderers[i].enabled = false;
        }
    }    

    /// <summary>
    /// �w����W�Ƀ}�[�J�[��\��
    /// </summary>
    /// <param name="position"></param>
    private void ShowPointer(Vector3 position)
    {
        pointerObject.transform.position = position;
        pointerObject.SetActive(true);
    }

    /// <summary>
    /// 2�_�Ԃ̐����ŏՓ˔��肵�A�Փ˂��鎞�Ԃ�Ԃ�
    /// </summary>
    /// <returns>�Փ˂�������(���ĂȂ��ꍇ��float.MaxValue)</returns>
    private float GetArcHitTime(float startTime, float endTime)
    {
        // Linecast��������̎n�I�_�̍��W
        Vector3 startPosition = GetArcPositionAtTime(startTime);
        Vector3 endPosition = GetArcPositionAtTime(endTime);

        // �Փ˔���
        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            // �Փ˂���Collider�܂ł̋���������ۂ̏Փˎ��Ԃ��Z�o
            float distance = Vector3.Distance(startPosition, endPosition);
            return startTime + (endTime - startTime) * (hitInfo.distance / distance);
        }
        return float.MaxValue;
    }

    public void On()
    {
        drawArc = true;
    }

    public void Off()
    {
        drawArc = false;
    }
}