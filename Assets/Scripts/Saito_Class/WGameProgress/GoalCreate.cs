using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCreate : MonoBehaviour
{
    [SerializeField] GameObject pre_goal_ = null;   // �S�[���v���n�u

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �S�[����ݒu����
    /// </summary>
    /// <param name="positon">�S�[���̍��W</param>
    public void CreateGoalArea(Vector3 position)
    {
        // �v���n�u����
        GameObject obj = Instantiate(pre_goal_, position, Quaternion.identity);
        
        // �e�I�u�W�F�N�g�̐ݒ�
        obj.transform.parent = transform;
    }
}
