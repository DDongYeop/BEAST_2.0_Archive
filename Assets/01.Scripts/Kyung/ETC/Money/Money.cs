using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Money : PoolableMono
{
    [SerializeField] private int _money;
    
    [SerializeField] private float _moveTime;
    
    [SerializeField] private float _xValue;
    [SerializeField] private float _yValue;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            GameManager.Instance.AddMoney(_money);
            PoolManager.Instance.Push(this);
        }
        if (other.gameObject.CompareTag("Ground") && gameObject.activeSelf)
        {
            StartCoroutine(MovementAndScaleCo());
        }
    }

    public override void Init()
    {
        _rigidbody.gravityScale = 1;
        float x = Random.Range(-_xValue, _xValue);
        _rigidbody.AddForce(new Vector2(x, _yValue));
    }

    private IEnumerator MovementAndScaleCo()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.gravityScale = 0;
        Vector3 startPos = transform.position;
        float current = 0;

        while (current <= _moveTime)
        {
            yield return null;
            current += Time.deltaTime;
            float t = current / _moveTime;
            
            //transform.position = Vector3.Slerp(startPos, GameManager.Instance.PlayerTrm.position + new Vector3(0, 2f), t);
            transform.position = new Vector3(Mathf.Lerp(startPos.x, GameManager.Instance.PlayerTrm.position.x, t),
                GameManager.Instance.PlayerTrm.position.y + Mathf.Sin(t * Mathf.PI), GameManager.Instance.PlayerTrm.position.z);
        }
    }
}
