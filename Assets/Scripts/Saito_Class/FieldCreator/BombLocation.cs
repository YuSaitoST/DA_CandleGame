using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLocation : MonoBehaviour
{
    [SerializeField] bool isInstalled_ = false; // �ݒu���


    // Start is called before the first frame update
    void Start()
    {
        isInstalled_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    // �v���C���[�����͂������A������Ăяo��
    public void ItemsInPlace()
    {
        // �Q�[���I����͏��������Ȃ�
        if (BombManager.instance_.IsAllInstalled())
        {
            return;
        }

        isInstalled_ = true;
        BombManager.instance_.Check_AllInstalled();   // �S�Ă̐ݒu�G���A�𒲂ׂ�
    }

    public bool IsInstalled()
    {
        return isInstalled_;
    }
}
