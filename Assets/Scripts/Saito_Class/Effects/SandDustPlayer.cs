using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDustPlayer : MonoBehaviour
{
    [SerializeField] GameObject prefab_ = null;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EffectCreator());
    }

    private void OnDestroy()
    {
        StopCoroutine(EffectCreator());
    }

    private void OnDisable()
    {
        StopCoroutine(EffectCreator());
    }

    IEnumerator EffectCreator()
    {
        while (true)
        {
            if(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") != 0.0f)
            {
                Instantiate(prefab_, GameProgress.instance_.GetPlayerPos(), Quaternion.identity);
            }

            yield return new WaitForSeconds(1.0f);
        }

        yield return null;
    }
}
