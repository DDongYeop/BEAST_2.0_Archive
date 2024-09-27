using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


enum Pos
{
    TOP = 74,
    MID = 33,
    LOW  = 0,
}


public class Scene_WeaponSelect : UI_Scene, IDataObserver
{
    [SerializeField] private Vector2 _slotSize;
    [SerializeField] private GameObject _itemSlot;
    [SerializeField] private GameObject _selectedSlot;
    [SerializeField] private ThrownWeaponInfo[] _weaponsData;
    [SerializeField] private float _snapForce;

    private int _curShowWeaponIndex = -1;
    private List<ThrownWeaponInfo> _selectedWeapons = new List<ThrownWeaponInfo>();

    private bool _isMouseDown = false;
    private float _snapSpeed = 0;
    private ScrollRect _scrollRect;
    private RectTransform _contentPanel;
    private HorizontalLayoutGroup _horGroup;

    private bool _isSnapped = true;
    private float _shakedTime = 0;

    protected override void Init()
    {
        base.Init();
        SaveLoadManager.Instance.Init();

        Bind<HorizontalLayoutGroup>();

        _scrollRect = Get<ScrollRect>("ScrollRect_WeaponSlots");
        _horGroup = Get<HorizontalLayoutGroup>("Content");
        _contentPanel = _horGroup.gameObject.GetComponent<RectTransform>();

        BindEvent(Get<ScrollRect>("ScrollRect_WeaponSlots").gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = true; }, Define.ClickType.Down);
        //BindEvent(Get<ScrollRect>("ScrollRect_WeaponSlots").gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = true; }, Define.ClickType.Move);
        BindEvent(Get<ScrollRect>("ScrollRect_WeaponSlots").gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = false; }, Define.ClickType.Up);

        BindEvent(Get<Image>("Image_Close").gameObject, CloseButton);



        DestroySlot();
        BindSlot();

        StartCoroutine(scoll());


        foreach (Transform _child in Get<Image>("Image_SelectedWeapons").transform)
        {
            Destroy(_child.gameObject);
        }

        
        SaveLoadManager.Instance.LoadData();
        
        Invoke("LateInit", 0.05f);   
    }

    private void LateInit()
    {
        foreach (var _item in _selectedWeapons)
        {
            _contentPanel.transform.Find("Image_" + _item.WeaponId).Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(true);
        }

        foreach (var _item in _selectedWeapons)
        {
            BindSelectSlot(_item);
        }

        _contentPanel.localPosition = Vector3.zero;
        _curShowWeaponIndex = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_slotSize.x + _horGroup.spacing));

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

        Get<TextMeshProUGUI>("Text_WeaponInfo").text = $"{_weaponsData[_curShowWeaponIndex].WeaponName}\n<size=30>{_weaponsData[_curShowWeaponIndex].WeaponDescription}</size>";

    }

    IEnumerator  scoll()
    {
        yield return new WaitForSeconds(0.05f);
        foreach (var _item in _selectedWeapons)
        {
            _contentPanel.transform.Find("Image_" + _item.WeaponId).Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(true);
        }
    }

    private void CloseButton(PointerEventData data, Transform transform)
    {
        if (_selectedWeapons.Count != 3)
        {
            if(_shakedTime + 0.6f <= Time.time)
            {
                Handheld.Vibrate();
                Get<Image>("Image_SelectedWeapons").rectTransform.DOShakeAnchorPos(0.5f, 30, 45, 0);
                _shakedTime = Time.time;
            }
            return;
        }

        SaveLoadManager.Instance.SaveData();

        UIManager_Menu.Instance.ShowScene("Panel_Menu");
        UIManager_Menu.Instance.HideScene("Panel_WeaponSelect");
    }

    protected override void OnActive()
    {
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //SaveLoadManager.Instance.Init();

        
    }

    protected override void Start()
    {
       
    }

    private List<ThrownWeaponInfo> GetWeapons()
    {
        //List<ThrownWeaponInfo> _weapons = new List<ThrownWeaponInfo>();

        //foreach(var _weapon in _selectedWeapons)
        //{
        //    _weapons.Add(_weaponsData[_weapon]);
        //}

        return _selectedWeapons;
    }

    private void BindSlot()
    {
        for(int i = 0; i < _weaponsData.Length; i++)
        {
            GameObject _newSlot = Instantiate(_itemSlot, _contentPanel.transform);
            _newSlot.name = "Image_" + _weaponsData[i].WeaponId;
            _newSlot.GetComponent<RectTransform>().sizeDelta = _slotSize;


            Image _icon = _newSlot.transform.Find("Image_Fill").Find("Image_InSide").Find("Image_Icon").GetComponent<Image>();
            _icon.sprite = _weaponsData[i].WeaponSprite;
            _icon.color = UnityEngine.Color.white;
            _icon.preserveAspect = true;


            if(_newSlot.transform.Find("Image_Fill").Find("Text_Name").TryGetComponent(out TextMeshProUGUI _text))
            {
                _text.text = _weaponsData[i].WeaponName;
            }

            BindEvent(_newSlot, SelectSlot);
        }
    }

    private void SelectSlot(PointerEventData _data, Transform _transform)
    {
        string _slotName =_transform.name.Split('_')[1];
        //_curWeaponIndex == _index
        if (!_isSnapped || _scrollRect.velocity.magnitude != 0)
            return;
        Debug.Log(_slotName);
        bool _selected = false;

        if(_selectedWeapons != null)
        {
            foreach (var _weapon in _selectedWeapons)
            {
                if (_weapon.WeaponId == _slotName)
                {
                    _selected = true;
                    break;
                }
            }
        }
        

        //if (_curWeaponIndex != -1)
        //{
        //    SwipeUI(_itemSlot, _curWeaponIndex);
        //}


        if (_selected)
        {
            _transform.Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(false);
            var _removeItem = _selectedWeapons.Where(i => i.WeaponId == _slotName).ToArray();
            _selectedWeapons.Remove(_removeItem[0]);

            DestroySelectSlot(_slotName);
            return;
        }

        if(_selectedWeapons == null || _selectedWeapons.Count < 3)
        {
            var _newItem = _weaponsData.Where(i => i.WeaponId == _slotName).ToArray();
            Debug.Log(_newItem[0].WeaponId);    
            _selectedWeapons.Add(_newItem[0]);
            _transform.Find("Image_Fill").transform.Find("Image_Outline").gameObject.SetActive(true);

            BindSelectSlot(_newItem[0]);
        }

        //_curWeaponIndex = _index;
        //SwipeUI(_selectedSlot, _curWeaponIndex);
        SaveLoadManager.Instance.SaveData();

    }

    private void BindSelectSlot(ThrownWeaponInfo _data)
    {
        GameObject _newSlot = Instantiate(_selectedSlot, Get<Image>("Image_SelectedWeapons").transform);
        _newSlot.name = "Image_" + _data.WeaponId;

        if (_newSlot.transform.Find("Image_Icon").TryGetComponent(out Image _image))
        {
            _image.sprite = _data.WeaponSprite;
            _image.preserveAspect = true;
        }
    }

    private void DestroySelectSlot(string _weaponId)
    {
        Transform _container = Get<Image>("Image_SelectedWeapons").transform;

        GameObject _slot = _container.Find("Image_" + _weaponId).gameObject;

        if(_slot != null)
        {
            Destroy(_slot);
        }
    }

    private void DestroySlot()
    {
        foreach(Transform _child in _contentPanel.transform)
        {
            Destroy(_child.gameObject);
        }
    }

    private void RiseUpUI(int _index, Pos _pos, float _duration = 0.25f)
    {
        //Transform _curObj = _contentPanel.transform.Find("Image_" + _index.ToString());
        Transform _curObj = _contentPanel.transform.GetChild(_index);

        foreach(Transform _child in _curObj)
        {
            if (_child.GetComponent<RectTransform>() == null || _child == null)
                return;

            _child.DOLocalMoveY((int)_pos, _duration);
        }
        
        if(_curObj.TryGetComponent(out Image _image))
        {
            _image.raycastPadding = new Vector4(0, (int)_pos, 0, -(int)_pos);
        }
    }


    protected override void Update()
    {
        base.Update();

        //_scrollRect.onValueChanged
        if(_scrollRect.velocity.magnitude > 0)
        {
            _curShowWeaponIndex = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_slotSize.x + _horGroup.spacing));


            Get<TextMeshProUGUI>("Text_WeaponInfo").text = $"{_weaponsData[_curShowWeaponIndex].WeaponName}\n\n<size=20>{_weaponsData[_curShowWeaponIndex].WeaponDescription}</size>";

            foreach (Transform _child in _contentPanel.transform)
            {
                if(_child.GetSiblingIndex() == _curShowWeaponIndex - 1)
                {
                    if(_child.GetSiblingIndex() >= 0)
                        RiseUpUI(_curShowWeaponIndex - 1, Pos.MID);
                }
                else if(_child.GetSiblingIndex() == _curShowWeaponIndex)
                {
                    RiseUpUI(_curShowWeaponIndex, Pos.TOP);
                }
                else if(_child.GetSiblingIndex() == _curShowWeaponIndex + 1)
                {
                    if(_child.GetSiblingIndex() < _contentPanel.childCount)
                        RiseUpUI(_curShowWeaponIndex + 1, Pos.MID);
                }
                else
                {
                    RiseUpUI(_child.GetSiblingIndex(), Pos.LOW);

                }
            }

        }

        if (_scrollRect.velocity.magnitude < 200 && !_isSnapped && !_isMouseDown)
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
        
        if(_scrollRect.velocity.magnitude > 200)
        {
            _isSnapped = false;
            _snapSpeed = 0;
        }
    }

    SkillInfo _curSkill;
    public void WriteData(ref SaveData data)
    {
        //data.weaponInfoList = GetWeapons();
        //data.SkillInfo = _curSkill;
    }

    public void ReadData(SaveData data)
    {
        //_curSkill = data?.SkillInfo;
        //var _newData = data?.weaponInfoList;

        //if(_newData != null)
        //{
        //    //_selectedWeapons = data.weaponInfoList;
        //}
    }
}