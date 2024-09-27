using UnityEngine;

public class DisplayThrownWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private string initSortingLayerName;

    [SerializeField] private bool isThrownWeapon = true;
    private bool isHeavyWeapon = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initSortingLayerName = spriteRenderer.sortingLayerName;
    }

    public void OnDisplay(ThrownWeaponStat stat)
    {
        isHeavyWeapon = stat.IsHeavy;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (isThrownWeapon)
        {
            spriteRenderer.sprite = stat.MainWeaponSprite;
        }
        else
        {
            spriteRenderer.sprite = stat.SubWeaponSprite;
        }
    }

    public void OffDisplay()
    {
        spriteRenderer.sprite = null;
    }

    public void SetForwardAngle(Vector3 direction)
    {
        if (false == isHeavyWeapon)
        {
            transform.up = direction;
        }
    }

    public void SetLayerToFront(string layerName)
    {
        if (spriteRenderer.sortingLayerName != layerName)
        {
            spriteRenderer.sortingLayerName = layerName;
            spriteRenderer.sortingOrder *= -1;
        }
    }

    public void SetLayerToBack()
    {
        spriteRenderer.sortingLayerName = initSortingLayerName;
        spriteRenderer.sortingOrder *= -1;
    }
}
