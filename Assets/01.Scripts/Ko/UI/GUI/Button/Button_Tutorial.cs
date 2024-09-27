using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Tutorial : Button_Menu
{
    public override void OnAction()
    {
        //UIManager_Menu.Instance.HideScene("Panel_Menu");
        //UIManager_Menu.Instance.ShowScene("Panel_WeaponSelect");
        sceneManager.Instance.ChangeSceen("Tutorial", TransitionsEffect.circleWap);
    }
}
