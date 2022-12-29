using UnityEngine;

public class RaderIcon : MonoBehaviour
{
    [SerializeField, Tooltip("アイコン")] GameObject pref_icon_ = null;
    GameObject icon_;

    private void Start()
    {
        Vector3 _pos = transform.position;
        icon_ = Instantiate(pref_icon_, new Vector3(_pos.x, 10.0f, _pos.z), Quaternion.Euler(90.0f, 0.0f, 0.0f));
        icon_.transform.parent = transform;
    }

    /// <summary>
    /// 発見したら、アイコンを削除する
    /// </summary>
    public void Detectioned()
    {
        Destroy(icon_);
    }
}
