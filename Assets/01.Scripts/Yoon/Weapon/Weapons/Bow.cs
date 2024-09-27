public class Bow : ThrownWeapon
{
    private bool isFalling;

    protected override void WeaponUpdate()
    {
        transform.up = rigidbody.velocity;

        // 떨어지는 상태일 때 무게 0.2배 
        if (rigidbody.velocity.y <= 0f && false == isFalling)
        {
            isFalling = true;
            rigidbody.mass *= 0.2f;
        }
    }
}
