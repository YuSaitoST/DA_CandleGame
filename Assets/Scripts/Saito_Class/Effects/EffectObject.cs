using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �G�t�F�N�g�����[�v�Đ�������
    /// </summary>
    public void Play()
    {

    }

    /// <summary>
    /// �G�t�F�N�g��P���Đ�������
    /// </summary>
    public void PlayOneShot()
    {

    }

    /// <summary>
    /// �G�t�F�N�g���~������
    /// </summary>
    public void Stop()
    {

    }

    /// <summary>
    /// �p�����[�^�[��ݒ肷��
    /// </summary>
    /// <param name="positionm">�\����������W</param>
    /// <param name="quaternion">��]</param>
    public void SetTransform(Vector2 positionm, Quaternion quaternion)
    {
        gameObject.transform.position = positionm;
        gameObject.transform.rotation = quaternion;
    }
}
