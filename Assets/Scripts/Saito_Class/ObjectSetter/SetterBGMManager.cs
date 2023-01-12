using UnityEngine;

public class SetterBGMManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameProgress.instance_.SetBGMMamager(GetComponent<BGMManager>());
    }
}
