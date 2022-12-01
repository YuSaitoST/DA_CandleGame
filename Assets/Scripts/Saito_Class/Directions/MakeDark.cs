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
            light_.color = new Color(0.13f, 0.3f, 0.3f, 1.0f);
            RenderSettings.fog = true;
        }
    }
}
