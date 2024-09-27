using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scene_LevelSelect : UI_Scene, IDataObserver
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _stageObj;
    [SerializeField] private float _snapForce;
    [SerializeField] private Vector2 _slotSize;

    private bool _isMouseDown = false;
    private bool _isSnapped = true;
    private float _snapSpeed = 0;
    private ScrollRect _scrollRect;
    private RectTransform _contentPanel;
    private HorizontalLayoutGroup _horGroup;

    private SaveData _saveData;

    private int _curIndex = -1;

    protected override void Init()
    {
        base.Init();

        if(TryGetComponent(out Image _image))
        {
            _image.color = new Color(0, 0, 0, 0);
            _image.DOColor(new Color(0, 0, 0, 0.7f), 0.4f);
        }

        Get<Image>("Image_LevelContainer").rectTransform.DOAnchorPosY(-500,0);
        Get<Image>("Image_LevelContainer").rectTransform.DOAnchorPosY(0, 0.5f);

        Bind<HorizontalLayoutGroup>();
        _scrollRect = Get<ScrollRect>("Scroll View");
        _horGroup = Get<HorizontalLayoutGroup>("Content");
        _contentPanel = _horGroup.gameObject.GetComponent<RectTransform>();

        _scrollRect.onValueChanged.AddListener(OnScrollMove);

        BindEvent(Get<ScrollRect>("Scroll View").gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = true; }, Define.ClickType.Down);
        BindEvent(Get<ScrollRect>("Scroll View").gameObject, (PointerEventData _data, Transform _transform) => { _isMouseDown = false; }, Define.ClickType.Up);

        BindEvent(Get<Image>("Image_CloseStage").gameObject, (PointerEventData _data, Transform _transform) => 
        {
            UIManager_Menu.Instance.HideScene("Image_Stages"); 
            UIManager_Menu.Instance.ShowScene("Panel_Menu"); 
        });
    }

    public void BindSlot(StageSO _data)
    {
        var _sceneNames = _data.GetSceneNames();

        foreach(Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < _sceneNames.Count; i++)
        {
            GameObject _newScene = Instantiate(_stageObj, _content);
            _newScene.name = "Image_" + _sceneNames[i];

            if (_newScene.TryGetComponent(out RectTransform _rect))
            {
                _rect.sizeDelta = _slotSize;
            }

            if (_newScene.TryGetComponent(out Button_Level _button))
            {
                _button.InitScene(_sceneNames[i]);

                //if (!_saveData.levels[_data.StartLevelIndex - 1 + i].Clear)
                //{
                //    _button.Lock();
                //}
            }
        }
    }

    private void OnScrollMove(Vector2 _vec)
    {
        _curIndex = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_slotSize.x + _horGroup.spacing));


        if (_scrollRect.velocity.magnitude < 200 && !_isSnapped && !_isMouseDown)
        {
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;

            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (_curIndex * (_slotSize.x + _horGroup.spacing)), _snapSpeed),
                _contentPanel.localPosition.y,
                _contentPanel.localPosition.z);


            if (_contentPanel.localPosition.x == 0 - (_curIndex * (_slotSize.x + _horGroup.spacing)))
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

    public void WriteData(ref SaveData data)
    {
        
    }

    public void ReadData(SaveData data)
    {
        _saveData = data;
    }
}
