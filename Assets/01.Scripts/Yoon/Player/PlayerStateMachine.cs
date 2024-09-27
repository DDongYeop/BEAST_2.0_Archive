using System.Collections.Generic;
using UnityEngine;  

public enum PlayerStateEnum
{
    Idle,
    Move,
    Aiming,
    BowAming,
    Throw,
    BowThrow,
    Sturn,
    Die
}

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> StateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();

    private PlayerController playerController;

    public void Initialize(PlayerStateEnum startState, PlayerController playerController)
    {
        this.playerController = playerController;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        PlayerState newPlayerState = StateDictionary[newState];

        if (CurrentState.ToString() == newPlayerState.ToString())
        {
            return;
        }

        CurrentState.Exit();
        CurrentState = newPlayerState;
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum state, PlayerState playerState)
    {
        StateDictionary.Add(state, playerState);
    }
}