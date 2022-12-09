using System;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] GameObject[] pref_enemys_; // 敵リスト
    [SerializeField] GameObject[] pref_items_;  // アイテムリスト
    [SerializeField] GameObject[] pref_bRocks_; // 壊せる岩

    [SerializeField] GameObject parent_enemy_;  // 敵の親オブジェクト
    [SerializeField] GameObject parent_items_;  // アイテムの親オブジェクト
    [SerializeField] GameObject parent_bRock_;  // 壊せる岩


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        try
        {
            string _inputString = Resources.Load<TextAsset>("InputData/EnemyData").ToString();
            DataList _dataList = JsonUtility.FromJson<DataList>(_inputString);
            Paramater _param = GameProgress.instance_.GetParameters();
            float[] _speed = new float[2]{
                _param.yadokarock.speed,
                _param.yadekarock.speed
            };

            if (parent_enemy_ != null)
            {
                foreach (CreateData data in _dataList.lists)
                {
                    GameObject _obj = Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
                    _obj.transform.parent = parent_enemy_.transform;
                    _obj.GetComponent<yadokarock>()
                        .SetParameters(_speed[data.kind]);
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
        string _inputString = Resources.Load<TextAsset>("InputData/EnemyData").ToString();
        DataList _dataList = JsonUtility.FromJson<DataList>(_inputString);
        Paramater _param = GameProgress.instance_.GetParameters();
        GameObject _obj;
        yadokarock _yadok;
        float[] _speed = new float[2]{
            _param.yadokarock.speed,
            _param.yadekarock.speed
        };

        if (parent_enemy_ != null)
        {
            foreach (CreateData data in _dataList.lists)
            {
                _obj = Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
                _obj.transform.parent = parent_enemy_.transform;
                _yadok = _obj.GetComponent<yadokarock>();
                _yadok.SetParameters(_speed[data.kind]);
            }
        }
        else
        {
            foreach (CreateData data in _dataList.lists)
            {
                _obj = Instantiate(pref_enemys_[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
                _yadok = _obj.GetComponent<yadokarock>();
                _yadok.SetParameters(_speed[data.kind]);
            }
        }
        PrefabCreater.CreateMultiplePrefabs("InputData/ItemData", pref_items_, parent_items_);
        //PrefabCreater.CreateMultiplePrefabs("InputData/BreakableRocksData", pref_bRocks_, parent_bRock_);
#endif
    }
}
