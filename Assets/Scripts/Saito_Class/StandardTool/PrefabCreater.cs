using System;
using UnityEngine;

[Serializable]
public class DataList
{
    public CreateData[] lists;
}

[Serializable]
public class CreateData
{
    public int id;
    public int kind;
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

public class PrefabCreater : MonoBehaviour
{
    public static void CreateMultiplePrefabs(string jsonName, GameObject[] prefabs, GameObject parent) {
        string _inputString = Resources.Load<TextAsset>(jsonName).ToString();
        DataList _dataList = JsonUtility.FromJson<DataList>(_inputString);

        if (parent != null)
        {
            GameObject _obj;
            foreach (CreateData data in _dataList.lists)
            {
                _obj = Instantiate(prefabs[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
                _obj.transform.parent = parent.transform;
            }
        }
        else
        {
            foreach (CreateData data in _dataList.lists)
            {
                Instantiate(prefabs[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
            }
        }
    }
}
