public class WoodBranch : ThrownWeapon
{
    private bool isFalling;

    protected override void WeaponUpdate()
    {
        transform.up = rigidbody.velocity;

        // 떨어지는 상태일 때 더 빨리 떨어지도록.
        if (rigidbody.velocity.y <= 0f && false == isFalling)
        {
            isFalling = true;
            rigidbody.gravityScale *= 1.5f;
        }
    }
}
