using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    bool shakeFrag_ = false;

    private void Start()
    {
        shakeFrag_ = false;
    }

    private void Update()
    {
        transform.position = GetPosition();
    }

    public void ResetParamater()
    {
        shakeFrag_ = false;
    }

    public Vector3 GetPosition()
    {
        Vector3 _pos = GameProgress.instance_.GetPlayerPos();
        _pos.y = 3.0f;
        _pos.z -= 0.5f;
        return _pos;
    }

    public void Shake(float duration, float magnitude, float range)
    {
        if (!shakeFrag_)
        {
            shakeFrag_ = true;
            StartCoroutine(DoShake(duration, magnitude, range));
        }
    }

    private IEnumerator DoShake(float duration, float magnitude, float range)
    {
        shakeFrag_ = true;

        //Vector3 pos = transform.localPosition;
        Vector3 pos;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            pos = GetPosition();
            float x = pos.x + Random.Range(-range, range) * magnitude;
            float y = pos.y + Random.Range(-range, range) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = GetPosition();

        shakeFrag_ = false;
    }
}
