using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/GameData/StageLevelData", fileName = "New StageLevelData")]
public class StageSO : ScriptableObject
{
    [SerializeField] private string m_bossName;
    public string BossName => m_bossName;

    [SerializeField] private int m_maxLevel;
    public int MaxLevel => m_maxLevel;

    [SerializeField] private int m_startLevelIndex;
    public int StartLevelIndex => m_startLevelIndex;

    public List<string> GetSceneNames()
    {
        List<string> _sceneNames = new List<string>();
        
        for(int i = 1; i <= m_maxLevel; i++)
        {
            _sceneNames.Add(i.ToString() + "." + m_bossName);
        }

        return _sceneNames;
    }
}
