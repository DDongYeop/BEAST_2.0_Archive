public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    public override void Enter()
    {
        SetAnimationParameter(animBoolHash);
    }
}
