using UnityEngine;

[System.Serializable]
public class CreateData : MonoBehaviour
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
    public static void CreateMultiplePrefabs(string jsonName, GameObject[] prefabs)
    {
        CreateData[] _data = JsonHelper.FromJson<CreateData>(jsonName);
        foreach (CreateData data in _data)
        {
            Instantiate(prefabs[data.id], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
        }
    }
}
