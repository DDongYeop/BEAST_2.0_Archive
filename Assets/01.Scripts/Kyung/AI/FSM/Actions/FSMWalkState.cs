namespace FSM
{
    public class FSMWalkState : FSMAction
    {
        public override void StartAction()
        {
            _animator.OnOtherBool("IsRun", true);
        }

        public override void UpdateAction()
        {
            //_brain.SetVelocityX(3);
        }

        public override void EndAction()
        {
            _animator.OnOtherBool("IsRun", false);
        }
    }
}
