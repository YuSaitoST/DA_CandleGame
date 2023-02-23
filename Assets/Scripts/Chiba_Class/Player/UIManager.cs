using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("UI")]
    //�i���Ԃ�A��Ă��Ȃ���ԁ������͂�2m�ȓ��ɂ���j�Ƃ��A
    public GameObject submarine_a_ = null;

    //��L��������A�{�^�����������Ƃ��usubmarine_UI�v���i0,0�j�ɕ\���B
    public GameObject submarine_UI_ = null;

    //�\���L�[or�X�e�B�b�N��y���W�𓮂���{ 1�� : 408, 2�� : 528, 3�� : 648 }
    public GameObject submarine_arrow_ = null;
    [HideInInspector]
    public RectTransform arrow_ = null;

    // �u�}�b�v�\��(y : 408)�v�ɂ���Ƃ��A�usubmarine_map�v���i902,330�j�ɕ\���B...(A)
    public GameObject submarine_map_ = null;

    // �u�}�b�v�\��(y : 408)�v�ɂ���Ƃ��A�usubmarine_map�v���i902,330�j�ɕ\���B...(A)
    public GameObject submarine_bigmap_ = null;

    //�u�_�f���[����(y : 528)�v�ɂ���Ƃ��A�usubmaline_oxygen�v���i870,420�j�ɕ\���B ...(B)
    public GameObject submaline_oxygen = null;

    //�u�E�o����(y : 648)�v�ɂ���Ƃ��A�usubmarine_escape�v���i959,518�j�ɕ\���B ...(C)
    public GameObject submarine_escape = null;

    public GameObject item_ui_ = null;
    public GameObject fellow_ui_ = null;
    public GameObject recovery_UI_ = null;
    public GameObject escape_no_UI_ = null;
    public GameObject escape_yes_no_UI_ = null;

    public GameObject rescue_gauge_UI = null;

    [HideInInspector]
    public Slider rescue_gauge_ = null;
    // Start is called before the first frame update
    void Start()
    {
        arrow_ = submarine_arrow_.GetComponent<RectTransform>();
        rescue_gauge_ = rescue_gauge_UI.GetComponent<Slider>();

        CloseUI();
    }

    public void CloseUI()
    {
        recovery_UI_.SetActive(false);
        submarine_a_.SetActive(false);
        submarine_UI_.SetActive(false);
        submarine_arrow_.SetActive(false);
        submarine_map_.SetActive(false);
        submarine_bigmap_.SetActive(false);
        submaline_oxygen.SetActive(false);
        submarine_escape.SetActive(false);
        item_ui_.SetActive(false);
        fellow_ui_.SetActive(false);
        escape_no_UI_.SetActive(false);
        escape_yes_no_UI_.SetActive(false);
        rescue_gauge_UI.SetActive(false);
    }
}
