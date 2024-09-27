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

        // 옳지 않은 조준 상태면 Idle로
        bool isAiming = playerAttack.Aiming(dragDirection);
        if (false == isAiming)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // 은 조준 상태이고, 발사 입력이 들어왔다면 BowThrow로
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
