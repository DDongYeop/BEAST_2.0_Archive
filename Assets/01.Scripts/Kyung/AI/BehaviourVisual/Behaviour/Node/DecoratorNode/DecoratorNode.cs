using UnityEngine;

namespace Behaviour
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector] public Node Child;

        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.Child = Child.Clone();
            return node;
        }

        public override void Init(EnemyBrain brain, Blackboard blackboard)
        {
            base.Init(brain, blackboard);
            Child.Init(brain, blackboard);
        }
    }
}
