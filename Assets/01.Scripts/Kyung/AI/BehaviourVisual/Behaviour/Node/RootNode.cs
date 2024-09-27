using UnityEngine;

namespace Behaviour
{
    public class RootNode : Node
    {
        public Node Child;

        public override void OnStart()
        {
            
        }

        public override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            var v = Child.Update();
            return v;
        }

        public override void Init(EnemyBrain brain, Blackboard blackboard)
        {
            base.Init(brain, blackboard);
            Child.Init(brain, blackboard);
        }

        public override Node Clone()
        {
            RootNode node = Instantiate(this);
            node.Child = Child.Clone();
            return node;
        }
    }
}
