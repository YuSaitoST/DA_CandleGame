using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Scroll : MonoBehaviour
{
    [SerializeField] RectTransform panel_sea_ = null;
    [SerializeField] RectTransform panel_mnt_ = null;

    [SerializeField] float speed_scroll_ = 8.0f;
    const float max_scroll_pos_y_ = -6580.0f;//-3960.0f;

    // �Ղ��Ղ�����
    [SerializeField] float reaction_range_sea_ = 0.6f;
    [SerializeField] float reaction_range_mnt_ = 0.25f;
    float time_ = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Scroll());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Scroll(IEnumerator displays)
    {
        // ���ʂɏオ��X�N���[������
        while (max_scroll_pos_y_ < panel_sea_.anchoredPosition.y)
        {
            speed_scroll_ = speed_scroll_ * Mathf.Pow(1.0005f, 2);
            panel_sea_.anchoredPosition -= new Vector2(0.0f, speed_scroll_);
            yield return null;
        }

        StartCoroutine(displays);

        // �Ղ��Ղ���������
        while (0.0f < reaction_range_sea_)
        {
            // �C
            time_ += Time.deltaTime;
            reaction_range_sea_ = Mathf.Max(reaction_range_sea_ - 0.00025f, 0.0f);
            panel_sea_.anchoredPosition -= new Vector2(0.0f, Mathf.Cos(time_) * reaction_range_sea_);

            // �R
            reaction_range_mnt_ = Mathf.Max(reaction_range_mnt_ - 0.00025f, 0.0f);
            panel_mnt_.anchoredPosition -= new Vector2(0.0f, Mathf.Cos(time_) * reaction_range_mnt_);

            yield return null;
        }

        yield return null;
    }
}
