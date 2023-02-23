using System.Collections;
using UnityEngine;  
using UnityEngine.UI;

public class TutorialPoint : MonoBehaviour
{
    enum Point { A, B, C, D, Tank, None }

    public static float TIME_FADEIN = 0.5f;
    public static float SPED_FADEIN = 2.0f;
    public static float SPED_FADEOT = 1.3f;

    public static TutorialPoint[] isOpenTutorial = { null, null, null, null, null };

    [SerializeField] GameObject target_ = null; // �`���[�g���A���̑Ώ�
    [SerializeField] Image img_talk_ = null;    // ��L�摜��\��������
    [SerializeField] Point point_ = Point.None;  // �z�u�ʒu


    // Start is called before the first frame update
    void Start()
    {
        img_talk_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (point_ == Point.A || point_ == Point.D)
            {
                CloseTutorial();
                StartCoroutine(FadeInOut());
            }
            else if (point_ == Point.B || point_ == Point.C)
            {
                CloseTutorial();
                StartCoroutine(FadeAndMission());
            }
        }
    }

    /// <summary>
    /// �����ɊJ�n�������ŕ���
    /// </summary>
    public void AutoTutorial()
    {
        CloseTutorial();
        StartCoroutine(FadeInOut());
    }

    public void Close()
    {
        img_talk_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void CloseTutorial()
    {
        // ���ɕ\������Ă�����̂�T��
        for (int i = 0; i < 5; ++i)
        {
            if (isOpenTutorial[i])
            {
                isOpenTutorial[i].Close();
                img_talk_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                break;
            }
        }
    }

    IEnumerator Fade(int inout, float speed)
    {
        if (inout == 1)
        {
            while (img_talk_.color.a < 1)
            {
                img_talk_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Min(img_talk_.color.a + speed * Time.deltaTime, 1.0f));

                yield return null;
            }
        }
        else if (inout == -1)
        {
            while(0 < img_talk_.color.a)
            {
                img_talk_.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Max(img_talk_.color.a - speed * Time.deltaTime, 0.0f));

                yield return null;
            }
        }
    }

    IEnumerator FadeInOut()
    {
        yield return StartCoroutine(Fade(1, SPED_FADEIN));

        yield return new WaitForSeconds(5.0f);

        yield return StartCoroutine(Fade(-1, SPED_FADEOT));

        gameObject.SetActive(false);
    }

    IEnumerator FadeAndMission()
    {
        yield return StartCoroutine(Fade(1, SPED_FADEIN));

        yield return null;

        Player _player = target_.GetComponent<Player>();

        if (point_ == Point.B)
        {
            // B ����
            while (_player.type_ != Player.State.Dash)
            {
                yield return null;
            }

            yield return new WaitForSeconds(5);

            yield return StartCoroutine(Fade(-1, SPED_FADEOT));
        }
        else if (point_ == Point.C)
        {
            // C ���e
            while (!_player.first_bomb_)
            {
                yield return null;
            }
        }

        yield return StartCoroutine(Fade(-1, SPED_FADEOT));

        gameObject.SetActive(false);
    }
}
