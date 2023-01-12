using UnityEngine;

public class SetterAudioSource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameProgress.instance_.SetAudioSource(GetComponent<AudioSource>());
    }
}
