using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawns
{
    public List<EnemySpawn> EnemySpawn;
}

[Serializable]
public class EnemySpawn
{
    public Vector2Int EnemyNum;
    public Vector2 DelayTime;
}
