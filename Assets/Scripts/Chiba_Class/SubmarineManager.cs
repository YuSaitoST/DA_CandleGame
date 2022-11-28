using UnityEngine;

public class SubmarineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //‰ŠúˆÊ’u‚Ìİ’è
       this.transform.position = new Vector3(
       GameProgress.instance_.GetParameters().sbmarine_pos_x,
       GameProgress.instance_.GetParameters().sbmarine_pos_y,
       GameProgress.instance_.GetParameters().sbmarine_pos_z);
    }

    
   
   
}
