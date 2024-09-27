using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scene_TreasureSelect : UI_Scene, IDataObserver
{
    [SerializeField] private SkillInfo[] _SkillData;
    [SerializeField] private GameObject _itemSlot;
    [SerializeField] private Vector2 _slotSize;
    [SerializeField] private float _snapForce;

    private bool _isMouseDown = false;
    private float _snapSpeed = 0;
    private bool _isSnapped = true;
    private int _curShowWeaponIndex = -1;

    private HorizontalLayoutGroup _horGroup;
    private ScrollRect _scrollRect;
    private RectTransform _contentPanel;


    private SkillInfo _curSkill = null;

    protected override void Init()
    {
        base.Init();

        BindEvent(Get<Image>("Image_Close").gameObject, CloseButton);


        Bind<HorizontalLayoutGroup>();

        _scrollRect = Get<ScrollRect>("ScrollRect_TreasureSlots");
        _horGroup = Get<HorizontalLayoutGroup>("Treasure_Content");
        _contentPanel = _horGroup.gameObject.GetComponent<RectTransform>();


        _scrollRect.onValueChanged.AddListener(OnScrollMove);

        BindEvent(_scrollRect.gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = true; }, Define.ClickType.Down);
        //BindEvent(_scrollRect.gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = true; }, Define.ClickType.Move);
        BindEvent(_scrollRect.gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = false; }, Define.ClickType.Up);

        DestroySlot();
        BindSlot();
        
        SaveLoadManager.Instance.LoadData();


        Invoke("LateInit", 0.02f);
    }

    private void LateInit()
    {
        if (_curSkill != null)
        {
            _horGroup.transform.Find($"Image_{_curSkill.SkillId}").transform.Find("Image_Fill").Find("Image_Outline").gameObject.SetActive(true);
        }

        _contentPanel.localPosition = Vector3.zero;
        _curShowWeaponIndex = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_slotSize.x + _horGroup.spacing));
        Get<TextMeshProUGUI>("Text_WeaponInfo").text = $"{_SkillData[_curShowWeaponIndex].SkillName}\n\n<size=20>{_SkillData[_curShowWeaponIndex].SkillDescription}</size>\n<size=20>{_SkillData[_curShowWeaponIndex].CountForRecharge}</size>";


        foreach (Transform _child in _contentPanel.transform)
        {
            if (_child.GetSiblingIndex() == _curShowWeaponIndex - 1)
            {
                if (_child.GetSiblingIndex() >= 0)
                    RiseUpUI(_curShowWeaponIndex - 1, Pos.MID);
            }
            else if (_child.GetSiblingIndex() == _curShowWeaponIndex)
            {
                RiseUpUI(_curShowWeaponIndex, Pos.TOP);
            }
            else if (_child.GetSiblingIndex() == _curShowWeaponIndex + 1)
            {
                if (_child.GetSiblingIndex() < _contentPanel.childCount)
                    RiseUpUI(_curShowWeaponIndex + 1, Pos.MID);
            }
            else
            {
                RiseUpUI(_child.GetSiblingIndex(), Pos.LOW);

            }
        }
    }

    private void DestroySlot()
    {
        foreach (Transform _child in _contentPanel.transform)
        {
            Destroy(_child.gameObject);
        }
    }

    private void CloseButton(PointerEventData data, Transform transform)
    {
        if(_curSkill == null)
        {
            Vector2 _vec = _horGroup.GetComponent<RectTransform>().anchoredPosition;
            Handheld.Vibrate();
            _horGroup.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 30, 45, 0).OnComplete(() =>
            {
                _horGroup.GetComponent<RectTransform>().DOAnchorPos(_vec, 0.2f);
            });
            return;
        }

        //SaveLoadManager.Instance.SaveData();

        UIManager_Menu.Instance.ShowScene("Panel_Menu");
        UIManager_Menu.Instance.HideScene("Panel_TreasureSelect");
    }

    private void BindSlot()
    {
        for (int i = 0; i < _SkillData.Length; i++)
        {
            GameObject _newSlot = Instantiate(_itemSlot, _contentPanel.transform);
            _newSlot.name = "Image_" + _SkillData[i].SkillId;
            _newSlot.GetComponent<RectTransform>().sizeDelta = _slotSize;


            Image _icon = _newSlot.transform.Find("Image_Fill").Find("Image_InSide").Find("Image_Icon").GetComponent<Image>();
            _icon.sprite = _SkillData[i].SkillSprite;
            _icon.color = Color.white;
            _icon.preserveAspect = true;

            if (!_SkillData[i].IsActive)
            {
                _newSlot.transform.Find("Image_Fill").Find("Image_InSide").Find("Image_Chain").gameObject.SetActive(true);
            }

            if (_newSlot.transform.Find("Image_Fill").Find("Text_Name").TryGetComponent(out TextMeshProUGUI _text))
            {
                _text.text = _SkillData[i].SkillId;
            }

            BindEvent(_newSlot, SelectSlot);
        }
    }

    private void SelectSlot(PointerEventData _data, Transform _transform)
    {
        string _slotName = _transform.name.Split('_')[1];

        //if (!_isSnapped)
        //    return;

        SkillInfo _newSkill = _SkillData.Where(i => i.SkillId == _slotName).FirstOrDefault();

        if (!_newSkill.IsActive)
            return;

        if (_curSkill != null)
        {
            _transform.Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(true);
            _contentPanel.Find($"Image_{_curSkill.SkillId}").transform.Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(false);

            if(_curSkill.SkillId == _slotName)
            {
                _curSkill = null;
            }
            else
            {
                _curSkill = _newSkill;
            }

            //DestroySelectSlot(_slotName);
        }
        else
        {
            _transform.Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(true);
            _curSkill = _newSkill;
        }
        SaveLoadManager.Instance.SaveData();

        //_curWeaponIndex = _index;
        //SwipeUI(_selectedSlot, _curWeaponIndex);
    }

    private void RiseUpUI(int _index, Pos _pos, float _duration = 0.25f)
    {
        //Transform _curObj = _contentPanel.transform.Find("Image_" + _index.ToString());
        Transform _curObj = _contentPanel.transform.GetChild(_index);

        foreach (Transform _child in _curObj)
        {
            if (_child.GetComponent<RectTransform>() == null || _child == null)
                return;

            _child.DOLocalMoveY((int)_pos, _duration);
        }

        if (_curObj.TryGetComponent(out Image _image))
        {
            _image.raycastPadding = new Vector4(0, (int)_pos, 0, -(int)_pos);
        }
    }

    private void OnScrollMove(Vector2 _vec)
    {
        if (_scrollRect.velocity.magnitude > 0)
        {
            _curShowWeaponIndex = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_slotSize.x + _horGroup.spacing));
            _curShowWeaponIndex = Mathf.Clamp(_curShowWeaponIndex, 0, _SkillData.Length - 1);

            Get<TextMeshProUGUI>("Text_WeaponInfo").text = $"{_SkillData[_curShowWeaponIndex].SkillName}\n\n<size=20>{_SkillData[_curShowWeaponIndex].SkillDescription}</size>\n<size=20>{_SkillData[_curShowWeaponIndex].CountForRecharge}</size>";


            foreach (Transform _child in _contentPanel.transform)
            {
                if (_child.GetSiblingIndex() == _curShowWeaponIndex - 1)
                {
                    if (_child.GetSiblingIndex() >= 0)
                        RiseUpUI(_curShowWeaponIndex - 1, Pos.MID);
                }
                else if (_child.GetSiblingIndex() == _curShowWeaponIndex)
                {
                    RiseUpUI(_curShowWeaponIndex, Pos.TOP);
                }
                else if (_child.GetSiblingIndex() == _curShowWeaponIndex + 1)
                {
                    if (_child.GetSiblingIndex() < _contentPanel.childCount)
                        RiseUpUI(_curShowWeaponIndex + 1, Pos.MID);
                }
                else
                {
                    RiseUpUI(_child.GetSiblingIndex(), Pos.LOW);

                }
            }

        }

        if (_scrollRect.velocity.magnitude < 200 && !_isSnapped)
        {
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;

            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (_curShowWeaponIndex * (_slotSize.x + _horGroup.spacing)), _snapSpeed),
                _contentPanel.localPosition.y,
                _contentPanel.localPosition.z);


            if (_contentPanel.localPosition.x == 0 - (_curShowWeaponIndex * (_slotSize.x + _horGroup.spacing)))
            {
                _isSnapped = true;
            }
        }

        if (_scrollRect.velocity.magnitude > 200)
        {
            _isSnapped = false;
            _snapSpeed = 0;
        }
    }

    List<ThrownWeaponInfo> _curWeapons;
    public void WriteData(ref SaveData data)
    {
        //data.SkillInfo = _curSkill;
       // data.weaponInfoList = _curWeapons;
    }

    public void ReadData(SaveData data)
    {
        //_curSkill = data?.SkillInfo;
        //_curWeapons = data?.weaponInfoList;
    }
}
