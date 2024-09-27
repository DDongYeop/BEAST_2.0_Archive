using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class Scene_OnEnd : UI_Scene
{
    [SerializeField] private Sprite _playBtn;
    [SerializeField] private Sprite _rePlayBtn;

    protected override void Init()
    {
        base.Init();

        Bind<GameObject>();
        Bind<Toggle>();


        


        //if (PlayerPrefs.HasKey("OperationMode"))
        //{
        //    (UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame).SetJoyStickPos(Convert.ToBoolean(PlayerPrefs.GetInt("OperationMode")));
        //}
    }

    protected override void Start()
    {
        base.Start();

        RaycastTarget(true);
        Get<Slider>("Slider_Volume").onValueChanged.AddListener(delegate { AudioListener.volume = Get<Slider>("Slider_Volume").value; });

        BindEvent(Get<Image>("Image_Map").gameObject, (PointerEventData _data, Transform _transform) =>
        {
            Time.timeScale = 1;
            RaycastTarget(false);

            PlayerPrefs.SetString("scene", "Panel_Menu");
            sceneManager.Instance.ChangeSceen("Menu 2", TransitionsEffect.circleWap);
        });

        //BindEvent(Get<Image>("Image_Weapon").gameObject, (PointerEventData _data, Transform _transform) =>
        //{
        //    Time.timeScale = 1;
        //    RaycastTarget(false);

        //    PlayerPrefs.SetString("scene", "Panel_WeaponSelect");
        //    sceneManager.Instance.ChangeSceen("Menu 2", TransitionsEffect.circleWap);
        //});

        BindEvent(Get<Image>("Image_Pause").gameObject, (PointerEventData _data, Transform _transform) => { OnGamePuase(); });

        Toggle _toggle = Get<Toggle>("Toggle_OperationMode");

        if (PlayerPrefs.HasKey("OperationMode"))
        {
            Get<Toggle>("Toggle_OperationMode").isOn = Convert.ToBoolean(PlayerPrefs.GetInt("OperationMode"));
        }

        (UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame).SetJoyStickPos(_toggle.isOn);

        _toggle.onValueChanged.AddListener(delegate
        {
            (UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame).SetJoyStickPos(_toggle.isOn);
            PlayerPrefs.SetInt("OperationMode", Convert.ToInt32(_toggle.isOn));
        });
    }


    protected override void OnActive()
    {
        base.OnActive();
        
        //StartCoroutine(ActiveSceneRoutine());
    }

    protected override void OnDeactive()
    {
        base.OnDeactive();
    }

    public void OnGameClear()
    {
        StartCoroutine(GameOverRoutine(true));
    }

    public void OnGameOver()
    {
        StartCoroutine(GameOverRoutine(false));
    }

    private void OnGamePuase()
    {
        StartCoroutine(GamePauseRoutine());
    }

    private IEnumerator GamePauseRoutine()
    {
        TimeManager.Instance.IsPause = true;
        Get<Image>("Image_Pause").raycastTarget = false;

        if (TryGetComponent(out Image _image))
        {
            _image.DOFade(0.75f, 0.2f);
        }

        BindEvent(Get<Image>("Image_Play").gameObject, (PointerEventData _data, Transform _transform) =>
        {
            TimeManager.Instance.IsPause = false;

            if (TryGetComponent(out Image _image))
            {
                _image.DOFade(0f, 0.2f);
            }

            Get<Image>("Image_EndPanel").rectTransform.DOAnchorPosY(-400, 0.2f);
            Get<Image>("Image_Pause").raycastTarget = true;

        });

        float playTime = GameManager.Instance.PlayTime;
        string timeType = (playTime / 60).ToString("F0") + ":" + (playTime % 60).ToString("F0");
        string str = string.Format("{0:D2}:{1:D2}", (int)(playTime / 60), (int)(playTime % 60));
        Get<TextMeshProUGUI>("Text_PlayTime").text = str;

        Get<TextMeshProUGUI>("Text_VICTORY").text = "일시정지";
        Get<Image>("Image_Setting").gameObject.SetActive(true);

        
        

        Get<Image>("Image_EndPanel").rectTransform.DOAnchorPosY(0, 0.2f).onComplete = () =>
        {
            //Time.timeScale = 0;
        };

        yield return null;
    }

    private IEnumerator GameOverRoutine(bool IsClear)
    {
        Get<Image>("Image_Pause").raycastTarget = false;
        //BindEvent(Get<Image>("Image_Play").gameObject, )

        Get<Image>("Image_Play").transform.GetChild(0).GetComponent<Image>().sprite = (IsClear ? _playBtn : _rePlayBtn);
        Get<Image>("Image_Setting").gameObject.SetActive(false);

        if (!IsClear)
        {
            Get<Image>("Image_Play").gameObject.SetActive(true);

            Get<TextMeshProUGUI>("Text_VICTORY").text = "토벌 실패";
            BindEvent(Get<Image>("Image_Play").gameObject, (PointerEventData _data, Transform _transform) => 
            {
                RaycastTarget(false);
                sceneManager.Instance.ChangeSceen(SceneManager.GetActiveScene().name, TransitionsEffect.circleWap); 
            });
        }
        else
        {
            string[] _str = SceneManager.GetActiveScene().name.Split('.');
            string _newScene = (int.Parse(_str[0]) + 1).ToString() + "." + _str[1];

            Get<TextMeshProUGUI>("Text_VICTORY").text = "토벌 성공";

            BindEvent(Get<Image>("Image_Play").gameObject, (PointerEventData _data, Transform _transform) =>
            {
                RaycastTarget(false);

                if (Application.CanStreamedLevelBeLoaded(_newScene))
                {
                    sceneManager.Instance.ChangeSceen(_newScene, TransitionsEffect.circleWap);
                }
                else
                {
                    Get<Image>("Image_Play").transform.GetChild(0).GetComponent<Image>().sprite = _rePlayBtn;
                    sceneManager.Instance.ChangeSceen(SceneManager.GetActiveScene().name, TransitionsEffect.circleWap);
                }
            });

            if (!Application.CanStreamedLevelBeLoaded(_newScene))
            {
                Get<Image>("Image_Play").gameObject.SetActive(false);
                //Get<Image>("Image_Play").transform.GetChild(0).GetComponent<Image>().sprite = _rePlayBtn;
            }
        }

        if (TryGetComponent(out Image _image))
        {
            _image.color = new Color(0, 0, 0, 0);
            _image.DOFade(0.75f, 0.6f);
        }

        

        if (IsClear)
        {
            Get<TextMeshProUGUI>("Text_PlayTime").text = "";
            Get<TextMeshProUGUI>("Text_Seconde").text = "";

            Transform _newSkill = transform.Find("Image_EndPanel").Find("NewSkill");
            _newSkill.gameObject.SetActive(true);
            //_newSkill.Find("Image_Skill").GetComponent<Image>().sprite = LevelManager.Instance._newSkill.SkillSprite;
            //_newSkill.Find("Text_Skill").GetComponent<TextMeshProUGUI>().text = LevelManager.Instance._newSkill.SkillName;
        }
        else
        {
            Get<TextMeshProUGUI>("Text_PlayTime").text = "";
            Get<Image>("Image_EndPanel").rectTransform.anchoredPosition = new Vector2(0, -400f);

            float playTime = GameManager.Instance.PlayTime;
            string timeType = (playTime / 60).ToString("F0") + ":" + (playTime % 60).ToString("F0");
            string str = string.Format("{0:D2}:{1:D2}", (int)(playTime / 60), (int)(playTime % 60));

            Get<TextMeshProUGUI>("Text_PlayTime").text = str;
        }


        Get<Image>("Image_EndPanel").rectTransform.DOAnchorPosY(0, 0.5f).onComplete = () =>
        {
            //DOTween.To(() => 0f, x => Get<TextMeshProUGUI>("Text_PlayTime").text = x.ToString("F0"), playTime, 0.8f);
            //DOTween.To(() => "", x => Get<TextMeshProUGUI>("Text_PlayTime").text = x, timeType, 0.8f);
        };


        yield return null;
    }

    private void RaycastTarget(bool _value)
    {
        Get<Image>("Image_Map").raycastTarget = _value;
        //Get<Image>("Image_Weapon").raycastTarget = _value;
        Get<Image>("Image_Play").raycastTarget = _value;
    }
}
