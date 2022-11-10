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
    /// パネルの表示状態を変更する
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// デバッグメッセージを表示させる
    /// </summary>
    /// <param name="message">メッセージ</param>
    public void SetMessageText(string message)
    {
        txt_message_.text = message;
    }
}
