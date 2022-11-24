using System;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] GameObject[] pref_enemys_; // �G���X�g
    [SerializeField] GameObject[] pref_items_;  // �A�C�e�����X�g

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            PrefabCreater.CreateMultiplePrefabs("InputData/EnemyData", pref_enemys_);
            PrefabCreater.CreateMultiplePrefabs("InputData/ItemData", pref_items_);

        }catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
