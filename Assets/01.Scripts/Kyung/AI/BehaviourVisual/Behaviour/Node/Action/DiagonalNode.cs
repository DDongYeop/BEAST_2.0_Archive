using UnityEngine;

namespace Behaviour
{
    public class DiagonalNode : ActionNode
    {
        [Header("Movement")] 
        [SerializeField] private float _speed;
        
        protected override State OnUpdate()
        {
            Vector3 totemPos = GameManager.Instance.TotemTrm.position;
            totemPos += new Vector3(0, 3);
            Vector3 dir = totemPos - Brain.transform.position;
            dir.Normalize();

            Brain.transform.position += dir * (_speed * Time.deltaTime);
            
            return State.RUNNING;
        }
    }
}
