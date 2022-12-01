using UnityEngine;

public class MakeDark : MonoBehaviour
{
    [SerializeField] bool dark_on_ = false;
    [SerializeField] Light light_ = null;

    
    // Start is called before the first frame update
    void Start()
    {
        if (dark_on_)
        {
            //light_.SetActive(false);
            //RenderSettings.reflectionIntensity = 0;
            //RenderSettings.ambientSkyColor = Color.black;
            light_.color = new Color(0.25f, 0.6f, 0.7f, 1.0f);
        }
    }
}
