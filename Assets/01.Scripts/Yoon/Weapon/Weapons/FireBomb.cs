using UnityEngine;

public class FireBomb : ThrownWeapon
{
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float initPositionY = -3f;

    private bool isFalling;

    public override void Init()
    {
        base.Init();
        isFalling = false;
    }

    protected override void WeaponUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        // 떨어지는 상태일 때 더 2배 느리게 
        if (rigidbody.velocity.y <= 0f && false == isFalling)
        {
            isFalling = true;
            rigidbody.mass *= 0.5f;
        }
    }

    protected override void OnGroundCollisionEvent()
    {
        base.OnGroundCollisionEvent();

        // 화염 오브젝트 생성 
        Firefield fireField = PoolManager.Instance.Pop("FireField") as Firefield;
        fireField.transform.position = new Vector2(transform.position.x, initPositionY);
    }
}
