using UnityEngine;

public class RaderIcon : MonoBehaviour
{
    [SerializeField, Tooltip("�A�C�R��")] GameObject pref_icon_ = null;
    GameObject icon_;

    private void Start()
    {
        icon_ = Instantiate(pref_icon_, new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity/*.Euler(90.0f, 0.0f, 0.0f)*/);
        icon_.transform.parent = transform;
    }

    /// <summary>
    /// ����������A�A�C�R�����폜����
    /// </summary>
    public void Detectioned()
    {
        Destroy(icon_);
    }
}
