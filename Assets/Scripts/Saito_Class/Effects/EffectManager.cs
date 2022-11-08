using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] GameObject prefab_effect_ = null;

    EffectObject effect_ = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �G�t�F�N�g�𐶐�����
    /// </summary>
    /// <param name="position">�\����������W</param>
    /// <param name="quaternion">��]</param>
    public void CreateEffect(Vector2 position, Quaternion quaternion)
    {
        GameObject gameObject = Instantiate(prefab_effect_, position, quaternion);
        gameObject.transform.parent = transform;
    }

    /// <summary>
    /// �G�t�F�N�g�����[�v�Đ�������
    /// </summary>
    public void Play()
    {
        effect_.Play();
    }

    /// <summary>
    /// �G�t�F�N�g��P���Đ�������
    /// </summary>
    public void PlayOneShot()
    {
        effect_.PlayOneShot();
    }

    /// <summary>
    /// �G�t�F�N�g���~������
    /// </summary>
    public void Stop()
    {
        effect_.Stop();
    }

    /// <summary>
    /// �p�����[�^�[��ݒ肷��
    /// </summary>
    /// <param name="position">�\����������W</param>
    /// <param name="quaternion">��]</param>
    public void SetTransform(Vector3 position, Quaternion quaternion)
    {
        effect_.SetTransform(position, quaternion);
    }
}
