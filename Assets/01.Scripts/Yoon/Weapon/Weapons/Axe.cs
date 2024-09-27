using UnityEngine;

public class Axe : ThrownWeapon
{
    [SerializeField] private float maxRotateSpeed;
    [SerializeField] private float minRotateSpeed;
    [SerializeField] private float dampingFactor;

    private float rotateSpeed;
    private bool isFalling;

    public override void Init()
    {
        base.Init();
        rotateSpeed = minRotateSpeed;
        isFalling = false;
    }
  
    protected override void WeaponUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        rotateSpeed = Mathf.Clamp((rotateSpeed + dampingFactor * Time.deltaTime), minRotateSpeed, maxRotateSpeed);

        // 떨어지는 상태일 때 더 빨리 떨어지도록.
        if (rigidbody.velocity.y <= 0f && false == isFalling)
        {
            isFalling = true;
            rigidbody.gravityScale *= 2f;
        }
    }
}
