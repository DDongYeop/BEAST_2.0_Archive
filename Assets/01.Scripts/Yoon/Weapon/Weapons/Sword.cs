using UnityEngine;

public class Sword : ThrownWeapon
{
    protected override void WeaponUpdate()
    {
        transform.up = rigidbody.velocity;
    }
}
