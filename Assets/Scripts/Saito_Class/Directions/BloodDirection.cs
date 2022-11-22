#define debug_on_

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodDirection : MonoBehaviour
{
    [SerializeField] Image img_blood_ = null;   // 血の演出画像

    [SerializeField] float time_recovery_       = 5.0f;     // 被ダメージから回復開始までの時間
    [SerializeField] float speed_recovery_      = 0.1f;    // 被ダメージ後の回復速度
    [SerializeField] float speed_damageFade_    = 1.0f;     // 被ダメージ時の増加速度
    [SerializeField] float ratio_damage_        = 0.5f;     // 被ダメージ時の増加値の割合(1回につき)

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

    // Update is called once per frame
    void Update()
    {
#if debug_on_
        if (Input.GetKey(KeyCode.T))
        {
            if (!pushFrag_)
            {
                pushFrag_ = true;
                DamageDone();
            }
        }
        else
        {
            pushFrag_ = false;
        }
#endif
    }

    /// <summary>
    /// 不透明度の設定
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
    /// 被ダメージ時の演出
    /// </summary>
    public void DamageDone()
    {
        StartCoroutine(DamageDoneDirection());
    }

    IEnumerator Wait_BeginsToRecover()
    {
        yield return new WaitForSeconds(time_recovery_);

        // 回復処理
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

        yield break;
    }
}
