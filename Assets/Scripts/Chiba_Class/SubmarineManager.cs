using UnityEngine;

public class SubmarineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //初期位置の設定
        Paramater _param = GameProgress.instance_.GetParameters();
        float _pos_x = _param.submaline.pos_x;
        float _pos_y = _param.submaline.pos_y;
        float _pos_z = _param.submaline.pos_z;

        this.transform.position = new Vector3(_pos_x, _pos_y, _pos_z);
    }

    
   
   
}
