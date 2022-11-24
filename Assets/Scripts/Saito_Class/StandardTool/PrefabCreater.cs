using UnityEngine;

[System.Serializable]
public class CreateData
{
    public int id;
    public int kind;
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

[System.Serializable]
public class DataList {
    public CreateData[] data;
}



public class PrefabCreater : MonoBehaviour
{
    public static void CreateMultiplePrefabs(string jsonName, GameObject[] prefabs) {
        string _inputString = Resources.Load<TextAsset>(jsonName).ToString();
        DataList _dataList = JsonUtility.FromJson<DataList>(_inputString);

        foreach (CreateData data in _dataList.data)
        {
            Instantiate(prefabs[data.id], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
        }
    }
}
