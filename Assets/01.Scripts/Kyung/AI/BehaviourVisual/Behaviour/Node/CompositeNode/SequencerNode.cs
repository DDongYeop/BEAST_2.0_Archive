using UnityEngine;

namespace Behaviour
{
    public class SequencerNode : CompositeNode
    {
        public override void OnStart()
        {
        }

        public override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            bool _isSuccess = false;
            
            foreach (var child in Children)
            {
                switch (child.Update())
                {
                    case State.FAILURE:
                        return State.FAILURE;
                    case State.RUNNING:
                        break;
                    case State.SUCCESS:
                        _isSuccess = true;
                        break;
                }
            }

            return _isSuccess ? State.SUCCESS : State.RUNNING;
        }
    }
}
