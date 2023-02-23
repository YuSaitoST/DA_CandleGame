using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesCreate : MonoBehaviour
{
    [SerializeField] GameObject prefab_ = null;
    [SerializeField] int pieces_per_side_ = 9;
    [SerializeField] float start_x_ = -13.0f;
    [SerializeField] float start_z_ = -14.5f;


    // Start is called before the first frame update
    void Start()
    {
        const float DLT = 3.0f;
        GameObject _obj;
        float _y = 2.7f;
        for(int i = 0; i < pieces_per_side_; ++i)
        {
            for(int j = 0; j < pieces_per_side_; ++j)
            {
                _obj = Instantiate(prefab_, new Vector3(start_x_ + DLT * i, _y, start_z_ + DLT * j), Quaternion.identity);
                _obj.transform.parent = transform;
            }
        }
    }
}
