using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodDirection : MonoBehaviour
{
    [SerializeField] Image img_blood_ = null;   // ���̉��o�摜

    [SerializeField] float time_recovery_       = 5.0f;     // ��_���[�W����񕜊J�n�܂ł̎���
    [SerializeField] float speed_recovery_      = 0.1f;    // ��_���[�W��̉񕜑��x
    [SerializeField] float speed_damageFade_    = 1.0f;     // ��_���[�W���̑������x
    [SerializeField] float ratio_damage_        = 0.5f;     // ��_���[�W���̑����l�̊���(1��ɂ�)

    float alpha_ = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        alpha_ = 0.0f;
        SetAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �s�����x�̐ݒ�
    /// </summary>
    private void SetAlpha()
    {
        img_blood_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Min(alpha_, 1.0f));

        if (img_blood_.color.a == 1.0f)
        {
            GameProgress.instance_.GameOver();
        }
    }

    /// <summary>
    /// ��_���[�W���̉��o
    /// </summary>
    public void DamageDone()
    {
        StartCoroutine(DamageDoneDirection());
    }

    IEnumerator Wait_BeginsToRecover()
    {
        yield return new WaitForSeconds(time_recovery_);

        // �񕜏���
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
