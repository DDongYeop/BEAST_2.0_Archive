using UnityEngine;

public class PlayerSturnState : PlayerState
{
    public PlayerSturnState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName) { }

    private readonly float fullSturnTime = 3.0f;
    private float startSturnTime;

    private ParticleLoop particleLoop;

    private Vector3 positionOffsetY = new Vector3(0, 3, 0);

    public override void Enter()
    {
        SetAnimationParameter(animBoolHash);

        startSturnTime = Time.time;

        particleLoop = PoolManager.Instance.Pop("StunEffect") as ParticleLoop;
        particleLoop.SetParentTrm(playerController.transform, positionOffsetY);
        particleLoop.transform.localPosition = new Vector3(0, 3, 0);
    }

    public override void UpdateState()
    {
        float passedTime = Time.time - startSturnTime;
        if (passedTime > fullSturnTime)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        PoolManager.Instance.Push(particleLoop);
        playerController.ToInvincibleMode();
    }

}
