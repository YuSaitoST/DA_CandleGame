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
        GameObject _obj;
        int _count = 9;
        float _x = -13.0f;
        float _y = 2.7f;
        float _z = -14.5f;
        for(int i = 0; i < _count; ++i)
        {
            for(int j = 0; j < _count; ++j)
            {
                _obj = Instantiate(prefab_, new Vector3(_x + DLT * i, _y, _z + DLT * j), Quaternion.identity);
                _obj.transform.parent = transform;
            }
        }
    }
}
