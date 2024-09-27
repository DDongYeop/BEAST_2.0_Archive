using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.VisualScripting;

public enum ArrowPos
{
    Left,
    Right
}
public class Scene_InGame : UI_Scene, IDataObserver
{
    private SkillInfo _skillInfo;
    private List<ThrownWeaponInfo> _weaponInfos = new List<ThrownWeaponInfo>();
    private ThrownWeaponInfo _currentItem = null;
    private Image _currentSlotUI = null;

    [SerializeField] private WeaponController weaponController;  
    [SerializeField] private Image Arrow;
    protected override void Init()
    {
        base.Init();
        Arrow.gameObject.SetActive(false);
        ShowInteraction(false);
        BindEvent(Get<Image>("Image_Interaction").gameObject, (_data, _transform) => {
            UIManager_InGame.Instance.ShowScene("Scene_TechTree", true);
        });
        //Get<Button>("AAA").onClick.AddListener(InitItem);
    }

    protected override void Start()
    {
        SaveLoadManager.Instance.LoadData();

        weaponController = GameManager.Instance.PlayerTrm.Find("WeaponController").GetComponent<WeaponController>();

        //Get<Image>("Image_TreasureIcon").sprite = _skillInfo.SkillSprite;
        //Get<Image>("Image_TreasureIcon").sprite = _skillInfo.Where(i => i.SkillType == ).
        //InitItem();
    }

    //public void SetTreasureUI(string _name)
    //{
    //    var _newSprite = (from item in _skillInfo where item.SkillId == _name select item.SkillSprite);
    //    Get<Image>("Image_TreasureIcon").sprite = _newSprite.First();
    //}
    
    private void InitItem()
    {

        weaponController.AttemptChangeSKillData(_skillInfo.SkillType);

        //for (int i = 0; i < 3; i++)
        //{
        //    BindEvent(Get<Image>($"Image_ItemSelector{i + 1}").gameObject, OnSelectItem, Define.ClickType.Click);

        //    Image _image = Get<Image>($"Image_ItemIcon{i + 1}");
        //    //_image.rectTransform.anchoredPosition = _weaponInfos[i - 1].SpritePivotPosition;
        //    _image.sprite = _weaponInfos[i].WeaponSprite;
        //    _image.preserveAspect = true;
        //}

        weaponController.AttemptChangeWeaponStat("Sword");
    }

    public void OnThrow(int count, int max)
    {
        //Get<TextMeshProUGUI>("Text_TreasureCount").text =  $"{count}/{max}";
    }

    public void SetChain(bool _enable) 
    {
        _currentSlotUI.transform.Find("Image_Backgorund").Find("Image_Chain").gameObject.SetActive(_enable);
    }

