using UnityEngine;

namespace Behaviour
{
    public class MoveNode : ActionNode
    {
        [Header("Movement")] 
        [SerializeField] private float _speed;

        protected override State OnUpdate()
        {
            Brain.AgentAnimator.OnMove(true);
            Brain.transform.position += new Vector3(Brain.MoveDirection, 0) * (_speed * Time.deltaTime);
            return State.RUNNING;
        }
    }
}
