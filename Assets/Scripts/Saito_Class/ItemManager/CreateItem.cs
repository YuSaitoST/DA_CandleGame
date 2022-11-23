using UnityEngine;

public class CreateItem : MonoBehaviour
{
    // アイテムプレハブ[0:パーツ,1:タンク]
    [SerializeField] GameObject[] prefabs_ = null;


    // Start is called before the first frame update
    void Start()
    {
        PrefabsCreate.CreateMultiplePrefab("InputData/ItemData", prefabs_);
    }
}
