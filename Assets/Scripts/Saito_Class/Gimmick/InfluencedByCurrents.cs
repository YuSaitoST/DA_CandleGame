using System.Collections.Generic;
using UnityEngine;

/*
 * �e�����󂯂�I�u�W�F�N�g�ɃA�^�b�`����
 */

public class InfluencedByCurrents : MonoBehaviour
{
    static List<Rigidbody> targets = new List<Rigidbody>();   // �C���̉e�����󂯂�Ώۂ̃X�N���v�g
    int id_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        id_=targets.Count;
        targets.Add(GetComponent<Rigidbody>());
    }

    void OnDestory()
    {
        // �|�[�Y�Ώۂ��珜�O����
        targets.Remove(GetComponent<Rigidbody>());
    }

    public static List<Rigidbody> GetTargetList() { return targets; }
    public int GetID() { return id_; }
}
