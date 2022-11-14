using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = GameProgress.instance_.GetPlayerPos();
        pos.y = 3.0f;
        pos.z += -4.0f;
        transform.position = pos;
    }
}
