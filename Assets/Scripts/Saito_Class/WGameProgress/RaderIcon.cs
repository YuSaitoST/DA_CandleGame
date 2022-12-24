using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderIcon : MonoBehaviour
{
    [SerializeField] GameObject pre_icon_ = null;


    void Start()
    {

        gameObject.transform.parent = Instantiate(pre_icon_, gameObject.transform.position, Quaternion.identity).transform;
    }

    /// <summary>
    /// Icon‚Ì•\¦ó‘Ô‚ğ•ÏX‚·‚é
    /// </summary>
    /// <param name="active">•\¦ó‘Ô</param>
    public void SetActive(bool active)
    {
        gameObject.transform.parent.gameObject.SetActive(active);
    }
}
