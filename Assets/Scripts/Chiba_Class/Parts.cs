using System.Collections;
using UnityEngine;

public class Parts : MonoBehaviour
{
    [SerializeField]
    private Canvas parts_ui_ = null;

    

    // Start is called before the first frame update
    void Start()
    {
        parts_ui_.enabled = false;
    }

    public void Pickup()
    {      
        StartCoroutine(OnPickup());
    }

    IEnumerator OnPickup()
    {
        parts_ui_.enabled = false;
        this.transform.position = new Vector3(0, -100, 0);
        PartsManager.GetInstance().CountPlus();

        yield return null;

        this.gameObject.SetActive(false);
        Debug.Log("アイテムを拾った");
       

    }

   
    private void OnTriggerEnter(Collider other)
    {
        //範囲内
        if (other.gameObject.tag == "Player")
        {
            parts_ui_.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //範囲外
        if (other.gameObject.tag == "Player")
        {
          parts_ui_.enabled = false;
        }

    }
}
