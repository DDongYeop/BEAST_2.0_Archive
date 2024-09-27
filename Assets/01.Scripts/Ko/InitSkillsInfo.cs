using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitSkillsInfo : MonoBehaviour, IDataObserver
{
    [SerializeField] private SkillInfo[] SkillInfos;

    public void ReadData(SaveData data)
    {
        
    }

    public void WriteData(ref SaveData data)
    {
        //if(data.skillInfoList == null || data.skillInfoList.Count < 1) 
        //{
        //    data.skillInfoList = SkillInfos.ToList();
        //}
    }

    void Start()
    {
        SaveLoadManager.Instance.SaveData();
    }
}
