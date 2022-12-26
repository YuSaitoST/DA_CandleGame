using UnityEngine;

public class RaderIcon : MonoBehaviour
{
    /// <summary>
    /// 発見したら、アイコンを削除する
    /// </summary>
    public void Detectioned()
    {
        Destroy(gameObject);
    }
}
