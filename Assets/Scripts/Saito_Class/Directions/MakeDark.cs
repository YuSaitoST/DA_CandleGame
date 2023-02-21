using UnityEngine;

public class MakeDark : MonoBehaviour
{
#if UNITY_EDITOR
    public static bool dark_on_ = false;
#else
    public static bool dark_on_ = true;
#endif

    [SerializeField] Light light_ = null;

    
    // Start is called before the first frame update
    void Start()
    {
        if (dark_on_)
        {
            //light_.color = new Color(0.13f, 0.3f, 0.3f, 1.0f);
            light_.color = new Color(0.07f, 0.2f, 0.2f, 1.0f);
        }
        
        RenderSettings.fog = dark_on_;
    }
}
