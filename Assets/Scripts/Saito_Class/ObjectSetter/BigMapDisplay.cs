using UnityEngine;

public class BigMapDisplay : MonoBehaviour
{
    [SerializeField] GameObject parent_bigicon_ = null;
    [SerializeField] GameObject pref_bigicon_ = null;


    // Start is called before the first frame update
    void Start()
    {
        GameObject _obj = Instantiate(pref_bigicon_);
        _obj.transform.parent = parent_bigicon_.transform;
        _obj.GetComponent<SetPositionInMap>().SetTarget(gameObject);
    }
}
