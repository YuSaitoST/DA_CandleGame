using System;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] GameObject[] pref_enemys_; // 敵リスト
    [SerializeField] GameObject[] pref_items_;  // アイテムリスト

    [SerializeField] GameObject parent_enemy_;  // 敵の親オブジェクト
    [SerializeField] GameObject parent_items_;  // アイテムの親オブジェクト

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            PrefabCreater.CreateMultiplePrefabs("InputData/EnemyData", pref_enemys_, parent_enemy_);
            PrefabCreater.CreateMultiplePrefabs("InputData/ItemData", pref_items_, parent_items_);

        }catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
