using UnityEngine;

public class PlayerAimingState : PlayerState
{
    protected Vector2 dragDirection;

    public PlayerAimingState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    public override void Enter()
    {
        playerController.StopImmediately();

        dragDirection = playerInput.GetDragDirection();
        PlayAnimation(animBoolHash);
    }

    public override void UpdateState()
    {
        dragDirection = playerInput.GetDragDirection();
        playerController.FlipPlayer(dragDirection.x);

        // 옳지 않은 조준 상태면 Idle로
        bool isAiming = playerAttack.Aiming(dragDirection);
        if (false == isAiming)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // 옳은 조준 상태이고, 발사 입력이 들어왔다면 Throw로
        if (false == playerInput.IsThrowReady)
        {
            stateMachine.ChangeState(PlayerStateEnum.Throw);
        }
    }

    public override void Exit()
    {
        playerAttack.EndAiming();
        StopAnimation(animBoolHash);
    }
    
}
