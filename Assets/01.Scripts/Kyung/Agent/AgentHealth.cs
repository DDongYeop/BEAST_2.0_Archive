using System.Collections;
using UnityEngine;

public abstract class AgentHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthSO _healthSO;

    protected EnemyBrain _brain;
    protected AgentAnimator _agentAnimator;

    private int _currentHp;
    public int CurrentHp => _currentHp;

    protected virtual void Awake()
    {
        _healthSO = Instantiate(_healthSO);
    }

    protected void Start()
    {
        _brain = GetComponent<EnemyBrain>();
        _agentAnimator = GetComponent<AgentAnimator>();
        Init();
    }

    public void Init()
    {
        _currentHp = _healthSO.MaxHP;
    }
    

    public virtual void OnDamage(int damage, Vector3 hitPos)
    {
        _currentHp -= damage;

        if (_currentHp <= 0)
            StartCoroutine(EnemyDie());
        
        // Damage Text 
        Vector3 pos = hitPos == Vector3.zero ? _brain.transform.position : hitPos;
        Color color = hitPos == Vector3.zero ? Color.red : Color.white;
        (PoolManager.Instance.Pop("DamagePopup") as DamagePopup)?.SetUp("-" + damage.ToString(), pos, 16, color);
    }

    /// <summary>
    /// 0.0f와 1.0f의 사이 값이 나옵니다 .나중에 HPbar을 만들때 쓰기 좋을거에요. 
    /// </summary>
    /// <returns></returns>
    public float HpPercent()
    {
        return (float)_currentHp / _healthSO.MaxHP;
    }

    protected virtual IEnumerator EnemyDie()
    {
        _brain.IsDie = true;
        _agentAnimator.OnDie();
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void Stun(float maxTime)
    {
        if (_brain.StunTime <= 0)
        {
            _brain.StunTime = maxTime;
            _agentAnimator.SetAnimEnd();
        }
    }
}

//도트댐 만들기 
