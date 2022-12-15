#define debug_on_

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodDirection : MonoBehaviour
{
    [SerializeField] Image img_blood_ = null;
    [SerializeField] AudioSource audioSource_ = null;
    [SerializeField] AudioClip se_heart_rate_ = null;

    [SerializeField] float time_recovery_       = 5.0f;
    [SerializeField] float speed_recovery_      = 0.1f;
    [SerializeField] float ratio_damage_        = 0.5f;
    [SerializeField] float ratio_oxyNoneDamage_ = 0.03f;

    [SerializeField] ParticleEffectPlayer effect_ = null;

#if debug_on_
    [SerializeField] Text txt_debug_ = null;
#endif

    bool pushFrag_damageDone_ = false;
    bool pushFrag_oxyEmpty_ = false;

    float alpha_ = 0.0f;

    IEnumerator coro_oxyEmpty_;


    // Start is called before the first frame update
    void Start()
    {
        alpha_ = 0.0f;
        ratio_damage_ = 0.5f;
        ratio_oxyNoneDamage_ = 0.03f;
        SetAlpha();

        audioSource_.volume = 0.5f;
        audioSource_.loop = true;

        coro_oxyEmpty_ = OxygenEmpty();

#if debug_on_
        txt_debug_.text = "b-a : " + alpha_;
#endif
    }

    /// <summary>
    /// alpha�l�̃Z�b�g
    /// </summary>
    private void SetAlpha()
    {
        img_blood_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Min(alpha_, 1.0f));

#if debug_on_
        txt_debug_.text = "b-a : " + alpha_;
#endif

        if (img_blood_.color.a == 1.0f)
        {
            audioSource_.Stop();
            audioSource_.volume = 0.0f;
            audioSource_.loop = false;
            GameProgress.instance_.GameOver();
        }
    }

    /// <summary>
    /// ��_���[�W���o
    /// </summary>
    public void DamageDone()
    {
        if (!pushFrag_damageDone_)
        {
            pushFrag_damageDone_ = true;
            effect_.PlayOneShot();
            audioSource_.PlayOneShot(se_heart_rate_);
            alpha_ += ratio_damage_;
            SetAlpha();
            audioSource_.volume = alpha_;
            StartCoroutine(DamageDoneDirection());
        }
    }

    /// <summary>
    /// �_�f����ɂȂ��������o
    /// </summary>
    public void OxyEmpty()
    {
        if (!pushFrag_oxyEmpty_)
        {
            pushFrag_oxyEmpty_ = true;
            audioSource_.PlayOneShot(se_heart_rate_);
            StartCoroutine(coro_oxyEmpty_);
        }
    }

    /// <summary>
    /// �_���[�W�񕜉��o
    /// </summary>
    public void DamageRecovery()
    {
        StopCoroutine(coro_oxyEmpty_);
        coro_oxyEmpty_ = null;
        coro_oxyEmpty_ = OxygenEmpty();

        StartCoroutine(Wait_BeginsToRecover());
    }

    /// <summary>
    /// �_���[�W���ő傩��Ԃ�
    /// </summary>
    /// <returns>�_���[�W���ő傩</returns>
    public bool DamageMax()
    {
        return alpha_ == 1.0f;
    }

    /// <summary>
    /// �񕜂��n�߂�܂őҋ@����
    /// </summary>
    /// <returns>�R���[�`��</returns>
    IEnumerator Wait_BeginsToRecover()
    {
        yield return new WaitForSeconds(time_recovery_);

        if (pushFrag_damageDone_)
        {
            pushFrag_damageDone_ = false;
        }

        if (pushFrag_oxyEmpty_)
        {
            pushFrag_oxyEmpty_ = false;
        }

        while (0.0f < alpha_)
        {
            yield return null;

            alpha_ = Mathf.Max(0.0f, alpha_ - speed_recovery_ * Time.deltaTime);
            SetAlpha();

            audioSource_.volume = alpha_;
        }

        audioSource_.Stop();

        yield break;
    }

    /// <summary>
    /// �u�Ԃ̃_���[�W�����E�񕜏���
    /// </summary>
    /// <returns>�R���[�`��</returns>
    IEnumerator DamageDoneDirection()
    {
        yield return StartCoroutine(Wait_BeginsToRecover());

        if (alpha_ == 1.0f)
        {
            audioSource_.Stop();
        }

        yield break;
    }

    /// <summary>
    /// �_�f�c�ʂ��Ȃ��Ȃ��������o
    /// </summary>
    /// <returns>�R���[�`��</returns>
    IEnumerator OxygenEmpty()
    {
        while (alpha_ < 1.0f)
        {
            yield return new WaitForSeconds(1.0f);

            alpha_ += ratio_oxyNoneDamage_;
            SetAlpha();

            audioSource_.volume = alpha_;
        }

        pushFrag_damageDone_ = false;

        if (alpha_ == 1.0f)
        {
            audioSource_.Stop();
        }

        yield break;
    }
}
