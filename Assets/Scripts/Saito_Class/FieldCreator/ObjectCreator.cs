using System;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] GameObject[] pref_enemys_; // �G���X�g
    [SerializeField] GameObject[] pref_items_;  // �A�C�e�����X�g
    [SerializeField] GameObject[] pref_bRocks_; // �󂹂��

    [SerializeField] GameObject parent_enemy_;  // �G�̐e�I�u�W�F�N�g
    [SerializeField] GameObject parent_items_;  // �A�C�e���̐e�I�u�W�F�N�g
    [SerializeField] GameObject parent_bRock_;  // �󂹂��


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        try
        {
            PrefabCreater.CreateMultiplePrefabs("InputData/EnemyData", pref_enemys_, parent_enemy_);

        }catch(Exception e)
        {
            Debug.Log("EnemysData : " + e.ToString());
        }

        try
        {
            PrefabCreater.CreateMultiplePrefabs("InputData/ItemData", pref_items_, parent_items_);

        }
        catch (Exception e)
        {
            Debug.Log("ItemsData : " + e.ToString());
        }

        //try
        //{
        //    PrefabCreater.CreateMultiplePrefabs("InputData/BreakableRocksData", pref_bRocks_, parent_bRock_);

        //}
        //catch (Exception e)
        //{
        //    Debug.Log("BreakableRocksData : " + e.ToString());
        //}
#else
        PrefabCreater.CreateMultiplePrefabs("InputData/EnemyData", pref_enemys_, parent_enemy_);
        PrefabCreater.CreateMultiplePrefabs("InputData/ItemData", pref_items_, parent_items_);
        //PrefabCreater.CreateMultiplePrefabs("InputData/BreakableRocksData", pref_bRocks_, parent_bRock_);
#endif
    }
}
