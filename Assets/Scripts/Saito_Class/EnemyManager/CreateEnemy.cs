using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    // �G�v���n�u[0:���h�J���b�N,1:�T��]
    [SerializeField] GameObject[] prefabs_ = null;


    // Start is called before the first frame update
    void Start()
    {
        PrefabsCreate.CreateMultiplePrefab("InputData/EnemyData", prefabs_);
    }
}
