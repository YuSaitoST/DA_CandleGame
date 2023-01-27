using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BG_Scroll : MonoBehaviour
{
    [Header("�A�j���[�V�����p�p�l���I�u�W�F�N�g")]
    [SerializeField] RectTransform panel_sea_ = null;
    [SerializeField] RectTransform panel_mnt_ = null;
    [SerializeField] Image panel_flash_ = null;

    [Header("�X�N���[���֘A�l")]
    [SerializeField] float speed_scroll_ = 8.0f;
    [SerializeField] float pos_y_max_scroll_ = -6580.0f;
    [SerializeField] float pos_y_out_of_sea_ = -6462.0f;

    [Header("�Ղ��Ղ������͈�")]
    [SerializeField] float reaction_range_sea_ = 0.6f;
    [SerializeField] float reaction_range_mnt_ = 0.25f;

    [Header("�t���b�V���֘A�l")]
    [SerializeField] float rate_of_increase_ = 0.8f;    // �t�F�[�h�̏㏸��
    [SerializeField] float start_timing_ = 6;

    [Header("������")]
    [SerializeField] AudioSource audioSource_ = null;
    [SerializeField] AudioClip se_bable_ = null;
    [SerializeField] AudioClip se_OutOnWater_ = null;

    float time_ = 0.0f; // �Ղ��Ղ����������̌o�ߎ���


    private void Start()
    {
        panel_flash_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        audioSource_.clip = se_bable_;
        audioSource_.loop = true;
    }

    // �E�o�A�j���[�V����
    public IEnumerator PlayONeShot(IEnumerator displays)
    {
        // SE�炷
#if !UNITY_EDITOR
        audioSource_.PlayOneShot(se_bable_);
#endif

        // ���ʂɏオ��X�N���[������
        while (pos_y_max_scroll_ < panel_sea_.anchoredPosition.y)
        {
            speed_scroll_ *= Mathf.Pow(1.0005f, 2);
            panel_sea_.anchoredPosition -= new Vector2(0.0f, speed_scroll_);

            // ���������Ă���
            if (panel_flash_.color.a < 1.0f)
            {
                time_ += Time.deltaTime;
                panel_flash_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Pow(3, time_ - start_timing_) * rate_of_increase_);
            }

            yield return null;
        }

        // �ڂ�����Ă���...
        StartCoroutine(FlashOut());

        yield return null;

        StartCoroutine(displays);

        time_ = 0.0f;

        // �Ղ��Ղ���������
#if !UNITY_EDITOR
        audioSource_.loop = false;
        audioSource_.PlayOneShot(se_OutOnWater_);
#endif
        float _cos = 0.0f;
        while (0.0f < reaction_range_sea_)
        {
            _cos = Mathf.Cos(time_ += Time.deltaTime);

            // �C
            reaction_range_sea_ = Mathf.Max(reaction_range_sea_ - 0.025f * Time.deltaTime, 0.0f);
            panel_sea_.anchoredPosition -= new Vector2(0.0f, _cos * reaction_range_sea_);

            // �R
            reaction_range_mnt_ = Mathf.Max(reaction_range_mnt_ - 0.025f * Time.deltaTime, 0.0f);
            panel_mnt_.anchoredPosition -= new Vector2(0.0f, _cos * reaction_range_mnt_);

            yield return null;
        }

        yield return null;
    }

    // �t���b�V��
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
