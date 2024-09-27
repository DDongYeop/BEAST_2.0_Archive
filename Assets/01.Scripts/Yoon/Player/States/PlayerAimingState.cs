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

        // ���� ���� ���� ���¸� Idle��
        bool isAiming = playerAttack.Aiming(dragDirection);
        if (false == isAiming)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // ���� ���� �����̰�, �߻� �Է��� ���Դٸ� Throw��
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
