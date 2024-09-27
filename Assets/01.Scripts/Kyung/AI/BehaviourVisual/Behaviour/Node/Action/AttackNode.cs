using UnityEngine;

namespace Behaviour
{
    public class AttackNode : ActionNode
    {
        [Header("Attack")] 
        [SerializeField] private LayerMask _attackObj;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _attackRange;
        [SerializeField] private int _damage;

        public override void Init(EnemyBrain brain, Blackboard blackboard)
        {
            base.Init(brain, blackboard);
            
            brain.GizmosSetup(_attackRange, _attackRadius);
        }

        protected override State OnUpdate()
        {
            if (_isAnimation)
                Brain.AgentAnimator.OnOtherTrigger(_animationKey);
            
            Collider2D obj = Physics2D.OverlapCircle(Brain.transform.position + new Vector3(Brain.MoveDirection * _attackRange, 0, 0), _attackRadius,_attackObj);
            if (obj && obj.TryGetComponent(out IDamageable attackObj))
                attackObj.OnDamage(_damage, Vector3.zero);
            
            return State.RUNNING; 
        }
    }
}
