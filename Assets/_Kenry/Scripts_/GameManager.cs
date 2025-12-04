using System;
using UnityEngine;

namespace _Kenry.Scripts_
{
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
                case GameState.Playing:
                    HandlePlaying();
                    break;
                case GameState.TankSelection:
                    HandleTankSelection();
                    break;
                case GameState.UpgradeSelection:
                    HandleTankSelection();
                    break;
                case GameState.End:
                    HandleEnd();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        
            OnAfterStateChange?.Invoke(newState);
        
            Debug.Log($"New state: {newState}");
        }
        
        private void HandlePlaying()
        {
            
        }

        private void HandleTankSelection()
        {
            // Handles the tank selection screen
        
        
            // When the player selects a tank, change the state to Playing
            ChangeState(GameState.Playing);
        }

        private void UpgradeSelection()
        {
            ChangeState(GameState.UpgradeSelection);
        }

        private void HandleEnd()
        {
            ChangeState(GameState.End);
        }
    
        

        
    }

    [SerializeField]
    public enum GameState
    {
        Playing,
        TankSelection,
        UpgradeSelection,
        End
    }
}