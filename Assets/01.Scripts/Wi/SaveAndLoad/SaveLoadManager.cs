using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
	//public List<StageSO> stages = new List<StageSO>();
	//public List<ThrownWeaponInfo> weaponInfoList = new List<ThrownWeaponInfo>();
	//public List<SkillInfo> skillInfoList = new List<SkillInfo>();
	//public List<Level> levels = new List<Level>(); //필요 없음.
	//public SkillInfo SkillInfo;
}

public class SaveLoadManager : MonoSingleton<SaveLoadManager>
{
	public ThrownWeaponInfo[] defaultWeaponInfos;
    public StageSO[] defaultStages;
    public SkillInfo defaultSkillInfo;
    private List<IDataObserver> observers;

	public SaveData data;
	private string path;
	private string fileName = "data.json";

    public override void Init()
    {
        /*path = Path.Combine(Application.persistentDataPath, "savefiles");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        observers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataObserver>().ToList();
        LoadData();*/
		//SaveData();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        SaveData();
    }

    public void SaveData()
	{
        /*string filePath = Path.Combine(path, fileName);
		if (!File.Exists(filePath))
		{
			File.Create(filePath);
		}

		//data = new SaveData();

        foreach (IDataObserver observer in observers)
		{
			observer.WriteData(ref data);
		}

		string jsonStr = JsonUtility.ToJson(data, true);
		byte[] jsonByte = System.Text.Encoding.UTF8.GetBytes(jsonStr);
		string jsonBase64 = System.Convert.ToBase64String(jsonByte);

		File.WriteAllText(filePath, jsonBase64);
		Debug.Log("Data Saved");*/
	}

	public void LoadData()
	{
        /*string filePath = Path.Combine(path, fileName);
		if (!File.Exists(filePath))
		{
			Debug.LogError(filePath + "Does not exists");
			if (!File.Exists(filePath))
			{
				File.Create(filePath);
			}
		}

		try
		{
            string jsonBase64 = File.ReadAllText(filePath);
            byte[] jsonByte = System.Convert.FromBase64String(jsonBase64);
            string jsonStr = System.Text.Encoding.UTF8.GetString(jsonByte);
            data = JsonUtility.FromJson<SaveData>(jsonStr);
        }
		catch (Exception ex)
		{
			Debug.LogWarning(ex.ToString());
		}
		

		if (data.levels.Count < 10)
		{
			if (data.levels.Count < 10)
			{
				data.levels = new List<Level>();
				for (int i = 0; i < 10; ++i)
				{
					Level level = new Level(i);
					data.levels.Add(level);
				}
			}
		}

		data.levels[0].Clear = true;

        //if (data.stages.Count < 1)
        //{
        //    data.stages = defaultStages.ToList();
        //}

        if (data.weaponInfoList.Count < 1)
        {
            data.weaponInfoList = defaultWeaponInfos.ToList();
        }

        data.SkillInfo ??= defaultSkillInfo;

        foreach (IDataObserver observer in observers)
		{
			observer.ReadData(data);
		}
		//Debug.Log("Data Loaded");*/
    }
}
