using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Bear : Button_Menu
{
    public override void OnAction()
    {
        UIManager_Menu.Instance.ShowScene("Image_BearStage");
    }
}
