using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> Children = new List<Node>();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.Children = Children.ConvertAll(c => c.Clone());
            return node;
        }

        public override void Init(EnemyBrain brain, Blackboard blackboard)
        {
            base.Init(brain, blackboard);
            foreach (var child in Children)
                child.Init(brain, blackboard);
        }
    }
}
