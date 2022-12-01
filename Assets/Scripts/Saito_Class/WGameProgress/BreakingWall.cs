using UnityEngine;

public class BreakingWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        string tags = other.tag.Substring(0, other.tag.Length - 3);
        if(tags == "OxyBomb")
        {
            Destroy(gameObject);
        }
    }
}
