using System.Collections;
using UnityEngine;  
using UnityEngine.UI;

public class TutorialPoint : MonoBehaviour
{
    enum Point { A, B, C, D, Tank, None }

    public static float TIME_FADEIN = 0.5f;
    public static float SPED_FADEIN = 2.0f;

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
                StartCoroutine(FadeInOut());
            }
            else if (point_ == Point.B || point_ == Point.C)
            {
                StartCoroutine(FadeAndMission());
            }
        }
    }

    /// <summary>
    /// �����ɊJ�n�������ŕ���
    /// </summary>
    public void AutoTutorial()
    {
        StartCoroutine(FadeInOut());
    }

    IEnumerator Fade(int inout)
    {
        if (inout == 1)
        {
            while (img_talk_.color.a < 1)
            {
                img_talk_.color += new Color(0.0f, 0.0f, 0.0f, inout * SPED_FADEIN * Time.deltaTime);

                yield return null;
            }
        }
        else if (inout == -1)
        {
            while(0 < img_talk_.color.a)
            {
                img_talk_.color += new Color(0.0f, 0.0f, 0.0f, inout * SPED_FADEIN * Time.deltaTime);

                yield return null;
            }
        }
        
        if (img_talk_.color.a == 0.0f)
        {
            gameObject.SetActive(false);
        }

        yield return null;
    }

    IEnumerator FadeInOut()
    {
        StartCoroutine(Fade(1));

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(Fade(-1));
        
        yield return null;
    }

    IEnumerator FadeAndMission()
    {
        StartCoroutine(Fade(1));

        yield return null;


        Player _player = target_.GetComponent<Player>();

        if (point_ == Point.B)
        {
            // B ����
            while (_player.type_ != Player.State.Dash)
            {
                yield return null;
            }
        }
        else if (point_ == Point.C)
        {
            // C ���e
            while (true)
            {
                yield return null;
            }
        }

        yield return null;

        StartCoroutine(Fade(-1));

        yield return null;
    }
}
