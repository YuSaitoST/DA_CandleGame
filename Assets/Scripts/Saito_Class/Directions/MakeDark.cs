using UnityEngine;

public class MakeDark : MonoBehaviour
{
    [SerializeField] bool dark_on_ = false;
    [SerializeField] GameObject light_ = null;


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
