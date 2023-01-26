using UnityEngine;

/*
 * 海流にアタッチする
 */

public class OceanCurrent : MonoBehaviour
{
    [SerializeField, Tooltip("影響範囲(小さい程広範囲)")] float DIST = 0.1f;
    [SerializeField, Tooltip("海流の力の最大値")] float POWER = 5.0f;
    [SerializeField, Tooltip("力の補正値")] float CORRECTION_POWER = 1.0f;

    Vector3 position_ = Vector3.zero;
    Vector3 direction_ = Vector3.zero;

    private void Start()
    {
        position_ = transform.position;
        direction_ = transform.forward.normalized;
    }

    private void OnTriggerStay(Collider other)
    {
        float _distance = Vector3.Distance(position_, other.transform.position) * CORRECTION_POWER;
        float _power = -DIST * Mathf.Pow(_distance, 2) + POWER;
        InfluencedByCurrents.GetTargetList()[other.gameObject.GetComponent<InfluencedByCurrents>().GetID()].AddForce(direction_ * _power);
    }
}