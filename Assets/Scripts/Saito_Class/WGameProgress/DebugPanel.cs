using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] Text txt_message_ = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �p�l���̕\����Ԃ�ύX����
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// �f�o�b�O���b�Z�[�W��\��������
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    public void SetMessageText(string message)
    {
        txt_message_.text = message;
    }
}
