using UnityEngine;

[CreateAssetMenu(menuName = "SO/ThronwWeapon/WeaponStat")]
public class ThrownWeaponStat : ScriptableObject
{
    [SerializeField] private string weaponId;
    public string WeaponId => weaponId;

    // 무기 이미지
    [SerializeField] private Sprite mainWeaponSprite;
    public Sprite MainWeaponSprite => mainWeaponSprite;
    [SerializeField] private Sprite subWeaponSprite;
    public Sprite SubWeaponSprite => subWeaponSprite;

    // 공격력
    [SerializeField] private int damage;
    public int Damage => damage;

    // 최대 발사 횟수
    [SerializeField] private int maxThrowCount = 0;
    public int MaxThrowCount => maxThrowCount;
    public int CurrentThrowCount { get; set; } = 0;
    public bool IsOverThrow
    {
        get
        {
            return MaxThrowCount != 0 && MaxThrowCount <= CurrentThrowCount;
        }
    }

    // 최대 충돌 횟수 (관통 가능에 대비)
    [SerializeField] private int maxCollisionCount = 1;
    public int MaximumCollisionCount => maxCollisionCount;
    // public int CurrentCollisionCOunt { get; set; } = 0;

    // 발사체 무게
    [SerializeField] private float weaponMass = 2.0f;
    public float WeaponMass => weaponMass;

    // 무거운 무기인지.
    [SerializeField] private bool isHeavy = false;
    public bool IsHeavy => isHeavy;

    [SerializeField] private bool isSharp = true;
    public bool IsSharp => isSharp;
}
