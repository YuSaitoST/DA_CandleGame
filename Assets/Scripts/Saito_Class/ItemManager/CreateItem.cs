using UnityEngine;

public class CreateItem : MonoBehaviour
{
    // �A�C�e���v���n�u[0:�p�[�c,1:�^���N]
    [SerializeField] GameObject[] prefabs_ = null;


    // Start is called before the first frame update
    void Start()
    {
        PrefabsCreate.CreateMultiplePrefab("InputData/ItemData", prefabs_);
    }
}
