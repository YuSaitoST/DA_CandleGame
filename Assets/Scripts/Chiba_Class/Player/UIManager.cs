using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("UI")]
    //（仲間を連れていない状態＆潜水艦の2m以内にいる）とき、
    public GameObject submarine_a_ = null;

    //上記条件下でAボタンを押したとき「submarine_UI」を（0,0）に表示。
    public GameObject submarine_UI_ = null;

    //十字キーorスティックでy座標を動かす{ 1つ目 : 408, 2つ目 : 528, 3つ目 : 648 }
    public GameObject submarine_arrow_ = null;
    [HideInInspector]
    public RectTransform arrow_ = null;

    // 「マップ表示(y : 408)」にあるとき、「submarine_map」を（902,330）に表示。...(A)
    public GameObject submarine_map_ = null;

    // 「マップ表示(y : 408)」にあるとき、「submarine_map」を（902,330）に表示。...(A)
    public GameObject submarine_bigmap_ = null;

    //「酸素を補充する(y : 528)」にあるとき、「submaline_oxygen」を（870,420）に表示。 ...(B)
    public GameObject submaline_oxygen = null;

    //「脱出する(y : 648)」にあるとき、「submarine_escape」を（959,518）に表示。 ...(C)
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
