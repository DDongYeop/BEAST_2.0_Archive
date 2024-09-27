using UnityEngine;

public class 
    PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;
    protected PlayerInput playerInput;
    protected PlayerAttack playerAttack;

    // 재생할 애니메이션 트랜지션의 Bool Property Hash
    protected int animBoolHash;

    // 재생하고 있는지
    protected bool isAnimPlaying;

    public PlayerState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName)
    {
        this.playerController = playerController;
        this.stateMachine = stateMachine;
        animBoolHash = Animator.StringToHash(animationBoolName);
        playerInput = playerController.PlayerInput;
        playerAttack = playerController.PlayerAttack;
    }

    //상태에 들어왔을 때 해줄 일
    public virtual void Enter() { }

    //상태에 있는 동안 해줘야할 일
    public virtual void UpdateState() { }

    //상태를 나갈 때 해줄 일
    public virtual void Exit() { }

    #region Animation

    public void PlayAnimation(int hashValue)
    {
        playerController.Animator.SetBool(hashValue, true);
        isAnimPlaying = true;
    }

    public void StopAnimation(int hashValue)
    {
        playerController.Animator.SetBool(hashValue, false);
        isAnimPlaying = false;
    }

    public void SetAnimationParameterValue(int hashValue, float value)
    {
        playerController.Animator.SetFloat(hashValue, value);
    }

    public void SetAnimationParameter(int hashValue, int value)
    {
        playerController.Animator.SetInteger(hashValue, value);
    }

    public void SetAnimationParameter(int hashValue)
    {
        playerController.Animator.SetTrigger(hashValue);
    }

    public void AnimationEndTrigger()
    {
        isAnimPlaying = false;
    }

    #endregion
}