using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyBrokenObj : MonoBehaviour
{
    [SerializeField] private int _maxHp;

    private int _currentHp;

    private void Start()
    {
        _currentHp = _maxHp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.TryGetComponent(out ThrownWeapon weapon) || !weapon.IsFlying)
            return;
        
        weapon.IsFlying = false;
        weapon.ObjectFall();
        PoolManager.Instance.Pop("Bear_Hit");

        if (weapon.Skill != null && weapon.SkillData.RunImmediately == false) // 스킬 확인
            weapon.UseSkill(transform.root.gameObject);

        
        Vector3 pos = transform.position;
        Color color = Color.yellow;
        (PoolManager.Instance.Pop("DamagePopup") as DamagePopup)?.SetUp("-" + weapon.Stat.Damage, pos, 16, color);
        
        _currentHp -= weapon.Stat.Damage;
        if (_currentHp <= 0)
            BrokenObj();
    }

    private void BrokenObj()
    {
        PoolableMono particle = PoolManager.Instance.Pop("DustEffectBear");
        particle.transform.SetParent(transform.parent);
        particle.transform.localPosition = transform.localPosition;
        Destroy(gameObject);
    }
}
