using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossArrow : MonoBehaviour
{
    private Scene_InGame scene_InGame;
    private Camera mainCam;

    private void Awake()
    {
        scene_InGame = (UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame);
        mainCam = Camera.main;
    }

    private void Update()
    {
        Vector3 _viewPos = mainCam.WorldToScreenPoint(transform.position);

        if(_viewPos.x < 0)
        {
            scene_InGame.SetArrowPos(ArrowPos.Left);
        }
        else if(_viewPos.x > 1)
        {
            scene_InGame.SetArrowPos(ArrowPos.Right);
        }
        else
        {
            scene_InGame.SetArrowVisible(false);
        }
    }
    private void OnEnable()
    {
        scene_InGame.SetArrowVisible(true);
    }

    private void OnDisable()
    {
        scene_InGame.SetArrowVisible(false);
    }

    private void OnDestroy()
    {
        scene_InGame.SetArrowVisible(false);
    }
}
