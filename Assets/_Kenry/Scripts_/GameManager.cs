using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnBeforeStateChange;
    public static event Action<GameState> OnAfterStateChange;
    
    public GameState State { get; private set; }

    void Start() => ChangeState(GameState.TankSelection);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChange?.Invoke(newState);
        
        State = newState;
        switch (newState)
        {
            case GameState.TankSelection:
                HandleTankSelection();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Paused:
                HandlePause();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        OnAfterStateChange?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }

    private void HandleTankSelection()
    {
        // Handles the tank selection screen
        
        
        // When the player selects a tank, change the state to Playing
        ChangeState(GameState.Playing);
    }

    private void HandlePlaying()
    {
        
        ChangeState(GameState.Paused);
    }
    
    private void HandlePause()
    {
        // Handles pausing the game
        
        // When the player presses pause again, change the state back to Playing
        ChangeState(GameState.Playing);
    }

    private void HandleWin()
    {
        // Handles winning the game
        
    }
    
    private void HandleLose()
    {
        // Handles losing the game
        
    }
}

[SerializeField]
public enum GameState
{
    TankSelection,
    Playing,
    Paused,
    Win,
    Lose
}
