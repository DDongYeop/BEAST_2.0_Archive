using UnityEngine;

namespace Behaviour
{
    public class RunningSequencerNode : CompositeNode
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
                    case State.FAILURE:
                        return State.FAILURE;
                    case State.RUNNING:
                        break;
                    case State.SUCCESS:
                        return State.SUCCESS;
                }
            }

            return State.RUNNING;
        }
    }
}
