using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SO/Agent/Enemy/EnemySpawner", fileName = "EnemySpawner")]
public class EnemySpawnSO : ScriptableObject
{
    [SerializeField] private List<GameObject> _enemyObj;
    [SerializeField] private List<EnemySpawns> _enemySpawns; 

    [SerializeField] private int _currentWave = 0;
    [SerializeField] private int _currentCnt = 0;
    [SerializeField] private int _enemyCnt = 0;

    public float NextSpawn(Vector2 spawnPos)
    {
        if (_currentCnt >= _enemySpawns[_currentWave].EnemySpawn.Count)
            return 0;

        int cnt = Random.Range(_enemySpawns[_currentWave].EnemySpawn[_currentCnt].EnemyNum.x, _enemySpawns[_currentWave].EnemySpawn[_currentCnt].EnemyNum.y + 1);
        float delay = Random.Range(_enemySpawns[_currentWave].EnemySpawn[_currentCnt].DelayTime.x, _enemySpawns[_currentWave].EnemySpawn[_currentCnt].DelayTime.y);
        Instantiate(_enemyObj[cnt], spawnPos, Quaternion.identity);
        
        ++_currentCnt;
        ++_enemyCnt;
        
        return delay;
    }

    public void EnemyDie()
    {
        --_enemyCnt;
        if (_enemyCnt > 0)
            return;
        
        if (_currentCnt >= _enemySpawns[_currentWave].EnemySpawn.Count && _currentWave < _enemySpawns.Count)
        {
            _currentCnt = 0;
            ++_currentWave;

            if (_currentWave >= _enemySpawns.Count)
                GameManager.Instance.GameClear();
        }
    }
}
