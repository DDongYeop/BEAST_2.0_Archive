using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Start : UI_Base
{
    [SerializeField] private GameObject _uiObjects;
    protected override void Awake()
    {
        base.Awake();

        Camera _cam = Camera.main;
        _cam.cullingMask = _cam.cullingMask & ~(1 << LayerMask.NameToLayer("Enemy"));
        _cam.cullingMask = _cam.cullingMask & ~(1 << LayerMask.NameToLayer("Totem"));
        _cam.cullingMask = _cam.cullingMask & ~(1 << LayerMask.NameToLayer("Player"));
        _cam.cullingMask = _cam.cullingMask & ~(1 << LayerMask.NameToLayer("UI"));
    }

    protected override void Start()
    {
        base.Start();

        TimeManager.Instance.IsPause = true;
        _uiObjects.gameObject.SetActive(false);
    }
    protected override void Init()
    {
        base.Init();


        Bind<Image>();
        Bind<TextMeshProUGUI>();
        Bind<Button>();
        //Get<TextMeshProUGUI>("Text_TaptoPlay").DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);
        //BindEvent(Get<Image>("Image_TouchInput").gameObject, (PointerEventData _data, Transform _transform) =>
        //{
        //    Get<Image>("Image_TouchInput").gameObject.SetActive(false);
        //    StartCoroutine(StartRoutine());
        //});

        //BindEvent(Get<Image>("Image_Pause").gameObject, (PointerEventData _data, Transform _transform) => { OnGamePuase(); });
        //Get<Button>("Button_Option").onClick.AddListener(() => OnGamePuase());
        Get<Button>("Button_Play").onClick.AddListener(() => 
        {
            Get<Button>("Button_Play").onClick.RemoveAllListeners();
            StartCoroutine(StartRoutine());
        });
    }

    public void Intro()
    {
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        Image FadeOutImage = Get<Image>("Image_FadeOut");
        FadeOutImage.raycastTarget = true;
        GameObject.Find("01.Bear").GetComponent<Animator>().SetBool("IsAttack01", true);

        yield return new WaitForSeconds(1.2f);

        FadeOutImage.DOFade(1, 1f);


        yield return new WaitForSeconds(1f);

        Destroy(transform.Find("01.Bear").gameObject);
        Destroy(transform.Find("Player(UI)").gameObject);

        Get<Image>("Image_Title").gameObject.SetActive(false);
        Get<Button>("Button_Play").gameObject.SetActive(false);
        Camera _cam = Camera.main;
        _cam.cullingMask |= 1 << LayerMask.NameToLayer("UI");
        _cam.cullingMask |= 1 << LayerMask.NameToLayer("Enemy");
        _cam.cullingMask |= 1 << LayerMask.NameToLayer("Player");
        _cam.cullingMask |= 1 << LayerMask.NameToLayer("Totem");
        _uiObjects.gameObject.SetActive(true);


        FadeOutImage.DOFade(0, 0.5f).OnComplete(() =>
        {
            TimeManager.Instance.IsPause = false;
            Destroy(gameObject);
        });
        //sceneManager.Instance.ChangeSceen("NewGame", TransitionsEffect.circleWap);

        //Get<Image>("Image_Title").gameObject.SetActive(false);
        //Get<Image>("Text_TaptoPlay").gameObject.SetActive(false);
        //Get<Image>("Image_Pause").gameObject.SetActive(false);
        //GameObject.Find("01.Bear").SetActive(false);
        //GameObject.Find("Player").SetActive(false);

        //FadeOutImage.DOFade(0, 0.5f).OnComplete(() =>
        //{
        //    SceneManager.LoadScene("2.5d Game Scene");
        //    gameObject.SetActive(false);
        //});

        //if (SaveLoadManager.Instance.data.levels[0].Clear)
        //    sceneManager.Instance.ChangeSceen("Menu 2", TransitionsEffect.fade);
        //else
        //{
        //    SaveLoadManager.Instance.data.levels[0].Clear = true;
        //    SaveLoadManager.Instance.SaveData();
        //    sceneManager.Instance.ChangeSceen("Tutorial", TransitionsEffect.fade);
        //}
    }

    private void OnGamePuase()
    {
        StartCoroutine(GamePauseRoutine());
    }

    private IEnumerator GamePauseRoutine()
    {
       // Get<Image>("Image_Pause").raycastTarget = false;
        Get<Image>("Image_EndPanel").rectTransform.DOAnchorPosY(0, 0.2f);
        Image _fade = Get<Image>("Image_Fade");
        _fade.raycastTarget = true;
        _fade.DOFade(0.7f, 0.2f);

        BindEvent(Get<Image>("Image_Play").gameObject, (PointerEventData _data, Transform _transform) =>
        {
            Get<Image>("Image_EndPanel").rectTransform.DOAnchorPosY(-600, 0.2f);
            //Get<Image>("Image_Pause").raycastTarget = true;
            _fade.DOFade(0, 0.2f);
            _fade.raycastTarget = false;
        });

        Toggle _toggle = Get<Toggle>("Toggle_OperationMode");

        if (PlayerPrefs.HasKey("OperationMode"))
        {
            Get<Toggle>("Toggle_OperationMode").isOn = Convert.ToBoolean(PlayerPrefs.GetInt("OperationMode"));
        }

        _toggle.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt("OperationMode", Convert.ToInt32(_toggle.isOn));
        });

        yield return null;
    }
}
