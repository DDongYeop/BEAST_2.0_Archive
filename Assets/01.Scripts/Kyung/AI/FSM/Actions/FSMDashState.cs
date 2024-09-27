namespace FSM
{
    public class FSMDashState : FSMAction
    {
        public override void StartAction()
        {
            _animator.OnOtherBool("IsDash", true);
        }

        public override void UpdateAction()
        {
            //_brain.SetVelocityX(4.5f);
        }

        public override void EndAction()
        {
            _animator.OnOtherBool("IsDash", false);
        }
    }
}
