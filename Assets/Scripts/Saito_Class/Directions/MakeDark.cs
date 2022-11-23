using UnityEngine;

public class MakeDark : MonoBehaviour
{
    [SerializeField] bool       dark_on_    = false;    // �_�[�N���[�h
    [SerializeField] GameObject light_      = null;     // ���C�����C�g


    // Start is called before the first frame update
    void Start()
    {
        if (dark_on_)
        {
            light_.SetActive(false);
            RenderSettings.reflectionIntensity = 0;
            RenderSettings.ambientSkyColor = Color.black;
        }
    }
}
