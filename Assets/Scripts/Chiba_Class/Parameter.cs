using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Parameter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI sText_;

    [SerializeField]
    TextMeshProUGUI vText_;

    [SerializeField]
    TextMeshProUGUI hText_;

    [SerializeField]
    bool canvasFlg_ = false;

    [SerializeField]
    private int maxPoint_ = 5;


    [SerializeField]
    private int point_ = 2;

    public int  speed_=0;
    public int  view_ =1;
    public int  hear_ =0;
    // Start is called before the first frame update
    void Start()
    {
        //sText_.text = speed_.ToString("f2");
        //vText_.text = view_.ToString("f2");
        //hText_.text = hear_.ToString("f2");
        sText_.text = (speed_+1).ToString("f0");
        vText_.text = (view_ + 1).ToString("f0");
        hText_.text = (hear_ + 1).ToString("f0");
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void sPlus()
    {
        if (speed_<3&&point_>0)
        {
            speed_ += 1;
            point_ -= 1;
        }
        Debug.Log("b");
        sText_.text = (speed_ + 1).ToString("f0");
    }

    public void sMinus()
    {
        if (speed_ > 0)
        {
            speed_ -= 1;
            point_ += 1;
        }
        Debug.Log("a");
        sText_.text = (speed_ + 1).ToString("f0");
    }

    public void vPlus()
    {
        if (view_ < 3 && point_ > 0)
        {
            view_ += 1;
            point_ -= 1;
        }
        Debug.Log("b");
        vText_.text = (view_ + 1).ToString("f0");
    }

    public void vMinus()
    {
        if (view_ > 0)
        {
            view_ -= 1;
            point_ += 1;
        }
        Debug.Log("a");
        vText_.text = (view_ + 1).ToString("f0");
    }

    public void hPlus()
    {
        if (hear_ < 3 && point_ > 0)
        {
            hear_ += 1;
            point_ -= 1;
        }
        Debug.Log("b");
        hText_.text = (hear_ + 1).ToString("f0");
    }

    public void hMinus()
    {
        if (hear_ > 0)
        {
            hear_ -= 1;
            point_ += 1;
        }
        Debug.Log("a");
        hText_.text = (hear_ + 1).ToString("f0");
    }
}
