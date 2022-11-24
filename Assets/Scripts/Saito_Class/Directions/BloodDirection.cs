#define debug_on_

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodDirection : MonoBehaviour
{
    [SerializeField] Image img_blood_ = null;

    [SerializeField] float time_recovery_       = 5.0f;
    [SerializeField] float speed_recovery_      = 0.1f;
    [SerializeField] float speed_damageFade_    = 1.0f;
    [SerializeField] float ratio_damage_        = 0.5f;

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
            StartCoroutine(DamageDoneDirection());
        }
    }

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

    IEnumerator DamageDoneDirection()
    {
        alpha_ += ratio_damage_;
        SetAlpha();

        yield return StartCoroutine(Wait_BeginsToRecover());

        pushFrag_ = false;

        yield break;
    }
}
