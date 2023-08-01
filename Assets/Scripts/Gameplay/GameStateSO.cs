using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Gameplay
{
    public enum EGameState
    {
        Menu = 0,
        Field = 1,
        Battle = 2,
        Dialogue = 3,
        Cutscene = 4,
    }


    [CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/Game State")]
    public class GameStateSO : ScriptableObject
    {
        [field: SerializeField, ReadOnly]
        public EGameState CurrentGameState { get; private set; }

        [field: SerializeField, ReadOnly]
        public EGameState PreviousGameState { get; private set; }

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _enterBattleChannelEvent;
        [SerializeField] private VoidEventChannelSO _enterFieldChannelEvent;

        private void OnEnable()
        {
            CurrentGameState = EGameState.Menu;
            PreviousGameState = EGameState.Menu;
        }

        public void UpdateGameState(EGameState newGameState)
        {
            if (newGameState == CurrentGameState) return;

            if (newGameState == EGameState.Battle) _enterBattleChannelEvent.RaiseEvent();
            if (newGameState == EGameState.Field) _enterFieldChannelEvent.RaiseEvent();

            PreviousGameState = CurrentGameState;
            CurrentGameState = newGameState;
        }

        /// <summary>
        /// Revert the current game state to the previous one.
        /// this will support when enter dialogue state while in field or battle state.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void RevertGameState()
        {
            if (PreviousGameState == CurrentGameState) return;

            if (PreviousGameState == EGameState.Battle) _enterBattleChannelEvent.RaiseEvent();
            if (PreviousGameState == EGameState.Field) _enterFieldChannelEvent.RaiseEvent();

            (PreviousGameState, CurrentGameState) = (CurrentGameState, PreviousGameState);
        }
    }
}