using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] AudioSource    bgm_main_   = null;
    [SerializeField] AudioSource    bgm_enes_   = null;
    [SerializeField] float          speed_fade_ = 1.5f;
    [SerializeField] float          volume_max_ = 0.5f;

    IEnumerator coro_MtoE_;
    IEnumerator coro_EtoM_;


    // Start is called before the first frame update
    void Start()
    {
        GameProgress.instance_.SetBGMMamager(this);

        bgm_main_.Play();
        bgm_main_.volume = volume_max_;
        bgm_enes_.volume = 0.0f;

        coro_MtoE_ = MtoE();
        coro_EtoM_ = EtoM();

    }

    /// <summary>
    /// MainBGM����EnemyBGM�֐؂�ւ���
    /// </summary>
    public void MainToEnemy()
    {
        bgm_enes_.Play();
        StopCoroutine(coro_EtoM_);
        coro_EtoM_ = null;
        coro_EtoM_ = EtoM();
        StartCoroutine(coro_MtoE_);
    }

    /// <summary>
    /// EnemyBGM����MainBGM�֐؂�ւ���
    /// </summary>
    public void EnemyToMain()
    {
        bgm_main_.Play();
        StopCoroutine(coro_MtoE_);
        coro_MtoE_ = null;
        coro_MtoE_ = MtoE();
        StartCoroutine(coro_EtoM_);
    }

    IEnumerator MtoE()
    {
        float _dt = 0;

        while (true)
        {
            _dt = Time.deltaTime;
            bgm_main_.volume = Mathf.Min(Mathf.Max(bgm_main_.volume - _dt, 0.0f), volume_max_);
            bgm_enes_.volume = Mathf.Min(Mathf.Max(bgm_enes_.volume + _dt, 0.0f), volume_max_);

            if (bgm_enes_.volume == 0.0f || bgm_main_.volume == 1.0f)
            {
                bgm_main_.Stop();
                break;
            }

            yield return null;
        }

        yield return null;
    }

    IEnumerator EtoM()
    {
        float _dt = 0;

        while (true)
        {
            _dt = Time.deltaTime;
            bgm_main_.volume = Mathf.Min(Mathf.Max(bgm_main_.volume + _dt, 0.0f), volume_max_);
            bgm_enes_.volume = Mathf.Min(Mathf.Max(bgm_enes_.volume - _dt, 0.0f), volume_max_);

            if (bgm_main_.volume == 0.0f && bgm_enes_.volume == 1.0f)
            {
                bgm_enes_.Stop();
                break;
            }

            yield return null;
        }

        yield return null;
    }
}

/*
 * [�l����]
 * �E�G�ɒǂ��Ă���Ԃ�ene�𗬂�
 * �E����ȊO��main���B
 * 
 * �܂�
 * �E�G�ƐڐG�����^�C�~���O�ŁA���ʂ��t�F�[�h�Ő؂�ւ��Ă�
 * �E�S�Ă̓G���瓦���΁A�t�F�[�h�Ő؂�ւ���
 * �E�G���ǐՏ�ԂȂ�A�}�l�[�W���[�N���X�ɐ\�����A���̐���0�ɂȂ�ΐؑ�
 */