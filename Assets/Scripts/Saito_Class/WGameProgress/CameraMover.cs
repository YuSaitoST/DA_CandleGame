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
        pos.y = 8.5f;
        transform.position = pos;
    }
}
