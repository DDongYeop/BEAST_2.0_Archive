using UnityEngine;

public class Stone : ThrownWeapon
{
    [SerializeField] private float rotateSpeed;

    protected override void WeaponUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
