using FSM;

public class TutoAnimator : AgentAnimator
{
    private FSMRunner _fsmRunner;

    protected override void Awake()
    {
        base.Awake();

        _fsmRunner = GetComponent<FSMRunner>();
    }

    public override void SetAnimEnd()
    {
        base.SetAnimEnd();
        
        if (_fsmRunner.GetCurrentAction().IsTrueEnd)
            _fsmRunner.ChangeState(FSMState.Idle);
    }
}
