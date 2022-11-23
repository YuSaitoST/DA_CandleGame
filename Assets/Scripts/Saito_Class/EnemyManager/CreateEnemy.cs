using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    // 敵プレハブ[0:ヤドカロック,1:サメ]
    [SerializeField] GameObject[] prefabs_ = null;


    // Start is called before the first frame update
    void Start()
    {
        PrefabsCreate.CreateMultiplePrefab("InputData/EnemyData", prefabs_);
    }
}
