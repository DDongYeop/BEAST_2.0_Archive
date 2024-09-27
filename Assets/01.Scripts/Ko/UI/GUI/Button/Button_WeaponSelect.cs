using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_WeaponSelect : Button_Menu
{
    public override void OnAction()
    {
        UIManager_Menu.Instance.HideScene("Panel_Menu");
        UIManager_Menu.Instance.ShowScene("Panel_WeaponSelect");
        //UIManager_Menu.Instance.transform.Find("Panel_WeaponSelect").gameObject.SetActive(true);
    }
}
