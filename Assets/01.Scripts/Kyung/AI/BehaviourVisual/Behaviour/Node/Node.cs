using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviour
{
    public abstract class Node : ScriptableObject
    {
        [HideInInspector] public State State;
        [HideInInspector] public bool Started = false;
        [HideInInspector] public string Guid;
        [HideInInspector] public Vector2 Position;
        [HideInInspector] public Blackboard Blackboard;
        [HideInInspector] public EnemyBrain Brain;
        [TextArea] public string Description;

        public State Update()
        {
            if (!Started)
            {
                OnStart();
                Started = true;
            }

            State = OnUpdate();

            if (State == State.FAILURE || State == State.SUCCESS)
            {
                OnStop();
                Started = false;
            }

            return State;
        }

        public virtual void Init(EnemyBrain brain, Blackboard blackboard)
        {
            Brain = brain;
            Blackboard = blackboard;
        }
        
        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        public abstract void OnStart();
        public abstract void OnStop();
        protected abstract State OnUpdate();
    }
}
