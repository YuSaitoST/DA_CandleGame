using System.Collections;
using UnityEngine;

public class FadeFootPrint : MonoBehaviour
{
    private MeshRenderer meshRender_ = null;
    float speed_ = 0.015f;


    void Start()
    {
        meshRender_ = GetComponent<MeshRenderer>();

        StartCoroutine(Disappearing());
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    IEnumerator Disappearing()
    {
        while (0 < meshRender_.material.color.a)
        {
            meshRender_.material.color = new Color(1, 1, 1, 1 - speed_ * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
