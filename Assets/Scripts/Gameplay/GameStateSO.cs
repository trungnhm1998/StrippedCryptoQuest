using System;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public enum EGameState
    {
        Menu = 0,
        Field = 1,
        Battle = 2
    }
    

    [CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/Game State")]
    public class GameStateSO : ScriptableObject
    {
        [Header("Game states")]
        [SerializeField] private EGameState _previousGameState;

        [field: SerializeField] 
        public EGameState CurrentGameState { get; private set; }

        public void UpdateGameState(EGameState newGameState)
        {
            if (newGameState == CurrentGameState) return;

            _previousGameState = CurrentGameState;
            CurrentGameState = newGameState;
        }
    }
}