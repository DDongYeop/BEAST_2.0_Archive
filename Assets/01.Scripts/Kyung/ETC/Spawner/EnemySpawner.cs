using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance = null;
    
    [SerializeField] private EnemySpawnSO _enemySpawnSo;
    [SerializeField] private Vector2 _addPos;

    private Transform _playerTrm;
    private float _delayTime;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple EnemySpawner is running");
        Instance = this;
        
        _enemySpawnSo = Instantiate(_enemySpawnSo);
    }

    private void Start()
    {
        _playerTrm = GameManager.Instance.PlayerTrm;
    }

    private void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        _delayTime -= Time.deltaTime;

        if (_delayTime <= 0)
            _delayTime = _enemySpawnSo.NextSpawn(_playerTrm.position + (Vector3)_addPos);
    }

    public void EnemyDie()
    {
        _enemySpawnSo.EnemyDie();
    }
}
