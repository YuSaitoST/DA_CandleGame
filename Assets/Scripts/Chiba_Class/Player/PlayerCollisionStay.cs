using UnityEngine;

public class PlayerCollisionStay : MonoBehaviour
{
    [Header("PlayerƒNƒ‰ƒX")]
    [SerializeField]
    private Player script_player_ = null;

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Vector3 _collison_transform = collision.transform.position;
            script_player_.EnemyHit(_collison_transform);
        }
    }
}
