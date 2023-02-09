using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialofTank : MonoBehaviour
{
    static bool isTutorialClear;

    // Start is called before the first frame update
    void Start()
    {
        isTutorialClear = false;
    }

    private void OnDisable()
    {
        if(!isTutorialClear)
        {
            isTutorialClear = true;
            GameObject.Find("Tutorial_Tank").GetComponent<TutorialPoint>().AutoTutorial();
        }
    }
}
