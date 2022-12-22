using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueArea : MonoBehaviour
{
    [SerializeField]
    private Fellow fellow_ = null;

    [SerializeField]
    private BoxCollider collider_ = null;

    private Transform transform_ = null;

    private void Start()
    {
        collider_ = GetComponent<BoxCollider>();
        transform_ = GetComponent<Transform>();
    }
    public void Follow()
    {
        transform_.position = new(0, -100, 0);
        collider_.enabled = false;
        fellow_.Follow();
       
        
        gameObject.SetActive(false);
    }
}
