using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialofTank : MonoBehaviour
{
    public static List<TutorialofTank> tutorialofTanks_ = new List<TutorialofTank>();
        

    // Start is called before the first frame update
    void Start()
    {
        tutorialofTanks_.Add(this);
    }

    private void OnDisable()
    {
        if (GetComponent<TutorialPoint>())
        {
            GameObject.Find("Tutorial_Tank").GetComponent<TutorialPoint>().AutoTutorial();

            foreach (TutorialofTank _tank in tutorialofTanks_)
            {
                _tank.enabled = false;
            }
        }
    }
}
