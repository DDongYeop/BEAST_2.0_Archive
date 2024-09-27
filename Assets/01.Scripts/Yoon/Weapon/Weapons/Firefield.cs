using System.Collections;
using UnityEngine;

public class Firefield : PoolableMono
{
    [SerializeField] private float checkDamageCoolTime = 2f;

    [SerializeField] private float lifeTime = 1f;
    private float aliveTime = 0.0f;

    private GameObject collisionObject;
    private string collisionTagName = "FloorCheck";

    public override void Init()
    {
        aliveTime = 0f;
        collisionObject = null;
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime >= lifeTime)
        {
            PoolManager.Instance.Push(this);
        }
    }

    private IEnumerator CheckDamageCor(Transform trm)
    {
        while (collisionObject != null)
        {
            bool isCheck = collisionObject.transform.root.TryGetComponent(out AgentHealth enemyHealth);
            if (isCheck)
            {
                if (enemyHealth.CurrentHp > 0)
                {
                    enemyHealth.OnDamage(1, Vector3.zero);
                }
            }

            yield return new WaitForSeconds(checkDamageCoolTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(collisionTagName))
        {
            collisionObject = collision.gameObject;
            StartCoroutine(CheckDamageCor(collision.transform));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(collisionTagName))
        {
            collisionObject = null;
        }
    }
}
