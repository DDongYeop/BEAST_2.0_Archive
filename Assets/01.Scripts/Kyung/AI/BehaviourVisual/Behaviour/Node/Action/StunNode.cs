using UnityEngine;

namespace Behaviour
{
    public class StunNode : ActionNode
    {
        [SerializeField] private Vector3 _addPos;
        private ParticleLoop _particle;
        
        public override void OnStart()
        {
            base.OnStart();
            if (PoolManager.Instance != null)
            {
                _particle = PoolManager.Instance.Pop("StunEffect") as ParticleLoop;
                _particle.SetParentTrm(Brain.transform, _addPos);
            }
        }

        public override void OnStop()
        {
            base.OnStop();
            if (_particle)
                PoolManager.Instance.Push(_particle);
        }

        protected override State OnUpdate()
        {
            if (Brain.StunTime > 0)
            {
                Brain.StunTime -= Time.deltaTime;
                return State.RUNNING;
            }
            return State.FAILURE;
        }
    }
}
