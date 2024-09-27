using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoSingleton<WaveManager>
{
    [SerializeField] private List<Transform> _spawnPos;
    [SerializeField] private List<WaveSO> _waveSos; // 웨이브들 모아둔거

    private int _currentWave = -1;
    private int _spawnCnt = 0;
    private float _currentTime = 0;

    private int _monsterCnt = 0;
    private int _dieMonsterCnt = 0;

    private void Start()
    {
        _currentWave = -1;
        StartWave();
    }

    private void Update()
    {
        if (!TimeManager.Instance.IsPause)
            SpawnMonster();
    }

    /// <summary>
    /// 몬스터 스폰 해주는거
    /// </summary>
    private void SpawnMonster()
    {
        if (_waveSos.Count <= _currentWave || _waveSos[_currentWave].Waves.Count <= _spawnCnt)
            return;

        _currentTime += Time.deltaTime;
        if (_waveSos[_currentWave].Waves[_spawnCnt].SpawnTime <= _currentTime)
        {
            PoolableMono monster = PoolManager.Instance.Pop(_waveSos[_currentWave].Waves[_spawnCnt].EnemyName); // 몹 스폰
            int cnt = Random.Range(0, _spawnPos.Count);
            monster.transform.position = _spawnPos[cnt].position;
            ++_spawnCnt;
            monster.Init();
        }
    }

    /// <summary>
    /// 웨이브 시작 
    /// </summary>
    private void StartWave()
    {
        ++_currentWave; // 다음웨이브로 넘어가고
        
        if (_waveSos.Count == _currentWave) // 웨이브 끝나면 (처음)
            GameManager.Instance.GameClear();
        if (_waveSos.Count <= _currentWave) // 웨이브 끝나면
            return;

        (UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame)?.WavePopup(_currentWave + 1);

        _monsterCnt = _waveSos[_currentWave].Waves.Count; // 몬스터 개수
        _currentTime = _spawnCnt = _dieMonsterCnt = 0; // 초기화
    }

    public void MonsterDie()
    {
        ++_dieMonsterCnt; // 죽인 몬스터 개수 추가

        if (_dieMonsterCnt >= _monsterCnt) // 스폰 될 몬스터의 개수가 죽인 몬스터의 개수를 넘는다면 (웨이브를 종료하면)
            StartWave(); // 다음 웨이브 시작
    }
}
