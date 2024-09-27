using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    [Header("Animator")] 
    [HideInInspector] public Animator Animator;
    
    [Header("Hash")]
    private readonly int _dieHash = Animator.StringToHash("Die");
    private readonly int _moveHash = Animator.StringToHash("Move");

    [Header("Other")]
    private WeaponStick _weaponStick;
    private EnemyFeedback _enemyFeedback;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        _weaponStick = GetComponent<WeaponStick>();
        _enemyFeedback = GetComponent<EnemyFeedback>();

        Init();
    }

    public void Init()
    {
        Animator.Rebind();
        Animator.Update(0f);
    }
        
    public void OnDie() => Animator.SetTrigger(_dieHash);
    public void OnMove(bool value) => Animator.SetBool(_moveHash, value);

    #region Ohter

    public void OnOtherTrigger(string hash) => Animator.SetTrigger(hash);
    public void OnOtherBool(string hash, bool value) => Animator.SetBool(hash, value);

    #endregion

    #region Event

    public virtual void SetAnimEnd()
    {
        _enemyFeedback.ShowAttackTrailFalse();
    }

    #endregion
}
