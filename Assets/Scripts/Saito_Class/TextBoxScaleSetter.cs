using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxScaleSetter : MonoBehaviour
{
    [SerializeField] Text txt_textbox_ = null;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform _rectT = txt_textbox_.GetComponent<RectTransform>();
        _rectT.anchoredPosition = new Vector2(700.296326f, -11.5821075f);
        _rectT.sizeDelta        = new Vector2(1134.90454f, -159.452805f);
    }
}