using System.Collections;
using System.Collections.Generic;
using Util;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class Button_Menu : UI_Base
{
    protected bool _enable = true;

    protected override void Init()
    {
        base.Init();

        Bind<Image>();
        Bind<TextMeshProUGUI>();
    }
    
    public void HideUI(float _fadeDuration = 0.5f)
    {
        var images = m_objects[typeof(Image)];
        var texts = m_objects[typeof(TextMeshProUGUI)];

        foreach(var _item in images )
        {
            Image _image = _item.Value as Image;
            _image.DOFade(0, _fadeDuration);
        }

        foreach(var _item in texts)
        {
            TextMeshProUGUI _text = _item.Value as TextMeshProUGUI;
            _text.DOFade(0, _fadeDuration);
        }
    }

    public void ShowUI(float _fadeDuration = 0.3f)
    {
        GetComponent<Image>().raycastTarget = true;

        Debug.Log(gameObject.name + "SHOW!");
        var images = m_objects?[typeof(Image)];
        var texts = m_objects?[typeof(TextMeshProUGUI)];

        while (images == null && texts == null)
        {
            images = m_objects[typeof(Image)];
            texts = m_objects?[typeof(TextMeshProUGUI)];
        }

        foreach (var _item in images)
        {
            Image _image = _item.Value as Image;
            _image.DOFade(1, _fadeDuration);
        }

        foreach (var _item in texts)
        {
            TextMeshProUGUI _text = _item.Value as TextMeshProUGUI;
            _text.DOFade(1, _fadeDuration);
        }
    }

    public void ButtonEvent(float _fadeDuration = 0.5f)
    {
        if (!_enable)
            return;

        GetComponent<Image>().raycastTarget = false;
        CancelInvoke("OnAction");
        Invoke("OnAction", _fadeDuration);
    }

    public abstract void OnAction();
}
