using System;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] GameObject[] pref_enemys_;     // 敵リスト
    [SerializeField] GameObject[] pref_fellows_;    // 仲間
    [SerializeField] GameObject[] pref_items_;      // アイテムリスト
    [SerializeField] GameObject[] pref_bRocks_;     // 壊せる岩
    
    [SerializeField] GameObject parent_enemy_;      // 敵の親オブジェクト
    [SerializeField] GameObject parent_fellow_;     // 仲間の親オブジェクト
    [SerializeField] GameObject parent_items_;      // アイテムの親オブジェクト
    [SerializeField] GameObject parent_bRock_;      // 壊せる岩

    [SerializeField] GameObject[] fellows_;         // 仲間


    // Start is called before the first frame update
    void Start()
    {
        string _inputString = Resources.Load<TextAsset>("InputData/EnemyData").ToString();
        DataList _dataList = JsonUtility.FromJson<DataList>(_inputString);
        Paramater _param = GameProgress.instance_.GetParameters();
        float[] _speed = new float[3]{
            _param.yadokarock.speed,
            0.0f,
            _param.yadekarock.speed
        };

#if UNITY_EDITOR
        try
        {
            if (parent_enemy_ != null)
            {
                foreach (CreateData data in _dataList.lists)
                {
                    GameObject _obj = Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
                    _obj.transform.parent = parent_enemy_.transform;
                    if (data.kind == 0 || data.kind == 2)
                    {
                        _obj.GetComponent<yadokarock>()
                            .SetParameters(_speed[data.kind]);
                    }
                }
            }
            else
            {
                foreach (CreateData data in _dataList.lists)
                {
                    Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up))
                        .GetComponent<yadokarock>()
                        .SetParameters(_speed[data.kind]);
                }
            }

        }
        catch(Exception e)
        {
            Debug.Log("EnemysData : " + e.ToString());
        }

        //try
        //{
        //    PrefabCreater.CreateMultiplePrefabs("InputData/FellowData", pref_fellows_, parent_fellow_);

        //}
        //catch (Exception e)
        //{
        //    Debug.Log("FollowsData : " + e.ToString());
        //}

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
        // 敵生成
        if (parent_enemy_ != null)
        {
            foreach (CreateData data in _dataList.lists)
            {
                GameObject _obj = Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
                _obj.transform.parent = parent_enemy_.transform;
                if (data.kind == 0 || data.kind == 2)
                {
                    _obj.GetComponent<yadokarock>()
                        .SetParameters(_speed[data.kind]);
                }
            }
        }
        else
        {
            foreach (CreateData data in _dataList.lists)
            {
                Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up))
                    .GetComponent<yadokarock>()
                    .SetParameters(_speed[data.kind]);
            }
        }

        // 仲間
        PrefabCreater.CreateMultiplePrefabs("InputData/ItemData", pref_items_, parent_items_);
        //PrefabCreater.CreateMultiplePrefabs("InputData/BreakableRocksData", pref_bRocks_, parent_bRock_);

        // 仲間の座標設定

        BOB _bob = _param.bob;
        fellows_[0].transform.position = new Vector3(_bob.pos_x, _bob.pos_y, _bob.pos_z);

        NIC _nic = _param.nic;
        fellows_[1].transform.position = new Vector3(_nic.pos_x, _nic.pos_y, _nic.pos_z);

        SPENCER _spe = _param.spencer;
        fellows_[2].transform.position = new Vector3(_spe.pos_x, _spe.pos_y, _spe.pos_z);

        ALAN _ala = _param.alan;
        fellows_[3].transform.position = new Vector3(_ala.pos_x, _ala.pos_y, _ala.pos_z);

        CATHERINE _cat = _param.catherine;
        fellows_[4].transform.position = new Vector3(_cat.pos_x, _cat.pos_y, _cat.pos_z);
#endif
    }
}
