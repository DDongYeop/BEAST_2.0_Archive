namespace Behaviour
{
    public class DieCheckNode : ActionNode
    {
        public override void OnStart()
        {
        }

        public override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Brain.IsDie)
            {
                Brain.AgentAnimator.OnDie();
                return State.SUCCESS;
            }
            
            return State.FAILURE;
        }
    }
}
