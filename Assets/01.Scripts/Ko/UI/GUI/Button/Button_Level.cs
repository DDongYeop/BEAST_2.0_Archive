using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Level : Button_Menu
{
    private string _SceneName;

    protected override void Init()
    {
        base.Init();

        GetComponent<Image>().raycastTarget = true;
        BindEvent(gameObject, (PointerEventData _data, Transform _transform) => { OnAction(); });
    }

    public void InitScene(string _scene)
    {
        _SceneName = _scene;

        Get<TextMeshProUGUI>("Text_Level").text = _SceneName.Split('.')[0];
    } 

    public override void OnAction()
    {
        GetComponent<Image>().raycastTarget = false;

        sceneManager.Instance.ChangeSceen(_SceneName, TransitionsEffect.circleWap);
    }

    public void Lock()
    {
        if(transform.Find("Text_Level").TryGetComponent(out TextMeshProUGUI _text))
        {
            _text.color = Color.black;
        }

        Destroy(GetComponent<UI_EventHandler>());
    }
}
