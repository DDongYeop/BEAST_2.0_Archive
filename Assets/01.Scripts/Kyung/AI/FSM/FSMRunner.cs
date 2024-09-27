using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class FSMRunner : MonoBehaviour
    {
        private FSMState _currentState = FSMState.Idle;
        protected Dictionary<FSMState, FSMAction> _actionDic = new Dictionary<FSMState, FSMAction>();

        private void Awake()
        {
            Init();
            ChangeState(FSMState.Idle);
        }

        public void ChangeState(FSMState nextState)
        {
            _actionDic[_currentState].EndAction();
            _currentState = nextState;
            _actionDic[_currentState].StartAction();
        }

        public FSMAction GetCurrentAction()
        {
            return _actionDic[_currentState];
        }
        
        protected abstract void Init();
    }
}
