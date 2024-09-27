using UnityEngine;

namespace Behaviour
{
    public class DebugNode : ActionNode
    {
        [SerializeField] private string _debug;

        public override void OnStart()
        {
            Debug.Log($"OnStart {_debug}");
        }

        public override void OnStop()
        {
            Debug.Log($"OnStop {_debug}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate {_debug}");
            return State.RUNNING;
        }
    }
}
