using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class RandomSelectorNode : CompositeNode
    {
        public override void OnStart()
        {
        }

        public override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Brain.CurrentNodeValue != -1)
                return Children[Brain.CurrentNodeValue].Update();

            List<int> _playIndexes = new List<int>();
            for (int i = 0; i < Children.Count; ++i)
            {
                ActionNode childAction = Children[i] as ActionNode;
                //if (childAction.IsCanPlay())
                //    _playIndexes.Add(i);
            }
            
            if (_playIndexes.Count >= 1)
            {
                int randomValue = Random.Range(0, _playIndexes.Count);
                Brain.CurrentNodeValue = _playIndexes[randomValue];
                //Brain.CurrentNode = Children[Brain.CurrentNodeValue] as ActionNode;
                Children[Brain.CurrentNodeValue].OnStart();
                return Children[Brain.CurrentNodeValue].Update();
            }

            return State.RUNNING;
        }
    }
}
