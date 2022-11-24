using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 _pos = GameProgress.instance_.GetPlayerPos();
        _pos.y = 3.0f;
        _pos.z -= 0.5f;
        transform.position = _pos;
    }
}
