using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BG_Scroll : MonoBehaviour
{
    [Header("アニメーション用パネルオブジェクト")]
    [SerializeField] RectTransform panel_sea_ = null;
    [SerializeField] RectTransform panel_mnt_ = null;
    [SerializeField] Image panel_flash_ = null;

    [Header("スクロール関連値")]
    [SerializeField] float speed_scroll_ = 8.0f;
    [SerializeField] float pos_y_max_scroll_ = -6580.0f;
    [SerializeField] float pos_y_out_of_sea_ = -6462.0f;

    [Header("ぷかぷか浮く範囲")]
    [SerializeField] float reaction_range_sea_ = 0.6f;
    [SerializeField] float reaction_range_mnt_ = 0.25f;

    [Header("フラッシュ関連値")]
    [SerializeField] float rate_of_increase_ = 0.8f;    // フェードの上昇率
    [SerializeField] float start_timing_ = 6;

    [Header("音周り")]
    [SerializeField] AudioSource audioSource_ = null;
    [SerializeField] AudioClip se_bable_ = null;
    [SerializeField] AudioClip se_OutOnWater_ = null;

    float time_ = 0.0f; // ぷかぷか浮く挙動の経過時間


    private void Start()
    {
        panel_flash_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        audioSource_.clip = se_bable_;
        audioSource_.loop = true;
    }

    // 脱出アニメーション
    public IEnumerator PlayONeShot(IEnumerator displays)
    {
        // SE鳴らす
#if !UNITY_EDITOR
        audioSource_.PlayOneShot(se_bable_);
#endif

        // 水面に上がるスクロール処理
        while (pos_y_max_scroll_ < panel_sea_.anchoredPosition.y)
        {
            speed_scroll_ *= Mathf.Pow(1.0005f, 2);
            panel_sea_.anchoredPosition -= new Vector2(0.0f, speed_scroll_);

            // 光が入ってくる
            if (panel_flash_.color.a < 1.0f)
            {
                time_ += Time.deltaTime;
                panel_flash_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Pow(3, time_ - start_timing_) * rate_of_increase_);
            }

            yield return null;
        }

        // 目が慣れてくる...
        StartCoroutine(FlashOut());

        yield return null;

        StartCoroutine(displays);

        time_ = 0.0f;

        // ぷかぷか浮く処理
#if !UNITY_EDITOR
        audioSource_.loop = false;
        audioSource_.PlayOneShot(se_OutOnWater_);
#endif
        float _cos = 0.0f;
        while (0.0f < reaction_range_sea_)
        {
            _cos = Mathf.Cos(time_ += Time.deltaTime);

            // 海
            reaction_range_sea_ = Mathf.Max(reaction_range_sea_ - 0.025f * Time.deltaTime, 0.0f);
            panel_sea_.anchoredPosition -= new Vector2(0.0f, _cos * reaction_range_sea_);

            // 山
            reaction_range_mnt_ = Mathf.Max(reaction_range_mnt_ - 0.025f * Time.deltaTime, 0.0f);
            panel_mnt_.anchoredPosition -= new Vector2(0.0f, _cos * reaction_range_mnt_);

            yield return null;
        }

        yield return null;
    }

    // フラッシュ
    IEnumerator FlashOut()
    {
        while (0.0f < panel_flash_.color.a)
        {
            panel_flash_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Max(panel_flash_.color.a - 0.125f * Time.deltaTime/*0.0125f*/, 0.0f));

            yield return null;
        }

        yield return null;
    }
}
