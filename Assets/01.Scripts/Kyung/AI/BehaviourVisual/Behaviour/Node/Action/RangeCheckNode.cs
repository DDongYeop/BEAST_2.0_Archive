using UnityEngine;
using Random = UnityEngine.Random;

namespace Behaviour
{
    public class RangeCheckNode : ActionNode
    {
        [Header("Range Check")]
        [SerializeField] private float _range;

        private float _rangeCheck;
        [SerializeField] private LayerMask _findLayer;

        private void Awake()
        {
            _rangeCheck = Random.Range(_range - 0.5f, _range + 0.5f);
        }

        protected override State OnUpdate()
        {
            Collider2D isHere = Physics2D.OverlapCircle(Brain.transform.position, _rangeCheck, _findLayer);
            if (isHere)
                return State.RUNNING;
            
            return State.FAILURE; 
        }
    }
}
 