using UnityEngine;

public class SpiderWeb : PoolableMono
{
    [SerializeField] private float _speed;
    [SerializeField] private float _downPos;

    [HideInInspector] public Vector2 TargetPos;
    private Rigidbody2D _rigidbody2D;
    private bool _isDown;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y > _downPos)
        {
            _isDown = true;
            transform.position = new Vector3(TargetPos.x, transform.position.y);
        }
        
        float speedY = _speed * (_isDown ? -1 : 1);
        _rigidbody2D.velocity = new Vector2(0, speedY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<IDamageable>().OnDamage(1, transform.position);
            GameManager.Instance.GameOver();
            PoolManager.Instance.Push(this);
        }

        if (other.CompareTag("Ground"))
        {
            // 거미줄 퍼지는 이펙트
            PoolManager.Instance.Push(this);
        }
    }

    public override void Init()
    {
        _isDown = false;
    }
}
