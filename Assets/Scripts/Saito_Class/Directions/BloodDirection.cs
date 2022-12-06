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
    [SerializeField] float ratio_oxyNoneDamage_ = 0.03f;

    [SerializeField] ParticleEffectPlayer effect_ = null;

    [SerializeField] Text txt_debug_ = null;
    bool pushFrag_ = false;

    float alpha_ = 0.0f;

    IEnumerator coro_oxyEmpty_;


    // Start is called before the first frame update
    void Start()
    {
        alpha_ = 0.0f;
        ratio_damage_ = 0.5f;
        ratio_oxyNoneDamage_ = 0.03f;
        SetAlpha();

        coro_oxyEmpty_ = OxygenEmpty();

#if debug_on_
        txt_debug_.text = "b-a : " + alpha_;
#endif
    }

    /// <summary>
    /// alpha値のセット
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
    /// 被ダメージ演出
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
            StartCoroutine(coro_oxyEmpty_);
        }
    }

    /// <summary>
    /// ダメージ回復演出
    /// </summary>
    public void DamageRecovery()
    {
        StopCoroutine(coro_oxyEmpty_);
        coro_oxyEmpty_ = null;
        coro_oxyEmpty_ = OxygenEmpty();

        StartCoroutine(Wait_BeginsToRecover());

        pushFrag_ = false;
    }

    /// <summary>
    /// ダメージが最大かを返す
    /// </summary>
    /// <returns>ダメージが最大か</returns>
    public bool DamageMax()
    {
        return alpha_ == 1.0f;
    }

    /// <summary>
    /// 回復し始めるまで待機する
    /// </summary>
    /// <returns>コルーチン</returns>
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
    /// 瞬間のダメージ処理・回復処理
    /// </summary>
    /// <returns>コルーチン</returns>
    IEnumerator DamageDoneDirection()
    {
        alpha_ += ratio_damage_;
        SetAlpha();

        yield return StartCoroutine(Wait_BeginsToRecover());

        pushFrag_ = false;

        yield break;
    }

    /// <summary>
    /// 酸素残量がなくなった時演出
    /// </summary>
    /// <returns>コルーチン</returns>
    IEnumerator OxygenEmpty()
    {
        while (alpha_ < 1.0f)
        {
            yield return new WaitForSeconds(1.0f);

            alpha_ += ratio_oxyNoneDamage_;
            SetAlpha();
        }

        pushFrag_ = false;

        yield break;
    }
}
