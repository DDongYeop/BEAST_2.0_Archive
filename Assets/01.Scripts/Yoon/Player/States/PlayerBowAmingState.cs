public class PlayerBowAmingState : PlayerAimingState
{
    public PlayerBowAmingState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    public override void Enter()
    {
        base.Enter();
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

        // �� ���� �����̰�, �߻� �Է��� ���Դٸ� BowThrow��
        if (false == playerInput.IsThrowReady)
        {
            stateMachine.ChangeState(PlayerStateEnum.BowThrow);
        }
    }

    public override void Exit() 
    {
        base.Exit();
    }
}
