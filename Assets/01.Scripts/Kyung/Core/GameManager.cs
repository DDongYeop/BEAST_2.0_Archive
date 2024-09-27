using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Player")] 
    public Transform PlayerTrm;

    [Header("Enemy")]
    public EnemyBrain EnemyBrain; // 삭제

    [Header("Object")] 
    public Transform TotemTrm; //이거 받아오기. 

    [Header("Pooling")] 
    [SerializeField] private PoolingListSO _poolingList;
    [SerializeField] private PoolingListSO _weaponPoolingList;
    [SerializeField] private PoolingListSO _monsterPollingList;

    [Header("Time")] 
    [SerializeField] private float _gameTime;

    [Header("Money")] 
    [HideInInspector] public int Money = 0;
    public event Action<int> OnMoneyChange = null;

    private float _playTime;
    public bool IsGameOver = false;

    public int ComboCount { get; set; }

    public float PlayTime
    {
        get { return Mathf.Floor(_playTime * 10f) / 10f; }
        set { _playTime = value; }
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        PlayerTrm = GameObject.Find("Player").transform;
        EnemyBrain = FindObjectOfType<EnemyBrain>();
        TotemTrm = FindObjectOfType<Totem>().transform;
        CreatePool();

        // Manager Script Init
        SaveLoadManager.Instance.Init();
        //LevelManager.Instance.Init();
    }

    private void Start()
    {
        FrameLimit();
        ComboReset();
    }

    private void Update()
    {
        Time.timeScale = _gameTime;
    }

    private void FixedUpdate()
    {
        _playTime += Time.fixedDeltaTime;
    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        _poolingList.PoolList.ForEach(p => PoolManager.Instance.CreatePool(p.Prefab, p.Count));
        _weaponPoolingList.PoolList.ForEach(p => PoolManager.Instance.CreatePool(p.Prefab, p.Count));
        _monsterPollingList.PoolList.ForEach(p => PoolManager.Instance.CreatePool(p.Prefab, p.Count));
    }

    public void ComboIncrease()
    { 
        ComboCount++;
        //(UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame).ApplyCombo(ComboCount);
    }

    public void ComboReset()
    {
        ComboCount = 1;
        //(UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame).ApplyCombo(ComboCount);
    }

    [ContextMenu("GameClear")]
    public void GameClear()
    {
        Debug.Log("Game Clear");

        // 보물 드랍 유무 
        //LevelManager.Instance.GameClear();
        
        UIManager_InGame.Instance.GetScene("Scene_OnEnd").GetComponent<Scene_OnEnd>().OnGameClear();
    }
    
    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        FindObjectOfType<EnemyBrain>().AgentAnimator.SetAnimEnd();
        Debug.Log("Game Over");
        UIManager_InGame.Instance.GetScene("Scene_OnEnd").GetComponent<Scene_OnEnd>().OnGameOver();
    }

    public void AddMoney(int money)
    {
        Money += money;
        (UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame)?.OnMoneyChanged(Money);
        OnMoneyChange?.Invoke(Money);
    }

    [ContextMenu("AddMoney")]
    public void AddMoney()
    {
        Money += 100000;
    }

    private void FrameLimit()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 120;
#endif
    }
}
