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
    /// Iconの表示状態を変更する
    /// </summary>
    /// <param name="active">表示状態</param>
    public void SetActive(bool active)
    {
        gameObject.transform.parent.gameObject.SetActive(active);
    }
}
