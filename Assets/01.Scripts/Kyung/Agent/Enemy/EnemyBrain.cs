using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyFeedback), typeof(WeaponStick))]
public class EnemyBrain : PoolableMono
{
    [Header("Brain")]
    [HideInInspector] public bool IsDie;

    [Header("Components")] 
    [HideInInspector] public AgentHealth AgentHealth;
    [HideInInspector] public AgentAnimator AgentAnimator;
    [HideInInspector] public WeaponStick WeaponStick;

    [Header("Slow")] 
    private Coroutine _slowCo = null;
    [HideInInspector] public float SlowValue = 100;

    [Header("AttackRange")] 
    private float _radius;
    private float _range;

    [Header("Money")] 
    [SerializeField] private int _minCnt;
    [SerializeField] private int _maxCnt;
    
    [Header("KnockBack")]
    [HideInInspector] public bool IsKnockback = false;
    [SerializeField] private float _knockbackValue = 50;
    [SerializeField] private float _knockBackTime = .25f;

    [Header("Other")]
    [SerializeField] private float _addYPos;
    private Rigidbody2D _rigidbody2D;
    [HideInInspector] public float StunTime = 0;
    [HideInInspector] public int CurrentNodeValue = -1;
    [HideInInspector] public int MoveDirection;

    private void Awake()
    {
        AgentHealth = GetComponent<AgentHealth>();
        AgentAnimator = GetComponent<AgentAnimator>();
        WeaponStick = GetComponent<WeaponStick>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SlowValue = 100;
    }

    /// <summary>
    /// 적 감소
    /// </summary>
    /// <param name="time">지속시간</param>
    /// <param name="value">백분율로 나눠진 슬로운 값. 70 == 70% 속도로 감</param>
    public void SetSlow(float time, float value)
    {
        if (_slowCo != null)
            StopCoroutine(_slowCo);
        _slowCo = StartCoroutine(SlowCo(time, value));
    }

    private IEnumerator SlowCo(float time, float value)
    {
        SlowValue = value;
        AgentAnimator.Animator.speed = SlowValue / 100.0f;
        yield return new WaitForSeconds(time);
        SlowValue = 100; 
        AgentAnimator.Animator.speed = 1;
    }

    public override void Init()
    {
        AgentHealth.Init();
        WeaponStick.RemoveAll();
        AgentAnimator.Init();
        
        MoveDirection = (int)Mathf.Sign(GameManager.Instance.TotemTrm.position.x - transform.position.x);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -MoveDirection, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + _addYPos, transform.position.z);
    }

    public void KnockBack()
    {
        StopAllCoroutines();
        StartCoroutine(KnockBackCo());
    }
    
    public IEnumerator KnockBackCo()
    {
        Vector3 knockbackValue = new Vector3(_knockbackValue * -MoveDirection, 0, 0);
        print(knockbackValue);
        float currentTime = 0;
        IsKnockback = true;

        while (currentTime < _knockBackTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            transform.position += knockbackValue * Time.deltaTime;
        }
        
        IsKnockback = false;
    }

    public void MoneyDrop()
    {
        int cnt = Random.Range(_minCnt, _maxCnt);

        for (int i = 0; i < cnt; ++i)
        {
            Transform trm = PoolManager.Instance.Pop("Money").transform;
            trm.position = transform.position;
        }
    }

    public void GizmosSetup(float range, float radius)
    {
        _range = range;
        _radius = radius;
    }
    
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(MoveDirection * _range, 0), _radius);
        Gizmos.color = Color.white;
    }

#endif
}