    private void OnSelectItem(PointerEventData _data, Transform _transform)
    {
        var itemIndex = _transform.name.Split('r');
        Debug.Log($"{itemIndex[1]} is Cliked");

        var selector = Get<Image>($"Image_ItemSelector{int.Parse(itemIndex[1])}");

        selector.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            selector.transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(Ease.InSine);
        });

        weaponController.AttemptChangeWeaponStat(_weaponInfos[int.Parse(itemIndex[1]) - 1].WeaponId);


        if(_currentItem != _weaponInfos[int.Parse(itemIndex[1]) - 1])
        {
            StopAllCoroutines();
            //StartCoroutine(ItemPopup(_weaponInfos[int.Parse(itemIndex[1]) - 1]));
        }


        if(_currentSlotUI != null)
        {
            if(_currentSlotUI.transform.Find("Image_Popup").TryGetComponent(out RectTransform _rect))
            {
                if(_rect.anchoredPosition.y != 0)
                {
                    _rect.DOAnchorPosY(0, 0.5f);
                }
            }
        }

        if (weaponController.CurrentWeaponStat.MaxThrowCount != 0)
        {
            if(selector.transform.Find("Image_Popup").TryGetComponent(out RectTransform _rect))
            {
                if(_rect.transform.Find("Text_Popup").TryGetComponent(out TextMeshProUGUI _text))
                {
                    _text.text = $"{weaponController.CurrentWeaponStat.CurrentThrowCount}/{weaponController.CurrentWeaponStat.MaxThrowCount}";
                }

                _rect.DOAnchorPosY(13, 0.5f);
            }
        }

        _currentItem = _weaponInfos[int.Parse(itemIndex[1]) - 1];
        _currentSlotUI = selector;
    }

    public void OnUtillWeaponThrowed(int _current, int _max)
    {
        if(_currentSlotUI.transform.Find("Image_Popup").Find("Text_Popup").TryGetComponent(out TextMeshProUGUI _text))
        {
            _text.text = $"{_current}/{_max}";
        }
    }

    public void WavePopup(int wave)
    {
        StartCoroutine(ItemPopup(null, $"{wave}\n¿þÀÌºê"));
    }

    private IEnumerator ItemPopup(ThrownWeaponInfo _data = null, string _text = null)
    {
        GameObject _itemPanel = Get<Image>("Image_CurrentItem").gameObject;

        //if(_itemPanel.transform.Find("Image_ItemIcon").TryGetComponent<Image>(out Image _image))
        //{
        //    _image.sprite = _data?.WeaponSprite ?? _currentItem.WeaponSprite;
        //    _image.preserveAspect = true;
        //}

        _itemPanel.transform.Find("Text_ItemName").GetComponent<TextMeshProUGUI>().text = (string.IsNullOrEmpty(_text) ? _data?.WeaponName : _text);

        _itemPanel.GetComponent<RectTransform>().DOAnchorPosY(-48, 0.6f).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(1.75f);
        _itemPanel.GetComponent<RectTransform>().DOAnchorPosY(100, 0.4f);
    }

    public void OnEnemyHPChanged(float percent)
    {
        DOTween.To(() => Get<Slider>("Slider_EnmyHP").value, NewValue => Get<Slider>("Slider_EnmyHP").value = NewValue, percent, 0.2f);
    }

    public void OnMoneyChanged(int amount)
    {
        DOTween.To(() => int.Parse(Get<TextMeshProUGUI>("Text_Money").text), NewValue => Get<TextMeshProUGUI>("Text_Money").text = NewValue.ToString(), amount, 0.2f);
    }

    private IEnumerator LearpRectPositionRoutine(RectTransform _obj, Vector3 _startPos, Vector3 _endPos)
    {
        float _timeStartedLerping = 0;

        while (true)
        {
            _timeStartedLerping += Time.deltaTime;

            _obj.transform.localPosition = Vector3.Lerp(_startPos, _endPos, _timeStartedLerping / 1);

            if (_timeStartedLerping >= 1.0f)
            {
                break;
            }
        }
        yield return null;
    }

    public void ApplyCombo(int _combo)
    {
        TextMeshProUGUI _text = Get<TextMeshProUGUI>("Text_Combo");
        if (_combo - 1 <= 1)
        {
            _text.DOFade(0, 0.3f);
            return;
        }

        if(_text.color != Color.white)
        {
            _text.DOFade(1, 0.3f);
        }

        _text.text = $"{_combo - 1}\n<size=20>COMBO</size>";
        //_text.rectTransform.DOShakeScale(0.5f, 0.4f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="right">ture = right, false = left</param>
    public void SetJoyStickPos(bool right)
    {

        if (Get<Image>("Image_MoveButtons").TryGetComponent(out RectTransform _rect))
        {
            _rect.anchorMin = new Vector2(Convert.ToInt32(!right), 0);
            _rect.anchorMax = new Vector2(Convert.ToInt32(!right), 0);
            _rect.anchoredPosition = new Vector2(right ? _rect.sizeDelta.x / 2 : -(_rect.sizeDelta.x / 2), _rect.sizeDelta.y / 2);
        }

        if (Get<Image>("Image_FixedJoystick").TryGetComponent(out RectTransform _joystick))
        {
            _joystick.anchorMin = new Vector2(Convert.ToInt32(right), 0);
            _joystick.anchorMax = new Vector2(Convert.ToInt32(right), 0);
            _joystick.anchoredPosition = new Vector2(right ? -124 : 124, 124);
        }
    }
  
    public void SetArrowPos(ArrowPos pos)
    {
        if(Arrow.TryGetComponent(out RectTransform _rect))
        {
            _rect.anchorMin = new Vector2((int)pos, 0.6f);
            _rect.anchorMax = new Vector2((int)pos, 0.6f);

            _rect.anchoredPosition = new Vector2(pos == ArrowPos.Left ? _rect.sizeDelta.x / 2 : -(_rect.sizeDelta.x / 2), 0);
            _rect.rotation = new Quaternion(0, pos == ArrowPos.Left ? 180 : 0, 0, 0);
        }
    }

    public void SetArrowVisible(bool visible)
    {
        Arrow.gameObject.SetActive(visible);
    }

    [ContextMenu("RightJoyStick")]
    public void SetRightJoyStick()
    {
        SetJoyStickPos(true);
    }

    [ContextMenu("LeftJoyStick")]
    public void SetLeftJoyStick()
    {
        SetJoyStickPos(false);
    }

    public void ShowInteraction(bool show, string text = null)
	{
        Get<Image>("Image_Interaction").gameObject.SetActive(show);

        if (!string.IsNullOrEmpty(text))
            Get<TextMeshProUGUI>("Text_Interaction").text = text;
	}

    public void WriteData(ref SaveData data)
    {
        
    }

    public void ReadData(SaveData data)
    {
        //_weaponInfos = data.weaponInfoList;
        //_skillInfo = data.SkillInfo;
    }
}
