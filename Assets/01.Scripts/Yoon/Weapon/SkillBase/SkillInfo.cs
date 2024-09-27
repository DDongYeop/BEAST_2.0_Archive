using UnityEngine;

// UI와 상호작용을 위한 SO
[CreateAssetMenu(menuName = "SO/ThronwWeapon/SkillInfo")]
public class SkillInfo : ScriptableObject
{
    public string SkillId => skillType.ToString();

    [SerializeField] private string skillName;
    public string SkillName => skillName;

    [SerializeField] private SkillType skillType; 
    public SkillType SkillType => skillType;

    // 스킬 아이콘 이미지
    [SerializeField] private Sprite skillSprite;
    public Sprite SkillSprite => skillSprite;

    [SerializeField] private Vector2 spritePivotPosition = Vector2.zero;
    public Vector2 SpritePivotPosition => spritePivotPosition;

    [TextArea(1, 2)][SerializeField] private string skillDescription;
    public string SkillDescription => skillDescription;

    [SerializeField] private int countForRecharge; // 스킬 카운트 충전을 위한 발사 횟수
    public int CountForRecharge => countForRecharge;

    //풀린 스킬인지 아닌지
    public bool IsActive = false;
}
