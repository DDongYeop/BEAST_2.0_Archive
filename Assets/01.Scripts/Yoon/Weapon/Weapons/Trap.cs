using UnityEngine;

public class Trap : ThrownWeapon
{
    private SturnSkill sturnComponent;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float sturnTime = 2f;

    [HideInInspector] public bool isRealTrap = false;

    private Vector3 initRotation = new Vector3(0, 0, -90);
    private float initPositionY = -7f;

    private string collisionTagName = "FloorCheck";

    // LifeTime
    [Header("LifeTime")]
    [SerializeField] private float lifeTime = 1f;
    private float aliveTime = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        sturnComponent = GetComponent<SturnSkill>();
    }

    public override void Init()
    {
        base.Init();
        collider.isTrigger = false;
        aliveTime = 0f;
        isRealTrap = false;
    }

    protected override void WeaponUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        aliveTime += Time.deltaTime;
        if (aliveTime >= lifeTime)
        {
            PoolManager.Instance.Push(this);
        }
    }

    protected override void OnGroundCollisionEvent()
    {
        base.OnGroundCollisionEvent();

        if (false == isRealTrap)
        {
            MakeTrap();
        }
    }

    private void MakeTrap()
    {
        Trap trap = PoolManager.Instance.Pop("Trap") as Trap;
        trap.transform.parent = null;
        trap.transform.localPosition = new Vector2(transform.position.x, initPositionY);
        trap.transform.localRotation = Quaternion.Euler(initRotation);
        trap.isRealTrap = true;
        trap.ColliderToTriggr();
    }

    public void ColliderToTriggr()
    {
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(collisionTagName))
        {
            sturnComponent.SturnTarget(collision.transform, sturnTime);
            PoolManager.Instance.Push(this);
        }
    }
}
