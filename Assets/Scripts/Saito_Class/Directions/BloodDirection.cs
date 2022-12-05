#define debug_on_

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodDirection : MonoBehaviour
{
    [SerializeField] Image img_blood_ = null;

    [SerializeField] float time_recovery_       = 5.0f;
    [SerializeField] float speed_recovery_      = 0.1f;
    [SerializeField] float ratio_damage_        = 0.5f;
    [SerializeField] float ratio_oxyNoneDamage_ = 0.3f;

    [SerializeField] ParticleEffectPlayer effect_ = null;

    [SerializeField] Text txt_debug_ = null;
    bool pushFrag_ = false;

    float alpha_ = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        alpha_ = 0.0f;
        ratio_damage_ = 0.5f;
        SetAlpha();

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
            GameProgress.instance_.GameOver();
        }
    }

    /// <summary>
    /// ��_���[�W���o
    /// </summary>
    public void DamageDone()
    {
        if (!pushFrag_)
        {
            pushFrag_ = true;
            effect_.PlayOneShot();
            StartCoroutine(DamageDoneDirection());
        }
    }

    public void OxyEmpty()
    {
        if (!pushFrag_)
        {
            pushFrag_ = true;
            StartCoroutine(OxygenEmpty());
        }
    }

    /// <summary>
    /// �_���[�W�񕜉��o
    /// </summary>
    public void DamageRecovery()
    {
        StopCoroutine(OxygenEmpty());
        pushFrag_ = false;
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
    /// �摜�𔖂����n�߂�܂őҋ@����
    /// </summary>
    /// <returns>�R���[�`��</returns>
    IEnumerator Wait_BeginsToRecover()
    {
        yield return new WaitForSeconds(time_recovery_);

        while (0.0f < alpha_)
        {
            yield return null;

            alpha_ = Mathf.Max(0.0f, alpha_ - speed_recovery_ * Time.deltaTime);
            SetAlpha();
        }

        yield break;
    }

    /// <summary>
    /// �_���[�W���o����
    /// </summary>
    /// <returns>�R���[�`��</returns>
    IEnumerator DamageDoneDirection()
    {
        alpha_ += ratio_damage_;
        SetAlpha();

        yield return StartCoroutine(Wait_BeginsToRecover());

        pushFrag_ = false;

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
            alpha_ += ratio_oxyNoneDamage_;
            SetAlpha();

            yield return new WaitForSeconds(1.0f);
        }

        pushFrag_= false;

        yield break;
    }
}
