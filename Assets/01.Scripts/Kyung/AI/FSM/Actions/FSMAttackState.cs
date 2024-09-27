namespace FSM
{
    public class FSMAttackState : FSMAction
    {
        public override void StartAction()
        {
            _animator.OnOtherBool("Attack01", true);
        }

        public override void UpdateAction()
        {
        }

        public override void EndAction()
        {
            _animator.OnOtherBool("Attack01", false);
        }
    }
}
