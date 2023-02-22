using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubmarineMenu : MonoBehaviour
{
    [Header("Player�N���X")]
    [SerializeField]
    private Player script_player_ = null;
    [SerializeField]
    private PlayerFellowCount script_player_count_ = null;
    [SerializeField]
    private UIManager UI_ = null;

    private bool menu_flg_ = false;

    [SerializeField]
    private float wait_time = 0.3f;
    private float wait_ = 0.0f;

    //�_�f��
    [SerializeField]
    private int oxy = 3;

    public bool menu_Flg
    {
        get { return menu_flg_; }
    }

    [HideInInspector]
    public enum menu_state_
    {
        Map,
        Oxygen,
        Escape,
        EscapeOK,
    }

    public menu_state_ state = menu_state_.Map;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (menu_flg_)
        {
            wait_ -= Time.deltaTime;
            if (wait_ <= 0)
            {
                Menu();

                MenuSelect();

                //���j���[�����
                if (Input.GetButtonDown("Fire2"))
                {
                    CloseMenu();
                }
            }
        }
    }
    private void SetUI()
    {
        switch (state)
        {
            case menu_state_.Map:
                {
                    UI_.submarine_escape.SetActive(false);
                    UI_.submaline_oxygen.SetActive(false);
                    UI_.submarine_map_.SetActive(true);

                    UI_.arrow_.anchoredPosition = new Vector2(484, -408);
                    break;
                }
            case menu_state_.Oxygen:
                {
                    UI_.submarine_map_.SetActive(false);
                    UI_.submarine_escape.SetActive(false);
                    UI_.submaline_oxygen.SetActive(true);

                    UI_.arrow_.anchoredPosition = new Vector2(484,-528);
                    break;

                }
            case menu_state_.Escape:
                {
                    UI_.submaline_oxygen.SetActive(false);
                    UI_.submarine_map_.SetActive(false);
                    UI_.submarine_escape.SetActive(true);

                    UI_.arrow_.anchoredPosition = new Vector2(484, -648);
                    break;
                }
        }
    }

    private void Menu()
    {
        float _stick_left_Vertical = Input.GetAxis("Vertical");

        //�X�e�B�b�N�����ɓ|�����
        if (_stick_left_Vertical < 0)
        {
            if (state == menu_state_.Map)
            {
                state = menu_state_.Oxygen;
                SetUI();
            }
            else if (state == menu_state_.Oxygen)
            {
                state = menu_state_.Escape;
                SetUI();
            }

            WaitTime();
        }
        //�X�e�B�b�N����ɓ|�����
        else if (_stick_left_Vertical > 0)
        {
            if (state == menu_state_.Oxygen)
            {
                state = menu_state_.Map;
                SetUI();
            }
            else if (state == menu_state_.Escape)
            {
                state = menu_state_.Oxygen;
                SetUI();
               
            }
            WaitTime();
        }
    }

    private void MenuSelect()
    {
        //���j���[��I��
        if (Input.GetButtonDown("Fire1"))
        {
            //�}�b�v���J��
            if (state == menu_state_.Map)
            {
                UI_.CloseUI();
                UI_.submarine_bigmap_.SetActive(true);
            }
            //�_�f��
            else if (state == menu_state_.Oxygen)
            {
                if (oxy > 0)
                {
                    script_player_.GetTank();
                    CloseMenu();
                }
                else
                {
                    //�񕜉\�񐔂̎c�肪0���͉������Ȃ�
                    CloseMenu();
                }
            }
            //�E�o
            else if (state == menu_state_.Escape)
            {
                UI_.CloseUI();
                //10�l�ȏ������Ă�����
                if (script_player_count_.fellow_rescue_ >= 10)
                {
                    UI_.escape_yes_no_UI_.SetActive(true);
                    state = menu_state_.EscapeOK;
                }
                //���Ă��Ȃ��ꍇ
                else
                {
                    CloseMenu();
                    StartCoroutine(Escape_No_UI());
                }
            }
            else if (state == menu_state_.EscapeOK)
            {
                UI_.CloseUI();
                GameProgress.instance_.GameClear();
            }


            WaitTime();
        }
    }

    private void WaitTime()
    {
        wait_ = wait_time;
    }

    //���j���[���J������
    public void OpenMenu()
    {
        script_player_.OpenMenu();

        WaitTime();
        UI_.submarine_UI_.SetActive(true);
        UI_.submarine_arrow_.SetActive(true);
        SetUI();
        menu_flg_ = true;
    }
    
    //���j���[�̏I������
    public void CloseMenu()
    {
        UI_.CloseUI();
        script_player_.CloseMenu();
        menu_flg_ = false;
        state = menu_state_.Map;
        wait_ = 0;
        
    }

    IEnumerator Escape_No_UI()
    {
        Debug.Log("����ĂȂ�");
        UI_.escape_no_UI_.SetActive(true);
        yield return new WaitForSeconds(1);
        UI_.escape_no_UI_.SetActive(true);
        //5�b�҂�
        yield return new WaitForSeconds(5);
        UI_.escape_no_UI_.SetActive(false);
    }
}
