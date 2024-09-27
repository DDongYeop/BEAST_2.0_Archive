using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_TreasureSelect : Button_Menu
{
    public override void OnAction()
    {
        UIManager_Menu.Instance.HideScene("Panel_Menu");
        UIManager_Menu.Instance.ShowScene("Panel_TreasureSelect");
    }
}
