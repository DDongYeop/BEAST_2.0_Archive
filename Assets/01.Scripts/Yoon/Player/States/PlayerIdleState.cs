public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    public override void Enter()
    {
        // playerController.StopImmediately();
        PlayAnimation(animBoolHash);
    }

    public override void UpdateState()
    {
        if (playerInput.IsMoveInputIn)
        {
            stateMachine.ChangeState(PlayerStateEnum.Move);
        }
    }

    public override void Exit()
    {
        StopAnimation(animBoolHash);
    }
}
