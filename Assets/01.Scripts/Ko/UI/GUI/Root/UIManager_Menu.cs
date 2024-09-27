using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class UIManager_Menu : UI_Root, IDataObserver
{
    [SerializeField] private ThrownWeaponInfo[] defaultWeaponInfos;
    [SerializeField] private SkillInfo defaultSkillInfo;

    public static UIManager_Menu Instance;

    [SerializeField] private string _defaultScene = "";

    //private Animator _animator;

  

    protected override void Awake()
    {
        base.Awake();

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }


        if (PlayerPrefs.HasKey("scene"))
        {
            _defaultScene = PlayerPrefs.GetString("scene");
            PlayerPrefs.DeleteKey("scene");
        }

        foreach (var item in _scenes)
        { 
            if (item.Value.name != _defaultScene)
                item.Value.gameObject.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();

        //SaveLoadManager.Instance.LoadData();
        //BindEvent(Get<Image>("Image_Book").gameObject, BookOpen, Define.ClickType.Click);


        //for(int i = 1; i <= 6; i++)
        //{
        //    //GameObject obj = Get<Image>("Image_ItemIcon" + i.ToString()).gameObject;
        //    //BindEvent(obj, OnSideTapCliked, Define.ClickType.Click);

        //    //obj.SetActive(false);
        //    Get<Image>("Image_Page" + i.ToString()).gameObject.SetActive(false);
        //}
        //Get<Image>("Image_Page" + 0.ToString()).gameObject.SetActive(false);

        //SetPage(1);
    }

    //private void OnSideTapCliked(PointerEventData _data, Transform _transform)
    //{
    //    var itemIndex = _transform.name.Split('n');

    //    SetPage(int.Parse(itemIndex[1]));
    //}

    //private void BookOpen(PointerEventData data = default, Transform transform = default)
    //{
    //    if (_bookState != BookState.Closed)
    //        return;

    //    //_animator.SetTrigger(OPEN);

    //    //SetPage(1);

    //    //Util.Util.BindAnimatorEvent(_animator, "ContentAppear", Util.AniState.OnStart, () => Get<Image>("Image_Page1").gameObject.SetActive(true));
    //    _bookState = BookState.Page1;
    //}


    public void WriteData(ref SaveData data)
    {
        //if(data.weaponInfoList.Count < 1)
        //    data.weaponInfoList = defaultWeaponInfos.ToList();

        //data.SkillInfo ??= defaultSkillInfo;
    }

    public void ReadData(SaveData data)
    {
        //if (data == null || data.weaponInfoList == null || data.weaponInfoList.Count == 0 || data.SkillInfo == null)
        //{
        //    SaveLoadManager.Instance.SaveData();
        //    Debug.Log("ù ������");
        //}
    }

    //private IEnumerator PageChangeRoutine(int num)
    //{
    //    yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"));
    //    _animator.SetTrigger(num > (int)_bookState ? FLIP_LEFT : FLIP_RIGHT);

    //    yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    //    Get<Image>("Image_Page" + ((int)_bookState).ToString()).gameObject.SetActive(false);

    //    yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("ContentAppear"));
    //    //Get<Image>("Image_Page" + 0.ToString()).gameObject.SetActive(true);
    //    _bookState = (BookState)num;
    //    Get<Image>("Image_Page" + ((int)_bookState).ToString()).gameObject.SetActive(true);
    //}
}