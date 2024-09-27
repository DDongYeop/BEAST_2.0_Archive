using UnityEngine;

namespace Behaviour
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private EnemyBrain _brain;
        public BehaviourTree Tree;

        private void Awake()
        {
            Tree = Tree.Clone();
            Tree.Bind(GetComponent<EnemyBrain>());

            _brain = GetComponent<EnemyBrain>();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameOver && !_brain.IsKnockback)
                Tree.Update();
        }
    }
}
