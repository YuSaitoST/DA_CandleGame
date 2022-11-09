using UnityEngine;

public class GoalArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        // ƒS[ƒ‹ˆ—
        GameProgress.instance_.GameFine();
    }
}
