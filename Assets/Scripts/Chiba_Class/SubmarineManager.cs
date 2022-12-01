using UnityEngine;

public class SubmarineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //‰ŠúˆÊ’u‚Ìİ’è
        Paramater _param = GameProgress.instance_.GetParameters();
        float _pos_x = _param.sbmarine_pos_x;
        float _pos_y = _param.sbmarine_pos_y;
        float _pos_z = _param.sbmarine_pos_z;

        this.transform.position = new Vector3(_pos_x, _pos_y, _pos_z);
    }

    
   
   
}
