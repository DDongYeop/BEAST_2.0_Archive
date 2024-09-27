using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action OnPlayerDieEvent;
    public event Action OnDamgeEvent;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth; 
    public int CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        IsDead = false;
        currentHealth = maxHealth;
    }

    public void OnDamage(int damage, Vector3 hitPos)
    {
        OnDamgeEvent?.Invoke();

        // currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        // 
        // // ui update
        // 
        // if ((currentHealth <= 0) && (false == IsDead))
        // {
        //     // Die
        //     Debug.Log("Player Die");
        // 
        //     IsDead = true;
        //     OnPlayerDieEvent.Invoke();
        // }
        // else
        // {
        //     // hurt sound
        // }
    }
}