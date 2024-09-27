using UnityEngine;

public class EnemyCollision : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyPartType _partType = EnemyPartType.SPOT;

    [Header("Component")] 
    private EnemyBrain _brain;
    private AgentHealth _agentHealth;

    private void Awake()
    {
        Transform trm = transform;
        while (!trm.parent.TryGetComponent(out _brain))
            trm = trm.parent;
        _agentHealth = _brain.GetComponent<AgentHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.transform.TryGetComponent(out ThrownWeapon weapon) || !weapon.IsFlying || _brain.IsDie)
            return;
        
        
        if (_partType == EnemyPartType.ATTACK)
        {
            weapon.IsFlying = false;
            weapon.ObjectFall();
            NotAttack(other.ClosestPoint(transform.position));
            return;
        }
        
        PoolManager.Instance.Pop("Bear_Hit");
        
        OnDamage(weapon.Stat.Damage, other.ClosestPoint(transform.position)); // 데미지 주기
        _brain.KnockBack();

        if (++weapon.CurrentCollisionCount >= weapon.Stat.MaximumCollisionCount)
        {
            weapon.IsFlying = false;
            weapon.ObjectFall();
        }
    }

    public void OnDamage(int damage, Vector3 hitPos = new Vector3())
    {
        hitPos += new Vector3(0, 0, transform.position.z);
        _agentHealth.OnDamage(damage, hitPos);
        Taptic.Light();
        Transform hitParticle = PoolManager.Instance.Pop("Basic Hit").transform;
        hitParticle.position = hitPos;
        Transform furParticle = PoolManager.Instance.Pop("FurEffect").transform;
        furParticle.position = hitPos;

        GameManager.Instance.ComboIncrease();
    }

    public void NotAttack(Vector3 hitPos = new Vector3())
    {
        Transform shineParticle = PoolManager.Instance.Pop("ShineEffect").transform;
        shineParticle.position = hitPos;
    }
}
