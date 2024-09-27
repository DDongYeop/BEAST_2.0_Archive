using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : PoolableMono
{
    private TextMeshPro _textMesh;
    private Camera _cam;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _cam = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetUp(string text, Vector3 pos, float fontSize = 25f, Color color = default)
    {
        pos.z = -152;
        transform.position = pos;
        _rectTransform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, - 152);
        //_rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y, -152);
        //transform.position = new Vector3(transform.position.x, transform.position.y, -152);
        //_rectTransform.transform.position = _cam.WorldToScreenPoint(pos);
        _textMesh.SetText(text);
        //_textMesh.color = (color == default ? Color.red : color);
        _textMesh.fontSize = fontSize;
        _textMesh.color = color;

        ShowingSequence(2f);
    }

    //public void Update()
    //{
    //    transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    //}

    public void ShowingSequence(float time)
    {
        Sequence seq = DOTween.Sequence();
        //transform.position = new Vector3(transform.position.x, transform.position.y, -152);

        seq.Append(_rectTransform.transform.DOMoveY(_rectTransform.transform.position.y + 6.5f, time));
        seq.Join(_textMesh.DOFade(0, 3f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }
    public override void Init()
    {
        _textMesh.alpha = 1f;
        transform.SetParent(UIManager_InGame.Instance.transform);
    }

}
