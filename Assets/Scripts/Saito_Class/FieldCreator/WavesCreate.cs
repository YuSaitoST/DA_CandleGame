using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesCreate : MonoBehaviour
{
    [SerializeField] GameObject prefab_ = null;


    // Start is called before the first frame update
    void Start()
    {
        const float DLT = 3.0f;
        GameObject obj;
        float x = -6;
        float y = 0.7f;
        float z = -6;
        for(int i = 0; i < 5; ++i)
        {
            for(int j = 0; j < 5; ++j)
            {
                obj = Instantiate(prefab_, new Vector3(x + DLT * i, y, z + DLT * j), Quaternion.identity);
                obj.transform.parent = transform;
            }
        }
    }
}
