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
    /// エフェクトをループ再生させる
    /// </summary>
    public void Play()
    {

    }

    /// <summary>
    /// エフェクトを単発再生させる
    /// </summary>
    public void PlayOneShot()
    {

    }

    /// <summary>
    /// エフェクトを停止させる
    /// </summary>
    public void Stop()
    {

    }

    /// <summary>
    /// パラメーターを設定する
    /// </summary>
    /// <param name="positionm">表示させる座標</param>
    /// <param name="quaternion">回転</param>
    public void SetTransform(Vector2 positionm, Quaternion quaternion)
    {
        gameObject.transform.position = positionm;
        gameObject.transform.rotation = quaternion;
    }
}
