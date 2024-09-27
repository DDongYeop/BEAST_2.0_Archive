using UnityEngine;

namespace Behaviour
{
    public abstract class ActionNode : Node
    {
        [Header("Animation")] 
        [SerializeField] protected bool _isAnimation;
        [SerializeField] protected string _animationKey;

        public override void OnStart()
        {
            if (_isAnimation)
                Brain.AgentAnimator.OnOtherTrigger(_animationKey);
        }

        public override void OnStop()
        {
            if (_isAnimation)
                Brain.AgentAnimator.OnOtherTrigger(_animationKey); 
        }
    }
}
