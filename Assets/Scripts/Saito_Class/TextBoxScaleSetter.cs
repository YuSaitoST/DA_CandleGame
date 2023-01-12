using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxScaleSetter : MonoBehaviour
{
    [SerializeField] RectTransform txt_textbox_ = null;
    [SerializeField] RectTransform img_chara_ = null;

    // Start is called before the first frame update
    void Start()
    {
        //txt_textbox_.anchoredPosition = new Vector2(703.200012f, 189.638794f);
        SetScale();

    }

    public void SetScale()
    {
        txt_textbox_.position = new Vector3(97.0f, -27.6f, 0.0f);
        txt_textbox_.sizeDelta = new Vector2(1207.8f, -122.6687f);

        img_chara_.position = new Vector3(-65.0f, 572.8f, 0.0f);
        img_chara_.sizeDelta = new Vector2(373.0f, 424.0f);
    }
}