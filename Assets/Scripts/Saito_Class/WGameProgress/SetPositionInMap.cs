using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionInMap : MonoBehaviour
{
    public static List<SetPositionInMap> positionInMap_ = new List<SetPositionInMap>();

    [SerializeField] GameObject targetObj_ = null;
    RectTransform rectT_;


    // Start is called before the first frame update
    void Start()
    {
        positionInMap_.Add(this);

        rectT_ = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectT_.anchoredPosition = new Vector3(targetObj_.transform.position.x * 24, targetObj_.transform.position.z * 24 + 50.0f, 0.0f);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetTarget(GameObject obj)
    {
        targetObj_ = obj;
    }
}
