using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    public Action<ThrownWeaponStat> OnWeaponStatChanged;
    public Action<SkillData> OnTresureDataChanged;

    [SerializeField] private List<ThrownWeaponStat> weaponStats = new ();
    private Dictionary<string, ThrownWeaponStat> weaponStatContainer = new ();

    [SerializeField] private List<SkillData> tresureDatas = new();
    private Dictionary<string, SkillData> tresureDataContainer = new();

    private ThrownWeaponStat currentWeaponStat;
    public ThrownWeaponStat CurrentWeaponStat => currentWeaponStat;
    private SkillData currentSkillData;
    public SkillData CurrentSkillData => currentSkillData;

    private void Awake()
    {
        foreach (var stat in weaponStats)
        {
            stat.CurrentThrowCount = 0;
            weaponStatContainer.Add(stat.WeaponId, stat);
        }

        foreach (var data in tresureDatas)
        {
            tresureDataContainer.Add(data.SkillId, data);
        }
    }

    public void AttemptChangeWeaponStat(string weaponId)
    {
        if (weaponStatContainer.ContainsKey(weaponId))
        {
            currentWeaponStat = weaponStatContainer[weaponId];
            RequestChangeWeaponStat();
        }
        else
        {
            Debug.LogError($"weaponStatContainer is not exist {weaponId} key...");
        }
    }

    private void RequestChangeWeaponStat()
    {
        OnWeaponStatChanged?.Invoke(currentWeaponStat);
    }

    public void AttemptChangeSKillData(SkillType skillType)
    {
        string skillTypeString = skillType.ToString();
        if (tresureDataContainer.ContainsKey(skillTypeString))
        {
            currentSkillData = tresureDataContainer[skillTypeString];
            RequestChangeTresureData();
        }
        else
        {
            Debug.LogError($"skillDataContainer is not exist {skillTypeString} key...");
        }
    }

    private void RequestChangeTresureData()
    {
        OnTresureDataChanged?.Invoke(currentSkillData);
    }

}
