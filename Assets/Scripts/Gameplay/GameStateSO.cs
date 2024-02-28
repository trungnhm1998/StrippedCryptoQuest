using System;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay
{
    public enum EGameState
    {
        Menu = 0,
        Field = 1,
        Battle = 2,
        Dialogue = 3,
        Cutscene = 4,
        Merchant = 5,
        Loading = 6,
    }

    public class GameStateSO : ScriptableObject
    {
        public event UnityAction<EGameState> Changed;
        [field: SerializeField, ReadOnly] public EGameState CurrentGameState { get; private set; }

        [field: SerializeField, ReadOnly] public EGameState PreviousGameState { get; private set; }

        [Header("Events")] [SerializeField] private VoidEventChannelSO _enterBattleEventChannel;
        [SerializeField] private VoidEventChannelSO _enterFieldEventChannel;

        private void OnEnable()
        {
            CurrentGameState = EGameState.Menu;
            PreviousGameState = EGameState.Menu;
        }

        public void UpdateGameState(EGameState newGameState)
        {
            if (newGameState == CurrentGameState) return;

            if (newGameState == EGameState.Battle) _enterBattleEventChannel.RaiseEvent();
            if (newGameState == EGameState.Field) _enterFieldEventChannel.RaiseEvent();

            PreviousGameState = CurrentGameState;
            CurrentGameState = newGameState;
            Changed.SafeInvoke(CurrentGameState);
        }

        /// <summary>
        /// Revert the current game state to the previous one.
        /// this will support when enter dialogue state while in field or battle state.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void RevertGameState()
        {
            if (PreviousGameState == CurrentGameState) return;

            if (PreviousGameState == EGameState.Battle) _enterBattleEventChannel.RaiseEvent();
            if (PreviousGameState == EGameState.Field) _enterFieldEventChannel.RaiseEvent();

            (PreviousGameState, CurrentGameState) = (CurrentGameState, PreviousGameState);
        }
    }
}