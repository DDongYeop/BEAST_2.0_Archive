using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthSO _healthSO;

    private Totem _totem;
    
    private int _currentHp;
    public int CurrentHp => _currentHp;

    private void Awake()
    {
        _totem = GetComponent<Totem>();
    }

    private void Start()
    {
        _currentHp = _healthSO.MaxHP;
    }

    public void OnDamage(int damage, Vector3 hitPos)
    {
        _currentHp -= damage;

        // 아래 코드 작동 안 함 이슈로 인하여, 잠시 주석. 
        Slider slider = GetComponentInChildren<Slider>();
        DOTween.To(() => slider.value, NewValue => slider.value = NewValue, HpPercent(), 0.2f);


        if (_currentHp <= 0)
            _totem.GameOver();
    }

    /// <summary>
    /// HP 비율을 표시합니다.
    /// </summary>
    /// <returns></returns>
    public float HpPercent() => (float)_currentHp / _healthSO.MaxHP;
}
