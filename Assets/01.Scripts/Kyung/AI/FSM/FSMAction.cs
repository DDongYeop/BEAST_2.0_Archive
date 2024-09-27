namespace FSM
{
    public abstract class FSMAction
    {
        protected EnemyBrain _brain;
        protected FSMRunner _fsmRunner;
        protected AgentAnimator _animator;
        public bool IsAttackStop = false;
        public bool IsTrueEnd = false;
        
        public virtual void Init(FSMRunner fsmRunner)
        {
            _fsmRunner = fsmRunner;
            _brain = _fsmRunner.GetComponent<EnemyBrain>();
            _animator = _fsmRunner.GetComponent<AgentAnimator>();
        }

        public abstract void StartAction();
        public abstract void UpdateAction();
        public abstract void EndAction();

        public virtual void BossHit()
        {
            
        }
    }
}
