using System;
using UnityEngine;

namespace Behaviour
{
    public class CoolTimeNode : ActionNode
    {
        [Header("CoolTime")] 
        [SerializeField] private float _coolTime;
        private double _lastTime = -1000;

        protected override State OnUpdate()
        {
            Brain.AgentAnimator.OnMove(false);
            
            if (_lastTime + _coolTime <= Time.time)
            {
                _lastTime = Time.time;
                return State.RUNNING;
            }

            return State.SUCCESS;
        }
    }
}
