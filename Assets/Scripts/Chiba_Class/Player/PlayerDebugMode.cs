using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugMode : MonoBehaviour
{
    [SerializeField, Tooltip("trueでゲームオーバーにならなくなります")]
    private bool debug_mode_ = false;
    public bool DebugMode
    {
        get { return debug_mode_; }
    }
}
