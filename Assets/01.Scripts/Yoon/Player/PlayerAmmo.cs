using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    private readonly int fullAmmoCount = 3;

    [SerializeField] private Image fillImage;
    private float imageFillAmount = 1;

    [Header("º¸°£")]
    [SerializeField] private float ammoChargeSpeed = 1.0f;
    [SerializeField] private float ammoChargingTime = 1.0f;

    private void Start()
    {
        StartCoroutine(ChargeAmmoRoop());
    }

    public int GetCurrentAmmoCount()
    {
        return (int)(imageFillAmount / (1.0f / fullAmmoCount));
    }

    public bool IsAmmoEmpty()
    {
        return GetCurrentAmmoCount() <= 0;
    }

    public void UseAmmo()
    {
        MinusImageFillAmount();
    }

    private void MinusImageFillAmount()
    {
        imageFillAmount -= (1.0f / fullAmmoCount);
        fillImage.fillAmount = imageFillAmount;
    }

    private IEnumerator ChargeAmmoRoop()
    {
        while (true)
        {
            if (imageFillAmount < 1.0f)
            {
                imageFillAmount = Mathf.SmoothDamp(imageFillAmount, 1.0f, ref ammoChargeSpeed, 
                    ammoChargingTime * (fullAmmoCount - GetCurrentAmmoCount()));
                fillImage.fillAmount = imageFillAmount;
            }
            yield return null;
        }
    }

}
