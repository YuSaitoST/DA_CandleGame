using UnityEngine;

public class SubmarineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //初期位置の設定
       this.transform.position = new Vector3(
       GameProgress.instance_.GetParameters().sbmarine_pos_x,
       GameProgress.instance_.GetParameters().sbmarine_pos_y,
       GameProgress.instance_.GetParameters().sbmarine_pos_z);
    }

    
   
   
}
