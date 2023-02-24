using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSE : MonoBehaviour
{
    [SerializeField]
    private AudioSource audiosource_ = null;

    [SerializeField]
    private AudioClip arrow_move_ = null;

    [SerializeField]
    private AudioClip decision_ = null;

    [SerializeField]
    private AudioClip oxygen_get_ = null;

    [SerializeField]
    private AudioClip oxygen_not_ = null;
    //�����ړ���������
    public void ArrowMoveSE()
    {
        audiosource_.clip = arrow_move_;
        audiosource_.Play();
    }
    //���肷�鎞
    public void DecisionSE()
    {
        audiosource_.clip = decision_;
        audiosource_.Play();
    }
    //�_�f�񕜎�
    public void OxygenGetSE()
    {
        audiosource_.clip = oxygen_get_;
        audiosource_.Play();

    }
    //�����͂ɂĎ_�f���񕜂ł��Ȃ��Ƃ�
    public void OxygenNotSE()
    {
        audiosource_.clip = oxygen_not_;
        audiosource_.Play();
    }
}
