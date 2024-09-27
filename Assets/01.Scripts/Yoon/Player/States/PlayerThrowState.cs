public class PlayerThrowState : PlayerState
{
    public PlayerThrowState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    public override void Enter()
    {
        PlayAnimation(animBoolHash);
        playerAttack.Throw();
        playerController.UseAmmo();
    }

    public override void UpdateState()
    {
        // 애니메이션 끝나면 Idle로
        if (false == isAnimPlaying)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
        playerInput.IsThrowReady = true;
    }

    public override void Exit()
    {
        StopAnimation(animBoolHash);
    }
}
