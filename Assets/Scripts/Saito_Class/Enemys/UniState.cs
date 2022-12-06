using UnityEngine;

/// <summary>
/// ÉEÉjÇÃèÛë‘
/// </summary>
enum UNI_STATE : int { 
    UNI_NORMAL  = 0,
    UNI_ANGRY   = 1
}


public class UniState : MonoBehaviour
{
    [SerializeField] GameObject[] obj_state_ = null;
    UNI_STATE state_ = UNI_STATE.UNI_NORMAL;


    // Start is called before the first frame update
    void Start()
    {
        state_ = UNI_STATE.UNI_NORMAL;
    }

    /// <summary>
    /// ägëÂââèo
    /// </summary>
    public void BigMode()
    {
        state_ = UNI_STATE.UNI_ANGRY;
        obj_state_[0].SetActive(false);
        obj_state_[1].SetActive(true);
    }

    public void SmallMode()
    {
        state_ = UNI_STATE.UNI_NORMAL;
        obj_state_[0].SetActive(true);
        obj_state_[1].SetActive(false);
    }
}
