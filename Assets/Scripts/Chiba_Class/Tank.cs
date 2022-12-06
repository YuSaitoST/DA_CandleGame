using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //[SerializeField]
    //private Canvas tank_ui_ = null;
    // Start is called before the first frame update
    void Start()
    {
       // tank_ui_.enabled = false;
    }
    public void Pickup()
    {
        StartCoroutine(OnPickup());
    }

    IEnumerator OnPickup()
    {
       // tank_ui_.enabled = false;
        this.transform.position = new Vector3(0, -100, 0);
        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);
        Debug.Log("ƒ^ƒ“ƒN‚ğE‚Á‚½");


    }
    private void OnTriggerEnter(Collider other)
    {
        //”ÍˆÍ“à
        if (other.gameObject.CompareTag("Player"))
        {
          //  tank_ui_.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //”ÍˆÍŠO
        if (other.gameObject.CompareTag("Player"))
        {
          //  tank_ui_.enabled = false;
        }

    }
}
