using System;
using UnityEngine;

[Serializable] public class FellowDataList
{
    public FellowData[] lists;
}

[Serializable] public class FellowData
{
    public bool move_start;
    public int id;
    public int kind;
    public float pos_x;
    public float pos_y;
    public float pos_z;
    public float rot_y;
}

public class FellowPosDatas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
