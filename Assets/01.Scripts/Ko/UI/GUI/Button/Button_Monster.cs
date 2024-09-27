using UnityEngine;

public class Button_Monster : Button_Menu, IDataObserver
{
    [SerializeField] private StageSO _stageData;
    private SaveData _data;

    public override void OnAction()
    {
        SaveLoadManager.Instance.LoadData();
        UIManager_Menu.Instance.ShowScene("Image_Stages");
        UIManager_Menu.Instance.HideScene("Panel_Menu");
        
        (UIManager_Menu.Instance.GetScene("Image_Stages") as Scene_LevelSelect).BindSlot(_stageData);
    }

    [ContextMenu("UnLcok")]
    private void UnLock()
    {
        transform.Find("Image_Fill").Find("Image_Mask").Find("Image_Chain").gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        //if(_stageData.StartLevelIndex == 1)
        //{
        //    UnLock();
        //    GetComponent<UI_EventHandler>().Enable = true;
        //}
        //else
        //{
        //    GetComponent<UI_EventHandler>().Enable = false;

        //}
    }

    protected override void Start()
    {
        base.Start();

        //if (_data.levels[_stageData.StartLevelIndex - 1].Clear)
        //{
        //    //_enable = true;
        //    GetComponent<UI_EventHandler>().Enable = true;

        //    UnLock();
        //}
        //else
        //{
        //    GetComponent<UI_EventHandler>().Enable = false;
        //    //Invoke("DestroyComponent", 0.2f);
        //}
    }

    public void ReadData(SaveData data)
    {
        Debug.Log(gameObject.name);


        _data = data;

        //if (_data.levels[_stageData.StartLevelIndex - 1].Clear)
        //{
        //    //_enable = true;
        //    GetComponent<UI_EventHandler>().Enable = true;

        //    UnLock();
        //}
        //else
        //{
        //    GetComponent<UI_EventHandler>().Enable = false;
        //    //Invoke("DestroyComponent", 0.2f);
        //}
    }

    private void DestroyComponent()
    {
        Destroy(gameObject.GetComponent<UI_DragEventHandler>());
        Destroy(gameObject.GetComponent<UI_EventHandler>());
    }

    public void WriteData(ref SaveData data)
    {
        
    }
}
