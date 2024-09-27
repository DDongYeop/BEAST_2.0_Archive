using UnityEngine;

// UI와 상호작용을 위한 SO
[CreateAssetMenu(menuName = "SO/ThronwWeapon/WeaponInfo")]
public class ThrownWeaponInfo : ScriptableObject
{
    [SerializeField] private string weaponId;
    public string WeaponId => weaponId;

    // 무기 이름 (For UI)
    [SerializeField] private string weaponName;
    public string WeaponName => weaponName;

    // 무기 이미지
    [SerializeField] private Sprite weaponSprite;
    public Sprite WeaponSprite => weaponSprite;

    [SerializeField] private Vector2 spritePivotPosition = Vector2.zero;
    public Vector2 SpritePivotPosition => spritePivotPosition;

    [TextArea(1, 2)][SerializeField] private string weaponDescription;
    public string WeaponDescription => weaponDescription;
}
