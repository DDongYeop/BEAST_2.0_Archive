using System;
using FSM;
using UnityEngine;

public class TutoRunner : FSMRunner
{
    protected override void Init()
    {
        foreach (FSMState state in Enum.GetValues(typeof(FSMState)))
        {
            string typeName = state.ToString();
            Type t = Type.GetType($"FSM.FSM{typeName}State");

            FSMAction action = Activator.CreateInstance(t) as FSMAction;
            _actionDic.Add(state, action);
        }
    }
}
