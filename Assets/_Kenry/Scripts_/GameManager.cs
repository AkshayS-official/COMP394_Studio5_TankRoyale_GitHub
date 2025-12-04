using System;
using UnityEngine;

namespace _Kenry.Scripts_
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action<GameState> OnBeforeStateChange;
        public static event Action<GameState> OnAfterStateChange;
    
        public GameState State { get; private set; }
        
        
        [SerializeField] private GameObject playerPrefab;

        void Start() => ChangeState(GameState.Playing);

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
            Time.timeScale = 1f;
            
            if (playerPrefab != null)
            {
                playerPrefab.SetActive(true);
            }
            
        }

        private void HandleTankSelection()
        {
            Time.timeScale = 0f;
        }

        private void UpgradeSelection()
        {
            Time.timeScale = 0f;
        }

        private void HandleEnd()
        {
            Time.timeScale = 0f;
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