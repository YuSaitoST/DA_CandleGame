using System;
using System.Collections.Generic;
using UnityEngine;

public class PauserObject : MonoBehaviour
{
    static List<PauserObject> targets = new List<PauserObject>();   // �|�[�Y�Ώۂ̃X�N���v�g
    Behaviour[] pauseBehavs = null; // �|�[�Y�Ώۂ̃R���|�[�l���g

    // ������
    void Start()
    {
        // �|�[�Y�Ώۂɒǉ�����
        targets.Add(this);
    }

    // �j�������Ƃ�
    void OnDestory()
    {
        // �|�[�Y�Ώۂ��珜�O����
        targets.Remove(this);
    }

    // �|�[�Y���ꂽ�Ƃ�
    void OnPause()
    {
        if (pauseBehavs != null)
        {
            return;
        }

        // �L����Behaviour���擾
        pauseBehavs = Array.FindAll(GetComponentsInChildren<Behaviour>(), (obj) => {
            if (obj == null)
            {
                return false;
            }
            return obj.enabled;
        });

        foreach (var com in pauseBehavs)
        {
            com.enabled = false;
        }
    }

    // �|�[�Y�������ꂽ�Ƃ�
    void OnResume()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // �|�[�Y�O�̏�Ԃ�Behaviour�̗L����Ԃ𕜌�
        foreach (var com in pauseBehavs)
        {
            com.enabled = true;
        }
        pauseBehavs = null;
    }

    // �|�[�Y
    public static void Pause()
    {
        foreach (PauserObject obj in FindObjectsOfType<PauserObject>())
        {
            if (obj != null)
            {
                obj.OnPause();
            }
        }
    }

    // �|�[�Y����
    public static void Resume()
    {
        foreach (var obj in targets)
        {
            obj.OnResume();
        }
    }
}

/*
 * [�Q�l]
 * https://sleepnel.hatenablog.com/entry/2016/08/10/093000
 */