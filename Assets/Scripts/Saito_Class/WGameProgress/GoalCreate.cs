using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCreate : MonoBehaviour
{
    [SerializeField] GameObject pre_goal_ = null;   // ゴールプレハブ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ゴールを設置する
    /// </summary>
    /// <param name="positon">ゴールの座標</param>
    public void CreateGoalArea(Vector3 position)
    {
        // プレハブ生成
        GameObject obj = Instantiate(pre_goal_, position, Quaternion.identity);
        
        // 親オブジェクトの設定
        obj.transform.parent = transform;
    }
}
