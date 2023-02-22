using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerStay : MonoBehaviour
{

    [Header("PlayerNX")]
    [SerializeField]
    private Player script_player_ = null;
    [SerializeField]
    private UIManager UI_ = null;
    [SerializeField]
    private PlayerFellowCount script_player_count_ = null;
    [SerializeField]
    private PlayersSEPlayer sePlayer_ = null;
    [SerializeField]
    PlayerSubmarineMenu script_player_menu_ = null;

    private void Update()
    {
        if(UI_.submarine_a_.activeSelf)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                UI_.CloseUI();
                script_player_menu_.OpenMenu();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {

        //ΤΜ
        if (other.gameObject.CompareTag("RescueArea"))
        {
            UI_.fellow_ui_.SetActive(true);

            // fire1_range_flg_ = true;
            if (Input.GetButton("Fire1"))
            {
                UI_.fellow_ui_.SetActive(false);

                script_player_count_.fellow_count_ += 1;
                sePlayer_.PartGet();

                var _fellow = other;
                _fellow.GetComponent<RescueArea>().Follow();
                _fellow = null;

            }
        }
        //{xρ
        if (other.gameObject.CompareTag("Tank"))
        {
           
            UI_.item_ui_.SetActive(true);

            //fire1_range_flg_ = true;
            if (Input.GetButton("Fire1"))
            {
                script_player_.GetTank();
                var _tank = other;
                _tank.GetComponent<Tank>().Pickup();
                _tank = null;
            }
        }

        //φΝ
        if (other.gameObject.CompareTag("Submarine")&&!script_player_menu_.menu_Flg)
        {
          
            if (script_player_count_.fellow_count_ > 0)
            {
                script_player_.Rescue();
                UI_.recovery_UI_.SetActive(true);
            }
            else
            {
                UI_.recovery_UI_.SetActive(false);
                // UI_.CloseUI();
                UI_.submarine_a_.SetActive(true);
               
            }

        }

    }


    private void OnTriggerExit(Collider other)
    {
        //ACeΜΝΝ©ηo½
        if (other.gameObject.CompareTag("Tank"))
        {
            UI_.CloseUI();

        }

        if (other.gameObject.CompareTag("RescueArea"))
        {

            UI_.CloseUI();

        }

        //φΝ
        if (other.gameObject.CompareTag("Submarine"))
        {
            UI_.CloseUI();
        }
    }

  
}

