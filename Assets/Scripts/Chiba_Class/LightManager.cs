
using UnityEngine;

//テスト用
[ExecuteInEditMode]
public class LightManager : MonoBehaviour
{
    [Header("スポットライト調整")]
    [SerializeField]
    GameObject pointlight_object_=null;

    [SerializeField]
    private float pointlight_strength_ = 0.3f;

    [SerializeField]
    private float pointlight_range_ = 1.2f;

    [Header("ポイントライト調整")]
    [SerializeField]
    GameObject spotlight_object_ = null;

    [SerializeField]
    private float spotlight_strength_ = 1.0f;

    [SerializeField]
    private float spotlight_range_ = 2.5f;

    //[SerializeField]
    ////入力を無効にしたいときにtrue
    //private bool input_stop_ = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        pointlight_object_.GetComponent<Light>().intensity = pointlight_strength_;
        pointlight_object_.GetComponent<Light>().range = pointlight_range_;

        spotlight_object_.GetComponent<Light>().intensity = spotlight_strength_;
        spotlight_object_.GetComponent<Light>().range = spotlight_range_;
    }
}
