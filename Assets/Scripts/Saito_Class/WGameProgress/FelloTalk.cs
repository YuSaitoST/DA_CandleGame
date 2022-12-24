using System.Collections;
using UnityEngine;
using Fungus;

public class FelloTalk : MonoBehaviour
{
    [SerializeField, Tooltip("Flow Chat")] Flowchart flowchart_ = null;
    [SerializeField, Tooltip("�_�C�A���O")] GameObject dialog_ = null;
    [SerializeField, Tooltip("�_�C�A���O�O�̉�b�u���b�N��")] string message_front_ = "";
    [SerializeField, Tooltip("�_�C�A���O��̉�b�u���b�N��")] string message_back_ = "";


    void Start()
    {
        dialog_.SetActive(false);
    }

    /// <summary>
    /// ��b�J�n
    /// </summary>
    public void PlayTalk()
    {
        flowchart_.SendFungusMessage(message_front_);
    }

    /// <summary>
    /// �_�C�A���O�\��
    /// </summary>
    public void OpenDialog()
    {
        dialog_.SetActive(true);
        StartCoroutine(StayNextInput());
    }

    IEnumerator StayNextInput()
    {

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        dialog_.SetActive(false);

        yield return null;
        flowchart_.SendFungusMessage(message_back_);

        yield return null;
    }
}

/*
 * [�A�Z�b�g�T�C�g]
 * https://github.com/snozbot/fungus/releases/tag/v3.13.7
 */