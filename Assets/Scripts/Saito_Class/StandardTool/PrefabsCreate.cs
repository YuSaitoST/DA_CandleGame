using UnityEngine;

public class PrefabsCreate : MonoBehaviour
{
    /// <summary>
    /// 複数のプレハブを生成する
    /// </summary>
    /// <param name="jsonName">パラメータをまとめたJSONファイル</param>
    /// <param name="prefabs">プレハブ</param>
    public static void CreateMultiplePrefab(string jsonName, GameObject[] prefabs)
    {
        string _inputStr = Resources.Load<TextAsset>(jsonName).ToString();
        ObjectsData[] _enemysData = JsonHelper.FromJson<ObjectsData>(_inputStr);
        foreach (ObjectsData data in _enemysData)
        {
            Instantiate(prefabs[data.kind], new Vector3(data.pos_x, data.pos_y, data.pos_z), Quaternion.AngleAxis(data.rot_y, Vector3.up));
        }
    }
}