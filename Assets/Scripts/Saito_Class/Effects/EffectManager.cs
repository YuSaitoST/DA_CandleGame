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
    /// エフェクトを生成する
    /// </summary>
    /// <param name="position">表示させる座標</param>
    /// <param name="quaternion">回転</param>
    public void CreateEffect(Vector2 position, Quaternion quaternion)
    {
        GameObject gameObject = Instantiate(prefab_effect_, position, quaternion);
        gameObject.transform.parent = transform;
    }

    /// <summary>
    /// エフェクトをループ再生させる
    /// </summary>
    public void Play()
    {
        effect_.Play();
    }

    /// <summary>
    /// エフェクトを単発再生させる
    /// </summary>
    public void PlayOneShot()
    {
        effect_.PlayOneShot();
    }

    /// <summary>
    /// エフェクトを停止させる
    /// </summary>
    public void Stop()
    {
        effect_.Stop();
    }

    /// <summary>
    /// パラメーターを設定する
    /// </summary>
    /// <param name="position">表示させる座標</param>
    /// <param name="quaternion">回転</param>
    public void SetTransform(Vector3 position, Quaternion quaternion)
    {
        effect_.SetTransform(position, quaternion);
    }
}
