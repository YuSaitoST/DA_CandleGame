using System.Collections.Generic;
using UnityEngine;

/*
 * 影響を受けるオブジェクトにアタッチする
 */

public class InfluencedByCurrents : MonoBehaviour
{
    static List<Rigidbody> targets = new List<Rigidbody>();   // 海流の影響を受ける対象のスクリプト
    int id_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        id_=targets.Count;
        targets.Add(GetComponent<Rigidbody>());
    }

    void OnDestory()
    {
        // ポーズ対象から除外する
        targets.Remove(GetComponent<Rigidbody>());
    }

    public static List<Rigidbody> GetTargetList() { return targets; }
    public int GetID() { return id_; }
}
