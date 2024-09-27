using UnityEngine;

[CreateAssetMenu(menuName = "SO/ThronwWeapon/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillId => skillType.ToString();

    [SerializeField] private SkillType skillType; // Skill Component 생성하기 위한 Reflection에 사용
    public SkillType SkillType => skillType;

    [SerializeField] private int countForRecharge; // 스킬 카운트 충전을 위한 발사 횟수
    public int CountForRecharge => countForRecharge;

    [SerializeField] private int damage = 0; // 공격력, 공격력 업그레이드 정도를 나타냄
    public int Damage => damage;

    [SerializeField] private float duration = 0f; // 발동 시간
    public float Duration => duration;

    public string CurrentWeaponId { get; set; }

    [SerializeField] private bool runImmediately = false; // 즉시 발동할지
    public bool RunImmediately => runImmediately;

}
