using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button_Scene : Button_Menu
{
    [SerializeField] private string _sceneName;

    public override void OnAction()
    {
        string[] str = _sceneName.Split('.');
        if (PlayerPrefs.HasKey(str[1]))
        {
            _sceneName = PlayerPrefs.GetInt(str[1]).ToString() + '.' + str[1];
        }

        sceneManager.Instance.ChangeSceen(_sceneName, TransitionsEffect.circleWap);
    }

    [ContextMenu("ResetStage")]
    private void ResetStage()
    {
        string[] str = _sceneName.Split('.');
        _sceneName = "1." + str[1];

        PlayerPrefs.SetInt(str[1], 1);
    }
}
