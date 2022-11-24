using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(
       GameProgress.instance_.GetParameters().sbmarine_pos_x,
       GameProgress.instance_.GetParameters().sbmarine_pos_y,
       GameProgress.instance_.GetParameters().sbmarine_pos_z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
