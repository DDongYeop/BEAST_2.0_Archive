using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private readonly int moveInputHash = Animator.StringToHash("moveInput");

    public PlayerMoveState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    public override void Enter()
    {
        PlayAnimation(animBoolHash);
        playerController.ControllGroundEffect(true);
    }

    public override void UpdateState()
    {
        if (playerInput.IsMoveInputIn)
        {
            playerController.SetVelocity(playerInput.XInput);
            SetAnimationParameterValue(moveInputHash, playerInput.XInput);
        }
        else
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        playerController.ControllGroundEffect(false);
        StopAnimation(animBoolHash);
    }
}
