using UnityEngine;

public class SetterMiniCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameProgress.instance_.SetMiniCamera(GetComponent<Camera>());
    }
}
