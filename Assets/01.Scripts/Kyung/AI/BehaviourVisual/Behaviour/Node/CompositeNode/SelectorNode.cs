namespace Behaviour
{
    public class SelectorNode : CompositeNode
    {
        public override void OnStart()
        {
        }

        public override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            foreach (var child in Children)
            {
                switch (child.Update())
                {
                    case State.RUNNING:
                        return State.RUNNING;
                    case State.SUCCESS:
                        return State.SUCCESS;
                }
            }

            return State.FAILURE;
        }
    }
}
