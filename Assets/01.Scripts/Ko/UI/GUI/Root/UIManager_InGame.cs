using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_InGame : UI_Root
{
    public static UIManager_InGame Instance = null;

    protected override void Awake()
    {
        base.Awake();

        if(Instance == null)
            Instance = this;
        else
            Destroy(this);

        //ShowScene("Scene_InGame");
    }
}
